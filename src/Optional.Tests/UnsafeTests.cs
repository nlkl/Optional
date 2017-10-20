using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Unsafe;

namespace Optional.Tests
{
    [TestClass]
    public class UnsafeTests
    {
        [TestMethod]
        public void Maybe_ToNullable()
        {
            Assert.AreEqual(default(int?), Option.None<int>().ToNullable());
            Assert.AreEqual(1, Option.Some(1).ToNullable());
        }

        [TestMethod]
        public void Maybe_GetValueOrDefault()
        {
            Assert.AreEqual(default(int), Option.None<int>().ValueOrDefault());
            Assert.AreEqual(1, Option.Some(1).ValueOrDefault());

            Assert.AreEqual(default(int?), Option.None<int?>().ValueOrDefault());
            Assert.AreEqual(1, Option.Some<int?>(1).ValueOrDefault());

            Assert.AreEqual(default(string), Option.None<string>().ValueOrDefault());
            Assert.AreEqual("a", Option.Some("a").ValueOrDefault());
        }

        [TestMethod]
        public void Maybe_GetUnsafeValue()
        {
            var none = "a".None();
            var some = "a".Some();

            Assert.AreEqual("a", some.ValueOrFailure());
            Assert.AreEqual("a", some.ValueOrFailure("Error message"));

            try
            {
                var result = none.ValueOrFailure();
                Assert.Fail();
            }
            catch (OptionValueMissingException)
            {
            }

            try
            {
                var result = none.ValueOrFailure("Error message");
                Assert.Fail();
            }
            catch (OptionValueMissingException ex)
            {
                Assert.AreEqual(ex.Message, "Error message");
            }

            try
            {
                var result = none.ValueOrFailure(() => "Error message");
                Assert.Fail();
            }
            catch (OptionValueMissingException ex)
            {
                Assert.AreEqual(ex.Message, "Error message");
            }
        }

        [TestMethod]
        public void Either_ToNullable()
        {
            Assert.AreEqual(default(int?), Option.None<int, bool>(false).ToNullable());
            Assert.AreEqual(1, Option.Some<int, bool>(1).ToNullable());
        }

        [TestMethod]
        public void Either_GetValueOrDefault()
        {
            Assert.AreEqual(default(int), Option.None<int, bool>(false).ValueOrDefault());
            Assert.AreEqual(1, Option.Some<int, bool>(1).ValueOrDefault());

            Assert.AreEqual(default(int?), Option.None<int?, bool>(false).ValueOrDefault());
            Assert.AreEqual(1, Option.Some<int?, bool>(1).ValueOrDefault());

            Assert.AreEqual(default(string), Option.None<string, bool>(false).ValueOrDefault());
            Assert.AreEqual("a", Option.Some<string, bool>("a").ValueOrDefault());
        }

        [TestMethod]
        public void Either_GetUnsafeValue()
        {
            var none = "a".None<string, string>("ex");
            var some = "a".Some<string, string>();

            Assert.AreEqual("a", some.ValueOrFailure());
            Assert.AreEqual("a", some.ValueOrFailure("Error message"));

            try
            {
                var result = none.ValueOrFailure();
                Assert.Fail();
            }
            catch (OptionValueMissingException)
            {
            }

            try
            {
                var result = none.ValueOrFailure("Error message");
                Assert.Fail();
            }
            catch (OptionValueMissingException ex)
            {
                Assert.AreEqual(ex.Message, "Error message");
            }

            try
            {
                var result = none.ValueOrFailure(ex => "Error message" + ex);
                Assert.Fail();
            }
            catch (OptionValueMissingException ex)
            {
                Assert.AreEqual(ex.Message, "Error messageex");
            }
        }
    }
}
