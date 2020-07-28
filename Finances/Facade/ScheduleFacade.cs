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

        public IList<Schedule> GetAllSchedules()
        {
            return _sqlService
                .ToList<Schedule>();
        }

        public bool Remove(Schedule schedule)
        {
            return _sqlService
                .Delete<Schedule>(x => x.Id == schedule.Id && x.BillId == schedule.BillId) == 1;
        }

        public bool Save(Schedule schedule)
        {
            return _sqlService
                .InsertOrReplace(schedule) == 1;
        }
    }

    public interface IScheduleFacade
    {
        IList<Schedule> GetAllSchedules();

        bool Remove(Schedule schedule);

        bool Save(Schedule schedule);
    }
}