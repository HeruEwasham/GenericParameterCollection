using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class BytesParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bytes)
            {
                return targetType == typeof(byte[]) || targetType == typeof(string);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            if (targetType == ParameterType.Bytes)
            {
                return sourceType == typeof(byte[]) || sourceType == typeof(string);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bytes)
            {
                var value = Convert.FromBase64String(rawValue.ToObject<string>(jsonSerializer));
                if (targetType == typeof(byte[]))
                {
                    return value;
                }
                else if (targetType == typeof(string))
                {
                    return BitConverter.ToString(value);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BytesParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.Bytes)
            {
                if (value == null)
                {
                    return JToken.FromObject(Convert.ToBase64String(new byte[0]));
                }
                else if (sourceType == typeof(byte[]))
                {
                    return JToken.FromObject(Convert.ToBase64String((byte[])value));
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(Convert.ToBase64String(Encoding.UTF8.GetBytes((string)value)));
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BytesParameterConverter));
        }
    }
}

