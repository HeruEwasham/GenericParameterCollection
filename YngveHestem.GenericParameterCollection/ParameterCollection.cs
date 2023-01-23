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

        public void Add(Parameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void Add(string key, string value, bool multiline = false)
        {
            Add(new Parameter(key, value, multiline));
        }

        public void Add(string key, int value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, float value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, long value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, double value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, DateTime value, bool onlyDate = false)
        {
            Add(new Parameter(key, value, onlyDate));
        }

        public void Add(string key, byte[] value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, bool value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, ParameterCollection value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, IEnumerable<string> value, bool multiline = false)
        {
            Add(new Parameter(key, value, multiline));
        }

        public void Add(string key, IEnumerable<int> value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, IEnumerable<float> value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, IEnumerable<double> value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, IEnumerable<long> value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, IEnumerable<bool> value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, IEnumerable<DateTime> value, bool onlyDate = false)
        {
            Add(new Parameter(key, value, onlyDate));
        }

        public void Add(string key, IEnumerable<ParameterCollection> value)
        {
            Add(new Parameter(key, value));
        }

        public void Add(string key, Enum value)
        {
            Add(new Parameter(key, value));
        }

        public object this[string key] => GetByKey(key);

        public object this[string key, ParameterType type] => GetByKeyAndType(key, type);

        public object this[string key, Type type] => GetByKeyAndType(key, type);

        public object GetByKey(string key)
        {
            return _parameters.Find(p => p != null && p.Key == key).GetValue();
        }

        public T GetByKey<T>(string key)
        {
            return (T)GetByKey(key);
        }

        public object GetByKeyAndType(string key, ParameterType type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type == type).GetValue();
        }

        public T GetByKeyAndType<T>(string key, ParameterType type)
        {
            return (T)GetByKeyAndType(key, type);
        }

        public object GetByKeyAndType(string key, Type type)
        {
            return _parameters.Find(p => p != null && p.Key == key && p.Type.IsValidType(type)).GetValue();
        }

        public T GetByKeyAndType<T>(string key, Type type)
        {
            return (T)GetByKeyAndType(key, type);
        }

        public ParameterType GetParameterType(string key)
        {
            return _parameters.Find(p => p != null && p.Key == key).Type;
        }

        public bool HasKey(string key)
        {
            return _parameters.Exists(p => p != null && p.Key == key);
        }

        public bool HasKeyWithType(string key, ParameterType type)
        {
            return _parameters.Exists(p => p != null && p.Key == key && p.Type == type);
        }

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
