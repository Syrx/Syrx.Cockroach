namespace Syrx.Cockroach.Tests.Integration
{
    public static class CockroachSetupExtensions
    {
        public static SyrxBuilder SetupCockroach(this SyrxBuilder builder, string alias, string connectionString)
        {
            return builder.UseCockroach(b => b
                .AddConnectionString(alias, connectionString)
                .AddSetupBuilderOptions(alias)
                .AddQueryMultimap(alias)
                .AddQueryMultiple(alias)
                .AddExecute(alias));
        }

        public static CommanderSettingsBuilder AddSetupBuilderOptions(this CommanderSettingsBuilder builder, string alias)
        {
            return builder.AddCommand(
                a => a.ForType<DatabaseBuilder>(
                    b => b
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateDatabase), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateDatabase))
                    .ForMethod(
                        nameof(DatabaseBuilder.DropTableCreatorProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.DropTableCreatorProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateTableCreatorProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateTableCreatorProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateTable), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateTable))
                    .ForMethod(
                        nameof(DatabaseBuilder.DropIdentityTesterProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.DropIdentityTesterProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateIdentityTesterProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateIdentityTesterProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.DropBulkInsertProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.DropBulkInsertProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateBulkInsertProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateBulkInsertProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.DropBulkInsertAndReturnProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.DropBulkInsertAndReturnProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateBulkInsertAndReturnProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateBulkInsertAndReturnProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.DropTableClearingProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.DropTableClearingProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.CreateTableClearingProcedure), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.CreateTableClearingProcedure))
                    .ForMethod(
                        nameof(DatabaseBuilder.ClearTable), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.ClearTable))
                    .ForMethod(
                        nameof(DatabaseBuilder.Populate), c => c
                        .UseConnectionAlias(alias)
                        .UseCommandText(CockroachCommandStrings.Setup.Populate))
                    ));
        }

        public static CommanderSettingsBuilder AddExecute(this CommanderSettingsBuilder builder, string alias)
        {
            return builder.AddCommand(
                b => b.ForType<Execute>(c => c
                .ForMethod(
                    nameof(Execute.ExceptionsAreReturnedToCaller), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.ExceptionsAreReturnedToCaller))
                .ForMethod(
                    nameof(Execute.SupportParameterlessCalls), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportParameterlessCalls))
                .ForMethod(
                    $"{nameof(Execute.SupportsRollbackOnParameterlessCalls)}.Count", d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportsRollbackOnParameterlessCallsCount))
                .ForMethod(
                    nameof(Execute.SupportsRollbackOnParameterlessCalls), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportsRollbackOnParameterlessCalls))
                .ForMethod(
                    nameof(Execute.SupportsTransactionRollback), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportsTransactionRollback))
                .ForMethod(
                    $"{nameof(Execute.SupportsTransactionRollback)}.Count", d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportsTransactionRollbackCount))
                .ForMethod(
                    nameof(Execute.SupportsSuppressedDistributedTransactions), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportsSuppressedDistributedTransactions))
                .ForMethod(
                    nameof(Execute.SupportsEnlistingInAmbientTransactions), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SupportsEnlistingInAmbientTransactions))
                .ForMethod(
                    nameof(Execute.Successful), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.Successful))
                .ForMethod(
                    nameof(Execute.SuccessfullyWithResponse), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SuccessfullyWithResponse))
                .ForMethod(
                    $"{nameof(Execute.SuccessfullyWithResponse)}.Response", d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SuccessfullyWithResponseResponse))
                .ForMethod(
                    nameof(Execute.SingleType), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Execute.SingleType))
                ));
        }

        public static CommanderSettingsBuilder AddQueryMultimap(this CommanderSettingsBuilder builder, string alias)
        {
            return builder.AddCommand(
                    b => b.ForType<Query>(
                        c => c
                        .ForMethod(
                            nameof(Query.ExceptionsAreReturnedToCaller), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.ExceptionsAreReturnedToCaller))
                        .ForMethod(
                            nameof(Query.SingleType), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.SingleType))
                        .ForMethod(
                            nameof(Query.SingleTypeWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.SingleTypeWithParameters))
                        .ForMethod(
                            nameof(Query.TwoTypes), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.TwoTypes))
                        .ForMethod(
                            nameof(Query.TwoTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.TwoTypesWithParameters))
                        .ForMethod(
                            nameof(Query.ThreeTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.ThreeTypesWithParameters))
                        .ForMethod(
                            nameof(Query.FourTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.FourTypesWithParameters))
                        .ForMethod(
                            nameof(Query.FiveTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.FiveTypesWithParameters))
                        .ForMethod(
                            nameof(Query.SixTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.SixTypesWithParameters))
                        .ForMethod(
                            nameof(Query.SevenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.SevenTypesWithParameters))
                        .ForMethod(
                            nameof(Query.EightTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.EightTypesWithParameters))
                        .ForMethod(
                            nameof(Query.NineTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.NineTypesWithParameters))
                        .ForMethod(
                            nameof(Query.TenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.TenTypesWithParameters))
                        .ForMethod(
                            nameof(Query.ElevenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.ElevenTypesWithParameters))
                        .ForMethod(
                            nameof(Query.TwelveTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.TwelveTypesWithParameters))
                        .ForMethod(
                            nameof(Query.ThirteenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.ThirteenTypesWithParameters))
                        .ForMethod(
                            nameof(Query.FourteenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.FourteenTypesWithParameters))
                        .ForMethod(
                            nameof(Query.FifteenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.FifteenTypesWithParameters))
                        .ForMethod(
                            nameof(Query.SixteenTypesWithParameters), d => d
                            .UseConnectionAlias(alias)
                            .UseCommandText(CockroachCommandStrings.Query.Multimap.SixteenTypesWithParameters))));

        }

        public static CommanderSettingsBuilder AddQueryMultiple(this CommanderSettingsBuilder builder, string alias)
        {
            return builder.AddCommand(
                b => b.ForType<Query>(c => c
                .ForMethod(
                    nameof(Query.OneTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.OneTypeMultiple))
                .ForMethod(
                    nameof(Query.TwoTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.TwoTypeMultiple))
                .ForMethod(
                    nameof(Query.ThreeTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.ThreeTypeMultiple))
                .ForMethod(
                    nameof(Query.FourTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.FourTypeMultiple))
                .ForMethod(
                    nameof(Query.FiveTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.FiveTypeMultiple))
                .ForMethod(
                    nameof(Query.SixTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.SixTypeMultiple))
                .ForMethod(
                    nameof(Query.SevenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.SevenTypeMultiple))
                .ForMethod(
                    nameof(Query.EightTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.EightTypeMultiple))
                .ForMethod(
                    nameof(Query.NineTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.NineTypeMultiple))
                .ForMethod(
                    nameof(Query.TenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.TenTypeMultiple))
                .ForMethod(
                    nameof(Query.ElevenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.ElevenTypeMultiple))
                .ForMethod(
                    nameof(Query.TwelveTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.TwelveTypeMultiple))
                .ForMethod(
                    nameof(Query.ThirteenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.ThirteenTypeMultiple))
                .ForMethod(
                    nameof(Query.FourteenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.FourteenTypeMultiple))
                .ForMethod(
                    nameof(Query.FifteenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.FifteenTypeMultiple))
                .ForMethod(
                    nameof(Query.SixteenTypeMultiple), d => d
                    .UseConnectionAlias(alias)
                    .UseCommandText(CockroachCommandStrings.Query.Multiple.SixteenTypeMultiple))));
        }
    }
}