using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
	public class AttributeParameterConverter : IParameterValueConverter
	{
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable).IsAssignableFrom(targetType))
            {
                foreach(var type in targetType.GetGenericIEnumerables())
                {
                    if (type.GetCustomAttribute<AttributeConvertibleAttribute>() != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable).IsAssignableFrom(sourceType))
            {
                foreach (var type in sourceType.GetGenericIEnumerables())
                {
                    if (type.GetCustomAttribute<AttributeConvertibleAttribute>() != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable).IsAssignableFrom(targetType))
            {
                var list = rawValue.ToObject<ParameterCollection[]>(jsonSerializer);
                foreach (var type in targetType.GetGenericIEnumerables())
                {
                    if (type.GetCustomAttribute<AttributeConvertibleAttribute>() != null)
                    {
                        var result = Array.CreateInstance(type, list.Length);
                        for (var i = 0; i < list.Length; i++)
                        {
                            result.SetValue(list[i].ToObject(type, customConverters), i);
                        }

                        if (targetType.IsArray)
                        {
                            return result;
                        }

                        Type genericListType = typeof(List<>);
                        Type concreteListType = genericListType.MakeGenericType(type);

                        return Activator.CreateInstance(concreteListType, new object[] { result });
                    }
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(AttributeParameterConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            foreach (var type in sourceType.GetGenericIEnumerables())
            {
                if (type.GetCustomAttribute<AttributeConvertibleAttribute>() != null)
                {
                    var result = new List<ParameterCollection>();
                    foreach(var item in (IEnumerable)value)
                    {
                        result.Add(ParameterCollection.FromObject(item, customConverters));
                    }
                    return JToken.FromObject(result);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(AttributeParameterConverter));
        }
    }
}

