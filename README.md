Optional
=====

Optional is a simple option/maybe type for C#.

Version: 0.1.0.0

## Features

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

## Usage

Is coming soon...

### Using the library

To use Optional, simply import the following namespace:


```csharp
using Optional;
```

To use the LINQ query syntax, also import:

```csharp
using Optional.Linq;
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

Clearly, optional values are conceptually quite similar to nullables. Hence, a method is provided to convert a nullable into an optional value:

```csharp
int? nullableWithoutValue = null;
int? nullableWithValue = 2;
var none = nullableWithoutValue.ToOption();
var some = nullableWithValue.ToOption();
```

### Retrieving values

Optional forces you to consider both cases, that is if a value is present or not. Therefore, there is no way to simply force a retrieval of the value (although you can of course implement one yourself).

The most basic way to retrieve a value from an `Option<T>` is the following:

```csharp
// Returns the value if present, or otherwise an alternative value (10)
var value = option.ValueOr(10); 
```

In more elobarate scenarios, the `Match` method evaluates a specified function:

```csharp
// Evaluates one of the provided functions, and returns the result
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

### Transforming and filtering values

A few extension methods are provided to safely manipulate optional values.

The `Map` function transforms the inner value of an option. If no value is present, `None` is simply propagated:

```csharp
var none = Option.None<int>();
var stillNone = none.Map(x => x + 10);

var some = 10.Some();
var somePlus10 = some.Map(x => x + 10);
```

The `FlatMap` function chains several option values. It is similar to `Map`, but one must transform the value into a new optional value. The result of this mapping will be a nested `Option<T>`, but this will simply be flattened to a single one:

```csharp
var none = Option.None<int>();
var stillNone = none.FlatMap(x => x.Some()); // Returns another Option<int>

var some = 10.Some();
var stillSome = some.FlatMap(x => x.Some()); 
var nowNone = some.FlatMap(x => x.None()); // Returns None as the resulting option is empty
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
var nowNone = some.Filter(x => x != 10);
```

### Working with LINQ query syntax
