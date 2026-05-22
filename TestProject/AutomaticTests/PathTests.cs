using System;
using System.Data;
using NUnit.Framework;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests;

[TestFixture]
public class PathTests
{
    private ParameterCollection _testParameters;

    [SetUp]
    public void SetUp()
    {
        _testParameters = ParameterCollection.FromAnyJson("""{"id":"abc123","name":"Eksempelobjekt","active":true,"metadata":{"createdAt":"2026-05-18T14:00:00Z","updatedAt":"2026-05-18T15:30:00Z","tags":["demo","json","struktur"]},"settings":{"theme":"dark","language":"no","permissions":{"read":true,"write":false,"admin":false}},"users":[{"userId":1,"username":"ola","roles":["editor","viewer"],"preferences":{"notifications":{"email":true,"sms":false},"dashboardWidgets":[{"type":"weather","enabled":true},{"type":"news","enabled":false}]}},{"userId":2,"username":"kari","roles":["admin"],"preferences":{"notifications":{"email":false,"sms":true},"dashboardWidgets":[]}}],"projects":[{"projectId":"p001","title":"Prosjekt Alfa","status":"ongoing","tasks":[{"taskId":"t1","title":"Planlegging","completed":false},{"taskId":"t2","title":"Design","completed":true}]},{"projectId":"p002","title":"Prosjekt Beta","status":"completed","tasks":[{ "taskId": "t3", "title": "Designtips", "completed": true }]}],"logs":[],"configurations":[{"configId":"c01","description":"Standardoppsett","options":[{"key":"autosave","value":true},{"key":"refreshRate","value":30}]}]}""");
    }

    [TestCase("id", typeof(string), "abc123")]
    [TestCase("active", typeof(bool), true)]
    [TestCase("metadata.createdAt", typeof(string), "05/18/2026 14:00:00")]
    [TestCase("metadata.tags", typeof(string[]), new string[] { "demo", "json", "struktur" })]
    [TestCase("settings.language", typeof(string), "no")]
    [TestCase("settings.permissions.read", typeof(bool), true)]
    [TestCase("settings.permissions.write", typeof(bool), false)]
    [TestCase("settings.permissions.admin", typeof(bool), false)]
    [TestCase("users.$$item0$$.userId", typeof(int), 1)]
    [TestCase("users.$$item1$$.username", typeof(string), "kari")]
    [TestCase("users.userId", typeof(int), 1)]
    [TestCase("users.username", typeof(string), "ola")]
    [TestCase("users.userId", typeof(int[]), new int[] {1,2})]
    [TestCase("users.username", typeof(string[]), new string[] { "ola", "kari"})]
    [TestCase("users.preferences.dashboardWidgets.type", typeof(string[]), new string[] { "weather", "news" })]
    [TestCase("projects.tasks.title", typeof(string[]), new string[] { "Planlegging", "Design", "Designtips" })]
    [TestCase("projects.tasks.completed", typeof(bool[]), new bool[] { false, true, true })]
    public void ReturnsCorrectValuesFromPath(string path, Type returnType, object expectedReturnValue)
    {
        var value = _testParameters.GetByPath(path, returnType);
        Assert.That(value,Is.EqualTo(expectedReturnValue));
    }

    [Test]
    public void ReturnsCorrectValueWithAdditionalInfo()
    {
        var parameters = new ParameterCollection
        {
            { "test1", "testvalue" },
            { 
                "test2", "test2Value", new ParameterCollection
                {
                    { "testInner", "test2" }
                }
            }
        };

        var value = parameters.GetByPath<string>("test2.$$ADDITIONAL_IMFO$$.testInner");

        Assert.That(value,Is.EqualTo("test2"));
    }
}