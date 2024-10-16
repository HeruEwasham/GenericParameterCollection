using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class IntParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Int)
            {
                if (rawValue == null)
                {
                    return false;
                }
                return targetType == typeof(int) || targetType == typeof(float) || targetType == typeof(double) || targetType == typeof(long) || targetType == typeof(decimal) || targetType == typeof(string);
            }
            else if (sourceType == ParameterType.Int_IEnumerable)
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
            if (targetType == ParameterType.Int)
            {
                return sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double) || sourceType == typeof(long) || sourceType == typeof(decimal) || (sourceType == typeof(string) && int.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.Int_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long>).IsAssignableFrom(sourceType) || typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => v == null || int.TryParse(v, out _)))
                    || typeof(IEnumerable<int?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<decimal?>).IsAssignableFrom(sourceType);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Int)
            {
                if (targetType == typeof(int))
                {
                    return rawValue.ToObject<int>(jsonSerializer);
                }
                else if (targetType == typeof(float))
                {
                    return rawValue.ToObject<float>(jsonSerializer);
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
            else if (sourceType == ParameterType.Int_IEnumerable)
            {
                if (rawValue == null)
                {
                    return null;
                }
                if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<int>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<float>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<float>>(jsonSerializer).ToCorrectIEnumerable(targetType);
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
                else if (typeof(IEnumerable<int?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<int?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<float?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<float?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
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

            throw new ArgumentException("The values was not supported to be converted by " + nameof(IntParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.Int)
            {
                if (sourceType == typeof(int) || sourceType == typeof(long))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(float)
                    || sourceType == typeof(double)
                    || sourceType == typeof(decimal))
                {
                    return JToken.FromObject((int)value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(int.Parse((string)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Int_IEnumerable)
            {
                if (typeof(IEnumerable<int>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<float>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<float>)value).Select(v => (int)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<double>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<double>)value).Select(v => (int)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<decimal>)value).Select(v => (int)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<string>)value).Select(v => int.Parse(v)), jsonSerializer);
                }
                else if (typeof(IEnumerable<int?>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<float?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<float?>)value).Select(v => (int?)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<double?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<double?>)value).Select(v => (int?)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<decimal?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<decimal?>)value).Select(v => (int?)v), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(IntParameterConverter));
        }
    }
}

