using System;
using System.Collections.Generic;

namespace YngveHestem.GenericParameterCollection.Editor
{
    public class EditorEngineOptions
    {
        /// <summary>
        /// The key used for the parameters-value. Default is "Parameters".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the parameters-value. Default is \"Parameters\".")]
        public string ParametersKey = "Parameters";

        /// <summary>
        /// The key used for the key-value. Default is "Key".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the key-value. Default is \"Key\".")]
        public string KeyKey = "Key";

        /// <summary>
        /// Some fields have a description that can make the user understand a little more what it means. Give the key in AdditionalInfo that should be used to show this. Like for example like a tooltip. Default value is "desc".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "Some fields have a description that can make the user understand a little more what it means. Give the key in AdditionalInfo that should be used to show this. Like for example like a tooltip. Default value is \"desc\".")]
        public string DescriptionOfParameterKey = "desc";

        /// <summary>
        /// Some fields have a default value set so views know what to use if the user adds a new entry in a parameterCollection-list. Give the key in AdditionalInfo that should be used to show this. Default value is "defaultValue".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "Some fields have a default value set so views know what to use if the user adds a new entry in a parameterCollection-list. Give the key in AdditionalInfo that should be used to show this. Default value is \"defaultValue\".")]
        public string DefaultValueKey = "defaultValue";

        /// <summary>
        /// The key to use to define the different extra parameters that might show up only if that specified value is selected. Use {0} to specify the given value. The default value is "parametersIf:{0}".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key to use to define the different extra parameters that might show up only if that specified value is selected. Use {0} to specify the given value. The default value is \"parametersIf:{0}\".")]
        public string SelectableExtraParametersKey = "parametersIf:{0}";

        /// <summary>
        /// The key used for the parameter-type. Default is "Type".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the parameter-type. Default is \"Type\".")]
        public string ParameterTypeKey = "Type";

        /// <summary>
        /// The key used for the parameters value. Default is "Value".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the parameters value. Default is \"Value\".")]
        public string ValueKey = "Value";

        /// <summary>
        /// The key used for the parameters additional info. Default is "Additional info".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the parameters additional info. Default is \"Additional info\".")]
        public string AdditionalInfoKey = "Additional info";

        /// <summary>
        /// The key used for the parameters enum if enum is selected. Default is "Select type".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the parameters enum if enum is selected. Default is \"Select type\".")]
        public string EnumSelectKey = "Select type";

        /// <summary>
        /// The key used for the parameters value if Enum, SelectOne or SelectMany is selected. Default is "Selected value".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for the parameters value if Enum, SelectOne or SelectMany is selected. Default is \"Selected value\".")]
        public string SelectValueKey = "Selected value";

        /// <summary>
        /// The key used to add choices if SelectOne or SelectMany is selected. Default is "Choices".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used for to add choices if SelectOne or SelectMany is selected. Default is \"Choices\".")]
        public string SelectChoicesKey = "Choices";

        /// <summary>
        /// The key used on the parameter to write a default value when ParameterCollection_IEnumerable is selected. Default is "Default value".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The key used on the parameter to write a default value when ParameterCollection_IEnumerable is selected. Default is \"Default value\".")]
        public string ParameterCollectionIEnumerableVisibleDefaultValueKey = "Default value";

        /// <summary>
        /// The description of the key-parameter. Default is "The key used for this parameter.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the key-parameter. Default is \"The key used for this parameter.\".")]
        public string KeyInputDescription = "The key used for this parameter.";

        /// <summary>
        /// The description of the parameter-type-parameter. Default is "The type of value this parameter contain.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the parameter-type-parameter. Default is \"The type of value this parameter contain.\".")]
        public string ParameterTypeInputDescription = "The type of value this parameter contain.";

        /// <summary>
        /// The description of the value-parameter. Default is "The default value of this parameter.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the value-parameter. Default is \"The default value of this parameter.\".")]
        public string ValueInputDescription = "The default value of this parameter.";

        /// <summary>
        /// The description of the EnumSelect-parameter. Default is "The selected type.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the EnumSelect-parameter. Default is \"The selected type.\".")]
        public string EnumSelectInputDescription = "The selected type.";

        /// <summary>
        /// The description of the SelectValue-parameter when type is enum. Default is "The selected default value.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the SelectValue-parameter when type is enum. Default is \"The selected default value.\".")]
        public string SelectEnumValueInputDescription = "The selected default value.";

