using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optional.Extensions.AsyncTmp;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class AsyncOptionTests
    {
        [TestMethod]
        public async Task Extension_Maybe_AsyncMap()
        {
            var someOptionTask = Task.FromResult(Option.Some<string>("abc")).Collapse();
            var noneOptionTask = Task.FromResult(Option.None<string>()).Collapse();

            Assert.AreEqual((await someOptionTask.Map(val => val + "d")).ValueOr("0"), "abcd");
            Assert.AreEqual((await noneOptionTask.Map(val => val + "d")).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_Maybe_AsyncFlatMap()
        {
            var someOptionTask = Task.FromResult(Option.Some<string>("abc")).Collapse();
            var noneOptionTask = Task.FromResult(Option.None<string>()).Collapse();

            Assert.AreEqual((await someOptionTask.FlatMap(val => Task.FromResult(Option.Some(val + "d")))).ValueOr("0"), "abcd");
            Assert.AreEqual((await someOptionTask.FlatMap(val => Task.FromResult(Option.None<string>()))).ValueOr("0"), "0");
            Assert.AreEqual((await noneOptionTask.FlatMap(val => Task.FromResult(Option.Some(val + "d")))).ValueOr("0"), "0");
            Assert.AreEqual((await noneOptionTask.FlatMap(val => Task.FromResult(Option.None<string>()))).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_Maybe_AsyncOptionMap()
        {
            var someOptionTask = Task.FromResult(Option.Some<string>("abc")).Collapse();
            var noneOptionTask = Task.FromResult(Option.None<string>()).Collapse();

            Assert.AreEqual((await someOptionTask.OptionMap(val => Option.Some(val + "d"))).ValueOr("0"), "abcd");
            Assert.AreEqual((await someOptionTask.OptionMap(val => Option.None<string>())).ValueOr("0"), "0");
            Assert.AreEqual((await noneOptionTask.OptionMap(val => Option.Some(val + "d"))).ValueOr("0"), "0");
            Assert.AreEqual((await noneOptionTask.OptionMap(val => Option.None<string>())).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_Maybe_AsyncTaskMap()
        {
            var someOptionTask = Task.FromResult(Option.Some<string>("abc")).Collapse();
            var noneOptionTask = Task.FromResult(Option.None<string>()).Collapse();

            Assert.AreEqual((await someOptionTask.TaskMap(val => Task.FromResult(val + "d"))).ValueOr("0"), "abcd");
            Assert.AreEqual((await noneOptionTask.TaskMap(val => Task.FromResult(val + "d"))).ValueOr("0"), "0");
        }
    }
}
