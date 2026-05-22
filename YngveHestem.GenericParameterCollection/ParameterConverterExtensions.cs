using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection
{
    public static class ParameterConverterExtensions
    {
        public static Type GetTypeByName(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }

        public static IEnumerable<T> ToCorrectIEnumerable<T>(this IEnumerable<T> value, Type returnType)
        {
            if (!typeof(IEnumerable<T>).IsAssignableFrom(returnType))
            {
                throw new ArgumentException("Method " + nameof(ToCorrectIEnumerable) + " can only handle values where " + nameof(returnType) + " inherits " + nameof(IEnumerable<T>) + ". Here " + nameof(T) + " is " + typeof(T) + "and " + nameof(returnType) + " is " + returnType.FullName);
            }

            if (returnType.IsArray)
            {
                return value.ToArray();
            }
            else if (typeof(IList<T>).IsAssignableFrom(returnType))
            {
                return value.ToList();
            }

            return value;
        }

        public static IEnumerable<T> ToIEnumerable<T>(this T value)
        {
            return JToken.FromObject(new T[] { value }, JsonSerializer).ToObject<IEnumerable<T>>(JsonSerializer);
        }

        public static Type GetDefaultValueType(this ParameterType type)
        {
            if (type == ParameterType.Int)
            {
                return typeof(int);
            }
            else if (type == ParameterType.String || type == ParameterType.String_Multiline)
            {
                return typeof(string);
            }
            else if (type == ParameterType.Decimal)
            {
                return typeof(decimal);
            }
            else if (type == ParameterType.Bool)
            {
                return typeof(bool);
            }
            else if (type == ParameterType.Bytes)
            {
                return typeof(byte[]);
            }
            else if (type == ParameterType.Date || type == ParameterType.DateTime)
            {
                return typeof(DateTime);
            }
            else if (type == ParameterType.ParameterCollection)
            {
                return typeof(ParameterCollection);
            }
            else if (type == ParameterType.String_IEnumerable || type == ParameterType.String_Multiline_IEnumerable || type == ParameterType.SelectMany)
            {
                return typeof(IEnumerable<string>);
            }
            else if (type == ParameterType.Int_IEnumerable)
            {
                return typeof(IEnumerable<int>);
            }
            else if (type == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<decimal>);
            }
            else if (type == ParameterType.Bool_IEnumerable)
            {
                return typeof(IEnumerable<bool>);
            }
            else if (type == ParameterType.Date_IEnumerable || type == ParameterType.DateTime_IEnumerable)
            {
                return typeof(IEnumerable<DateTime>);
            }
            else if (type == ParameterType.ParameterCollection_IEnumerable)
            {
                return typeof(IEnumerable<ParameterCollection>);
            }
            else
            {
                return typeof(string);
            }
        }

        public static Type GetDefaultValueTypeWithNullableTypes(this ParameterType type)
        {
            if (type == ParameterType.Int)
            {
                return typeof(long?);
            }
            else if (type == ParameterType.String || type == ParameterType.String_Multiline)
            {
                return typeof(string);
            }
            else if (type == ParameterType.Decimal)
            {
                return typeof(decimal?);
            }
            else if (type == ParameterType.Bool)
            {
                return typeof(bool?);
            }
            else if (type == ParameterType.Bytes)
            {
                return typeof(byte[]);
            }
            else if (type == ParameterType.Date || type == ParameterType.DateTime)
            {
                return typeof(DateTime?);
            }
            else if (type == ParameterType.ParameterCollection)
            {
                return typeof(ParameterCollection);
            }
            else if (type == ParameterType.String_IEnumerable || type == ParameterType.String_Multiline_IEnumerable || type == ParameterType.SelectMany)
            {
                return typeof(IEnumerable<string>);
            }
            else if (type == ParameterType.Int_IEnumerable)
            {
                return typeof(IEnumerable<long?>);
            }
            else if (type == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<decimal?>);
            }
            else if (type == ParameterType.Bool_IEnumerable)
            {
                return typeof(IEnumerable<bool?>);
            }
            else if (type == ParameterType.Date_IEnumerable || type == ParameterType.DateTime_IEnumerable)
            {
                return typeof(IEnumerable<DateTime?>);
            }
            else if (type == ParameterType.ParameterCollection_IEnumerable)
            {
                return typeof(IEnumerable<ParameterCollection>);
            }
            else
            {
                return typeof(string);
            }
        }

        public static object GetDefaultValue(this ParameterType type)
        {
            if (type == ParameterType.Int)
            {
                return 0;
            }
            else if (type == ParameterType.String || type == ParameterType.String_Multiline)
            {
                return string.Empty;
            }
            else if (type == ParameterType.Decimal)
            {
                return 0.0m;
            }
            else if (type == ParameterType.Bool)
            {
                return false;
            }
            else if (type == ParameterType.Bytes)
            {
                return Array.Empty<byte>();
            }
            else if (type == ParameterType.Date || type == ParameterType.DateTime)
            {
                return DateTime.Now;
            }
            else if (type == ParameterType.ParameterCollection)
            {
                return new ParameterCollection();
            }
            else if (type == ParameterType.String_IEnumerable || type == ParameterType.String_Multiline_IEnumerable || type == ParameterType.SelectMany)
            {
                return Array.Empty<string>();
            }
            else if (type == ParameterType.Int_IEnumerable)
            {
                return Array.Empty<int>();
            }
            else if (type == ParameterType.Decimal_IEnumerable)
            {
                return Array.Empty<decimal>();
            }
            else if (type == ParameterType.Bool_IEnumerable)
            {
                return Array.Empty<bool>();
            }
            else if (type == ParameterType.Date_IEnumerable || type == ParameterType.DateTime_IEnumerable)
            {
                return Array.Empty<DateTime>();
            }
            else if (type == ParameterType.ParameterCollection_IEnumerable)
            {
                return Array.Empty<ParameterCollection>();
            }
            else
            {
                return string.Empty;
            }
        }

        public static ParameterCollection ToParameterCollection(this Enum enumValue)
        {
            var type = enumValue.GetType();
            return new ParameterCollection
            {
                { "type", type.FullName },
                { "value", Enum.GetName(type, enumValue) },
                { "choices", Enum.GetNames(type) }
            };
        }

        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = GetJsonConverters(),
                FloatParseHandling = FloatParseHandling.Decimal,
                MaxDepth = 256
            };
        }

        public static JsonConverter[] GetJsonConverters()
        {
            return new JsonConverter[] {
                    new StringEnumConverter()
                };
        }

        public static bool CanConvertFromValue(object value, Type type, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            foreach (var parameterType in (ParameterType[])Enum.GetValues(typeof(ParameterType)))
            {
                if (CanConvertFromValue(value, type, parameterType, additionalInfo, customConverters))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanConvertFromValue(object value, Type type, ParameterType targetType, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            if (additionalInfo == null)
            {
                additionalInfo = new ParameterCollection();
            }

            var acAttribute = type.GetCustomAttribute<AttributeConvertibleAttribute>();
            if (acAttribute != null && acAttribute.ParameterType == targetType)
            {
                return true;
            }

            if (customConverters != null)
            {
                if (customConverters.Any(converter => converter.CanConvertFromValue(targetType, type, value, additionalInfo, customConverters)))
                {
                    return true;
                }
            }
            return Parameter.DefaultParameterValueConverters.Any(converter => converter.CanConvertFromValue(targetType, type, value, additionalInfo, customConverters));
        }

        internal static ParameterCollection SelectOneToParameterCollection(string value, IEnumerable<string> choices)
        {
            return new ParameterCollection
            {
                { "value", value },
                { "choices", choices }
            };
        }

        internal static ParameterCollection SelectManyToParameterCollection(IEnumerable<string> value, IEnumerable<string> choices)
        {
            return new ParameterCollection
            {
                { "value", value },
                { "choices", choices }
            };
        }

        internal static JsonSerializer JsonSerializer = JsonSerializer.CreateDefault(GetJsonSerializerSettings());

        internal static ParameterCollection GetParameterCollectionFromAttributes(this Type type, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            var parameterCollection = new ParameterCollection();
            foreach (var property in type.GetRuntimeProperties())
            {
                var ppa = property.GetCustomAttribute<ParameterPropertyAttribute>();
                if (ppa != null)
                {
                    var key = ppa.Key;
                    if (key == null)
                    {
                        key = property.Name;
                    }
                    ParameterCollection aInfo = null;
                    GetAdditionalInfoFromAttributes(property.GetCustomAttributes<AdditionalInfoAttribute>(), ref aInfo, customConverters);
                    var pValue = property.GetValue(value);
                    if (ppa.ParameterType.HasValue)
                    {
                        parameterCollection.Add(key, pValue, ppa.ParameterType.Value, aInfo, customConverters);
                    }
                    else if (pValue != null)
                    {
                        parameterCollection.Add(key, pValue, pValue.GetType(), aInfo, customConverters);
                    }
                    else
                    {
                        parameterCollection.Add(key, pValue, property.PropertyType, aInfo, customConverters);
                    }
                }
            }
            foreach (var field in type.GetRuntimeFields())
            {
                var ppa = field.GetCustomAttribute<ParameterPropertyAttribute>();
                if (ppa != null)
                {
                    var key = ppa.Key;
                    if (key == null)
                    {
                        key = field.Name;
                    }
                    ParameterCollection aInfo = null;
                    GetAdditionalInfoFromAttributes(field.GetCustomAttributes<AdditionalInfoAttribute>(), ref aInfo, customConverters);
                    var fValue = field.GetValue(value);
                    if (ppa.ParameterType.HasValue)
                    {
                        parameterCollection.Add(key, fValue, ppa.ParameterType.Value, aInfo, customConverters);
                    }
                    else if (fValue != null)
                    {
                        parameterCollection.Add(key, fValue, fValue.GetType(), aInfo, customConverters);
                    }
                    else
                    {
                        parameterCollection.Add(key, fValue, field.FieldType, aInfo, customConverters);
                    }
                }
            }
            return parameterCollection;
        }

        internal static object GetObjectFromAttributes(this Type typeToGet, JToken value, AttributeConvertibleAttribute acAttribute, IEnumerable<IParameterValueConverter> customConverters)
        {
            var obj = Activator.CreateInstance(typeToGet);

            if (acAttribute.ParameterType == ParameterType.ParameterCollection)
            {
                var parameterCollection = value.ToObject<ParameterCollection>(JsonSerializer);
                foreach (var property in typeToGet.GetRuntimeProperties())
                {
                    var ppa = property.GetCustomAttribute<ParameterPropertyAttribute>();
                    if (ppa != null)
                    {
                        var key = ppa.Key;
                        if (key == null)
                        {
                            key = property.Name;
                        }
                        if (parameterCollection.HasKeyAndCanConvertTo(key, property.PropertyType, customConverters))
                        {
                            property.SetValue(obj, parameterCollection.GetByKey(key, property.PropertyType, customConverters));
                        }
                    }
                }
                foreach (var field in typeToGet.GetRuntimeFields())
                {
                    var ppa = field.GetCustomAttribute<ParameterPropertyAttribute>();
                    if (ppa != null)
                    {
                        var key = ppa.Key;
                        if (key == null)
                        {
                            key = field.Name;
                        }
                        if (parameterCollection.HasKeyAndCanConvertTo(key, field.FieldType, customConverters))
                        {
                            field.SetValue(obj, parameterCollection.GetByKey(key, field.FieldType, customConverters));
                        }
                    }
                }
            }

            return obj;
        }

        internal static void GetAdditionalInfoFromAttributes(this IEnumerable<AdditionalInfoAttribute> attributes, ref ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (attributes != null && attributes.Count() > 0)
            {
                if (additionalInfo == null)
                {
                    additionalInfo = new ParameterCollection();
                }
                foreach (var aInfoAttr in attributes)
                {
                    if (aInfoAttr.KeyIsPath)
                    {
                        var parts = aInfoAttr.Key.Split(new string[] { aInfoAttr.KeyPathDivider }, StringSplitOptions.RemoveEmptyEntries);
                        CreatePathAndAddNewItem(ref additionalInfo, parts, aInfoAttr, customConverters);
                    }
                    else
                    {
                        if (!additionalInfo.HasKey(aInfoAttr.Key))
                        {
                            if (aInfoAttr.ParameterTypeIsSet)
                            {
                                additionalInfo.Add(aInfoAttr.Key, aInfoAttr.Value, aInfoAttr.ParameterType, null, customConverters);
                            }
                            else
                            {
                                additionalInfo.Add(aInfoAttr.Key, aInfoAttr.Value, null, customConverters);
                            }
                        }
                        else if (aInfoAttr.OverrideIfKeyExist)
                        {
                            additionalInfo.GetParameterByKey(aInfoAttr.Key).SetValue(aInfoAttr.Value, customConverters);
                        }
                    }
                }
            }
        }

        private static bool CreatePathAndAddNewItem(ref ParameterCollection additionalInfo, string[] parts, AdditionalInfoAttribute aInfoAttr, IEnumerable<IParameterValueConverter> customConverters)
        {
            var aInfoList = new List<ParameterCollection>
            {
                additionalInfo
            };
            for (var i = 0; i < parts.Length - 1; i++)
            {
                if (aInfoList[i].HasKey(parts[i]))
                {
                    if (aInfoList[i].GetParameterType(parts[i]) != ParameterType.ParameterCollection)
                    {
                        return false;
                    }
                    aInfoList.Add(aInfoList[i].GetByKey<ParameterCollection>(parts[i], customConverters));
                }
                else
                {
                    aInfoList.Add(new ParameterCollection());
                }
            }

            var lastAInfoNumber = aInfoList.Count() - 1;
            if (!aInfoList[lastAInfoNumber].HasKey(parts[lastAInfoNumber]))
            {
                if (aInfoAttr.ParameterTypeIsSet)
                {
                    aInfoList[lastAInfoNumber].Add(parts[lastAInfoNumber], aInfoAttr.Value, aInfoAttr.ParameterType, null, customConverters);
                }
                else
                {
                    aInfoList[lastAInfoNumber].Add(parts[lastAInfoNumber], aInfoAttr.Value, null, customConverters);
                }
            }
            else if (aInfoAttr.OverrideIfKeyExist)
            {
                aInfoList[lastAInfoNumber].GetParameterByKey(parts[lastAInfoNumber - 1]).SetValue(aInfoAttr.Value, customConverters);
            }

            for (var i = lastAInfoNumber - 1; i > 0; i--)
            {
                if (aInfoList[i].HasKey(parts[i]))
                {
                    aInfoList[i].GetParameterByKey(parts[i]).SetValue(aInfoList[i + 1], customConverters);
                }
                else
                {
                    aInfoList[i].Add(parts[i], aInfoList[i + 1], ParameterType.ParameterCollection, null, customConverters);
                }
            }

            if (additionalInfo.HasKey(parts[0]))
            {
                additionalInfo.GetParameterByKey(parts[0]).SetValue(aInfoList[1], customConverters);
            }
            else
            {
                additionalInfo.Add(parts[0], aInfoList[1], ParameterType.ParameterCollection, null, customConverters);
            }

            return true;
        }

        internal static IEnumerable<Type> GetGenericIEnumerables(this Type type)
        {
            return type.GetInterfaces()
                    .Where(t => t.IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(t => t.GetGenericArguments()[0]);
        }

        internal static IEnumerable<T> ConcatWithNullCheck<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            return (list1 ?? Enumerable.Empty<T>()).Concat(list2 ?? Enumerable.Empty<T>());
        }

        internal static ParameterType? GuessType(JToken token, bool skipNullValues, bool convertBase64ToBytesType)
        {
            switch (token.Type)
            {
                case JTokenType.Integer:
                    return ParameterType.Int;
    
                case JTokenType.Float:
                    return ParameterType.Decimal;
    
                case JTokenType.Boolean:
                    return ParameterType.Bool;
    
                case JTokenType.String:
                    var str = token.ToString();

                    if (DateTime.TryParse(str, out var dt))
                    {
                        return dt.TimeOfDay == TimeSpan.Zero ? ParameterType.Date : ParameterType.DateTime;
                    }

                    if (convertBase64ToBytesType)
                    {
                        try { Convert.FromBase64String(str); return ParameterType.Bytes; } catch { }
                    }
    
                    return str.Contains('\n') ? ParameterType.String_Multiline : ParameterType.String;
    
                case JTokenType.Array:
                    var first = token.First;
                    if (first != null)
                    {
                        switch (first.Type)
                        {
                            case JTokenType.String:
                                bool hasMultiline = token.Any(t => t.Type == JTokenType.String && t.ToString().Contains('\n'));
                                return hasMultiline ? ParameterType.String_Multiline_IEnumerable : ParameterType.String_IEnumerable;
                            case JTokenType.Integer: return ParameterType.Int_IEnumerable;
                            case JTokenType.Float: return ParameterType.Decimal_IEnumerable;
                            case JTokenType.Boolean: return ParameterType.Bool_IEnumerable;
                            case JTokenType.Date: return ParameterType.DateTime_IEnumerable;
                            case JTokenType.Object: return ParameterType.ParameterCollection_IEnumerable;
                        }
                    }
                    return ParameterType.String_IEnumerable;
    
                case JTokenType.Object:
                    var obj = (JObject)token;

                    if (obj.ContainsKey("value") && obj.ContainsKey("choices") && obj.ContainsKey("type"))
                    {
                        return ParameterType.Enum;
                    }

                    if (obj.ContainsKey("value") && obj["value"] is JArray && obj.ContainsKey("choices"))
                    {
                        return ParameterType.SelectMany;
                    }

                    if (obj.ContainsKey("value") && obj["value"] is JValue && obj.ContainsKey("choices"))
                    {
                        return ParameterType.SelectOne;
                    }
    
                    return ParameterType.ParameterCollection;

                case JTokenType.Null:
                    if (skipNullValues)
                    {
                        return null;
                    }
                    else
                    {
                        return ParameterType.String;
                    }

                default:
                    return ParameterType.String;
            }
        }

        internal static string RemoveFirstOccurence(this string str, string toRemove)
        {
            int index = str.IndexOf(toRemove, StringComparison.Ordinal);

            if (index >= 0)
            {
                // Remove characters starting at the found index for the length of the substring
                return str.Remove(index, toRemove.Length);
            }

            return str;
        }

        /// <summary>
        /// Get a value based on a path containing keys.
        /// </summary>
        /// <param name="list">The list of ParameterCollection</param>
        /// <param name="path">The given path of keys.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as the wanted type.</returns>
        public static T GetByPath<T>(this IEnumerable<ParameterCollection> list, string path, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            return (T)GetByPath(list, path, typeof(T), parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
        }

        /// <summary>
        /// Get a value based on a path containing keys.
        /// </summary>
        /// <param name="list">The list of ParameterCollection</param>
        /// <param name="path">The given path of keys.</param>
        /// <param name="type">The wanted type.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public static object GetByPath(this IEnumerable<ParameterCollection> list, string path, Type type, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            var pathDivided = path.Split(new string[] {pathDivider}, StringSplitOptions.RemoveEmptyEntries);
            
            if (pathDivided.Length == 0 || string.IsNullOrEmpty(pathDivided[0]))
            {
                throw new ArgumentException("The path can't be empty.");
            }

            var listMarkerPattern = "^" + Regex.Escape(listMarker).Replace(@"\{0}", @"(\d+)") + "$";

            var isListMarker = Regex.Match(pathDivided[0], listMarkerPattern);

            int.TryParse(isListMarker.Groups[1].Value, out int listMarkerNumber);

            if (isListMarker.Success && pathDivided.Length > 1)
            {
                if (listMarkerNumber >= list.Count())
                {
                    throw new ArgumentOutOfRangeException("The item number given is bigger than the list itself.");
                }

                return list.ToArray()[listMarkerNumber].GetByPath(path.RemoveFirstOccurence(pathDivided[0]), type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
            }

            if (isListMarker.Success && pathDivided.Length == 1)
            {
                throw new ArgumentException("To just get an entry of the root ParameterCollection-list, please use normal list/array/linq functionality. Support of getting a whole entry of a ParameterCollection-list is supported as long as it is not the root list you try to get.");
            }
            
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                Type innerType = null;
                if (type.IsArray)
                {
                    innerType = type.GetElementType();
                }
                else if (type.IsGenericType)
                {
                    innerType = type.GetGenericArguments()[0]; 
                }

                if (innerType == null)
                {
                    throw new ArgumentException("Did not expect innerType to still be null. Should have gotten a value.", nameof(innerType));
                }

                var listOfResults = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(innerType));

                foreach(var item in list)
                {
                    if (pathDivided.Length > 1)
                    {
                        try
                        {
                            var res = item.GetByPath(path, type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                            if (res is IEnumerable enumerableResult && !(res is string))
                            {
                                foreach(var resItem in enumerableResult)
                                {
                                    listOfResults.Add(resItem);
                                }
                            }
                            else if (res != null)
                            {
                                listOfResults.Add(res);
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            var res = item.GetByPath(path, innerType, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                            if (res != null)
                            {
                                listOfResults.Add(res);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                if (type.IsArray)
                {
                    Array destinationArray = Array.CreateInstance(innerType, listOfResults.Count);
                    listOfResults.CopyTo(destinationArray, 0);
                    return destinationArray;
                }
                
                return listOfResults;
            }

            foreach(var item in list)
            {
                if (!item.HasKey(pathDivided[0]))
                {
                    continue;
                }

                /*var paramType = item.GetParameterByKey(pathDivided[0]).Type;
                if (paramType == ParameterType.ParameterCollection)
                {*/
                    try
                    {
                        return item/*.GetByKey<ParameterCollection>(pathDivided[0], parameterValueConverters)*/.GetByPath(path/*.RemoveFirstOccurence(pathDivided[0])*/, type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                    }
                    catch
                    {
                        continue;
                    }
                /*}
                else if (paramType == ParameterType.ParameterCollection_IEnumerable)
                {
                    try
                    {
                        return item/*.GetByKey<ParameterCollection[]>(pathDivided[0], parameterValueConverters)*///.GetByPath(path.RemoveFirstOccurence(pathDivided[0]), type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                    /*}
                    catch
                    {
                        continue;
                    }
                }/*
                /*else if (pathDivided.Length == 1)
                {
                    try
                    {
                        return item.GetByKey(pathDivided[0], type, parameterValueConverters);
                    }
                    catch(Exception ex)
                    {
                        throw new ArgumentException("Could not get a value for given path. Error was: " + ex.Message, ex);
                    }
                }*/
            }

            throw new ArgumentException("Could not get a value for given path.");
        }
    }
}

