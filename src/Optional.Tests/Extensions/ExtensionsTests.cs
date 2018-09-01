using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void SomeWhen_Exception_Factory_Some()
        {
            // Arrange
            var value = OperationResult.Success();

            // Act
            var option = value.SomeWhen(
                x => x.Succeeded,
                x => x.Errors);

            // Assert
            Assert.IsTrue(option.HasValue);
            Assert.IsTrue(option.Exists(x => x.Equals(value)));
        }

        [TestMethod]
        public void SomeWhen_Exception_Factory_None()
        {
            // Arrange
            var value = OperationResult.Failure("Error 1", "Error 2");

            // Act
            var option = value.SomeWhen(
                x => x.Succeeded,
                x => x.Errors);
            
            // Assert
            Assert.IsFalse(option.HasValue);
            AssertContainsErrors(option, value.Errors);
        }

        [TestMethod]
        public void NoneWhen_Exception_Factory_Some()
        {
            // Arrange
            var value = OperationResult.Success();

            // Act
            var option = value.NoneWhen(
                x => x.Succeeded,
                x => x.Errors);

            // Assert
            Assert.IsFalse(option.HasValue);
            AssertContainsErrors(option, value.Errors);
        }

        [TestMethod]
        public void NoneWhen_Exception_Factory_None()
        {
            // Arrange
            var value = OperationResult.Failure("Error 1", "Error 2");

            // Act
            var option = value.NoneWhen(
                x => x.Succeeded,
                x => x.Errors);

            // Assert
            Assert.IsTrue(option.HasValue);
            AssertContainsErrors(option, value.Errors);
        }

        private void AssertContainsErrors(Option<OperationResult, List<string>> option, List<string> errors) =>
            option.MatchNone(errs => Assert.IsTrue(errs.All(err => errors.Contains(err))));

        private class OperationResult
        {
            public bool Succeeded { get; set; }

            public List<string> Errors { get; set; } = new List<string>();

            public static OperationResult Success() => new OperationResult { Succeeded = true };

            public static OperationResult Failure(params string[] errors) => new OperationResult { Succeeded = false, Errors = errors.ToList() };
        }
    }
}
