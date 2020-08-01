using SQLite;

namespace Finances.Model
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