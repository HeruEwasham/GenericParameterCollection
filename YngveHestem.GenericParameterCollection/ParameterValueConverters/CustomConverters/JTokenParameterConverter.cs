using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters.CustomConverters
{
    public class JTokenParameterConverter : IParameterValueConverter
    {
        private readonly bool _convertBase64ToBytesType;

        /// <summary>
        /// Creates a JTokenParameterConverter.
        /// </summary>
        /// <param name="convertBase64ToBytesType">If converting from JToken, and the value is base64-encoded, should the type be set to Parameter.Bytes?</param>
        public JTokenParameterConverter(bool convertBase64ToBytesType = false)
        {
            _convertBase64ToBytesType = convertBase64ToBytesType;
        }
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return targetType == typeof(JToken) || typeof(IEnumerable<bool>).IsAssignableFrom(targetType);
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return sourceType == typeof(JToken) && targetType == ParameterConverterExtensions.GuessType((JToken)value, false, _convertBase64ToBytesType);
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (targetType == typeof(JToken))
            {
                return rawValue;
            }
            else
            {
                return new JToken[] { rawValue }.ToCorrectIEnumerable(targetType);
            }
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return (JToken)value;
        }
    }
}