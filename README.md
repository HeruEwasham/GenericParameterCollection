# GenericParameterCollection

ParameterCollection is a simple to use collection for different parameters and types defined with a key. It supports many of the standard types like int, string, double, float, long, DateTime, bool, byte[], enums and more. It can also support nearly every other objects by easy converting the object to it´s own ParameterCollection, which can be nested togheter as parameters. It also supports many of the parameters as IEnumerables (List, Array, etc.). Methods to convert to and from JSON is also included.

## Supported types

Currently, these C#-types are supported.

- int
- string
- double
- long
- float
- byte[]
- bool
- DateTime
- ParameterCollection
- IEnumerable of int
- IEnumerable of string
- IEnumerable of double
- IEnumerable of long
- IEnumerable of float
- IEnumerable of bool
- IEnumerable of DateTime
- IEnumerable of ParameterCollection
- Enum-types

It also supports selecting an entry between different choices. It also support to select multiple choices.

For the possibillity to differentiate if it should be possible to write multiline or not in a string, and if both the date and time is important in a DateTime, this is also possible to differentiate. This can for example be useful if you for example autogenerates input-forms for a gui.

## When should I use this?

The ParameterCollection was not written to be used instead of classes. This was primarly written to be used with Interfaces or other similar situations where the classes who implements an interface can get nearly any number and different type of response from a user. It is also suitable in other situations where much input from a user is required. This as it is relatively easy to create a GUI with usable controls for the user that can be used over and over again, instead of manually define the controls for each parameter/input by hand.

### Concrete example

A concrete example will be a program that let you create an image. This program uses an interface to define everything you can do with the image. Like creating different shapes or blurring image.

Different shapes need different forms of parameters. A rectangle will need a starting point and some hight and width sizes. A circle will need a position and a radius. They might also need for example colors or brushes (or not). Then something like this is needed.

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

### Add a custom type as a parameter

Below you can see a example for how to define some structures and convert it. The example also show the use of a custom enum, that are supported without any converting.

```
public class Example
	{
		public void DefineAnExampleSchool()
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

