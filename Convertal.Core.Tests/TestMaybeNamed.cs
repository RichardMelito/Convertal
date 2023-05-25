// Created by Richard Melito and licensed to you under The Clear BSD License.

using Convertal.Core.Extensions;
using Xunit;

namespace Convertal.Core.Tests;

public class TestMaybeNamed : BaseTestClass
{
    class TestNamedClass : MaybeNamed
    {
        public TestNamedClass(Database database, string name)
            : base(database, name)
        {
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        public override MaybeNamedProto ToProto()
        {
            return new(Name, Symbol);
        }
    }

    class OtherTestNamedClass : MaybeNamed
    {
        public OtherTestNamedClass(Database database, string name)
            : base(database, name)
        {
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        public override MaybeNamedProto ToProto()
        {
            throw new NotImplementedException();
        }
    }

    /*  
     *  Construction
     */


    public TestMaybeNamed()
    {
        _ = new TestNamedClass(Database, "name1");
        _ = new TestNamedClass(Database, "name2");
        _ = new TestNamedClass(Database, "uniqueName");

        _ = new OtherTestNamedClass(Database, "name1");
    }

    [Fact]
    public void TestTryGetFromName()
    {
        Assert.True(Database.TryGetFromName<TestNamedClass>(
            "uniqueName",
            out var uniqueName1));

        Assert.True(Database.TryGetFromName<TestNamedClass>(
            "uniqueName",
            out var uniqueName2));
        Assert.Same(uniqueName1, uniqueName2);

        Assert.True(Database.TryGetFromName<TestNamedClass>(
            "name1",
            out var name1));

        Assert.True(Database.TryGetFromName<OtherTestNamedClass>(
            "name1",
            out var otherName1));
        Assert.NotSame(name1, otherName1);

        Assert.False(Database.TryGetFromName<TestNamedClass>(
            "DNE",
            out var namedObj));
        Assert.Null(namedObj);
    }

    [Fact]
    public void TestGetFromName()
    {
        var uniqueName1 = Database.GetFromName<TestNamedClass>("uniqueName");
        var uniqueName2 = Database.GetFromName<TestNamedClass>("uniqueName");
        Assert.Same(uniqueName1, uniqueName2);

        var name1 = Database.GetFromName<TestNamedClass>("name1");
        var otherName1 = Database.GetFromName<OtherTestNamedClass>("name1");
        Assert.NotSame(name1, otherName1);

        Assert.Throws<InvalidOperationException>(
            () => Database.GetFromName<TestNamedClass>("DNE"));
    }

    [Fact]
    public void TestNameAlreadyRegistered()
    {
        Assert.True(Database.NameAlreadyRegistered<TestNamedClass>("name1"));
        Assert.True(Database.NameAlreadyRegistered<OtherTestNamedClass>("name1"));
        Assert.True(Database.NameAlreadyRegistered<TestNamedClass>("name2"));
        Assert.False(Database.NameAlreadyRegistered<OtherTestNamedClass>("name2"));
        Assert.False(Database.NameAlreadyRegistered<TestNamedClass>("DNE"));
    }

    [Fact]
    public void TestNameIsValid()
    {
        Assert.False(Database.NameIsValid<TestNamedClass>("name1"));
        Assert.True(Database.NameIsValid<TestNamedClass>("DNE"));
    }

    [Fact]
    public void TestChangeName()
    {
        TestNamedClass toTest = new(Database, "name3");

        toTest.ChangeName("newName");
        Assert.Equal("newName", toTest.Name);

        Assert.Throws<InvalidOperationException>(
            () => toTest.ChangeName("name2"));
    }

    [Fact]
    public void TestDispose()
    {
        using (TestNamedClass toTest = new(Database, "name3"))
        {
            Assert.True(Database.TryGetFromName<TestNamedClass>(
                "name3",
                out var same));

            Assert.Same(toTest, same);
        }

        Assert.False(Database.TryGetFromName<TestNamedClass>(
            "name3",
            out var shouldBeNull));

        Assert.Null(shouldBeNull);
    }

    [Fact]
    public void TestConstruction()
    {
        Assert.Throws<ArgumentException>(
            () => new TestNamedClass(Database, ""));

        Assert.Throws<ArgumentException>(
            () => new TestNamedClass(Database, "\t"));

        Assert.Throws<InvalidOperationException>(
            () => new TestNamedClass(Database, "name1"));
    }
}
