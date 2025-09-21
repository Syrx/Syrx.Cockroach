namespace Syrx.Commanders.Databases.Builders
{
    public class Database
    {
        public string Name { get; }
        public IEnumerable<Table> Tables { get; }

        public Database(
            string name,
            IEnumerable<Table> tables)
        {
            Throw<ArgumentNullException>(!string.IsNullOrWhiteSpace(name), nameof(name));
            Throw<ArgumentNullException>(tables != null, nameof(tables));
            Throw<ArgumentOutOfRangeException>(tables!.Any(), nameof(tables));

            Name = name!;
            Tables = tables!;
        }
    }
}
