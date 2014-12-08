using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Optional.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void CreateAndCheckExistence()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.IsFalse(noneStruct.HasValue);
            Assert.IsFalse(noneNullable.HasValue);
            Assert.IsFalse(noneClass.HasValue);

            var someStruct = Option.Some<int>(1);
            var someNullable = Option.Some<int?>(1);
            var someNullableEmpty = Option.Some<int?>(null);
            var someClass = Option.Some("1");
            var someClassNull = Option.Some<string>(null);

            Assert.IsTrue(someStruct.HasValue);
            Assert.IsTrue(someNullable.HasValue);
            Assert.IsTrue(someNullableEmpty.HasValue);
            Assert.IsTrue(someClass.HasValue);
            Assert.IsTrue(someClassNull.HasValue);
        }

        [TestMethod]
        public void GetValue()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.AreEqual(noneStruct.ValueOr(-1), -1);
            Assert.AreEqual(noneNullable.ValueOr(-1), -1);
            Assert.AreEqual(noneClass.ValueOr("1"), "1");

            var someStruct = Option.Some<int>(1);
            var someNullable = Option.Some<int?>(1);
            var someNullableEmpty = Option.Some<int?>(null);
            var someClass = Option.Some("1");
            var someClassNull = Option.Some<string>(null);

            Assert.AreEqual(someStruct.ValueOr(-1), 1);
            Assert.AreEqual(someNullable.ValueOr(-1), 1);
            Assert.AreEqual(someNullableEmpty.ValueOr(-1), null);
            Assert.AreEqual(someClass.ValueOr("-1"), "1");
            Assert.AreEqual(someClassNull.ValueOr("-1"), null);
        }

        [TestMethod]
        public void CreateExtensions()
        {
            var none = 1.None();
            var some = 1.Some();

            Assert.AreEqual(none.ValueOr(-1), -1);
            Assert.AreEqual(some.ValueOr(-1), 1);

            var noneNotNull = ((string)null).SomeNotNull();
            var someNotNull = "1".SomeNotNull();

            Assert.AreEqual(noneNotNull.ValueOr("-1"), "-1");
            Assert.AreEqual(someNotNull.ValueOr("-1"), "1");

            var noneNullableNotNull = ((int?)null).SomeNotNull();
            var someNullableNotNull = ((int?)1).SomeNotNull();

            Assert.IsInstanceOfType(noneNullableNotNull.ValueOr(-1), typeof(int?));
            Assert.IsInstanceOfType(someNullableNotNull.ValueOr(-1), typeof(int?));
            Assert.AreEqual(noneNullableNotNull.ValueOr(-1), -1);
            Assert.AreEqual(someNullableNotNull.ValueOr(-1), 1);

            var noneFromNullable = ((int?)null).ToOption();
            var someFromNullable = ((int?)1).ToOption();

            Assert.IsInstanceOfType(noneFromNullable.ValueOr(-1), typeof(int));
            Assert.IsInstanceOfType(someFromNullable.ValueOr(-1), typeof(int));
            Assert.AreEqual(noneFromNullable.ValueOr(-1), -1);
            Assert.AreEqual(someFromNullable.ValueOr(-1), 1);
        }

        [TestMethod]
        public void Matching()
        {
            var none = 1.None();
            var some = 1.Some();

            var failure = none.Match(
                some: x => 2,
                none: () => -2
            );

            var success = some.Match(
                some: x => 2,
                none: () => -2
            );

            Assert.AreEqual(failure, -2);
            Assert.AreEqual(success, 2);

            none.Match(
                some: x => Assert.Fail(),
                none: () => { }
            );

            some.Match(
                some: x => { },
                none: () => Assert.Fail()
            );
        }

        [TestMethod]
        public void Transformation()
        {
            var none = "a".None();
            var some = "a".Some();

            var noneNull = ((string)null).None();
            var someNull = ((string)null).Some();

            var noneUpper = none.Map(x => x.ToUpper());
            var someUpper = some.Map(x => x.ToUpper());

            Assert.IsFalse(noneUpper.HasValue);
            Assert.IsTrue(someUpper.HasValue);
            Assert.AreEqual(noneUpper.ValueOr("b"), "b");
            Assert.AreEqual(someUpper.ValueOr("b"), "A");

            var noneNotNull = none.FlatMap(x => x.SomeNotNull());
            var someNotNull = some.FlatMap(x => x.SomeNotNull());
            var noneNullNotNull = noneNull.FlatMap(x => x.SomeNotNull());
            var someNullNotNull = someNull.FlatMap(x => x.SomeNotNull());

            Assert.IsFalse(noneNotNull.HasValue);
            Assert.IsTrue(someNotNull.HasValue);
            Assert.IsFalse(noneNullNotNull.HasValue);
            Assert.IsFalse(someNullNotNull.HasValue);

            var noneNotA = none.Filter(x => x != "a");
            var someNotA = some.Filter(x => x != "a");
            var noneA = none.Filter(x => x == "a");
            var someA = some.Filter(x => x == "a");

            Assert.IsFalse(noneNotA.HasValue);
            Assert.IsFalse(someNotA.HasValue);
            Assert.IsFalse(noneA.HasValue);
            Assert.IsTrue(someA.HasValue);
        }
    }
}
