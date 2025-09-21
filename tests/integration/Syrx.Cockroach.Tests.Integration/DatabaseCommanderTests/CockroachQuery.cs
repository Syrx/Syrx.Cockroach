namespace Syrx.Cockroach.Tests.Integration.DatabaseCommanderTests
{
    [Collection(nameof(CockroachFixtureCollection))]
    public class CockroachQuery(CockroachFixture fixture) : Query(fixture) 
    {
        // Inherit all tests from base Query class
        // CockroachDB supports all PostgreSQL-compatible query operations
    }
}