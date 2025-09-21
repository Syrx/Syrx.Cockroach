namespace Syrx.Cockroach.Tests.Integration
{
    public class CockroachFixture : Fixture, IAsyncLifetime
    {
        public const string Alias = "cockroach-integration-tests";
        private readonly CockroachDbContainer _container;

        public CockroachFixture()
        {
            _container = new CockroachDbBuilder()
                .WithImage("cockroachdb/cockroach:v23.1.11")
                .WithUsername("root")
                .WithDatabase("defaultdb")
                .Build();
        }

        public async Task DisposeAsync()
        {
            if (_container != null)
            {
                await _container.DisposeAsync();
            }
        }

        public async Task InitializeAsync()
        {
            // Start the container first
            await _container.StartAsync();
            
            var connectionString = _container.GetConnectionString();

            // Use the Install method from base Fixture class
            Install(() => Installer.Install(Alias, connectionString));
            
            // Try database setup with simplified retry logic
            await RetryDatabaseSetup();

            // Set CockroachDB-specific assertion messages (similar to PostgreSQL since CockroachDB is PostgreSQL-compatible)
            AssertionMessages.Add<Execute>(nameof(Execute.SupportsTransactionRollback), "22003: value overflows numeric format");
            AssertionMessages.Add<Execute>(nameof(Execute.ExceptionsAreReturnedToCaller), "22012: division by zero");
            AssertionMessages.Add<Execute>(nameof(Execute.SupportsRollbackOnParameterlessCalls), "22012: division by zero");

            AssertionMessages.Add<ExecuteAsync>(nameof(ExecuteAsync.SupportsTransactionRollback), "22003: value overflows numeric format");
            AssertionMessages.Add<ExecuteAsync>(nameof(ExecuteAsync.ExceptionsAreReturnedToCaller), "22012: division by zero");
            AssertionMessages.Add<ExecuteAsync>(nameof(ExecuteAsync.SupportsRollbackOnParameterlessCalls), "22012: division by zero");

            AssertionMessages.Add<Query>(nameof(Query.ExceptionsAreReturnedToCaller), "22012: division by zero");
            AssertionMessages.Add<QueryAsync>(nameof(QueryAsync.ExceptionsAreReturnedToCaller), "22012: division by zero");
        }

        private async Task RetryDatabaseSetup()
        {
            const int maxRetries = 5;
            const int delayMs = 3000;
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    // Try to setup the database with a fresh service provider each time
                    var connectionString = _container.GetConnectionString();
                    Install(() => Installer.Install(Alias, connectionString));
                    
                    Installer.SetupDatabase(base.ResolveCommander<DatabaseBuilder>());
                    return; // Success
                }
                catch (Exception ex) when (i < maxRetries - 1)
                {
                    Console.WriteLine($"{ex.Message}");
                    // Log the error (if we had logging) and wait before retrying
                    await Task.Delay(delayMs * (i + 1)); // Exponential backoff
                }
            }
            
            // Final attempt without catching exceptions
            var finalConnectionString = _container.GetConnectionString();
            Install(() => Installer.Install(Alias, finalConnectionString));
            Installer.SetupDatabase(base.ResolveCommander<DatabaseBuilder>());
        }
    }
}