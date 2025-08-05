using NUnit.Framework;
using NUnit.Framework.Legacy;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests;
 
[TestFixture]
public class FromAnyJsonTests
{
    [Test]
    public void ObjectRoot_ShouldParseCorrectly()
    {
        string json = "{\"name\": \"John\", \"age\": 30}";
        var collection = ParameterCollection.FromAnyJson(json);
 
        ClassicAssert.AreEqual(2, collection.Count());
        ClassicAssert.AreEqual(ParameterType.String, collection.GetParameterType("name"));
        ClassicAssert.AreEqual(ParameterType.Int, collection.GetParameterType("age"));
    }
 
    [Test]
    public void ArrayRoot_DefaultKey_ShouldParseAsParameterCollection_IEnumerable()
    {
        string json = "[{\"id\": 1, \"title\": \"A\"}, {\"id\": 2, \"title\": \"B\"}]";
        var collection = ParameterCollection.FromAnyJson(json);
 
        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("default", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.ParameterCollection_IEnumerable, collection.First().Type);
    }
 
    [Test]
    public void ArrayRoot_CustomKey_ShouldParseAsString_IEnumerable()
    {
        string json = "[\"apple\", \"banana\", \"cherry\"]";
        var collection = ParameterCollection.FromAnyJson(json, defaultKey: "fruits");
 
        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("fruits", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.String_IEnumerable, collection.First().Type);
    }

    [Test]
    public void NestedObject_ShouldBeParsedAsParameterCollection()
    {
        string json = "{ \"user\": { \"name\": \"Alice\", \"age\": 25 } }";
        var collection = ParameterCollection.FromAnyJson(json);

        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("user", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.ParameterCollection, collection.First().Type);

        Assert.DoesNotThrow(() => collection.GetByKey<ParameterCollection>("user"));

        var user = collection.GetByKey<ParameterCollection>("user");
        ClassicAssert.AreEqual(2, user.Count());
        ClassicAssert.AreEqual("name", user.First().Key);
        ClassicAssert.AreEqual(ParameterType.String, collection.First().Type);
        ClassicAssert.AreEqual("Alice", user.GetByKey<string>("name"));
        ClassicAssert.AreEqual(ParameterType.Int, collection.GetParameterType("age"));
        ClassicAssert.AreEqual(25, user.GetByKey<int>("age"));
    }
 
    [Test]
    public void MultilineString_ShouldBeParsedAsString_Multiline()
    {
        string json = "{ \"description\": \"Line1\\nLine2\" }";
        var collection = ParameterCollection.FromAnyJson(json);
 
        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("description", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.String_Multiline, collection.First().Type);
    }
 
    [Test]
    public void Base64String_ShouldBeParsedAsBytes()
    {
        string json = "{ \"file\": \"SGVsbG8gd29ybGQ=\" }"; // "Hello world" in base64
        var collection = ParameterCollection.FromAnyJson(json);
 
        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("file", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.Bytes, collection.First().Type);
    }
}
