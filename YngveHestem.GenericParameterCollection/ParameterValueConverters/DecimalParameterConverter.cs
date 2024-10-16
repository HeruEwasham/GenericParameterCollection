using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class DecimalParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Decimal)
            {
                if (rawValue == null)
                {
                    return false;
                }
                return targetType == typeof(int) || targetType == typeof(float) || targetType == typeof(double) || targetType == typeof(long) || targetType == typeof(decimal) || targetType == typeof(string);
            }
            else if (sourceType == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(targetType) || typeof(IEnumerable<float>).IsAssignableFrom(targetType) || typeof(IEnumerable<double>).IsAssignableFrom(targetType) || typeof(IEnumerable<long>).IsAssignableFrom(targetType) || typeof(IEnumerable<decimal>).IsAssignableFrom(targetType) || typeof(IEnumerable<string>).IsAssignableFrom(targetType)
                    || typeof(IEnumerable<int?>).IsAssignableFrom(targetType) || typeof(IEnumerable<float?>).IsAssignableFrom(targetType) || typeof(IEnumerable<double?>).IsAssignableFrom(targetType) || typeof(IEnumerable<long?>).IsAssignableFrom(targetType) || typeof(IEnumerable<decimal?>).IsAssignableFrom(targetType);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType == ParameterType.Decimal)
            {
                return sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double) || sourceType == typeof(long) || sourceType == typeof(decimal) || (sourceType == typeof(string) && float.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long>).IsAssignableFrom(sourceType) || typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => v == null || decimal.TryParse(v, out _)))
                    || typeof(IEnumerable<int?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<decimal?>).IsAssignableFrom(sourceType);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Decimal)
            {
                if (targetType == typeof(float))
                {
                    return rawValue.ToObject<float>(jsonSerializer);
                }
                else if (targetType == typeof(int))
                {
                    return rawValue.ToObject<int>(jsonSerializer);
                }
                else if (targetType == typeof(double))
                {
                    return rawValue.ToObject<double>(jsonSerializer);
                }
                else if (targetType == typeof(long))
                {
                    return rawValue.ToObject<long>(jsonSerializer);
                }
                else if (targetType == typeof(decimal))
                {
                    return rawValue.ToObject<decimal>(jsonSerializer);
                }
                else if (targetType == typeof(string))
                {
                    return rawValue.ToObject<string>(jsonSerializer);
                }
            }
            else if (sourceType == ParameterType.Decimal_IEnumerable)
            {
                if (rawValue == null)
                {
                    return null;
                }
                if (typeof(IEnumerable<float>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<float>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<int>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<double>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<double>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<long>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<long>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<decimal>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<decimal>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<string>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<float?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<float?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<int?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<int?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<double?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<double?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<long?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<long?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<decimal?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<decimal?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DecimalParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
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
                    || typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<float?>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<int?>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<double?>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<long?>).IsAssignableFrom(sourceType)
                    || typeof(IEnumerable<decimal?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<string>)value)/*.Select(decimal.Parse)*/, jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DecimalParameterConverter));
        }
    }
}

