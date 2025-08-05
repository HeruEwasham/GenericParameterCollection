using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        private ParameterCollection _additionalInfo { get; set; }

        [JsonProperty("customConverters", NullValueHandling = NullValueHandling.Ignore)]
        private List<IParameterValueConverter> _customParameterValueConverters { get; set; }

        internal static IParameterValueConverter[] DefaultParameterValueConverters = new IParameterValueConverter[]
        {
            new IntParameterConverter(),
            new StringParameterConverter(),
            new DecimalParameterConverter(),
            new BoolParameterConverter(),
            new BytesParameterConverter(),
            new DateTimeParameterConverter(),
            new ParameterCollectionParameterConverterForParameterCollection(),
            new EnumParameterConverter(),
            new SelectParameterConverter(),
            new AttributeParameterConverter(),
            new NullableParameterConverter()
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
            if (additionalInfo == null)
            {
                additionalInfo = new ParameterCollection();
            }
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
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given string value.</param>
        /// <param name="multiline">Is the string meant to be multiline or should it only be a one-liner.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, string value, bool multiline, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, multiline ? ParameterType.String_Multiline : ParameterType.String, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, DateTime value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, onlyDate ? ParameterType.Date : ParameterType.DateTime, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, DateTime? value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, onlyDate ? ParameterType.Date : ParameterType.DateTime, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="multiline">Is the strings meant to be multiline or should it only be one-liners.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, IEnumerable<string> value, bool multiline, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, multiline ? ParameterType.String_Multiline_IEnumerable : ParameterType.String_IEnumerable, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, IEnumerable<DateTime> value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, onlyDate ? ParameterType.Date_IEnumerable : ParameterType.DateTime_IEnumerable, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, IEnumerable<DateTime?> value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, onlyDate ? ParameterType.Date_IEnumerable : ParameterType.DateTime_IEnumerable, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter where you can choose one value between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value. This value must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, string value, IEnumerable<string> choices, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, ParameterConverterExtensions.SelectOneToParameterCollection(value, choices), ParameterType.SelectOne, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter where you can choose one or more values between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value(s). Each string must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, IEnumerable<string> value, IEnumerable<string> choices, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, ParameterConverterExtensions.SelectManyToParameterCollection(value, choices), ParameterType.SelectMany, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter where you pass an object and the parameter-type you want it saved as. Mark that it needs to be a converter that supports the conversion, so if one of the default converters do not support the conversion, one or more converters that support the conversion must be added.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="parameterType">The wanted type the parameter shall save/consider it as.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConvertersToSave">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will be saved to the parameter. More converters can also be added later.</param>
        /// <param name="customConvertersToOnlyUseNow">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter.</param>
        public Parameter(string key, object value, ParameterType parameterType, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null)
        {
            if (value != null)
            {
                InitParameter(key, value, value.GetType(), parameterType, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow);
            }
            else
            {
                if (additionalInfo == null)
                {
                    additionalInfo = new ParameterCollection();
                }
                _value = null;
                Key = key;
                Type = parameterType;
                _additionalInfo = additionalInfo;
            }
        }

        /// <summary>
        /// Create a new parameter where you pass an object. How the parameter should be converted to one of the parameter-types is decided automatically based on the values type. Mark that it needs to be a converter that supports the conversion, so if one of the default converters do not support the conversion, one or more converters that support the conversion must be added.
        /// </summary>
        /// <param name="key">he given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here can custom converters needed to convert value (if default converters don't support it, or you want it saved differently). More converters can also be added later.</param>
        public Parameter(string key, object value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null) : this(key, value, GetBestSuitableParameterType(value, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow), additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow) { }

        /// <summary>
        /// Create a new parameter where you pass an object. How the parameter should be converted to one of the parameter-types is decided automatically based on the values type. Mark that it needs to be a converter that supports the conversion, so if one of the default converters do not support the conversion, one or more converters that support the conversion must be added.
        /// </summary>
        /// <param name="key">he given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="valueType">Specify the type of the value given.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here can custom converters needed to convert value (if default converters don't support it, or you want it saved differently). More converters can also be added later.</param>
        public Parameter(string key, object value, Type valueType, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConvertersToSave = null, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow = null)
        {
            InitParameter(key, value, valueType, GetBestSuitableParameterType(value, valueType, additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow), additionalInfo, customConvertersToSave, customConvertersToOnlyUseNow);
        }

        private void InitParameter(string key, object value, Type valueType, ParameterType parameterType, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConvertersToSave, IEnumerable<IParameterValueConverter> customConvertersToOnlyUseNow)
        {
            if (customConvertersToSave != null)
            {
                _customParameterValueConverters = customConvertersToSave.ToList();
            }

            if (additionalInfo == null)
            {
                additionalInfo = new ParameterCollection();
            }

            // Check if can convert via attributes
            var allCustomConverters = customConvertersToOnlyUseNow.ConcatWithNullCheck(_customParameterValueConverters);
            valueType.GetCustomAttributes<AdditionalInfoAttribute>().GetAdditionalInfoFromAttributes(ref additionalInfo, customConvertersToOnlyUseNow);
            var acAttribute = valueType.GetCustomAttribute<AttributeConvertibleAttribute>();
            var valueSet = false;
            if (acAttribute != null)
            {
                if (parameterType == ParameterType.ParameterCollection)
                {
                    _value = JToken.FromObject(valueType.GetParameterCollectionFromAttributes(value, customConvertersToOnlyUseNow), ParameterConverterExtensions.JsonSerializer);
                    valueSet = true;
                }
            }


            // Use converters if value not already set.
            if (!valueSet)
            {
                var converter = GetSuitableConverterFromValue(value, valueType, parameterType, additionalInfo, customConvertersToOnlyUseNow);
                _value = converter.ConvertFromValue(parameterType, valueType, value, additionalInfo, allCustomConverters, ParameterConverterExtensions.JsonSerializer);
            }

            Key = key;
            Type = parameterType;
            _additionalInfo = additionalInfo;
        }

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
            foreach (var converter in parameterValueConverters)
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

        /// <summary>
        /// Gets the value converted to correct value as object.
        /// </summary>
        /// <param name="typeToGet">The specified type to get.</param>
        /// <returns></returns>
        public object GetValue(Type typeToGet)
        {
            try
            {
                var acAttribute = typeToGet.GetCustomAttribute<AttributeConvertibleAttribute>();
                if (acAttribute != null)
                {
                    return typeToGet.GetObjectFromAttributes(_value, acAttribute, null);
                }
                return GetSuitableConverterToValue(typeToGet, _customParameterValueConverters).ConvertFromParameter(Type, typeToGet, _value, _additionalInfo, _customParameterValueConverters, ParameterConverterExtensions.JsonSerializer);
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        /// <summary>
        /// Gets the value converted to correct value as object. Here you can provide your own converters to check first.
        /// </summary>
        /// <param name="typeToGet">The specified type to get.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <returns></returns>
        public object GetValue(Type typeToGet, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            try
            {
                var acAttribute = typeToGet.GetCustomAttribute<AttributeConvertibleAttribute>();
                if (acAttribute != null)
                {
                    return typeToGet.GetObjectFromAttributes(_value, acAttribute, parameterValueConverters);
                }
                return GetSuitableConverterToValue(typeToGet, parameterValueConverters).ConvertFromParameter(Type, typeToGet, _value, _additionalInfo, parameterValueConverters.ConcatWithNullCheck(_customParameterValueConverters), ParameterConverterExtensions.JsonSerializer);
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value from one of the provided converters. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
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
        /// Tries to convert the value to correct type and return it as this type. Here you can provide your own converters to check first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <returns></returns>
        public T GetValue<T>(IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            return (T)GetValue(typeof(T), parameterValueConverters);
        }

        /// <summary>
        /// If this is an enum or one of the Select-parameter-types (SelectOne or SelectMany), this will return the possible values that are available to choose from.
        /// </summary>
        /// <returns>The possible values of the parameter.</returns>
        /// <exception cref="NotSupportedException">If this is called for a Parameter with a ParameterType that not support this method. This Exception will be thrown.</exception>
        public IEnumerable<string> GetChoices()
        {
            if (_value == null)
            {
                return Array.Empty<string>();
            }
            var obj = _value.ToObject<ParameterCollection>(ParameterConverterExtensions.JsonSerializer);
            if (obj.HasKeyAndCanConvertTo("choices", typeof(IEnumerable<string>)))
            {
                return obj.GetByKey<IEnumerable<string>>("choices");
            }

            throw new NotSupportedException("The method " + nameof(GetChoices) + " is currently not supported with the parameter type " + Enum.GetName(typeof(ParameterType), Type) + ". This currently only supports parameters with the option \"choices\". " + ParameterType.Enum.ToString() + ", " + ParameterType.SelectOne.ToString() + " and " + ParameterType.SelectMany.ToString() + " and possibly some ParameterCollections are supported. Other parameters has not an option for this.");
        }

        /// <summary>
        /// Get if the parameter's current value is not null.
        /// </summary>
        /// <returns></returns>
        public bool HasValue()
        {
            return _value != null && _value.Type != JTokenType.Null;
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

        /// <summary>
        /// Sets a new value for this parameter.
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool SetValue(object newValue)
        {
            if (newValue == null)
            {
                if (Type == ParameterType.Enum || Type == ParameterType.SelectOne)
                {
                    newValue = string.Empty;
                }
                else if (Type == ParameterType.SelectMany)
                {
                    newValue = Array.Empty<string>();
                }
                else
                {
                    _value = null;
                    return true;
                }
            }

            var valueType = newValue.GetType();
            try
            {
                var acAttribute = valueType.GetCustomAttribute<AttributeConvertibleAttribute>();
                if (acAttribute != null)
                {
                    if (Type == ParameterType.ParameterCollection)
                    {
                        _value = JToken.FromObject(valueType.GetParameterCollectionFromAttributes(newValue, null), ParameterConverterExtensions.JsonSerializer);
                        return true;
                    }
                }

                if (SetValueCustomConverters(newValue, valueType))
                {
                    return true;
                }

                try
                {
                    _value = GetSuitableConverterFromValue(newValue, valueType, Type, _additionalInfo, null).ConvertFromValue(Type, valueType, newValue, _additionalInfo, _customParameterValueConverters, ParameterConverterExtensions.JsonSerializer);
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

        /// <summary>
        /// Sets a new value for this parameter.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="parameterValueConverters"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool SetValue(object newValue, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            if (newValue == null)
            {
                if (Type == ParameterType.Enum || Type == ParameterType.SelectOne)
                {
                    newValue = string.Empty;
                }
                else if (Type == ParameterType.SelectMany)
                {
                    newValue = Array.Empty<string>();
                }
                else
                {
                    _value = null;
                    return true;
                }
            }

            try
            {
                var valueType = newValue.GetType();
                var acAttribute = valueType.GetCustomAttribute<AttributeConvertibleAttribute>();
                if (acAttribute != null)
                {
                    if (Type == ParameterType.ParameterCollection)
                    {
                        _value = JToken.FromObject(valueType.GetParameterCollectionFromAttributes(newValue, parameterValueConverters), ParameterConverterExtensions.JsonSerializer);
                        return true;
                    }
                }
                if (SetValueCustomConverters(newValue, valueType))
                {
                    return true;
                }

                try
                {
                    _value = GetSuitableConverterFromValue(newValue, valueType, Type, _additionalInfo, parameterValueConverters).ConvertFromValue(Type, valueType, newValue, _additionalInfo, parameterValueConverters.ConcatWithNullCheck(_customParameterValueConverters), ParameterConverterExtensions.JsonSerializer);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                throw new Exception("Got exception when setting a new parameter value based on inputted converter. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        /// <summary>
        /// Sets the given ParameterCollection as the new additionalInfo-parameters.
        /// </summary>
        /// <param name="additionalInfo">The new additionalInfo-object.</param>
        public void SetAdditionalInfo(ParameterCollection additionalInfo)
        {
            _additionalInfo = additionalInfo;
        }

        /// <summary>
        /// Sets both a new value for this parameter and updates the additionalInfo.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public bool SetValue(object newValue, ParameterCollection additionalInfo)
        {
            if (SetValue(newValue))
            {
                SetAdditionalInfo(additionalInfo);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets both a new value for this parameter and updates the additionalInfo.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="additionalInfo"></param>
        /// <param name="parameterValueConverters"></param>
        /// <returns></returns>
        public bool SetValue(object newValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            if (SetValue(newValue, parameterValueConverters))
            {
                SetAdditionalInfo(additionalInfo);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var additionalInfo = "None";
            if (HasAdditionalInfo())
            {
                additionalInfo = _additionalInfo.ToString();
            }
            return "Parameter has these values:" + Environment.NewLine +
                "\tThe key is: " + Key + Environment.NewLine +
                "\tThe type to convert to is: " + Enum.GetName(typeof(ParameterType), Type) + Environment.NewLine +
                "\tThe value to convert from is: " + (HasValue() ? _value.ToString() : "null") + Environment.NewLine +
                "\tAdditionalInfo: " + additionalInfo;
        }

        /// <summary>
        /// Based on the converters, can we convert it to given type.
        /// </summary>
        /// <param name="type">The type we want to convert to.</param>
        /// <returns></returns>
        public bool CanBeConvertedTo(Type type)
        {
            var acAttribute = type.GetCustomAttribute<AttributeConvertibleAttribute>();
            if (acAttribute != null && acAttribute.ParameterType == Type)
            {
                return true;
            }

            if (_customParameterValueConverters != null)
            {
                if (_customParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, type, _value, _additionalInfo, _customParameterValueConverters, ParameterConverterExtensions.JsonSerializer)) != null)
                {
                    return true;
                }
            }

            if (DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, type, _value, _additionalInfo, _customParameterValueConverters, ParameterConverterExtensions.JsonSerializer)) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Based on the converters, can we convert it to given type.
        /// </summary>
        /// <param name="type">The type we want to convert to.</param>
        /// <param name="parameterValueConverters">Some converters. The method will try these converters first before it will check the other converters.</param>
        /// <returns></returns>
        public bool CanBeConvertedTo(Type type, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            if (parameterValueConverters != null && parameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, type, _value, _additionalInfo, parameterValueConverters.ConcatWithNullCheck(_customParameterValueConverters), ParameterConverterExtensions.JsonSerializer)) != null)
            {
                return true;
            }

            return CanBeConvertedTo(type);
        }

        private IParameterValueConverter GetSuitableConverterToValue(Type typeToGet, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            var allCustomConverters = parameterValueConverters.ConcatWithNullCheck(_customParameterValueConverters);

            var converter = allCustomConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, typeToGet, _value, _additionalInfo, allCustomConverters, ParameterConverterExtensions.JsonSerializer));

            if (converter != null)
            {
                return converter;
            }

            converter = DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(Type, typeToGet, _value, _additionalInfo, allCustomConverters, ParameterConverterExtensions.JsonSerializer));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + typeToGet.Name + " from parameter type " + Type + " was not found.");
        }

        private static ParameterType GetBestSuitableParameterType(object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters1, IEnumerable<IParameterValueConverter> customConverters2 = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("You are trying to create a parameter with a null value without any way for us to understand what type of object to assume you want. If you want to create a new parameter with a null-value, please use one of the methods that let us understand the type.");
            }
            return GetBestSuitableParameterType(value, value.GetType(), additionalInfo, customConverters1, customConverters2);
        }

        private static ParameterType GetBestSuitableParameterType(object value, Type valueType, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters1, IEnumerable<IParameterValueConverter> customConverters2 = null)
        {
            var acAttribute = valueType.GetCustomAttribute<AttributeConvertibleAttribute>();
            if (acAttribute != null)
            {
                return acAttribute.ParameterType;
            }

            var customConverters = customConverters1.ConcatWithNullCheck(customConverters2).ToArray();

            if (valueType == typeof(string))
            {
                if (value != null && ((string)value).Contains('\n'))
                {
                    return ParameterType.String_Multiline;
                }
                else
                {
                    return ParameterType.String;
                }
            }
            else if (valueType == typeof(int) || valueType == typeof(long)
                || valueType == typeof(int?) || valueType == typeof(long?))
            {
                return ParameterType.Int;
            }
            else if (valueType == typeof(float) || valueType == typeof(double) || valueType == typeof(decimal)
                || valueType == typeof(float?) || valueType == typeof(double?) || valueType == typeof(decimal?))
            {
                return ParameterType.Decimal;
            }
            else if (typeof(IEnumerable<byte>).IsAssignableFrom(valueType))
            {
                return ParameterType.Bytes;
            }
            else if (valueType == typeof(bool) || valueType == typeof(bool?))
            {
                return ParameterType.Bool;
            }
            else if (valueType == typeof(DateTime) || valueType == typeof(DateTime?))
            {
                if (value != null && ((DateTime)value).TimeOfDay == TimeSpan.Zero)
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
                if (value != null && ((IEnumerable<string>)value).Any(s => s.Contains('\n')))
                {
                    return ParameterType.String_Multiline_IEnumerable;
                }
                else
                {
                    return ParameterType.String_IEnumerable;
                }
            }
            else if (typeof(IEnumerable<int>).IsAssignableFrom(valueType) || typeof(IEnumerable<long>).IsAssignableFrom(valueType)
                || typeof(IEnumerable<int?>).IsAssignableFrom(valueType) || typeof(IEnumerable<long?>).IsAssignableFrom(valueType))
            {
                return ParameterType.Int_IEnumerable;
            }
            else if (typeof(IEnumerable<float>).IsAssignableFrom(valueType) || typeof(IEnumerable<double>).IsAssignableFrom(valueType) || typeof(IEnumerable<decimal>).IsAssignableFrom(valueType)
                || typeof(IEnumerable<float?>).IsAssignableFrom(valueType) || typeof(IEnumerable<double?>).IsAssignableFrom(valueType) || typeof(IEnumerable<decimal?>).IsAssignableFrom(valueType))
            {
                return ParameterType.Decimal_IEnumerable;
            }
            else if (typeof(IEnumerable<bool>).IsAssignableFrom(valueType) || typeof(IEnumerable<bool?>).IsAssignableFrom(valueType))
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
            else if (typeof(IEnumerable<DateTime?>).IsAssignableFrom(valueType))
            {
                if (value != null && ((IEnumerable<DateTime?>)value).Any(v => v.HasValue) && ((IEnumerable<DateTime?>)value).All(v => !v.HasValue || v.Value.TimeOfDay == TimeSpan.Zero))
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
            else if (typeof(Enum).IsAssignableFrom(valueType) || Nullable.GetUnderlyingType(valueType) != null && typeof(Enum).IsAssignableFrom(Nullable.GetUnderlyingType(valueType)))
            {
                return ParameterType.Enum;
            }
            else if (customConverters != null)
            {
                foreach (var type in (IEnumerable<ParameterType>)Enum.GetValues(typeof(ParameterType)))
                {
                    var converter = customConverters.FirstOrDefault(c => c.CanConvertFromValue(type, valueType, value, additionalInfo, customConverters));
                    if (converter != null)
                    {
                        return type;
                    }
                }
            }

            foreach (var type in (IEnumerable<ParameterType>)Enum.GetValues(typeof(ParameterType)))
            {
                var converter = DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(type, valueType, value, additionalInfo, customConverters));
                if (converter != null)
                {
                    return type;
                }
            }

            throw new ArgumentException("Did not find any suitable ParameterType for the valuetype " + valueType);
        }

        private IParameterValueConverter GetSuitableConverterFromValue(object value, Type valueType, ParameterType parameterType, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            var allCustomConverters = parameterValueConverters.ConcatWithNullCheck(_customParameterValueConverters);
            var converter = parameterValueConverters != null ? parameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value, additionalInfo, allCustomConverters)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = _customParameterValueConverters != null ? _customParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value, additionalInfo, allCustomConverters)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value, additionalInfo, allCustomConverters));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + valueType.Name + " to parameter type " + parameterType + " was not found.");
        }

        private bool SetValueCustomConverters(object newValue, Type valueType)
        {
            if ((Type == ParameterType.Enum || Type == ParameterType.SelectOne) && valueType == typeof(string))
            {
                var v = _value.ToObject<ParameterCollection>(ParameterConverterExtensions.JsonSerializer);
                if (v.GetByKeyAndType<List<string>>("choices", ParameterType.String_IEnumerable).Contains((string)newValue) || string.IsNullOrEmpty((string)newValue))
                {
                    if (v.GetParameterByKeyAndType("value", ParameterType.String).SetValue(newValue))
                    {
                        _value = JToken.FromObject(v, ParameterConverterExtensions.JsonSerializer);
                        return true;
                    }
                }
                return false;
            }
            else if (Type == ParameterType.SelectMany && typeof(IEnumerable<string>).IsAssignableFrom(valueType))
            {
                var va = _value.ToObject<ParameterCollection>(ParameterConverterExtensions.JsonSerializer);
                if (((IEnumerable<string>)newValue).All(value => va.GetByKeyAndType<List<string>>("choices", ParameterType.String_IEnumerable).Contains(value)))
                {
                    if (va.GetParameterByKeyAndType("value", ParameterType.String_IEnumerable).SetValue(newValue))
                    {
                        _value = JToken.FromObject(va, ParameterConverterExtensions.JsonSerializer);
                        return true;
                    }
                }
                return false;
            }

            return false;
        }
        
        /// <summary>
        /// Creates a new parameter based on the values. If skipNullValues is true and the value is null, it will return null, if false and the value is null, it will be created as a ParameterType.String.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="skipNullValues"></param>
        /// <returns></returns>
        public static Parameter CreateFromJToken(string key, JToken value, bool skipNullValues = false)
        {
            var type = ParameterConverterExtensions.GuessType(value, skipNullValues);
            if (!type.HasValue)
            {
                return null;
            }
            return new Parameter(key, value, type.Value, null, null);
        }
    }
}

