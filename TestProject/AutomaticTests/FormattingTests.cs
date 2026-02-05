using System;
using System.Data.SqlTypes;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests;

[TestFixture]
public class FormattingTests
{
    [Test]
    public void TestNumbers()
    {
        var parameters = new ParameterCollection
        {
            { "intWithout", 5 },
            { "intWith", 255, new ParameterCollection
                {
                    { "format", "x" }
                }
            },
            { "decimalWithout", 5.5 },
            { "decimalWith", 8.3, new ParameterCollection
                {
                    { "format", "f3" }
                }
            }
        };
        
        ClassicAssert.AreEqual("5", parameters.GetByKey<string>("intWithout"));
        ClassicAssert.AreEqual("ff", parameters.GetByKey<string>("intWith"));
        ClassicAssert.AreEqual("5,5", parameters.GetByKey<string>("decimalWithout"));
        ClassicAssert.AreEqual("8,300", parameters.GetByKey<string>("decimalWith"));
    }

    [Test]
    public void TestDateTime()
    {
        var parameters = new ParameterCollection
        {
            { "dateWith", new DateTime(2005, 5, 1), new ParameterCollection
                {
                    { "format", "yyyy-MM-dd" }
                }
            },
            { "dateTimeWith", new DateTime(2005, 5, 1, 12, 5, 0), new ParameterCollection
                {
                    { "format", "dd MM yyyy HH:mm" }
                }
            }
        };
        
        ClassicAssert.AreEqual("2005-05-01", parameters.GetByKey<string>("dateWith"));
        ClassicAssert.AreEqual("01 05 2005 12:05", parameters.GetByKey<string>("dateTimeWith"));
    }

    [Test]
    public void TestEnum()
    {
        var parameters = new ParameterCollection
        {
            { "enumWithout", ParameterType.Date },
            { "enumWith", ParameterType.ParameterCollection, new ParameterCollection
                {
                    { "format", "D" }
                }
            }
        };
        
        ClassicAssert.AreEqual("Date", parameters.GetByKey<string>("enumWithout"));
        ClassicAssert.AreEqual("15", parameters.GetByKey<string>("enumWith"));
    }

    [Test]
    public void TestEnumCustom()
    {
        var parameters = new ParameterCollection
        {
            { "enumWithout", EnumTest.Default },
            { "enumWith", EnumTest.Test2, new ParameterCollection
                {
                    { "format", "D" }
                }
            }
        };
        
        ClassicAssert.AreEqual("Default", parameters.GetByKey<string>("enumWithout"));
        ClassicAssert.AreEqual("1", parameters.GetByKey<string>("enumWith"));
    }
}
