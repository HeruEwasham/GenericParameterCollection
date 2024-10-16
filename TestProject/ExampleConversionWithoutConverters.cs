using YngveHestem.GenericParameterCollection;

namespace TestProject.ExampleWithoutConversion
{
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
}

