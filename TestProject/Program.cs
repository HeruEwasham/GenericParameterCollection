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


            Console.ReadLine();
        }
    }
}