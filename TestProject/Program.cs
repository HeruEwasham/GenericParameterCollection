using System;
using YngveHestem.GenericParameterCollection;

namespace TestProject
{
    public static class TestClass
    {
        public static void Main(params string[] args)
        {
            var c = new ParameterCollection();

            byte[] data = new byte[3];
            data[0] = byte.MinValue;
            data[1] = 0;
            data[2] = byte.MaxValue;

            c.Add("string", "Hello, this is a string");
            c.Add("float", 1.6f);
            c.Add("int", 1);
            c.Add("double", 1.5d);
            c.Add("string ie", new string[] { "Example 1", "Example 2" });
            c.Add("byte-array", data);
            c.Add("selectOne", "Test 6", new string[] { "Test 1", "Test 3", "Test 6", "Test 45" });
            c.Add("selectOne 2", new ParameterCollection
            {
                { "value", "Value 2" },
                { "choices", new string[] {"Value 1", "Value 2", "Value 3"} }
            }, ParameterType.SelectOne);

            var stringIe = c.GetByKey<List<string>>("string ie");
            var stringIeFromString = c.GetByKey<List<string>>("string");
            var bytesArray = c.GetByKey<byte[]>("byte-array");
            var bytesList = c.GetByKey<List<byte>>("byte-array");
            var parameterSelectValue = c.GetByKey<string>("selectOne");
            var parameterSelectFull = c.GetByKey<ParameterCollection>("selectOne");
            var paraFull = c.GetByKey<string>("selectOne 2");

            var c2 = new ParameterCollection();
            c2.AddCustomConverter(c.GetCustomConverters());
            c2.AddCustomConverter(new YngveHestem.GenericParameterCollection.ParameterValueConverters.StringParameterConverter());
            c2.Add("Testparameter", true);

            var c3 = new ParameterCollection();

            var json = c.ToJson();
            var json2 = c2.ToJson();
            var json3 = c3.ToJson();

            Console.WriteLine(json);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(json2);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(json3);

            var cCopy = ParameterCollection.FromJson(json);
            var c2Copy = ParameterCollection.FromJson(json2);
            var c3Copy = ParameterCollection.FromJson(json3);

            var example1Params = new TestProject.ExampleWithoutConversion.ExampleConversionWithoutConverters().DefineAnExampleSchool();

            var Example2Params = new TestProject.ExampleWithConversion.ExampleConversionWithConverter().DefineAnExampleSchool();

            var example3 = new TestProject.ExampleWithConversionWithSchool.ExampleConversionWithConverterWithSchool();
            var example3Params = example3.DefineAnExampleSchool();
            var example3SchoolObject = example3.GetSchool(example3Params);

            var example4 = new TestProject.ExampleWithAttributeConversion.ExampleWithAttributeConversion();
            var example4Params = example4.DefineExamplePerson();
            Console.WriteLine(example4Params.ToJson());
            var example4PersonObject = example4.GetPersonObject(example4Params);
            var example4Params2 = example4.DefineExamplePersons();
            Console.WriteLine(example4Params2.ToJson());
            var example4PersonsArray = example4.GetPersonArrayFromParameterCollection(example4Params2);
            var example4PersonsList = example4.GetPersonListFromParameterCollection(example4Params2);


            Console.WriteLine(Environment.NewLine + "Parameter SelectMany tests:");

            var paramTestSelectMany = new ParameterCollection
            {
                { "Test", new string[] {}, new string[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" } }
            };
            Console.WriteLine(paramTestSelectMany.ToJson());
            paramTestSelectMany.GetParameterByKey("Test").SetValue(new string[] { "Option 2", "Option 3" });
            Console.WriteLine(paramTestSelectMany.ToJson());
            paramTestSelectMany.GetParameterByKey("Test").SetValue(new string[] { });
            Console.WriteLine(paramTestSelectMany.ToJson());
            Console.WriteLine("Parameter SelectMany tests finished.");


            Console.WriteLine(Environment.NewLine + "Testing setting new values:");
            var paramNewValueTesting = new ParameterCollection
            {
                {
                    "test", "This is a test", new ParameterCollection
                    {
                        { "status", "Initial" }
                    }
                }
            };
            paramNewValueTesting.GetParameterByKey("test").SetValue("New Value set.");
            paramNewValueTesting.GetParameterByKey("test").SetAdditionalInfo(new ParameterCollection
                    {
                        { "status", "newAdditionalInfoSet" }
                    });
            paramNewValueTesting.GetParameterByKey("test").SetValue("New value and new additionalInfo set", new ParameterCollection
                    {
                        { "status", "newAdditionalInfoSet togheter with new value" },
                        { "param", "This is a new parameter in additionalInfo" }
                    });


            // Null testing
            Console.WriteLine(Environment.NewLine + "Testing null");
            var setNull = new ParameterCollection();
            try
            {
                setNull.Add("nullWithoutKnowingTypeShouldNotBePossible", null, new ParameterCollection
                {
                    { "extra", true }
                });
                Console.WriteLine("Sending a null value that should not be possible became possible!");
            }
            catch (ArgumentNullException argEx)
            {
                Console.WriteLine("Sending a null value that should not be possible failed with expected type of exception: " + argEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sending a null value that should not be possible failed with an exception not expected: " + ex.Message);
            }
            setNull.Add("string", null, ParameterType.String);
            setNull.Add("int", null, ParameterType.Int);
            setNull.Add("decimal", null, ParameterType.Decimal);
            setNull.Add("bytes", null, ParameterType.Bytes);
            setNull.Add("date", null, ParameterType.Date);
            setNull.Add("parameterCollection", null, ParameterType.ParameterCollection);
            setNull.Add("stringArray", null, ParameterType.String_IEnumerable);
            Console.WriteLine("NullParameters after setting some (in JSON): " + setNull.ToJson());


            Console.ReadLine();
        }
    }
}