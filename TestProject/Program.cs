using System;
using System.Diagnostics;
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
            Console.WriteLine(example4Params.ToJson(Newtonsoft.Json.Formatting.Indented));
            var example4PersonObject = example4.GetPersonObject(example4Params);
            var example4Params2 = example4.DefineExamplePersons();
            Console.WriteLine(example4Params2.ToJson(Newtonsoft.Json.Formatting.Indented));
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
            setNull.Add("stringNull", (string?)null, true);
            Console.WriteLine("NullParameters after setting some (in JSON): " + setNull.ToJson());

            var setNullableTypes = new ParameterCollection();
            setNullableTypes.Add("int", (int?)17);
            setNullableTypes.Add("dateTime", (DateTime?)DateTime.Now);
            setNullableTypes.Add("string", (string?)"Hello.", true);
            setNullableTypes.Add("enum nullable specified nullable type but is with value", ParameterType.DateTime, typeof(ParameterType?));
            Console.WriteLine("NullableTypesParameters after setting some (in JSON): " + setNullableTypes.ToJson());

            setNull.GetParameterByKey("int").SetValue(32);
            setNull.GetParameterByKey("decimal").SetValue((float?)32f);
            setNull.GetParameterByKey("string").SetValue("Test");
            Console.WriteLine("NullParameters after changing some values (in JSON): " + setNull.ToJson());

            setNull.GetParameterByKey("string").SetValue(null);
            setNull.GetParameterByKey("int").SetValue(null);
            Console.WriteLine("NullParameters after changing some values back to null (in JSON): " + setNull.ToJson());

            var integer = setNullableTypes.GetByKey<int>("int");
            var nullableInteger = setNullableTypes.GetByKey<int?>("int");
            var text = setNullableTypes.GetByKey<string>("string");
            var textNullable = setNullableTypes.GetByKey<string?>("string");

            setNull.GetParameterByKey("string").SetValue(null);
            var stringNull = setNull.GetByKey<string>("string");

            var allNull = new ParameterCollection
            {
                { "int", null, ParameterType.Int },
                { "string", null, ParameterType.String },
                { "string_multiline", null, ParameterType.String_Multiline },
                { "string_multiline_short", (string?)null, true },
                { "decimal", null, ParameterType.Decimal },
                { "bytes", null, ParameterType.Bytes },
                { "bool", null, ParameterType.Bool },
                { "datetime", null, ParameterType.DateTime },
                { "datetime_short", (DateTime?)null, false },
                { "date", null, ParameterType.Date },
                { "string_array", null, ParameterType.String_IEnumerable },
                { "string_multiline_array", null, ParameterType.String_Multiline_IEnumerable },
                { "int_array", null, ParameterType.Int_IEnumerable },
                { "decimal_array", null, ParameterType.Decimal_IEnumerable },
                { "bool_array", null, ParameterType.Bool_IEnumerable },
                { "datetime_array", null, ParameterType.DateTime_IEnumerable },
                { "datetime_array_short", (List<DateTime?>)null, false },
                { "date_array", null, ParameterType.Date_IEnumerable },
                { "parameterCollection", null, ParameterType.ParameterCollection },
                { "parameterCollection_array", null, ParameterType.ParameterCollection_IEnumerable },
                { "enum", null, ParameterType.Enum },
                { "selectOne", null, ParameterType.SelectOne },
                { "selectMany", null, ParameterType.SelectMany },
                { "int as nullable", null, typeof(int?) },
            };
            Console.WriteLine("All null (as JSON): " + allNull.ToJson());
            Console.WriteLine("All null (as string): " + allNull.ToString());

            var nullInt = allNull.GetByKey<int?>("int");
            var nullString = allNull.GetByKey<string>("string");
            var nullStringMultiline = allNull.GetByKey<string>("string_multiline");
            var nullStringMultilineShort = allNull.GetByKey<string>("string_multiline_short");
            var nullDecimal = allNull.GetByKey<decimal?>("decimal");
            var nullFloat = allNull.GetByKey<float?>("decimal");
            var nullBytes = allNull.GetByKey<byte[]>("bytes");
            var nullBool = allNull.GetByKey<bool?>("bool");
            var nullDateTime = allNull.GetByKey<DateTime?>("datetime");
            var nullDateTimeShort = allNull.GetByKey<DateTime?>("datetime_short");
            var nullDate = allNull.GetByKey<DateTime?>("date");
            var nullStringArray = allNull.GetByKey<string[]>("string_array");
            var nullStringMultilineArray = allNull.GetByKey<string[]>("string_multiline_array");
            var nullIntArray = allNull.GetByKey<int[]>("int_array");
            var nullDecimalArray = allNull.GetByKey<decimal[]>("decimal_array");
            var nullBoolArray = allNull.GetByKey<bool[]>("bool_array");
            var nullDateTimeArray = allNull.GetByKey<DateTime[]>("datetime_array");
            var nullDateTimeArrayShort = allNull.GetByKey<DateTime[]>("datetime_array_short");
            var nullDateArray = allNull.GetByKey<DateTime[]>("date_array");
            var nullParameterCollection = allNull.GetByKey<ParameterCollection>("parameterCollection");
            var nullParameterCollectionArray = allNull.GetByKey<ParameterCollection[]>("parameterCollection_array");
            var nullEnum = allNull.GetByKey<ParameterType?>("enum");
            var nullSelectOne = allNull.GetByKey<string>("selectOne");
            var nullSelectOneChoices = allNull.GetParameterByKey("selectOne").GetChoices();
            var nullSelectMany = allNull.GetByKey<string[]>("selectMany");
            var nullSelectManyChoices = allNull.GetParameterByKey("selectMany").GetChoices();

            var p = new ParameterCollection
            {
                { "intIe", new int[] {1,5,7} },
                { "intIeNullable", new int?[] {1,6, null, 5} },
                { "decimalIe", new float[] {1.5f,5,7.7f} },
                { "decimalIeNullable", new float?[] {1.3f,6, null, 5.23f} },
                { "bool", new bool?[] { true, false, null, true, true } },
                { "datetime", new DateTime?[] { null, DateTime.Now, DateTime.Today, DateTime.UtcNow } }
            };

            var pIntIe_double = p.GetByKey<double[]>("intIe");
            var pIntIe_string = p.GetByKey<string[]>("intIe");
            var pIntIe_double_nullable = p.GetByKey<double?[]>("intIeNullable");
            var pIntIe_string_nullable = p.GetByKey<string?[]>("intIeNullable");
            var pDecimalIe_double = p.GetByKey<double[]>("decimalIe");
            var pDecimalIe_string = p.GetByKey<string[]>("decimalIe");
            var pDecimalIe_double_nullable = p.GetByKey<double?[]>("decimalIeNullable");
            var pDecimalIe_string_nullable = p.GetByKey<string?[]>("decimalIeNullable");
            var pBool = p.GetByKey<bool?[]>("bool");
            var pDateTime = p.GetByKey<DateTime?[]>("datetime");

            var nullableExample = new ExampleNullableTypes.ExampleNullableTypesClass();
            var taskParameters = nullableExample.GetExampleParameters();
            var tasks = nullableExample.GetAsList(taskParameters);

            Console.WriteLine("Int in allNull is null: " + allNull.GetParameterByKey("int").HasValue().ToString());
            Console.WriteLine("Int as nullable in allNull is null: " + allNull.GetParameterByKey("int as nullable").HasValue().ToString());

            allNull.Add("enum with type specified", null, typeof(ParameterType?));
            var hasEnumKeyAllNull = allNull.HasKeyAndCanConvertTo("enum with type specified", typeof(ParameterType?));
            var nullEnumSpecified = allNull.GetByKey<ParameterType?>("enum with type specified");
            var nullEnumSpecifiedAsString = allNull.GetByKey<string>("enum with type specified");
            var nullEnumSpecifiedChoices = allNull.GetParameterByKey("enum with type specified").GetChoices();
            allNull.Add("int nullable type specified as nullable", null, typeof(int?));
            var intNullable = allNull.GetByKey<int?>("int nullable type specified as nullable");
            var nullableEnumSpecified = setNullableTypes.GetByKey<ParameterType?>("enum nullable specified nullable type but is with value");
            var nullableEnumSpecifiedAsString = setNullableTypes.GetByKey<string>("enum nullable specified nullable type but is with value");
            var nullableEnumSpecifiedChoices = setNullableTypes.GetParameterByKey("enum nullable specified nullable type but is with value").GetChoices();

            var someMoreTests = new ParameterCollection
            {
                { "enum", ParameterType.ParameterCollection },
                { "enumAsString", "Date" },
                { "enumAsInt", 5 },
                { "int", 15 },
                { "nullableEnumStartNull", null, typeof(ParameterType?) },
                { "nullableEnumStartValue", ParameterType.SelectOne, typeof(ParameterType?) },
            };

            var hasEnumKey = someMoreTests.HasKeyAndCanConvertTo("enum", typeof(ParameterType));
            var hasIntKey = someMoreTests.HasKeyAndCanConvertTo("int", typeof(int));
            var hasEnumKeyAsStringAndCanConvertToEnum = someMoreTests.HasKeyAndCanConvertTo("enumAsString", typeof(ParameterType));
            var hasEnumKeyAsIntAndCanConvertToEnum = someMoreTests.HasKeyAndCanConvertTo("enumAsInt", typeof(ParameterType));
            var hasEnumKeyWithoutCheckingConvertionType = someMoreTests.HasKey("enum");

            var enumFromEnum = someMoreTests.GetByKey<ParameterType>("enum");
            var enumFromString = someMoreTests.GetByKey<ParameterType>("enumAsString");
            var enumFromInt = someMoreTests.GetByKey<ParameterType>("enumAsInt");
            var nullableEnumFromEnum = someMoreTests.GetByKey<ParameterType?>("enum");

            var enumChoices = someMoreTests.GetParameterByKey("enum").GetChoices();
            var nullableEnumStartNullChoices = someMoreTests.GetParameterByKey("nullableEnumStartNull").GetChoices();
            var nullableEnumStartValueChoices = someMoreTests.GetParameterByKey("nullableEnumStartValue").GetChoices();
            var nullableEnumStartNull = someMoreTests.GetByKey<ParameterType?>("nullableEnumStartNull");

            var testNumberBoundaries = new ParameterCollection
            {
                { "minInt", int.MinValue },
                { "maxInt", int.MaxValue },
                { "minFloat", float.MinValue },
                { "maxFloat", float.MaxValue },
                { "minDouble", double.MinValue },
                { "maxDouble", double.MaxValue },
                { "minLong", long.MinValue },
                { "maxLong", long.MaxValue },
                { "minDecimal", decimal.MinValue },
                { "maxDecimal", decimal.MaxValue },
            };

            Console.WriteLine(Environment.NewLine + "TestNumberBoundaries:" + Environment.NewLine + testNumberBoundaries.ToJson(Newtonsoft.Json.Formatting.Indented));

            var testNumberBoundaries_MinInt = testNumberBoundaries.GetByKey<int>("minInt");
            var testNumberBoundaries_MaxInt = testNumberBoundaries.GetByKey<int>("maxInt");
            var testNumberBoundaries_MinFloat = testNumberBoundaries.GetByKey<float>("minFloat");
            var testNumberBoundaries_MaxFloat = testNumberBoundaries.GetByKey<float>("maxFloat");
            var testNumberBoundaries_MinDouble = testNumberBoundaries.GetByKey<double>("minDouble");
            var testNumberBoundaries_MaxDouble = testNumberBoundaries.GetByKey<double>("maxDouble");
            var testNumberBoundaries_MinLong = testNumberBoundaries.GetByKey<long>("minLong");
            var testNumberBoundaries_MaxLong = testNumberBoundaries.GetByKey<long>("maxLong");
            var testNumberBoundaries_MinDecimal = testNumberBoundaries.GetByKey<decimal>("minDecimal");
            var testNumberBoundaries_MaxDecimal = testNumberBoundaries.GetByKey<decimal>("maxDecimal");
            var testNumberBoundaries_MinDecimalDirect = testNumberBoundaries.GetParameterByKey("minDecimal").GetValue<decimal>();


            var testClassForAdditionalInfoPathSerialisation = new ExampleWithAttributeConversionWithManyAdditonalInfoLayers.ExampleWithAttributeConversionWithManyAdditonalInfoLayers();
            var testClassForAdditionalInfoPathSerialisationResult = testClassForAdditionalInfoPathSerialisation.DefineExample();
            Console.WriteLine("testClassForAdditionalInfoPathSerialisationResult:" + Environment.NewLine + testClassForAdditionalInfoPathSerialisationResult.ToJson(Newtonsoft.Json.Formatting.Indented));

            var testClassForAdditionalInfoPathSerialisationWithOtherAdditionalinfoResult = testClassForAdditionalInfoPathSerialisation.DefineExampleWithPartlyPathAlreadyMadeWithSomeOtherParameters();
            Console.WriteLine("testClassForAdditionalInfoPathSerialisationWithOtherAdditionalinfoResult:" + Environment.NewLine + testClassForAdditionalInfoPathSerialisationWithOtherAdditionalinfoResult.ToJson(Newtonsoft.Json.Formatting.Indented));
            
            var testClassForAdditionalInfoPathSerialisation2Result = testClassForAdditionalInfoPathSerialisation.DefineExampleWithClass2();
            Console.WriteLine("testClassForAdditionalInfoPathSerialisation2Result:" + Environment.NewLine + testClassForAdditionalInfoPathSerialisation2Result.ToJson(Newtonsoft.Json.Formatting.Indented));

            Console.ReadLine();
        }
    }
}