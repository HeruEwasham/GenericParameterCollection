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

        private static JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault(ParameterConverterExtensions.GetJsonSerializerSettings());

        [JsonConstructor]
        private Parameter(string key, JToken value, ParameterType type)
        {
            Key = key;
            _value = value;
            Type = type;
        }

        private Parameter(string key, ParameterCollection value, ParameterType type) : this(key, JToken.FromObject(value, _jsonSerializer), type) { }

        private Parameter(string key, IEnumerable<ParameterCollection> value, ParameterType type) : this(key, JToken.FromObject(value, _jsonSerializer), type) { }

        private Parameter(string key, IEnumerable<string> value, ParameterType type) : this(key, JToken.FromObject(value, _jsonSerializer), type) { }

        public Parameter(string key, string value, bool multiline = false) : this(key, value, multiline ? ParameterType.String_Multiline : ParameterType.String) { }

        public Parameter(string key, int value) : this(key, value.ToString(), ParameterType.Int) { }

        public Parameter(string key, float value) : this(key, value, ParameterType.Float) { }

        public Parameter(string key, long value) : this(key, value, ParameterType.Long) { }

        public Parameter(string key, double value) : this(key, value, ParameterType.Double) { }

        public Parameter(string key, byte[] value) : this(key, Convert.ToBase64String(value), ParameterType.Bytes) { }

        public Parameter(string key, bool value) : this(key, value.ToString(), ParameterType.Bool) { }

        public Parameter(string key, DateTime value, bool onlyDate = false) : this(key, value, onlyDate ? ParameterType.Date : ParameterType.DateTime) { }

        public Parameter(string key, IEnumerable<string> value, bool multiline = false) : this(key, value, multiline ? ParameterType.String_Multiline_IEnumerable : ParameterType.String_IEnumerable) { }

        public Parameter(string key, IEnumerable<int> value) : this(key, JToken.FromObject(value), ParameterType.Int_IEnumerable) { }

        public Parameter(string key, IEnumerable<float> value) : this(key, JToken.FromObject(value), ParameterType.Float_IEnumerable) { }

        public Parameter(string key, IEnumerable<double> value) : this(key, JToken.FromObject(value), ParameterType.Double_IEnumerable) { }

        public Parameter(string key, IEnumerable<long> value) : this(key, JToken.FromObject(value), ParameterType.Long_IEnumerable) { }

        public Parameter(string key, IEnumerable<bool> value) : this(key, JToken.FromObject(value), ParameterType.Bool_IEnumerable) { }

        public Parameter(string key, IEnumerable<DateTime> value, bool onlyDate = false) : this(key, JToken.FromObject(value), onlyDate ? ParameterType.Date_IEnumerable : ParameterType.DateTime_IEnumerable) { }

        public Parameter(string key, ParameterCollection value) : this(key, value, ParameterType.ParameterCollection) { }

        public Parameter(string key, IEnumerable<ParameterCollection> value) : this(key, value, ParameterType.ParameterCollection_IEnumerable) { }

        public Parameter(string key, Enum value) : this(key, value.ToParameterCollection(), ParameterType.Enum) { }

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

        public override string ToString()
        {
            return "Parameter has these values:" + Environment.NewLine +
                "\tThe key is: " + Key + Environment.NewLine +
                "\tThe type to convert to is: " + Enum.GetName(typeof(ParameterType), Type) + Environment.NewLine +
                "\tThe value to convert from is: " + _value.ToString();
        }
    }
}