        /// <summary>
        /// The description of the SelectValue-parameter when type is SelectOne. Default is "The selected default value. This must be written exactly the same as the choice you want to use. If it is not exactly the same, somthing might go wrong. You can also decide to have an empty field to say that no choice is selected.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the SelectValue-parameter when type is SelectOne. Default is \"The selected default value. This must be written exactly the same as the choice you want to use. If it is not exactly the same, somthing might go wrong. You can also decide to have an empty field to say that no choice is selected.\".")]
        public string SelectOneValueInputDescription = "The selected default value. This must be written exactly the same as the choice you want to use. If it is not exactly the same, somthing might go wrong. You can also decide to have an empty field to say that no choice is selected.";

        /// <summary>
        /// The description of the SelectValue-parameter when type is SelectMany. Default is "The selected default value. This must be written exactly the same as the choice(s) you want to use. If it is not exactly the same, somthing might go wrong. You can also decide to have no choices selected.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the SelectValue-parameter when type is SelectMany. Default is \"The selected default value. This must be written exactly the same as the choice(s) you want to use. If it is not exactly the same, somthing might go wrong. You can also decide to have no choices selected.\".")]
        public string SelectManyValueInputDescription = "The selected default value. This must be written exactly the same as the choice(s) you want to use. If it is not exactly the same, somthing might go wrong. You can also decide to have no choices selected.";

        /// <summary>
        /// The description of the SelectChoices-parameter. Default is "The values you can select between.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the SelectChoices-parameter. Default is \"The values you can select between.\".")]
        public string SelectChoicesInputDescription = "The values you can select between.";

        /// <summary>
        /// The description of the ParameterCollectionIEnumerableVisibleDefaultValueKey-parameter. Default is "Here you can define how one ParameterCollection in this list should look like.".
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "The description of the ParameterCollectionIEnumerableVisibleDefaultValue-parameter. Default is \"Here you can define how one ParameterCollection in this list should look like.\".")]
        public string ParameterCollectionIEnumerableVisibleDefaultValueInputDescription = "Here you can define how one ParameterCollection in this list should look like.";

        /// <summary>
        /// Only enums supported will be in the list to choose from. It is expected that any enum-types that might be converted is in this list. Default, the list is null.
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "Only enums supported will be in the list to choose from. It is expected that any enum-types that might be converted is in this list. Default, the list is null.")]
        public List<Enum> SupportedEnumsToSelect = null;

        /// <summary>
        /// This will list up all the ParameterTypes the editor will support. The default list contains all the types. This can be handy if you for some reason do not want to allow editing/creating some types, like for instance enums.
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "This will list up all the ParameterTypes the editor will support. The default list contains all the types. This can be handy if you for some reason do not want to allow editing/creating some types, like for instance enums.")]
        public List<ParameterType> SupportedTypes = EditorConstants.AllParameterTypes;

        /// <summary>
        /// If you want to change the visible name of any ParameterType, like writing "Object" instead of "ParameterCollection", give an entry here. Default this is null.
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "If you want to change the visible name of any ParameterType, like writing \"Object\" instead of \"ParameterCollection\", give an entry here. Default this is null")]
        public Dictionary<ParameterType, string> PrettyPrintParameterTypes = null;

        /// <summary>
        /// How should an ParameeterCollectionIEnumerable-parameter behave? Should it let the user define the default value simply by a parameter, or should the user need to add it directly in the additional-info-part of the parameter. Mark that the parameter to add default value will not override if the user adds a parameter manually to the additional info. But if it already exist a defaultValue-parameter, this will be used as the value for that parameter and not be shown in the additionalInfo. If the default value should be manually added, it will bee shown in additional info if exist as normal.
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "How should an ParameeterCollectionIEnumerable-parameter behave? Should it let the user define the default value simply by a parameter, or should the user need to add it directly in the additional-info-part of the parameter. Mark that the parameter to add default value will not override if the user adds a parameter manually to the additional info. But if it already exist a defaultValue-parameter, this will be used as the value for that parameter and not be shown in the additionalInfo. If the default value should be manually added, it will bee shown in additional info if exist as normal.")]
        public ParameterCollectionIEnumerablesBehavior ParameterCollectionIEnumerablesBehavior = ParameterCollectionIEnumerablesBehavior.ShowParameterToSetDefaultValue;

        /// <summary>
        /// How many layers would you want to max give an additional info. Default is 3.
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "How many layers would you want to max give an additional info. Default is 3.")]
        public int AdditionalInfoMaxRenderValue = 3;

        /// <summary>
        /// Do you want to let users add AdditionalInfo? Default is true.
        /// </summary>
        [ParameterProperty]
        [AdditionalInfo("desc", "Do you want to let users add AdditionalInfo? Default is true.")]
        public bool ShowAdditionalInfo = true;

        /// <summary>
        /// Select the default parameter. This parameter will be used when a new entry of a parameter is created.
        /// </summary>
        public Parameter DefaultParameter = new Parameter(string.Empty, string.Empty, ParameterType.String);
    }
}
