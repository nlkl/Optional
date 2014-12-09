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

### Transforming and filtering values

### Working with LINQ query syntax
