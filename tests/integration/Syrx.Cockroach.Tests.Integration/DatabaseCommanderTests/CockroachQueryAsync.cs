namespace Syrx.Cockroach.Tests.Integration.DatabaseCommanderTests
{
    [Collection(nameof(CockroachFixtureCollection))]
    public class CockroachQueryAsync(CockroachFixture fixture) : QueryAsync(fixture) 
    {
        // Inherit all tests from base QueryAsync class
        // CockroachDB supports all PostgreSQL-compatible async query operations
    }
}