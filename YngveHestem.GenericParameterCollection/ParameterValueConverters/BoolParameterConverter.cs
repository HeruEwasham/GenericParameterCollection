using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class BoolParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bool)
            {
                return targetType == typeof(bool) || targetType == typeof(int) || targetType == typeof(string) || typeof(IEnumerable<bool>).IsAssignableFrom(targetType);
            }
            else if (sourceType == ParameterType.Int && targetType == typeof(bool))
            {
                var value = rawValue.ToObject<int>(jsonSerializer);
                return value == 0 || value == 1;
            }
            else if (sourceType == ParameterType.Bool_IEnumerable)
            {
                return typeof(IEnumerable<bool>).IsAssignableFrom(targetType) || typeof(IEnumerable<int>).IsAssignableFrom(targetType) || typeof(IEnumerable<string>).IsAssignableFrom(targetType);
            }
            else if (sourceType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool>).IsAssignableFrom(targetType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            if (targetType == ParameterType.Bool)
            {
                return sourceType == typeof(bool) || (sourceType == typeof(int) && ((int)value == 0 || (int)value == 1)) || (sourceType == typeof(string) && bool.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.Int && sourceType == typeof(bool))
            {
                return true;
            }
            else if (targetType == ParameterType.Bool_IEnumerable)
            {
                return typeof(IEnumerable<bool>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<int>).IsAssignableFrom(sourceType) && ((IEnumerable<int>)value).All(v => v == 0 || v == 1)) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => bool.TryParse(v, out _)));
            }
            else if (targetType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool>).IsAssignableFrom(sourceType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bool)
            {
                var value = rawValue.ToObject<bool>(jsonSerializer);
                if (targetType == typeof(bool))
                {
                    return value;
                }
                else if (targetType == typeof(int))
                {
                    return Convert.ToInt32(value);
                }
                else if (targetType == typeof(string))
                {
                    return value.ToString();
                }
                else if (typeof(IEnumerable<bool>).IsAssignableFrom(targetType))
                {
                    return value.ToIEnumerable().ToCorrectIEnumerable(targetType);
                }
            }
            else if (sourceType == ParameterType.Int && targetType == typeof(bool))
            {
                return Convert.ToBoolean(rawValue.ToObject<int>(jsonSerializer));
            }
            else if (sourceType == ParameterType.Bool_IEnumerable)
            {
                var value = rawValue.ToObject<IEnumerable<bool>>(jsonSerializer);
                if (typeof(IEnumerable<bool>).IsAssignableFrom(targetType))
                {
                    return value.ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    value.Select(v => Convert.ToInt32(value)).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    value.Select(v => v.ToString()).ToCorrectIEnumerable(targetType);
                }
            }
            else if (sourceType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool>).IsAssignableFrom(targetType))
            {
                return rawValue.ToObject<IEnumerable<int>>(jsonSerializer).Select(v => Convert.ToBoolean(v)).ToCorrectIEnumerable(targetType);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BoolParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.Bool)
            {
                if (sourceType == typeof(bool))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(int))
                {
                    return JToken.FromObject(Convert.ToBoolean((int)value), jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(bool.Parse((string)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Int && sourceType == typeof(bool))
            {
                return JToken.FromObject(Convert.ToInt32((bool)value), jsonSerializer);
            }
            else if (targetType == ParameterType.Bool_IEnumerable)
            {
                if (typeof(IEnumerable<bool>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<int>)value).Select(v => Convert.ToBoolean(v)), jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<string>)value).Select(v => bool.Parse(v)), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool>).IsAssignableFrom(sourceType))
            {
                return JToken.FromObject(((IEnumerable<bool>)value).Select(v => Convert.ToInt32(v)), jsonSerializer);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BoolParameterConverter));
        }
    }
}

