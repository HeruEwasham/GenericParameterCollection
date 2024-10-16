using YngveHestem.GenericParameterCollection;

namespace TestProject.ExampleWithAttributeConversionWithManyAdditonalInfoLayers;

public class ExampleWithAttributeConversionWithManyAdditonalInfoLayers
{
    public ParameterCollection DefineExample()
    {
        return ParameterCollection.FromObject(new TestSerializableClass());
    }

    public ParameterCollection DefineExampleWithClass2()
    {
        return ParameterCollection.FromObject(new TestSerializableClass2
        {
            Name = "Items",
            List = new List<TestSerializableClass>
            {
                new TestSerializableClass
                {
                    Name = "Item 1",
                    Description = "A description for item 1",
                },
                new TestSerializableClass
                {
                    Name = "Item 2",
                    Description = "A description for item 2",
                }
            }
        });
    }

    public ParameterCollection DefineExampleWithPartlyPathAlreadyMadeWithSomeOtherParameters()
    {
        var additionalInfo = new ParameterCollection
        {
            { "Another info at base level", 1.5 },
            { "some", 
                new ParameterCollection
                {
                    { "property",
                        new ParameterCollection
                        {
                            { "property inside property", "This is a property" },
                            { "inside",
                                new ParameterCollection
                                {
                                    { "property inside 123", "This is a property inside the property \"inside\"." }
                                }
                            }
                        }
                    }
                } 
            }
        };
        return new ParameterCollection
        {
            { "Test-Object", new TestSerializableClass(), additionalInfo},
        };
    }
}

[AttributeConvertible]
[AdditionalInfo("some.property.inside.many.others.on.class", "Some info sent trough many layers on class", KeyIsPath = true)]
public class TestSerializableClass
{
    [ParameterProperty]
    public string Name { get; set;} = string.Empty;

    [ParameterProperty(ParameterType.String_Multiline)]
    [AdditionalInfo("property 1", "Some info directly inside the AddditionalInfo, as most is...")]
    [AdditionalInfo("some.property.inside.Something", 1.5, KeyIsPath = true)]
    [AdditionalInfo("some.property.inside.many.others", "Some info sent trough many layers", KeyIsPath = true)]
    [AdditionalInfo("some.property.isTestProperty", true, KeyIsPath = true)]
    [AdditionalInfo("properties.prooperty 1", "This is inside only one parameterProperty-layer", KeyIsPath = true)]
    public string Description { get; set;} = string.Empty;

    public TestSerializableClass() {}

    public TestSerializableClass(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

[AttributeConvertible]
public class TestSerializableClass2
{
    [ParameterProperty]
    public string Name = string.Empty;

    [ParameterProperty]
    [AdditionalInfo("defaultValue", typeof(TestSerializableClass), "Item X", "This is the default value.")]
    public List<TestSerializableClass> List = new List<TestSerializableClass>();
}
