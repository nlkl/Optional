using Optional;
using Optional.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = 2.0;
            string y = null;
            int? z = 2;

            var option1 = Option.None<int>();
            var option2 = x.Some();
            var option3 = y.SomeNotNull();
            var option4 = z.ToOption();

            var option5 = option1
                .Map(a => a + 1)
                .Map(a => a + 2)
                .Map(a => a + 3);

            var option6 = option4
                .Map(a => a + 1)
                .Map(a => a + 2)
                .Map(a => a + 3);

            var option7 = from opt1 in option1
                          from opt2 in option2
                          where opt2 > 1.0
                          select opt1 + opt2;

            Console.WriteLine(option1.ValueOr(-10));
            Console.WriteLine(option2.ValueOr(-10.0));
            Console.WriteLine(option3.ValueOr("-10"));
            Console.WriteLine(option4.ValueOr(-10));
            Console.WriteLine(option5.ValueOr(-10));
            Console.WriteLine(option6.ValueOr(-10));
            Console.WriteLine(option7.ValueOr(-10));

            var vals = Enumerable.Empty<int>();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000000; i++)
            {
                try
                {
                    var v = vals.Take(1).ToArray();
                }
                catch (InvalidOperationException ex)
                {

                }
            }
            Console.WriteLine(sw.ElapsedMilliseconds);

            Console.Read();
        }
    }
}
