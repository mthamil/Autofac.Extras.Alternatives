namespace Autofac.Extras.Alternatives
{
    public static class AlternativeRegistrationExtensions
    {
        /// <summary>
        /// Registers alternative relationship types
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public static void RegisterAlternativeRelationships(this ContainerBuilder builder)
        {
            builder.RegisterSource(new KeyedServiceDictionarySource());
        }
    }
}