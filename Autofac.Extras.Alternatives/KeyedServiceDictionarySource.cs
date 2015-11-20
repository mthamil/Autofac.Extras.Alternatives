using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Extras.Alternatives
{
    /// <summary>
    /// Provides service indexes that implement <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
    /// </summary>
    public class KeyedServiceDictionarySource : IRegistrationSource
    {
        /// <summary>
        /// Retrieve registrations for an unregistered service, to be used
        /// by the container.
        /// </summary>
        /// <param name="service">The service that was requested.</param>
        /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
        /// <returns> Registrations providing the service.</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var typedService = service as IServiceWithType;
            if (typedService != null)
            {
                var serviceType = typedService.ServiceType;
                if (serviceType.IsClosedTypeOf(typeof(IReadOnlyDictionary<,>)))
                {
                    var existing = registrationAccessor(service);
                    if (existing.Any())
                        return existing;

                    var keyType = serviceType.GenericTypeArguments[0];
                    var valueType = serviceType.GenericTypeArguments[1];

                    var registration = RegistrationBuilder
                        .ForType(typeof(KeyedServiceDictionary<,>).MakeGenericType(keyType, valueType))
                        .As(serviceType)
                        .InstancePerLifetimeScope()
                        .CreateRegistration();

                    return new[] { registration };
                }
            }

            return Enumerable.Empty<IComponentRegistration>();
        }

        /// <summary>
        /// Gets whether the registrations provided by this source are 1:1 adapters on top
        /// of other components (I.e. like Meta, Func or Owned.)
        /// </summary>
        public bool IsAdapterForIndividualComponents => false;
    }
}