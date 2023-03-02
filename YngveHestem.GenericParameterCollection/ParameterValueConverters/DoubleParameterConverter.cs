using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class DoubleParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Double)
            {
                return targetType == typeof(int) || targetType == typeof(float) || targetType == typeof(double) || targetType == typeof(long) || targetType == typeof(string);
            }
            else if (sourceType == ParameterType.Double_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(targetType) || typeof(IEnumerable<float>).IsAssignableFrom(targetType) || typeof(IEnumerable<double>).IsAssignableFrom(targetType) || typeof(IEnumerable<long>).IsAssignableFrom(targetType);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            if (targetType == ParameterType.Double)
            {
                return sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double) || sourceType == typeof(long) || (sourceType == typeof(string) && double.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.Double_IEnumerable)
            {
                return typeof(IEnumerable<int>).IsAssignableFrom(sourceType) || typeof(IEnumerable<float>).IsAssignableFrom(sourceType) || typeof(IEnumerable<double>).IsAssignableFrom(sourceType) || typeof(IEnumerable<long>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => double.TryParse(v, out _)));
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Double)
            {
                var value = rawValue.ToObject<double>(jsonSerializer);
                if (targetType == typeof(double))
                {
                    return value;
                }
                else if (targetType == typeof(int))
                {
                    return (int)value;
                }
                else if (targetType == typeof(float))
                {
                    return (float)value;
                }
                else if (targetType == typeof(long))
                {
                    return (long)value;
                }
                else if (targetType == typeof(string))
                {
                    return value.ToString();
                }
            }
            else if (sourceType == ParameterType.Double_IEnumerable)
            {
                var value = rawValue.ToObject<IEnumerable<double>>(jsonSerializer);
                if (typeof(IEnumerable<double>).IsAssignableFrom(targetType))
                {
                    return value.ToArray();
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (int)v).ToArray();
                }
                else if (typeof(IEnumerable<float>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (float)v).ToArray();
                }
                else if (typeof(IEnumerable<long>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => (long)v).ToArray();
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    return value.Select(v => v.ToString()).ToArray();
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DoubleParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.Double)
            {
                if (sourceType == typeof(double))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(int)
                    || sourceType == typeof(float)
                    || sourceType == typeof(long))
                {
                    return JToken.FromObject((double)value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(double.Parse((string)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Double_IEnumerable)
            {
                if (typeof(IEnumerable<double>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<int>)value).Select(v => (double)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<float>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<float>)value).Select(v => (double)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<long>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<long>)value).Select(v => (double)v), jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<string>)value).Select(v => double.Parse(v)), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DoubleParameterConverter));
        }
    }
}

