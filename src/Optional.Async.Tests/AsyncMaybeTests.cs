using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Unsafe;
using System.Threading;
using System.Threading.Tasks;

namespace Optional.Async.Tests
{
    [TestClass]
    public class AsyncMaybeTests
    {
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
