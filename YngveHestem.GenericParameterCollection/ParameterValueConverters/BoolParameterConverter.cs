using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class BoolParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.Bool)
            {
                if (rawValue == null)
                {
                    return false;
                }
                return targetType == typeof(bool) || targetType == typeof(int) || targetType == typeof(string) || typeof(IEnumerable<bool>).IsAssignableFrom(targetType);
            }
            else if (sourceType == ParameterType.Int && targetType == typeof(bool))
            {
                if (rawValue == null)
                {
                    return false;
                }
                var value = rawValue.ToObject<int>(jsonSerializer);
                return value == 0 || value == 1;
            }
            else if (sourceType == ParameterType.Bool_IEnumerable)
            {
                return typeof(IEnumerable<bool>).IsAssignableFrom(targetType) || typeof(IEnumerable<int>).IsAssignableFrom(targetType) || typeof(IEnumerable<string>).IsAssignableFrom(targetType)
                    || typeof(IEnumerable<bool?>).IsAssignableFrom(targetType) || typeof(IEnumerable<int?>).IsAssignableFrom(targetType);
            }
            else if (sourceType == ParameterType.Int_IEnumerable && (typeof(IEnumerable<bool>).IsAssignableFrom(targetType) || typeof(IEnumerable<bool?>).IsAssignableFrom(targetType)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
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
                return typeof(IEnumerable<bool>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<int>).IsAssignableFrom(sourceType) && ((IEnumerable<int>)value).All(v => v == 0 || v == 1)) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => v == null || bool.TryParse(v, out _)))
                    || typeof(IEnumerable<bool?>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<int?>).IsAssignableFrom(sourceType) && ((IEnumerable<int?>)value).All(v => v == null || v == 0 || v == 1));
            }
            else if (targetType == ParameterType.Int_IEnumerable && (typeof(IEnumerable<bool>).IsAssignableFrom(sourceType) || typeof(IEnumerable<bool?>).IsAssignableFrom(sourceType)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
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
                    return GetStringValue(value, additionalInfo);
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
                if (rawValue == null)
                {
                    return null;
                }
                if (typeof(IEnumerable<bool>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<bool>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<int>).IsAssignableFrom(targetType))
                {
                    rawValue.ToObject<IEnumerable<bool>>(jsonSerializer).Select(v => Convert.ToInt32(v)).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    rawValue.ToObject<IEnumerable<bool>>(jsonSerializer).Select(v => GetStringValue(v, additionalInfo)).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<bool?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<bool?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<int?>).IsAssignableFrom(targetType))
                {
                    rawValue.ToObject<IEnumerable<bool?>>(jsonSerializer).Select(v => v == null ? (int?)null : Convert.ToInt32(v)).ToCorrectIEnumerable(targetType);
                }
            }
            else if (sourceType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool>).IsAssignableFrom(targetType))
            {
                if (rawValue == null)
                {
                    return null;
                }
                return rawValue.ToObject<IEnumerable<int>>(jsonSerializer).Select(v => Convert.ToBoolean(v)).ToCorrectIEnumerable(targetType);
            }
            else if (sourceType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool?>).IsAssignableFrom(targetType))
            {
                if (rawValue == null)
                {
                    return null;
                }
                return rawValue.ToObject<IEnumerable<int?>>(jsonSerializer).Select(v => v == null ? (bool?)null : Convert.ToBoolean(v)).ToCorrectIEnumerable(targetType);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BoolParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
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
                    return JToken.FromObject(((IEnumerable<string>)value).Select(v => v == null ? (bool?)null : bool.Parse(v)), jsonSerializer);
                }
                else if (typeof(IEnumerable<bool?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<int?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<int?>)value).Select(v => v == null ? (bool?)null : Convert.ToBoolean(v)), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool>).IsAssignableFrom(sourceType))
            {
                return JToken.FromObject(((IEnumerable<bool>)value).Select(Convert.ToInt32), jsonSerializer);
            }
            else if (targetType == ParameterType.Int_IEnumerable && typeof(IEnumerable<bool?>).IsAssignableFrom(sourceType))
            {
                return JToken.FromObject(((IEnumerable<bool?>)value).Select(v => v == null ? (int?)null : Convert.ToInt32(v)), jsonSerializer);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(BoolParameterConverter));
        }

        private string GetStringValue(bool value, ParameterCollection additionalInfo)
        {
            Tuple<string,string> format = null;
            if (additionalInfo.HasKeyAndCanConvertTo("format", typeof(string)))
            {
                var str = additionalInfo.GetByKey<string>("format");
                var array = str.Split(new [] { ":::" }, StringSplitOptions.None);
                if (array.Length == 2)
                {
                    format = new Tuple<string, string>(array[0], array[1]);
                }
                else if (str == "{lower}")
                {
                    format = new Tuple<string, string>("true", "false");
                }
                else if (str == "{upper}")
                {
                    format = new Tuple<string, string>("TRUE", "FALSE");
                }
            }
            else if (additionalInfo.HasKeyAndCanConvertTo("format", typeof(ParameterCollection)))
            {
                var pc = additionalInfo.GetByKey<ParameterCollection>("format");
                if (pc.HasKeyAndCanConvertTo("true", typeof(string)) && pc.HasKeyAndCanConvertTo("false", typeof(string)))
                {
                    format = new Tuple<string, string>(pc.GetByKey<string>("true"), pc.GetByKey<string>("false"));
                }
            }
            
            if (format == null)
            {
                return value.ToString();
            }

            if (value)
            {
                return format.Item1;
            }
            else
            {
                return format.Item2;
            }
        }
    }
}

