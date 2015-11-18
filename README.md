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
Visit https://www.nuget.org/packages/Autofac.Extras.Alternatives/ to download.

Usage
=====
