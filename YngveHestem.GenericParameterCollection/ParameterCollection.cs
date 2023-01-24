using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YngveHestem.GenericParameterCollection
{
    public class ParameterCollection : IEnumerable<Parameter>
    {
        [JsonProperty("parameters")]
        private List<Parameter> _parameters;

        public ParameterCollection()
        {
            _parameters = new List<Parameter>();
        }

        [JsonConstructor]
        public ParameterCollection(IEnumerable<Parameter> parameters)
        {
            _parameters = new List<Parameter>(parameters);
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
        public void Add(string key, string value, bool multiline = false)
        {
            Add(new Parameter(key, value, multiline));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, int value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, float value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, long value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, double value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant or is it only the date that is relevant.</param>
        public void Add(string key, DateTime value, bool onlyDate = false)
        {
            Add(new Parameter(key, value, onlyDate));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, byte[] value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, bool value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, ParameterCollection value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="multiline">Is the strings meant to be multiline or should it only be one-liners.</param>
        public void Add(string key, IEnumerable<string> value, bool multiline = false)
        {
            Add(new Parameter(key, value, multiline));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, IEnumerable<int> value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, IEnumerable<float> value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, IEnumerable<double> value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, IEnumerable<long> value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, IEnumerable<bool> value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        /// <param name="onlyDate">Is both the date and date part relevant in this list or is it only the date that is relevant.</param>
        public void Add(string key, IEnumerable<DateTime> value, bool onlyDate = false)
        {
            Add(new Parameter(key, value, onlyDate));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, IEnumerable<ParameterCollection> value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create and add a new parameter.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value.</param>
        public void Add(string key, Enum value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Create a new parameter where you can choose one value between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value. This value must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        public void Add(string key, string value, IEnumerable<string> choices)
        {
            Add(new Parameter(key, value, choices));
        }

        /// <summary>
        /// Create a new parameter where you can choose one or more values between some given choices.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="value">The given value(s). Each string must be exact the same as one of the choices in the list of choices.</param>
        /// <param name="choices">A list of choices.</param>
        public void Add(string key, IEnumerable<string> value, IEnumerable<string> choices)
        {
            Add(new Parameter(key, value, choices));
        }

        /// <summary>
        /// Get the value by key.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object this[string key] => GetByKey(key);

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
        public object this[string key, Type type] => GetByKeyAndType(key, type);

        /// <summary>
        /// Get the value by key.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKey(string key)
        {
            return _parameters.Find(p => p != null && p.Key == key).GetValue();
        }

        /// <summary>
        /// Get the value by key.
        /// </summary>
        /// <typeparam name="T">The value-type expected to get back.</typeparam>
        /// <param name="key">The given key.</param>
        /// <returns>Returns the value as the given type.</returns>
        public T GetByKey<T>(string key)
        {
            return (T)GetByKey(key);
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKeyAndType(string key, ParameterType type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type).GetValue();
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
            return (T)GetByKeyAndType(key, type);
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as a generic object.</returns>
        public object GetByKeyAndType(string key, Type type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type.IsValidType(type)).GetValue();
        }

        /// <summary>
        /// Get the value by key and type.
        /// </summary>
        /// <typeparam name="T">The value-type expected to get back.</typeparam>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns>Returns the value as the given type.</returns>
        public T GetByKeyAndType<T>(string key, Type type)
        {
            return (T)GetByKeyAndType(key, type);
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
        /// Do this collection has a parameter with the given key and type.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <param name="type">The given type.</param>
        /// <returns></returns>
        public bool HasKeyWithType(string key, Type type)
        {
            return _parameters.Exists(p => p != null && p.Key == key && p.Type.IsValidType(type));
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
        /// Get a parameter collection from json.
        /// </summary>
        /// <param name="json">The json representation of the parameter collection.</param>
        /// <returns>A new ParameterCollection-instance based on the json-settings.</returns>
        public static ParameterCollection FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ParameterCollection>(json, ParameterConverterExtensions.GetJsonSerializerSettings());
        }
    }
}
