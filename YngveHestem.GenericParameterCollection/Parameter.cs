using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection
{
    public class Parameter
    {
        [JsonProperty("key")]
        public string Key { get; private set; }

        [JsonProperty("type")]
        public ParameterType Type { get; private set; }

        [JsonProperty("value")]
        private JToken _value { get; set; }

        [JsonProperty("info")]
        private ParameterCollection _additionalInfo { get; set; }

        private static JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault(ParameterConverterExtensions.GetJsonSerializerSettings());

        /// <summary>
        /// Save a new parameter with the given values. The value are converted to the raw JToken that is stored.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as a JToken.</param>
        /// <param name="type">The type this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        [JsonConstructor]
        private Parameter(string key, JToken value, ParameterType type, ParameterCollection additionalInfo)
        {
            Key = key;
            _value = value;
            Type = type;
            _additionalInfo = additionalInfo;
        }

        /// <summary>
        /// Save a new parameter with a specified ParameterType as a ParameterCollection. This is just for internal use when new types are created.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as a ParameterCollection.</param>
        /// <param name="type">The type to tell this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        private Parameter(string key, ParameterCollection value, ParameterType type, ParameterCollection additionalInfo) : this(key, JToken.FromObject(value, _jsonSerializer), type, additionalInfo) { }

        /// <summary>
        /// Save a new parameter with a specified ParameterType as an ienumerable of ParameterCollection. This is just for internal use when new types are created.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as an ienumerable of ParameterCollection.</param>
        /// <param name="type">The type to tell this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        private Parameter(string key, IEnumerable<ParameterCollection> value, ParameterType type, ParameterCollection additionalInfo) : this(key, JToken.FromObject(value, _jsonSerializer), type, additionalInfo) { }

        /// <summary>
        /// Save a new parameter with a specified ParameterType as an ienumerable of strings. This is just for internal use when new types are created.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as an ienumerable of strings.</param>
        /// <param name="type">The type to tell this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        private Parameter(string key, IEnumerable<string> value, ParameterType type, ParameterCollection additionalInfo) : this(key, JToken.FromObject(value, _jsonSerializer), type, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given string value.</param>
        /// <param name="multiline">Is the string meant to be multiline or should it only be a one-liner.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, string value, bool multiline = false, ParameterCollection additionalInfo = null) : this(key, value, multiline ? ParameterType.String_Multiline : ParameterType.String, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, int value, ParameterCollection additionalInfo = null) : this(key, value.ToString(), ParameterType.Int, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, float value, ParameterCollection additionalInfo = null) : this(key, value, ParameterType.Float, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, long value, ParameterCollection additionalInfo = null) : this(key, value, ParameterType.Long, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, double value, ParameterCollection additionalInfo = null) : this(key, value, ParameterType.Double, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only wants the content of an image or video files. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, byte[] value, ParameterCollection additionalInfo = null) : this(key, Convert.ToBase64String(value), ParameterType.Bytes, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, bool value, ParameterCollection additionalInfo = null) : this(key, value, ParameterType.Bool, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, DateTime value, bool onlyDate = false, ParameterCollection additionalInfo = null) : this(key, value, onlyDate ? ParameterType.Date : ParameterType.DateTime, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="multiline">Is the strings meant to be multiline or should it only be one-liners.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<string> value, bool multiline = false, ParameterCollection additionalInfo = null) : this(key, value, multiline ? ParameterType.String_Multiline_IEnumerable : ParameterType.String_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<int> value, ParameterCollection additionalInfo = null) : this(key, JToken.FromObject(value), ParameterType.Int_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<float> value, ParameterCollection additionalInfo = null) : this(key, JToken.FromObject(value), ParameterType.Float_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<double> value, ParameterCollection additionalInfo = null) : this(key, JToken.FromObject(value), ParameterType.Double_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<long> value, ParameterCollection additionalInfo = null) : this(key, JToken.FromObject(value), ParameterType.Long_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<bool> value, ParameterCollection additionalInfo = null) : this(key, JToken.FromObject(value), ParameterType.Bool_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<DateTime> value, bool onlyDate = false, ParameterCollection additionalInfo = null) : this(key, JToken.FromObject(value), onlyDate ? ParameterType.Date_IEnumerable : ParameterType.DateTime_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, ParameterCollection value, ParameterCollection additionalInfo = null) : this(key, value, ParameterType.ParameterCollection, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo = null) : this(key, value, ParameterType.ParameterCollection_IEnumerable, additionalInfo) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, Enum value, ParameterCollection additionalInfo = null) : this(key, value.ToParameterCollection(), ParameterType.Enum, additionalInfo) { }

        /// <summary>
        /// Create a new parameter where you can choose one value between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value. This value must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, string value, IEnumerable<string> choices, ParameterCollection additionalInfo = null) : this(key, ParameterConverterExtensions.SelectOneToParameterCollection(value, choices), ParameterType.SelectOne, additionalInfo) { }

        /// <summary>
        /// Create a new parameter where you can choose one or more values between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value(s). Each string must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<string> value, IEnumerable<string> choices, ParameterCollection additionalInfo = null) : this(key, ParameterConverterExtensions.SelectManyToParameterCollection(value, choices), ParameterType.SelectMany, additionalInfo) { }

        /// <summary>
        /// Gets the value converted to correct value as object
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            try
            {
                switch (Type)
                {
                    case ParameterType.Int:
                        return _value.ToObject<int>(_jsonSerializer);
                    case ParameterType.String:
                        return _value.ToObject<string>(_jsonSerializer);
                    case ParameterType.String_Multiline:
                        return _value.ToObject<string>(_jsonSerializer);
                    case ParameterType.Float:
                        return _value.ToObject<float>(_jsonSerializer);
                    case ParameterType.Double:
                        return _value.ToObject<double>(_jsonSerializer);
                    case ParameterType.Long:
                        return _value.ToObject<long>(_jsonSerializer);
                    case ParameterType.Bytes:
                        return Convert.FromBase64String(_value.ToObject<string>(_jsonSerializer));
                    case ParameterType.Bool:
                        return _value.ToObject<bool>();
                    case ParameterType.DateTime:
                        return _value.ToObject<DateTime>();
                    case ParameterType.Date:
                        return _value.ToObject<DateTime>();
                    case ParameterType.ParameterCollection:
                        return _value.ToObject<ParameterCollection>(_jsonSerializer);
                    case ParameterType.String_IEnumerable:
                        return _value.ToObject<IEnumerable<string>>(_jsonSerializer);
                    case ParameterType.String_Multiline_IEnumerable:
                        return _value.ToObject<IEnumerable<string>>(_jsonSerializer);
                    case ParameterType.Int_IEnumerable:
                        return _value.ToObject<IEnumerable<int>>(_jsonSerializer);
                    case ParameterType.Float_IEnumerable:
                        return _value.ToObject<IEnumerable<float>>(_jsonSerializer);
                    case ParameterType.Double_IEnumerable:
                        return _value.ToObject<IEnumerable<double>>(_jsonSerializer);
                    case ParameterType.Long_IEnumerable:
                        return _value.ToObject<IEnumerable<long>>(_jsonSerializer);
                    case ParameterType.Bool_IEnumerable:
                        return _value.ToObject<IEnumerable<bool>>(_jsonSerializer);
                    case ParameterType.DateTime_IEnumerable:
                        return _value.ToObject<IEnumerable<DateTime>>(_jsonSerializer);
                    case ParameterType.Date_IEnumerable:
                        return _value.ToObject<IEnumerable<DateTime>>(_jsonSerializer);
                    case ParameterType.ParameterCollection_IEnumerable:
                        return _value.ToObject<IEnumerable<ParameterCollection>>(_jsonSerializer);
                    case ParameterType.Enum:
                        var v = _value.ToObject<ParameterCollection>(_jsonSerializer);
                        return Enum.Parse(ParameterConverterExtensions.GetTypeByName(v.GetByKeyAndType<string>("enumType", ParameterType.String)), v.GetByKeyAndType<string>("enumValue", ParameterType.String), true);
                    case ParameterType.SelectOne:
                        return _value.ToObject<ParameterCollection>(_jsonSerializer).GetByKeyAndType<string>("value", ParameterType.String);
                    case ParameterType.SelectMany:
                        return _value.ToObject<ParameterCollection>(_jsonSerializer).GetByKeyAndType<string>("value", ParameterType.String);
                    default:
                        throw new ArgumentOutOfRangeException();
                };
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value. " + Environment.NewLine + ToString(), e);
            }
        }

        /// <summary>
        /// Tries to convert the value to correct type and return it as this type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            return (T)GetValue();
        }

        /// <summary>
        /// If this is an enum or one of the Select-parameter-types (SelectOne or SelectMany), this will return the possible values that are available to choose from.
        /// </summary>
        /// <returns>The possible values of the parameter.</returns>
        /// <exception cref="NotSupportedException">If this is called for a Parameter with a ParameterType that not support this method. This Exception will be thrown.</exception>
        public IEnumerable<string> GetChoices()
        {
            if (Type == ParameterType.Enum)
            {
                return _value.ToObject<ParameterCollection>(_jsonSerializer).GetByKeyAndType<IEnumerable<string>>("allPossibleValues", ParameterType.String_IEnumerable);
            }
            else if (Type == ParameterType.SelectOne || Type == ParameterType.SelectMany)
            {
                return _value.ToObject<ParameterCollection>(_jsonSerializer).GetByKeyAndType<IEnumerable<string>>("choices", ParameterType.String_IEnumerable);
            }
            else
            {
                throw new NotSupportedException("The method " + nameof(GetChoices) + " is currently not supported with the parameter type " + Enum.GetName(typeof(ParameterType), Type) + ". This currently only supports " + ParameterType.Enum.ToString() + ", " + ParameterType.SelectOne.ToString() + " and " + ParameterType.SelectMany.ToString() + ". Other parameters has not an option for this.");
            }
        }

        /// <summary>
        /// Get if parameter has additional information or not attached to it.
        /// </summary>
        /// <returns></returns>
        public bool HasAdditionalInfo()
        {
            return _additionalInfo != null;
        }

        /// <summary>
        /// Get additional information attached to parameter.
        /// </summary>
        /// <returns></returns>
        public ParameterCollection GetAdditionalInfo()
        {
            return _additionalInfo;
        }

        public override string ToString()
        {
            return "Parameter has these values:" + Environment.NewLine +
                "\tThe key is: " + Key + Environment.NewLine +
                "\tThe type to convert to is: " + Enum.GetName(typeof(ParameterType), Type) + Environment.NewLine +
                "\tThe value to convert from is: " + _value.ToString();
        }
    }
}

