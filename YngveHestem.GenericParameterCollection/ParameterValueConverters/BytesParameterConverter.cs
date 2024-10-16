﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class BytesParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bytes)
            {
                return typeof(IEnumerable<byte>).IsAssignableFrom(targetType) || targetType == typeof(string);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType == ParameterType.Bytes)
            {
                return typeof(IEnumerable<byte>).IsAssignableFrom(sourceType) || sourceType == typeof(string);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bytes)
            {
                if (rawValue == null)
                {
                    return null;
                }
                var value = Convert.FromBase64String(rawValue.ToObject<string>(jsonSerializer));
                if (typeof(IEnumerable<byte>).IsAssignableFrom(targetType))
                {
                    return value.ToCorrectIEnumerable(targetType);
                }
                else if (targetType == typeof(string))
                {
                    return BitConverter.ToString(value);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BytesParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.Bytes)
            {
                if (value == null)
                {
                    return JToken.FromObject(Convert.ToBase64String(new byte[0]), jsonSerializer);
                }
                else if (typeof(IEnumerable<byte>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(Convert.ToBase64String(((IEnumerable<byte>)value).ToArray()), jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(Convert.ToBase64String(Encoding.UTF8.GetBytes((string)value)), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BytesParameterConverter));
        }
    }
}

