namespace Syrx.Cockroach.Tests.Integration
{
    public static class CockroachCommandStrings
    {
        public const string Alias = "Syrx.Cockroach";

        public static class Setup
        {
            public const string CreateDatabase = @"CREATE DATABASE IF NOT EXISTS Syrx;";

            public const string DropTableCreatorProcedure = @"SELECT 1 -- No-op: CockroachDB uses direct SQL instead of procedures";

            public const string CreateTableCreatorProcedure = @"SELECT 1 -- No-op: CockroachDB uses direct SQL instead of procedures";

            public const string CreateTable = @"
DROP TABLE IF EXISTS poco;
CREATE SEQUENCE IF NOT EXISTS poco_id_seq AS INT4 START 1;
CREATE TABLE poco (
    id INT4 PRIMARY KEY DEFAULT nextval('poco_id_seq'), 
    name VARCHAR(50), 
    value DECIMAL(18,2), 
    modified TIMESTAMP
);

DROP TABLE IF EXISTS identity_tester;
CREATE SEQUENCE IF NOT EXISTS identity_tester_id_seq AS INT4 START 1;
CREATE TABLE identity_tester (
    id INT4 PRIMARY KEY DEFAULT nextval('identity_tester_id_seq'),
    name VARCHAR(50),
    value DECIMAL(18,2),
    modified TIMESTAMP
);

DROP TABLE IF EXISTS bulk_insert;
CREATE SEQUENCE IF NOT EXISTS bulk_insert_id_seq AS INT4 START 1;
CREATE TABLE bulk_insert (
    id INT4 PRIMARY KEY DEFAULT nextval('bulk_insert_id_seq'),
    name VARCHAR(50),
    value DECIMAL(18,2),
    modified TIMESTAMP
);

DROP TABLE IF EXISTS distributed_transaction;
CREATE SEQUENCE IF NOT EXISTS distributed_transaction_id_seq AS INT4 START 1;
CREATE TABLE distributed_transaction (
    id INT4 PRIMARY KEY DEFAULT nextval('distributed_transaction_id_seq'),
    name VARCHAR(50),
    value DECIMAL(18,2),
    modified TIMESTAMP
);

DROP TABLE IF EXISTS writes;
CREATE SEQUENCE IF NOT EXISTS writes_id_seq AS INT4 START 1;
CREATE TABLE writes (
    id INT4 PRIMARY KEY DEFAULT nextval('writes_id_seq'),
    name VARCHAR(50),
    value DECIMAL(18,2),
    modified TIMESTAMP
);";

            public const string DropIdentityTesterProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string CreateIdentityTesterProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string DropBulkInsertProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string CreateBulkInsertProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string DropBulkInsertAndReturnProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string CreateBulkInsertAndReturnProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string DropTableClearingProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string CreateTableClearingProcedure = @"SELECT 1 -- No-op: CockroachDB doesn't use stored procedures for this";

            public const string ClearTable = @"TRUNCATE TABLE poco;";

            public const string Populate = @"INSERT INTO poco(name, value, modified) VALUES (@Name, @Value, @Modified);";
        }

        public static class Query
        {
            public static class Multimap
            {
                public const string ExceptionsAreReturnedToCaller = @"SELECT 1/0;";

                public const string SingleType = @"SELECT id AS ""Id"",
       name AS ""Name"",
       value AS ""Value"",
       modified AS ""Modified""
FROM poco;";

                public const string SingleTypeWithParameters = @"SELECT id AS ""Id"",
       name AS ""Name"",
       value AS ""Value"",
       modified AS ""Modified""
FROM poco
WHERE id = @Id;";

                public const string TwoTypes = @"SELECT a.id,
       a.name AS ""Name"",
       a.value AS ""Value"",
       a.modified AS ""Modified"",
       b.id AS ""Id"",
       b.name,
       b.value,
       b.modified
FROM poco a
JOIN (
    SELECT id,
           name,
           value,
           modified
    FROM poco
) b ON b.id = (a.id + 10);";

