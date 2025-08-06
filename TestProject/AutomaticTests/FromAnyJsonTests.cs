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
        ClassicAssert.AreEqual(ParameterType.String, user.First().Type);
        ClassicAssert.AreEqual("Alice", user.GetByKey<string>("name"));
        ClassicAssert.AreEqual(ParameterType.Int, user.GetParameterType("age"));
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

    [Test]
    public void NestedParameterCollectionInsideCollectionEnumerable_ShouldBeParsedCorrectly()
    {
        string json = @"
        {
            ""groups"": [
                {
                    ""groupName"": ""Admins"",
                    ""members"": [
                        { ""name"": ""Alice"", ""age"": 30 },
                        { ""name"": ""Bob"", ""age"": 25 }
                    ]
                },
                {
                    ""groupName"": ""Guests"",
                    ""members"": [
                        { ""name"": ""Charlie"", ""age"": 22 }
                    ]
                }
            ]
        }";

        var collection = ParameterCollection.FromAnyJson(json);

        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("groups", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.ParameterCollection_IEnumerable, collection.First().Type);

        var groups = collection.GetByKey<List<ParameterCollection>>("groups");
        ClassicAssert.AreEqual(2, groups.Count);

        var admins = groups[0];
        ClassicAssert.AreEqual("Admins", admins.GetByKey<string>("groupName"));

        var adminMembers = admins.GetByKey<List<ParameterCollection>>("members");
        ClassicAssert.AreEqual("Alice", adminMembers[0].GetByKey<string>("name"));
        ClassicAssert.AreEqual(30, adminMembers[0].GetByKey<int>("age"));
        ClassicAssert.AreEqual("Bob", adminMembers[1].GetByKey<string>("name"));
        ClassicAssert.AreEqual(25, adminMembers[1].GetByKey<int>("age"));

        var guests = groups[1];
        ClassicAssert.AreEqual("Guests", guests.GetByKey<string>("groupName"));

        var guestMembers = guests.GetByKey<List<ParameterCollection>>("members");
        ClassicAssert.AreEqual("Charlie", guestMembers[0].GetByKey<string>("name"));
        ClassicAssert.AreEqual(22, guestMembers[0].GetByKey<int>("age"));
    }
    
    [Test]
    public void DeeplyNestedParameterCollections_ShouldBeParsedCorrectly()
    {
        string json = @"
        {
            ""departments"": [
                {
                    ""name"": ""Engineering"",
                    ""teams"": [
                        {
                            ""teamName"": ""Backend"",
                            ""members"": [
                                { ""name"": ""Alice"", ""age"": 30 },
                                { ""name"": ""Bob"", ""age"": 28 }
                            ]
                        },
                        {
                            ""teamName"": ""Frontend"",
                            ""members"": [
                                { ""name"": ""Charlie"", ""age"": 25 }
                            ]
                        }
                    ]
                },
                {
                    ""name"": ""Design"",
                    ""teams"": [
                        {
                            ""teamName"": ""UX"",
                            ""members"": [
                                { ""name"": ""Dana"", ""age"": 32 }
                            ]
                        }
                    ]
                }
            ]
        }";
    
        var collection = ParameterCollection.FromAnyJson(json);
    
        ClassicAssert.AreEqual(1, collection.Count());
        ClassicAssert.AreEqual("departments", collection.First().Key);
        ClassicAssert.AreEqual(ParameterType.ParameterCollection_IEnumerable, collection.First().Type);
    
        var departments = collection.GetByKey<List<ParameterCollection>>("departments");
        ClassicAssert.AreEqual(2, departments.Count);
    
        var engineering = departments[0];
        ClassicAssert.AreEqual("Engineering", engineering.GetByKey<string>("name"));
    
        var teams = engineering.GetByKey<List<ParameterCollection>>("teams");
        ClassicAssert.AreEqual(2, teams.Count);
    
        var backend = teams[0];
        ClassicAssert.AreEqual("Backend", backend.GetByKey<string>("teamName"));
    
        var backendMembers = backend.GetByKey<List<ParameterCollection>>("members");
        ClassicAssert.AreEqual("Alice", backendMembers[0].GetByKey<string>("name"));
        ClassicAssert.AreEqual(30, backendMembers[0].GetByKey<int>("age"));
        ClassicAssert.AreEqual("Bob", backendMembers[1].GetByKey<string>("name"));
        ClassicAssert.AreEqual(28, backendMembers[1].GetByKey<int>("age"));
    
        var design = departments[1];
        ClassicAssert.AreEqual("Design", design.GetByKey<string>("name"));
    
        var designTeams = design.GetByKey<List<ParameterCollection>>("teams");
        ClassicAssert.AreEqual(1, designTeams.Count);
    
        var uxTeam = designTeams[0];
        ClassicAssert.AreEqual("UX", uxTeam.GetByKey<string>("teamName"));
    
        var uxMembers = uxTeam.GetByKey<List<ParameterCollection>>("members");
        ClassicAssert.AreEqual("Dana", uxMembers[0].GetByKey<string>("name"));
        ClassicAssert.AreEqual(32, uxMembers[0].GetByKey<int>("age"));
    }
}
