using NUnit.Framework;
using TestProject.ExampleWithAttributeConversion;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests
{
    [TestFixture]
    public class AttributeConvertibleTests
    {
        [Test]
        public void SimpleAttributeConvertionFromObject()
        {
            var color = new ExampleColor
            {
                Blue = 0.5f,
                Red = 0.9f
            };

            var parameters = ParameterCollection.FromObject(new Person("Rick Mortimer", color));

            Assert.That(parameters.GetByKey<string>("name"), Is.EqualTo("Rick Mortimer"));
            Assert.That(parameters.GetByKey<ParameterCollection>("_favoriteColor").GetByKey<float>("r"), Is.EqualTo(0.9f));
        }

        [Test]
        public void AdditionalInfoAttributeConvertionFromObject()
        {
            var color = new ExampleColor
            {
                Blue = 0.5f,
                Red = 0.9f
            };

            var parameters = ParameterCollection.FromObject(new Person("Rick Mortimer", color));

            Assert.That(parameters.GetByKey<ParameterCollection>("_favoriteColor").GetParameterByKey("r").GetAdditionalInfo().GetByKey<float>("minValue"), Is.EqualTo(0.0f));
            Assert.That(parameters.GetByKey<ParameterCollection>("_favoriteColor").GetParameterByKey("r").GetAdditionalInfo().GetByKey<float>("maxValue"), Is.EqualTo(1.0f));
        }

        [Test]
        public void AdditionalInfoParameterPropertyAttributeConvertionFromObject()
        {
            var color = new ExampleColor
            {
                Blue = 0.5f,
                Red = 0.9f
            };

            var parameters = ParameterCollection.FromObject(new Person("Rick Mortimer", color));

            Assert.That(parameters.GetByKey<ParameterCollection>("_favoriteColor").GetParameterByKey("r").GetAdditionalInfo().GetByKey<float>("minValue"), Is.EqualTo(0.0f));
            Assert.That(parameters.GetByKey<ParameterCollection>("_favoriteColor").GetParameterByKey("r").GetAdditionalInfo().GetByKey<float>("maxValue"), Is.EqualTo(1.0f));
        }
    }
}