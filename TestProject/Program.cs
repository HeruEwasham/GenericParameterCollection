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

var stringIe = c.GetByKey<List<string>>("string ie");
var stringIeFromString = c.GetByKey<List<string>>("string");
var bytesArray = c.GetByKey<byte[]>("byte-array");
var bytesList = c.GetByKey<List<byte>>("byte-array");

Console.ReadLine();