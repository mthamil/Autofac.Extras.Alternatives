using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Extras.KeyedDictionary
{
    /// <summary>
    /// Provides support for <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
    /// </summary>
    public class KeyedServiceDictionaryRegistrationSource : IRegistrationSource
    {

        /// <summary>
        /// Retrieve registrations for an unregistered service, to be used by the container.
        /// </summary>
        /// <param name="service">The service that was requested.</param>
        /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
        /// <returns>Registrations providing the service.</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var typedService = service as IServiceWithType;
            if (typedService != null)
            {
                var serviceType = typedService.ServiceType;
                if (serviceType.IsClosedTypeOf(typeof(IReadOnlyDictionary<,>)))
                {
                    var existingRegistrations = registrationAccessor(service);
                    if (existingRegistrations.Any())
                        return existingRegistrations;

                    var registration = RegistrationBuilder.ForType(typeof(KeyedServiceDictionary<,>))
                                                          .As(typeof(IReadOnlyDictionary<,>))
                                                          .InstancePerLifetimeScope()
                                                          .CreateRegistration();

                    return new[] { registration };
                }
            }

            return Enumerable.Empty<IComponentRegistration>();
        }

        /// <summary>
        /// Gets whether the registrations provided by this source are 1:1 adapters on top
        /// of other components (ie. like Meta, Func, or Owned.)
        /// </summary>
        /// <remarks>Always returns false.</remarks>
        public bool IsAdapterForIndividualComponents => false;

        ///// <summary>
        ///// A description of the registration source.
        ///// </summary>
        //public override string ToString() => OrderedRegistrationSourceResources.OrderedRegistrationSourceDescription;

        //private static IComponentRegistration CreateOrderedRegistration<TService>()
        //{
        //    var registration = RegistrationBuilder
        //        .ForDelegate((c, ps) =>
        //            c.ResolveOrdered<TService>(ps))
        //        .ExternallyOwned()
        //        .CreateRegistration();

        //    return registration;
        //}

        //private static readonly MethodInfo CreateRegistrationMethod =
        //    typeof(OrderedRegistrationSource).GetMethod(nameof(CreateOrderedRegistration),
        //                                                BindingFlags.NonPublic |
        //                                                BindingFlags.Static);

        //internal const string OrderingMetadataKey = "AutofacOrderingMetadataKey";
    }
}