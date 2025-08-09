# GenericParameterCollection

ParameterCollection is a simple to use collection for different parameters and types defined with a key. It supports many of the standard types like int, string, double, float, long, DateTime, bool, byte[], enums and more. It can also support nearly every other objects by easy converting the object to it´s own ParameterCollection, which can be nested togheter as parameters. It also supports many of the parameters as IEnumerables (List, Array, etc.). Methods to convert to and from JSON is also included.

## How to use this package

The easiest way to use the package is to download it from nuget: https://www.nuget.org/packages/YngveHestem.GenericParameterCollection/

## Supported types

Currently, these C#-types are supported out of the box, with some conversion between themselves.

- int
- string
- double
- long
- float
- decimal
- byte[]
- bool
- DateTime
- ParameterCollection
- IEnumerable of int
- IEnumerable of string
- IEnumerable of double
- IEnumerable of long
- IEnumerable of float
- IEnumerable of decimal
- IEnumerable of bool
- IEnumerable of DateTime
- IEnumerable of ParameterCollection
- Enum-types
- IEnumerable of objects of types that use attribute-converters
- Nullable types of the one above (int?, double?, IEnumerable of int?, etc.)

We have also implemented converters for theese types (theese will need to be added as a custom converter when needed):

| Type(s)     | Converter-class | Description |
| ----------- | ----------- | --- |
| JToken      | JTokenParameterConverter | |
| IDictionary<,>/Dictionary<,> | DictionaryParameterConverter | Supports all generic dictionaries and types who either implement or inherit from IDictionary<,> or Dictionary<,>. All types with generic values who also is supported is supported. So both Dictionary<string, string>, Dictionary<int, bool>, and Dictionary<MyEnum, string> would work (if MyEnum is a enum that exist). If you for instance would like a Dictionary<string, MyClass>, you would need to create a converter for MyClass for this to be supported. |

Converters for other types are easy to implement.

It also supports selecting an entry between different choices. It also support to select multiple choices.

For the possibillity to differentiate if it should be possible to write multiline or not in a string, and if both the date and time is important in a DateTime, this is also possible to differentiate. This can for example be useful if you for example autogenerates input-forms for a gui.

It is also possible to add multiple parameters to a parameter, which can be used to send much more information togheter with a parameter, or be used to validate the input the way a backend wants it in GUI-level.

## When should I use this?

The ParameterCollection was not written to be used instead of classes. This was primarly written to be used with Interfaces or other similar situations where the classes who implements an interface can get nearly any number and different type of response from a user. It is also suitable in other situations where much input from a user is required. This as it is relatively easy to create a GUI with reusable controls for the user that can be used over and over again, instead of manually define the controls for each parameter/input by hand.

### Concrete example

A concrete example will be a program that let you create an image. This program uses an interface to define everything you can do with the image. Like creating different shapes or blurring image.

Different shapes need different forms of parameters. A rectangle will need a starting point and some hight and width sizes. A circle will need a position and a radius. They might also need for example colors or brushes (or not). Then something like this is needed.

## Custom converters

While the package has some default converters built in, you can add nearly any value as a parameter by creating custom converters. It is mainly two ways to create custom converters.

### Create converters with the use of attributes

Maybe the simplest possible way of creating converters on your own classes will be to use some custom attributes on a type you want to convert. See the examples below for how you can implement different converters.

When using attributes, simple ienumerables of theese objects are also supported by default.

#### Attributes you can use when creating a custom converter based on attributes

Remember to look at the examples for how this is done.

##### AttributeConvertibleAttribute

To mark a class or struct as convertable via attributes, this has to be set on the given class/struct. If not set, it will not convert the class as this even if it has other attributes.

This has a property where you can set which ParameterType it should preferably convert to. As of this writing, only the default ParameterCollection is supported.

This has also some parameters regarding how the class wants to give a default value if some code asks for it (many of the editors may for instance need it if handling an IEnumerable of the class). The parameters are "DefaultValueHandling", "DefaultValueKey" and "DefaultValueArguments".

DefaultValueHandling lets you decide if you want it to be any default value at all. Of course, the code that calls it might not respect it, but hopefully it does. The default value is InitializeNewObject, which means it will create a new object. But if that is not wanted, this propertycan be set to None.

DefaultValueKey gives the wanted key that should be used if added to the additionalInfo of for instance the IEnumerable. The default value is "defaultValue", which is the key used on many of the knnown editors.

