using Microsoft.Extensions.Configuration;
using Syrx.Extensions;
using static Syrx.Validation.Contract;

namespace Syrx.Commanders.Databases.Settings.Extensions.Xml
{
    public static class UseFileExtensions
    {
        public static SyrxBuilder UseFile(this SyrxBuilder factory, string fileName, IConfigurationBuilder builder)
        {
            Throw<ArgumentNullException>(builder != null, $"ConfigurationBuilder is null! Check bootstrap.");
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(fileName), nameof(fileName));

            builder?.AddXmlFile(fileName);

            return factory;
        }
    }
}
