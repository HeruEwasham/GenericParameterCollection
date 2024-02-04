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
        /// Can this converter convert this list of ParameterCollection. The default implementation checks that all list-items validates sucessfully and that the list itself is not null.
        /// </summary>
        /// <param name="value">The list of ParameterCollection to possibly convert later.</param>
        /// <returns></returns>
        protected virtual bool CanConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value)
        {
            if (value == null)
            {
                return false;
            }

            foreach(var item in value)
            {
                if (!CanConvertFromParameterCollection(item))
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
        /// <returns></returns>
        protected abstract bool CanConvertToParameterCollection(TValueType value);

        /// <summary>
        /// Can this value be converted to a list of ParameterCollection by this converter. The default implementation checks that all list-items validates sucessfully and that the list itself is not null.
        /// </summary>
        /// <param name="value">The list of values to possibly convert later.</param>
        /// <returns></returns>
        protected virtual bool CanConvertToListOfParameterCollection(IEnumerable<TValueType> value)
        {
            if (value == null)
            {
                return false;
            }

            foreach (var item in value)
            {
                if (!CanConvertToParameterCollection(item))
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
        /// <returns></returns>
        protected abstract TValueType ConvertFromParameterCollection(ParameterCollection value);

        /// <summary>
        /// Convert list of ParameterCollection to a list of wanted type. The default implementation goes through each element in the list and converts each element before returning the result as a list.
        /// </summary>
        /// <param name="value">The list of ParameterCollection to convert.</param>
        /// <returns></returns>
        protected virtual IEnumerable<TValueType> ConvertFromListOfParameterCollection(IEnumerable<ParameterCollection> value)
        {
            var result = new List<TValueType>();

            foreach(var item in value)
            {
                result.Add(ConvertFromParameterCollection(item));
            }

            return result;
        }

        /// <summary>
        /// Convert value to ParameterCollection.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        protected abstract ParameterCollection ConvertToParameterCollection(TValueType value);

        /// <summary>
        /// Convert list of values to list of ParameterCollection. The default implementation goes through each element in the list and converts each element before returning the result as a list.
        /// </summary>
        /// <param name="value">The list of values to convert.</param>
        /// <returns></returns>
        protected virtual IEnumerable<ParameterCollection> ConvertToListOfParameterCollection(IEnumerable<TValueType> value)
        {
            var result = new List<ParameterCollection>();

            foreach (var item in value)
            {
                result.Add(ConvertToParameterCollection(item));
            }

            return result;
        }
    }
}

