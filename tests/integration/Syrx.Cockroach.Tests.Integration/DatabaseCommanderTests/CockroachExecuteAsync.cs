namespace Syrx.Cockroach.Tests.Integration.DatabaseCommanderTests
{
    [Collection(nameof(CockroachFixtureCollection))]
    public class CockroachExecuteAsync(CockroachFixture fixture) : ExecuteAsync(fixture) 
    {
        private readonly ICommander<Execute> _commander = fixture.ResolveCommander<Execute>();

        [Theory(Skip = "Not supported by CockroachDB")]
        [MemberData(nameof(TransactionScopeOptions))] // TransactionScopeOptions is taken from base Exeucte
        public override Task SupportsEnlistingInAmbientTransactions(TransactionScopeOption scopeOption)
        {
            return base.SupportsEnlistingInAmbientTransactions(scopeOption);
        }

        [Fact]
        public override async Task SupportsTransactionRollback()
        {
            var ex = await Assert.ThrowsAnyAsync<Exception>(() => _commander.ExecuteAsync<bool>());
            Assert.Contains("42601", ex.Message); // CockroachDB syntax error code
        }

        [Fact]
        public override async Task SupportsRollbackOnParameterlessCalls()
        {
            var ex = await Assert.ThrowsAnyAsync<Exception>(() => _commander.ExecuteAsync<bool>());
            Assert.Contains("25001", ex.Message); // CockroachDB error code for transaction already in progress
        }
    }
}