namespace Syrx.Commanders.Databases.Connectors.Cockroach.Extensions.Tests.Unit.CockroachConnectorExtensionsTests
{
    public class UseCockroach
    {
        private IServiceCollection _services;

        public UseCockroach()
        {
            _services = new ServiceCollection();
        }

        [Fact]
        public void Successful()
        {
            _services.UseSyrx(a => a
                .UseCockroach(b => b
                    .AddCommand(c => c
                        .ForType<UseCockroach>(d => d
                            .ForMethod(nameof(Successful), e => e.UseCommandText("test-command").UseConnectionAlias("test-alias"))))));

            var provider = _services.BuildServiceProvider();
            var connector = provider.GetService<IDatabaseConnector>();
            IsType<CockroachDatabaseConnector>(connector);
        }
    }
}