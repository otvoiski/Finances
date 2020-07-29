using Finances.Data;
using Finances.Model;
using Finances.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finances.Facade
{
    public class ScheduleFacade : IScheduleFacade
    {
        private readonly ISqlService _sqlService;

        public ScheduleFacade(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public bool Delete(Schedule schedule)
        {
            return _sqlService
                .Delete(schedule) > 0;
        }

        public IList<ScheduleInteface> GetAllSchedules()
        {
            var schedules = new List<ScheduleInteface>();
            foreach (var schedule in _sqlService.ToList<Schedule>())
            {
                var bill = _sqlService.ToList<Bill>(x => x.Id == schedule.BillId).FirstOrDefault();

                if (bill != null)
                {
                    schedules.Add(new ScheduleInteface
                    {
                        Id = schedule.Id,
                        BillId = bill.Id,
                        Description = bill.Description,
                        IsActive = schedule.IsActive,
                        End = schedule.End == default(DateTime)
                        ? null
                        : schedule.End,
                        Installments = schedule.Installment,
                        Start = schedule.Start,
                        Price = bill.Price
                    });
                }
            }
            return schedules;
        }

        public bool Save(Schedule schedule)
        {
            return _sqlService
                .Update(schedule) == 1;
        }
    }

    public interface IScheduleFacade
    {
        IList<ScheduleInteface> GetAllSchedules();

        bool Save(Schedule schedule);

        bool Delete(Schedule schedule);
    }
}