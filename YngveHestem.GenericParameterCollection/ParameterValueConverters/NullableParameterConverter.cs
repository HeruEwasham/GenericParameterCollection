using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class NullableParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                if (rawValue == null || (sourceType == ParameterType.Enum && typeof(Enum).IsAssignableFrom(underlyingType)))
                {
                    return true;
                }
                return customConverters.ConcatWithNullCheck(Parameter.DefaultParameterValueConverters).Any(c => c.CanConvertFromParameter(sourceType, underlyingType, rawValue, customConverters, jsonSerializer));
            }

            return false;
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            var underlyingType = Nullable.GetUnderlyingType(sourceType);
            return underlyingType != null && customConverters.ConcatWithNullCheck(Parameter.DefaultParameterValueConverters).Any(c => c.CanConvertFromValue(targetType, underlyingType, value, customConverters));
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType == null)
            {
                throw new ArgumentException("The values was not supported to be converted by " + nameof(NullableParameterConverter) + ". It is not nullable.");
            }
            if (rawValue == null || rawValue.Type == JTokenType.Null
                || (sourceType == ParameterType.Enum && typeof(Enum).IsAssignableFrom(underlyingType) && string.IsNullOrEmpty(rawValue.ToObject<ParameterCollection>().GetByKey<string>("value"))))
            {
                return null;
            }
            var converters = customConverters.ConcatWithNullCheck(Parameter.DefaultParameterValueConverters);
            var converter = converters.FirstOrDefault(c => c.CanConvertFromParameter(sourceType, underlyingType, rawValue, customConverters, jsonSerializer));
            if (converter != null)
            {
                return converter.ConvertFromParameter(sourceType, underlyingType, rawValue, customConverters, jsonSerializer);
            }
            else
            {
                throw new ArgumentException("The values was not supported to be converted by " + nameof(NullableParameterConverter) + ". No converter to convert the underlying type was found.");
            }
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            var underlyingType = Nullable.GetUnderlyingType(sourceType);
            if (underlyingType == null)
            {
                throw new ArgumentException("The values was not supported to be converted by " + nameof(NullableParameterConverter) + ". It is not nullable.");
            }

            if (targetType == ParameterType.Enum && typeof(Enum).IsAssignableFrom(underlyingType))
            {
                var valueAsString = string.Empty;
                if (value != null)
                {
                    valueAsString = Enum.GetName(underlyingType, value);
                }
                var choices = new List<string>()
                {
                    string.Empty
                };
                choices.AddRange(Enum.GetNames(underlyingType));
                return JToken.FromObject(new ParameterCollection
                    {
                        { "value", valueAsString, false },
                        { "type", underlyingType.FullName },
                        { "choices", choices }
                    }, jsonSerializer);
            }

            if (value == null)
            {
                return null;
            }

            var converters = customConverters.ConcatWithNullCheck(Parameter.DefaultParameterValueConverters);
            var converter = converters.FirstOrDefault(c => c.CanConvertFromValue(targetType, underlyingType, value, customConverters));
            if (converter != null)
            {
                return converter.ConvertFromValue(targetType, underlyingType, value, customConverters, jsonSerializer);
            }
            else
            {
                throw new ArgumentException("The values was not supported to be converted by " + nameof(NullableParameterConverter) + ". No converter to convert the underlying type was found.");
            }
        }
    }
}

