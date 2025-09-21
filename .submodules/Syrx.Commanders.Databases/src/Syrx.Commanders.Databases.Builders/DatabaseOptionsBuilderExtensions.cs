namespace Syrx.Commanders.Databases.Builders
{
    public static class DatabaseOptionsBuilderExtensions
    {
        public static Database Build(Action<DatabaseOptions> builder)
        {
            var options = new DatabaseOptions();
            builder(options);
            return options.Build();
        }
    }
}
