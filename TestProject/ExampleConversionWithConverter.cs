using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace TestProject.ExampleWithConversion
{
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
        protected override bool CanConvertFromParameterCollection(ParameterCollection value)
        {
            return value.HasKeyAndCanConvertTo("name", typeof(string))
                && value.HasKeyAndCanConvertTo("gender", typeof(Sex))
                && value.HasKeyAndCanConvertTo("birthDate", typeof(DateTime))
                && value.HasKeyAndCanConvertTo("summary", typeof(string));
        }

        protected override bool CanConvertToParameterCollection(Person value)
        {
            return true;        // As the object type is already checked, and I currently have no other reason to check anything in the object to know if I can convert it or not, I just return true.
        }

        protected override Person ConvertFromParameterCollection(ParameterCollection value)
        {
            return new Person
            {
                Name = value.GetByKey<string>("name"),
                Gender = value.GetByKey<Sex>("gender"),
                BirthDate = value.GetByKey<DateTime>("birthDate"),
                Summary = value.GetByKey<string>("summary")
            };
        }

        protected override ParameterCollection ConvertToParameterCollection(Person value)
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
}

