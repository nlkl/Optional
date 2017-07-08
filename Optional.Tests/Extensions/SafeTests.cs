using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Utilities;
using Optional.Tests.Utilities;

using Ex = System.Exception;
using Ex1 = System.ArgumentNullException;
using Ex2 = System.Text.EncoderFallbackException;
using Ex3 = System.FormatException;
using Ex4 = System.NullReferenceException;
using Ex5 = System.NotImplementedException;
using Ex6 = System.RankException;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class SafeTests
    {
        [TestMethod]
        public void Extensions_Safe_CatchAll()
        {
            var ex1 = new Ex("ex");
            var ex2 = new Ex1("ex");

            var error1 = Safe.Try<bool>(() => { throw ex1; });
            var error2 = Safe.Try<bool>(() => { throw ex2; });

            Assert.IsFalse(error1.HasValue);
            Assert.IsFalse(error2.HasValue);

            Assert.IsInstanceOfType(error1.Match(x => null, ex => ex), typeof(Ex));
            Assert.IsInstanceOfType(error2.Match(x => null, ex => ex), typeof(Ex1));

            Assert.AreEqual(error1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(error2.Match(x => null, ex => ex), ex2);

            var success = Safe.Try(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Safe_Catch1()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Safe.Try<bool, Ex1>(() => { throw ex1; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);

            CustomAssert.Throws<Ex>(() => Safe.Try<bool, Ex2>(() => { throw ex0; }));
            CustomAssert.Throws<Ex1>(() => Safe.Try<bool, Ex2>(() => { throw ex1; }));

            Safe.Try<bool, BaseEx>(() => { throw subEx; });
            Safe.Try<bool, Ex>(() => { throw ex0; });
            Safe.Try<bool, Ex>(() => { throw ex1; });

            var success = Safe.Try<bool, Ex1>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Safe_Catch2()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Safe.Try<bool, Ex1, Ex2>(() => { throw ex1; });
            var err2 = Safe.Try<bool, Ex1, Ex2>(() => { throw ex2; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsFalse(err2.HasValue);

            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.IsInstanceOfType(err2.Match(x => null, ex => ex), typeof(Ex2));

            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(err2.Match(x => null, ex => ex), ex2);

            CustomAssert.Throws<Ex>(() => Safe.Try<bool, Ex2, Ex3>(() => { throw ex0; }));
            CustomAssert.Throws<Ex1>(() => Safe.Try<bool, Ex2, Ex3>(() => { throw ex1; }));

            Safe.Try<bool, BaseEx, Ex2>(() => { throw subEx; });
            Safe.Try<bool, Ex, Ex2>(() => { throw ex0; });
            Safe.Try<bool, Ex, Ex2>(() => { throw ex1; });

            var success = Safe.Try<bool, Ex1, Ex2>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Safe_Catch3()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var ex4 = new Ex4("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Safe.Try<bool, Ex1, Ex2, Ex3>(() => { throw ex1; });
            var err2 = Safe.Try<bool, Ex1, Ex2, Ex3>(() => { throw ex2; });
            var err3 = Safe.Try<bool, Ex1, Ex2, Ex3>(() => { throw ex3; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsFalse(err2.HasValue);
            Assert.IsFalse(err3.HasValue);

            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.IsInstanceOfType(err2.Match(x => null, ex => ex), typeof(Ex2));
            Assert.IsInstanceOfType(err3.Match(x => null, ex => ex), typeof(Ex3));

            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(err2.Match(x => null, ex => ex), ex2);
            Assert.AreEqual(err3.Match(x => null, ex => ex), ex3);

            CustomAssert.Throws<Ex>(() => Safe.Try<bool, Ex2, Ex3, Ex4>(() => { throw ex0; }));
            CustomAssert.Throws<Ex1>(() => Safe.Try<bool, Ex2, Ex3, Ex4>(() => { throw ex1; }));

            Safe.Try<bool, BaseEx, Ex2, Ex3>(() => { throw subEx; });
            Safe.Try<bool, Ex, Ex2, Ex3>(() => { throw ex0; });
            Safe.Try<bool, Ex, Ex2, Ex3>(() => { throw ex1; });

            var success = Safe.Try<bool, Ex1, Ex2, Ex3>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Safe_Catch4()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var ex4 = new Ex4("ex");
            var ex5 = new Ex5("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex1; });
            var err2 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex2; });
            var err3 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex3; });
            var err4 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex4; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsFalse(err2.HasValue);
            Assert.IsFalse(err3.HasValue);
            Assert.IsFalse(err4.HasValue);

            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.IsInstanceOfType(err2.Match(x => null, ex => ex), typeof(Ex2));
            Assert.IsInstanceOfType(err3.Match(x => null, ex => ex), typeof(Ex3));
            Assert.IsInstanceOfType(err4.Match(x => null, ex => ex), typeof(Ex4));

            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(err2.Match(x => null, ex => ex), ex2);
            Assert.AreEqual(err3.Match(x => null, ex => ex), ex3);
            Assert.AreEqual(err4.Match(x => null, ex => ex), ex4);

            CustomAssert.Throws<Ex>(() => Safe.Try<bool, Ex2, Ex3, Ex4, Ex5>(() => { throw ex0; }));
            CustomAssert.Throws<Ex1>(() => Safe.Try<bool, Ex2, Ex3, Ex4, Ex5>(() => { throw ex1; }));

            Safe.Try<bool, BaseEx, Ex2, Ex3, Ex4>(() => { throw subEx; });
            Safe.Try<bool, Ex, Ex2, Ex3, Ex4>(() => { throw ex0; });
            Safe.Try<bool, Ex, Ex2, Ex3, Ex4>(() => { throw ex1; });

            var success = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Safe_Catch5()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var ex4 = new Ex4("ex");
            var ex5 = new Ex5("ex");
            var ex6 = new Ex6("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex1; });
            var err2 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex2; });
            var err3 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex3; });
            var err4 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex4; });
            var err5 = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex5; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsFalse(err2.HasValue);
            Assert.IsFalse(err3.HasValue);
            Assert.IsFalse(err4.HasValue);
            Assert.IsFalse(err5.HasValue);

            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.IsInstanceOfType(err2.Match(x => null, ex => ex), typeof(Ex2));
            Assert.IsInstanceOfType(err3.Match(x => null, ex => ex), typeof(Ex3));
            Assert.IsInstanceOfType(err4.Match(x => null, ex => ex), typeof(Ex4));
            Assert.IsInstanceOfType(err5.Match(x => null, ex => ex), typeof(Ex5));

            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(err2.Match(x => null, ex => ex), ex2);
            Assert.AreEqual(err3.Match(x => null, ex => ex), ex3);
            Assert.AreEqual(err4.Match(x => null, ex => ex), ex4);
            Assert.AreEqual(err5.Match(x => null, ex => ex), ex5);

            CustomAssert.Throws<Ex>(() => Safe.Try<bool, Ex2, Ex3, Ex4, Ex5, Ex6>(() => { throw ex0; }));
            CustomAssert.Throws<Ex1>(() => Safe.Try<bool, Ex2, Ex3, Ex4, Ex5, Ex6>(() => { throw ex1; }));

            Safe.Try<bool, BaseEx, Ex2, Ex3, Ex4, Ex5>(() => { throw subEx; });
            Safe.Try<bool, Ex, Ex2, Ex3, Ex4, Ex5>(() => { throw ex0; });
            Safe.Try<bool, Ex, Ex2, Ex3, Ex4, Ex5>(() => { throw ex1; });

            var success = Safe.Try<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        private class BaseEx : System.Exception
        {
            public BaseEx()
            {
            }

            public BaseEx(string message) : base(message)
            {
            }
        }

        private class SubEx : BaseEx
        {
            public SubEx() : base()
            {
            }

            public SubEx(string message) : base(message)
            {
            }
        }
    }
}
