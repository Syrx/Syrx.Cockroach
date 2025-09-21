namespace Syrx.Commanders.Databases.Builders
{
    public class DatabaseOptions
    {
        private string _name;
        private IDictionary<string, Table> _tables;

        public DatabaseOptions()
        {
            _name = string.Empty;
            _tables = new Dictionary<string, Table>();
        }

        public DatabaseOptions WithName(string name)
        {
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(name), nameof(name));
            _name = name;
            return this;
        }

        public DatabaseOptions AddTable(Action<TableOptions> factory)
        {
            var table = TableOptionsBuilderExtensions.Build(factory);
            return AddTable(table);
        }

        public DatabaseOptions AddTable(Table table)
        {
            Throw<ArgumentNullException>(table != null, nameof(table));
            _tables.Add(table!.Name, table);
            return this;
        }

        internal protected Database Build()
        {
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(_name), nameof(_name));
            Throw<ArgumentNullException>(_tables != null, nameof(_tables));
            Throw<ArgumentOutOfRangeException>(_tables!.Any(), nameof(_tables));

            return new Database(_name, _tables!.Values.ToList());
        }
    }
}
