using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Optional.Async.Tests
{
    [TestClass]
    public class AsyncEitherTests
    {
        [TestMethod]
        public async Task AsyncEither_SomeNotNullAsync()
        {
            // Arrange
            var task = Task.FromResult<object>(null);
            var exception = "A truly exceptional string :)";

            // Act
            var result = await task.SomeNotNullAsync(exception);

            // Assert
            var expectedResult = (await task).SomeNotNull(exception);

            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task AsyncEither_SomeWhenAsync()
        {
            // Arrange
            var value = ValueGenerator.RandomString();
            var task = Task.FromResult(value);

            Func<string, bool> predicate = s => s == "This should definitely return false";
            var error = "Error";

            // Act
            var result = await task.SomeWhenAsync(predicate, error);

            // Assert
            var expectedResult = (await task).SomeWhen(predicate, error);

            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task AsyncEither_SomeWhenAsync_With_ExceptionFactory()
        {
            // Arrange
            var value = ValueGenerator.RandomString();
            var task = Task.FromResult(value);

            Func<string, bool> predicate = s => s == "This should definitely return false";
            Func<string, int> exceptionFactory = _ => 10;

            // Act
            var result = await task.SomeWhenAsync(predicate, exceptionFactory);

            // Assert
            var expectedResult = (await task).SomeWhen(predicate, exceptionFactory);

            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task AsyncEither_ToAsync()
        {
            // Arrange
            var option = 10.Some<int, string>();

            // Act
            var result = option.ToAsync();

            // Assert
            (await result).Should().Be(option);
        }

        [TestMethod]
        public void AsyncEither_MapAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_MapExceptionAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_FlatMapAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_FilterAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_NotNullAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_OrAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_ElseAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_FlattenAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_WithoutExceptionAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncEither_Linq()
        {
            Assert.Inconclusive();
        }
    }
}
