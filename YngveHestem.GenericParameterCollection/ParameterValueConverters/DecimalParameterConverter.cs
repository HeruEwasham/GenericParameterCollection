using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class DecimalParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Decimal)
            {
                return targetType == typeof(int) || targetType == typeof(float) || targetType == typeof(double) || targetType == typeof(long) || targetType == typeof(decimal) || targetType == typeof(string);
            }
            else if (sourceType == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(targetType) || typeof(IEnumerable<float>).IsAssignableFrom(targetType) || typeof(IEnumerable<double>).IsAssignableFrom(targetType) || typeof(IEnumerable<long>).IsAssignableFrom(targetType) || typeof(IEnumerable<decimal>).IsAssignableFrom(targetType) || typeof(IEnumerable<string>).IsAssignableFrom(targetType);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType == ParameterType.Decimal)
            {
                return sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double) || sourceType == typeof(long) || sourceType == typeof(decimal) || (sourceType == typeof(string) && float.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long>).IsAssignableFrom(sourceType) || typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => decimal.TryParse(v, out _)));
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Decimal)
            {
                var value = rawValue.ToObject<float>(jsonSerializer);
                if (targetType == typeof(float))
                {
                    return value;
                }
                else if (targetType == typeof(int))
                {
                    return (int)value;
                }
                else if (targetType == typeof(double))
                {
                    return (double)value;
                }
                else if (targetType == typeof(long))
                {
                    return (long)value;
                }
                else if (targetType == typeof(decimal))
                {
                    return (decimal)value;
                }
                else if (targetType == typeof(string))
                {
                    return value.ToString();
                }
            }
            else if (sourceType == ParameterType.Decimal_IEnumerable)
            {
                var value = rawValue.ToObject<IEnumerable<float>>(jsonSerializer);
                if (typeof(IEnumerable<float>).IsAssignableFrom(targetType))
                {
                    return value.ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (int)v).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<double>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (double)v).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<long>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (long)v).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<decimal>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (decimal)v).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => v.ToString()).ToCorrectIEnumerable(targetType);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DecimalParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.Decimal)
            {
                if (sourceType == typeof(float)
                    || sourceType == typeof(int)
                    || sourceType == typeof(double)
                    || sourceType == typeof(long)
                    || sourceType == typeof(decimal))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(float.Parse((string)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Decimal_IEnumerable)
            {
                if (typeof(IEnumerable<float>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<int>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<double>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<long>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<string>)value).Select(decimal.Parse), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DecimalParameterConverter));
        }
    }
}

