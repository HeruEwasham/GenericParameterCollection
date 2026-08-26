using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace TestProject.ExampleWithConversionWithSchool
{
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
}


