namespace Syrx.Cockroach.Extensions
{
    public static class SyrxBuilderExtensions
    {
        public static SyrxBuilder SetupCockroach(this SyrxBuilder builder, string alias, string connectionString)
        {
            return builder.UseCockroach(commanderBuilder => commanderBuilder
                .AddConnectionString(alias, connectionString));
        }
    }
}