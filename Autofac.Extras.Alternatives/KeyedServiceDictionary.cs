using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;

namespace Autofac.Extras.Alternatives
{
    internal class KeyedServiceDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IComponentContext _context;

        public KeyedServiceDictionary(IComponentContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var service in GetServices())
            {
                yield return new KeyValuePair<TKey, TValue>(
                    (TKey)service.ServiceKey,
                    (TValue)_context.ResolveService(service));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => GetServices().Count();

        public bool ContainsKey(TKey key) => _context.IsRegisteredService(GetService(key));

        public bool TryGetValue(TKey key, out TValue value)
        {
            object instance;
            if (_context.TryResolveService(GetService(key), out instance))
            {
                value = (TValue)instance;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key] => (TValue)_context.ResolveService(GetService(key));

        public IEnumerable<TKey> Keys => GetServices().Select(s => s.ServiceKey).Cast<TKey>();

        public IEnumerable<TValue> Values => this.Select(kvp => kvp.Value);

        private IEnumerable<KeyedService> GetServices()
        {
            return (
                from r in _context.ComponentRegistry.Registrations
                from s in r.Services.OfType<KeyedService>()
                where s.ServiceKey is TKey && s.ServiceType == typeof(TValue)
                select s).ToArray();
        }

        private static KeyedService GetService(TKey key) => new KeyedService(key, typeof(TValue));
    }
}