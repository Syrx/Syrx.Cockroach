using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Syrx.Extensions;
using static Xunit.Assert;

namespace Syrx.Commanders.Databases.Settings.Extensions.Xml.Tests.Unit.ServiceCollectionExtensionsTests
{
    public class UseFile(XmlFileFixture fixture) : IClassFixture<XmlFileFixture>
    {

        [Fact]
        public void UseFileOverload()
        {
            var fileName = $"syrx.settings.file-overload.{DateTime.UtcNow.ToString("yyMMddHH")}.json";
            var services = fixture.Services;
            var builder = fixture.ConfigurationBuilder;

            // write file
            var settings = fixture.GetTestOptions<UseFile>();
            var filename = fixture.WriteToFile(settings, fileName);

            services.UseSyrx(a => a.UseFile(filename, builder));

            var provider = services.BuildServiceProvider();

            var configuration = builder.Build();
            var resolved = configuration.Get<CommanderSettings>();

            NotNull(resolved);

            Equivalent(settings, resolved);
        }




    }
}
