namespace Autofac.Extras.Alternatives
{
    /// <summary>
    /// Adds registration syntax for alternative relationship types to <see cref="ContainerBuilder"/>.
    /// </summary>
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