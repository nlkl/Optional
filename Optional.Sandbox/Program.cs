using Optional;
using Optional.Linq;
using Optional.Extensions;
using Optional.Extensions.Collections;
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
            var someResult = "Yes!".Some();
            var noneResult = Option.None<string>();

            var someWithHint = someResult.WithException("Missing!");
            var someWithoutHint = someWithHint.WithoutException();

            var noneWithHint = noneResult.WithException("Missing!");
            var noneWithoutHint = noneWithHint.WithoutException();

            var success = GetResponse(someResult);
            var error = GetResponse(noneResult);

            var x =
                from res in someWithHint.Some<Option<string, string>, string>()
                from y in res
                select res.ValueOr("a") + y;

            Console.WriteLine(x.ValueOr("----"));

            Console.WriteLine(success);
            Console.WriteLine(error);

            Console.Read();
        }

        private abstract class Response { }
        private class Success : Response { public string Result { get; set; } }
        private class Error : Response { }

        private static Response GetResponse(Option<string> result)
        {
            return result.Match<Response>(r => GetSuccessResponse(r), () => GetErrorResponse());
        }

        private static Success GetSuccessResponse(string result)
        {
            return new Success() { Result = result };
        }

        private static Error GetErrorResponse()
        {
            return new Error();
        }
    }
}
