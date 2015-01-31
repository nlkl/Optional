using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Tests
{
    [TestClass]
    public class EitherTests
    {
        [TestMethod]
        public void Either_CreateAndCheckExistence()
        {
            var noneStruct = Option.None<int, string>("ex");
            var noneNullable = Option.None<int?, string>("ex");
            var noneClass = Option.None<string, string>("ex");

            Assert.IsFalse(noneStruct.HasValue);
            Assert.IsFalse(noneNullable.HasValue);
            Assert.IsFalse(noneClass.HasValue);

            var someStruct = Option.Some<int, string>(1);
            var someNullable = Option.Some<int?, string>(1);
            var someNullableEmpty = Option.Some<int?, string>(null);
            var someClass = Option.Some<string, string>("1");
            var someClassNull = Option.Some<string, string>(null);

            Assert.IsTrue(someStruct.HasValue);
            Assert.IsTrue(someNullable.HasValue);
            Assert.IsTrue(someNullableEmpty.HasValue);
            Assert.IsTrue(someClass.HasValue);
            Assert.IsTrue(someClassNull.HasValue);
        }

        [TestMethod]
        public void Either_Equality()
        {
            Assert.AreEqual(Option.None<int, string>("ex"), Option.None<int, string>("ex"));
            Assert.AreEqual(Option.None<int?, string>("ex"), Option.None<int?, string>("ex"));
            Assert.AreEqual(Option.None<string, string>("ex"), Option.None<string, string>("ex"));

            Assert.AreNotEqual(Option.None<int, string>("ex"), Option.None<int, string>("ex1"));
            Assert.AreNotEqual(Option.None<int?, string>("ex"), Option.None<int?, string>("ex1"));
            Assert.AreNotEqual(Option.None<string, string>("ex"), Option.None<string, string>("ex1"));

            Assert.AreNotEqual(Option.None<int, int>(1), Option.None<int, double>(1));
            Assert.AreNotEqual(Option.None<int?, int?>(1), Option.None<int?, double?>(1));
            Assert.AreNotEqual(Option.None<string, string>("ex"), Option.None<string, object>("ex1"));

            Assert.AreNotEqual(Option.None<int, string>("ex"), Option.None<double, string>("ex"));
            Assert.AreNotEqual(Option.None<int?, string>("ex"), Option.None<double?, string>("ex"));
            Assert.AreNotEqual(Option.None<string, string>("ex"), Option.None<object, string>("ex"));

            Assert.AreEqual(Option.Some<int, string>(1), Option.Some<int, string>(1));
            Assert.AreEqual(Option.Some<int?, string>(1), Option.Some<int?, string>(1));
            Assert.AreEqual(Option.Some<int?, string>(null), Option.Some<int?, string>(null));
            Assert.AreEqual(Option.Some<string, string>("1"), Option.Some<string, string>("1"));
            Assert.AreEqual(Option.Some<string, string>(null), Option.Some<string, string>(null));

            Assert.AreNotEqual(Option.Some<int, string>(1), Option.Some<int, object>(1));
            Assert.AreNotEqual(Option.Some<int?, int?>(1), Option.Some<int?, int>(1));
            Assert.AreNotEqual(Option.Some<int?, int>(null), Option.Some<int?, int?>(null));
            Assert.AreNotEqual(Option.Some<string, float>("1"), Option.Some<string, double>("1"));
            Assert.AreNotEqual(Option.Some<string, long>(null), Option.Some<string, int>(null));

            Assert.AreNotEqual(Option.Some<int, string>(1), Option.Some<int, string>(2));
            Assert.AreNotEqual(Option.Some<int?, string>(1), Option.Some<int?, string>(2));
            Assert.AreNotEqual(Option.Some<int?, string>(1), Option.Some<int?, string>(null));
            Assert.AreNotEqual(Option.Some<string, string>("1"), Option.Some<string, string>("2"));
            Assert.AreNotEqual(Option.Some<string, string>("2"), Option.Some<string, string>(null));

            Assert.AreNotEqual(Option.Some<int, string>(1), Option.Some<long, string>(1));
            Assert.AreNotEqual(Option.Some<int?, string>(1), Option.Some<int, string>(1));
            Assert.AreNotEqual(Option.Some<string, string>(null), Option.Some<int?, string>(null));
            Assert.AreNotEqual(Option.Some<string, string>("1"), Option.Some<DateTime, string>(DateTime.Now));
            Assert.AreNotEqual(Option.Some<string, string>(null), Option.Some<object, string>(null));
        }

        [TestMethod]
        public void Either_Hashing()
        {
            // None all have identical hash codes
            Assert.AreEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<int, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<int?, string>("ex").GetHashCode(), Option.None<int?, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<string, string>("ex").GetHashCode(), Option.None<string, string>("ex").GetHashCode());

            Assert.AreEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<double, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<int?, string>("ex").GetHashCode(), Option.None<double?, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<string, string>("ex").GetHashCode(), Option.None<object, string>("ex").GetHashCode());

            Assert.AreEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<string, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<int?, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<int?, string>("ex").GetHashCode(), Option.None<string, string>("ex").GetHashCode());

            Assert.AreNotEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<int, string>("ex1").GetHashCode());
            Assert.AreNotEqual(Option.None<int?, string>("ex").GetHashCode(), Option.None<int?, string>("ex1").GetHashCode());
            Assert.AreNotEqual(Option.None<string, string>("ex").GetHashCode(), Option.None<string, string>("ex1").GetHashCode());

            Assert.AreNotEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<double, string>("ex1").GetHashCode());
            Assert.AreNotEqual(Option.None<int?, string>("ex").GetHashCode(), Option.None<double?, string>("ex1").GetHashCode());
            Assert.AreNotEqual(Option.None<string, string>("ex").GetHashCode(), Option.None<object, string>("ex1").GetHashCode());

            Assert.AreNotEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<string, string>("ex1").GetHashCode());
            Assert.AreNotEqual(Option.None<int, string>("ex").GetHashCode(), Option.None<int?, string>("ex1").GetHashCode());
            Assert.AreNotEqual(Option.None<int?, string>("ex").GetHashCode(), Option.None<string, string>("ex1").GetHashCode());

            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<int, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<int?, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<string, string>("ex").GetHashCode());

            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<double, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<double?, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<object, string>("ex").GetHashCode());

            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<string, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<int?, string>("ex").GetHashCode());
            Assert.AreEqual(Option.None<object, string>("ex").GetHashCode(), Option.None<string, string>("ex").GetHashCode());

            // Null values return identical hash codes
            Assert.AreEqual(Option.Some<int?, string>(null).GetHashCode(), Option.Some<int?, string>(null).GetHashCode());
            Assert.AreEqual(Option.Some<string, string>(null).GetHashCode(), Option.Some<string, string>(null).GetHashCode());
            Assert.AreEqual(Option.Some<int?, string>(null).GetHashCode(), Option.Some<double?, string>(null).GetHashCode());
            Assert.AreEqual(Option.Some<string, string>(null).GetHashCode(), Option.Some<object, string>(null).GetHashCode());
            Assert.AreEqual(Option.Some<int?, string>(null).GetHashCode(), Option.Some<string, string>(null).GetHashCode());

            Assert.AreEqual(Option.Some<int?, string>(null).GetHashCode(), Option.Some<int?, int>(null).GetHashCode());
            Assert.AreEqual(Option.Some<string, string>(null).GetHashCode(), Option.Some<string, int>(null).GetHashCode());
            Assert.AreEqual(Option.Some<int?, string>(null).GetHashCode(), Option.Some<double?, int>(null).GetHashCode());
            Assert.AreEqual(Option.Some<string, string>(null).GetHashCode(), Option.Some<object, int>(null).GetHashCode());
            Assert.AreEqual(Option.Some<int?, string>(null).GetHashCode(), Option.Some<string, int>(null).GetHashCode());

            // Some values 
            Assert.AreEqual(Option.Some<int, string>(1).GetHashCode(), Option.Some<int, string>(1).GetHashCode());
            Assert.AreEqual(Option.Some<int?, string>(1).GetHashCode(), Option.Some<int?, string>(1).GetHashCode());
            Assert.AreEqual(Option.Some<string, string>("1").GetHashCode(), Option.Some<string, string>("1").GetHashCode());

            Assert.AreNotEqual(Option.Some<int, string>(1).GetHashCode(), Option.Some<int, string>(2).GetHashCode());
            Assert.AreNotEqual(Option.Some<int?, string>(1).GetHashCode(), Option.Some<int?, string>(2).GetHashCode());
            Assert.AreNotEqual(Option.Some<string, string>("1").GetHashCode(), Option.Some<string, string>("2").GetHashCode());

            Assert.AreNotEqual(Option.Some<int, string>(1).GetHashCode(), Option.None<int, string>("ex").GetHashCode());
            Assert.AreNotEqual(Option.Some<int?, string>(1).GetHashCode(), Option.None<int?, string>("ex").GetHashCode());
            Assert.AreNotEqual(Option.Some<string, string>("1").GetHashCode(), Option.None<string, string>("ex").GetHashCode());

            Assert.AreEqual(Option.Some<int, string>(1).GetHashCode(), Option.Some<int, object>(1).GetHashCode());
            Assert.AreEqual(Option.Some<int?, string>(1).GetHashCode(), Option.Some<int?, object>(1).GetHashCode());
            Assert.AreEqual(Option.Some<string, string>("1").GetHashCode(), Option.Some<string, object>("1").GetHashCode());

            Assert.AreNotEqual(Option.Some<int, string>(1).GetHashCode(), Option.Some<int, object>(2).GetHashCode());
            Assert.AreNotEqual(Option.Some<int?, string>(1).GetHashCode(), Option.Some<int?, object>(2).GetHashCode());
            Assert.AreNotEqual(Option.Some<string, string>("1").GetHashCode(), Option.Some<string, object>("2").GetHashCode());

            // None and null are different
            Assert.AreNotEqual(Option.None<int?, string>("ex").GetHashCode(), Option.Some<int?, string>(null).GetHashCode());
            Assert.AreNotEqual(Option.None<string, string>("ex").GetHashCode(), Option.Some<string, string>(null).GetHashCode());
        }

        [TestMethod]
        public void Either_StringRepresentation()
        {
            Assert.AreEqual(Option.None<int?, int?>(null).ToString(), "None(null)");
            Assert.AreEqual(Option.None<string, string>(null).ToString(), "None(null)");

            Assert.AreEqual(Option.None<int, int>(1).ToString(), "None(1)");
            Assert.AreEqual(Option.None<int?, int?>(1).ToString(), "None(1)");
            Assert.AreEqual(Option.None<string, string>("ex").ToString(), "None(ex)");

            Assert.AreEqual(Option.Some<int?, string>(null).ToString(), "Some(null)");
            Assert.AreEqual(Option.Some<string, string>(null).ToString(), "Some(null)");

            Assert.AreEqual(Option.Some<int, string>(1).ToString(), "Some(1)");
            Assert.AreEqual(Option.Some<int?, string>(1).ToString(), "Some(1)");
            Assert.AreEqual(Option.Some<string, string>("1").ToString(), "Some(1)");

            var now = DateTime.Now;
            Assert.AreEqual(Option.Some<DateTime, DateTime>(now).ToString(), "Some(" + now.ToString() + ")");
            Assert.AreEqual(Option.None<DateTime, DateTime>(now).ToString(), "None(" + now.ToString() + ")");
        }

        [TestMethod]
        public void Either_GetValue()
        {
            var noneStruct = Option.None<int, string>("ex");
            var noneNullable = Option.None<int?, string>("ex");
            var noneClass = Option.None<string, string>("ex");

            Assert.AreEqual(noneStruct.ValueOr(-1), -1);
            Assert.AreEqual(noneNullable.ValueOr(-1), -1);
            Assert.AreEqual(noneClass.ValueOr("-1"), "-1");

            var someStruct = Option.Some<int, string>(1);
            var someNullable = Option.Some<int?, string>(1);
            var someNullableEmpty = Option.Some<int?, string>(null);
            var someClass = Option.Some<string, string>("1");
            var someClassNull = Option.Some<string, string>(null);

            Assert.AreEqual(someStruct.ValueOr(-1), 1);
            Assert.AreEqual(someNullable.ValueOr(-1), 1);
            Assert.AreEqual(someNullableEmpty.ValueOr(-1), null);
            Assert.AreEqual(someClass.ValueOr("-1"), "1");
            Assert.AreEqual(someClassNull.ValueOr("-1"), null);
        }

        [TestMethod]
        public void Either_AlternativeValue()
        {
            var noneStruct = Option.None<int, string>("ex");
            var noneNullable = Option.None<int?, string>("ex");
            var noneClass = Option.None<string, string>("ex");

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
        public void Either_CreateExtensions()
        {
            var none = 1.None("ex");
            var some = 1.Some<int, string>();

            Assert.AreEqual(none.ValueOr(-1), -1);
            Assert.AreEqual(some.ValueOr(-1), 1);

            var noneNotNull = ((string)null).SomeNotNull<string, string>("ex");
            var someNotNull = "1".SomeNotNull<string, string>("ex");

            Assert.AreEqual(noneNotNull.ValueOr("-1"), "-1");
            Assert.AreEqual(someNotNull.ValueOr("-1"), "1");

            var noneNullableNotNull = ((int?)null).SomeNotNull<int?, string>("ex");
            var someNullableNotNull = ((int?)1).SomeNotNull<int?, string>("ex");

            Assert.IsInstanceOfType(noneNullableNotNull.ValueOr(-1), typeof(int?));
            Assert.IsInstanceOfType(someNullableNotNull.ValueOr(-1), typeof(int?));
            Assert.AreEqual(noneNullableNotNull.ValueOr(-1), -1);
            Assert.AreEqual(someNullableNotNull.ValueOr(-1), 1);

            var noneFromNullable = ((int?)null).ToOption<int, string>("ex");
            var someFromNullable = ((int?)1).ToOption<int, string>("ex");

            Assert.IsInstanceOfType(noneFromNullable.ValueOr(-1), typeof(int));
            Assert.IsInstanceOfType(someFromNullable.ValueOr(-1), typeof(int));
            Assert.AreEqual(noneFromNullable.ValueOr(-1), -1);
            Assert.AreEqual(someFromNullable.ValueOr(-1), 1);
        }

        [TestMethod]
        public void Either_Matching()
        {
            var none = "val".None("ex");
            var some = "val".Some<string, string>();

            var failure = none.Match(
                some: val => val,
                none: ex => ex
            );

            var success = some.Match(
                some: val => val,
                none: ex => ex
            );

            Assert.AreEqual(failure, "ex");
            Assert.AreEqual(success, "val");

            none.Match(
                some: val => Assert.Fail(),
                none: ex => { }
            );

            some.Match(
                some: val => { },
                none: ex => Assert.Fail()
            );
        }

        [TestMethod]
        public void Either_Transformation()
        {
            var none = "val".None("ex");
            var some = "val".Some<string, string>();

            var noneNull = ((string)null).None("ex");
            var someNull = ((string)null).Some<string, string>();

            var noneUpper = none.Map(x => x.ToUpper());
            var someUpper = some.Map(x => x.ToUpper());

            var noneExUpper = none.MapException(x => x.ToUpper());
            var someExUpper = some.MapException(x => x.ToUpper());

            Assert.IsFalse(noneUpper.HasValue);
            Assert.IsTrue(someUpper.HasValue);
            Assert.AreEqual(noneUpper.ValueOr("ex"), "ex");
            Assert.AreEqual(someUpper.ValueOr("ex"), "VAL");
            Assert.AreEqual(noneExUpper.Match(val => val, ex => ex), "EX");
            Assert.AreEqual(someExUpper.Match(val => val, ex => ex), "val");

            var noneNotNull = none.FlatMap(x => x.SomeNotNull<string, string>("ex1"));
            var someNotNull = some.FlatMap(x => x.SomeNotNull<string, string>("ex1"));
            var noneNullNotNull = noneNull.FlatMap(x => x.SomeNotNull<string, string>("ex1"));
            var someNullNotNull = someNull.FlatMap(x => x.SomeNotNull<string, string>("ex1"));

            Assert.IsFalse(noneNotNull.HasValue);
            Assert.IsTrue(someNotNull.HasValue);
            Assert.IsFalse(noneNullNotNull.HasValue);
            Assert.IsFalse(someNullNotNull.HasValue);
            Assert.AreEqual(noneNotNull.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(someNotNull.Match(val => val, ex => ex), "val");
            Assert.AreEqual(noneNullNotNull.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(someNullNotNull.Match(val => val, ex => ex), "ex1");

            var noneNotVal = none.Filter(x => x != "val", "ex1");
            var someNotVal = some.Filter(x => x != "val", "ex1");
            var noneVal = none.Filter(x => x == "val", "ex1");
            var someVal = some.Filter(x => x == "val", "ex1");

            Assert.IsFalse(noneNotVal.HasValue);
            Assert.IsFalse(someNotVal.HasValue);
            Assert.IsFalse(noneVal.HasValue);
            Assert.IsTrue(someVal.HasValue);
            Assert.AreEqual(noneNotVal.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(someNotVal.Match(val => val, ex => ex), "ex1");
            Assert.AreEqual(noneVal.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(someVal.Match(val => val, ex => ex), "val");
        }

        [TestMethod]
        public void Either_ExceptionPropagation()
        {
            var none = "val".None("ex");
            var some = "val".Some<string, string>();

            Assert.AreEqual(none.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(some.Match(val => val, ex => ex), "val");

            var none1 = none.Filter(val => false, "ex1");
            var some1 = some.Filter(val => false, "ex1");

            Assert.AreEqual(none1.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(some1.Match(val => val, ex => ex), "ex1");

            var none2 = none1.Filter(val => false, "ex2");
            var some2 = some1.Filter(val => false, "ex2");

            Assert.AreEqual(none2.Match(val => val, ex => ex), "ex");
            Assert.AreEqual(some2.Match(val => val, ex => ex), "ex1");
        }
    }
}
