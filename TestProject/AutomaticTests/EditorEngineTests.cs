using NUnit.Framework;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.Editor;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.AutomaticTests
{
    [TestFixture]
    public class EditorEngineTests
    {
        private EditorEngine _engine;
        private IEnumerable<IParameterValueConverter> _converters;

        [SetUp]
        public void Setup()
        {
            _engine = new EditorEngine(new EditorEngineOptions { SupportedEnumsToSelect = new List<Enum> {DayOfWeek.Monday}});
            _converters = null;
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_EmptyCollection_ReturnsEmptyJson()
        {
            var original = new ParameterCollection();
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_StringParameter_RoundTripJson()
        {
            var original = new ParameterCollection {
                new Parameter("strKey", "hello", ParameterType.String)
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_IntParameter_RoundTripJson()
        {
            var original = new ParameterCollection {
                new Parameter("intKey", 42, ParameterType.Int)
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_EnumParameter_RoundTripJson()
        {
            /*var enumParam = new ParameterCollection {
                { "type", typeof(DayOfWeek).AssemblyQualifiedName, ParameterType.String },
                { "value", "Friday", ParameterType.String },
                { "choices", Enum.GetNames(typeof(DayOfWeek)), ParameterType.String_IEnumerable }
            };*/
            var original = new ParameterCollection {
                new Parameter("enumKey", DayOfWeek.Friday, ParameterType.Enum)
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_SelectOneParameter_RoundTripJson()
        {
            var selectOneParam = new ParameterCollection {
                { "value", "A", ParameterType.String },
                { "choices", new[] { "A", "B", "C" }, ParameterType.String_IEnumerable }
            };
            var original = new ParameterCollection {
                new Parameter("selectOneKey", selectOneParam, ParameterType.SelectOne)
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_SelectManyParameter_RoundTripJson()
        {
            var selectManyParam = new ParameterCollection {
                { "value", new[] { "A", "C" }, ParameterType.String_IEnumerable },
                { "choices", new[] { "A", "B", "C" }, ParameterType.String_IEnumerable }
            };
            var original = new ParameterCollection {
                new Parameter("selectManyKey", selectManyParam, ParameterType.SelectMany)
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_ParameterCollection_RoundTripJson()
        {
            var nested = new ParameterCollection {
                new Parameter("innerKey", "innerValue", ParameterType.String)
            };
            var original = new ParameterCollection {
                new Parameter("collKey", nested, ParameterType.ParameterCollection)
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }

        [Test]
        public void ConvertEditorParameterCollectionToNormal_ParameterCollection_IEnumerable_RoundTripJson()
        {
            // We make a ParameterCollection_IEnumerable that might not be exactly as wanted (the keys are different). But that will make it easier to spot unwanted differences in test...)
            var nestedList = new List<ParameterCollection> {
                new ParameterCollection { new Parameter("item1", 1, ParameterType.Int) },
                new ParameterCollection { new Parameter("item2", 2, ParameterType.Int) }
            };
            var original = new ParameterCollection {
                new Parameter("collEnumKey", nestedList, ParameterType.ParameterCollection_IEnumerable, new ParameterCollection
                {
                    {
                        "defaultValue", new ParameterCollection
                        {
                            new Parameter("itemX", 1, ParameterType.Int)
                        }
                    }
                })
            };
            var editor = _engine.CreateEditorParameterCollection(original, _converters);
            var converted = _engine.ConvertEditorParameterCollectionToNormal(editor, _converters);
            Assert.That(converted.ToJson(), Is.EqualTo(original.ToJson()));
        }
    }
}
