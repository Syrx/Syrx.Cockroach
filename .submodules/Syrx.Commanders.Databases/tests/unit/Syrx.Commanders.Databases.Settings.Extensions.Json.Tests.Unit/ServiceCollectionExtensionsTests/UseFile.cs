using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Syrx.Extensions;
using static Xunit.Assert;

namespace Syrx.Commanders.Databases.Settings.Extensions.Json.Tests.Unit.ServiceCollectionExtensionsTests
{
    public class UseFile(JsonFileFixture fixture) : IClassFixture<JsonFileFixture>
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

        //[Fact]
        //public void IntegratesWithSettings()
        //{
        //    var fileName = $"syrx.settings.integration.{DateTime.UtcNow.ToString("yyMMddHH")}.json";
        //    var services = fixture.Services;
        //    //var builder = fixture.ConfigurationBuilder;
        //    var builder = new ConfigurationBuilder();

        //    // write file
        //    var settings = fixture.GetTestOptions<AddSyrxJsonFile>();
        //    var filename = fixture.WriteToFile(settings, fileName);

        //    var connector = new Mock<DbProviderFactory>().Object;

        //    //services.UseSyrx(a => a                
        //    //    .UseConnector(() => connector, b => b
        //    //        .AddCommand(c => c
        //    //            .ForType<AddSyrxJsonFile>(d => d
        //    //                .ForMethod(nameof(IntegratesWithSettings), e => e
        //    //                    .UseConnectionAlias("test-alias-2")
        //    //                    .UseCommandText("select 1/0;")))))
        //    //    //.UseFile(filename, builder)
        //    //    );

        //    services.UseSyrx(a => a.UseFile(filename, builder));

        //    var provider = services.BuildServiceProvider();

        //    var configuration = builder.Build();
        //    var resolved = configuration.Get<CommanderSettings>();


        //    //var commanderSettings = provider.GetRequiredService<Syrx.Commanders.Databases.Settings.ICommanderSettings>();
        //    //NotNull(commanderSettings);

        //    resolved.PrintAsJson();
        //    //commanderSettings.PrintAsJson();

        //    var method = resolved.Namespaces.Single().Types.Single().Commands.First().Key;
        //    Equal("Method1", method);
        //    method.PrintAsJson();

        //    var reader = provider.GetRequiredService<Syrx.Commanders.Databases.Settings.Readers.IDatabaseCommandReader>();
        //    var method1 = reader.GetCommand(typeof(AddSyrxJsonFile), "Method1");
        //    var method2 = reader.GetCommand(typeof(AddSyrxJsonFile), "Method2");



        //    NotNull(resolved);

        //    Equivalent(settings, resolved);
        //    //Equivalent(settings, commanderSettings);
        //    //Equivalent(resolved, commanderSettings);
        //}

    }
}
