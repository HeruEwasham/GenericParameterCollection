using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
                return typeof(IEnumerable<Parameter>);
            }
            else
            {
                return typeof(string);
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
                FloatParseHandling = FloatParseHandling.Decimal
            };
        }

        public static JsonConverter[] GetJsonConverters()
        {
            return new JsonConverter[] {
                    new StringEnumConverter()
                };
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

        internal static JsonSerializer JsonSerializer = JsonSerializer.CreateDefault(ParameterConverterExtensions.GetJsonSerializerSettings());

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

        internal static ParameterType? GuessType(JToken token, bool skipNullValues)
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
                        return dt.TimeOfDay == TimeSpan.Zero ? ParameterType.Date : ParameterType.DateTime;
    
                    try { Convert.FromBase64String(str); return ParameterType.Bytes; } catch { }
    
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
    }
}

