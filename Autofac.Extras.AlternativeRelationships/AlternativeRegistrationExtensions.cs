using System.Collections.Generic;

namespace Autofac.Extras.AlternativeRelationships
{
    public static class AlternativeRegistrationExtensions
    {
        /// <summary>
        /// Registers alternative relationship types
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public static void RegisterAlternativeRelationships(this ContainerBuilder builder)
        {
            builder.RegisterKeyedServiceProvider();
        }

        /// <summary>
        /// Registers <see cref="IReadOnlyDictionary{TKey,TValue}"/> as a keyed service provider.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        static void RegisterKeyedServiceProvider(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(KeyedServiceDictionary<,>))
                   .As(typeof(IReadOnlyDictionary<,>))
                   .InstancePerLifetimeScope();
        }
    }
}