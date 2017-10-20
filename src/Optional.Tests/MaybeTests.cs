using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

#if !NETSTANDARD10
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Optional.Tests
{
    [TestClass]
    public class MaybeTests
    {
        [TestMethod]
        public void Maybe_CreateAndCheckExistence()
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
        public void Maybe_CheckContainment()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.IsFalse(noneStruct.Contains(0));
            Assert.IsFalse(noneNullable.Contains(null));
            Assert.IsFalse(noneClass.Contains(null));

            Assert.IsFalse(noneStruct.Exists(val => true));
            Assert.IsFalse(noneNullable.Exists(val => true));
            Assert.IsFalse(noneClass.Exists(val => true));

            var someStruct = Option.Some<int>(1);
            var someNullable = Option.Some<int?>(1);
            var someNullableEmpty = Option.Some<int?>(null);
            var someClass = Option.Some("1");
            var someClassNull = Option.Some<string>(null);

            Assert.IsTrue(someStruct.Contains(1));
            Assert.IsTrue(someNullable.Contains(1));
            Assert.IsTrue(someNullableEmpty.Contains(null));
            Assert.IsTrue(someClass.Contains("1"));
            Assert.IsTrue(someClassNull.Contains(null));

            Assert.IsTrue(someStruct.Exists(val => val == 1));
            Assert.IsTrue(someNullable.Exists(val => val == 1));
            Assert.IsTrue(someNullableEmpty.Exists(val => val == null));
            Assert.IsTrue(someClass.Exists(val => val == "1"));
            Assert.IsTrue(someClassNull.Exists(val => val == null));

            Assert.IsFalse(someStruct.Contains(-1));
            Assert.IsFalse(someNullable.Contains(-1));
            Assert.IsFalse(someNullableEmpty.Contains(1));
            Assert.IsFalse(someClass.Contains("-1"));
            Assert.IsFalse(someClassNull.Contains("1"));

            Assert.IsFalse(someStruct.Exists(val => val != 1));
            Assert.IsFalse(someNullable.Exists(val => val != 1));
            Assert.IsFalse(someNullableEmpty.Exists(val => val != null));
            Assert.IsFalse(someClass.Exists(val => val != "1"));
            Assert.IsFalse(someClassNull.Exists(val => val != null));
        }

        [TestMethod]
        public void Maybe_Equality()
        {
            // Basic equality
            Assert.AreEqual(Option.None<string>(), Option.None<string>());

            Assert.AreEqual(Option.Some<string>("val"), Option.Some<string>("val"));
            Assert.AreEqual(Option.Some<string>(null), Option.Some<string>(null));
            Assert.AreNotEqual(Option.Some<string>("val"), Option.Some<string>(null));
            Assert.AreNotEqual(Option.Some<string>(null), Option.Some<string>("val"));
            Assert.AreNotEqual(Option.Some<string>("val"), Option.Some<string>("val1"));

            // Must have same types
            Assert.AreNotEqual(Option.None<string>(), Option.None<object>());
            Assert.AreNotEqual(Option.Some<string>("val"), Option.Some<object>("val"));

            // Some and None are different
            Assert.AreNotEqual(Option.Some<string>("ex"), Option.None<string>());
            Assert.AreNotEqual(Option.Some<string>(null), Option.None<string>());

            // Works for val. types, nullables and ref. types
            Assert.AreEqual(Option.None<int>(), Option.None<int>());
            Assert.AreEqual(Option.None<int?>(), Option.None<int?>());
            Assert.AreEqual(Option.None<string>(), Option.None<string>());

            Assert.AreEqual(Option.Some<int>(1), Option.Some<int>(1));
            Assert.AreEqual(Option.Some<int?>(1), Option.Some<int?>(1));
            Assert.AreEqual(Option.Some<string>("1"), Option.Some<string>("1"));
            Assert.AreNotEqual(Option.Some<int>(1), Option.Some<int>(-1));
            Assert.AreNotEqual(Option.Some<int?>(1), Option.Some<int?>(-1));
            Assert.AreNotEqual(Option.Some<string>("1"), Option.Some<string>("-1"));

            // Works when when boxed
            Assert.AreEqual((object)Option.None<int>(), (object)Option.None<int>());
            Assert.AreEqual((object)Option.Some(22), (object)Option.Some(22));
            Assert.AreNotEqual((object)Option.None<int>(), (object)Option.Some(22));
            Assert.AreNotEqual((object)Option.Some(21), (object)Option.Some(22));

            // Works with default equalitycomparer 
            Assert.IsTrue(EqualityComparer<Option<int>>.Default.Equals(Option.None<int>(), Option.None<int>()));
            Assert.IsTrue(EqualityComparer<Option<int>>.Default.Equals(Option.Some(22), Option.Some(22)));
            Assert.IsFalse(EqualityComparer<Option<int>>.Default.Equals(Option.Some(22), Option.None<int>()));
            Assert.IsFalse(EqualityComparer<Option<int>>.Default.Equals(Option.Some(22), Option.Some(21)));

            // Works with equality operators
            Assert.IsTrue(Option.None<int>() == Option.None<int>());
            Assert.IsTrue(Option.Some(22) == Option.Some(22));
            Assert.IsTrue(Option.Some(22) != Option.None<int>());
            Assert.IsTrue(Option.Some(22) != Option.Some(21));
        }

        [TestMethod]
        public void Maybe_CompareTo()
        {
            void LessThan<TValue>(Option<TValue> lesser, Option<TValue> greater)
            {
                Assert.IsTrue(lesser.CompareTo(greater) < 0);
                Assert.IsTrue(greater.CompareTo(lesser) > 0);
                Assert.IsTrue(lesser < greater);
                Assert.IsTrue(lesser <= greater);
                Assert.IsTrue(greater > lesser);
                Assert.IsTrue(greater >= lesser);
            }

            void EqualTo<TValue>(Option<TValue> left, Option<TValue> right)
            {
                Assert.IsTrue(left.CompareTo(right) == 0);
                Assert.IsTrue(right.CompareTo(left) == 0);
                Assert.IsTrue(left <= right);
                Assert.IsTrue(left >= right);
                Assert.IsTrue(right <= left);
                Assert.IsTrue(right >= left);
            }

            // Value type comparisons
            var noneStruct = Option.None<int>();
            var someStruct1 = Option.Some<int>(1);
            var someStruct2 = Option.Some<int>(2);

            LessThan(noneStruct, someStruct1);
            LessThan(someStruct1, someStruct2);

            EqualTo(noneStruct, noneStruct);
            EqualTo(someStruct1, someStruct1);

            // IComparable comparisons
            var noneComparable = Option.None<string>();
            var someComparableNull = Option.Some<string>(null);
            var someComparable1 = Option.Some<string>("1");
            var someComparable2 = Option.Some<string>("2");

            LessThan(noneComparable, someComparable1);
            LessThan(noneComparable, someComparableNull);
            LessThan(someComparableNull, someComparable1);
            LessThan(someComparable1, someComparable2);

            EqualTo(noneComparable, noneComparable);
            EqualTo(someComparableNull, someComparableNull);
            EqualTo(someComparable1, someComparable1);

            // Non-IComparable comparisons
            var noneNotComparable = Option.None<Dictionary<string, string>>();
            var someNotComparableNull = Option.Some<Dictionary<string, string>>(null);
            var someNotComparable1 = Option.Some<Dictionary<string, string>>(new Dictionary<string, string>());
            var someNotComparable2 = Option.Some<Dictionary<string, string>>(new Dictionary<string, string>());

            Assert.ThrowsException<ArgumentException>(() => someNotComparable1.CompareTo(someNotComparable2));
            Assert.ThrowsException<ArgumentException>(() => someNotComparable2.CompareTo(someNotComparable1));

            LessThan(noneNotComparable, someNotComparable1);
            LessThan(noneNotComparable, someNotComparableNull);
            LessThan(someNotComparableNull, someNotComparable1);

            EqualTo(noneNotComparable, noneNotComparable);
            EqualTo(someNotComparableNull, someNotComparableNull);
            EqualTo(someNotComparable1, someNotComparable1);
        }

        [TestMethod]
        public void Maybe_Hashing()
        {
            Assert.AreEqual(Option.None<string>().GetHashCode(), Option.None<string>().GetHashCode());
            Assert.AreEqual(Option.None<object>().GetHashCode(), Option.None<object>().GetHashCode());

            Assert.AreEqual(Option.Some<string>("val").GetHashCode(), Option.Some<string>("val").GetHashCode());
            Assert.AreEqual(Option.Some<object>("val").GetHashCode(), Option.Some<object>("val").GetHashCode());

            Assert.AreEqual(Option.Some<string>(null).GetHashCode(), Option.Some<string>(null).GetHashCode());
            Assert.AreEqual(Option.Some<object>(null).GetHashCode(), Option.Some<object>(null).GetHashCode());

            Assert.AreNotEqual(Option.Some<string>(null).GetHashCode(), Option.None<string>().GetHashCode());
            Assert.AreNotEqual(Option.Some<object>(null).GetHashCode(), Option.None<object>().GetHashCode());
        }

        [TestMethod]
        public void Maybe_StringRepresentation()
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
        public void Maybe_GetValue()
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
        public void Maybe_GetValueLazy()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.AreEqual(noneStruct.ValueOr(() => -1), -1);
            Assert.AreEqual(noneNullable.ValueOr(() => -1), -1);
            Assert.AreEqual(noneClass.ValueOr(() => "-1"), "-1");

            var someStruct = Option.Some<int>(1);
            var someNullable = Option.Some<int?>(1);
            var someNullableEmpty = Option.Some<int?>(null);
            var someClass = Option.Some("1");
            var someClassNull = Option.Some<string>(null);

            Assert.AreEqual(someStruct.ValueOr(() => -1), 1);
            Assert.AreEqual(someNullable.ValueOr(() => -1), 1);
            Assert.AreEqual(someNullableEmpty.ValueOr(() => -1), null);
            Assert.AreEqual(someClass.ValueOr(() => "-1"), "1");
            Assert.AreEqual(someClassNull.ValueOr(() => "-1"), null);

            Assert.AreEqual(someStruct.ValueOr(() => { Assert.Fail(); return -1; }), 1);
            Assert.AreEqual(someNullable.ValueOr(() => { Assert.Fail(); return -1; }), 1);
            Assert.AreEqual(someNullableEmpty.ValueOr(() => { Assert.Fail(); return -1; }), null);
            Assert.AreEqual(someClass.ValueOr(() => { Assert.Fail(); return "-1"; }), "1");
            Assert.AreEqual(someClassNull.ValueOr(() => { Assert.Fail(); return "-1"; }), null);
        }

        [TestMethod]
        public void Maybe_AlternativeValue()
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
        public void Maybe_AlternativeOption()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.IsFalse(noneStruct.HasValue);
            Assert.IsFalse(noneNullable.HasValue);
            Assert.IsFalse(noneClass.HasValue);

            var noneStruct2 = noneStruct.Else(Option.None<int>());
            var noneNullable2 = noneNullable.Else(Option.None<int?>());
            var noneClass2 = noneClass.Else(Option.None<string>());

            Assert.IsFalse(noneStruct2.HasValue);
            Assert.IsFalse(noneNullable2.HasValue);
            Assert.IsFalse(noneClass2.HasValue);

            var someStruct = noneStruct.Else(1.Some());
            var someNullable = noneNullable.Else(Option.Some<int?>(1));
            var someClass = noneClass.Else("1".Some());

            Assert.IsTrue(someStruct.HasValue);
            Assert.IsTrue(someNullable.HasValue);
            Assert.IsTrue(someClass.HasValue);

            Assert.AreEqual(someStruct.ValueOr(-1), 1);
            Assert.AreEqual(someNullable.ValueOr(-1), 1);
            Assert.AreEqual(someClass.ValueOr("-1"), "1");
        }

        [TestMethod]
        public void Maybe_AlternativeValueLazy()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.IsFalse(noneStruct.HasValue);
            Assert.IsFalse(noneNullable.HasValue);
            Assert.IsFalse(noneClass.HasValue);

            var someStruct = noneStruct.Or(() => 1);
            var someNullable = noneNullable.Or(() => 1);
            var someClass = noneClass.Or(() => "1");

            Assert.IsTrue(someStruct.HasValue);
            Assert.IsTrue(someNullable.HasValue);
            Assert.IsTrue(someClass.HasValue);

            Assert.AreEqual(someStruct.ValueOr(() => -1), 1);
            Assert.AreEqual(someNullable.ValueOr(() => -1), 1);
            Assert.AreEqual(someClass.ValueOr(() => "-1"), "1");

            someStruct.Or(() => { Assert.Fail(); return -1; });
            someNullable.Or(() => { Assert.Fail(); return -1; });
            someClass.Or(() => { Assert.Fail(); return "-1"; });
        }

        [TestMethod]
        public void Maybe_AlternativeOptionLazy()
        {
            var noneStruct = Option.None<int>();
            var noneNullable = Option.None<int?>();
            var noneClass = Option.None<string>();

            Assert.IsFalse(noneStruct.HasValue);
            Assert.IsFalse(noneNullable.HasValue);
            Assert.IsFalse(noneClass.HasValue);

            var noneStruct2 = noneStruct.Else(() => Option.None<int>());
            var noneNullable2 = noneNullable.Else(() => Option.None<int?>());
            var noneClass2 = noneClass.Else(() => Option.None<string>());

            Assert.IsFalse(noneStruct2.HasValue);
            Assert.IsFalse(noneNullable2.HasValue);
            Assert.IsFalse(noneClass2.HasValue);

            var someStruct = noneStruct.Else(() => 1.Some());
            var someNullable = noneNullable.Else(() => Option.Some<int?>(1));
            var someClass = noneClass.Else(() => "1".Some());

            Assert.IsTrue(someStruct.HasValue);
            Assert.IsTrue(someNullable.HasValue);
            Assert.IsTrue(someClass.HasValue);

            Assert.AreEqual(someStruct.ValueOr(() => -1), 1);
            Assert.AreEqual(someNullable.ValueOr(() => -1), 1);
            Assert.AreEqual(someClass.ValueOr(() => "-1"), "1");

            someStruct.Else(() => { Assert.Fail(); return Option.None<int>(); });
            someNullable.Else(() => { Assert.Fail(); return Option.None<int?>(); });
            someClass.Else(() => { Assert.Fail(); return Option.None<string>(); });
        }

        [TestMethod]
        public void Maybe_CreateExtensions()
        {
            var none = 1.None();
            var some = 1.Some();

            Assert.AreEqual(none.ValueOr(-1), -1);
            Assert.AreEqual(some.ValueOr(-1), 1);

            var noneLargerThanTen = 1.SomeWhen(x => x > 10);
            var someLargerThanTen = 20.SomeWhen(x => x > 10);

            Assert.AreEqual(noneLargerThanTen.ValueOr(-1), -1);
            Assert.AreEqual(someLargerThanTen.ValueOr(-1), 20);

            someLargerThanTen = 1.NoneWhen(x => x > 10);
            noneLargerThanTen = 20.NoneWhen(x => x > 10);

            Assert.AreEqual(someLargerThanTen.ValueOr(-1), 1);
            Assert.AreEqual(noneLargerThanTen.ValueOr(-1), -1);

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
        public void Maybe_Matching()
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

            var hasMatched = false;
            none.Match(
                some: x => Assert.Fail(),
                none: () => hasMatched = true
            );
            Assert.IsTrue(hasMatched);

            hasMatched = false;
            some.Match(
                some: x => hasMatched = x == 1,
                none: () => Assert.Fail()
            );
            Assert.IsTrue(hasMatched);

            none.MatchSome(x => Assert.Fail());
            hasMatched = false;
            some.MatchSome(x => hasMatched = x == 1);
            Assert.IsTrue(hasMatched);

            some.MatchNone(() => Assert.Fail());
            hasMatched = false;
            none.MatchNone(() => hasMatched = true);
            Assert.IsTrue(hasMatched);
        }

        [TestMethod]
        public void Maybe_Transformation()
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
        }

        [TestMethod]
        public void Maybe_Flatten()
        {
            var noneNone = Option.None<Option<string>>();
            var someNone = Option.Some(Option.None<string>());
            var someSome = Option.Some(Option.Some("a"));

            Assert.IsFalse(noneNone.HasValue);
            Assert.IsFalse(noneNone.Flatten().HasValue);

            Assert.IsTrue(someNone.HasValue);
            Assert.IsFalse(someNone.Flatten().HasValue);

            Assert.IsTrue(someSome.HasValue);
            Assert.IsTrue(someSome.Flatten().HasValue);
            Assert.AreEqual(someSome.Flatten().ValueOr("b"), "a");
        }

        [TestMethod]
        public void Maybe_Filtering()
        {
            var none = "a".None();
            var some = "a".Some();

            var noneNotA = none.Filter(x => x != "a");
            var someNotA = some.Filter(x => x != "a");
            var noneA = none.Filter(x => x == "a");
            var someA = some.Filter(x => x == "a");

            Assert.IsFalse(noneNotA.HasValue);
            Assert.IsFalse(someNotA.HasValue);
            Assert.IsFalse(noneA.HasValue);
            Assert.IsTrue(someA.HasValue);

            var noneFalse = none.Filter(false);
            var someFalse = some.Filter(false);
            var noneTrue = none.Filter(true);
            var someTrue = some.Filter(true);

            Assert.IsFalse(noneFalse.HasValue);
            Assert.IsFalse(someFalse.HasValue);
            Assert.IsFalse(noneTrue.HasValue);
            Assert.IsTrue(someTrue.HasValue);

            var someNull = Option.Some<string>(null);
            Assert.IsTrue(someNull.HasValue);
            Assert.IsFalse(someNull.NotNull().HasValue);

            var someNullableNull = Option.Some<int?>(null);
            Assert.IsTrue(someNullableNull.HasValue);
            Assert.IsFalse(someNullableNull.NotNull().HasValue);

            var someStructNull = Option.Some(default(int));
            Assert.IsTrue(someStructNull.HasValue);
            Assert.IsTrue(someStructNull.NotNull().HasValue);

            Assert.IsTrue(some.HasValue);
            Assert.IsTrue(some.NotNull().HasValue);

            Assert.IsFalse(none.HasValue);
            Assert.IsFalse(none.NotNull().HasValue);
        }

        [TestMethod]
        public void Maybe_ToEnumerable()
        {
            var none = "a".None();
            var some = "a".Some();

            var noneAsEnumerable = none.ToEnumerable();
            var someAsEnumerable = some.ToEnumerable();

            foreach (var value in noneAsEnumerable)
            {
                Assert.Fail();
            }

            int count = 0;
            foreach (var value in someAsEnumerable)
            {
                Assert.AreEqual(value, "a");
                count += 1;
            }
            Assert.AreEqual(count, 1);

            foreach (var value in someAsEnumerable)
            {
                Assert.AreEqual(value, "a");
                count += 1;
            }
            Assert.AreEqual(count, 2);

            Assert.AreEqual(noneAsEnumerable.Count(), 0);
            Assert.AreEqual(someAsEnumerable.Count(), 1);
        }

        [TestMethod]
        public void Maybe_Enumerate()
        {
            var none = "a".None();
            var some = "a".Some();

            foreach (var value in none)
            {
                Assert.Fail();
            }

            int count = 0;
            foreach (var value in some)
            {
                Assert.AreEqual(value, "a");
                count += 1;
            }
            Assert.AreEqual(count, 1);

            foreach (var value in some)
            {
                Assert.AreEqual(value, "a");
                count += 1;
            }
            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void Maybe_Default()
        {
            var none1 = default(Option<int>);
            var none2 = default(Option<int?>);
            var none3 = default(Option<string>);

            Assert.IsFalse(none1.HasValue);
            Assert.IsFalse(none2.HasValue);
            Assert.IsFalse(none3.HasValue);

            var some1 = none1.Or(1);
            var some2 = none2.Or(1);
            var some3 = none3.Or("1");

            Assert.AreEqual(some1.ValueOr(-1), 1);
            Assert.AreEqual(some2.ValueOr(-1), 1);
            Assert.AreEqual(some3.ValueOr("-1"), "1");
        }

#if !NETSTANDARD10
        [TestMethod]
        public void Maybe_Serialization()
        {
            var some = Option.Some("1");
            var none = Option.None<string>();

            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, some);
                stream.Position = 0;
                var someDeserialized = (Option<string>)formatter.Deserialize(stream);
                Assert.AreEqual(some, someDeserialized);
            }

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, none);
                stream.Position = 0;
                var noneDeserialized = (Option<string>)formatter.Deserialize(stream);
                Assert.AreEqual(none, noneDeserialized);
            }
        }
#endif
    }
}
