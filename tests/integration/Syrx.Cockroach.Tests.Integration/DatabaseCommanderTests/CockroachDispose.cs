namespace Syrx.Cockroach.Tests.Integration.DatabaseCommanderTests
{
    [Collection(nameof(CockroachFixtureCollection))]
    public class CockroachDispose(CockroachFixture fixture) : Dispose(fixture) 
    {
        // Inherit all tests from base Dispose class
        // CockroachDB supports proper resource disposal
    }
}