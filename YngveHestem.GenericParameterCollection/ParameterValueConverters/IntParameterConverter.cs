using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class IntParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Int)
            {
                return targetType == typeof(int) || targetType == typeof(float) || targetType == typeof(double) || targetType == typeof(long) || targetType == typeof(decimal) || targetType == typeof(string);
            }
            else if (sourceType == ParameterType.Int_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(targetType) || typeof(IEnumerable<float>).IsAssignableFrom(targetType) || typeof(IEnumerable<double>).IsAssignableFrom(targetType) || typeof(IEnumerable<long>).IsAssignableFrom(targetType) || typeof(IEnumerable<decimal>).IsAssignableFrom(targetType) || typeof(IEnumerable<string>).IsAssignableFrom(targetType);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            if (targetType == ParameterType.Int)
            {
                return sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double) || sourceType == typeof(long) || sourceType == typeof(decimal) || (sourceType == typeof(string) && int.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.Int_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long>).IsAssignableFrom(sourceType) || typeof(IEnumerable<decimal>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => int.TryParse(v, out _)));
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Int)
            {
                var value = rawValue.ToObject<int>(jsonSerializer);
                if (targetType == typeof(int))
                {
                    return value;
                }
                else if (targetType == typeof(float))
                {
                    return (float)value;
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
            else if (sourceType == ParameterType.Int_IEnumerable)
            {
                var value = rawValue.ToObject<IEnumerable<int>>(jsonSerializer);
                if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    return value.ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<float>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (float)v).ToCorrectIEnumerable(targetType);
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

            throw new ArgumentException("The values was not supported to be converted by " + nameof(IntParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
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
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(IntParameterConverter));
        }
    }
}

