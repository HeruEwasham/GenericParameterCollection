using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public class DateTimeParameterConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.DateTime || sourceType == ParameterType.Date)
            {
                if (rawValue == null)
                {
                    return false;
                }
                return targetType == typeof(DateTime) || targetType == typeof(string) || typeof(IEnumerable<DateTime>).IsAssignableFrom(targetType);
            }
            else if (sourceType == ParameterType.DateTime_IEnumerable || sourceType == ParameterType.Date_IEnumerable)
            {
                return typeof(IEnumerable<DateTime>).IsAssignableFrom(targetType) || typeof(IEnumerable<string>).IsAssignableFrom(targetType) || typeof(IEnumerable<DateTime?>).IsAssignableFrom(targetType);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType == ParameterType.DateTime || targetType == ParameterType.Date)
            {
                return sourceType == typeof(DateTime) || (sourceType == typeof(string) && DateTime.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.DateTime_IEnumerable || targetType == ParameterType.Date_IEnumerable)
            {
                return typeof(IEnumerable<DateTime>).IsAssignableFrom(sourceType) || (typeof(IEnumerable<string>).IsAssignableFrom(sourceType) && ((IEnumerable<string>)value).All(v => v == null || DateTime.TryParse(v, out _))) || typeof(IEnumerable<DateTime?>).IsAssignableFrom(sourceType);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.DateTime || sourceType == ParameterType.Date)
            {
                if (targetType == typeof(DateTime))
                {
                    return rawValue.ToObject<DateTime>(jsonSerializer);
                }
                else if (targetType == typeof(string))
                {
                    var pattern = "D";
                    if (sourceType == ParameterType.DateTime)
                    {
                        pattern = "F";
                    }
                    return rawValue.ToObject<DateTime>(jsonSerializer).ToString(pattern, CultureInfo.CurrentCulture);
                }
                else if (typeof(IEnumerable<DateTime>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<DateTime>(jsonSerializer).ToIEnumerable().ToCorrectIEnumerable(targetType);
                }
            }
            else if (sourceType == ParameterType.DateTime_IEnumerable || sourceType == ParameterType.Date_IEnumerable)
            {
                if (rawValue == null)
                {
                    return null;
                }
                if (typeof(IEnumerable<DateTime>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<DateTime>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(targetType))
                {
                    var pattern = "D";
                    if (sourceType == ParameterType.DateTime_IEnumerable)
                    {
                        pattern = "F";
                    }
                    return rawValue.ToObject<IEnumerable<DateTime>>(jsonSerializer).Select(v => v.ToString(pattern, CultureInfo.CurrentCulture)).ToCorrectIEnumerable(targetType);
                }
                else if (typeof(IEnumerable<DateTime?>).IsAssignableFrom(targetType))
                {
                    return rawValue.ToObject<IEnumerable<DateTime?>>(jsonSerializer).ToCorrectIEnumerable(targetType);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DateTimeParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.DateTime || targetType == ParameterType.Date)
            {
                if (sourceType == typeof(DateTime))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(DateTime.Parse((string)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.DateTime_IEnumerable || targetType == ParameterType.Date_IEnumerable)
            {
                if (typeof(IEnumerable<DateTime>).IsAssignableFrom(sourceType) || typeof(IEnumerable<DateTime?>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(sourceType))
                {
                    return JToken.FromObject(((IEnumerable<string>)value).Select(v => DateTime.Parse(v)), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DateTimeParameterConverter));
        }
    }
}