DefaultValueArguments gives you a chance to set the arguments used on the constructor when creating that new object.

The IEnumerable-converter for the AttributeConvertibles will use theese properties when deciding what to do with parameters that don't have the defaultValue set in additionalInfo.

##### ParameterPropertyAttribute

This attribute should be marked on any field or property in the class/struct that should be converted. Only the marked properties/fields are converted.

This has a property "Key" and a property "ParameterType", and some constructors.

None of the properties needs to be set though. If ParameterType is omitted, the converter will do it's best to find the correct ParameterType based on either the given value's type, or if value is null, the type is based on the property's Type. This means that if the property's type is an interface, and the value is Null, the interface-type is used to convert this property. But if it has a value, that implementation of the interface is used to find the best ParameterType.

If the Key-property is omitted, the converter will use the name of the property/field as the key.

##### AdditionalInfoAttribute

This attribute can be used on both classes, structs, enums, properties and fields. This will as the name implies, add an AdditionalInfo-property to the given class/struct/enum/property/field. This can become handy if you want to tell something more than the value and name to the parts that use it (like and editor). For example if you have a property that can be in only a given range, this could be used.

The attribute has the properties "Key", "Value", "OvverrideIfKeyExist", "ParameterType", "KeyIsPath", "KeyPathDivider" and "ParameterTypeIsSet".

The Key- and Value-properties are of course mandatory.

The property OverrideIfKeyExist has a default value of false. This property just says that if the additioalInfo already has a property with that name, should it override the value (if this property is true), or not.

ParameterType just declares the ParameterType. If not set, the converter will do it's best to find the correct value. Since attributes has a very limited number of value-types, this means that if you for instance want to set a Date- or DateTime-value, you would need to define the value as string, and set the ParameterType directly, as the parameter else most likely would be set as string. The ParameterTypeIsSet is a get-parameter that is true if you set the ParameterType.

The property KeyIsPath is by default false. If set to true, this will tell the converter that the key is not just defining the key for it's own property, that should be set in the additionalInfo, but is defining that the property should be in another property in the additionalInfo. The path can consist of many stages out from the additionalInfo, but will of course all need to be a ParameterCollection, except the property, which can be anything.

The property KeyPathDivider is by default ".", and defines what the divider between the Path-parts should be.

For different ways to use AdditionalInfo, look at the correct examples below, and in the TestProject.

### Create a converter-class to convert between values

It is possible to convert a object to and from any ParameterType by creating a class that implement the IParameterValueConverter.

If you will make converter class between the ParameterType ParameterCollection and a single object type (per converter-class), you can instead of implementing IParameterValueConverter directly, create a class that inherit from the class ParameterCollectionParameterConverter<TValueType>. This class do some of the heavy lifting for you. TValueType will here be the type that should be converted to/from a ParameterCollection.

### A quick note about how null-value is handled

As we handle null-values, this will mean that both the rawValue and value in the converters can be null.

Another thing to mark is that the default converters will make an exception if you try to get a value that is null as a type that can not contain null (for instance you can not convert a null value to int, but you can convert it to int?). If you want to return for instance a default value if the value is null and you try to get it as a type that are not able to be null, you will be able to do that by creating a custom converter.

## GUI-frontends

Here is a list of known packages that will provide an editor for a given framework (mark that some may be more finished, updated and in better shape than others):

