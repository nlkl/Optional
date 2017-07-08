using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Optional.Tests.Utilities
{
    public static class CustomAssert
    {
        public static void Throws<TException>(Action action)
            where TException : Exception
        {
            var success = false;
            try
            {
                action();
                success = true;
            }
            catch (TException) { }
            catch (Exception)
            {
                Assert.Fail();
            }

            if (success)
            {
                Assert.Fail();
            }
        }
    }
}
