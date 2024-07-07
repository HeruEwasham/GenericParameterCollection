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
                if (rawValue == null)
                {
                    return true;
                }
                return customConverters.ConcatWithNullCheck(Parameter.DefaultParameterValueConverters).Any(c => c.CanConvertFromParameter(sourceType, underlyingType, rawValue, customConverters, jsonSerializer));
            }

            return false;
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return false; // Converting from nullable values should not be necessarry as the value gotten if not null is the underlying type on nullable types.
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType == null)
            {
                throw new ArgumentException("The values was not supported to be converted by " + nameof(NullableParameterConverter) + ". It is not nullable.");
            }
            if (rawValue == null || rawValue.Type == JTokenType.Null)
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
            throw new NotImplementedException();
        }
    }
}

