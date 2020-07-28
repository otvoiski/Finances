using SQLite;

namespace Finances.Model
{
    public class Installment
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Indexed]
        public int Child { get; set; }

        [NotNull]
        public int Number { get; set; }
    }
}