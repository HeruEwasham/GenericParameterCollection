using System;
namespace YngveHestem.GenericParameterCollection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class AttributeConvertibleAttribute : Attribute
	{
		public ParameterType ParameterType { get; set; } = ParameterType.ParameterCollection;
	}
}

