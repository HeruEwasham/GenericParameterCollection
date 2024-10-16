using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public abstract class ParameterCollectionParameterConverter<TValueType> : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.ParameterCollection && targetType == typeof(TValueType))
            {
                return CanConvertFromParameterCollection(rawValue?.ToObject<ParameterCollection>(jsonSerializer), additionalInfo, customConverters);
            }
            else if (sourceType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable<TValueType>).IsAssignableFrom(targetType))
            {
                return CanConvertFromListOfParameterCollection(rawValue?.ToObject<IEnumerable<ParameterCollection>>(jsonSerializer), additionalInfo, customConverters);
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (targetType == ParameterType.ParameterCollection && sourceType == typeof(TValueType))
            {
                return CanConvertToParameterCollection((TValueType)value, additionalInfo, customConverters);
            }
            else if (targetType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable<TValueType>).IsAssignableFrom(sourceType))
            {
                return CanConvertToListOfParameterCollection((IEnumerable<TValueType>)value, additionalInfo, customConverters);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.ParameterCollection)
            {
                try
                {
                    return ConvertFromParameterCollection(rawValue?.ToObject<ParameterCollection>(jsonSerializer), additionalInfo, customConverters);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(TValueType).Name + " by " + GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }
            else if (sourceType == ParameterType.ParameterCollection_IEnumerable)
            {
                try
                {
                    return ConvertFromListOfParameterCollection(rawValue?.ToObject<IEnumerable<ParameterCollection>>(jsonSerializer), additionalInfo, customConverters)?.ToCorrectIEnumerable(targetType);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(IEnumerable<TValueType>).Name + " by " + GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + GetType().Name);
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.ParameterCollection)
            {
                try
                {
                    return JToken.FromObject(ConvertToParameterCollection((TValueType)value, additionalInfo, customConverters), jsonSerializer);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(TValueType).Name + " by " + GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }
            else if (targetType == ParameterType.ParameterCollection_IEnumerable)
            {
                try
                {
                    return JToken.FromObject(ConvertToListOfParameterCollection((IEnumerable<TValueType>)value, additionalInfo, customConverters), jsonSerializer);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(IEnumerable<TValueType>).Name + " by " + GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + GetType().Name);
        }

        /// <summary>
        /// Can this converter convert this ParameterCollection.
        /// </summary>
        /// <param name="value">The ParameterCollection to possibly convert later.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected abstract bool CanConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters);

        /// <summary>
        /// Can this converter convert this list of ParameterCollection. The default implementation checks that all list-items validates sucessfully and that the list itself is not null.
        /// </summary>
        /// <param name="value">The list of ParameterCollection to possibly convert later.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected virtual bool CanConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (value == null)
            {
                return false;
            }

            foreach(var item in value)
            {
                if (!CanConvertFromParameterCollection(item, additionalInfo, customConverters))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Can this value be converted to a ParameterCollection by this converter.
        /// </summary>
        /// <param name="value">The value to possibly convert later.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected abstract bool CanConvertToParameterCollection(TValueType value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters);

        /// <summary>
        /// Can this value be converted to a list of ParameterCollection by this converter. The default implementation checks that all list-items validates sucessfully and that the list itself is not null.
        /// </summary>
        /// <param name="value">The list of values to possibly convert later.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected virtual bool CanConvertToListOfParameterCollection(IEnumerable<TValueType> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (value == null)
            {
                return false;
            }

            foreach (var item in value)
            {
                if (!CanConvertToParameterCollection(item, additionalInfo, customConverters))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Convert ParameterCollection to wanted type.
        /// </summary>
        /// <param name="value">The ParameterCollection to convert.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected abstract TValueType ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters);

        /// <summary>
        /// Convert list of ParameterCollection to a list of wanted type. The default implementation goes through each element in the list and converts each element before returning the result as a list.
        /// </summary>
        /// <param name="value">The list of ParameterCollection to convert.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected virtual IEnumerable<TValueType> ConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new List<TValueType>();

            foreach(var item in value)
            {
                result.Add(ConvertFromParameterCollection(item, additionalInfo, customConverters));
            }

            return result;
        }

        /// <summary>
        /// Convert value to ParameterCollection.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected abstract ParameterCollection ConvertToParameterCollection(TValueType value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters);

        /// <summary>
        /// Convert list of values to list of ParameterCollection. The default implementation goes through each element in the list and converts each element before returning the result as a list.
        /// </summary>
        /// <param name="value">The list of values to convert.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected virtual IEnumerable<ParameterCollection> ConvertToListOfParameterCollection(IEnumerable<TValueType> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new List<ParameterCollection>();

            foreach (var item in value)
            {
                result.Add(ConvertToParameterCollection(item, additionalInfo, customConverters));
            }

            var defaultValueKey = GetDefaultValueKey(value, additionalInfo, customConverters);
            if (defaultValueKey != null && !additionalInfo.HasKey(defaultValueKey))
            {
                additionalInfo.Add(defaultValueKey, GetDefaultValue(value, additionalInfo, customConverters));
            }

            return result;
        }

        /// <summary>
        /// Should a default value be set when IEnumerables of the value (if the value is not already set) in the additionalInfo? Then return the name of the key. If you don't want to, return null. The default value are "defaultValue".
        /// </summary>
        /// <param name="value">The list of values that will be converted.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        protected virtual string GetDefaultValueKey(IEnumerable<TValueType> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return "defaultValue";
        }

        /// <summary>
        /// Gets the default value.
        /// This method will not be called if SetDefaultValueOnIEnumerablesKey is null or the key gotten from that method is already set.
        /// The default value are creating a new instance with the constructor without any arguments.
        /// </summary>
        /// <param name="value">The list of values that will be converted.</param>
        /// <param name="additionalInfo">Any additional info.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        protected virtual TValueType GetDefaultValue(IEnumerable<TValueType> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return Activator.CreateInstance<TValueType>();
        }
    }
}

