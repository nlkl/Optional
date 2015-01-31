using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Linq;

namespace Optional.Tests
{
    [TestClass]
    public class LinqTests
    {
        [TestMethod]
        public void Maybe_LinqTransformations()
        {
            var none = "a".None();
            var some = "a".Some();

            var noneNull = ((string)null).None();
            var someNull = ((string)null).Some();

            var noneUpper =
                from x in none
                select x.ToUpper();

            var someUpper =
                from x in some
                select x.ToUpper();

            Assert.IsFalse(noneUpper.HasValue);
            Assert.IsTrue(someUpper.HasValue);
            Assert.AreEqual(noneUpper.ValueOr("b"), "b");
            Assert.AreEqual(someUpper.ValueOr("b"), "A");

            var noneNotNull =
                from x in none
                from y in x.SomeNotNull()
                select y;

            var someNotNull =
                from x in some
                from y in x.SomeNotNull()
                select y;

            var noneNullNotNull =
                from x in noneNull
                from y in x.SomeNotNull()
                select y;

            var someNullNotNull =
                from x in someNull
                from y in x.SomeNotNull()
                select y;

            Assert.IsFalse(noneNotNull.HasValue);
            Assert.IsTrue(someNotNull.HasValue);
            Assert.IsFalse(noneNullNotNull.HasValue);
            Assert.IsFalse(someNullNotNull.HasValue);

            var noneNotA =
                from x in none
                where x != "a"
                select x;

            var someNotA =
                from x in some
                where x != "a"
                select x;

            var noneA =
                from x in none
                where x == "a"
                select x;

            var someA =
                from x in some
                where x == "a"
                select x;

            Assert.IsFalse(noneNotA.HasValue);
            Assert.IsFalse(someNotA.HasValue);
            Assert.IsFalse(noneA.HasValue);
            Assert.IsTrue(someA.HasValue);
        }

        [TestMethod]
        public void Either_LinqTransformations()
        {
            var none = "a".None<string, string>("ex");
            var some = "a".Some<string, string>();

            var noneNull = ((string)null).None<string, string>("ex");
            var someNull = ((string)null).Some<string, string>();

            var noneUpper =
                from x in none
                select x.ToUpper();

            var someUpper =
                from x in some
                select x.ToUpper();

            Assert.IsFalse(noneUpper.HasValue);
            Assert.IsTrue(someUpper.HasValue);
            Assert.AreEqual(noneUpper.ValueOr("b"), "b");
            Assert.AreEqual(someUpper.ValueOr("b"), "A");

            var noneNotNull =
                from x in none
                from y in x.SomeNotNull<string, string>("ex1")
                select y;

            var someNotNull =
                from x in some
                from y in x.SomeNotNull<string, string>("ex1")
                select y;

            var noneNullNotNull =
                from x in noneNull
                from y in x.SomeNotNull<string, string>("ex1")
                select y;

            var someNullNotNull =
                from x in someNull
                from y in x.SomeNotNull<string, string>("ex1")
                select y;

            Assert.IsFalse(noneNotNull.HasValue);
            Assert.IsTrue(someNotNull.HasValue);
            Assert.IsFalse(noneNullNotNull.HasValue);
            Assert.IsFalse(someNullNotNull.HasValue);
        }
    }
}
