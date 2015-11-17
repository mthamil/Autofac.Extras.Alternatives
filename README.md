Autofac.Extras.IndexAlternative
=======================
An Autofac extension that adds recognition of `IReadOnlyDictionary<TKey, TValue>` as a relationship type that provides the same
capabilities as the built-in Autofac type `IIndex<TKey, TValue>`. While Autofac's relationship types are very useful, sometimes 
it is desirable to not require a dependency on Autofac for these types in libraries that make use of keyed dependencies.

Download
========
Visit https://www.nuget.org/packages/Autofac.Extras.IndexAlternative/ to download.

Usage
=====
