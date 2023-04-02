using System;
using YngveHestem.GenericParameterCollection;

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

var json = c.ToJson();

Console.WriteLine(json);

Console.ReadLine();