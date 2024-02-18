using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection
{
    [JsonObject]
    public class ParameterCollection : IEnumerable<Parameter>
    {
        [JsonProperty("parameters")]
        private List<Parameter> _parameters;

        [JsonProperty("customConverters", NullValueHandling = NullValueHandling.Ignore)]
        private List<IParameterValueConverter> _customParameterValueConverters { get; set; }

        public ParameterCollection()
        {
            _parameters = new List<Parameter>();
        }

        [JsonConstructor]
        public ParameterCollection(IEnumerable<Parameter> parameters, IEnumerable<IParameterValueConverter> customConverters)
        {
            _parameters = new List<Parameter>(parameters);
            if (customConverters != null)
            {
                _customParameterValueConverters = new List<IParameterValueConverter>(customConverters);
            }
        }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add a parameter directly to the list.
        /// </summary>
        /// <param name="parameter">The parameter to add.</param>
        public void Add(Parameter parameter)
        {
            _parameters.Add(parameter);
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given string value.</param>
        /// <param name="multiline">Is the string meant to be multiline or should it only be a one-liner.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, string value, bool multiline = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, multiline, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, DateTime value, bool onlyDate = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, onlyDate, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="multiline">Is the strings meant to be multiline or should it only be one-liners.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, IEnumerable<string> value, bool multiline = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, multiline, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, IEnumerable<DateTime> value, bool onlyDate = false, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, onlyDate, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create a new parameter where you can choose one value between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value. This value must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, string value, IEnumerable<string> choices, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, choices, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create a new parameter where you can choose one or more values between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value(s). Each string must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, IEnumerable<string> value, IEnumerable<string> choices, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, choices, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter. This will use either one of the default converters or another converter provided.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, object value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter. You also decides the parameterType, which define how the parameter is saved. This will use either one of the default converters or another converter provided.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="parameterType">The wanted type the parameter shall save/consider it as.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, object value, ParameterType parameterType, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, parameterType, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Adds a new converter to the list of custom converters for this ParameterCollection.
        /// </summary>
        /// <param name="parameterValueConverter">The converter to add.</param>
        /// <param name="addToExisting">If set to true, the converter will also be added to all existing parameters in this collection already. If set to false, only parameters added afterwards will get this converter.</param>
        public void AddCustomConverter(IParameterValueConverter parameterValueConverter, bool addToExisting = false)
        {
            if (parameterValueConverter == null)
            {
                return;
            }

            if (_customParameterValueConverters == null)
            {
                _customParameterValueConverters = new List<IParameterValueConverter>();
            }
            
            _customParameterValueConverters.Add(parameterValueConverter);

            if (addToExisting)
            {
                for (var i = 0; i < _parameters.Count; i++)
                {
                    _parameters[i].AddCustomConverter(parameterValueConverter);
                }
            }
        }

        /// <summary>
        /// Adds one or more converters to the list of custom converters for this ParameterCollection.
        /// </summary>
        /// <param name="parameterValueConverters">The converter(s) to add.</param>
        /// <param name="addToExisting">If set to true, the converter(s) will also be added to all existing parameters in this collection already. If set to false, only parameters added afterwards will get this converter.</param>
        public void AddCustomConverter(IEnumerable<IParameterValueConverter> parameterValueConverters, bool addToExisting = false)
        {
            if (parameterValueConverters == null)
            {
                return;
            }

            foreach(var converter in parameterValueConverters)
            {
                AddCustomConverter(converter, addToExisting);
            }
        }

        /// <summary>
        /// Gets all the custom converters added to this ParameterCollection. Mark that it will not check what is in each parameter, only what is added to this ParameterCollection.
        /// </summary>
        /// <returns></returns>
        public List<IParameterValueConverter> GetCustomConverters()
        {
            return _customParameterValueConverters;
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object this[string key, ParameterType type] => GetByKeyAndType(key, type);

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object this[string key, Type type] => GetByKey(key, type);

        /// <summary>
        /// Get the value by key.
        /// </summary>
        /// <typeparam name="T">The value-type expected to get back.</typeparam>
        /// <param name="key">The given key.</param>
        /// <returns>Returns the value as the given type.</returns>
        public T GetByKey<T>(string key)
        {
            return (T)_parameters.Find(p => p != null && p.Key == key).GetValue<T>();
        }

        /// <summary>
        /// Get the value by key.
        /// </summary>
        /// <typeparam name="T">The value-type expected to get back.</typeparam>
        /// <param name="key">The given key.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <returns>Returns the value as the given type.</returns>
        public T GetByKey<T>(string key, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            return (T)_parameters.Find(p => p != null && p.Key == key).GetValue<T>(parameterValueConverters);
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKeyAndType(string key, ParameterType type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type).GetValue(type.GetDefaultValueType());
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKeyAndType(string key, ParameterType type, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type).GetValue(type.GetDefaultValueType(), parameterValueConverters);
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <typeparam name="T">The value-type expected to get back.</typeparam>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as the given type.</returns>
        public T GetByKeyAndType<T>(string key, ParameterType type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type).GetValue<T>();
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <typeparam name="T">The value-type expected to get back.</typeparam>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <returns>Returns the value as the given type.</returns>
        public T GetByKeyAndType<T>(string key, ParameterType type, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type).GetValue<T>(parameterValueConverters);
        }

        /// <summary>
        /// Get the value by key in type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The wanted type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKey(string key, Type type)
        {
            return _parameters.Find(p => p != null && p.Key == key).GetValue(type);
        }

        /// <summary>
        /// Get the value by key in type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The wanted type.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKey(string key, Type type, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            return _parameters.Find(p => p != null && p.Key == key).GetValue(type, parameterValueConverters);
        }

        /// <summary>
        /// Get the type a given parameter has.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns></returns>
        public ParameterType GetParameterType(string key)
        {
            return _parameters.Find(p => p != null && p.Key == key).Type;
        }

        /// <summary>
        /// Get the whole parameter by key.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public Parameter GetParameterByKey(string key)
        {
            return _parameters.Find(p => p != null && p.Key == key);
        }

        /// <summary>
        /// Get the whole parameter by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public Parameter GetParameterByKeyAndType(string key, ParameterType type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type);
        }

        /// <summary>
        /// Do this collection has a parameter with the given key.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            return _parameters.Exists(p => p != null && p.Key == key);
        }

        /// <summary>
        /// Do this collection has a parameter with the given key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns></returns>
        public bool HasKeyWithType(string key, ParameterType type)
        {
            return _parameters.Exists(p => p != null && p.Key == key && p.Type == type);
        }

        /// <summary>
        /// Do this collection has a parameter that can be converted to type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns></returns>
        public bool HasKeyAndCanConvertTo(string key, Type type)
        {
            return _parameters.Exists(p => p != null && p.Key == key && p.CanBeConvertedTo(type));
        }

        /// <summary>
        /// Do this collection has a parameter that can be converted to type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <param name="parameterValueConverters">Some converters. The method will try these converters first before it will check the other converters.</param>
        /// <returns></returns>
        public bool HasKeyAndCanConvertTo(string key, Type type, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            return _parameters.Exists(p => p != null && p.Key == key && p.CanBeConvertedTo(type, parameterValueConverters));
        }

        public override string ToString()
        {
            var text = "ParameterCollection has these parameters:";
            foreach (var parameter in _parameters)
            {
                text += Environment.NewLine;
                if (parameter != null)
                {
                    text += parameter.ToString();
                }
                else
                {
                    text += "This parameter-object is null";
                }
            }
            return text;
        }

        /// <summary>
        /// Convert this collection to json.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, ParameterConverterExtensions.GetJsonSerializerSettings());
        }

        /// <summary>
        /// Converting the whole ParameterConverter to the given object.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="customConverters">Any custom converters to use when creating the object.</param>
        /// <returns></returns>
        public T ToObject<T>(IEnumerable<IParameterValueConverter> customConverters = null)
        {
            return (T)ToObject(typeof(T), customConverters);
        }

        /// <summary>
        /// Converting the whole ParameterConverter to the given object.
        /// </summary>
        /// <param name="type">The type to convert to.</typeparam>
        /// <param name="customConverters">Any custom converters to use when creating the object.</param>
        /// <returns></returns>
        public object ToObject(Type type, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            try
            {
                var acAttribute = type.GetCustomAttribute<AttributeConvertibleAttribute>();
                if (acAttribute != null)
                {
                    return type.GetObjectFromAttributes(JToken.FromObject(this), acAttribute, customConverters);
                }

                if (customConverters != null)
                {
                    var converter = customConverters.FirstOrDefault(c => c.CanConvertFromParameter(ParameterType.ParameterCollection, type, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer));

                    if (converter != null)
                    {
                        return converter.ConvertFromParameter(ParameterType.ParameterCollection, type, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer);
                    }
                }

                return GetSuitableConverterToValue(type).ConvertFromParameter(ParameterType.ParameterCollection, type, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer);
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value from one of the provided converters. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        /// <summary>
        /// Can this parameterCollection be converted to the given object.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="customConverters">Any custom converters to use when creating the object.</param>
        /// <returns></returns>
        public bool CanConvertToObject<T>(IEnumerable<IParameterValueConverter> customConverters = null)
        {
            return CanConvertToObject(typeof(T), customConverters);
        }

        public bool CanConvertToObject(Type type, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            try
            {
                if (customConverters != null)
                {
                    if (customConverters.Any(c => c.CanConvertFromParameter(ParameterType.ParameterCollection, type, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer)))
                    {
                        return true;
                    }
                }

                return GetSuitableConverterToValue(type).CanConvertFromParameter(ParameterType.ParameterCollection, type, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer);
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value from one of the provided converters. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        private IParameterValueConverter GetSuitableConverterToValue(Type typeToGet)
        {
            var converter = _customParameterValueConverters != null ? _customParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(ParameterType.ParameterCollection, typeToGet, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = Parameter.DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(ParameterType.ParameterCollection, typeToGet, JToken.FromObject(this), ParameterConverterExtensions.JsonSerializer));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + typeToGet.Name + " from parameter type " + ParameterType.ParameterCollection + " was not found.");
        }

        private static IParameterValueConverter GetSuitableConverterFromValue(object value, ParameterType parameterType, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            var valueType = value.GetType();
            var converter = parameterValueConverters != null ? parameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = Parameter.DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + valueType.Name + " to parameter type " + parameterType + " was not found.");
        }

        /// <summary>
        /// Get a parameter collection from json.
        /// </summary>
        /// <param name="json">The json representation of the parameter collection.</param>
        /// <returns>A new ParameterCollection-instance based on the json-settings.</returns>
        public static ParameterCollection FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ParameterCollection>(json, ParameterConverterExtensions.GetJsonSerializerSettings());
        }

        /// <summary>
        /// Create a new ParameterCollection 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="customConverters"></param>
        /// <returns></returns>
        public static ParameterCollection FromObject(object value, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            var type = value.GetType();
            var acAttribute = type.GetCustomAttribute<AttributeConvertibleAttribute>();
            if (acAttribute != null && acAttribute.ParameterType == ParameterType.ParameterCollection)
            {
                return type.GetParameterCollectionFromAttributes(value, customConverters);
            }

            var converter = GetSuitableConverterFromValue(value, ParameterType.ParameterCollection, customConverters);
            return converter.ConvertFromValue(ParameterType.ParameterCollection, type, value, ParameterConverterExtensions.JsonSerializer).ToObject<ParameterCollection>(ParameterConverterExtensions.JsonSerializer);
        }
    }
}
