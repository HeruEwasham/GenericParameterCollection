using YngveHestem.GenericParameterCollection;

namespace TestProject.ExampleWithAttributeConversion
{
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
}

