using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optional.Unsafe;
using Optional.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ex = System.Exception;
using Ex1 = System.ArgumentNullException;
using Ex2 = System.Text.EncoderFallbackException;
using Ex3 = System.FormatException;
using Ex4 = System.NullReferenceException;
using Ex5 = System.NotImplementedException;
using Ex6 = System.RankException;
using BaseEx = System.MemberAccessException;
using SubEx = System.MethodAccessException;

namespace Optional.Tests.Extensions
{
    [TestClass]
    public class TryTests
    {
        [TestMethod]
        public void Extensions_Try_CatchAll()
        {
            var ex1 = new Ex("ex");
            var ex2 = new Ex1("ex");

            var error1 = Try.Run<bool>(() => { throw ex1; });
            var error2 = Try.Run<bool>(() => { throw ex2; });

            Assert.IsFalse(error1.HasValue);
            Assert.IsFalse(error2.HasValue);

            Assert.IsInstanceOfType(error1.Match(x => null, ex => ex), typeof(Ex));
            Assert.IsInstanceOfType(error2.Match(x => null, ex => ex), typeof(Ex1));

            Assert.AreEqual(error1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(error2.Match(x => null, ex => ex), ex2);

            var success = Try.Run(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Try_Catch1()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Try.Run<bool, Ex1>(() => { throw ex1; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);

            try
            {
                Try.Run<bool, Ex1>(() => { throw ex0; });
                Assert.Fail();
            }
            catch { }

            try
            {
                Try.Run<bool, Ex1>(() => { throw ex2; });
                Assert.Fail();
            }
            catch { }

            Try.Run<bool, BaseEx>(() => { throw subEx; });
            Try.Run<bool, Ex>(() => { throw ex0; });
            Try.Run<bool, Ex>(() => { throw ex1; });

            var success = Try.Run<bool, Ex1>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Try_Catch2()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Try.Run<bool, Ex1, Ex2>(() => { throw ex1; });
            var err2 = Try.Run<bool, Ex1, Ex2>(() => { throw ex2; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsFalse(err2.HasValue);

            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.IsInstanceOfType(err2.Match(x => null, ex => ex), typeof(Ex2));

            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(err2.Match(x => null, ex => ex), ex2);

            try
            {
                Try.Run<bool, Ex1, Ex2>(() => { throw ex0; });
                Assert.Fail();
            }
            catch { }

            try
            {
                Try.Run<bool, Ex1, Ex2>(() => { throw ex3; });
                Assert.Fail();
            }
            catch { }

            Try.Run<bool, BaseEx, Ex2>(() => { throw subEx; });
            Try.Run<bool, Ex, Ex2>(() => { throw ex0; });
            Try.Run<bool, Ex, Ex2>(() => { throw ex1; });

            var success = Try.Run<bool, Ex1, Ex2>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Try_Catch3()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var ex4 = new Ex4("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Try.Run<bool, Ex1, Ex2, Ex3>(() => { throw ex1; });
            var err2 = Try.Run<bool, Ex1, Ex2, Ex3>(() => { throw ex2; });
            var err3 = Try.Run<bool, Ex1, Ex2, Ex3>(() => { throw ex3; });

            Assert.IsFalse(err1.HasValue);
            Assert.IsFalse(err2.HasValue);
            Assert.IsFalse(err3.HasValue);

            Assert.IsInstanceOfType(err1.Match(x => null, ex => ex), typeof(Ex1));
            Assert.IsInstanceOfType(err2.Match(x => null, ex => ex), typeof(Ex2));
            Assert.IsInstanceOfType(err3.Match(x => null, ex => ex), typeof(Ex3));

            Assert.AreEqual(err1.Match(x => null, ex => ex), ex1);
            Assert.AreEqual(err2.Match(x => null, ex => ex), ex2);
            Assert.AreEqual(err3.Match(x => null, ex => ex), ex3);

            try
            {
                Try.Run<bool, Ex1, Ex2, Ex3>(() => { throw ex0; });
                Assert.Fail();
            }
            catch { }

            try
            {
                Try.Run<bool, Ex1, Ex2, Ex3>(() => { throw ex4; });
                Assert.Fail();
            }
            catch { }

            Try.Run<bool, BaseEx, Ex2, Ex3>(() => { throw subEx; });
            Try.Run<bool, Ex, Ex2, Ex3>(() => { throw ex0; });
            Try.Run<bool, Ex, Ex2, Ex3>(() => { throw ex1; });

            var success = Try.Run<bool, Ex1, Ex2, Ex3>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Try_Catch4()
        {
            var ex0 = new Ex("ex");
            var ex1 = new Ex1("ex");
            var ex2 = new Ex2("ex");
            var ex3 = new Ex3("ex");
            var ex4 = new Ex4("ex");
            var ex5 = new Ex5("ex");
            var baseEx = new BaseEx("ex");
            var subEx = new SubEx("ex");

            var err1 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex1; });
            var err2 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex2; });
            var err3 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex3; });
            var err4 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex4; });

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

            try
            {
                Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex0; });
                Assert.Fail();
            }
            catch { }

            try
            {
                Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => { throw ex5; });
                Assert.Fail();
            }
            catch { }

            Try.Run<bool, BaseEx, Ex2, Ex3, Ex4>(() => { throw subEx; });
            Try.Run<bool, Ex, Ex2, Ex3, Ex4>(() => { throw ex0; });
            Try.Run<bool, Ex, Ex2, Ex3, Ex4>(() => { throw ex1; });

            var success = Try.Run<bool, Ex1, Ex2, Ex3, Ex4>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }

        [TestMethod]
        public void Extensions_Try_Catch5()
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

            var err1 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex1; });
            var err2 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex2; });
            var err3 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex3; });
            var err4 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex4; });
            var err5 = Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex5; });

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

            try
            {
                Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex0; });
                Assert.Fail();
            }
            catch { }

            try
            {
                Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => { throw ex6; });
                Assert.Fail();
            }
            catch { }

            Try.Run<bool, BaseEx, Ex2, Ex3, Ex4, Ex5>(() => { throw subEx; });
            Try.Run<bool, Ex, Ex2, Ex3, Ex4, Ex5>(() => { throw ex0; });
            Try.Run<bool, Ex, Ex2, Ex3, Ex4, Ex5>(() => { throw ex1; });

            var success = Try.Run<bool, Ex1, Ex2, Ex3, Ex4, Ex5>(() => true);
            Assert.IsTrue(success.ValueOr(false));
        }
    }
}
