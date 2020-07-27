using SQLite;

namespace Finances.Model
{
    public class Parcel
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Indexed]
        public int Child { get; set; }

        [NotNull]
        public int Number { get; set; }
    }
}