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

        public static ParameterCollection ToParameterCollection(this Enum enumValue)
        {
            var type = enumValue.GetType();
            return new ParameterCollection
            {
                { "enumType", type.FullName },
                { "enumValue", Enum.GetName(type, enumValue) },
                { "allPossibleValues", Enum.GetNames(type) }
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

        /// <summary>
        /// Checks if a given type is valid for the ParameterType.
        /// </summary>
        /// <param name="validType">The ParameterType to check if a type is valid against.</param>
        /// <param name="typeToCheck">The type to check if valid.</param>
        /// <returns></returns>
        public static bool IsValidType(this ParameterType validType, Type typeToCheck)
        {
            switch (validType)
            {
                case ParameterType.Int:
                    return typeToCheck == typeof(int);
                case ParameterType.String:
                    return typeToCheck == typeof(string);
                case ParameterType.String_Multiline:
                    return typeToCheck == typeof(string);
                case ParameterType.Float:
                    return typeToCheck == typeof(float);
                case ParameterType.Double:
                    return typeToCheck == typeof(double);
                case ParameterType.Long:
                    return typeToCheck == typeof(long);
                case ParameterType.Bytes:
                    return typeToCheck == typeof(byte[]);
                case ParameterType.Bool:
                    return typeToCheck == typeof(bool);
                case ParameterType.DateTime:
                    return typeToCheck == typeof(DateTime);
                case ParameterType.Date:
                    return typeToCheck == typeof(DateTime);
                case ParameterType.ParameterCollection:
                    return typeToCheck == typeof(ParameterCollection);
                case ParameterType.String_IEnumerable:
                    return typeof(IEnumerable<string>).IsAssignableFrom(typeToCheck);
                case ParameterType.String_Multiline_IEnumerable:
                    return typeof(IEnumerable<string>).IsAssignableFrom(typeToCheck);
                case ParameterType.Int_IEnumerable:
                    return typeof(IEnumerable<int>).IsAssignableFrom(typeToCheck);
                case ParameterType.Float_IEnumerable:
                    return typeof(IEnumerable<float>).IsAssignableFrom(typeToCheck);
                case ParameterType.Double_IEnumerable:
                    return typeof(IEnumerable<double>).IsAssignableFrom(typeToCheck);
                case ParameterType.Long_IEnumerable:
                    return typeof(IEnumerable<long>).IsAssignableFrom(typeToCheck);
                case ParameterType.Bool_IEnumerable:
                    return typeof(IEnumerable<bool>).IsAssignableFrom(typeToCheck);
                case ParameterType.DateTime_IEnumerable:
                    return typeof(IEnumerable<DateTime>).IsAssignableFrom(typeToCheck);
                case ParameterType.Date_IEnumerable:
                    return typeof(IEnumerable<DateTime>).IsAssignableFrom(typeToCheck);
                case ParameterType.ParameterCollection_IEnumerable:
                    return typeof(IEnumerable<ParameterCollection>).IsAssignableFrom(typeToCheck);
                case ParameterType.Enum:
                    return typeof(Enum).IsAssignableFrom(typeToCheck);
                case ParameterType.SelectOne:
                    return typeToCheck == typeof(string);
                case ParameterType.SelectMany:
                    return typeof(IEnumerable<string>).IsAssignableFrom(typeToCheck);
                default:
                    throw new ArgumentOutOfRangeException();
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
    }
}

