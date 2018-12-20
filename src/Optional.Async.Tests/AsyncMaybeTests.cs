using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Unsafe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Optional.Async.Tests
{
    [TestClass]
    public class AsyncMaybeTests
    {
        [TestMethod]
        public async Task AsyncMaybe_ToAsync()
        {
            // Arrange
            var option = 10.Some();

            // Act
            var result = option.ToAsync();

            // Assert
            (await result).Should().Be(option);
        }

        [TestMethod]
        public async Task AsyncMaybe_FlatMapAsync_To_Exceptional()
        {
            // Arrange
            var option = Option.None<int>();
            var exceptionalOption = Option.None<int, string>(ValueGenerator.RandomString());
            var error = "Error";

            // Act
            var result = await option.FlatMapAsync(_ => Task.FromResult(exceptionalOption), error);

            // Assert
            var expected = Option.None<int, string>(error);

            result.Should().Be(expected);
        }

        [TestMethod]
        public async Task AsyncMaybe_FilterAsync_Should_Return_Exception()
        {
            // Arrange
            var optionTask = ValueGenerator.DelayedSome(10);

            Func<int, Task<bool>> predicate = _ => Task.FromResult(false);
            var exception = "Error";

            // Act
            var result = await optionTask.FilterAsync(predicate, exception);

            // Assert
            var expected = Option.None<int, string>(exception);

            result.Should().Be(expected);
        }

        [TestMethod]
        public async Task AsyncMaybe_FilterAsync_Should_Return_Value()
        {
            // Arrange
            var value = 10;
            var optionTask = ValueGenerator.DelayedSome(value);

            Func<int, Task<bool>> predicate = _ => Task.FromResult(true);
            var exception = "Error";

            // Act
            var result = await optionTask.FilterAsync(predicate, exception);

            // Assert
            var expected = Option.Some<int, string>(value);

            result.Should().Be(expected);
        }

        [TestMethod]
        public async Task AsyncMaybe_SomeNotNull()
        {
            // Arrange
            object nullObject = null;
            var nullTask = Task.FromResult(nullObject);

            // Act
            var result = await nullTask.SomeNotNullAsync();

            // Assert
            var expected = nullObject.SomeNotNull();

            result.Should().Be(expected);
        }

        [TestMethod]
        public async Task AsyncMaybe_CanMapAsync()
        {
            var value = 999;
            string Mapping(int v) => v.ToString();
            Task<string> AsyncMapping(int v) => Task.FromResult(Mapping(v));

            var option = value.Some();
            var optionTask = Task.FromResult(option);

            var resultOption1 = await option.MapAsync(AsyncMapping).ConfigureAwait(false);
            var resultOption2 = await optionTask.MapAsync(Mapping).ConfigureAwait(false);
            var resultOption3 = await optionTask.MapAsync(AsyncMapping).ConfigureAwait(false);

            resultOption1.HasValue.Should().BeTrue();
            resultOption1.ValueOrFailure().Should().Be(Mapping(value));
            resultOption2.HasValue.Should().BeTrue();
            resultOption2.ValueOrFailure().Should().Be(Mapping(value));
            resultOption3.HasValue.Should().BeTrue();
            resultOption3.ValueOrFailure().Should().Be(Mapping(value));
        }

        [TestMethod]
        public async Task AsyncMaybe_MapAsyncCanCaptureContext()
        {
            var ctx = new TestSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);

            var resultOption = await ValueGenerator.DelayedSome(0)
                .MapAsync(v => v, executeOnCapturedContext: true)
                .MapAsync(v => v, executeOnCapturedContext: true)
                .MapAsync(v => v, executeOnCapturedContext: true)
                .ConfigureAwait(false);

            ctx.Called.Should().Be(3);
        }

        [TestMethod]
        public async Task AsyncMaybe_MapAsyncCanIgnoreContext()
        {
            var ctx = new TestSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);

            var resultOption = await ValueGenerator.DelayedSome(0)
                .MapAsync(v => v, executeOnCapturedContext: false)
                .MapAsync(v => v, executeOnCapturedContext: false)
                .MapAsync(v => v, executeOnCapturedContext: false)
                .ConfigureAwait(false);

            ctx.Called.Should().Be(0);
        }

        [TestMethod]
        public async Task AsyncMaybe_MapAsyncCanCaptureAndIgnoreContext()
        {
            var ctx = new TestSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);

            var resultOption = await ValueGenerator.DelayedSome(0)
                .MapAsync(v => v, executeOnCapturedContext: false)
                .MapAsync(v => v, executeOnCapturedContext: true)
                .MapAsync(v => v, executeOnCapturedContext: false)
                .ConfigureAwait(false);

            ctx.Called.Should().Be(1);
        }

        [TestMethod]
        public void AsyncMaybe_FlatMapAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_FilterAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_NotNullAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_OrAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_ElseAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_FlattenAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_WithExceptionAsync()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void AsyncMaybe_Linq()
        {
            Assert.Inconclusive();
        }
    }
}
