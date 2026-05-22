using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection
{
    public enum PathMappingErrorHandling
    {
        Null,
        DefaultValue
    }

    public static class PathMappingEngine
    {
        public static ParameterCollection Map(ParameterCollection source, ParameterCollection mappingDefinition, PathMappingErrorHandling errorHandling = PathMappingErrorHandling.Null, IEnumerable<IParameterValueConverter> converters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (mappingDefinition == null) throw new ArgumentNullException(nameof(mappingDefinition));

            var result = new ParameterCollection();

            // Traverse mapping definition and populate result
            foreach (var mapParam in mappingDefinition)
            {
                var key = mapParam.Key;
                switch (mapParam.Type)
                {
                    case ParameterType.String:
                    case ParameterType.String_Multiline:
                        {
                            var path = mapParam.GetValue<string>(converters);
                            var (val, resolvedParameterType) = ResolvePathAuto(source, path, converters, pathDivider, listMarker, additionalInfoMarker);
                            if (val == null)
                            {
                                if (errorHandling == PathMappingErrorHandling.DefaultValue && resolvedParameterType.HasValue)
                                {
                                    result.Add(key, resolvedParameterType.Value.GetDefaultValue(), resolvedParameterType.Value);
                                }
                                else
                                {
                                    result.Add(key, null, ParameterType.String);
                                }
                            }
                            else
                            {
                                result.Add(key, val);
                            }
                        }
                        break;
                    case ParameterType.ParameterCollection:
                        {
                            var nestedMapping = mapParam.GetValue<ParameterCollection>(converters);
                            var nested = Map(source, nestedMapping, errorHandling, converters, pathDivider, listMarker, additionalInfoMarker);
                            result.Add(key, nested);
                        }
                        break;
                    case ParameterType.ParameterCollection_IEnumerable:
                        {
                            var templates = mapParam.GetValue<IEnumerable<ParameterCollection>>(converters)?.ToArray();
                            if (templates == null || templates.Length == 0)
                            {
                                result.Add(key, Array.Empty<ParameterCollection>());
                                break;
                            }

                            var template = templates[0];

                            // collect candidate paths from template
                            var candidatePaths = CollectStringPaths(template);

                            // find primary list root and count
                            string listRoot = null;
                            int count = 0;
                            foreach (var candidate in candidatePaths)
                            {
                                var found = FindListRootAndCount(source, candidate, pathDivider, converters, listMarker, additionalInfoMarker);
                                if (found.count > 0)
                                {
                                    listRoot = found.listRoot;
                                    count = found.count;
                                    break;
                                }
                            }

                            // fallback: try first candidate as flattened array
                            if (count == 0 && candidatePaths.Length > 0)
                            {
                                var tryArr = TryResolveAsAnyArray(source, candidatePaths[0], converters, pathDivider, listMarker, additionalInfoMarker);
                                if (tryArr != null)
                                {
                                    count = tryArr.Count();
                                    // assume listRoot is first segment
                                    listRoot = candidatePaths[0].Split(new string[] { pathDivider }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                                }
                            }

                            var items = new List<ParameterCollection>();
                            for (int i = 0; i < count; i++)
                            {
                                var item = new ParameterCollection();
                                ProcessTemplateNode(template, item, source, errorHandling, converters, pathDivider, listMarker, additionalInfoMarker, listRoot, i);
                                items.Add(item);
                            }

                            result.Add(key, items, ParameterType.ParameterCollection_IEnumerable);
                        }
                        break;
                    default:
                        // For other types, if mappingDefinition contains non-string literal (numbers etc.), copy as-is
                        try
                        {
                            var literal = mapParam.GetValue<object>(converters);
                            result.Add(key, literal);
                        }
                        catch
                        {
                            result.Add(key, null, ParameterType.String);
                        }
                        break;
                }
            }

            return result;
        }

        private static (object value, ParameterType? parameterType) ResolvePathAuto(ParameterCollection source, string path, IEnumerable<IParameterValueConverter> converters, string pathDivider, string listMarker, string additionalInfoMarker)
        {
            if (string.IsNullOrEmpty(path)) return (null, null);

            // Try ParameterCollection
            try
            {
                var pc = source.GetByPath(path, typeof(ParameterCollection), converters, pathDivider, listMarker, additionalInfoMarker) as ParameterCollection;
                if (pc != null)
                {
                    return (pc, ParameterType.ParameterCollection);
                }
            }
            catch {}

            // Try arrays / lists of common types
            var pathSegments = path.Split(new[] { pathDivider }, StringSplitOptions.RemoveEmptyEntries);
            var listMarkerPattern = "^" + Regex.Escape(listMarker).Replace("\\{0\\}", "(\\d+)") + "$";
            var containsListMarker = pathSegments.Any(p => Regex.IsMatch(p, listMarkerPattern));

            var arrayTypes = new (Type clrType, ParameterType paramType)[]
            {
                (typeof(ParameterCollection[]), ParameterType.ParameterCollection_IEnumerable),
                (typeof(string[]), ParameterType.String_IEnumerable),
                (typeof(int[]), ParameterType.Int_IEnumerable),
                (typeof(decimal[]), ParameterType.Decimal_IEnumerable),
                (typeof(bool[]), ParameterType.Bool_IEnumerable),
                (typeof(DateTime[]), ParameterType.DateTime_IEnumerable)
            };

            if (!containsListMarker)
            {
                foreach (var (clrType, paramType) in arrayTypes)
                {
                    try
                    {
                        var arr = source.GetByPath(path, clrType, converters, pathDivider, listMarker, additionalInfoMarker);
                        if (arr != null)
                        {
                            return (arr, paramType);
                        }
                    }
                    catch {}
                }
            }

            // Try scalar types
            var scalarTypes = new (Type clrType, ParameterType paramType)[]
            {
                (typeof(string), ParameterType.String),
                (typeof(int), ParameterType.Int),
                (typeof(decimal), ParameterType.Decimal),
                (typeof(bool), ParameterType.Bool),
                (typeof(DateTime), ParameterType.DateTime),
            };

            foreach (var (clrType, paramType) in scalarTypes)
            {
                try
                {
                    var v = source.GetByPath(path, clrType, converters, pathDivider, listMarker, additionalInfoMarker);
                    if (v != null)
                    {
                        return (v, paramType);
                    }
                }
                catch {}
            }

            return (null, null);
        }

        private static string[] CollectStringPaths(ParameterCollection template)
        {
            var list = new List<string>();
            foreach (var p in template)
            {
                if (p.Type == ParameterType.String || p.Type == ParameterType.String_Multiline)
                {
                    try { list.Add(p.GetValue<string>()); } catch {} 
                }
                else if (p.Type == ParameterType.ParameterCollection)
                {
                    try {
                        var nested = p.GetValue<ParameterCollection>();
                        list.AddRange(CollectStringPaths(nested));
                    } catch {}
                }
                else if (p.Type == ParameterType.ParameterCollection_IEnumerable)
                {
                    try {
                        var templates = p.GetValue<IEnumerable<ParameterCollection>>();
                        if (templates != null)
                        {
                            foreach (var t in templates)
                            {
                                list.AddRange(CollectStringPaths(t));
                            }
                        }
                    } catch {}
                }
            }
            return list.ToArray();
        }

        private static (string listRoot, int count) FindListRootAndCount(ParameterCollection source, string candidatePath, string pathDivider, IEnumerable<IParameterValueConverter> converters, string listMarker, string additionalInfoMarker)
        {
            var parts = candidatePath.Split(new string[] { pathDivider }, StringSplitOptions.RemoveEmptyEntries);
            var fallback = (listRoot: (string)null, count: 0);
            for (int i = parts.Length; i >= 1; i--)
            {
                var prefix = string.Join(pathDivider, parts.Take(i));
                // try ParameterCollection enumerable first
                try
                {
                    var arr = source.GetByPath(prefix, typeof(ParameterCollection[]), converters, pathDivider, listMarker, additionalInfoMarker) as ParameterCollection[];
                    if (arr != null && arr.Length > 0)
                    {
                        return (prefix, arr.Length);
                    }
                }
                catch {}

                // try any primitive array as a fallback if no list root was discovered yet
                try
                {
                    var arr = TryResolveAsAnyArray(source, prefix, converters, pathDivider, listMarker, additionalInfoMarker);
                    if (arr != null)
                    {
                        var count = arr.Count();
                        if (count > 0 && fallback.count == 0)
                        {
                            fallback = (prefix, count);
                        }
                    }
                }
                catch {}
            }

            return fallback;
        }

        private static IEnumerable<object> TryResolveAsAnyArray(ParameterCollection source, string path, IEnumerable<IParameterValueConverter> converters, string pathDivider, string listMarker, string additionalInfoMarker)
        {
            var tryTypes = new Type[] { typeof(string[]), typeof(int[]), typeof(decimal[]), typeof(bool[]), typeof(DateTime[]), typeof(ParameterCollection[]) };
            foreach (var t in tryTypes)
            {
                try
                {
                    var arr = source.GetByPath(path, t, converters, pathDivider, listMarker, additionalInfoMarker) as IEnumerable<object>;
                    if (arr != null) return arr;
                }
                catch {}
            }
            return null;
        }

        private static void ProcessTemplateNode(ParameterCollection template, ParameterCollection resultItem, ParameterCollection source, PathMappingErrorHandling errorHandling, IEnumerable<IParameterValueConverter> converters, string pathDivider, string listMarker, string additionalInfoMarker, string listRoot, int index)
        {
            foreach (var p in template)
            {
                var key = p.Key;
                if (p.Type == ParameterType.String || p.Type == ParameterType.String_Multiline)
                {
                    string path = null;
                    try { path = p.GetValue<string>(); } catch { path = null; }
                    if (path == null)
                    {
                        if (errorHandling == PathMappingErrorHandling.DefaultValue) resultItem.Add(key, string.Empty);
                        else resultItem.Add(key, null, ParameterType.String);
                        continue;
                    }

                    // If this path references the list root, insert index marker
                    string effectivePath = path;
                    if (!string.IsNullOrEmpty(listRoot))
                    {
                        var prefix = listRoot;
                        if (path == prefix)
                        {
                            effectivePath = prefix + "." + string.Format(listMarker, index);
                        }
                        else if (path.StartsWith(prefix + pathDivider))
                        {
                            var suffix = path.Substring(prefix.Length + pathDivider.Length);
                            effectivePath = prefix + "." + string.Format(listMarker, index) + "." + suffix;
                        }
                    }

                    var (val, resolvedParamType) = ResolvePathAuto(source, effectivePath, converters, pathDivider, listMarker, additionalInfoMarker);
                    if (val == null)
                    {
                        if (errorHandling == PathMappingErrorHandling.DefaultValue && resolvedParamType.HasValue)
                        {
                            resultItem.Add(key, resolvedParamType.Value.GetDefaultValue(), resolvedParamType.Value);
                        }
                        else
                        {
                            resultItem.Add(key, null, ParameterType.String);
                        }
                    }
                    else
                    {
                        resultItem.Add(key, val);
                    }
                }
                else if (p.Type == ParameterType.ParameterCollection)
                {
                    var nestedTemplate = p.GetValue<ParameterCollection>(converters);
                    var nestedResult = new ParameterCollection();
                    ProcessTemplateNode(nestedTemplate, nestedResult, source, errorHandling, converters, pathDivider, listMarker, additionalInfoMarker, listRoot, index);
                    resultItem.Add(key, nestedResult);
                }
                else if (p.Type == ParameterType.ParameterCollection_IEnumerable)
                {
                    // Nested list inside item: process similarly, determine primary list rooted relative to overall source
                    var nestedTemplates = p.GetValue<IEnumerable<ParameterCollection>>(converters)?.ToArray();
                    if (nestedTemplates == null || nestedTemplates.Length == 0)
                    {
                        resultItem.Add(key, Array.Empty<ParameterCollection>());
                        continue;
                    }

                    var nestedTemplate = nestedTemplates[0];
                    var candidatePaths = CollectStringPaths(nestedTemplate);
                    string nestedListRoot = null;
                    int nestedCount = 0;
                    foreach (var candidate in candidatePaths)
                    {
                        var found = FindListRootAndCount(source, candidate, pathDivider, converters, listMarker, additionalInfoMarker);
                        if (found.count > 0)
                        {
                            nestedListRoot = found.listRoot;
                            nestedCount = found.count;
                            break;
                        }
                    }

                    var nestedItems = new List<ParameterCollection>();
                    for (int j = 0; j < nestedCount; j++)
                    {
                        var nestedItem = new ParameterCollection();
                        ProcessTemplateNode(nestedTemplate, nestedItem, source, errorHandling, converters, pathDivider, listMarker, additionalInfoMarker, nestedListRoot, j);
                        nestedItems.Add(nestedItem);
                    }
                    resultItem.Add(key, nestedItems, ParameterType.ParameterCollection_IEnumerable);
                }
                else
                {
                    // literal or other: copy value
                    try
                    {
                        var val = p.GetValue<object>(converters);
                        resultItem.Add(key, val);
                    }
                    catch
                    {
                        resultItem.Add(key, null, ParameterType.String);
                    }
                }
            }
        }
    }
}
