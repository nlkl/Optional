using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Extensions.Internal
{
    internal static class Guard
    {
        public static void NotNull<T>(T source, string name)
        {
            if (source == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
