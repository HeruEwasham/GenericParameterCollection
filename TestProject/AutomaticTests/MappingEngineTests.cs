using NUnit.Framework;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests
{
    [TestFixture]
    public class MappingEngineTests
    {
        [Test]
        public void Map_SimplePaths_ReturnsScalarValues()
        {
            var source = new ParameterCollection
            {
                { "id", 123 },
                { "name", "Alice" },
                { "nested", new ParameterCollection { { "city", "Oslo" } } }
            };

            var mapping = new ParameterCollection
            {
                { "userId", "id" },
                { "userName", "name" },
                { "userCity", "nested.city" }
            };

            var result = PathMappingEngine.Map(source, mapping);

            Assert.That(result.GetByKey<string>("userId"), Is.EqualTo("123"));
            Assert.That(result.GetByKey<string>("userName"), Is.EqualTo("Alice"));
            Assert.That(result.GetByKey<string>("userCity"), Is.EqualTo("Oslo"));
        }

        [Test]
        public void Map_ListTemplate_ReturnsRepeatedItems()
        {
            var source = new ParameterCollection();
            var alice = new ParameterCollection { { "id", 1 }, { "name", "Alice" } };
            var bob = new ParameterCollection { { "id", 2 }, { "name", "Bob" } };
            source.Add("people", new[] { alice, bob }, ParameterType.ParameterCollection_IEnumerable);

            var rowTemplate = new ParameterCollection
            {
                { "personId", "people.id" },
                { "personName", "people.name" }
            };

            var mapping = new ParameterCollection();
            mapping.Add("rows", new[] { rowTemplate }, ParameterType.ParameterCollection_IEnumerable);

            var result = PathMappingEngine.Map(source, mapping);
            var rows = result.GetByKey<ParameterCollection[]>("rows");

            Assert.That(rows, Is.Not.Null);
            Assert.That(rows.Length, Is.EqualTo(2));
            Assert.That(rows[0].GetByKey<string>("personId"), Is.EqualTo("1"));
            Assert.That(rows[0].GetByKey<string>("personName"), Is.EqualTo("Alice"));
            Assert.That(rows[1].GetByKey<string>("personId"), Is.EqualTo("2"));
            Assert.That(rows[1].GetByKey<string>("personName"), Is.EqualTo("Bob"));
        }

        [Test]
        public void Map_NestedObjectTemplate_ReturnsNestedParameterCollection()
        {
            var source = new ParameterCollection
            {
                { "id", 42 },
                { "name", "Charlie" }
            };

            var nestedTemplate = new ParameterCollection
            {
                { "userId", "id" },
                { "userName", "name" }
            };

            var mapping = new ParameterCollection
            {
                { "details", nestedTemplate }
            };

            var result = PathMappingEngine.Map(source, mapping);
            var details = result.GetByKey<ParameterCollection>("details");

            Assert.That(details.GetByKey<string>("userId"), Is.EqualTo("42"));
            Assert.That(details.GetByKey<string>("userName"), Is.EqualTo("Charlie"));
        }
    }
}
