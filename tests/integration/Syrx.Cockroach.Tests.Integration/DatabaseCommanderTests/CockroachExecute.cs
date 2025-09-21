namespace Syrx.Cockroach.Tests.Integration.DatabaseCommanderTests
{
    [Collection(nameof(CockroachFixtureCollection))]
    public class CockroachExecute(CockroachFixture fixture) : Execute(fixture) 
    {
        private readonly ICommander<Execute> _commander = fixture.ResolveCommander<Execute>();

        [Theory(Skip = "Not supported by CockroachDB")]
        [MemberData(nameof(TransactionScopeOptions))] // TransactionScopeOptions is taken from base Execute
        public override void SupportsEnlistingInAmbientTransactions(TransactionScopeOption scopeOption)
        {
            base.SupportsEnlistingInAmbientTransactions(scopeOption);
        }

        [Fact]
        public override void SupportsTransactionRollback()
        {
            var ex = Assert.ThrowsAny<Exception>(() => _commander.Execute<bool>());
            Assert.Contains("42601", ex.Message); // CockroachDB syntax error code
        }

        [Fact]
        public override void SupportsRollbackOnParameterlessCalls()
        {
            var ex = Assert.ThrowsAny<Exception>(() => _commander.Execute<bool>());
            Assert.Contains("25001", ex.Message); // CockroachDB error code for transaction already in progress
        }
    }
}