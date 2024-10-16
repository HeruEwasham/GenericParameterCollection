using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
	public class EnumParameterConverter : IParameterValueConverter
	{
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return (typeof(Enum).IsAssignableFrom(targetType) && EnumParameterHasValue(rawValue, sourceType, jsonSerializer) &&
                ((sourceType == ParameterType.Enum && Enum.IsDefined(targetType, rawValue.ToObject<ParameterCollection>(jsonSerializer).GetByKey<string>("value"))) ||
                (sourceType == ParameterType.String && Enum.IsDefined(targetType, rawValue.ToObject<string>(jsonSerializer))) ||
                (sourceType == ParameterType.Int && Enum.IsDefined(targetType, rawValue.ToObject<int>(jsonSerializer)))))
                || (sourceType == ParameterType.Enum && (targetType == typeof(string) || (targetType == typeof(int) && EnumParameterHasValue(rawValue, sourceType, jsonSerializer)) || targetType == typeof(ParameterCollection)));
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return (typeof(Enum).IsAssignableFrom(sourceType) && (targetType == ParameterType.Enum || targetType == ParameterType.String || targetType == ParameterType.Int))
                || (targetType == ParameterType.Enum && sourceType == typeof(ParameterCollection) && ((ParameterCollection)value).HasKeyAndCanConvertTo("value", typeof(string)) && ((ParameterCollection)value).HasKeyAndCanConvertTo("type", typeof(string)) && ((ParameterCollection)value).HasKeyAndCanConvertTo("choices", typeof(List<string>))); 
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (typeof(Enum).IsAssignableFrom(targetType))
            {
                if (sourceType == ParameterType.Enum)
                {
                    return Enum.Parse(targetType, rawValue.ToObject<ParameterCollection>(jsonSerializer).GetByKey<string>("value"), true);
                }
                else if(sourceType == ParameterType.String)
                {
                    return Enum.Parse(targetType, rawValue.ToObject<string>(jsonSerializer), true);
                }
                else if (sourceType == ParameterType.Int)
                {
                    return Enum.ToObject(targetType, rawValue.ToObject<int>(jsonSerializer));
                }
            }
            else if (sourceType == ParameterType.Enum)
            {
                if (targetType == typeof(string))
                {
                    return rawValue.ToObject<ParameterCollection>(jsonSerializer).GetByKey<string>("value");
                }
                else if (targetType == typeof(int))
                {
                    var obj = rawValue.ToObject<ParameterCollection>(jsonSerializer);
                    return (int)Enum.Parse(ParameterConverterExtensions.GetTypeByName(obj.GetByKeyAndType<string>("type", ParameterType.String)), obj.GetByKeyAndType<string>("value", ParameterType.String), true);
                }
                else if (targetType == typeof(ParameterCollection))
                {
                    return rawValue.ToObject<ParameterCollection>(jsonSerializer);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(EnumParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (typeof(Enum).IsAssignableFrom(sourceType))
            {
                if (targetType == ParameterType.Enum)
                {
                    return JToken.FromObject(new ParameterCollection
                    {
                        { "value", Enum.GetName(sourceType, value), false },
                        { "type", sourceType.FullName },
                        { "choices", Enum.GetNames(sourceType) }
                    }, jsonSerializer);
                }
                else if (targetType == ParameterType.String)
                {
                    return JToken.FromObject(Enum.GetName(value.GetType(), value), jsonSerializer);
                }
                else if (targetType == ParameterType.Int)
                {
                    return JToken.FromObject((int)value, jsonSerializer);
                }
            }
            else if (targetType == ParameterType.Enum && sourceType == typeof(ParameterCollection))
            {
                return JToken.FromObject(value, jsonSerializer);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(EnumParameterConverter));
        }

        private bool EnumParameterHasValue(JToken rawValue, ParameterType sourceType, JsonSerializer jsonSerializer)
        {
            if (rawValue != null)
            {
                if (sourceType == ParameterType.Enum)
                {
                    return !string.IsNullOrEmpty(rawValue.ToObject<ParameterCollection>(jsonSerializer).GetByKey<string>("value"));
                }
                else if (sourceType == ParameterType.String)
                {
                    return rawValue.ToObject<string>(jsonSerializer) != string.Empty;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