                public const string TwoTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string ThreeTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string FourTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string FiveTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string SixTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string SevenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string EightTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string NineTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string TenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string ElevenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
JOIN data eleven ON eleven.""Id"" = (ten.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string TwelveTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
JOIN data eleven ON eleven.""Id"" = (ten.""Id"" + 1)
JOIN data twelve ON twelve.""Id"" = (eleven.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string ThirteenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
JOIN data eleven ON eleven.""Id"" = (ten.""Id"" + 1)
JOIN data twelve ON twelve.""Id"" = (eleven.""Id"" + 1)
JOIN data thirteen ON thirteen.""Id"" = (twelve.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string FourteenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
JOIN data eleven ON eleven.""Id"" = (ten.""Id"" + 1)
JOIN data twelve ON twelve.""Id"" = (eleven.""Id"" + 1)
JOIN data thirteen ON thirteen.""Id"" = (twelve.""Id"" + 1)
JOIN data fourteen ON fourteen.""Id"" = (thirteen.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string FifteenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
JOIN data eleven ON eleven.""Id"" = (ten.""Id"" + 1)
JOIN data twelve ON twelve.""Id"" = (eleven.""Id"" + 1)
JOIN data thirteen ON thirteen.""Id"" = (twelve.""Id"" + 1)
JOIN data fourteen ON fourteen.""Id"" = (thirteen.""Id"" + 1)
JOIN data fifteen ON fifteen.""Id"" = (fourteen.""Id"" + 1)
WHERE one.""Id"" = @Id;";

                public const string SixteenTypesWithParameters = @"WITH data AS (
    SELECT id AS ""Id"",
           name AS ""Name"",
           value AS ""Value"",
           modified AS ""Modified""
    FROM poco
)
SELECT *
FROM data one
JOIN data two ON two.""Id"" = (one.""Id"" + 1)
JOIN data three ON three.""Id"" = (two.""Id"" + 1)
JOIN data four ON four.""Id"" = (three.""Id"" + 1)
JOIN data five ON five.""Id"" = (four.""Id"" + 1)
JOIN data six ON six.""Id"" = (five.""Id"" + 1)
JOIN data seven ON seven.""Id"" = (six.""Id"" + 1)
JOIN data eight ON eight.""Id"" = (seven.""Id"" + 1)
JOIN data nine ON nine.""Id"" = (eight.""Id"" + 1)
JOIN data ten ON ten.""Id"" = (nine.""Id"" + 1)
JOIN data eleven ON eleven.""Id"" = (ten.""Id"" + 1)
JOIN data twelve ON twelve.""Id"" = (eleven.""Id"" + 1)
JOIN data thirteen ON thirteen.""Id"" = (twelve.""Id"" + 1)
JOIN data fourteen ON fourteen.""Id"" = (thirteen.""Id"" + 1)
JOIN data fifteen ON fifteen.""Id"" = (fourteen.""Id"" + 1)
JOIN data sixteen ON sixteen.""Id"" = (fifteen.""Id"" + 1)
WHERE one.""Id"" = @Id;";
            }

            public static class Multiple
            {
                public const string OneTypeMultiple = @"select * from poco where id < 2;";

                public const string TwoTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;";

                public const string ThreeTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;";

                public const string FourTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;";

                public const string FiveTypeMultiple = @"select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;";

                public const string SixTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;";

                public const string SevenTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;";

                public const string EightTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;";

                public const string NineTypeMultiple = @"select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;";

                public const string TenTypeMultiple = @"select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;";

                public const string ElevenTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;
select * from poco where id < 12;";

                public const string TwelveTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;
select * from poco where id < 12;
select * from poco where id < 13;";

                public const string ThirteenTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;
select * from poco where id < 12;
select * from poco where id < 13;
select * from poco where id < 14;";

                public const string FourteenTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;
select * from poco where id < 12;
select * from poco where id < 13;
select * from poco where id < 14;
select * from poco where id < 15;";

                public const string FifteenTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;
select * from poco where id < 12;
select * from poco where id < 13;
select * from poco where id < 14;
select * from poco where id < 15;
select * from poco where id < 16;";

                public const string SixteenTypeMultiple = @"
select * from poco where id < 2;
select * from poco where id < 3;
select * from poco where id < 4;
select * from poco where id < 5;
select * from poco where id < 6;
select * from poco where id < 7;
select * from poco where id < 8;
select * from poco where id < 9;
select * from poco where id < 10;
select * from poco where id < 11;
select * from poco where id < 12;
select * from poco where id < 13;
select * from poco where id < 14;
select * from poco where id < 15;
select * from poco where id < 16;
select * from poco where id < 17;";
            }
        }

        public static class Execute
        {
            public const string ExceptionsAreReturnedToCaller = @"SELECT 1/0;";

            public const string SupportParameterlessCalls = @"CREATE TABLE IF NOT EXISTS test_table_temp (value INT4); 
INSERT INTO test_table_temp (value) VALUES (1); 
DROP TABLE test_table_temp;";

            public const string SupportsRollbackOnParameterlessCallsCount = @"SELECT count(1) AS result FROM poco;";

            public const string SupportsRollbackOnParameterlessCalls = @"BEGIN;
DELETE FROM poco;
-- This will cause an error and rollback in a transaction block
SELECT 1 / 0;
ROLLBACK;";

            public const string SupportsSuppressedDistributedTransactions = @"INSERT INTO poco (name, value, modified)
VALUES (@Name, @Value, @Modified);";

            public const string SupportsTransactionRollbackCount = @"SELECT * FROM poco WHERE name = @Name;";

            public const string SupportsTransactionRollback = @"INSERT INTO poco (name, value)
VALUES (@Name, @Value * POWER(@Value, @Value));";

            public const string SupportsEnlistingInAmbientTransactions = @"
INSERT INTO distributed_transaction (name, value, modified)
VALUES (@Name, @Value, @Modified);";
            
            public const string SuccessfullyWithResponse = @"
INSERT INTO poco (name, value, modified)
VALUES (@Name, @Value, @Modified);";
            
            public const string SuccessfullyWithResponseResponse = @"SELECT id, name, value, modified FROM poco WHERE name = @Name;";

            public const string Successful = @"
INSERT INTO poco (name, value, modified)
VALUES (@Name, @Value, @Modified);";

            public const string SingleType = @"
INSERT INTO poco (name, value, modified)
VALUES (@Name, @Value, @Modified);";
        }

        public static class Dispose
        {
            public const string Successfully = @"SELECT floor(random() * 100)::int;";
        }
    }
}
