using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Alternatives;
using Xunit;

namespace Tests.Unit
{
    public class KeyedServiceDictionaryTests
    {
        public KeyedServiceDictionaryTests()
        {
            _builder.RegisterAlternativeRelationships();

            _builder.RegisterType<List<string>>()
                    .Keyed<IList<string>>(3);

            _builder.RegisterType<HashSet<string>>()
                    .Keyed<ISet<string>>("D");
        }

        [Fact]
        public void Test_Indexer()
        {
            // Arrange.
            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            _builder.RegisterType<DependencyB>()
                    .Keyed<IDependency>("B");

            _builder.RegisterType<DependencyC>()
                    .Keyed<IDependency>("C");

            var container = _builder.Build();

            // Act.
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Assert.
            Assert.NotNull(index);

            Assert.IsType<DependencyA>(index["A"]);
            Assert.IsType<DependencyB>(index["B"]);
            Assert.IsType<DependencyC>(index["C"]);

            Assert.Equal("A", index["A"].Name);
            Assert.Equal("B", index["B"].Name);
            Assert.Equal("C", index["C"].Name);
        }

        [Fact]
        public void Test_TryGetValue_When_Service_Found()
        {
            // Arrange.
            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            IDependency expected;
            bool found = index.TryGetValue("A", out expected);

            // Assert.
            Assert.True(found);
            Assert.NotNull(expected);
            Assert.IsType<DependencyA>(expected);
        }

        [Fact]
        public void Test_TryGetValue_When_Service_Not_Found()
        {
            // Arrange.
            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            IDependency expected;
            bool found = index.TryGetValue("C", out expected);

            // Assert.
            Assert.False(found);
            Assert.Null(expected);
        }

        [Theory]
        [InlineData(true,  "A")]
        [InlineData(true,  "B")]
        [InlineData(false, "C")]
        public void Test_ContainsKey(bool expected, string key)
        {
            // Arrange.
            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            _builder.RegisterType<DependencyB>()
                    .Keyed<IDependency>("B");

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            bool actual = index.ContainsKey(key);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_GetEnumerator()
        {
            // Arrange.
            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            _builder.RegisterType<DependencyB>()
                    .Keyed<IDependency>("B");

            _builder.RegisterType<DependencyC>()
                    .Keyed<IDependency>("C");

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            var keys = index.Select(kvp => kvp.Key);
            var values = index.Select(kvp => kvp.Value);

            // Assert.
            Assert.Equal(new[] { "A", "B", "C" }, values.Select(v => v.Name));
            Assert.Equal(new[] { "A", "B", "C" }, keys);
        }

        [Fact]
        public void Test_GetEnumerator_With_Scanning()
        {
            // Arrange.
            _builder.RegisterAssemblyTypes(GetType().Assembly)
                    .Keyed(t => t.Name.Replace("Dependency", string.Empty), typeof(IDependency));

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            var keys = index.Select(kvp => kvp.Key);
            var values = index.Select(kvp => kvp.Value);

            // Assert.
            Assert.Equal(new[] { "A", "B", "C" }, values.Select(v => v.Name));
            Assert.Equal(new[] { "A", "B", "C" }, keys);
        }

        [Fact]
        public void Test_Count()
        {
            // Arrange.
            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            _builder.RegisterType<DependencyB>()
                    .Keyed<IDependency>("B");

            _builder.RegisterType<DependencyC>()
                    .Keyed<IDependency>("C");

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            int actual = index.Count;

            // Assert.
            Assert.Equal(3, actual);
        }

        [Fact]
        public void Test_Keys()
        {
            // Arrange.
            _builder.RegisterAssemblyTypes(GetType().Assembly)
                    .Keyed(t => t.Name.Replace("Dependency", string.Empty), typeof(IDependency));

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            var keys = index.Keys;

            // Assert.
            Assert.Equal(new[] { "A", "B", "C" }, keys);
        }
        [Fact]
        public void Test_Values()
        {
            // Arrange.
            _builder.RegisterAssemblyTypes(GetType().Assembly)
                    .Keyed(t => t.Name.Replace("Dependency", string.Empty), typeof(IDependency));

            var container = _builder.Build();
            var index = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Act.
            var values = index.Values;

            // Assert.
            Assert.Equal(new[] { "A", "B", "C" }, values.Select(v => v.Name));
        }

        [Fact]
        public void Test_Explicitly_Registered_Dictionary()
        {
            // Arrange.
            _builder.Register(_ => new Dictionary<int, IDependency> { { 1, new DependencyC() } })
                    .As<IReadOnlyDictionary<int, IDependency>>();

            _builder.RegisterType<DependencyA>()
                    .Keyed<IDependency>("A");

            _builder.RegisterType<DependencyB>()
                    .Keyed<IDependency>("B");

            var container = _builder.Build();

            // Act.
            var explicitIndex = container.Resolve<IReadOnlyDictionary<int, IDependency>>();
            var implicitIndex = container.Resolve<IReadOnlyDictionary<string, IDependency>>();

            // Assert.
            Assert.Equal(new[] { "C" }, explicitIndex.Values.Select(v => v.Name));
            Assert.IsNotType<KeyedServiceDictionary<int, IDependency>>(explicitIndex);

            Assert.Equal(new[] { "A", "B" }, implicitIndex.Values.Select(v => v.Name));
            Assert.IsType<KeyedServiceDictionary<string, IDependency>>(implicitIndex);
        }

        interface IDependency
        {
            string Name { get; }
        }

        class DependencyA : IDependency
        {
            public string Name => "A";
        }

        class DependencyB : IDependency
        {
            public string Name => "B";
        }

        class DependencyC : IDependency
        {
            public string Name => "C";
        }

        private readonly ContainerBuilder _builder = new ContainerBuilder();
    }
}