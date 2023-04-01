using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
	public class StringParameterConverter : IParameterValueConverter
	{
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            return ((sourceType == ParameterType.String || sourceType == ParameterType.String_Multiline) && targetType == typeof(string)) || ((sourceType == ParameterType.String_IEnumerable || sourceType == ParameterType.String_Multiline_IEnumerable) && (typeof(IEnumerable<string>).IsAssignableFrom(targetType) || targetType == typeof(string)));
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            return ((targetType == ParameterType.String || targetType == ParameterType.String_Multiline) && sourceType == typeof(string)) || ((targetType == ParameterType.String_IEnumerable || targetType == ParameterType.String_Multiline_IEnumerable) && (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) || sourceType == typeof(string)));
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if ((sourceType == ParameterType.String || sourceType == ParameterType.String_Multiline) && targetType == typeof(string))
            {
                return rawValue.ToObject<string>(jsonSerializer);
            }
            else if (sourceType == ParameterType.String_IEnumerable || sourceType == ParameterType.String_Multiline_IEnumerable)
            {
                if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<string>>(jsonSerializer).ToArray();
                }
                else if (targetType == typeof(string))
                {
                    return string.Join(", ", rawValue.ToObject<IEnumerable<string>>(jsonSerializer));
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(StringParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
        {
            if ((targetType == ParameterType.String || targetType == ParameterType.String_Multiline) && sourceType == typeof(string))
            {
                return JToken.FromObject(value, jsonSerializer);
            }
            else if (targetType == ParameterType.String_IEnumerable || targetType == ParameterType.String_Multiline_IEnumerable)
            {
                if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(new string[] { (string)value }, jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(StringParameterConverter));
        }
    }
}

