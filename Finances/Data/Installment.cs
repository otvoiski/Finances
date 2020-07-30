using SQLite;

namespace Finances.Data
{
    public class Installment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int ScheduleId { get; set; }

        [Indexed]
        public int BillId { get; set; }
    }
}