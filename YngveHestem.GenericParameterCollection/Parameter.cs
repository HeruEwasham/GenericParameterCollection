using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

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

        [JsonProperty("customConverters")]
        private List<IParameterValueConverter> _customParameterValueConverters { get; set; }

        private static JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault(ParameterConverterExtensions.GetJsonSerializerSettings());

        private static IParameterValueConverter[] _defaultParameterValueConverters = new IParameterValueConverter[]
        {
            new IntParameterConverter(),
            new StringParameterConverter(),
            new FloatParameterConverter(),
            new DoubleParameterConverter(),
            new LongParameterConverter(),
            new BoolParameterConverter(),
            new BytesParameterConverter(),
            new DateTimeParameterConverter(),
            new ParameterCollectionParameterConverterForParameterCollection(),
            new EnumParameterConverter(),
            new SelectParameterConverter()
        };

        /// <summary>
        /// Save a new parameter with the given values. The value are converted to the raw JToken that is stored.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as a JToken.</param>
        /// <param name="type">The type this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        [JsonConstructor]
        private Parameter(string key, JToken value, ParameterType type, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            Key = key;
            _value = value;
            Type = type;
            _additionalInfo = additionalInfo;
            if (customConverters != null)
            {
                _customParameterValueConverters = customConverters.ToList();
            }
        }

        /// <summary>
        /// Save a new parameter with a specified ParameterType as a ParameterCollection. This is just for internal use when new types are created.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as a ParameterCollection.</param>
        /// <param name="type">The type to tell this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        private Parameter(string key, ParameterCollection value, ParameterType type, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters) : this(key, JToken.FromObject(value, _jsonSerializer), type, additionalInfo, customConverters) { }

        /// <summary>
        /// Save a new parameter with a specified ParameterType as an ienumerable of ParameterCollection. This is just for internal use when new types are created.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as an ienumerable of ParameterCollection.</param>
        /// <param name="type">The type to tell this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        private Parameter(string key, IEnumerable<ParameterCollection> value, ParameterType type, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters) : this(key, JToken.FromObject(value, _jsonSerializer), type, additionalInfo, customConverters) { }

        /// <summary>
        /// Save a new parameter with a specified ParameterType as an ienumerable of strings. This is just for internal use when new types are created.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value as an ienumerable of strings.</param>
        /// <param name="type">The type to tell this parameter is.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        private Parameter(string key, IEnumerable<string> value, ParameterType type, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters) : this(key, JToken.FromObject(value, _jsonSerializer), type, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given string value.</param>
        /// <param name="multiline">Is the string meant to be multiline or should it only be a one-liner.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, string value, bool multiline = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, multiline ? ParameterType.String_Multiline : ParameterType.String, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, int value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.Int, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, float value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.Float, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, long value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.Long, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, double value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.Double, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only wants the content of an image or video files. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, byte[] value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, Convert.ToBase64String(value), ParameterType.Bytes, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, bool value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.Bool, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, DateTime value, bool onlyDate = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, onlyDate ? ParameterType.Date : ParameterType.DateTime, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="multiline">Is the strings meant to be multiline or should it only be one-liners.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<string> value, bool multiline = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, multiline ? ParameterType.String_Multiline_IEnumerable : ParameterType.String_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<int> value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, JToken.FromObject(value), ParameterType.Int_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<float> value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, JToken.FromObject(value), ParameterType.Float_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<double> value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, JToken.FromObject(value), ParameterType.Double_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<long> value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, JToken.FromObject(value), ParameterType.Long_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<bool> value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, JToken.FromObject(value), ParameterType.Bool_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<DateTime> value, bool onlyDate = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, JToken.FromObject(value), onlyDate ? ParameterType.Date_IEnumerable : ParameterType.DateTime_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, ParameterCollection value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.ParameterCollection, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, ParameterType.ParameterCollection_IEnumerable, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, Enum value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value.ToParameterCollection(), ParameterType.Enum, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter where you can choose one value between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value. This value must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, string value, IEnumerable<string> choices, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, ParameterConverterExtensions.SelectOneToParameterCollection(value, choices), ParameterType.SelectOne, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter where you can choose one or more values between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value(s). Each string must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        public Parameter(string key, IEnumerable<string> value, IEnumerable<string> choices, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, ParameterConverterExtensions.SelectManyToParameterCollection(value, choices), ParameterType.SelectMany, additionalInfo, customConverters) { }

        /// <summary>
        /// Create a new parameter where you pass an object and the parameter-type you want it saved as. Mark that it needs to be a converter that supports the conversion, so if one of the default converters do not support the conversion, one or more converters that support the conversion must be added.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="parameterType">The wanted type the parameter shall save/consider it as.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here can custom converters needed to convert value (if default converters don't support it, or you want it saved differently). More converters can also be added later.</param>
        public Parameter(string key, object value, ParameterType parameterType, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            if (customConverters != null)
            {
                _customParameterValueConverters = customConverters.ToList();
            }

            var converter = GetSuitableConverterFromValue(value, parameterType);

            Key = key;
            _value = converter.ConvertFromValue(parameterType, value.GetType(), value, _jsonSerializer);
            Type = parameterType;
            _additionalInfo = additionalInfo;
        }

        /// <summary>
        /// Create a new parameter where you pass an object. How the parameter should be converted to one of the parameter-types is decided automatically based on the values type. Mark that it needs to be a converter that supports the conversion, so if one of the default converters do not support the conversion, one or more converters that support the conversion must be added.
        /// </summary>
        /// <param name="key">he given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here can custom converters needed to convert value (if default converters don't support it, or you want it saved differently). More converters can also be added later.</param>
        public Parameter(string key, object value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null) : this(key, value, GetBestSuitableParameterType(value, customConverters), additionalInfo, customConverters) { }

        /// <summary>
        /// Adds a new custom converter that could be used by parameter.
        /// </summary>
        /// <param name="parameterValueConverter">The converter to add.</param>
        public void AddCustomConverter(IParameterValueConverter parameterValueConverter)
        {
            if (_customParameterValueConverters == null)
            {
                _customParameterValueConverters = new List<IParameterValueConverter>();
            }
            _customParameterValueConverters.Add(parameterValueConverter);
        }

        /// <summary>
        /// Adds one or more custom converters that could be used by parameter.
        /// </summary>
        /// <param name="parameterValueConverters">The converter(s) to add.</param>
        public void AddCustomConverter(IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            foreach(var converter in parameterValueConverters)
            {
                AddCustomConverter(converter);
            }
        }

        /// <summary>
        /// Gets all the custom converters added to the given parameter.
        /// </summary>
        /// <returns></returns>
        public List<IParameterValueConverter> GetCustomConverters()
        {
            return _customParameterValueConverters;
        }

        private static ParameterType GetBestSuitableParameterType(object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            var valueType = value.GetType();
            if (valueType == typeof(string))
            {
                if (((string)value).Contains('\n'))
                {
                    return ParameterType.String_Multiline;
                }
                else
                {
                    return ParameterType.String;
                }
            }
            else if (valueType == typeof(int))
            {
                return ParameterType.Int;
            }
            else if (valueType == typeof(float))
            {
                return ParameterType.Float;
            }
            else if (valueType == typeof(double))
            {
                return ParameterType.Double;
            }
            else if (valueType == typeof(long))
            {
                return ParameterType.Long;
            }
            else if (valueType == typeof(byte[]))
            {
                return ParameterType.Bytes;
            }
            else if (valueType == typeof(bool))
            {
                return ParameterType.Bool;
            }
            else if (valueType == typeof(DateTime))
            {
                if (((DateTime)value).TimeOfDay == TimeSpan.Zero)
                {
                    return ParameterType.Date;
                }
                else
                {
                    return ParameterType.DateTime;
                }
            }
            else if (valueType == typeof(ParameterCollection))
            {
                return ParameterType.ParameterCollection;
            }
            else if (typeof(IEnumerable<string>).IsAssignableFrom(valueType))
            {
                if (((IEnumerable<string>)value).Any(s => s.Contains('\n')))
                {
                    return ParameterType.String_Multiline_IEnumerable;
                }
                else
                {
                    return ParameterType.String_IEnumerable;
                }
            }
            else if (typeof(IEnumerable<int>).IsAssignableFrom(valueType))
            {
                return ParameterType.Int_IEnumerable;
            }
            else if (typeof(IEnumerable<float>).IsAssignableFrom(valueType))
            {
                return ParameterType.Float_IEnumerable;
            }
            else if (typeof(IEnumerable<double>).IsAssignableFrom(valueType))
            {
                return ParameterType.Double_IEnumerable;
            }
            else if (typeof(IEnumerable<long>).IsAssignableFrom(valueType))
            {
                return ParameterType.Long_IEnumerable;
            }
            else if (typeof(IEnumerable<bool>).IsAssignableFrom(valueType))
            {
                return ParameterType.Bool_IEnumerable;
            }
            else if (typeof(IEnumerable<DateTime>).IsAssignableFrom(valueType))
            {
                if (((IEnumerable<DateTime>)value).All(v => v.TimeOfDay == TimeSpan.Zero))
                {
                    return ParameterType.Date_IEnumerable;
                }
                else
                {
                    return ParameterType.DateTime_IEnumerable;
                }
            }
            else if (typeof(IEnumerable<ParameterCollection>).IsAssignableFrom(valueType))
            {
                return ParameterType.ParameterCollection_IEnumerable;
            }
            else if (typeof(Enum).IsAssignableFrom(valueType))
            {
                return ParameterType.Enum;
            }
            else if (customConverters != null)
            {
                foreach (var type in (IEnumerable<ParameterType>)Enum.GetValues(typeof(ParameterType)))
                {
                    var converter = customConverters.FirstOrDefault(c => c.CanConvertFromValue(type, valueType, value));
                    if (converter != null)
                    {
                        return type;
                    }
                }
            }

            throw new ArgumentException("Did not find any suitable ParameterType for the valuetype " + valueType);
        }

        private IParameterValueConverter GetSuitableConverterFromValue(object value, ParameterType parameterType)
        {
            var valueType = value.GetType();
            var converter = _customParameterValueConverters != null ? _customParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(Type, valueType, value)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = _defaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + valueType.Name + " to parameter type " + parameterType + " was not found.");
        }

        /// <summary>
        /// Gets the value converted to correct value as object
        /// </summary>
        /// <param name="typeToGet">The specified type to get.</param>
        /// <returns></returns>
        public object GetValue(Type typeToGet)
        {
            try
            {
                return GetSuitableConverterToValue(typeToGet).ConvertFromParameter(Type, typeToGet, _value, _jsonSerializer);
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        private IParameterValueConverter GetSuitableConverterToValue(Type typeToGet)
        {
            var converter = _customParameterValueConverters != null ? _customParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, typeToGet, _value, _jsonSerializer)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = _defaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, typeToGet, _value, _jsonSerializer));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + typeToGet.Name + " from parameter type " + Type + " was not found.");
        }

        /// <summary>
        /// Tries to convert the value to correct type and return it as this type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        /// <summary>
        /// If this is an enum or one of the Select-parameter-types (SelectOne or SelectMany), this will return the possible values that are available to choose from.
        /// </summary>
        /// <returns>The possible values of the parameter.</returns>
        /// <exception cref="NotSupportedException">If this is called for a Parameter with a ParameterType that not support this method. This Exception will be thrown.</exception>
        public IEnumerable<string> GetChoices()
        {
            var obj = _value.ToObject<ParameterCollection>(_jsonSerializer);
            if (obj.HasKeyAndCanConvertTo("choices", typeof(IEnumerable<string>)))
            {
                return obj.GetByKey<IEnumerable<string>>("choices");
            }

            throw new NotSupportedException("The method " + nameof(GetChoices) + " is currently not supported with the parameter type " + Enum.GetName(typeof(ParameterType), Type) + ". This currently only supports parameters with the option \"choices\". " + ParameterType.Enum.ToString() + ", " + ParameterType.SelectOne.ToString() + " and " + ParameterType.SelectMany.ToString() + " and possibly some ParameterCollections are supported. Other parameters has not an option for this.");
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


        public bool SetValue(object newValue)
        {
            var valueType = newValue.GetType();
            try
            {
                if (Type == ParameterType.Enum && valueType == typeof(string))
                {
                    var v = _value.ToObject<ParameterCollection>(_jsonSerializer);
                    if (v.GetByKeyAndType<List<string>>("choices", ParameterType.String_IEnumerable).Contains((string)newValue))
                    {
                        if (v.GetParameterByKeyAndType("value", ParameterType.String).SetValue(newValue))
                        {
                            _value = JToken.FromObject(v, _jsonSerializer);
                            return true;
                        }
                    }
                    return false;
                }
                else if (Type == ParameterType.SelectOne && valueType == typeof(string))
                {
                    var va = _value.ToObject<ParameterCollection>(_jsonSerializer);
                    if (va.GetByKeyAndType<List<string>>("choices", ParameterType.String_IEnumerable).Contains((string)newValue))
                    {
                        if (va.GetParameterByKeyAndType("value", ParameterType.String).SetValue(newValue))
                        {
                            _value = JToken.FromObject(va, _jsonSerializer);
                            return true;
                        }
                    }
                    return false;
                }
                else if (Type == ParameterType.SelectMany && typeof(IEnumerable<string>).IsAssignableFrom(valueType))
                {
                    var va = _value.ToObject<ParameterCollection>(_jsonSerializer);
                    if (va.GetByKeyAndType<List<string>>("choices", ParameterType.String_IEnumerable).Contains((string)newValue))
                    {
                        if (va.GetParameterByKeyAndType("value", ParameterType.String_IEnumerable).SetValue(newValue))
                        {
                            _value = JToken.FromObject(va, _jsonSerializer);
                            return true;
                        }
                    }
                    return false;
                }

                try
                {
                    _value = GetSuitableConverterFromValue(newValue, Type).ConvertFromValue(Type, valueType, newValue, _jsonSerializer);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
                
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when setting a new parameter value. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        public override string ToString()
        {
            return "Parameter has these values:" + Environment.NewLine +
                "\tThe key is: " + Key + Environment.NewLine +
                "\tThe type to convert to is: " + Enum.GetName(typeof(ParameterType), Type) + Environment.NewLine +
                "\tThe value to convert from is: " + _value.ToString();
        }

        /// <summary>
        /// Based on the converters, can we convert it to given type.
        /// </summary>
        /// <param name="type">The type we want to convert to.</param>
        /// <returns></returns>
        public bool CanBeConvertedTo(Type type)
        {
            if (_customParameterValueConverters != null)
            {
                if (_customParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, type, _value, _jsonSerializer)) != null)
                {
                    return true;
                }
            }

            if (_defaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, type, _value, _jsonSerializer)) != null)
            {
                return true;
            }

            return false;
        }
    }
}

