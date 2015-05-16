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
    public class AsyncOptionTests
    {
        [TestMethod]
        public async Task Extension_AsyncMaybe_Creation()
        {
            var some1 = AsyncOption.FromTask(Task.FromResult(Option.Some<string>("abc")));
            var none1 = AsyncOption.FromTask(Task.FromResult(Option.None<string>()));

            var some2 = Task.FromResult(Option.Some<string>("abc")).ToAsyncOption();
            var none2 = Task.FromResult(Option.None<string>()).ToAsyncOption();

            var some3a = AsyncOption.Some("abc");
            var some3b = AsyncOption.Some(Task.FromResult("abc"));
            var none3 = AsyncOption.None<string>();

            Assert.IsTrue((await some1).HasValue);
            Assert.IsFalse((await none1).HasValue);
            Assert.IsTrue((await some2).HasValue);
            Assert.IsFalse((await none2).HasValue);
            Assert.IsTrue((await some3a).HasValue);
            Assert.IsTrue((await some3b).HasValue);
            Assert.IsFalse((await none3).HasValue);
        }

        [TestMethod]
        public async Task Extension_AsyncMaybe_Map()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual((await some.Map(val => val + "d")).ValueOr("0"), "abcd");
            Assert.AreEqual((await none.Map(val => val + "d")).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_AsyncMaybe_FlatMap()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual((await some.FlatMap(val => Task.FromResult(Option.Some(val + "d")))).ValueOr("0"), "abcd");
            Assert.AreEqual((await some.FlatMap(val => Task.FromResult(Option.None<string>()))).ValueOr("0"), "0");
            Assert.AreEqual((await none.FlatMap(val => Task.FromResult(Option.Some(val + "d")))).ValueOr("0"), "0");
            Assert.AreEqual((await none.FlatMap(val => Task.FromResult(Option.None<string>()))).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_AsyncMaybe_FlatMapOption()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual((await some.FlatMap(val => Option.Some(val + "d"))).ValueOr("0"), "abcd");
            Assert.AreEqual((await some.FlatMap(val => Option.None<string>())).ValueOr("0"), "0");
            Assert.AreEqual((await none.FlatMap(val => Option.Some(val + "d"))).ValueOr("0"), "0");
            Assert.AreEqual((await none.FlatMap(val => Option.None<string>())).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extension_AsyncMaybe_FlatMapTask()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual((await some.FlatMap(val => Task.FromResult(val + "d"))).ValueOr("0"), "abcd");
            Assert.AreEqual((await none.FlatMap(val => Task.FromResult(val + "d"))).ValueOr("0"), "0");
        }
    }
}
