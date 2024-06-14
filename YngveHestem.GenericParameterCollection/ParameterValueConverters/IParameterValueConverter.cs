using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.ParameterValueConverters
{
	public interface IParameterValueConverter
	{
		/// <summary>
		/// Can this converter convert given source type to given target type?
		/// </summary>
		/// <param name="sourceType">The parameter type the parameter is saved as.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="rawValue">Current value to possibly convert later.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
		/// <param name="jsonSerializer">A JSON-serializer to use if conversion is needed. It is reccomended that this is used so everything serializes correct.</param>
		/// <returns></returns>
		bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer);

        /// <summary>
        /// Can this converter convert given source type to given target type?
        /// </summary>
        /// <param name="targetType">The parameter type the parameter will be saved as.</param>
        /// <param name="sourceType">The type to convert from.</param>
        /// <param name="value">The value to possibly convert.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <returns></returns>
        bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters);

        /// <summary>
        /// Convert parameter to value.
        /// </summary>
        /// <param name="sourceType">The parameter type the parameter is saved as.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="rawValue">The value of the parameter to convert.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <param name="jsonSerializer">A JSON-serializer to use when converting. It is reccomended that this is used so everything serializes correct.</param>
        /// <returns></returns>
        object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer);

        /// <summary>
        /// Convert value to parameter's value.
        /// </summary>
        /// <param name="targetType">The parameter type the parameter will be saved as.</param>
        /// <param name="sourceType">The type to convert from.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="customConverters">Any custom converters that might have been given.</param>
        /// <param name="jsonSerializer">A JSON-serializer to use when converting. It is reccomended that this is used so everything serializes correct.</param>
        /// <returns></returns>
        JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer);
    }
}

