using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection
{
    public static class ParameterConverterExtensions
    {
        public static Type GetTypeByName(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }

        public static IEnumerable<T> ToCorrectIEnumerable<T>(this IEnumerable<T> value, Type returnType)
        {
            if (!typeof(IEnumerable<T>).IsAssignableFrom(returnType))
            {
                throw new ArgumentException("Method " + nameof(ToCorrectIEnumerable) + " can only handle values where " + nameof(returnType) + " inherits " + nameof(IEnumerable<T>) + ". Here " + nameof(T) + " is " + typeof(T) + "and " + nameof(returnType) + " is " + returnType.FullName);
            }

            if (returnType.IsArray)
            {
                return value.ToArray();
            }
            else if (typeof(IList<T>).IsAssignableFrom(returnType))
            {
                return value.ToList();
            }

            return value;
        }

        public static IEnumerable<T> ToIEnumerable<T>(this T value)
        {
            return JToken.FromObject(new T[] { value }).ToObject<IEnumerable<T>>();
        }

        public static Type GetDefaultValueType(this ParameterType type)
        {
            if (type == ParameterType.Int)
            {
                return typeof(int);
            }
            else if (type == ParameterType.String || type == ParameterType.String_Multiline)
            {
                return typeof(string);
            }
            else if (type == ParameterType.Decimal)
            {
                return typeof(decimal);
            }
            else if (type == ParameterType.Bool)
            {
                return typeof(bool);
            }
            else if (type == ParameterType.Bytes)
            {
                return typeof(byte[]);
            }
            else if (type == ParameterType.Date || type == ParameterType.DateTime)
            {
                return typeof(DateTime);
            }
            else if (type == ParameterType.ParameterCollection)
            {
                return typeof(ParameterCollection);
            }
            else if (type == ParameterType.String_IEnumerable || type == ParameterType.String_Multiline_IEnumerable || type == ParameterType.SelectMany)
            {
                return typeof(IEnumerable<string>);
            }
            else if (type == ParameterType.Int_IEnumerable)
            {
                return typeof(IEnumerable<int>);
            }
            else if (type == ParameterType.Decimal_IEnumerable)
            {
                return typeof(IEnumerable<decimal>);
            }
            else if (type == ParameterType.Bool_IEnumerable)
            {
                return typeof(IEnumerable<bool>);
            }
            else if (type == ParameterType.Date_IEnumerable || type == ParameterType.DateTime_IEnumerable)
            {
                return typeof(IEnumerable<DateTime>);
            }
            else if (type == ParameterType.ParameterCollection_IEnumerable)
            {
                return typeof(IEnumerable<Parameter>);
            }
            else
            {
                return typeof(string);
            }
        }

        public static ParameterCollection ToParameterCollection(this Enum enumValue)
        {
            var type = enumValue.GetType();
            return new ParameterCollection
            {
                { "type", type.FullName },
                { "value", Enum.GetName(type, enumValue) },
                { "choices", Enum.GetNames(type) }
            };
        }

        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = GetJsonConverters()
            };
        }

        public static JsonConverter[] GetJsonConverters()
        {
            return new JsonConverter[] {
                    new StringEnumConverter()
                };
        }

        internal static ParameterCollection SelectOneToParameterCollection(string value, IEnumerable<string> choices)
        {
            return new ParameterCollection
            {
                { "value", value },
                { "choices", choices }
            };
        }

        internal static ParameterCollection SelectManyToParameterCollection(IEnumerable<string> value, IEnumerable<string> choices)
        {
            return new ParameterCollection
            {
                { "value", value },
                { "choices", choices }
            };
        }

        internal static JsonSerializer JsonSerializer = JsonSerializer.CreateDefault(ParameterConverterExtensions.GetJsonSerializerSettings());
    }
}

