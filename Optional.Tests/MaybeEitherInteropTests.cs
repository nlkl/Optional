using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
