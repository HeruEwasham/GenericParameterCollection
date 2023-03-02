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
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.DateTime || sourceType == ParameterType.Date)
            {
                return targetType == typeof(DateTime) || targetType == typeof(string);
            }
            else if (sourceType == ParameterType.DateTime_IEnumerable || sourceType == ParameterType.Date_IEnumerable)
            {
                return targetType == typeof(IEnumerable<DateTime>) || targetType == typeof(IEnumerable<string>);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            if (targetType == ParameterType.DateTime || targetType == ParameterType.Date)
            {
                return sourceType == typeof(DateTime) || (sourceType == typeof(string) && DateTime.TryParse((string)value, out _));
            }
            else if (targetType == ParameterType.DateTime_IEnumerable || targetType == ParameterType.Date_IEnumerable)
            {
                return sourceType == typeof(IEnumerable<DateTime>) || (sourceType == typeof(IEnumerable<string>) && ((IEnumerable<string>)value).All(v => DateTime.TryParse(v, out _)));
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
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
            }
            else if (sourceType == ParameterType.DateTime_IEnumerable || sourceType == ParameterType.Date_IEnumerable)
            {
                if (targetType == typeof(IEnumerable<DateTime>))
                {
                    return rawValue.ToObject<IEnumerable<DateTime>>(jsonSerializer).ToArray();
                }
                else if (targetType == typeof(IEnumerable<string>))
                {
                    var pattern = "D";
                    if (sourceType == ParameterType.DateTime_IEnumerable)
                    {
                        pattern = "F";
                    }
                    return rawValue.ToObject<IEnumerable<DateTime>>(jsonSerializer).Select(v => v.ToString(pattern, CultureInfo.CurrentCulture)).ToArray();
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DateTimeParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.DateTime || targetType == ParameterType.Date)
            {
                if (sourceType == typeof(DateTime))
                {
                    JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(string))
                {
                    return JToken.FromObject(DateTime.Parse((string)value), jsonSerializer);
                }
            }
            else if (targetType == ParameterType.DateTime_IEnumerable || targetType == ParameterType.Date_IEnumerable)
            {
                if (sourceType == typeof(IEnumerable<DateTime>))
                {
                    return JToken.FromObject(value, jsonSerializer);
                }
                else if (sourceType == typeof(IEnumerable<string>))
                {
                    return JToken.FromObject(((IEnumerable<string>)value).Select(v => DateTime.Parse(v)), jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(DateTimeParameterConverter));
        }
    }
}

