using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class StringParameterConverter : IParameterValueConverter
	{
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return ((sourceType == ParameterType.String || sourceType == ParameterType.String_Multiline) && (targetType == typeof(string) || typeof(IEnumerable<string>).IsAssignableFrom(targetType))) || ((sourceType == ParameterType.String_IEnumerable || sourceType == ParameterType.String_Multiline_IEnumerable) && (typeof(IEnumerable<string>).IsAssignableFrom(targetType) || targetType == typeof(string)));
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return ((targetType == ParameterType.String || targetType == ParameterType.String_Multiline) && (sourceType == typeof(string) || typeof(IEnumerable<string>).IsAssignableFrom(sourceType))) || ((targetType == ParameterType.String_IEnumerable || targetType == ParameterType.String_Multiline_IEnumerable) && (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) || sourceType == typeof(string)));
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (rawValue == null || rawValue.Type == JTokenType.Null)
            {
                return null;
            }
            if (sourceType == ParameterType.String || sourceType == ParameterType.String_Multiline)
            {
                if (targetType == typeof(string))
                {
                    return rawValue.ToObject<string>(jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<string>(jsonSerializer).ToIEnumerable().ToCorrectIEnumerable(targetType);
                }
            }
            else if (sourceType == ParameterType.String_IEnumerable || sourceType == ParameterType.String_Multiline_IEnumerable)
            {
                if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<string>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (targetType == typeof(string))
                {
                    return string.Join(", ", rawValue.ToObject<IEnumerable<string>>(jsonSerializer));
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(StringParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (value == null)
            {
                return null;
            }
            if (targetType == ParameterType.String || targetType == ParameterType.String_Multiline)
            {
                if (sourceType == typeof(string))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(string.Join(", ", (IEnumerable<string>)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.String_IEnumerable || targetType == ParameterType.String_Multiline_IEnumerable)
            {
                if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(value.ToIEnumerable(), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(StringParameterConverter));
        }
    }
}

