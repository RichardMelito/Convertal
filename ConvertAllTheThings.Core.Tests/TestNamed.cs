using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.MaybeNamed;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public class TestNamed
    {
        class TestNamedClass : MaybeNamed
        {
            static TestNamedClass()
            {
                AddTypeToDictionary<TestNamedClass>();
            }

            public TestNamedClass(string name)
                : base(name)
            {

            }
        }

        class OtherTestNamedClass : MaybeNamed
        {
            static OtherTestNamedClass()
            {
                AddTypeToDictionary<OtherTestNamedClass>();
            }

            public OtherTestNamedClass(string name)
                : base(name)
            {

            }
        }

        /*  
         *  Construction
         */

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            new TestNamedClass("name1");
            new TestNamedClass("name2");
            new TestNamedClass("uniqueName");

            new OtherTestNamedClass("name1");
        }

        [TestMethod]
        public void TestTryGetFromName()
        {
            Assert.IsTrue(TryGetFromName<TestNamedClass>(
                "uniqueName",
                out var uniqueName1));

            Assert.IsTrue(TryGetFromName<TestNamedClass>(
                "uniqueName",
                out var uniqueName2));
            Assert.AreSame(uniqueName1, uniqueName2);

            Assert.IsTrue(TryGetFromName<TestNamedClass>(
                "name1",
                out var name1));

            Assert.IsTrue(TryGetFromName<OtherTestNamedClass>(
                "name1",
                out var otherName1));
            Assert.AreNotSame(name1, otherName1);

            Assert.IsFalse(TryGetFromName<TestNamedClass>(
                "DNE",
                out var namedObj));
            Assert.IsNull(namedObj);
        }

        [TestMethod]
        public void TestGetFromName()
        {
            var uniqueName1 = GetFromName<TestNamedClass>("uniqueName");
            var uniqueName2 = GetFromName<TestNamedClass>("uniqueName");
            Assert.AreSame(uniqueName1, uniqueName2);

            var name1 = GetFromName<TestNamedClass>("name1");
            var otherName1 = GetFromName<OtherTestNamedClass>("name1");
            Assert.AreNotSame(name1, otherName1);

            Assert.ThrowsException<InvalidOperationException>(
                () => GetFromName<TestNamedClass>("DNE"));
        }

        [TestMethod]
        public void TestNameAlreadyRegistered()
        {
            Assert.IsTrue(NameAlreadyRegistered<TestNamedClass>("name1"));
            Assert.IsTrue(NameAlreadyRegistered<OtherTestNamedClass>("name1"));
            Assert.IsTrue(NameAlreadyRegistered<TestNamedClass>("name2"));
            Assert.IsFalse(NameAlreadyRegistered<OtherTestNamedClass>("name2"));
            Assert.IsFalse(NameAlreadyRegistered<TestNamedClass>("DNE"));
        }

        [TestMethod]
        public void TestNameAndNameSpaceValid()
        {
            Assert.IsFalse(NameIsValid<TestNamedClass>("name1"));
            Assert.IsTrue(NameIsValid<TestNamedClass>("DNE"));
        }

        [TestMethod]
        public void TestChangeNameAndNameSpace()
        {
            using TestNamedClass toTest = new("name3");
            
            toTest.ChangeName("newName");
            Assert.AreEqual("newName", toTest.MaybeName);

            Assert.ThrowsException<InvalidOperationException>(
                () => toTest.ChangeName("name2"));
        }

        [TestMethod]
        public void TestDispose()
        {
            using (TestNamedClass toTest = new("name3"))
            {
                Assert.IsTrue(TryGetFromName<TestNamedClass>(
                    "name3",
                    out var same));

                Assert.AreSame(toTest, same);
            }

            Assert.IsFalse(TryGetFromName<TestNamedClass>(
                "name3",
                out var shouldBeNull));

            Assert.IsNull(shouldBeNull);
        }

        [TestMethod]
        public void TestConstruction()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new TestNamedClass(""));

            Assert.ThrowsException<ArgumentException>(
                () => new TestNamedClass("\t"));

            Assert.ThrowsException<InvalidOperationException>(
                () => new TestNamedClass("name1"));
        }
    }
}
