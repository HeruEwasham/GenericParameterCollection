using System;
namespace YngveHestem.GenericParameterCollection
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class ParameterPropertyAttribute : Attribute
	{
		/// <summary>
		/// The key the parameter should be given.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// An optionally given ParameterType. If null the system will try to get the type based on the field/property type. If set, the given parameterType will be given.
		/// </summary>
		public ParameterType? ParameterType { get; }

        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="key">The key the parameter should be given.</param>
        public ParameterPropertyAttribute()
        {
            Key = null;
            ParameterType = null;
        }
        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="key">The key the parameter should be given.</param>
        public ParameterPropertyAttribute(string key)
		{
			Key = key;
            ParameterType = null;
		}

        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="key">The key the parameter should be given.</param>
        /// <param name="parameterType">You can specify which ParameterType to use. If this is not given, the system will try to determine the best type.</param>
        public ParameterPropertyAttribute(string key, ParameterType parameterType)
        {
            Key = key;
            ParameterType = parameterType;
        }

        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="parameterType">You can specify which ParameterType to use. If this is not given, the system will try to determine the best type.</param>
        public ParameterPropertyAttribute(ParameterType parameterType)
        {
            Key = null;
            ParameterType = parameterType;
        }
    }
}

