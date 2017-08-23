using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Tests
{
    [TestClass]
    public class MaybeEitherInteropTests
    {
        [TestMethod]
        public void MaybeEither_Conversion()
        {
            var noneMaybe = Option.None<string>();
            var someMaybe = Option.Some<string>("val");

            var noneEither = Option.None<string, string>("ex");
            var someEither = Option.Some<string, string>("val");

            Assert.AreEqual(noneMaybe.WithException("ex"), noneEither);
            Assert.AreEqual(someMaybe.WithException("ex"), someEither);

            Assert.AreEqual(noneMaybe, noneEither.WithoutException());
            Assert.AreEqual(someMaybe, someEither.WithoutException());
        }

        [TestMethod]
        public void MaybeEither_ConversionLazy()
        {
            var noneMaybe = Option.None<string>();
            var someMaybe = Option.Some<string>("val");

            Assert.AreEqual(noneMaybe.WithException(() => "ex").ValueOrException(), "ex");
            Assert.AreEqual(someMaybe.WithException(() => { Assert.Fail(); return "ex"; }).ValueOrException(), "val");
        }

        [TestMethod]
        public void MaybeEither_Transformation()
        {
            var noneMaybe = Option.None<string>();
            var someMaybe = Option.Some<string>("val");

            var noneEither = Option.None<string, string>("ex");
            var someEither = Option.Some<string, string>("val");

            Assert.AreEqual(noneMaybe.FlatMap(val => Option.None<string, string>("ex")), noneMaybe);
            Assert.AreEqual(noneMaybe.FlatMap(val => Option.Some<string, string>("val")), noneMaybe);
            Assert.AreEqual(someMaybe.FlatMap(val => Option.None<string, string>("ex")), noneMaybe);
            Assert.AreEqual(someMaybe.FlatMap(val => Option.Some<string, string>("val")), someMaybe);
            Assert.AreEqual(someMaybe.FlatMap(val => Option.Some<string, string>("val1")).ValueOr("ex"), "val1");

            Assert.AreEqual(noneEither.FlatMap(val => Option.None<string>(), "ex1"), noneEither);
            Assert.AreEqual(noneEither.FlatMap(val => Option.Some<string>("val"), "ex1"), noneEither);
            Assert.AreEqual(someEither.FlatMap(val => Option.None<string>(), "ex"), noneEither);
            Assert.AreEqual(someEither.FlatMap(val => Option.Some<string>("val"), "ex"), someEither);
            Assert.AreEqual(someEither.FlatMap(val => Option.Some<string>("val1"), "ex").ValueOr("ex"), "val1");
        }

        [TestMethod]
        public void MaybeEither_TransformationLazy()
        {
            var noneMaybe = Option.None<string>();
            var someMaybe = Option.Some<string>("val");

            var noneEither = Option.None<string, string>("ex");
            var someEither = Option.Some<string, string>("val");

            Assert.AreEqual(noneEither.FlatMap(val => Option.None<string>(), () => "ex1"), noneEither);
            Assert.AreEqual(noneEither.FlatMap(val => Option.Some<string>("val"), () => "ex1"), noneEither);
            Assert.AreEqual(someEither.FlatMap(val => Option.None<string>(), () => "ex"), noneEither);
            Assert.AreEqual(someEither.FlatMap(val => Option.Some<string>("val"), () => { Assert.Fail(); return "ex"; }), someEither);
            Assert.AreEqual(someEither.FlatMap(val => Option.Some<string>("val1"), () => { Assert.Fail(); return "ex"; }).ValueOr("ex"), "val1");
        }
    }
}
