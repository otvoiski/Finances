using Finances.Model;
using Finances.Service;
using SQLite;
using System.Collections.Generic;

namespace Finances.Facade
{
    public class ScheduleFacade : IScheduleFacade
    {
        private readonly ISqlService _sqlService;

        public ScheduleFacade(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public List<Schedule> GetAllSchedules()
        {
            using SQLiteConnection db = _sqlService.Factory();
            return db.Table<Schedule>().ToList();
        }

        public bool Remove(Schedule schedule)
        {
            using SQLiteConnection db = _sqlService.Factory();
            return db.Table<Schedule>().Delete(x => x.Id == schedule.Id && x.BillId == schedule.BillId) > 0;
        }

        public bool Save(Schedule schedule)
        {
            using SQLiteConnection db = _sqlService.Factory();
            return db.InsertOrReplace(schedule) > 0;
        }
    }

    public interface IScheduleFacade
    {
        List<Schedule> GetAllSchedules();

        bool Remove(Schedule schedule);

        bool Save(Schedule schedule);
    }
}