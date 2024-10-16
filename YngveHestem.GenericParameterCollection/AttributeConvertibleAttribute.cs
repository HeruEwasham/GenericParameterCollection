using System;
namespace YngveHestem.GenericParameterCollection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class AttributeConvertibleAttribute : Attribute
	{
		public ParameterType ParameterType { get; set; } = ParameterType.ParameterCollection;

		/// <summary>
		/// What should be done if some parts might want a defaultValue of this object.
		/// </summary>
		public DefaultValueHandling DefaultValueHandling = DefaultValueHandling.InitializeNewObject;

		/// <summary>
		/// If wanting a default value set, which key should be used if set in an additionalInfo.
		/// </summary>
		public string DefaultValueKey = "defaultValue";

		/// <summary>
		/// If initializing a new object as DefaultValue, are there any arguments needed.
		/// </summary>
		public object[] DefaultValueArguments = null;
	}
}

