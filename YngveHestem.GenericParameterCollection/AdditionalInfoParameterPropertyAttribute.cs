using System;
namespace YngveHestem.GenericParameterCollection
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class AdditionalInfoParameterPropertyAttribute : Attribute
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
        /// Which parameter's AdditionalInfo should this parameter be added to.
        /// </summary>
        public string AdditionalInfoParameterKey { get; }

        /// <summary>
		/// If set to true, this value should be used on the same key if the key already exists in given AdditionalInfo.
		/// </summary>
		public bool OverrideIfKeyExist { get; set; } = true;

        /// <summary>
		/// If set to true, the key will be divided up to possible ParameterCollections in ParameterCollections based on the KeyPathDivider.
		/// </summary>
		public bool KeyIsPath { get; set; } = false;

		/// <summary>
		/// If key is a path to a sub-AdditionalInfo, what is the divider between the key-parts.
		/// </summary>
		public string KeyPathDivider { get; set; } = ".";

        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="additionalInfoParameterKey">The key to the parameter that this parameter should be added to their additionalInfo.</param>
        public AdditionalInfoParameterPropertyAttribute(string additionalInfoParameterKey)
        {
            Key = null;
            ParameterType = null;
            AdditionalInfoParameterKey = additionalInfoParameterKey;
        }
        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="additionalInfoParameterKey">The key to the parameter that this parameter should be added to their additionalInfo.</param>
        /// <param name="key">The key the parameter should be given.</param>
        public AdditionalInfoParameterPropertyAttribute(string additionalInfoParameterKey, string key)
		{
			Key = key;
            ParameterType = null;
            AdditionalInfoParameterKey = additionalInfoParameterKey;
		}

        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="additionalInfoParameterKey">The key to the parameter that this parameter should be added to their additionalInfo.</param>
        /// <param name="key">The key the parameter should be given.</param>
        /// <param name="parameterType">You can specify which ParameterType to use. If this is not given, the system will try to determine the best type.</param>
        public AdditionalInfoParameterPropertyAttribute(string additionalInfoParameterKey, string key, ParameterType parameterType)
        {
            Key = key;
            ParameterType = parameterType;
            AdditionalInfoParameterKey = additionalInfoParameterKey;
        }

        /// <summary>
        /// Indicate that the given field/property can be converted to a parameter with given attributes.
        /// </summary>
        /// <param name="additionalInfoParameterKey">The key to the parameter that this parameter should be added to their additionalInfo.</param>
        /// <param name="parameterType">You can specify which ParameterType to use. If this is not given, the system will try to determine the best type.</param>
        public AdditionalInfoParameterPropertyAttribute(string additionalInfoParameterKey, ParameterType parameterType)
        {
            Key = null;
            ParameterType = parameterType;
            AdditionalInfoParameterKey = additionalInfoParameterKey;
        }
    }
}

