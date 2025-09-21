using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Syrx.Validation.Contract;

namespace Syrx.Commanders.Databases.Settings.Extensions.Json
{
    public static class ServiceCollectionExtensions
    {
        //[Obsolete("This method is deprecated and will be removed in the 3.0.0 version.", true)]
        public static IServiceCollection AddSyrxJsonFile(
            this IServiceCollection services,
            IConfigurationBuilder builder,
            string fileName)
        {
            Throw<ArgumentNullException>(builder != null, $"ConfigurationBuilder is null! Check bootstrap.");
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(fileName), nameof(fileName));

            builder?.AddJsonFile(fileName);
            return services;
        }

        
    }
}
