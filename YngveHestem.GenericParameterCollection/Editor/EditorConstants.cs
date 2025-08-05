using System;
using System.Collections.Generic;

namespace YngveHestem.GenericParameterCollection.Editor
{
    public static class EditorConstants
    {
        public static List<ParameterType> AllParameterTypes = new List<ParameterType>
        {
            ParameterType.Int,
            ParameterType.Decimal,
            ParameterType.String,
            ParameterType.String_Multiline,
            ParameterType.Bytes,
            ParameterType.Bool,
            ParameterType.DateTime,
            ParameterType.Date,
            ParameterType.Int_IEnumerable,
            ParameterType.Decimal_IEnumerable,
            ParameterType.String_IEnumerable,
            ParameterType.String_Multiline_IEnumerable,
            ParameterType.Bool_IEnumerable,
            ParameterType.DateTime_IEnumerable,
            ParameterType.Date_IEnumerable,
            ParameterType.ParameterCollection,
            ParameterType.ParameterCollection_IEnumerable,
            ParameterType.Enum,
            ParameterType.SelectOne,
            ParameterType.SelectMany
        };

        public static List<ParameterType> AllParameterTypesExceptEnum = new List<ParameterType>
        {
            ParameterType.Int,
            ParameterType.Decimal,
            ParameterType.String,
            ParameterType.String_Multiline,
            ParameterType.Bytes,
            ParameterType.Bool,
            ParameterType.DateTime,
            ParameterType.Date,
            ParameterType.Int_IEnumerable,
            ParameterType.Decimal_IEnumerable,
            ParameterType.String_IEnumerable,
            ParameterType.String_Multiline_IEnumerable,
            ParameterType.Bool_IEnumerable,
            ParameterType.DateTime_IEnumerable,
            ParameterType.Date_IEnumerable,
            ParameterType.ParameterCollection,
            ParameterType.ParameterCollection_IEnumerable,
            ParameterType.SelectOne,
            ParameterType.SelectMany
        };

        public const string ExistingValueKey = "EditorEngine_ExistingValue";
    }
}
