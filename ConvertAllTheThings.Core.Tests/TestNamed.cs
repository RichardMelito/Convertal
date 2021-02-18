using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.Named;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public class TestNamed
    {
        class TestNamedClass : Named
        {
            static TestNamedClass()
            {
                AddTypeToDictionary<TestNamedClass>();
            }

            public TestNamedClass(string name, string nameSpace)
                : base(name, nameSpace)
            {

            }
        }

        class OtherTestNamedClass : Named
        {
            static OtherTestNamedClass()
            {
                AddTypeToDictionary<OtherTestNamedClass>();
            }

            public OtherTestNamedClass(string name, string nameSpace)
                : base(name, nameSpace)
            {

            }
        }

        /*  GetFromName
         *  TryGetFromName
         *  Construction
         *  NameAndNameSpaceValid
         *  NameAlreadyRegistered
         *  Other types
         *  Dispose
         *  ChangeNameANdNameSpace
         */

        [AssemblyInitialize]
        public static void Initialize(TestContext _)
        {
            new TestNamedClass("name1", "ns1");
            new TestNamedClass("name1", "ns2");
            new TestNamedClass("name2", "ns1");
            new TestNamedClass("name2", "ns2");
            new TestNamedClass("uniqueName", "ns1");

            new OtherTestNamedClass("name1", "ns1");
        }

        [TestMethod]
        public void TestTryGetFromName()
        {
            NameLookupError error;

            Assert.IsTrue(TryGetFromName<TestNamedClass>(
                "uniqueName",
                out var uniqueName1,
                out error));
            Assert.AreEqual(NameLookupError.NoError, error);

            Assert.IsTrue(TryGetFromName<TestNamedClass>(
                "uniqueName",
                "ns1",
                out var uniqueName2,
                out error));
            Assert.AreEqual(NameLookupError.NoError, error);
            Assert.AreSame(uniqueName1, uniqueName2);

            Assert.IsTrue(TryGetFromName<TestNamedClass>(
                "name1",
                "ns1",
                out var name1,
                out error));
            Assert.AreEqual(NameLookupError.NoError, error);

            Assert.IsTrue(TryGetFromName<OtherTestNamedClass>(
                "name1",
                "ns1",
                out var otherName1,
                out error));
            Assert.AreEqual(NameLookupError.NoError, error);
            Assert.AreNotSame(name1, otherName1);

            Assert.IsFalse(TryGetFromName<TestNamedClass>(
                "DNE",
                out var namedObj,
                out error));
            Assert.AreEqual(NameLookupError.NoneFound, error);
            Assert.IsNull(namedObj);

            Assert.IsFalse(TryGetFromName<TestNamedClass>(
                "name1",
                out namedObj,
                out error));
            Assert.AreEqual(NameLookupError.NeedNamespace, error);
            Assert.IsNull(namedObj);
        }

        [TestMethod]
        public void TestGetFromName()
        {
            var uniqueName1 = GetFromName<TestNamedClass>("uniqueName");
            var uniqueName2 = GetFromName<TestNamedClass>("uniqueName", "ns1");
            Assert.AreSame(uniqueName1, uniqueName2);

            var name1 = GetFromName<TestNamedClass>("name1", "ns1");
            var otherName1 = GetFromName<OtherTestNamedClass>("name1", "ns1");
            Assert.AreNotSame(name1, otherName1);

            Assert.ThrowsException<InvalidOperationException>(
                () => GetFromName<TestNamedClass>("DNE"));

            Assert.ThrowsException<InvalidOperationException>(
                () => GetFromName<TestNamedClass>("name1"));
        }

        [TestMethod]
        public void TestNameAlreadyRegistered()
        {
            Assert.IsTrue(NameAlreadyRegistered<TestNamedClass>("name1", "ns1"));
            Assert.IsFalse(NameAlreadyRegistered<TestNamedClass>("DNE", "ns1"));
            Assert.IsFalse(NameAlreadyRegistered<TestNamedClass>("name1", "DNE"));
        }

        [TestMethod]
        public void TestNameAndNameSpaceValid()
        {
            Assert.IsFalse(NameAndNameSpaceValid<TestNamedClass>("name1", "ns1"));
            Assert.IsTrue(NameAndNameSpaceValid<TestNamedClass>("DNE", "ns1"));
            Assert.IsTrue(NameAndNameSpaceValid<TestNamedClass>("name1", "DNE"));
        }

        [TestMethod]
        public void TestChangeNameAndNameSpace()
        {
            TestNamedClass toTest = new("name3", "ns3");
            
            toTest.ChangeNameAndNameSpace("newName", "ns1");
            Assert.AreEqual("newName", toTest.Name);
            Assert.AreEqual("ns1", toTest.NameSpace);

            toTest.ChangeNameAndNameSpace("name1", "newNameSpace");
            Assert.AreEqual("name1", toTest.Name);
            Assert.AreEqual("newNameSpace", toTest.NameSpace);

            Assert.ThrowsException<InvalidOperationException>(
                () => toTest.ChangeNameAndNameSpace("name2", "ns2"));
        }
    }
}
