namespace Syrx.Cockroach.Tests.Integration
{
    [CollectionDefinition(nameof(CockroachFixtureCollection))]
    public class CockroachFixtureCollection : ICollectionFixture<CockroachFixture> { }
}