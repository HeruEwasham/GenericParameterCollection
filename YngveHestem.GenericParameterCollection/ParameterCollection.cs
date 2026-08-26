using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;
using YngveHestem.GenericParameterCollection.ParameterValueConverters.CustomConverters;

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
        public void Add(string key, string value, bool multiline, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
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
        public void Add(string key, DateTime value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, onlyDate, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, DateTime? value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
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
        public void Add(string key, IEnumerable<string> value, bool multiline, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
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
        public void Add(string key, IEnumerable<DateTime> value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, onlyDate, additionalInfo, _customParameterValueConverters, customConverters));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here you can put custom converters if you want the value saved differently than the default converters save it as. The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, IEnumerable<DateTime?> value, bool onlyDate, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
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
        /// <typeparam name="T">Specify the type of the value given.</typeparam>
        public void Add<T>(string key, T value, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, typeof(T), additionalInfo, _customParameterValueConverters, customConverters));
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
        /// Create and add a new parameter. This will use either one of the default converters or another converter provided.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="valueType">Specify the type of the value given.</param>
        /// <param name="additionalInfo">This is a parameter that can be used to add more information to the parameter. This can for example be used to communicate between the part of the program that wants some parameters, and the part that show the parameters to the user, like tell that it only allow subsets of what the type can deliver. It can also be used the other way, to give more information about the content without needing to have seperate parameters to search for.</param>
        /// <param name="customConverters">Here goes custom converters needed to convert value (if default converters don't support it, or you want it saved differently). The converters added here will not be saved to the parameter, but only be used to convert the inputted value to the parameter. If you want to save a custom converter to the parameter(s), use AddCustomConverter(..)-method before calling the add-method instead.</param>
        public void Add(string key, object value, Type valueType, ParameterCollection additionalInfo = null, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            Add(new Parameter(key, value, valueType, additionalInfo, _customParameterValueConverters, customConverters));
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

            foreach (var converter in parameterValueConverters)
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
            var p = _parameters.Find(q => q != null && q.Key == key);
            if (p == null || !p.HasValue())
            {
                var t = typeof(T);
                if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                {
                    throw new KeyNotFoundException($"Parameter with key '{key}' not found or has null value but requested type {t.Name} is non-nullable.");
                }
                return default(T);
            }
            return p.GetValue<T>();
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
            var p = _parameters.Find(q => q != null && q.Key == key);
            if (p == null || !p.HasValue())
            {
                var t = typeof(T);
                if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                {
                    throw new KeyNotFoundException($"Parameter with key '{key}' not found or has null value but requested type {t.Name} is non-nullable.");
                }
                return default(T);
            }
            return (T)p.GetValue<T>(parameterValueConverters);
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKeyAndType(string key, ParameterType type)
        {
            var p = _parameters.Find(q => q != null && q.Key == key && q.Type == type);
            if (p == null || !p.HasValue())
            {
                var t = type.GetDefaultValueType();
                if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                {
                    throw new KeyNotFoundException($"Parameter with key '{key}' not found or has null value but requested type {t.Name} is non-nullable.");
                }
                return null;
            }
            return p.GetValue(type.GetDefaultValueType());
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
            var p = _parameters.Find(q => q != null && q.Key == key && q.Type == type);
            if (p == null || !p.HasValue())
            {
                var t = type.GetDefaultValueType();
                if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                {
                    throw new KeyNotFoundException($"Parameter with key '{key}' not found or has null value but requested type {t.Name} is non-nullable.");
                }
                return null;
            }
            return p.GetValue(type.GetDefaultValueType(), parameterValueConverters);
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
            var p = _parameters.Find(q => q != null && q.Key == key && q.Type == type);
            if (p == null || !p.HasValue())
            {
                var t = typeof(T);
                if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                {
                    throw new KeyNotFoundException($"Parameter with key '{key}' not found or has null value but requested type {t.Name} is non-nullable.");
                }
                return default(T);
            }
            return p.GetValue<T>();
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
            var p = _parameters.Find(q => q != null && q.Key == key && q.Type == type);
            if (p == null || !p.HasValue())
            {
                var t = typeof(T);
                if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                {
                    throw new KeyNotFoundException($"Parameter with key '{key}' not found or has null value but requested type {t.Name} is non-nullable.");
                }
                return default(T);
            }
            return p.GetValue<T>(parameterValueConverters);
        }

        /// <summary>
        /// Get the value by key in type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The wanted type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKey(string key, Type type)
        {
            var p = _parameters.Find(q => q != null && q.Key == key);
            if (p == null) return null;
            if (!p.HasValue()) return null;
            return p.GetValue(type);
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
            var p = _parameters.Find(q => q != null && q.Key == key);
            if (p == null) return null;
            if (!p.HasValue()) return null;
            return p.GetValue(type, parameterValueConverters);
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
        /// <param name="formatting">Any special formatting?</param>
        /// <returns></returns>
        public string ToJson(Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(this, formatting, ParameterCollectionExtensions.GetJsonSerializerSettings());
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
                    return type.GetObjectFromAttributes(JToken.FromObject(this, ParameterCollectionExtensions.JsonSerializer), acAttribute, customConverters);
                }
                ParameterCollection additionalInfo = new ParameterCollection();

                return GetSuitableConverterToValue(type, additionalInfo, customConverters).ConvertFromParameter(ParameterType.ParameterCollection, type, JToken.FromObject(this), additionalInfo, customConverters.ConcatWithNullCheck(_customParameterValueConverters), ParameterCollectionExtensions.JsonSerializer);
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
                ParameterCollection additionalInfo = new ParameterCollection();
                return GetSuitableConverterToValue(type, additionalInfo, customConverters) != null;
            }
            catch (Exception e)
            {
                throw new Exception("Got exception when getting a parameter value from one of the provided converters. Message on exception: " + e.Message + Environment.NewLine + ToString(), e);
            }
        }

        private IParameterValueConverter GetSuitableConverterToValue(Type typeToGet, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            var allCustomConverters = customConverters.ConcatWithNullCheck(_customParameterValueConverters);
            var converter = allCustomConverters.FirstOrDefault(c => c.CanConvertFromParameter(ParameterType.ParameterCollection, typeToGet, JToken.FromObject(this), additionalInfo, allCustomConverters, ParameterCollectionExtensions.JsonSerializer));

            if (converter != null)
            {
                return converter;
            }

            converter = Parameter.DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromParameter(ParameterType.ParameterCollection, typeToGet, JToken.FromObject(this), additionalInfo, allCustomConverters, ParameterCollectionExtensions.JsonSerializer));

            if (converter != null)
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException("Converter to support this conversion between this value in type " + typeToGet.Name + " from parameter type " + ParameterType.ParameterCollection + " was not found.");
        }

        private static IParameterValueConverter GetSuitableConverterFromValue(object value, ParameterType parameterType, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> parameterValueConverters)
        {
            var valueType = value.GetType();
            var converter = parameterValueConverters != null ? parameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value, additionalInfo, parameterValueConverters)) : null;

            if (converter != null)
            {
                return converter;
            }

            converter = Parameter.DefaultParameterValueConverters.FirstOrDefault(c => c.CanConvertFromValue(parameterType, valueType, value, additionalInfo, parameterValueConverters));

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
            return JsonConvert.DeserializeObject<ParameterCollection>(json, ParameterCollectionExtensions.GetJsonSerializerSettings());
        }

        /// <summary>
        /// Create a new ParameterCollection 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="customConverters"></param>
        /// <returns></returns>
        public static ParameterCollection FromObject(object value, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            return FromObject(value, value.GetType(), customConverters);
        }

        /// <summary>
        /// Create a new ParameterCollection 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="customConverters"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static ParameterCollection FromObject<TValue>(TValue value, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            return FromObject(value, typeof(TValue), customConverters);
        }

        /// <summary>
        /// Create a new ParameterCollection 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType">Specify the type of the value given.</param>
        /// <param name="customConverters"></param>
        /// <returns></returns>
        public static ParameterCollection FromObject(object value, Type valueType, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            var acAttribute = valueType.GetCustomAttribute<AttributeConvertibleAttribute>();
            if (acAttribute != null && acAttribute.ParameterType == ParameterType.ParameterCollection)
            {
                return valueType.GetParameterCollectionFromAttributes(value, customConverters);
            }

            ParameterCollection additionalInfo = new ParameterCollection();
            var converter = GetSuitableConverterFromValue(value, ParameterType.ParameterCollection, additionalInfo, customConverters);
            return converter.ConvertFromValue(ParameterType.ParameterCollection, valueType, value, additionalInfo, customConverters, ParameterCollectionExtensions.JsonSerializer).ToObject<ParameterCollection>(ParameterCollectionExtensions.JsonSerializer);
        }

        /// <summary>
        /// Creates a ParameterCollection from any inputted json. This will try it's best to determine the type, but that depends on how good the values in the json are.
        /// </summary>
        /// <param name="json">The JSON you want to convert to a ParameterCollection. If json-list inptted as root, default key will most likely be used as the only key in it's own root-ParameterCollection. If json starts as a list instead of an object, or you will rather that json is returned as a list of ParameterCollections anyway, FromAnyJsonList(..) may be better based on use-case.</param>
        /// <param name="defaultKey">If a key can not be decided from the json, this will be used as the key. This will most likely be used if the json starts as an array.</param>
        /// <param name="skipNullValues">If true, all parameters that contain null will be skipped, if not, it will be set as ParameterType.String.</param>
        /// <param name="convertBase64ToBytesType">Should all that might be converted to base64 be converted to ParameterType.Bytes?</param>
        /// <returns></returns>
        public static ParameterCollection FromAnyJson(string json, string defaultKey = "default", bool skipNullValues = false, bool convertBase64ToBytesType = false)
        {
            var token = JToken.Parse(json);
            return FromAnyJson(token, defaultKey, skipNullValues, convertBase64ToBytesType);
        }

        /// <summary>
        /// Creates a ParameterCollection from any inputted json. This will try it's best to determine the type, but that depends on how good the values in the json are.
        /// </summary>
        /// <param name="token">The JToken you want to convert to a ParameterCollection. If JToken inptted is an array, default key will most likely be used as the only key in it's own root-ParameterCollection. If json starts as a list instead of an object, or you will rather that json is returned as a list of ParameterCollections anyway, FromAnyJsonList(..) may be better based on use-case.</param>
        /// <param name="defaultKey">If a key can not be decided from the json, this will be used as the key. This will most likely be used if the json starts as an array.</param>
        /// <param name="skipNullValues">If true, all parameters that contain null will be skipped, if not, it will be set as ParameterType.String.</param>
        /// <param name="convertBase64ToBytesType">Should all that might be converted to base64 be converted to ParameterType.Bytes?</param>
        /// <returns></returns>
        public static ParameterCollection FromAnyJson(JToken token, string defaultKey = "default", bool skipNullValues = false, bool convertBase64ToBytesType = false)
        {
            var collection = new ParameterCollection();

            if (token.Type == JTokenType.Object)
            {
                var dict = token.ToObject<Dictionary<string, JToken>>();
                foreach (var kvp in dict)
                {
                    var parameter = Parameter.CreateFromJToken(kvp.Key, kvp.Value, skipNullValues, convertBase64ToBytesType);
                    if (parameter != null)
                    {
                        collection.Add(parameter);
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                var parameter = Parameter.CreateFromJToken(defaultKey, token, skipNullValues, convertBase64ToBytesType);
                if (parameter != null)
                {
                    collection.Add(parameter);
                }
            }

            return collection;
        }

        /// <summary>
        /// Creates a ParameterCollection from any inputted json. This will try it's best to determine the type, but that depends on how good the values in the json are.
        /// </summary>
        /// <param name="json">The JSON you want to convert to a ParameterCollection. If json inptted has an object as root (and not a list), one ParameterCollection will be created in list, so it is safe to usse it with any json. Bt if you rather want it as a single ParameterCollection, FromAnyJson(..) might be better bassed on use case.</param>
        /// <param name="defaultKey">If a key can not be decided from the json, this will be used as the key. This will most likely be used if the json starts as an array.</param>
        /// <param name="skipNullValues">If true, all parameters that contain null will be skipped, if not, it will be set as ParameterType.String.</param>
        /// <param name="convertBase64ToBytesType">Should all that might be converted to base64 be converted to ParameterType.Bytes?</param>
        /// <returns></returns>
        public static List<ParameterCollection> FromAnyJsonList(string json, string defaultKey = "default", bool skipNullValues = false, bool convertBase64ToBytesType = false)
        {
            var token = JToken.Parse(json);
            return FromAnyJsonList(token, defaultKey, skipNullValues, convertBase64ToBytesType);
        }

        /// <summary>
        /// Creates a ParameterCollection from any inputted json. This will try it's best to determine the type, but that depends on how good the values in the json are.
        /// </summary>
        /// <param name="token">The JToken you want to convert to a ParameterCollection. If JTokeen inptted is an object (and not an array), one ParameterCollection will be created in the list, so it is safe to usse it with any json. But if you rather want it as a single ParameterCollection, FromAnyJson(..) might be better bassed on use case.</param>
        /// <param name="defaultKey">If a key can not be decided from the json, this will be used as the key. This will most likely be used if the json starts as an array.</param>
        /// <param name="skipNullValues">If true, all parameters that contain null will be skipped, if not, it will be set as ParameterType.String.</param>
        /// <param name="convertBase64ToBytesType">Should all that might be converted to base64 be converted to ParameterType.Bytes?</param>
        /// <returns></returns>
        public static List<ParameterCollection> FromAnyJsonList(JToken token, string defaultKey = "default", bool skipNullValues = false, bool convertBase64ToBytesType = false)
        {
            var collectionList = new List<ParameterCollection>();

            if (token.Type == JTokenType.Array)
            {
                foreach(var jObject in token)
                {
                    collectionList.Add(FromAnyJson(jObject, defaultKey, skipNullValues, convertBase64ToBytesType));
                }
            }
            else if (token.Type == JTokenType.Object)
            {
                collectionList.Add(FromAnyJson(token, defaultKey, skipNullValues, convertBase64ToBytesType));
            }

            return collectionList;
        }

        /// <summary>
        /// Convert the ParameterCollection to json in the form of { "key": "value" }. This will omit everything in AdditionalInfo, etc.
        /// </summary>
        /// <param name="formatting">Any special formatting?</param>
        /// <param name="enumValueHandling">How enums should be handdled. Should it be returned as string, int, or as an object with value and choices-parameters.</param>
        /// <param name="enumSelectOneManyValueParameterName">The name to use for the value-parameter of SelectOne,SelectMany and evt. Enum (if EnumValueHandling is set to show this).</param>
        /// <param name="enumSelectOneManyChoicesParameterName">The name to use for the choices-parameter of SelectOne,SelectMany and evt. Enum (if EnumValueHandling is set to show this).</param>
        /// <returns></returns>
        public string ToSimpleJson(Formatting formatting = Formatting.None, EnumValueHandling enumValueHandling = EnumValueHandling.CurrentValueAsString, string enumSelectOneManyValueParameterName = "value", string enumSelectOneManyChoicesParameterName = "choices")
        {
            var customConverters = new IParameterValueConverter[] { new JTokenParameterConverter() };
            var result = new Dictionary<string, JToken>();
            foreach (var parameter in _parameters)
            {
                if (parameter.Type == ParameterType.ParameterCollection)
                {
                    result.Add(parameter.Key, JToken.Parse(parameter.GetValue<ParameterCollection>(customConverters).ToSimpleJson(formatting, enumValueHandling, enumSelectOneManyValueParameterName, enumSelectOneManyChoicesParameterName)));
                }
                else if (parameter.Type == ParameterType.ParameterCollection_IEnumerable)
                {
                    result.Add(parameter.Key, JToken.FromObject(parameter.GetValue<IEnumerable<ParameterCollection>>(customConverters).Select(p => JToken.Parse(p.ToSimpleJson(formatting, enumValueHandling, enumSelectOneManyValueParameterName, enumSelectOneManyChoicesParameterName))), ParameterCollectionExtensions.JsonSerializer));
                }
                else if (parameter.Type == ParameterType.Enum)
                {
                    if (enumValueHandling == EnumValueHandling.CurrentValueAsInt)
                    {
                        result.Add(parameter.Key, JToken.FromObject(parameter.GetValue<int>()));
                    }
                    else if (enumValueHandling == EnumValueHandling.CurrentValueAsString)
                    {
                        result.Add(parameter.Key, JToken.FromObject(parameter.GetValue<string>()));
                    }
                    else if (enumValueHandling == EnumValueHandling.BothValueAndOptionsAsString)
                    {
                        var p = parameter.GetValue<ParameterCollection>();
                        var dict = new Dictionary<string, JToken>
                        {
                            { enumSelectOneManyValueParameterName, JToken.FromObject(p.GetByKey<string>("value")) },
                            { enumSelectOneManyChoicesParameterName, JToken.FromObject(p.GetByKey<string[]>("choices")) }
                        };
                        result.Add(parameter.Key, JToken.FromObject(dict));
                    }
                }
                else if (parameter.Type == ParameterType.SelectOne)
                {
                    var p = parameter.GetValue<ParameterCollection>();
                        var dict = new Dictionary<string, JToken>
                        {
                            { enumSelectOneManyValueParameterName, JToken.FromObject(p.GetByKey<string>("value")) },
                            { enumSelectOneManyChoicesParameterName, JToken.FromObject(p.GetByKey<string[]>("choices")) }
                        };
                        result.Add(parameter.Key, JToken.FromObject(dict));
                }
                else if (parameter.Type == ParameterType.SelectMany)
                {
                    var p = parameter.GetValue<ParameterCollection>();
                        var dict = new Dictionary<string, JToken>
                        {
                            { enumSelectOneManyValueParameterName, JToken.FromObject(p.GetByKey<string[]>("value")) },
                            { enumSelectOneManyChoicesParameterName, JToken.FromObject(p.GetByKey<string[]>("choices")) }
                        };
                        result.Add(parameter.Key, JToken.FromObject(dict));
                }
                else
                {
                    result.Add(parameter.Key, parameter.GetValue<JToken>(customConverters));
                }
            }
            return JsonConvert.SerializeObject(result, formatting, ParameterCollectionExtensions.GetJsonSerializerSettings());
        }

        /// <summary>
        /// Get a value based on a path containing keys.
        /// </summary>
        /// <param name="path">The given path of keys.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as the wanted type.</returns>
        public T GetByPath<T>(string path, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            return (T)GetByPath(path, typeof(T), parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
        }

        /// <summary>
        /// Get a value based on a path containing keys.
        /// </summary>
        /// <param name="path">The given path of keys.</param>
        /// <param name="type">The wanted type.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByPath(string path, Type type, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            var pathDivided = path.Split(new string[] {pathDivider}, StringSplitOptions.RemoveEmptyEntries);
            
            if (pathDivided.Length == 0 || string.IsNullOrEmpty(pathDivided[0]))
            {
                throw new ArgumentException("The path can't be empty.");
            }
            
            if (pathDivided.Length > 1)
            {
                if (pathDivided[1] == additionalInfoMarker)
                {
                    var additionalInfo = GetParameterByKey(pathDivided[0]).GetAdditionalInfo();
                    if (additionalInfo == null)
                    {
                        throw new ArgumentException($"Key \"{pathDivided[0]}\" has no additionalInfo, but you wanted that.");
                    }
                    return additionalInfo.GetByPath(path.RemoveFirstOccurence(pathDivided[0] + "." + pathDivided[1]), type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                }
                if (HasKeyWithType(pathDivided[0], ParameterType.ParameterCollection))
                {
                    return GetByKey<ParameterCollection>(pathDivided[0], parameterValueConverters).GetByPath(path.RemoveFirstOccurence(pathDivided[0]), type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                }
                if (HasKeyWithType(pathDivided[0], ParameterType.ParameterCollection_IEnumerable))
                {
                    return GetByKey<ParameterCollection[]>(pathDivided[0], parameterValueConverters).GetByPath(path.RemoveFirstOccurence(pathDivided[0]), type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                }
            }
            
            return GetByKey(pathDivided[0], type, parameterValueConverters);
        }

        /// <summary>
        /// Get a value based on a path containing keys.
        /// </summary>
        /// <param name="path">The given path of keys.</param>
        /// <param name="defaultValue">The default value to use if something fails.</param>
        /// <param name="allowNull">Should null be considered a legal output, or should ddefaltValue be sent if null is returned. If true, only on exception defauultValue will be sent.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as the wanted type.</returns>
        public T GetByPath<T>(string path, T defaultValue, bool allowNull, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            return (T)GetByPathOrDefault(path, typeof(T), defaultValue, allowNull, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
        }

        /// <summary>
        /// Get a value based on a path containing keys.
        /// </summary>
        /// <param name="path">The given path of keys.</param>
        /// <param name="type">The wanted type.</param>
        /// <param name="defaultValue">The default value to use if something fails. This must be of the ssame type ass type.</param>
        /// <param name="allowNull">Should null be considered a legal output, or should ddefaltValue be sent if null is returned. If true, only on exception defauultValue will be sent.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByPathOrDefault(string path, Type type, object defaultValue, bool allowNull = true, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            try
            {
                var res = GetByPath(path, type, parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);

                if (allowNull || res != null)
                {
                    return res;
                }
            }
            catch {}

            return defaultValue;
        }

        /// <summary>
        /// Get a parameter type based on a path containing keys. If a path goes through a list, first occurence is used.
        /// </summary>
        /// <param name="path">The given path of keys.</param>
        /// <param name="parameterValueConverters">Some converters. The function will try these converters first before it will check the other converters.</param>
        /// <param name="pathDivider">The divider between the keys in the path.</param>
        /// <param name="listMarker">The special key to mark a list. Use {0} in the marker to specify where the item number of list item to get (starts at 0). If you don't use this and the parameter is a list, it will either return only the first occurence of the final parameter, or, if the return-type given is a list, and the final parameter to get is not a list (or an inner list), it will return all occurences found.</param>
        /// <param name="additionalInfoMarker">The special key to say that the rest of the path is in the additionalInfo-value of the previous key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public ParameterType GetParameterTypeByPath(string path, IEnumerable<IParameterValueConverter> parameterValueConverters = null, string pathDivider = ".", string listMarker = "$$item{0}$$", string additionalInfoMarker = "$$ADDITIONAL_IMFO$$")
        {
            var pathDivided = path.Split(new string[] {pathDivider}, StringSplitOptions.RemoveEmptyEntries);
            
            if (pathDivided.Length == 0 || string.IsNullOrEmpty(pathDivided[0]))
            {
                throw new ArgumentException("The path can't be empty.");
            }
            
            if (pathDivided.Length > 1)
            {
                if (pathDivided[1] == additionalInfoMarker)
                {
                    var additionalInfo = GetParameterByKey(pathDivided[0]).GetAdditionalInfo();
                    if (additionalInfo == null)
                    {
                        throw new ArgumentException($"Key \"{pathDivided[0]}\" has no additionalInfo, but you wanted that.");
                    }
                    return additionalInfo.GetParameterTypeByPath(path.RemoveFirstOccurence(pathDivided[0] + "." + pathDivided[1]), parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                }
                if (HasKeyWithType(pathDivided[0], ParameterType.ParameterCollection))
                {
                    return GetByKey<ParameterCollection>(pathDivided[0], parameterValueConverters).GetParameterTypeByPath(path.RemoveFirstOccurence(pathDivided[0]), parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                }
                if (HasKeyWithType(pathDivided[0], ParameterType.ParameterCollection_IEnumerable))
                {
                    return GetByKey<ParameterCollection[]>(pathDivided[0], parameterValueConverters).GetParameterTypeByPath(path.RemoveFirstOccurence(pathDivided[0]), parameterValueConverters, pathDivider, listMarker, additionalInfoMarker);
                }
            }
            
            var p = GetParameterByKey(pathDivided[0]);
            if (p == null)
            {
                return ParameterType.String;
            }
            return p.Type;
        }
    }
}
