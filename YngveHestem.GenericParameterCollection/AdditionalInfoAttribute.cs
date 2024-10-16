using System;
namespace YngveHestem.GenericParameterCollection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class AdditionalInfoAttribute : Attribute
	{
		public string Key { get; }

		public object Value { get; }

		/// <summary>
		/// If set to true, this value should be used on the same key if the key already exists in given AdditionalInfo.
		/// </summary>
		public bool OverrideIfKeyExist { get; set; } = false;

		/// <summary>
		/// This can be set to a value to define what ParameterType the given AdditionalInfo-parameter's value is. If not set, the value's type is used to decide. Mark that this only is used if the key don't already exists.
		/// </summary>
		public ParameterType ParameterType 
		{
			get
			{
				return _parameterType;
			}
			set
			{
				_parameterType = value;
				ParameterTypeIsSet = true;
			}
		}

		/// <summary>
		/// If set to true, the key will be divided up to possible ParameterCollections in ParameterCollections based on the KeyPathDivider.
		/// </summary>
		public bool KeyIsPath { get; set; } = false;

		/// <summary>
		/// If key is a path to a sub-AdditionalInfo, what is the dividder between the key-parts.
		/// </summary>
		public string KeyPathDivider { get; set; } = ".";

		public bool ParameterTypeIsSet { get; private set; } = false;

		private ParameterType _parameterType;

		/// <summary>
		/// Defines an additional info entry with the given key and value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public AdditionalInfoAttribute(string key, object value)
		{
			Key = key;
			Value = value;
		}

		/// <summary>
		/// Defines an additional info entry with the given key. The value will be an instance of the created type.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="typeToCreate">The type you want to have an instance of. Remember that this type must be able to be converted when conversion happens.</param>
		/// <param name="arguments">The arguments (if any) of the constructor you want to use.</param>
		public AdditionalInfoAttribute(string key, Type typeToCreate, params object[] arguments)
		{
			Key = key;
			Value = Activator.CreateInstance(typeToCreate, arguments);
		}
	}
}

