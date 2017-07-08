#if !NOASYNC

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optional.Async;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class AsyncOptionTests
    {
        [TestMethod]
        public async Task Extensions_AsyncMaybe_Creation()
        {
            var some1 = new AsyncOption<string>(Task.FromResult(Option.Some<string>("abc")));
            var none1 = new AsyncOption<string>(Task.FromResult(Option.None<string>()));


            var some2a = Option.Some<string>("abc").ToAsyncOption();
            var none2a = Option.None<string>().ToAsyncOption();
            var some2b = Task.FromResult(Option.Some<string>("abc")).ToAsyncOption();
            var none2b = Task.FromResult(Option.None<string>()).ToAsyncOption();

            var some3a = AsyncOption.Some("abc");
            var some3b = AsyncOption.Some(Task.FromResult("abc"));
            var none3 = AsyncOption.None<string>();

            // Awaiting the whole option
            Assert.IsTrue((await some1).HasValue);
            Assert.IsFalse((await none1).HasValue);
            Assert.IsTrue((await some2a).HasValue);
            Assert.IsFalse((await none2a).HasValue);
            Assert.IsTrue((await some2b).HasValue);
            Assert.IsFalse((await none2b).HasValue);
            Assert.IsTrue((await some3a).HasValue);
            Assert.IsTrue((await some3b).HasValue);
            Assert.IsFalse((await none3).HasValue);

            // awaiting only HasValue
            Assert.IsTrue(await some1.HasValue);
            Assert.IsFalse(await none1.HasValue);
            Assert.IsTrue(await some2a.HasValue);
            Assert.IsFalse(await none2a.HasValue);
            Assert.IsTrue(await some2b.HasValue);
            Assert.IsFalse(await none2b.HasValue);
            Assert.IsTrue(await some3a.HasValue);
            Assert.IsTrue(await some3b.HasValue);
            Assert.IsFalse(await none3.HasValue);
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybe_RetrievalAndContainment()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.IsTrue(await some.Contains("abc"));
            Assert.IsFalse(await some.Contains("0"));
            Assert.IsTrue(await some.Exists(value => value == "abc"));
            Assert.IsFalse(await some.Exists(value => value == "0"));

            Assert.IsFalse(await none.Contains("abc"));
            Assert.IsFalse(await none.Exists(value => true));

            Assert.IsTrue(await some.Or("0").Contains("abc"));
            Assert.IsTrue(await none.Or("0").Contains("0"));
            Assert.IsTrue(await some.Or(() => "0").Contains("abc"));
            Assert.IsTrue(await none.Or(() => "0").Contains("0"));

            Assert.AreEqual(await some.ValueOr("0"), "abc");
            Assert.AreEqual(await none.ValueOr("0"), "0");
            Assert.AreEqual(await some.ValueOr(() => "0"), "abc");
            Assert.AreEqual(await none.ValueOr(() => "0"), "0");
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybe_Matching()
        {
            var some = AsyncOption.Some<string>("abc");
            var none = AsyncOption.None<string>();

            var success = await some.Match(
                some: val => val,
                none: () => "ex"
            );

            var failure = await none.Match(
                some: val => val,
                none: () => "ex"
            );

            Assert.AreEqual(success, "abc");
            Assert.AreEqual(failure, "ex");

            await none.Match(
                some: val => Assert.Fail(),
                none: () => { }
            );

            await some.Match(
                some: val => Assert.AreEqual(val, "abc"),
                none: () => Assert.Fail()
            );
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybe_Map()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual(await some.Map(val => val + "d").ValueOr("0"), "abcd");
            Assert.AreEqual(await none.Map(val => val + "d").ValueOr("0"), "0");

            Assert.AreEqual(await some.Map(val => Task.FromResult(val + "d")).ValueOr("0"), "abcd");
            Assert.AreEqual(await none.Map(val => Task.FromResult(val + "d")).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybe_FlatMap()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some(val + "d"))).ValueOr("0"), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string>())).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some(val + "d"))).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string>())).ValueOr("0"), "0");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some(val + "d")).ToAsyncOption()).ValueOr("0"), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string>()).ToAsyncOption()).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some(val + "d")).ToAsyncOption()).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string>()).ToAsyncOption()).ValueOr("0"), "0");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d"))).ValueOr("0"), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string, string>("ex"))).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d"))).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string, string>("ex"))).ValueOr("0"), "0");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d")).ToAsyncOption()).ValueOr("0"), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string, string>("ex")).ToAsyncOption()).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d")).ToAsyncOption()).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string, string>("ex")).ToAsyncOption()).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybe_FlatMapOption()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.AreEqual(await some.FlatMap(val => Option.Some(val + "d")).ValueOr("0"), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Option.None<string>()).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Option.Some(val + "d")).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Option.None<string>()).ValueOr("0"), "0");

            Assert.AreEqual(await some.FlatMap(val => Option.Some<string, string>(val + "d")).ValueOr("0"), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Option.None<string, string>("ex")).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Option.Some<string, string>(val + "d")).ValueOr("0"), "0");
            Assert.AreEqual(await none.FlatMap(val => Option.None<string, string>("ex")).ValueOr("0"), "0");
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybe_Filter()
        {
            var some = AsyncOption.Some("abc");
            var none = AsyncOption.None<string>();

            Assert.IsTrue(await some.Filter(value => value.StartsWith("a")).HasValue);
            Assert.IsFalse(await some.Filter(value => value.StartsWith("0")).HasValue);
            Assert.IsFalse(await none.Filter(value => value.StartsWith("a")).HasValue);
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_Creation()
        {
            var some1 = new AsyncOption<string, string>(Task.FromResult(Option.Some<string, string>("abc")));
            var none1 = new AsyncOption<string, string>(Task.FromResult(Option.None<string, string>("ex")));

            var some2a = Option.Some<string, string>("abc").ToAsyncOption();
            var none2a = Option.None<string, string>("ex").ToAsyncOption();
            var some2b = Task.FromResult(Option.Some<string, string>("abc")).ToAsyncOption();
            var none2b = Task.FromResult(Option.None<string, string>("ex")).ToAsyncOption();

            var some3a = AsyncOption.Some<string, string>("abc");
            var some3b = AsyncOption.Some<string, string>(Task.FromResult("abc"));
            var none3 = AsyncOption.None<string, string>("ex");

            // Awaiting the whole option
            Assert.IsTrue((await some1).HasValue);
            Assert.IsFalse((await none1).HasValue);
            Assert.IsTrue((await some2a).HasValue);
            Assert.IsFalse((await none2a).HasValue);
            Assert.IsTrue((await some2b).HasValue);
            Assert.IsFalse((await none2b).HasValue);
            Assert.IsTrue((await some3a).HasValue);
            Assert.IsTrue((await some3b).HasValue);
            Assert.IsFalse((await none3).HasValue);

            // awaiting only HasValue
            Assert.IsTrue(await some1.HasValue);
            Assert.IsFalse(await none1.HasValue);
            Assert.IsTrue(await some2a.HasValue);
            Assert.IsFalse(await none2a.HasValue);
            Assert.IsTrue(await some2b.HasValue);
            Assert.IsFalse(await none2b.HasValue);
            Assert.IsTrue(await some3a.HasValue);
            Assert.IsTrue(await some3b.HasValue);
            Assert.IsFalse(await none3.HasValue);
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_RetrievalAndContainment()
        {
            var some = AsyncOption.Some<string, string>("abc");
            var none = AsyncOption.None<string, string>("ex");

            Assert.IsTrue(await some.Contains("abc"));
            Assert.IsFalse(await some.Contains("0"));
            Assert.IsTrue(await some.Exists(value => value == "abc"));
            Assert.IsFalse(await some.Exists(value => value == "0"));

            Assert.IsFalse(await none.Contains("abc"));
            Assert.IsFalse(await none.Exists(value => true));

            Assert.IsTrue(await some.Or("0").Contains("abc"));
            Assert.IsTrue(await none.Or("0").Contains("0"));
            Assert.IsTrue(await some.Or(() => "0").Contains("abc"));
            Assert.IsTrue(await none.Or(() => "0").Contains("0"));

            Assert.AreEqual(await some.ValueOr("0"), "abc");
            Assert.AreEqual(await none.ValueOr("0"), "0");
            Assert.AreEqual(await some.ValueOr(() => "0"), "abc");
            Assert.AreEqual(await none.ValueOr(() => "0"), "0");

            Assert.AreEqual(await some.ValueOrException(), "abc");
            Assert.AreEqual(await none.ValueOrException(), "ex");
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_Matching()
        {
            var some = AsyncOption.Some<string, string>("abc");
            var none = AsyncOption.None<string, string>("ex");

            var success = await some.Match(
                some: val => val,
                none: ex => ex
            );

            var failure = await none.Match(
                some: val => val,
                none: ex => ex
            );

            Assert.AreEqual(success, "abc");
            Assert.AreEqual(failure, "ex");

            await none.Match(
                some: val => Assert.Fail(),
                none: ex => Assert.AreEqual(ex, "ex")
            );

            await some.Match(
                some: val => Assert.AreEqual(val, "abc"),
                none: ex => Assert.Fail()
            );
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_Map()
        {
            var some = AsyncOption.Some<string, string>("abc");
            var none = AsyncOption.None<string, string>("ex");

            Assert.AreEqual(await some.Map(val => val + "d").ValueOrException(), "abcd");
            Assert.AreEqual(await none.Map(val => val + "d").ValueOrException(), "ex");

            Assert.AreEqual(await some.MapException(ex => ex + "d").ValueOrException(), "abc");
            Assert.AreEqual(await none.MapException(ex => ex + "d").ValueOrException(), "exd");

            Assert.AreEqual(await some.Map(val => Task.FromResult(val + "d")).ValueOrException(), "abcd");
            Assert.AreEqual(await none.Map(val => Task.FromResult(val + "d")).ValueOrException(), "ex");

            Assert.AreEqual(await some.MapException(ex => Task.FromResult(ex + "d")).ValueOrException(), "abc");
            Assert.AreEqual(await none.MapException(ex => Task.FromResult(ex + "d")).ValueOrException(), "exd");
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_FlatMap()
        {
            var some = AsyncOption.Some<string, string>("abc");
            var none = AsyncOption.None<string, string>("ex");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d"))).ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string, string>("ex"))).ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d"))).ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string, string>("ex"))).ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")), "ex").ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string>()), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string>()), "ex").ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")), () => "ex").ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string>()), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string>()), () => "ex").ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d")).ToAsyncOption()).ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string, string>("ex")).ToAsyncOption()).ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string, string>(val + "d")).ToAsyncOption()).ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string, string>("ex")).ToAsyncOption()).ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")).ToAsyncOption(), "ex").ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string>()).ToAsyncOption(), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")).ToAsyncOption(), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string>()).ToAsyncOption(), "ex").ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")).ToAsyncOption(), () => "ex").ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Task.FromResult(Option.None<string>()).ToAsyncOption(), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.Some<string>(val + "d")).ToAsyncOption(), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Task.FromResult(Option.None<string>()).ToAsyncOption(), () => "ex").ValueOrException(), "ex");
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_FlatMapOption()
        {
            var some = AsyncOption.Some<string, string>("abc");
            var none = AsyncOption.None<string, string>("ex");

            Assert.AreEqual(await some.FlatMap(val => Option.Some<string, string>(val + "d")).ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Option.None<string, string>("ex")).ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Option.Some<string, string>(val + "d")).ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Option.None<string, string>("ex")).ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Option.Some<string>(val + "d"), "ex").ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Option.None<string>(), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Option.Some<string>(val + "d"), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Option.None<string>(), "ex").ValueOrException(), "ex");

            Assert.AreEqual(await some.FlatMap(val => Option.Some<string>(val + "d"), () => "ex").ValueOrException(), "abcd");
            Assert.AreEqual(await some.FlatMap(val => Option.None<string>(), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Option.Some<string>(val + "d"), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.FlatMap(val => Option.None<string>(), () => "ex").ValueOrException(), "ex");
        }

        [TestMethod]
        public async Task Extensions_AsyncEither_Filter()
        {
            var some = AsyncOption.Some<string, string>("abc");
            var none = AsyncOption.None<string, string>("ex");

            Assert.AreEqual(await some.Filter(value => value.StartsWith("a"), "ex").ValueOrException(), "abc");
            Assert.AreEqual(await some.Filter(value => value.StartsWith("0"), "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.Filter(value => value.StartsWith("a"), "ex2").ValueOrException(), "ex");

            Assert.AreEqual(await some.Filter(value => value.StartsWith("a"), () => "ex").ValueOrException(), "abc");
            Assert.AreEqual(await some.Filter(value => value.StartsWith("0"), () => "ex").ValueOrException(), "ex");
            Assert.AreEqual(await none.Filter(value => value.StartsWith("a"), () => "ex2").ValueOrException(), "ex");
        }

        [TestMethod]
        public async Task Extensions_AsyncMaybeEither_Conversions()
        {
            var someMaybe = AsyncOption.Some<string>("abc");
            var noneMaybe = AsyncOption.None<string>();

            var someEither = AsyncOption.Some<string, string>("abc");
            var noneEither = AsyncOption.None<string, string>("ex");

            Assert.AreEqual(await someMaybe.WithException("ex").ValueOrException(), "abc");
            Assert.AreEqual(await noneMaybe.WithException("ex").ValueOrException(), "ex");

            Assert.AreEqual(await someMaybe.WithException(() => "ex").ValueOrException(), "abc");
            Assert.AreEqual(await noneMaybe.WithException(() => "ex").ValueOrException(), "ex");

            Assert.AreEqual(await someEither.WithoutException().ValueOr("ex2"), "abc");
            Assert.AreEqual(await noneEither.WithoutException().ValueOr("ex2"), "ex2");

            Assert.AreEqual(await someEither.WithoutException().ValueOr(() => "ex2"), "abc");
            Assert.AreEqual(await noneEither.WithoutException().ValueOr(() => "ex2"), "ex2");
        }
    }
}

#endif