- [GenericParameterCollection.EtoForms](https://github.com/HeruEwasham/GenericParameterCollection.EtoForms)
- [GenericParameterCollection.Maui](https://github.com/HeruEwasham/GenericParameterCollection.Maui)
- [GenericParameterCollection.Console](https://github.com/HeruEwasham/GenericParameterCollection.Console)
- [GenericParameterCollection.RadzenBlazor](https://github.com/HeruEwasham/GenericParameterCollection.RadzenBlazor)
- [GenericParameterCollection.Avalonia](https://github.com/HeruEwasham/GenericParameterCollection.Avalonia)

Have you made a package that will fit here? Create an issue with link or create a PR.

## Other notable features

Here comes some special features (besides the main features) that might be interesting to mention.

### Converting to/from JSON

#### "Normal" conversion

The ParameterCollection can be converted to it's JSON structure by calling ToJson(..) on it's ParameterCollection and be converted from that structure by calling the static method ParameterCollection.FromJson(..).

It will of course also be converted togheter with other data you convert via Newtonsoft.JSON or a compliant setting. If you do this you might want to use the same settings, and this can be found at ParameterConverterExtensions.JsonSerializer (or ParameterConverterExtensions.GetJsonSerializerSettings() if you only want the JsonSerializerSettings). Theese settings contain for instance how things are handled.

#### From any JSON

The static method ParameterCollection.FromAnyJson(..) will make a ParameterCollection of any inputted JSON. This will determine the type based on the value alone, so the result might be different based on how the values are.

This means that you in theory can convert any normal json and show it in a ParameterCollection-view. Another possible use case is to get some specific values that might be quite different based on some values, without needing to make some complicated types, and you don't want to use JTokens and JObjects directly.

#### To simple JSON

The method ToSimpleJson(..) on ParameterCollection will translate the ParameterCollection to a json with key-value-pairs instead of the full ParameterColleection-structure.

A ParameterCollection with a parameter named "name" and value "James", and has another parameter named "age" and value 36, might look like this:

```
{
    "name": "James",
    "age": 36
}
```

### Editor (EditorEngine)

In the namespace YngveHestem.GenericParameterCollection.Editor, it exist an "Editor". This creates a ParameterCollection where you in a viewer can edit an inputted ParameterCollection (which of course can be empty for a clean "editor").

This functionality might be in handy if you for instance create something where a user can create their own forms or other input, for instance.

Mark that this based on the viewer and configuration might be some slow. If you configure it to not allow creating ParameterCollections (and ParameterCollections_IEnumerable) inside the editor, it might fasten it up.

#### Required configuration

To use this, you have at least to configure/change at least one of the following parameters in the options:

1. Edit SupportedEnumsToSelect with the given Enum-types you want to support. As you most likely would not allow the user to use all enums it might find, this is at default set to null.
2. If you do not want to use any enums, edit SupportedTypes to not contain the Enum-type. If you want all other types, you can set it to EditorConstants.AllParameterTypesExceptEnum.

## Code-Examples

### Simple use

In the code-block below, you can see some of the different methods in use.

```
var parameters = new ParameterCollection();
parameters.Add("1+1", 2);											// int
parameters.Add("5+5", 10);											// int
parameters.Add("Day", true);										// bool
parameters.Add("name", "William Wallace", false);					// string
parameters.Add("description", "- He was a knight." +
	Environment.NewLine + "- He was scottish", true);				// string (defined as multiline)

var answer = parameters.GetByKey<int>("1+1");						// returns 2 as an int
var name = parameters.GetByKey("name");								// returns the name as a string
var parameterTypeDay = parameters.GetParameterType("Day");			// returns ParameterType.Bool
var descriptionType = parameters.GetParameterType("description");   // returns ParameterType.String_Multiline
var nameType = parameters.GetParameterType("name");                 // returns ParameterType.String

var hasKeyNight = parameters.HasKey("Night");                       // returns false

var json = parameters.ToJson();                                     // Convert the collection to a json-string.

var parameters2 = ParameterCollection.FromJson(json);				// Get a new ParameterCollection-object from a json-string.
```

### Add a custom type as a parameter without using custom converters

Below you can see a example for how to define some structures and convert it. The example also show the use of a custom enum, that are supported without any converting.

```
    public class ExampleConversionWithoutConverters
    {
        public ParameterCollection DefineAnExampleSchool()
        {
            var school = new ParameterCollection();
            school.Add("name", "Au High School");
            school.Add("headmaster", new Person
            {
                Name = "Rick Rickerson",
                Gender = Sex.Male,
                BirthDate = new DateTime(1960, 1, 23),
                Summary = "He has done a lot of work"
            }.ToParameterCollection());
            return school;
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public Sex Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Summary { get; set; }

        public ParameterCollection ToParameterCollection()
        {
            return new ParameterCollection
            {
                { "name", Name, false },
                { "gender", Gender },
                { "birthDate", BirthDate, true },
                { "summary", Summary, true }
            };
        }

        public static Person FromParameterCollection(ParameterCollection person)
        {
            return new Person
            {
                Name = person.GetByKey<string>("name"),
                Gender = person.GetByKey<Sex>("gender"),
                BirthDate = person.GetByKey<DateTime>("birthDate"),
                Summary = person.GetByKey<string>("summary")
            };
        }
    }

    public enum Sex
    {
        Male,
        Female,
        Other
    }
```

### Add a custom type as a parameter by using a custom converter


Below you can see a example for how to define some structures and convert it using a custom converter for the Person-class. The example also show the use of a custom enum, that are supported without any converting.

Since the Person-class converts to ParameterCollection, the converter-class derives from ParameterCollectionParameterConverter<T>.

```
    public class ExampleConversionWithConverter
    {
        private static IParameterValueConverter[] _parameterValueConverters = new IParameterValueConverter[]
        {
            new PersonConverter()
        };

        public ParameterCollection DefineAnExampleSchool()
        {
            var school = new ParameterCollection();
            school.Add("name", "Au High School");
            school.Add("headmaster", new Person
            {
                Name = "Rick Rickerson",
                Gender = Sex.Male,
                BirthDate = new DateTime(1960, 1, 23),
                Summary = "He has done a lot of work"
            }, null, _parameterValueConverters);
            return school;
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public Sex Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Summary { get; set; }
    }

    public enum Sex
    {
        Male,
        Female,
        Other
    }

    public class PersonConverter : ParameterCollectionParameterConverter<Person>
    {
        protected override bool CanConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value.HasKeyAndCanConvertTo("name", typeof(string))
                && value.HasKeyAndCanConvertTo("gender", typeof(Sex))
                && value.HasKeyAndCanConvertTo("birthDate", typeof(DateTime))
                && value.HasKeyAndCanConvertTo("summary", typeof(string));
        }

        protected override bool CanConvertToParameterCollection(Person value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;        // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override Person ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new Person
            {
                Name = value.GetByKey<string>("name"),
                Gender = value.GetByKey<Sex>("gender"),
                BirthDate = value.GetByKey<DateTime>("birthDate"),
                Summary = value.GetByKey<string>("summary")
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(Person value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new ParameterCollection
            {
                { "name", value.Name },
                { "gender", value.Gender },
                { "birthDate", value.BirthDate },
                { "summary", value.Summary }
            };
        }
    }
```

### Add a custom type as a parameter by using a custom converter for both the parameter and the whole ParameterCollection


Below you can see a example for how to define some structures and convert it using both a custom converter for the Person-class and defining a class for the School and convert it to a ParameterCollection directly via a custom converter. The example also show the use of a custom enum, that are supported without any converting.

Since both the Person and School-classes converts to ParameterCollection, both the converter-classes derives from ParameterCollectionParameterConverter<T>.

```
    public class ExampleConversionWithConverterWithSchool
    {
        private static IParameterValueConverter[] _parameterValueConverters = new IParameterValueConverter[]
        {
            new SchoolConverter()
        };

        public ParameterCollection DefineAnExampleSchool()
        {
            var schoolObject = new School
            {
                Name = "Au High School",
                Headmaster = new Person
                {
                    Name = "Rick Rickerson",
                    Gender = Sex.Male,
                    BirthDate = new DateTime(1960, 1, 23),
                    Summary = "He has done a lot of work"
                }
            };
            return ParameterCollection.FromObject(schoolObject, _parameterValueConverters);
        }

        public School GetSchool(ParameterCollection parameters)
        {
            return parameters.ToObject<School>(_parameterValueConverters);
        }
    }

    public class School
    {
        public string Name { get; set; }
        public Person Headmaster { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public Sex Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Summary { get; set; }
    }

    public enum Sex
    {
        Male,
        Female,
        Other
    }

    public class PersonConverter : ParameterCollectionParameterConverter<Person>
    {
        protected override bool CanConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value.HasKeyAndCanConvertTo("name", typeof(string))
                && value.HasKeyAndCanConvertTo("gender", typeof(Sex))
                && value.HasKeyAndCanConvertTo("birthDate", typeof(DateTime))
                && value.HasKeyAndCanConvertTo("summary", typeof(string));
        }

        protected override bool CanConvertToParameterCollection(Person value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;        // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override Person ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new Person
            {
                Name = value.GetByKey<string>("name"),
                Gender = value.GetByKey<Sex>("gender"),
                BirthDate = value.GetByKey<DateTime>("birthDate"),
                Summary = value.GetByKey<string>("summary")
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(Person value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new ParameterCollection
            {
                { "name", value.Name },
                { "gender", value.Gender },
                { "birthDate", value.BirthDate },
                { "summary", value.Summary }
            };
        }
    }

    public class SchoolConverter : ParameterCollectionParameterConverter<School>
    {
        private static IParameterValueConverter[] _parameterValueConverters = new IParameterValueConverter[]
        {
            new PersonConverter()
        };

        protected override bool CanConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value.HasKeyAndCanConvertTo("name", typeof(string)) && value.HasKeyAndCanConvertTo("headmaster", typeof(Person), _parameterValueConverters);
        }

        protected override bool CanConvertToParameterCollection(School value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;         // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override School ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new School
            {
                Name = value.GetByKey<string>("name"),
                Headmaster = value.GetByKey<Person>("headmaster", _parameterValueConverters)
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(School value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new ParameterCollection
            {
                { "name", value.Name },
                { "headmaster", value.Headmaster, null, _parameterValueConverters }
            };
        }
    }
```

Mark that the example above sets the custom converter of the person inside the school-converter, which ensures that the school-converter are not dependent on that other converters is also given. But it is also possible to let the school-converter expect that the person-converter are given as a custom converter by using the customConverter-parameters in the converter.

### Add types by using attributes instead of custom converters

Here comes an example for using some custom attributes to convert an object to and from ParameterCollection. This might make it easier to convert objects you have at your disposal, as you can mark the object with some attributes.

#### Example code

```
public class ExampleWithAttributeConversion
    {
        public ParameterCollection DefineExamplePerson()
        {
            var color = new ExampleColor
            {
                Blue = 0.5f,
                Red = 0.9f
            };

            return ParameterCollection.FromObject(new Person("Rick Mortimer", color));
        }

        public Person GetPersonObject(ParameterCollection parameters)
        {
            return parameters.ToObject<Person>();
        }
    }

    [AttributeConvertible]
    [AdditionalInfo("type", "color")]
	public class ExampleColor
	{
        [ParameterProperty("r")]
		[AdditionalInfo("desc", "How much red color. Goes from 0 to 1.")]
        [AdditionalInfo("minValue", 0.0f)]
        [AdditionalInfo("maxValue", 1.0f)]
		public float Red { get; set; } = 1;

        [ParameterProperty("g")]
        [AdditionalInfo("desc", "How much green color. Goes from 0 to 1.")]
        [AdditionalInfo("minValue", 0.0f)]
        [AdditionalInfo("maxValue", 1.0f)]
        public float Green { get; set; } = 1;

        [ParameterProperty("b")]
        [AdditionalInfo("desc", "How much blue color. Goes from 0 to 1.")]
        [AdditionalInfo("minValue", 0.0f)]
        [AdditionalInfo("maxValue", 1.0f)]
        public float Blue { get; set; } = 1;

        [ParameterProperty("a")]
        [AdditionalInfo("desc", "Should the color be transparent or not (or something in between). Goes from 0 to 1.")]
        [AdditionalInfo("minValue", 0.0f)]
        [AdditionalInfo("maxValue", 1.0f)]
        public float Alpha { get; set; } = 1;
	}

    [AttributeConvertible]
    public class Person
    {
        [ParameterProperty("name")]
        private string _name;

        [ParameterProperty]
        private ExampleColor _favoriteColor;

        public Person()
        {
            _name = "Unknown";
            _favoriteColor = new ExampleColor();
        }

        public Person(string name, ExampleColor color)
        {
            _name = name;
            _favoriteColor = color;
        }
    }
```
#### How do the attributes work

In the example code, we have two classes that will be converted based on the attribute-notations. The ExampleColor-class and the Person-class.

Both classes have the `[AttributeConvertible]` attribute set. This tells the converter to convert this class using the given attributes.

We can see that all the properties and fields we want to convert has the attribute `[ParameterProperty]` set. This can either be used without given any parameters (as we can see with Persons-class property _favoriteColor), then the given property name will be used as a key, or with a specified key (as the other properties are). You are also able to specify a ParameterType if you want. If this is not set, the system will try to find the best suitable ParameterType for you.

In the example code above we do also use another custom attribute. This is the `[AdditionalInfo]` attribute. This attribute can be used to populate the AdditionalInfo-ParameterCollection for given property. As you might see, even the class itself can have this parameter, and this will be set to the parent-parameter's additionalInfo if a parent parameter exists.

While the `[AttributeConvertible]` attribute needs to be set for the `[AdditionalInfo]` attributes on the classes properties to be used. Any `[AdditionalInfo]` attributes on the class itself will be picked up even if no [AttributeConvertible] is used.

Mark that all classes that will be converted using attributes currently needs to have a parameterless constructor.