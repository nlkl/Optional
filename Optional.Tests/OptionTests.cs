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
        public void Equality()
        {
            Assert.AreEqual(Option.None<int>(), Option.None<int>());
            Assert.AreEqual(Option.None<int?>(), Option.None<int?>());
            Assert.AreEqual(Option.None<string>(), Option.None<string>());

            Assert.AreNotEqual(Option.None<int>(), Option.None<double>());
            Assert.AreNotEqual(Option.None<int?>(), Option.None<double?>());
            Assert.AreNotEqual(Option.None<string>(), Option.None<object>());

            Assert.AreEqual(Option.Some(1), Option.Some(1));
            Assert.AreEqual(Option.Some<int?>(1), Option.Some<int?>(1));
            Assert.AreEqual(Option.Some<int?>(null), Option.Some<int?>(null));
            Assert.AreEqual(Option.Some("1"), Option.Some("1"));
            Assert.AreEqual(Option.Some<string>(null), Option.Some<string>(null));

            Assert.AreNotEqual(Option.Some(1), Option.Some(2));
            Assert.AreNotEqual(Option.Some<int?>(1), Option.Some<int?>(2));
            Assert.AreNotEqual(Option.Some<int?>(1), Option.Some<int?>(null));
            Assert.AreNotEqual(Option.Some("1"), Option.Some("2"));
            Assert.AreNotEqual(Option.Some("2"), Option.Some<string>(null));

            Assert.AreNotEqual(Option.Some(1), Option.Some<long>(1));
            Assert.AreNotEqual(Option.Some<int?>(1), Option.Some<int>(1));
            Assert.AreNotEqual(Option.Some<string>(null), Option.Some<int?>(null));
            Assert.AreNotEqual(Option.Some("1"), Option.Some(DateTime.Now));
            Assert.AreNotEqual(Option.Some<string>(null), Option.Some<object>(null));
        }

        [TestMethod]
        public void Hashing()
        {
            // None all have identical hash codes
            Assert.AreEqual(Option.None<int>().GetHashCode(), Option.None<int>().GetHashCode());
            Assert.AreEqual(Option.None<int?>().GetHashCode(), Option.None<int?>().GetHashCode());
            Assert.AreEqual(Option.None<string>().GetHashCode(), Option.None<string>().GetHashCode());

            Assert.AreEqual(Option.None<int>().GetHashCode(), Option.None<double>().GetHashCode());
            Assert.AreEqual(Option.None<int?>().GetHashCode(), Option.None<double?>().GetHashCode());
            Assert.AreEqual(Option.None<string>().GetHashCode(), Option.None<object>().GetHashCode());
            
            Assert.AreEqual(Option.None<int>().GetHashCode(), Option.None<string>().GetHashCode());
            Assert.AreEqual(Option.None<int>().GetHashCode(), Option.None<int?>().GetHashCode());
            Assert.AreEqual(Option.None<int?>().GetHashCode(), Option.None<string>().GetHashCode());

            // Null values return identical hash codes
            Assert.AreEqual(Option.Some<int?>(null).GetHashCode(), Option.Some<int?>(null).GetHashCode());
            Assert.AreEqual(Option.Some<string>(null).GetHashCode(), Option.Some<string>(null).GetHashCode());

            Assert.AreEqual(Option.Some<int?>(null).GetHashCode(), Option.Some<double?>(null).GetHashCode());
            Assert.AreEqual(Option.Some<string>(null).GetHashCode(), Option.Some<object>(null).GetHashCode());

            Assert.AreEqual(Option.Some<int?>(null).GetHashCode(), Option.Some<string>(null).GetHashCode());

            // Some values 
            Assert.AreEqual(Option.Some(1).GetHashCode(), Option.Some(1).GetHashCode());
            Assert.AreEqual(Option.Some<int?>(1).GetHashCode(), Option.Some<int?>(1).GetHashCode());
            Assert.AreEqual(Option.Some("1").GetHashCode(), Option.Some("1").GetHashCode());

            Assert.AreNotEqual(Option.Some(1).GetHashCode(), Option.Some(2).GetHashCode());
            Assert.AreNotEqual(Option.Some<int?>(1).GetHashCode(), Option.Some<int?>(2).GetHashCode());
            Assert.AreNotEqual(Option.Some("1").GetHashCode(), Option.Some("2").GetHashCode());

            Assert.AreNotEqual(Option.Some(1).GetHashCode(), Option.None<int>().GetHashCode());
            Assert.AreNotEqual(Option.Some<int?>(1).GetHashCode(), Option.None<int?>().GetHashCode());
            Assert.AreNotEqual(Option.Some("1").GetHashCode(), Option.None<string>().GetHashCode());

            // None and null are different
            Assert.AreNotEqual(Option.None<int?>().GetHashCode(), Option.Some<int?>(null).GetHashCode());
            Assert.AreNotEqual(Option.None<string>().GetHashCode(), Option.Some<string>(null).GetHashCode());
        }

        [TestMethod]
        public void StringRepresentation()
        {
            Assert.AreEqual(Option.None<int>().ToString(), "None");
            Assert.AreEqual(Option.None<int?>().ToString(), "None");
            Assert.AreEqual(Option.None<string>().ToString(), "None");

            Assert.AreEqual(Option.Some<int?>(null).ToString(), "Some(null)");
            Assert.AreEqual(Option.Some<string>(null).ToString(), "Some(null)");

            Assert.AreEqual(Option.Some<int>(1).ToString(), "Some(1)");
            Assert.AreEqual(Option.Some<int?>(1).ToString(), "Some(1)");
            Assert.AreEqual(Option.Some<string>("1").ToString(), "Some(1)");

            var now = DateTime.Now;
            Assert.AreEqual(Option.Some<DateTime>(now).ToString(), "Some(" + now.ToString() + ")");
        }

        [TestMethod]
        public void GetValue()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.AreEqual(noneStruct.ValueOr(-1), -1);
            Assert.AreEqual(noneNullable.ValueOr(-1), -1);
            Assert.AreEqual(noneClass.ValueOr("-1"), "-1");

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
        public void AlternativeValue()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.IsFalse(noneStruct.HasValue);
            Assert.IsFalse(noneNullable.HasValue);
            Assert.IsFalse(noneClass.HasValue);

            var someStruct = noneStruct.Or(1);
            var someNullable = noneNullable.Or(1);
            var someClass = noneClass.Or("1");

            Assert.IsTrue(someStruct.HasValue);
            Assert.IsTrue(someNullable.HasValue);
            Assert.IsTrue(someClass.HasValue);

            Assert.AreEqual(someStruct.ValueOr(-1), 1);
            Assert.AreEqual(someNullable.ValueOr(-1), 1);
            Assert.AreEqual(someClass.ValueOr("-1"), "1");
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
