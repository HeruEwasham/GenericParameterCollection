using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Editor
{
    public class EditorEngine
    {
        private EditorEngineOptions _options;

        public EditorEngine() : this(null) { }

        public EditorEngine(EditorEngineOptions options)
        {
            if (options == null)
            {
                _options = new EditorEngineOptions();
            }
            else
            {
                _options = options;
            }
        }

        public ParameterCollection CreateEditorParameterCollection(ParameterCollection parameters, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            var result = new ParameterCollection();
            var resultParameters = new List<ParameterCollection>();
            foreach (var parameter in parameters)
            {
                resultParameters.Add(CreateParameterAsEditableParameterCollection(parameter, _options.AdditionalInfoMaxRenderValue/*_options.AdditionalInfoMaxRenderValue*/, customConverters));
            }
            result.Add(_options.ParametersKey, resultParameters, typeof(List<ParameterCollection>), new ParameterCollection
            {
                { _options.DefaultValueKey, CreateParameterAsEditableParameterCollection(_options.DefaultParameter, 0, customConverters) }
            }, customConverters);
            return result;
        }

        private ParameterCollection CreateParameterAsEditableParameterCollection(Parameter parameter, int depthLeft, IEnumerable<IParameterValueConverter> customConverters)
        {
            if (depthLeft < 0)
            {
                return null;
            }
            var result = new ParameterCollection
            {
                {
                    _options.KeyKey, parameter.Key, typeof(string), new ParameterCollection
                    {
                        { _options.DescriptionOfParameterKey, _options.KeyInputDescription, null, customConverters }
                    },
                    customConverters
                },
                { _options.ParameterTypeKey, GetCorrectTypeName(parameter.Type), GetCorrectTypeNameList(), SetAdditionalInfoForParameterType(parameter, depthLeft-1, customConverters), customConverters }
            };
            if (depthLeft > 0)
            {
                result.Add(_options.AdditionalInfoKey, GetParametersAdditionalInfo(parameter, customConverters), new ParameterCollection
                    {
                        { _options.DefaultValueKey, CreateParameterAsEditableParameterCollection(_options.DefaultParameter, 0, customConverters) }
                    },
                    customConverters);
            }
            return result;
        }

        private List<ParameterCollection> GetParametersAdditionalInfo(Parameter parameter, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new List<ParameterCollection>();
            if (!parameter.HasAdditionalInfo())
            {
                return result;
            }
            foreach (var aInfoParameeter in parameter.GetAdditionalInfo())
            {
                if (parameter.Type == ParameterType.ParameterCollection_IEnumerable && aInfoParameeter.Key == _options.DefaultValueKey)
                {
                    continue;
                }
                var p = CreateParameterAsEditableParameterCollection(aInfoParameeter, _options.AdditionalInfoMaxRenderValue, customConverters);
                if (p != null)
                {
                    result.Add(p);
                }
            }
            return result;
        }

        private ParameterCollection SetAdditionalInfoForParameterType(Parameter parameter, int depthLeft, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new ParameterCollection
            {
                { _options.DescriptionOfParameterKey, _options.ParameterTypeInputDescription, null, customConverters }
            };
            foreach (var type in _options.SupportedTypes)
            {
                if (type == ParameterType.ParameterCollection)
                {
                    var aInfo = SetAdditionalInfoToValueParameter(customConverters);
                    var p = CreateParameterAsEditableParameterCollection(_options.DefaultParameter, depthLeft, customConverters);
                    if (p != null)
                    {
                        aInfo.Add(_options.DefaultValueKey, p, null, customConverters);
                    }
                    result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                    {
                        { _options.ValueKey, parameter.Type == type ? parameter.GetValue(type.GetDefaultValueTypeWithNullableTypes(), customConverters) : Array.Empty<ParameterCollection>(), ParameterType.ParameterCollection_IEnumerable, aInfo, customConverters }
                    },
                    null, customConverters);
                }
                else if (type == ParameterType.ParameterCollection_IEnumerable)
                {
                    if (_options.ParameterCollectionIEnumerablesBehavior == ParameterCollectionIEnumerablesBehavior.ShowParameterToSetDefaultValue)
                    {
                        var aInfo = new ParameterCollection
                        {
                            { _options.DescriptionOfParameterKey, _options.ParameterCollectionIEnumerableVisibleDefaultValueInputDescription}
                        };
                        if (parameter.HasAdditionalInfo() && parameter.GetAdditionalInfo().HasKeyAndCanConvertTo(_options.DefaultValueKey, typeof(ParameterCollection)))
                        {
                            var p = CreateParameterAsEditableParameterCollection(parameter.GetAdditionalInfo().GetParameterByKey(_options.DefaultValueKey), depthLeft, customConverters);
                            if (p != null)
                            {
                                aInfo.Add(_options.DefaultValueKey, p, null, customConverters);
                            }
                        }
                        else
                        {
                            var p = CreateParameterAsEditableParameterCollection(_options.DefaultParameter, depthLeft, customConverters);
                            if (p != null)
                            {
                                aInfo.Add(_options.DefaultValueKey, p, null, customConverters);
                            }
                        }
                        if (parameter.Type == ParameterType.ParameterCollection_IEnumerable)
                        {
                            var parameterValue = parameter.GetValue<ParameterCollection[]>(customConverters);
                            if (parameterValue != null && parameterValue.Length > 0)
                            {
                                aInfo.Add(EditorConstants.ExistingValueKey, parameterValue, null, customConverters);
                            }
                        }
                        
                        result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                        {
                            { _options.ParameterCollectionIEnumerableVisibleDefaultValueKey, parameter.Type == type ? parameter.GetValue(type.GetDefaultValueTypeWithNullableTypes(), customConverters) : Array.Empty<ParameterCollection>(), ParameterType.ParameterCollection_IEnumerable, aInfo, customConverters }
                        },
                        null, customConverters);
                    }
                }
                else if (type == ParameterType.Enum)
                {
                    result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                    {
                        { _options.EnumSelectKey, parameter.Type == ParameterType.Enum ? parameter.GetValue<ParameterCollection>().GetByKey<string>("type") : _options.SupportedEnumsToSelect.FirstOrDefault().GetType().AssemblyQualifiedName, _options.SupportedEnumsToSelect.Select(v => v.GetType().AssemblyQualifiedName), GetAdditionalInfoForEnums(parameter, customConverters), customConverters }
                    },
                    null, customConverters);
                }
                else if (type == ParameterType.SelectOne)
                {
                    result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                    {
                        {
                            _options.SelectChoicesKey, parameter.Type == ParameterType.SelectOne ? parameter.GetChoices() : Array.Empty<string>(), ParameterType.String_IEnumerable, new ParameterCollection
                            {
                                { _options.DescriptionOfParameterKey, _options.SelectChoicesInputDescription, null, customConverters }
                            },
                            customConverters
                        },
                        {
                            _options.SelectValueKey, parameter.Type == ParameterType.SelectOne ? parameter.GetValue<string>() : string.Empty, ParameterType.String_IEnumerable, new ParameterCollection
                            {
                                { _options.DescriptionOfParameterKey, _options.SelectOneValueInputDescription, null, customConverters }
                            },
                            customConverters
                        }
                    },
                    null, customConverters);
                }
                else if (type == ParameterType.SelectMany)
                {
                    result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                    {
                        {
                            _options.SelectChoicesKey, parameter.Type == ParameterType.SelectMany ? parameter.GetChoices() : Array.Empty<string>(), ParameterType.String_IEnumerable, new ParameterCollection
                            {
                                { _options.DescriptionOfParameterKey, _options.SelectChoicesInputDescription, null, customConverters }
                            },
                            customConverters
                        },
                        {
                            _options.SelectValueKey, parameter.Type == ParameterType.SelectMany ? parameter.GetValue<string[]>() : Array.Empty<string>(), ParameterType.String_IEnumerable, new ParameterCollection
                            {
                                { _options.DescriptionOfParameterKey, _options.SelectManyValueInputDescription, null, customConverters }
                            },
                            customConverters
                        }
                    },
                    null, customConverters);
                }
                else
                {
                    result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                    {
                        { _options.ValueKey, parameter.Type == type ? parameter.GetValue(type.GetDefaultValueTypeWithNullableTypes(), customConverters) : type.GetDefaultValue(), type, SetAdditionalInfoToValueParameter(customConverters), customConverters }
                    },
                    null, customConverters);
                }
            }
            return result;
        }

        private ParameterCollection GetAdditionalInfoForEnums(Parameter parameter, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new ParameterCollection();
            var parameterType = parameter.Type == ParameterType.Enum ? Type.GetType(parameter.GetValue<ParameterCollection>().GetByKey<string>("type")) : _options.SupportedEnumsToSelect[0].GetType();
            foreach (var type in _options.SupportedEnumsToSelect)
            {
                var t = type.GetType();
                result.Add(string.Format(_options.SelectableExtraParametersKey, t.AssemblyQualifiedName), new ParameterCollection
                {
                    {
                        _options.SelectValueKey, parameterType == t ? parameter.GetValue<string>(customConverters) : Enum.GetNames(t)[0], Enum.GetNames(t), new ParameterCollection
                        {
                            { _options.DescriptionOfParameterKey, _options.SelectEnumValueInputDescription, null, customConverters }
                        },
                        customConverters
                    }
                },
                null, customConverters);
            }
            return result;
        }

        private string GetCorrectTypeName(ParameterType type)
        {
            if (_options.PrettyPrintParameterTypes == null)
            {
                return type.ToString();
            }
            if (_options.PrettyPrintParameterTypes.ContainsKey(type))
            {
                return _options.PrettyPrintParameterTypes[type];
            }
            return type.ToString();
        }

        private List<string> GetCorrectTypeNameList()
        {
            var result = new List<string>();
            foreach (var type in _options.SupportedTypes)
            {
                result.Add(GetCorrectTypeName(type));
            }
            return result;
        }

        private ParameterCollection SetAdditionalInfoToValueParameter(IEnumerable<IParameterValueConverter> customConverters)
        {
            return new ParameterCollection
            {
                { _options.DescriptionOfParameterKey, _options.ValueInputDescription, null, customConverters }
            };
        }
    }
}
