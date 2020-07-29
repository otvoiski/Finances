using SQLite;

namespace Finances.Data
{
    public class Installment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        [Indexed]
        public int BillId { get; set; }
    }
}