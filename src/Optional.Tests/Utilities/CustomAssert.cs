namespace Optional.Tests.Utilities
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
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
