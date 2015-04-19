using Optional;
using Optional.Linq;
using Optional.Extensions;
using Optional.Extensions.Collections;
using Optional.Extensions.Async;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional.Sandbox
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Option<Address> TryGetAddress()
        {
            return new Address
            {
                Id = 1,
                StreetName = "Option Street",
                StreetNumber = "22",
                City = "Optionburg",
                PostalCode = "2222",
                Country = "Optionland"
            }.Some();
        }

        public Task<Option<Address>> TryGetAddressAsync()
        {
            return Task.FromResult(TryGetAddress());
        }
    }

    public class Address
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public static class Repository
    {
        public static Option<Person> TryGetPerson(int id)
        {
            if (id == 1)
            {
                return new Person
                {
                    Id = 1,
                    Name = "Jens Jensen",
                    Age = 32
                }.Some();
            }

            return Option.None<Person>();
        }

        public static Task<Option<Person>> TryGetPersonAsync(int id)
        {
            return Task.FromResult(TryGetPerson(id));
        }
    }

    class Program
    {
        public static async Task TestAddresses()
        {
            var optionalAddress = Repository
                .TryGetPerson(1)
                .FlatMap(p => p.TryGetAddress());

            var optionalAddress2 = await await Repository
                .TryGetPersonAsync(1)
                .ContinueWith(optionalPerson =>
                    optionalPerson.Result
                    .Match(
                        some: p => p.TryGetAddressAsync(),
                        none: () => Task.FromResult(Option.None<Address>())
                    ));

            var optionalAddress3 = await Repository
                .TryGetPersonAsync(1)
                .FlatMap(optionalPerson => optionalPerson
                    .Match(
                        some: p => p.TryGetAddressAsync(),
                        none: () => Task.FromResult(Option.None<Address>())
                    ));

            optionalAddress.Match(
                some: addr => Console.WriteLine(addr.Country),
                none: () => Console.WriteLine("Not found")
            );

            optionalAddress2.Match(
                some: addr => Console.WriteLine(addr.Country),
                none: () => Console.WriteLine("Not found")
            );

            optionalAddress3.Match(
                some: addr => Console.WriteLine(addr.Country),
                none: () => Console.WriteLine("Not found")
            );
        }

        static void Main(string[] args)
        {
            TestAddresses().Wait();

            Console.Read();
        }
    }
}
