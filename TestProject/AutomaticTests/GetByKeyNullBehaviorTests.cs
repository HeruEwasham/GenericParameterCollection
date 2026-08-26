using System.Collections.Generic;
using NUnit.Framework;
using YngveHestem.GenericParameterCollection;

namespace TestProject.AutomaticTests
{
    [TestFixture]
    public class GetByKeyNullBehaviorTests
    {
        [Test]
        public void MissingKey_NonNullableValueType_Throws()
        {
            var pc = new ParameterCollection();
            Assert.Throws<KeyNotFoundException>(() => pc.GetByKey<int>("missing"));
        }

        [Test]
        public void MissingKey_NullableValueType_ReturnsNull()
        {
            var pc = new ParameterCollection();
            var v = pc.GetByKey<int?>("missing");
            Assert.That(v, Is.Null);
        }

        [Test]
        public void MissingKey_ReferenceType_ReturnsNull()
        {
            var pc = new ParameterCollection();
            var s = pc.GetByKey<string>("missing");
            Assert.That(s, Is.Null);
        }

        [Test]
        public void ExistingParameter_NullValue_NonNullableRequest_Throws()
        {
            var pc = new ParameterCollection();
            pc.Add<int?>("age", null);
            Assert.Throws<KeyNotFoundException>(() => pc.GetByKey<int>("age"));
        }

        [Test]
        public void ExistingParameter_NullValue_NullableRequest_ReturnsNull()
        {
            var pc = new ParameterCollection();
            pc.Add<int?>("age", null);
            var v = pc.GetByKey<int?>("age");
            Assert.That(v, Is.Null);
        }

        [Test]
        public void ExistingParameter_NullValue_ReferenceRequest_ReturnsNull()
        {
            var pc = new ParameterCollection();
            pc.Add<string>("note", null);
            var s = pc.GetByKey<string>("note");
            Assert.That(s, Is.Null);

            var p = pc.GetParameterByKey("note");
            var s2 = p.GetValue<string>();
            Assert.That(s2, Is.Null);
        }

        [Test]
        public void ParameterGetValue_ReturnsNullForNullable()
        {
            var pc = new ParameterCollection();
            pc.Add<int?>("maybe", null);
            var p = pc.GetParameterByKey("maybe");
            var v = p.GetValue<int?>();
            Assert.That(v, Is.Null);
        }
    }
}
