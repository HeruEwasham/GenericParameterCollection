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

### Create a converter-class to convert between values

It is possible to convert a object to and from any ParameterType by creating a class that implement the IParameterValueConverter.

If you will make converter class between the ParameterType ParameterCollection and a single object type (per converter-class), you can instead of implementing IParameterValueConverter directly, create a class that inherit from the class ParameterCollectionParameterConverter<TValueType>. This class do some of the heavy lifting for you. TValueType will here be the type that should be converted to/from a ParameterCollection.

## GUI-frontends

Here is a list of known packages that will provide an editor for a given framework (mark that some may be more finished, updated and in better shape than others):

- [GenericParameterCollection.EtoForms](https://github.com/HeruEwasham/GenericParameterCollection.EtoForms)
- [GenericParameterCollection.Maui](https://github.com/HeruEwasham/GenericParameterCollection.Maui)
- [GenericParameterCollection.Console](https://github.com/HeruEwasham/GenericParameterCollection.Console)
- [GenericParameterCollection.RadzenBlazor](https://github.com/HeruEwasham/GenericParameterCollection.RadzenBlazor)

Have you made a package that will fit here? Create an issue with link or create a PR.

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

        protected override bool CanConvertToParameterCollection(Person value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;        // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override Person ConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new Person
            {
                Name = value.GetByKey<string>("name"),
                Gender = value.GetByKey<Sex>("gender"),
                BirthDate = value.GetByKey<DateTime>("birthDate"),
                Summary = value.GetByKey<string>("summary")
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(Person value, IEnumerable<IParameterValueConverter> customConverters)
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
        protected override bool CanConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value.HasKeyAndCanConvertTo("name", typeof(string))
                && value.HasKeyAndCanConvertTo("gender", typeof(Sex))
                && value.HasKeyAndCanConvertTo("birthDate", typeof(DateTime))
                && value.HasKeyAndCanConvertTo("summary", typeof(string));
        }

        protected override bool CanConvertToParameterCollection(Person value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;        // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override Person ConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new Person
            {
                Name = value.GetByKey<string>("name"),
                Gender = value.GetByKey<Sex>("gender"),
                BirthDate = value.GetByKey<DateTime>("birthDate"),
                Summary = value.GetByKey<string>("summary")
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(Person value, IEnumerable<IParameterValueConverter> customConverters)
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

        protected override bool CanConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value.HasKeyAndCanConvertTo("name", typeof(string)) && value.HasKeyAndCanConvertTo("headmaster", typeof(Person), _parameterValueConverters);
        }

        protected override bool CanConvertToParameterCollection(School value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;         // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override School ConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new School
            {
                Name = value.GetByKey<string>("name"),
                Headmaster = value.GetByKey<Person>("headmaster", _parameterValueConverters)
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(School value, IEnumerable<IParameterValueConverter> customConverters)
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