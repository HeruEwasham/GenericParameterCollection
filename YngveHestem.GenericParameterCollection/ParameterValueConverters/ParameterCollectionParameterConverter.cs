using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
    public abstract class ParameterCollectionParameterConverter<TValueType> : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.ParameterCollection && targetType == typeof(TValueType))
            {
                return CanConvertFromParameterCollection(rawValue.ToObject<ParameterCollection>(jsonSerializer));
            }
            else if (sourceType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable<TValueType>).IsAssignableFrom(targetType))
            {
                return CanConvertFromListOfParameterCollection(rawValue.ToObject<IEnumerable<ParameterCollection>>(jsonSerializer));
            }
            else
            {
                return false;
            }
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value)
        {
            if (targetType == ParameterType.ParameterCollection && sourceType == typeof(TValueType))
            {
                return CanConvertToParameterCollection((TValueType)value);
            }
            else if (targetType == ParameterType.ParameterCollection_IEnumerable && typeof(IEnumerable<TValueType>).IsAssignableFrom(sourceType))
            {
                return CanConvertToListOfParameterCollection((IEnumerable<TValueType>)value);
            }
            else
            {
                return false;
            }
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, JsonSerializer jsonSerializer)
        {
            if (sourceType == ParameterType.ParameterCollection)
            {
                try
                {
                    return ConvertFromParameterCollection(rawValue.ToObject<ParameterCollection>());
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(TValueType).Name + " by " + this.GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }
            else if (sourceType == ParameterType.ParameterCollection_IEnumerable)
            {
                try
                {
                    return ConvertFromListOfParameterCollection(rawValue.ToObject<IEnumerable<ParameterCollection>>()).ToCorrectIEnumerable(targetType);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(IEnumerable<TValueType>).Name + " by " + this.GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + this.GetType().Name);
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, JsonSerializer jsonSerializer)
        {
            if (targetType == ParameterType.ParameterCollection)
            {
                try
                {
                    return JToken.FromObject(ConvertToParameterCollection((TValueType)value), jsonSerializer);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(TValueType).Name + " by " + this.GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }
            else if (targetType == ParameterType.ParameterCollection_IEnumerable)
            {
                try
                {
                    return JToken.FromObject(ConvertToListOfParameterCollection((IEnumerable<TValueType>)value), jsonSerializer);
                }
                catch (Exception e)
                {
                    throw new Exception("Got exception when trying to convert parameterCollection to " + typeof(IEnumerable<TValueType>).Name + " by " + this.GetType().Name + ". Exception message was: " + e.Message, e);
                }
            }

            throw new ArgumentException("The values was not supported to be converted by " + this.GetType().Name);
        }

        /// <summary>
        /// Can this converter convert this ParameterCollection.
        /// </summary>
        /// <param name="value">The ParameterCollection to possibly convert later.</param>
        /// <returns></returns>
        protected abstract bool CanConvertFromParameterCollection(ParameterCollection value);

        /// <summary>
        /// Can this converter convert this list of ParameterCollection.
        /// </summary>
        /// <param name="value">The list of ParameterCollection to possibly convert later.</param>
        /// <returns></returns>
        protected abstract bool CanConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value);

        /// <summary>
        /// Can this value be converted to a ParameterCollection by this converter.
        /// </summary>
        /// <param name="value">The value to possibly convert later.</param>
        /// <returns></returns>
        protected abstract bool CanConvertToParameterCollection(TValueType value);

        /// <summary>
        /// Can this value be converted to a list of ParameterCollection by this converter.
        /// </summary>
        /// <param name="value">The list of values to possibly convert later.</param>
        /// <returns></returns>
        protected abstract bool CanConvertToListOfParameterCollection(IEnumerable<TValueType> value);

        /// <summary>
        /// Convert ParameterCollection to wanted type.
        /// </summary>
        /// <param name="value">The ParameterCollection to convert.</param>
        /// <returns></returns>
        protected abstract TValueType ConvertFromParameterCollection(ParameterCollection value);

        /// <summary>
        /// Convert list of ParameterCollection to a list of wanted type.
        /// </summary>
        /// <param name="value">The list of ParameterCollection to convert.</param>
        /// <returns></returns>
        protected abstract IEnumerable<TValueType> ConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value);

        /// <summary>
        /// Convert value to ParameterCollection.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        protected abstract ParameterCollection ConvertToParameterCollection(TValueType value);

        /// <summary>
        /// Convert list of values to list of ParameterCollection.
        /// </summary>
        /// <param name="value">The list of values to convert.</param>
        /// <returns></returns>
        protected abstract IEnumerable<ParameterCollection> ConvertToListOfParameterCollection(IEnumerable<TValueType> value);
    }
}

