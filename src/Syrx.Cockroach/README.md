# Syrx.Cockroach

CockroachDB data access provider for the Syrx framework.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
  - [1. Configure Services](#1-configure-services)
  - [2. Create Repository](#2-create-repository)
  - [3. Configure Commands](#3-configure-commands)
- [CockroachDB-Specific Features](#cockroachdb-specific-features)
  - [JSON Support](#json-support)
  - [Array Support](#array-support)
  - [CRDB-Specific Functions](#crdb-specific-functions)
- [Configuration Patterns](#configuration-patterns)
  - [Basic Configuration](#basic-configuration)
  - [Multi-Region Setup](#multi-region-setup)
  - [Connection Pool Optimization](#connection-pool-optimization)
  - [SSL Configuration](#ssl-configuration)
- [Advanced Usage](#advanced-usage)
  - [Transaction Retry Logic](#transaction-retry-logic)
  - [UPSERT Operations](#upsert-operations)
  - [Geospatial Queries](#geospatial-queries)
- [Error Handling](#error-handling)
- [Performance Tips](#performance-tips)
  - [Connection Management](#connection-management)
  - [Query Optimization](#query-optimization)
  - [Data Types](#data-types)
- [Testing](#testing)
- [Related Packages](#related-packages)
- [Requirements](#requirements)
- [License](#license)
- [Credits](#credits)

## Overview

`Syrx.Cockroach` provides CockroachDB database connectivity for the Syrx data access framework. Built on top of Npgsql (utilizing CockroachDB's PostgreSQL wire protocol compatibility), this package offers seamless integration with CockroachDB while maintaining Syrx's core principles of control, performance, and flexibility.

## Features

- **CockroachDB Integration**: Native support for CockroachDB via PostgreSQL wire protocol
- **High Performance**: Leverages Npgsql's optimized connection handling and Dapper's speed
- **Distributed SQL**: Full support for CockroachDB's distributed architecture
- **JSON Support**: Built-in support for JSON and JSONB data types
- **Array Support**: Native PostgreSQL-style array type handling
- **ACID Transactions**: Strong consistency with automatic retry logic
- **Connection Pooling**: Efficient connection pool management
- **Multi-Region**: Support for CockroachDB's multi-region capabilities
- **Async/Await**: Complete async operation support
- **Multi-mapping**: Complex object composition from query results

## Installation

```bash
dotnet add package Syrx.Cockroach
```

**Package Manager**
```bash
Install-Package Syrx.Cockroach
```

**PackageReference**
```xml
<PackageReference Include="Syrx.Cockroach" Version="2.4.3" />
```

## Quick Start

### 1. Configure Services

```csharp
using Syrx.Cockroach.Extensions;

public void ConfigureServices(IServiceCollection services)
{
    services.UseSyrx(builder => builder
        .UseCockroach(cockroach => cockroach
            .AddConnectionString("Default", "Host=localhost;Port=26257;Database=mydb;Username=root;Password=admin;SSL Mode=Require")
            .AddCommand(types => types
                .ForType<UserRepository>(methods => methods
                    .ForMethod(nameof(UserRepository.GetAllUsersAsync), command => command
                        .UseConnectionAlias("Default")
                        .UseCommandText("SELECT id, name, email, created_at FROM users"))))));
}
```

### 2. Create Repository

```csharp
public class UserRepository
{
    private readonly ICommander<UserRepository> _commander;

    public UserRepository(ICommander<UserRepository> commander)
    {
        _commander = commander;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
        => await _commander.QueryAsync<User>();

    public async Task<User> GetUserByIdAsync(Guid id)
        => await _commander.QueryAsync<User>(new { id }).SingleOrDefaultAsync();

    public async Task<bool> CreateUserAsync(User user)
        => await _commander.ExecuteAsync(user) > 0;
}
```

### 3. Configure Commands

```csharp
services.UseSyrx(builder => builder
    .UseCockroach(cockroach => cockroach
        .AddConnectionString("Default", connectionString)
        .AddCommand(types => types
            .ForType<UserRepository>(methods => methods
                .ForMethod(nameof(UserRepository.GetUserByIdAsync), command => command
                    .UseConnectionAlias("Default")
                    .UseCommandText("SELECT id, name, email, created_at FROM users WHERE id = $1"))
                .ForMethod(nameof(UserRepository.CreateUserAsync), command => command
                    .UseConnectionAlias("Default")
                    .UseCommandText("INSERT INTO users (id, name, email) VALUES (gen_random_uuid(), $1, $2)"))))));
```

## CockroachDB-Specific Features

### JSON Support

CockroachDB's JSON capabilities are fully supported:

```csharp
public class ProductRepository
{
    private readonly ICommander<ProductRepository> _commander;

    public ProductRepository(ICommander<ProductRepository> commander)
    {
        _commander = commander;
    }

    // Query JSON data
    public async Task<IEnumerable<Product>> GetProductsWithAttributesAsync()
        => await _commander.QueryAsync<Product>();

    // Store JSON data  
    public async Task<bool> SaveProductAsync(Product product)
        => await _commander.ExecuteAsync(product) > 0;
}

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public JsonDocument Attributes { get; set; }  // CockroachDB JSON
    public string[] Tags { get; set; }            // CockroachDB Array
}
```

Configure JSON commands:
```csharp
.ForMethod(nameof(ProductRepository.GetProductsWithAttributesAsync), command => command
    .UseConnectionAlias("Default")
    .UseCommandText("SELECT id, name, attributes::json, tags FROM products"))
.ForMethod(nameof(ProductRepository.SaveProductAsync), command => command
    .UseConnectionAlias("Default")
    .UseCommandText("INSERT INTO products (id, name, attributes, tags) VALUES (gen_random_uuid(), $1, $2::jsonb, $3)"))
```

### Array Support

CockroachDB arrays are natively supported:

```csharp
public async Task<IEnumerable<User>> GetUsersByRolesAsync(string[] roles)
    => await _commander.QueryAsync<User>(new { roles });

// Command configuration
.UseCommandText("SELECT id, name, email FROM users WHERE roles && $1")
```

### CRDB-Specific Functions

```csharp
public class SessionRepository
{
    // Use CockroachDB's built-in functions
    public async Task<IEnumerable<Session>> GetActiveSessionsAsync()
        => await _commander.QueryAsync<Session>();
}

// Using CockroachDB-specific functions
.UseCommandText(@"
    SELECT 
        id,
        user_id,
        created_at,
        cluster_logical_timestamp() as version,
        crdb_internal.locality_value('region') as region
    FROM sessions 
    WHERE expires_at > now()")
```

## Configuration Patterns

### Basic Configuration

```csharp
services.UseSyrx(builder => builder
    .UseCockroach(cockroach => cockroach
        .AddConnectionString("Default", "Host=localhost;Port=26257;Database=myapp;Username=root;SSL Mode=Require")));
```

### Multi-Region Setup

```csharp
services.UseSyrx(builder => builder
    .UseCockroach(cockroach => cockroach
        .AddConnectionString("US-East", "Host=us-east.cockroachdb.cloud;Port=26257;Database=myapp;Username=app;Password=secret;SSL Mode=Require")
        .AddConnectionString("EU-West", "Host=eu-west.cockroachdb.cloud;Port=26257;Database=myapp;Username=app;Password=secret;SSL Mode=Require")
        .AddCommand(types => types
            .ForType<UserRepository>(methods => methods
                .ForMethod("GetLocalUsers", command => command
                    .UseConnectionAlias("US-East")))  // Route to closest region
            .ForType<ReportRepository>(methods => methods
                .ForMethod("GetGlobalReport", command => command
                    .UseConnectionAlias("EU-West"))))));  // Use specific region for compliance
```

### Connection Pool Optimization

```csharp
services.UseSyrx(builder => builder
    .UseCockroach(cockroach => cockroach
        .AddConnectionString("Optimized", 
            "Host=localhost;Port=26257;Database=myapp;Username=app;Password=secret;" +
            "MinPoolSize=5;MaxPoolSize=100;ConnectionLifeTime=300;" +
            "Timeout=30;CommandTimeout=60;SSL Mode=Require;")));
```

### SSL Configuration

```csharp
services.UseSyrx(builder => builder
    .UseCockroach(cockroach => cockroach
        .AddConnectionString("Secure", 
            "Host=secure.cockroachdb.cloud;Port=26257;Database=myapp;Username=app;Password=secret;" +
            "SSL Mode=Require;Root Certificate=ca.crt;" +
            "Client Certificate=client.crt;Client Certificate Key=client.key;")));
```

## Advanced Usage

### Transaction Retry Logic

```csharp
public async Task<bool> TransferFundsAsync(Guid fromAccount, Guid toAccount, decimal amount)
{
    // CockroachDB automatic retry logic
    return await _commander.ExecuteAsync(new { fromAccount, toAccount, amount }) > 0;
}

// Configuration with savepoint for retries
.ForMethod(nameof(AccountRepository.TransferFundsAsync), command => command
    .UseConnectionAlias("Default")
    .UseCommandText(@"
        BEGIN;
        SAVEPOINT transfer_sp;
        UPDATE accounts SET balance = balance - $3 WHERE id = $1;
        UPDATE accounts SET balance = balance + $3 WHERE id = $2;
        RELEASE SAVEPOINT transfer_sp;
        COMMIT;")
    .SetCommandTimeout(30))
```

### UPSERT Operations

```csharp
public async Task<bool> UpsertUserAsync(User user)
    => await _commander.ExecuteAsync(user) > 0;

// CockroachDB UPSERT syntax
.UseCommandText(@"
    INSERT INTO users (id, name, email, updated_at) 
    VALUES ($1, $2, $3, now()) 
    ON CONFLICT (id) 
    DO UPDATE SET 
        name = EXCLUDED.name,
        email = EXCLUDED.email,
        updated_at = now()")
```

### Geospatial Queries

```csharp
public async Task<IEnumerable<Store>> GetNearbyStoresAsync(double latitude, double longitude, double radiusKm)
    => await _commander.QueryAsync<Store>(new { latitude, longitude, radiusKm });

.UseCommandText(@"
    SELECT 
        id, 
        name, 
        location,
        ST_Distance(location, ST_MakePoint($2, $1)::geography) / 1000 as distance_km
    FROM stores 
    WHERE ST_DWithin(location, ST_MakePoint($2, $1)::geography, $3 * 1000)
    ORDER BY distance_km")
```

## Error Handling

CockroachDB-specific error handling:

```csharp
public async Task<User> CreateUserAsync(User user)
{
    try
    {
        var success = await _commander.ExecuteAsync(user) > 0;
        return success ? user : null;
    }
    catch (PostgresException ex) when (ex.SqlState == "40001") // Serialization failure
    {
        // CockroachDB transaction retry
        throw new TransactionRetryException("Transaction needs retry", ex);
    }
    catch (PostgresException ex) when (ex.SqlState == "23505") // Unique violation
    {
        throw new DuplicateUserException($"User with email {user.Email} already exists", ex);
    }
}
```

## Performance Tips

### Connection Management
- Use connection pooling for better performance
- Configure appropriate pool sizes for your load
- Consider region-aware connection strategies

### Query Optimization
- Use CockroachDB's query planner with EXPLAIN
- Leverage secondary indexes for complex queries
- Use appropriate partitioning strategies

### Data Types
- Use UUID for primary keys (CockroachDB default)
- Leverage JSONB for flexible document storage
- Use arrays instead of separate junction tables when appropriate

## Testing

Example test configuration:

```csharp
[Test]
public async Task Should_Retrieve_Users_From_CockroachDB()
{
    // Arrange
    var services = new ServiceCollection();
    services.UseSyrx(builder => builder
        .UseCockroach(cockroach => cockroach
            .AddConnectionString("Test", "Host=localhost;Port=26257;Database=testdb;Username=root;SSL Mode=Disable")
            .AddCommand(types => types
                .ForType<UserRepository>(methods => methods
                    .ForMethod(nameof(UserRepository.GetAllUsersAsync), command => command
                        .UseConnectionAlias("Test")
                        .UseCommandText("SELECT id, name, email FROM users"))))));

    var provider = services.BuildServiceProvider();
    var repository = provider.GetService<UserRepository>();

    // Act
    var users = await repository.GetAllUsersAsync();

    // Assert
    Assert.IsNotNull(users);
}
```

## Related Packages

- **[Syrx.Cockroach.Extensions](https://www.nuget.org/packages/Syrx.Cockroach.Extensions/)**: Dependency injection extensions
- **[Syrx.Commanders.Databases.Connectors.Cockroach](https://www.nuget.org/packages/Syrx.Commanders.Databases.Connectors.Cockroach/)**: Core CockroachDB connector
- **[Syrx](https://www.nuget.org/packages/Syrx/)**: Core Syrx framework
- **[Syrx.Commanders.Databases](https://www.nuget.org/packages/Syrx.Commanders.Databases/)**: Database command framework

## Requirements

- **.NET 8.0** or later
- **CockroachDB 22.1** or later (recommended)
- **Npgsql 8.0** or later

## License

This project is licensed under the [MIT License](https://github.com/Syrx/Syrx/blob/main/LICENSE).

## Credits

- Built on top of [Npgsql](https://github.com/npgsql/npgsql) - leveraging CockroachDB's PostgreSQL wire protocol compatibility
- Uses [Dapper](https://github.com/DapperLib/Dapper) for object mapping
- Inspired by CockroachDB's distributed SQL capabilities