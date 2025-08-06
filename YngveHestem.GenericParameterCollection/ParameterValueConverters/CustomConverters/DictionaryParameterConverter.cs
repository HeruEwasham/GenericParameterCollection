using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters.CustomConverters
{
    public class DictionaryParameterConverter : IParameterValueConverter
    {
        private readonly string _keyName;
        private readonly string _valueName;

        private static readonly ConcurrentDictionary<Type, Type> _dictionaryInterfaceCache = new ConcurrentDictionary<Type, Type>();

        public DictionaryParameterConverter(string keyName = "Key", string valueName = "Value")
        {
            _keyName = keyName;
            _valueName = valueName;
        }

        public bool CanConvertFromValue(
            ParameterType targetType,
            Type sourceType,
            object value,
            ParameterCollection additionalInfo,
            IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType != ParameterType.ParameterCollection_IEnumerable)
            {
                return false;
            }

            var dictionaryInterface = _dictionaryInterfaceCache.GetOrAdd(sourceType, FindGenericDictionaryInterface);

            if (dictionaryInterface == null)
            {
                return false;
            }

            var genericArguments = dictionaryInterface.GetGenericArguments();
            var keyType = genericArguments[0];
            var valueType = genericArguments[1];

            var firstEntriesOrDefault = ExtractFirstEntryOrDefault(value, keyType, valueType);

            return ParameterConverterExtensions.CanConvertFromValue(firstEntriesOrDefault.Key, keyType, null, customConverters)
                && ParameterConverterExtensions.CanConvertFromValue(firstEntriesOrDefault.Value, valueType, null, customConverters);
        }

        public JToken ConvertFromValue(
            ParameterType targetType,
            Type sourceType,
            object value,
            ParameterCollection additionalInfo,
            IEnumerable<IParameterValueConverter> customConverters,
            JsonSerializer jsonSerializer)
        {
            if (targetType != ParameterType.ParameterCollection_IEnumerable)
            {
                throw new InvalidOperationException("Target type must be ParameterCollection_IEnumerable.");
            }

            if (value == null)
            {
                return JValue.CreateNull();
            }

            var dictionaryInterface = _dictionaryInterfaceCache.GetOrAdd(sourceType, FindGenericDictionaryInterface);

            if (dictionaryInterface == null)
            {
                throw new InvalidOperationException($"Type {sourceType} is not a supported dictionary type.");
            }

            var genericArguments = dictionaryInterface.GetGenericArguments();
            var keyType = genericArguments[0];
            var valueType = genericArguments[1];

            var resultList = new List<ParameterCollection>();

            var enumerable = (IEnumerable)value;
            foreach (var entry in enumerable)
            {
                var entryType = entry.GetType();
                var keyProp = entryType.GetProperty("Key");
                var valueProp = entryType.GetProperty("Value");

                var keyObj = keyProp?.GetValue(entry);
                var valueObj = valueProp?.GetValue(entry);

                var pairCollection = new ParameterCollection
                {
                    { _keyName, keyObj, keyType, null, customConverters },
                    { _valueName, valueObj, valueType, null, customConverters }
                };

                resultList.Add(pairCollection);
            }

            return JToken.FromObject(resultList, jsonSerializer);
        }

        public bool CanConvertFromParameter(
            ParameterType sourceType,
            Type targetType,
            JToken rawValue,
            ParameterCollection additionalInfo,
            IEnumerable<IParameterValueConverter> customConverters,
            JsonSerializer jsonSerializer)
        {
            if (sourceType != ParameterType.ParameterCollection_IEnumerable)
            {
                return false;
            }

            if (rawValue == null || rawValue.Type != JTokenType.Array)
            {
                return false;
            }

            var dictionaryInterface = _dictionaryInterfaceCache.GetOrAdd(targetType, FindGenericDictionaryInterface);

            if (dictionaryInterface == null)
            {
                return false;
            }

            var genericArguments = dictionaryInterface.GetGenericArguments();
            var keyType = genericArguments[0];
            var valueType = genericArguments[1];

            foreach (var item in rawValue.Children<JObject>())
            {
                var parameterCollection = item.ToObject<ParameterCollection>(jsonSerializer);
                if (parameterCollection == null)
                {
                    return false;
                }

                if (!parameterCollection.HasKeyAndCanConvertTo(_keyName, keyType, customConverters))
                {
                    return false;
                }

                if (!parameterCollection.HasKeyAndCanConvertTo(_valueName, valueType, customConverters))
                {
                    return false;
                }
            }

            return true;
        }

        public object ConvertFromParameter(
            ParameterType sourceType,
            Type targetType,
            JToken rawValue,
            ParameterCollection additionalInfo,
            IEnumerable<IParameterValueConverter> customConverters,
            JsonSerializer jsonSerializer)
        {
            if (sourceType != ParameterType.ParameterCollection_IEnumerable)
            {
                throw new InvalidOperationException("Source type must be ParameterCollection_IEnumerable.");
            }

            if (rawValue == null || rawValue.Type == JTokenType.Null)
            {
                return null;
            }

            var dictionaryInterface = _dictionaryInterfaceCache.GetOrAdd(targetType, FindGenericDictionaryInterface);

            if (dictionaryInterface == null)
            {
                throw new InvalidOperationException($"Target type {targetType} is not a supported dictionary type.");
            }

            var genericArguments = dictionaryInterface.GetGenericArguments();
            var keyType = genericArguments[0];
            var valueType = genericArguments[1];

            var dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));

            foreach (var item in rawValue.Children<JObject>())
            {
                var parameterCollection = item.ToObject<ParameterCollection>(jsonSerializer);
                if (parameterCollection == null)
                {
                    continue;
                }

                var key = parameterCollection.GetByKey(_keyName, keyType, customConverters);
                var value = parameterCollection.GetByKey(_valueName, valueType, customConverters);

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private static Type FindGenericDictionaryInterface(Type type)
        {
            while (type != null && type != typeof(object))
            {
                var dictInterface = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));

                if (dictInterface != null)
                    return dictInterface;

                type = type.BaseType;
            }

            return null;
        }

        private static KeyValuePair<object, object> ExtractFirstEntryOrDefault(object dictObj, Type keyType, Type valueType)
        {
            if (dictObj is IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    var entry = enumerator.Current;
                    var entryType = entry.GetType();

                    var key = entryType.GetProperty("Key")?.GetValue(entry);
                    var value = entryType.GetProperty("Value")?.GetValue(entry);

                    return new KeyValuePair<object, object>(key, value);
                }
            }

            // Hvis ikke dictionary har elementer eller er null
            return new KeyValuePair<object, object>(GetDefaultValue(keyType), GetDefaultValue(valueType));
        }

        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
