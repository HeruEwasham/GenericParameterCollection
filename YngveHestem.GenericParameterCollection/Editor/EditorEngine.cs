using System;
using System.Collections.Generic;
using System.Linq;
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
            if (parameters == null)
            {
                parameters = new ParameterCollection();
            }
            var result = new ParameterCollection();
            var resultParameters = new List<ParameterCollection>();
            foreach (var parameter in parameters)
            {
                resultParameters.Add(CreateParameterAsEditableParameterCollection(parameter, _options.AdditionalInfoMaxRenderValue, customConverters));
            }
            result.Add(_options.ParametersKey, resultParameters, typeof(List<ParameterCollection>), new ParameterCollection
            {
                { _options.DefaultValueKey, CreateParameterAsEditableParameterCollection(_options.DefaultParameter, _options.AdditionalInfoMaxRenderValue, customConverters) }
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
            if (depthLeft > 0 && _options.ShowAdditionalInfo)
            {
                result.Add(_options.AdditionalInfoKey, GetParametersAdditionalInfo(parameter, depthLeft, customConverters), new ParameterCollection
                    {
                        { _options.DefaultValueKey, CreateParameterAsEditableParameterCollection(_options.DefaultParameter, depthLeft - 1, customConverters) }
                    },
                    customConverters);
            }
            return result;
        }

        private List<ParameterCollection> GetParametersAdditionalInfo(Parameter parameter, int depthLeft, IEnumerable<IParameterValueConverter> customConverters)
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
                var p = CreateParameterAsEditableParameterCollection(aInfoParameeter, depthLeft - 1, customConverters);
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
                        { _options.ValueKey, parameter.Type == type ? GetAsParameterEditableParameterCollection_IEnumerable(parameter.GetValue<ParameterCollection>(customConverters), customConverters) : Array.Empty<ParameterCollection>(), ParameterType.ParameterCollection_IEnumerable, aInfo, customConverters }
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
                        ParameterCollection p = null;
                        if (parameter.HasAdditionalInfo() && parameter.GetAdditionalInfo().HasKeyAndCanConvertTo(_options.DefaultValueKey, typeof(ParameterCollection)))
                        {
                            p = CreateParameterAsEditableParameterCollection(parameter.GetAdditionalInfo().GetParameterByKey(_options.DefaultValueKey), depthLeft - 1, customConverters);
                        }
                        else
                        {
                            p = CreateParameterAsEditableParameterCollection(_options.DefaultParameter, depthLeft - 1, customConverters);
                        }
                        if (p != null)
                        {
                            aInfo.Add(_options.DefaultValueKey, p, null, customConverters);
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
                            { _options.ParameterCollectionIEnumerableVisibleDefaultValueKey, parameter.Type == type ? GetAsParameterEditableParameterCollection_IEnumerable(parameter.HasAdditionalInfo() && parameter.GetAdditionalInfo().HasKey(_options.DefaultValueKey) ? parameter.GetAdditionalInfo().GetByKey<ParameterCollection>(_options.DefaultValueKey) : parameter.GetValue<ParameterCollection[]>(customConverters).FirstOrDefault(), customConverters) : Array.Empty<ParameterCollection>(), ParameterType.ParameterCollection_IEnumerable, aInfo, customConverters }
                        },
                        null, customConverters);
                    }
                }
                else if (type == ParameterType.Enum)
                {
                    result.Add(string.Format(_options.SelectableExtraParametersKey, GetCorrectTypeName(type)), new ParameterCollection
                    {
                        { _options.EnumSelectKey, parameter.Type == ParameterType.Enum ? parameter.GetValue<ParameterCollection>().GetByKey<string>("type") : _options.SupportedEnumsToSelect.FirstOrDefault().GetType().FullName, _options.SupportedEnumsToSelect.Select(v => v.GetType().FullName), GetAdditionalInfoForEnums(parameter, customConverters), customConverters }
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
                            _options.SelectValueKey, parameter.Type == ParameterType.SelectOne ? parameter.GetValue<string>() : string.Empty, ParameterType.String, new ParameterCollection
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

        private ParameterCollection[] GetAsParameterEditableParameterCollection_IEnumerable(ParameterCollection parameters, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new List<ParameterCollection>();
            foreach (var parameter in parameters)
            {
                result.Add(CreateParameterAsEditableParameterCollection(parameter, _options.AdditionalInfoMaxRenderValue, customConverters));
            }
            return result.ToArray();
        }

        private ParameterCollection GetAdditionalInfoForEnums(Parameter parameter, IEnumerable<IParameterValueConverter> customConverters)
        {
            var result = new ParameterCollection();
            var parameterType = parameter.Type == ParameterType.Enum ? Type.GetType(parameter.GetValue<ParameterCollection>().GetByKey<string>("type")) : _options.SupportedEnumsToSelect[0].GetType();
            foreach (var type in _options.SupportedEnumsToSelect)
            {
                var t = type.GetType();
                result.Add(string.Format(_options.SelectableExtraParametersKey, t.FullName), new ParameterCollection
                {
                    {
                        _options.SelectValueKey, parameter.Type == ParameterType.Enum && parameterType == t ? parameter.GetValue<string>(customConverters) : Enum.GetNames(t)[0], Enum.GetNames(t), new ParameterCollection
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

        /// <summary>
        /// Converts an editor-ParameterCollection (from CreateEditorParameterCollection) back to a normal ParameterCollection.
        /// </summary>
        /// <param name="editorCollection">The ParameterCollection from editor-mode.</param>
        /// <param name="customConverters">Any custom converters to use.</param>
        /// <returns>A new ParameterCollection.</returns>
        public ParameterCollection ConvertEditorParameterCollectionToNormal(ParameterCollection editorCollection, IEnumerable<IParameterValueConverter> customConverters = null)
        {
            var result = new ParameterCollection();
            var parametersList = editorCollection.GetByKey<List<ParameterCollection>>(_options.ParametersKey);
            foreach (var editorParam in parametersList)
            {
                var originalParam = ConvertEditorParameterToNormal(editorParam, customConverters);
                if (originalParam != null)
                {
                    result.Add(originalParam);
                }
            }
            return result;
        }

        private Parameter ConvertEditorParameterToNormal(ParameterCollection editorParam, IEnumerable<IParameterValueConverter> customConverters)
        {
            // Get key
            var key = editorParam.GetByKey<string>(_options.KeyKey);

            // Get type
            var typeName = editorParam.GetByKey<string>(_options.ParameterTypeKey);
            var type = GetParameterTypeFromName(typeName);

            //Get additionalInfo if exists
            ParameterCollection additionalInfo = null;
            if (editorParam.HasKey(_options.AdditionalInfoKey))
            {
                additionalInfo = new ParameterCollection();
                foreach (var item in editorParam.GetByKey<ParameterCollection[]>(_options.AdditionalInfoKey))
                {
                    additionalInfo.Add(ConvertEditorParameterToNormal(item, customConverters));
                }
            }

            // Find value
                object value = null;
            ParameterType valueType = type;
            if (type == ParameterType.ParameterCollection)
            {
                var valueEditorList = GetValueParameterCollectionParameterList(editorParam, _options.ValueKey, type, customConverters);
                var list = new ParameterCollection();
                foreach (var item in valueEditorList.Item1)
                {
                    list.Add(ConvertEditorParameterToNormal(item, customConverters));
                }
                value = list;
            }
            else if (type == ParameterType.ParameterCollection_IEnumerable)
            {
                var valueEditorList = GetValueParameterCollectionParameterList(editorParam, _options.ParameterCollectionIEnumerableVisibleDefaultValueKey, type, customConverters);
                var list = new ParameterCollection();
                foreach (var item in valueEditorList.Item1)
                {
                    list.Add(ConvertEditorParameterToNormal(item, customConverters));
                }
                value = list;
                if (additionalInfo == null)
                {
                    additionalInfo = new ParameterCollection();
                }
                if (!additionalInfo.HasKey(_options.DefaultValueKey))
                {
                    additionalInfo.Add(_options.DefaultValueKey, value, null, customConverters);
                }
                var currentValue = Array.Empty<ParameterCollection>();
                if (valueEditorList.Item2.HasKey(EditorConstants.ExistingValueKey))
                {
                    currentValue = valueEditorList.Item2.GetByKey<ParameterCollection[]>(EditorConstants.ExistingValueKey);
                }
                return new Parameter(key, currentValue, type, additionalInfo, null, customConverters);
            }
            else if (type == ParameterType.Enum)
            {
                var valueParam = GetValuesParameterCollection(editorParam, type, customConverters);
                var enumTypeName = valueParam.GetByKey<string>(_options.EnumSelectKey);
                var specifiedEnumValues = valueParam.GetParameterByKey(_options.EnumSelectKey).GetAdditionalInfo().GetByKey<ParameterCollection>(string.Format(_options.SelectableExtraParametersKey,  enumTypeName), customConverters);
                var enumValue = specifiedEnumValues.GetByKey<string>(_options.SelectValueKey);
                var enumChoices = specifiedEnumValues.GetParameterByKey(_options.SelectValueKey).GetChoices();
                var enumParamCollection = new ParameterCollection
                {
                    { "value", enumValue, ParameterType.String, null, customConverters },
                    { "type", enumTypeName, ParameterType.String, null, customConverters },
                    { "choices", enumChoices, ParameterType.String_IEnumerable, null, customConverters }
                };
                value = enumParamCollection;
            }
            else if (type == ParameterType.SelectOne)
            {
                var valueParam = GetValuesParameterCollection(editorParam, type, customConverters);
                var selectedValue = valueParam.GetByKey<string>(_options.SelectValueKey);
                var choices = valueParam.GetByKey<string[]>(_options.SelectChoicesKey);
                var selectOneParamCollection = new ParameterCollection
                {
                    { "value", selectedValue, ParameterType.String, null, customConverters },
                    { "choices", choices, ParameterType.String_IEnumerable, null, customConverters }
                };
                value = selectOneParamCollection;
            }
            else if (type == ParameterType.SelectMany)
            {
                var valueParam = GetValuesParameterCollection(editorParam, type, customConverters);
                var selectedValues = valueParam.GetByKey<string[]>(_options.SelectValueKey);
                var choices = valueParam.GetByKey<string[]>(_options.SelectChoicesKey);
                var selectManyParamCollection = new ParameterCollection
                {
                    { "value", selectedValues, ParameterType.String_IEnumerable, null, customConverters },
                    { "choices", choices, ParameterType.String_IEnumerable, null, customConverters }
                };
                value = selectManyParamCollection;
            }
            else
            {
                // Value is a simple value
                value = GetSimpleValue(editorParam, type, customConverters);
            }

            // Make Parameter
            return new Parameter(key, value, valueType, additionalInfo, null, customConverters);
        }

        // Get ParameterType from name
        private ParameterType GetParameterTypeFromName(string typeName)
        {
            foreach (var t in _options.SupportedTypes)
            {
                if (GetCorrectTypeName(t) == typeName)
                {
                    return t;
                }
            }
            // fallback
            return ParameterType.String;
        }

        private Tuple<List<ParameterCollection>, ParameterCollection> GetValueParameterCollectionParameterList(ParameterCollection editorParam, string valueKey, ParameterType type, IEnumerable<IParameterValueConverter> customConverters)
        {
            var valueParam = GetValuesParameterCollection(editorParam, type, customConverters);
            if (valueParam.HasKey(valueKey))
            {
                return new Tuple<List<ParameterCollection>, ParameterCollection>(valueParam.GetByKey<List<ParameterCollection>>(valueKey), valueParam.GetParameterByKey(valueKey).GetAdditionalInfo());
            }
            return null;
        }

        private object GetSimpleValue(ParameterCollection editorParam, ParameterType type, IEnumerable<IParameterValueConverter> customConverters)
        {
            var valueParam = GetValuesParameterCollection(editorParam, type, customConverters);
            if (valueParam.HasKey(_options.ValueKey))
            {
                return valueParam.GetByKey(_options.ValueKey, type.GetDefaultValueType(), customConverters);
            }
            return null;
        }

        private ParameterCollection GetValuesParameterCollection(ParameterCollection editorParam, ParameterType type, IEnumerable<IParameterValueConverter> customConverters)
        {
            return editorParam.GetParameterByKey(_options.ParameterTypeKey).GetAdditionalInfo().GetByKey<ParameterCollection>(string.Format(_options.SelectableExtraParametersKey, Enum.GetName(typeof(ParameterType), type)), customConverters);
        }
    }
}
