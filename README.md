# Syrx.Cockroach

This project provides Syrx support for CockroachDB. The overall experience of using [Syrx](https://github.com/Syrx/Syrx) remains the same. The only difference should be during dependency registration. 

## Table of Contents

- [Installation](#installation)
- [Extensions](#extensions)
- [Credits](#credits) 


## Installation 
> [!TIP]
> We recommend installing the Extensions package which includes extension methods for easier configuration. 

|Source|Command|
|--|--|
|.NET CLI|```dotnet add package Syrx.Cockroach.Extensions```
|Package Manager|```Install-Package Syrx.Cockroach.Extensions```
|Package Reference|```<PackageReference Include="Syrx.Cockroach.Extensions" Version="3.0.0" />```|
|Paket CLI|```paket add Syrx.Cockroach.Extensions --version 3.0.0```|

However, if you don't need the configuration options, you can install the standalone package via [nuget](https://www.nuget.org/packages/Syrx.Cockroach/).  

|Source|Command|
|--|--|
|.NET CLI|```dotnet add package Syrx.Cockroach```
|Package Manager|```Install-Package Syrx.Cockroach```
|Package Reference|```<PackageReference Include="Syrx.Cockroach" Version="3.0.0" />```|
|Paket CLI|```paket add Syrx.Cockroach --version 3.0.0```|


## Extensions
The `Syrx.Cockroach.Extensions` package provides dependency injection support via extension methods. 

```csharp
// add a using statement to the top of the file or in a global usings file.
using Syrx.Commanders.Databases.Connectors.Cockroach.Extensions;

public static IServiceCollection Install(this IServiceCollection services)
{
    return services
        .UseSyrx(factory => factory         // inject Syrx
        .UseCockroach(builder => builder        // using the CockroachDB implementation
            .AddConnectionString(/*...*/)   // add/resolve connection string details 
            .AddCommand(/*...*/)            // add/resolve commands for each type/method
            )
        );
}
```

## Credits
Syrx is inspired by and build on top of [Dapper](https://github.com/DapperLib/Dapper).    
CockroachDB support is provided by [Npgsql](https://github.com/npgsql/npgsql) since CockroachDB uses the PostgreSQL wire protocol.
