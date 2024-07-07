using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class SelectParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return (sourceType == ParameterType.SelectOne || sourceType == ParameterType.SelectMany) && (targetType == typeof(string) || typeof(IEnumerable<string>).IsAssignableFrom(targetType) || targetType == typeof(ParameterCollection));
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return (targetType == ParameterType.SelectOne || targetType == ParameterType.SelectMany) && sourceType == typeof(ParameterCollection) && (((ParameterCollection)value).HasKeyAndCanConvertTo("value", typeof(string)) || ((ParameterCollection)value).HasKeyAndCanConvertTo("value", typeof(List<string>))) && ((ParameterCollection)value).HasKeyAndCanConvertTo("choices", typeof(List<string>));
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.SelectOne)
            {
                if (targetType == typeof(string))
                {
                    if (rawValue == null)
                    {
                        return string.Empty;
                    }
                    return rawValue.ToObject<ParameterCollection>().GetByKey<string>("value");
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    if (rawValue == null)
                    {
                        return Array.Empty<string>().ToCorrectIEnumerable(targetType);
                    }
                    return rawValue.ToObject<ParameterCollection>().GetByKey<IEnumerable<string>>("value").ToCorrectIEnumerable(targetType);
                }
                else if (targetType == typeof(ParameterCollection))
                {
                    if (rawValue == null)
                    {
                        return null;
                    }
                    return rawValue.ToObject<ParameterCollection>();
                }
            }
            else if (sourceType == ParameterType.SelectMany)
            {
                if (targetType == typeof(string))
                {
                    if (rawValue == null)
                    {
                        return string.Empty;
                    }
                    return string.Join(", ", rawValue.ToObject<ParameterCollection>().GetByKey<string[]>("value"));
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    if (rawValue == null)
                    {
                        return Array.Empty<string>().ToCorrectIEnumerable(targetType);
                    }
                    return rawValue.ToObject<ParameterCollection>().GetByKey<IEnumerable<string>>("value").ToCorrectIEnumerable(targetType);
                }
                else if (targetType == typeof(ParameterCollection))
                {
                    if (rawValue == null)
                    {
                        return null;
                    }
                    return rawValue.ToObject<ParameterCollection>();
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(SelectParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == typeof(ParameterCollection) && (targetType == ParameterType.SelectOne || targetType == ParameterType.SelectMany))
            {
                return JToken.FromObject(value);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(SelectParameterConverter));
        }
    }
}

