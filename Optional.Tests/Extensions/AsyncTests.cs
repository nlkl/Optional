using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optional.Extensions.Async;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class AsyncTests
    {
        [TestMethod]
        public async Task Extension_AsyncMap()
        {
            var someOptionTask = Task.FromResult(Option.Some<string>("abc"));
            var noneOptionTask = Task.FromResult(Option.None<string>());

            Assert.AreEqual((await someOptionTask.Map(val => val + "d")).ValueOr("0"), "abcd");
            Assert.AreEqual((await noneOptionTask.Map(val => val + "d")).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_AsyncFlatMap()
        {
            var someOptionTask = Task.FromResult(Option.Some<string>("abc"));
            var noneOptionTask = Task.FromResult(Option.None<string>());

            Assert.AreEqual((await someOptionTask.FlatMap(val => Task.FromResult(Option.Some(val + "d")))).ValueOr("0"), "abcd");
            Assert.AreEqual((await someOptionTask.FlatMap(val => Task.FromResult(Option.None<string>()))).ValueOr("0"), "0");
            Assert.AreEqual((await noneOptionTask.FlatMap(val => Task.FromResult(Option.Some(val + "d")))).ValueOr("0"), "0");
            Assert.AreEqual((await noneOptionTask.FlatMap(val => Task.FromResult(Option.None<string>()))).ValueOr("0"), "0");
        }
    }
}
