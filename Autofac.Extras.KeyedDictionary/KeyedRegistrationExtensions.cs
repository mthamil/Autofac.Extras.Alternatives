using System.Collections.Generic;

namespace Autofac.Extras.KeyedDictionary
{
    public static class KeyedRegistrationExtensions
    {
        /// <summary>
        /// Registers <see cref="IReadOnlyDictionary{TKey,TValue}"/> as a keyed service provider.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public static void RegisterKeyedDictionary(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(KeyedServiceDictionary<,>))
                   .As(typeof(IReadOnlyDictionary<,>))
                   .InstancePerLifetimeScope();
        }
    }
}