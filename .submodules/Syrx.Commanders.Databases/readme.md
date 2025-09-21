# Overview 
Syrx is intended to be a project that can access multiple different underlying data stores (not only RDBMSs). 
This repository hosts code that is common to all RDBMS implementations as well as extensions to make configuration easier. 

What follows is a brief overview of each of the projects within this repository.

## Table of Contents 
1. [Syrx.Commanders.Databases](#Syrx.Commanders.Databases)
2. [Syrx.Commanders.Databases.Connectors](#Syrx.Commanders.Databases.Connectors)
3. [Syrx.Commander.Databases.Settings](#Syrx.Commander.Databases.Settings)
4. [Syrx.Commander.Databases.Settings.Readers](#Syrx.Commander.Databases.Settings.Readers)


---

# Syrx.Commanders.Databases
This is the implementation of the `ICommander` and where Syrx uses `Dapper` extension methods for reading and writing to an RDBMS. There are multiple overloads for both reading (`Query`) and writing (`Execute`) from/to the database as well as asynchronous variants for the same.


### [CallerMemberName] 
Each method accepts an optional `string` parameter decorated with the [`CallerMemberNameAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute). Using this, and the fully qualified type name of the generic type used by the `ICommander<>` we can interrogate our settings to resolve to a single `CommandSetting` entry for the method to be executed. 

#### Example
For example, assuming you have a method called `RetrieveAll` in a repository exposing a `Country` type in the namespace of `MyProject.Repositories`. 

```csharp
namespace MyProject.Repositories
{
    public class CountryRepository(ICommander<CountryRepository> commander) 
    {
        public IEnumerable<Country> RetrieveAll() => commander.Query<Country>();    
    }    
}
```

In this example, we can see: 
* `CountryRepository` is passed as a generic type to the `ICommander<>` which is injected into the class. 
* Our `RetrieveAll` method has no parameters. 

When this method executes, the `DatabaseCommander` will already know that the fully qualified type for the method being executed is `MyProject.Repositories.CountryRepository` and, using the `[CallerMemberName]`, know that the method being executed is called `RetrieveAll`. 
This allows us to map the method ro be executed to a specific SQL query. 

Structurally, you could think of it as being part of a hierarchy:

```
MyProject.Repositories                      // <-- the namespace
    CountryRepository                       // <-- the type
        RetrieveAll                         // <-- the method
            select * from [dbo].[country]   // <-- the SQL
```

#### Overloads
We can support overloads of the same method by passing in the `method:` argument and adding this overload to our configuration. 

```csharp
namespace MyProject.Repositories
{
    public class CountryRepository(ICommander<CountryRepository> commander) 
    {
        public IEnumerable<Country> RetrieveAll() => commander.Query<Country>();

        public IEnumerable<Country> RetrieveAll(string language) => commander.Query<Country>(new { language }, method:"RetrieveAllByLanguage");
    }    
}
```

Structurally, you could think of it as being part of a hierarchy:

```
MyProject.Repositories                                                      // <-- the namespace
    CountryRepository                                                       // <-- the type
        RetrieveAll                                                         // <-- the method
            select * from [dbo].[country]                                   // <-- the SQL
        RetrieveAllByLanguage                                               // <-- the overload
            select * from [dbo].[country] where [language] = @language;     // <-- the overload SQL
```

---

# Syrx.Commanders.Databases.Connectors
Establishes a connection to a database using the [`IDbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.idbconnection) construct.

There's only one class in this project - `DatabaseConnector`. This class inherits from the `IDatabaseConnector` which itself inherits from the `IConnector<>` in the `Syrx.Connectors` project.

### Func\<DbProviderFactory\>
This class accepts an instance of the `ICommanderSettings` and a `Func<DbProvider>` delegate. This delegate is responsible for creating the connection. The `ICommanderSettings` instance is passed in solely to retrieve the connection string from the aliased `ConnectionStringSetting`. 

This class, and specifically this delegate, are the base type for supporting all other RDBMS implementations. 

#### Inheritance
* `DatabaseConnector`
	- `IDatabaseConnector`
		- `IConnector<>`


### Syrx.Commanders.Databases.Connectors.Extensions
The primary purpose of this project is to provide syntactic sugar extension methods.
There are extension methods against the `SyrxBuilder` as well as `IServiceCollection`.

The `UseConnector` method can be used to inject any `Func<DbProviderFactory>` delegate. This is useful is you need to support an RDBMS that isn't already natively supported by Syrx. As long as the RDBMS client library exposes a `DbProviderFactory` it can be supported by Syrx. 

---

# Syrx.Commander.Databases.Settings
This project is one of the main reason Syrx exists - it's the library which separates the SQL from the .NET code. This is achieved with the `CommandSetting` type. The properties of this type hold all the necessary information executing a SQL command against an RDBMS. It has very similar properties to the `CommandDefinition` used by `Dapper` - this is deliberate as Syrx uses `Dapper` to execute commands against an RDBMS. Syrx is not itself a micro-orm but a wrapper over `Dapper`.

As mentioned elsewhere, Syrx uses a unqiue configuration in that it mimics the structure of your code. This is achieved by using the following structure: 

* `CommanderSettings`
	* `NamespaceSettings`
		* `TypeSettings` 
			* `CommandSetting`

### Syrx.Commander.Databases.Settings.Extensions
Hosts the builders and extension methods for creating and populating settings. 

This is currently the preferred method for configuring Syrx.

### Syrx.Commander.Databases.Settings.Extensions.Json
Provides support for configuring Syrx via JSON file. 

**NOTE:** 
* In future, the JSON and XML extensions will be folded into the Readers namespace. 

### Syrx.Commander.Databases.Settings.Extensions.Xml
Provides support for configuring Syrx via XML file. 

**NOTE:** 
* In future, the JSON and XML extensions will be folded into the Readers namespace. 


---

# Syrx.Commander.Databases.Settings.Readers
Readers are used to bridge the `DatabaseCommander` to the `CommanderSettings`. It's a really simple type that you're unlikely to have to interact with directly. 

In future, the JSON and XML extensions will be folded into the Readers namespace. 

### Syrx.Commander.Databases.Settings.Readers.Extensions
Provides an extension point to add an `IDatabaseCommandReader` instance to an `IServiceCollection`

--- 