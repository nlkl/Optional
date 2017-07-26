namespace Optional.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Unsafe;

    [TestClass]
    public class UnsafeTests
    {
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
