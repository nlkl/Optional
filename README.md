![Optional](https://raw.githubusercontent.com/nlkl/Optional/master/icon/Logo.png)

Optional is a robust option/maybe type for C#.

Version: 4.0.0

## What and Why?

Optional is a strongly typed alternative to null values that lets you:

* Avoid those pesky null-reference exceptions
* Signal intent and model your data more explictly
* Cut down on manual null checks and focus on your domain

## Features

* Robust and well tested
* Self contained with no dependencies
* Easily installed through NuGet
* Supports **.NET 3.5+** and **.NET Core** (.NET Standard 1.0+)
* Focused but full-featured API

## Installation

Simply reference Optional.dll and you are good to go!

Optional is also available via NuGet:

```
PM> Install-Package Optional 
```

Or visit: [https://www.nuget.org/packages/Optional/](https://www.nuget.org/packages/Optional/)

## Core concepts

The core concept behind Optional is derived from two common functional programming constructs, typically referred to as a maybe type and an either type (referred to as `Option<T>` and `Option<T, TException>` in Optional).

Many functional programming languages disallow null values, as null-references can introduce hard-to-find bugs. A maybe type is a type-safe alternative to null values. 

In general, an optional value can be in one of two states: Some (representing the presence of a value) and None (representing the lack of a value). Unlike null, an option type forces the user to check if a value is actually present, thereby mitigating many of the problems of null values. `Option<T>` is a struct in Optional, making it impossible to assign a null value to an option itself.

Further, an option type is a lot more explicit than a null value, which can make APIs based on optional values a lot easier to understand. Now, the type signature will indicate if a value can be missing!

An either type is conceptually similar to a maybe type. Whereas a maybe type only indicates if a value is present or not, an either type contains an auxiliary value describing how an operation failed. Apart from this *exceptional* value, an either-type behaves much like its simpler counterpart.

Working with maybe and either types is very similar, and the description below will therefore focus on the maybe type, and only provide a quick summary for the either type.

Finally, Optional offers several utility methods that make it easy and convenient to work with both of the above described optional values.

## Usage

### Using the library

To use Optional simply import the following namespace:

```csharp
using Optional;
```

A few auxiliary namespaces are provided:

```csharp
using Optional.Linq; // Linq query syntax support
using Optional.Unsafe; // Unsafe value retrieval
```

### Creating optional values

The most basic way to create optional values is to use the static `Option` class:

```csharp
var none = Option.None<int>();
var some = Option.Some(10);
```

For convenience, a set of extension methods are provided to make this a little less verbose:

```csharp
var none = 10.None(); // Creates a None value, with 10 determining its type (int)
var some = 10.Some();
```

Note that it is also allowed (but hardly recommended) to wrap null values in an Option instance:

```csharp
string nullString = null;
var someWithNull = nullString.Some();
```

To make it easier to filter away such null values, a specialized extension method is provided:

```csharp
string nullString = null;
var none = nullString.SomeNotNull(); // Returns None if original value is null
```

Similarly, a more general extension method is provided, allowing a specified predicate:

```csharp
string str = "abc";
var none = str.SomeWhen(s => s == "cba"); // Return None if predicate is violated
var none = str.NoneWhen(s => s == "abc"); // Return None if predicate is satisfied
```

Clearly, optional values are conceptually quite similar to nullables. Hence, a method is provided to convert a nullable into an optional value:

```csharp
int? nullableWithoutValue = null;
int? nullableWithValue = 2;
var none = nullableWithoutValue.ToOption();
var some = nullableWithValue.ToOption();
```

### Retrieving values

When retrieving values, Optional forces you to consider both cases (that is if a value is present or not).

Firstly, it is possible to check if a value is actually present:

```csharp
var hasValue = option.HasValue;
```

If you want to check if an option contains a specific value, you can use the `Contains` or `Exists` methods. The former checks if the optional contains a specified value, the latter if the contained value satisfies some predicate:

```csharp
var isThousand = option.Contains(1000);
var isGreaterThanThousand = option.Exists(val => val > 1000);
```

The most basic way to retrieve a value from an `Option<T>` is the following:

```csharp
// Returns the value if present, or otherwise an alternative value (10)
var value = option.ValueOr(10);
var value = option.ValueOr(() => SlowOperation());  // Lazy variant
```

In more elaborate scenarios, the `Match` method evaluates a specified function:

```csharp
// Evaluates one of the provided functions and returns the result
var value = option.Match(x => x + 1, () => 10); 

// Or written in a more functional'ish style (think pattern matching)
var value = option.Match(
  some: x => x + 1, 
  none: () => 10
);
```

There is a similar `Match` function to simply induce side-effects:

```csharp
// Evaluates one of the provided actions
option.Match(x => Console.WriteLine(x), () => Console.WriteLine(10)); 

// Or pattern matching'ish as before
option.Match(
  some: x => Console.WriteLine(x), 
  none: () => Console.WriteLine(10)
);
```

Finally, side-effect matching (that is matching without returning a value) can be carried out for each case separately:

```csharp
// Evaluated if the value is present
option.MatchSome(x => 
{
    Console.WriteLine(x)
});

// Evaluated if the value is absent
option.MatchNone(() => 
{
    Console.WriteLine("Not found") 
});
```

### Retrieving values without safety

In some cases you might be absolutely sure that a value is present. Alternatively, the lack of a value might be fatal to your program, in which case you just want to indicate such a failure.

In such scenarios, Optional allows you to drive without a seatbelt. However, to stress the lack safety, another namespace needs to be imported:

```csharp
using Optional.Unsafe;
```

When imported, values can be retrieved unsafely as:

```csharp
var value = option.ValueOrFailure();
var anotherValue = option.ValueOrFailure("An error message"); 
```

In case of failure an `OptionValueMissingException` is thrown.

In a lot of interop scenarios, it might be necessary to convert an option into a potentially null value. Once the Unsafe namespace is imported, this can be done relatively concisely as:

```csharp
var value = option.ValueOrDefault(); // value will be default(T) if the option is empty.
```

As a rule of thumb, such conversions should be performed only just before the nullable value is needed (e.g. passed to an external library), to minimize and localize the potential for null reference exceptions and the like. 

### Transforming and filtering values

A few extension methods are provided to safely manipulate optional values.

The `Or` function makes it possible to specify an alternative value. If the option is none, a some instance will be returned:

```csharp
var none = Option.None<int>();
var some = none.Or(10); // A some instance, with value 10
var some = none.Or(() => SlowOperation()); // Lazy variant
```

Similarly, the `Else` function enables you to specify an alternative option, which will replace the current one, in case no value is present. Notice, that both options might be none, in which case a none-option will be returned:

```csharp
var none = Option.None<int>();
var some = none.Else(10.Some()); // A some instance, with value 10
var some = none.Else(Option.None<int>()); // A none instance
var some = none.Else(() => Option.Some<int>()); // Lazy variant
```

The `Map` function transforms the inner value of an option. If no value is present none is simply propagated:

```csharp
var none = Option.None<int>();
var stillNone = none.Map(x => x + 10);

var some = 10.Some();
var somePlus10 = some.Map(x => x + 10);
```

The `FlatMap` function chains several optional values. It is similar to `Map`, but the return type of the transformation must be another optional. If either the resulting or original optional value is none, a none instance is returned. Otherwise, a some instance is returned according to the specified transformation:

```csharp
var none = Option.None<int>();
var stillNone = none.FlatMap(x => x.Some()); // Returns another Option<int>

var some = 10.Some();
var stillSome = some.FlatMap(x => x.Some()); 
var none = some.FlatMap(x => x.None()); // Turns empty, as it maps to none
```

`FlatMap` is useful in combination with methods that return optional values themselves:

```csharp
public static Option<Person> FindPersonById(int id) { ... }
public static Option<Hairstyle> GetHairstyle(Person person) { ... }

var id = 10;
var person = FindPersonById(id);
var hairstyle = person.FlatMap(p => GetHairstyle(p));
hairstyle.Match( ... );
```

In case you end up with a nested optional (e.g. `Option<Option<T>>`), you might flatten it by flatmapping it onto itself, but a dedicated `Flatten` function is offered for convenience:

```csharp
Option<Option<T>> nestedOption = ...
Option<T> option = nestedOption.Flatten(); // same as nestedOption.FlatMap(o => o)
```

Finally, it is possible to perform filtering. The `Filter` function returns none, if the specified predicate is not satisfied. If the option is already none, it is simply returned as is:

```csharp
var none = Option.None<int>();
var stillNone = none.Filter(x => x > 10);

var some = 10.Some();
var stillSome = some.Filter(x => x == 10);
var none = some.Filter(x => x != 10);
```

A recurring scenario, when working with null-returning APIs, is that of filtering away null values after a mapping. To ease the pain, a specific `NotNull` filter is provided:

```csharp
// Returns none if the parent node is null
var parent = GetNode()
    .Map(node => node.Parent)
    .NotNull(); 
```

### Enumerating options

An option implements `GetEnumerator`, allowing you to loop over the value, as if it was a collection with either a single or no elements.

```csharp
foreach (var value in option)
{
    Console.WriteLine(value);
}
```

As you might have noticed, this is a nice and lightweight alternative to `Match` in cases where you only want to do something if the value is present. Also, you should use this instead of the more verbose and unsafe combination of `option.HasValue` and `option.ValueOrFailure()`, which you might otherwise be tempted to try.

Notice, however, that options don't actually implement `IEnumerable<T>`, in order to not pollute the options with LINQ extension methods and the like. Although many LINQ methods share functionality similar to those offered by an option, they offer a more collection-oriented interface, and includes several unsafe functions (such as `First`, `Single`, etc).

Although options deliberately don't act as enumerables, you can easily convert an option to an enumerable by calling the `ToEnumerable()` method:

```csharp
var enumerable = option.ToEnumerable();
```

### Working with LINQ query syntax

Optional supports LINQ query syntax, to make the above transformations somewhat cleaner.

To use LINQ query syntax you must import the following namespace:

```csharp
using Optional.Linq;
```

This allows you to do fancy stuff such as:

```csharp
var personWithGreenHair =
  from person in FindPersonById(10)
  from hairstyle in GetHairstyle(person)
  from color in ParseStringToColor("green")
  where hairstyle.Color == color
  select person;
```

In general, this closely resembles a sequence of calls to `FlatMap` and `Filter`. However, using query syntax can be a lot easier to read in complex cases.

### Equivalence and comparison

Two optional values are equal if the following is satisfied:

* The two options have the same type
* Both are none, both contain null values, or the contained values are equal

An option both overrides `object.Equals` and implements `IEquatable<T>`, allowing efficient use in both generic and untyped scenarios. The `==` and `!=` operators are also provided for convenience. In each case, the semantics are identical.

The generated hashcodes also reflect the semantics described above.

Further, options implement `IComparable<T>` and overload the corresponding comparison operators (`< > <= >=`). The implementation is consistent with the above described equality semantics, and comparison itself is based on the following rules:

* An empty option is considered less than a non-empty option
* For non-empty options comparison is delegated to the default comparer and applied on the contained value

### Options with exceptional values

As described above, Optional support the notion of an either type, which adds and exception value, indicating how an operation went wrong.

An `Option<T, TException>` can be created directly, just like the `Option<T>`. Unlike in this simple case, we need to specify potential exceptional values (and a lot of verbose type annotations - sorry guys):

```csharp
var none = Option.None<int, ErrorCode>(ErrorCode.GeneralError);
var some = Option.Some<int, ErrorCode>(10);

// These extension methods are hardly useful in this case,
// but here for consistency
var none = 10.None(ErrorCode.GeneralError);
var some = 10.Some<int, ErrorCode>();

string str = "abc";
var none = str.SomeWhen(s => s == "cba", ErrorCode.GeneralError);
var none = str.SomeWhen(s => s == "cba", () => SlowOperation()); // Lazy variant

string nullString = null;
var none = nullString.SomeNotNull(ErrorCode.GeneralError); 
var none = nullString.SomeNotNull(() => SlowOperation()); // Lazy variant

int? nullableWithoutValue = null;
int? nullableWithValue = 2;
var none = nullableWithoutValue.ToOption(ErrorCode.GeneralError);
var some = nullableWithValue.ToOption(ErrorCode.GeneralError);
var some = nullableWithValue.ToOption(() => SlowOperation()); // Lazy variant
```

Retrieval of values is very similar as well:

```csharp
var hasValue = option.HasValue;
var isThousand = option.Contains(1000);
var isGreaterThanThousand = option.Exists(val => val > 1000);

var value = option.ValueOr(10);
var value = option.ValueOr(() => SlowOperation()); // Lazy variant
var value = option.ValueOr(exception => (int)exception); // Mapped from exceptional value

// If the value and exception is of identical type, 
// it is possible to return the one which is present
var value = option.ValueOrException(); 
```

The `Match` methods include the exceptional value in the none-case:

```csharp
var value = option.Match(
  some: value => value + 1, 
  none: exception => (int)exception
);

option.Match(
  some: value => Console.WriteLine(value), 
  none: exception => Console.WriteLine(exception)
);

option.MatchSome(value => Console.WriteLine(value));
option.MatchNone(exception => Console.WriteLine(exception));
```

And again, when `Optional.Unsafe` is imported, it is possible to retrieve the value without safety:

```csharp
var value = option.ValueOrFailure();
var anotherValue = option.ValueOrFailure("An error message"); 
var potentiallyNullValue = option.ValueOrDefault();
```

Values can be conveniently transformed using similar operations to that of the `Option<T>`. It is however important to note, that these transformations are all **short-circuiting**! That is, if an option is already none, the current exceptional value will remain, and not be replaced by any subsequent filtering. In this respect, this exceptional value is very similar to actual exceptions (hence the name).

```csharp
var none = Option.None<int, ErrorCode>(ErrorCode.GeneralError);
var some = none.Or(10);
var some = none.Or(() => SlowOperation()); // Lazy variant
var some = none.Or(exception = (int)exception); // Mapped from exceptional value
var some = none.Else(10.Some<int, ErrorCode>()); // A some instance with value 10
var some = none.Else(Option.None<int, ErrorCode>(ErrorCode.FatalError)); // A none instance carrying a ErrorCode.FatalError
var some = none.Else(() => 10.Some<int, ErrorCode>()); // Lazy variant
var some = none.Else(exception = Option.None<int, ErrorCode>(exception)); // Mapped from exceptional value

// Mapping

var none = Option.None<int, ErrorCode>(ErrorCode.GeneralError);
var stillNone = none.Map(x => x + 10);

var some = Option.Some<int, ErrorCode>(10);
var somePlus10 = some.Map(x => x + 10);

// Flatmapping

var none = Option.None<int, ErrorCode>(ErrorCode.GeneralError);
var stillNone = none.FlatMap(x => x.Some<int, ErrorCode>());

var some = Option.Some<int, ErrorCode>(10);
var stillSome = some.FlatMap(x => x.Some<int, ErrorCode>()); 
var none = some.FlatMap(x => x.None(ErrorCode.GeneralError));

Option<Option<int, ErrorCode>, ErrorCode> nestedOption = ...
Option<int, ErrorCode> option = nestedOption.Flatten();

// Filtering

var result = Option.Some<int, ErrorCode>(10)
    .Filter(x => true, ErrorCode.GeneralError) // Stil some
    .Filter(x => false, ErrorCode.GeneralError) // Now "GeneralError"
    .Filter(x => false, ErrorCode.IncorrectValue) // Still "GeneralError"
    .Filter(x => false, () => SlowOperation()); // Lazy variant

var result = Option.Some<string, ErrorCode>(null)
    .NotNull(ErrorCode.GeneralError) // Returns none if the contained value is null
    .NotNull(() => SlowOperation()); // Lazy variant
```

Enumeration works identically to that of `Option<T>`:

```csharp
foreach (var value in option)
{
    // Do something
}

var enumerable = option.ToEnumerable();
```

LINQ query syntax is supported, with the notable exception of the `where` operator (as it doesn't allow us to specify an exceptional value to use in case of failure):

```csharp
var optionalDocument =
  from file in user.GetFileFromDatabase()
  from document in FetchFromService(file.DocumentId)
  select document;

optionalDocument.Match(
    some: document => Console.WriteLine(document.Contents), 
    none: errorCode => Console.WriteLine(errorCode)
);
```

### Interop between `Option<T>` and `Option<T, TException>`

To make interop between `Option<T>` and `Option<T, TException>` more convenient, several utility methods are provided for this purpose.

The most basic of such operations, is to simply convert between the two types:

```csharp
var some = Option.Some("This is a string");

// To convert to an Option<T, TException>, we need to tell which 
// exceptional value to use if the current option is none
var someWithException = some.WithException(ErrorCode.GeneralError);
var someWithException = some.WithException(() => SlowOperation()); // Lazy variant

// It is easy to simply drop the exceptional value
var someWithoutException = someWithException.WithoutException();
```

When flatmapping, it is similarly possible to flatmap into a value of the other type:

```csharp
// The following flatmap simply ignores the new exceptional value
var some = Option.Some("This is a string");
var none = some.FlatMap(x => x.None(ErrorCode.GeneralError));

// The following flatmap needs an explicit exceptional value 
// as a second argument
var some = Option.Some<string, ErrorCode>("This is a string");
var none = some.FlatMap(x => Option.None<string>(), ErrorCode.GeneralError);
var none = some.FlatMap(x => Option.None<string>(), () => SlowOperation()); // Lazy variant
```

### Working with collections

Optional provides a few convenience methods to ease interoperability with common .NET collections, and improve null safety a bit in the process.

LINQ provides a lot of useful methods when working with enumerables, but methods such as `FirstOrDefault`, `LastOrDefault`, `SingleOrDefault`, and `ElementAtOrDefault`, all return null (more precisely `default(T)`) to indicate that no value was found (e.g. if the enumerable was empty). Optional provides a safer alternative to all these methods, returning an option to indicate success/failure instead of nulls. As an added benefit, these methods work unambiguously for non-nullable/structs types as well, unlike their LINQ counterparts. 

```csharp
var option = values.FirstOrNone();
var option = values.FirstOrNone(v => v != 0);
var option = values.LastOrNone();
var option = values.LastOrNone(v => v != 0);
var option = values.SingleOrNone();
var option = values.SingleOrNone(v => v != 0);
var option = values.ElementAtOrNone(10);
```

(Note that unlike `SingleOrDefault`, `SingleOrNone` never throws an exception but returns None in all "invalid" cases. This slight deviation in semantics was considered a safer alternative to the existing behavior, and is easy to work around in practice, if the finer granularity is needed.)

Optional provides a safe way to retrieve values from a dictionary:

```csharp
var option = dictionary.GetValueOrNone("key");
```

`GetValueOrNone` behaves similarly to `TryGetValue` on an `IDictionary<TKey, TValue>` or `IReadOnlyDictionary<TKey, TValue>`, but actually supports any `IEnumerable<KeyValuePair<TKey, TValue>>` (falling back to iteration, when a direct lookup is not possible).

Another common scenario, is to perform various transformations on an enumerable and ending up with a sequence of options (e.g. `IEnumerable<Option<T>>`). In many cases, only the non-empty options are relevant, and as such Optional provides a convenient method to flatten a sequence of options into a sequence containing all the inner values (whereas empty options are simply thrown away):

```csharp
var options = new List<Option<int>> { Option.Some(1), Option.Some(2), Option.None<int>() };
var values = option.Values(); // IEnumerable<int> { 1, 2 }
```

When working with a sequence of `Option<T, TException>` a similar method is provided, as well a way to extract all the exceptional values:

```csharp
var options = GetOptions(); // IEnumerable<Option<int, string>> { Some(1), None("error"), Some(2) }
var values = options.Values(); // IEnumerable<int> { 1, 2 }
var exceptions = options.Exceptions(); // IEnumerable<string> { "error" }
```
