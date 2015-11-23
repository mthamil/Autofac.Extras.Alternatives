Autofac.Extras.Alternatives
=======================
An Autofac extension that adds support for alternatives to some of the Autofac-provided relationship types.

This includes recognition of `IReadOnlyDictionary<TKey, TValue>` as a relationship type that provides the same
capabilities as the built-in Autofac type `IIndex<TKey, TValue>`. 

While Autofac's relationship types are very useful, it is sometimes desirable to not require a dependency 
on Autofac for these types in libraries.

[![Build status](https://ci.appveyor.com/api/projects/status/5wc1v3lqwealil3x)](https://ci.appveyor.com/project/mthamil/autofac-extras-alternatives)

Download
========
Visit [![NuGet](https://img.shields.io/nuget/v/Autofac.Extras.Alternatives.svg)](https://www.nuget.org/packages/Autofac.Extras.Alternatives/) to download.

Usage
=====

First, register the alternative relationship types using an extension method on `ContainerBuilder`:

```C#
    _builder.RegisterAlternativeRelationships();
```

Similarly to Autofac's `IIndex<TKey, TValue>`, register types that implement the same interface with a particular key:

```C#
    public interface IDependency
    {
        string Name { get; }
    }

    public class DependencyA : IDependency
    {
        public string Name => "A";
    }

    public class DependencyB : IDependency
    {
        public string Name => "B";
    }

    ...

    _builder.RegisterType<DependencyA>()
            .Keyed<IDependency>("A");

    _builder.RegisterType<DependencyB>()
            .Keyed<IDependency>("B");
```

Then, register a type that has a constructor declared that accepts an `IReadOnlyDictionary<TKey, TValue>` corresponding to the keyed types:

```C#
    public class SomeComponent
    {
        public SomeComponent(IReadOnlyDictionary<string, IDependency> index)
        {
        }
    }
```

When `SomeDependency` is resolved, it will be supplied with an implementation that provides `DependencyA` and `DependencyB`.