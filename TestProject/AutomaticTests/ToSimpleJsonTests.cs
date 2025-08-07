using NUnit.Framework;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests;

[TestFixture]
public class ToSimpleJsonTests
{
    [Test]
    public void ToSimpleJson_NestedParameterCollectionEnumerable_ProducesExpectedJson()
    {
        var adminsMembers = new ParameterCollection[]
        {
            new ParameterCollection {
                new Parameter("name", "Alice"),
                new Parameter("age", 30)
            },
            new ParameterCollection {
                new Parameter("name", "Bob"),
                new Parameter("age", 25)
            }
        };

        var guestsMembers = new ParameterCollection[]
        {
            new ParameterCollection {
                new Parameter("name", "Charlie"),
                new Parameter("age", 22)
            }
        };

        var groups = new ParameterCollection[]
        {
            new ParameterCollection {
                new Parameter("groupName", "Admins"),
                new Parameter("members", adminsMembers, ParameterType.ParameterCollection_IEnumerable)
            },
            new ParameterCollection {
                new Parameter("groupName", "Guests"),
                new Parameter("members", guestsMembers, ParameterType.ParameterCollection_IEnumerable)
            }
        };

        var collection = new ParameterCollection {
            new Parameter("groups", groups, ParameterType.ParameterCollection_IEnumerable)
        };

        var json = collection.ToSimpleJson(Newtonsoft.Json.Formatting.None);
        var expected = "{\"groups\":[{\"groupName\":\"Admins\",\"members\":[{\"name\":\"Alice\",\"age\":30},{\"name\":\"Bob\",\"age\":25}]},{\"groupName\":\"Guests\",\"members\":[{\"name\":\"Charlie\",\"age\":22}]}]}";
        Assert.That(json, Is.EqualTo(expected));
    }

    [Test]
    public void ToSimpleJson_ReturnsEmptyJson_WhenNoParameters()
    {
        var collection = new ParameterCollection();
        var json = collection.ToSimpleJson();
        var expected = "{}";
        Assert.That(json, Is.EqualTo(expected));
    }

    [Test]
    public void ToSimpleJson_ReturnsCorrectJson_ForSimpleTypes()
    {
        var collection = new ParameterCollection
        {
            new Parameter("intKey", 42),
            new Parameter("strKey", "hello")
        };
        var json = collection.ToSimpleJson();
        var expected = "{\"intKey\":42,\"strKey\":\"hello\"}";
        Assert.That(json, Is.EqualTo(expected));
    }

    [Test]
    public void ToSimpleJson_RespectsFormattingIndented()
    {
        var collection = new ParameterCollection
        {
            new Parameter("key", "value")
        };
        var json = collection.ToSimpleJson(Newtonsoft.Json.Formatting.Indented);
        var expected = "{\n  \"key\": \"value\"\n}";
        Assert.That(json, Is.EqualTo(expected));
    }

    [Test]
    public void ToSimpleJson_SerializesComplexTypes()
    {
        var collection = new ParameterCollection();
        var obj = new ParameterCollection
        {
            new Parameter("Name", "Test"),
            new Parameter("Age", 30)
         };
        collection.Add(new Parameter("objKey", obj));
        var json = collection.ToSimpleJson();
        var expected = "{\"objKey\":{\"Name\":\"Test\",\"Age\":30}}";
        Assert.That(json, Is.EqualTo(expected));
    }

    [Test]
    public void ToSimpleJson_HandlesNullValues()
    {
        var collection = new ParameterCollection
        {
            new Parameter("nullKey", null, ParameterType.String)
        };
        var json = collection.ToSimpleJson();
        var expected = "{\"nullKey\":null}";
        Assert.That(json, Is.EqualTo(expected));
    }
}
