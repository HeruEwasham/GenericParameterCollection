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

		public bool ParameterTypeIsSet { get; private set; } = false;

		private ParameterType _parameterType;

		public AdditionalInfoAttribute(string key, object value)
		{
			Key = key;
			Value = value;
		}
	}
}

