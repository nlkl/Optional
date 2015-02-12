Optional
=====

Optional is a simple option/maybe type for C#.

Version: 2.1.0

## Features at a glance

* Avoid null-reference exceptions, by using a type-safe alternative to null values.
* Transform optional values safely, without the manual null-checks.
* Easily chain and combine null values using LINQ query syntax.

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

Further, an option type is a lot more explicit than a null value, which can make APIs based on optional values a lot easier to understand.

An either type is conceptually similar to a maybe type. Whereas a maybe type only indicates if a value is present or not, an either type contains an auxilliary value describing how an operation failed. Apart from this *exceptional* value, an either-type behaves much like its simpler counterpart.

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

If you want to check if an option contains a specific value, you can use the `Contains` or `Exists` methods. The first one checks if the optional contains a specified value, the second if the contained value satisfies some predicate:

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

In more elobarate scenarios, the `Match` method evaluates a specified function:

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

### Transforming and filtering values

A few extension methods are provided to safely manipulate optional values.

The `Or` function makes it possible to specify an alternative value. If the option is none, a some instance will be returned:

```csharp
var none = Option.None<int>();
var some = none.Or(10); // A some instance, with value 10
var some = none.Or(() => SlowOperation()); // Lazy variant
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

Finally, it is possible to perform filtering. The `Filter` function returns none, if the specified predicate is not satisfied. If the option is already none, it is simply returned as is:

```csharp
var none = Option.None<int>();
var stillNone = none.Filter(x => x > 10);

var some = 10.Some();
var stillSome = some.Filter(x => x == 10);
var none = some.Filter(x => x != 10);
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

### Equivalence

Two optional values are equal if the following is satisfied:

* The two options have the same type
* Both are none, both contain null values, or the contained values are equal
 
The generated hashcodes also reflect the semantics described above.

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
```
And again, when `Optional.Unsafe` is imported, it is possible to retrieve the value without safety:

```csharp
var value = option.ValueOrFailure();
var anotherValue = option.ValueOrFailure("An error message"); 
```

Values can be conveniently transformed using similar operations to that of the `Option<T>`. It is however important to note, that these transformations are all **short-circuiting**! That is, if an option is already none, the current exceptional value will remain, and not be replaced by any subsequent filtering. In this respect, this exceptional value is very similar to actual exceptions (hence the name).

```csharp
var none = Option.None<int, ErrorCode>(ErrorCode.GeneralError);
var some = none.Or(10);
var some = none.Or(() => SlowOperation()); // Lazy variant

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

// Filtering

var result = Option.Some<int, ErrorCode>(10)
    .Filter(x => true, ErrorCode.GeneralError) // Stil some
    .Filter(x => false, ErrorCode.GeneralError) // Now "GeneralError"
    .Filter(x => false, ErrorCode.IncorrectValue) // Still "GeneralError"
    .Filter(x => false, () => SlowOperation()); // Lazy variant
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
