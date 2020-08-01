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
            using var transaction = _sqlService.BeginTransaction();
            try
            {
                var valid = true;

                valid &= RemoveBillAndInstallment(schedule, transaction);
                valid &= transaction.Delete(schedule) > 0;

                if (valid)
                    transaction.Commit();
                else
                    transaction.Rollback();

                return valid;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Close();
            }
        }

        private bool RemoveBillAndInstallment(Schedule schedule, SQLiteConnection transaction)
        {
            var valid = true;

            // check if have a bill forengh key in installment
            var installments = transaction
                .Table<Installment>()
                .Where(x => x.ScheduleId == schedule.Id).ToList();

            foreach (var installment in installments)
            {
                // if have, delete :)
                valid &= transaction.Table<Bill>().Delete(x => x.Id == installment.BillId) > 0;
            }

            if (installments.Count > 0)
                valid &= transaction.Table<Installment>().Delete(x => x.ScheduleId == schedule.Id) > 0;

            return valid;
        }

        public IList<Schedule> GetAllSchedules()
        {
            var schedules = new List<Schedule>();
            foreach (var schedule in _sqlService.ToList<Schedule>())
            {
                schedules.Add(new Schedule
                {
                    Id = schedule.Id,
                    Description = schedule.Description,
                    IsActive = schedule.IsActive,
                    End = schedule.End == default(DateTime)
                    ? null
                    : schedule.End,
                    Installment = schedule.Installment,
                    Start = schedule.Start,
                    Price = schedule.Price,
                });
            }

            return schedules;
        }

        public bool Save(Schedule schedule)
        {
            if (schedule.Installment >= 0 && schedule.Installment <= 60)
            {
                if (schedule.Id > 0)
                {
                    #region Edit Schedule

                    // start transaction
                    using var transaction = _sqlService.BeginTransaction();

                    try
                    {
                        var valid = true;
                        // update schedule
                        valid &= transaction.Update(schedule) > 0;

                        // remove all bills and installments
                        valid &= RemoveBillAndInstallment(schedule, transaction);

                        // add bill and schedule
                        valid &= AddBillAndSchedule(schedule, transaction);

                        if (valid)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                        return valid;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                    finally
                    {
                        transaction.Close();
                    }

                    #endregion Edit Schedule
                }
                else
                {
                    #region New Schedule

                    // start transaction
                    using var transaction = _sqlService.BeginTransaction();

                    try
                    {
                        var valid = true;

                        // insert schedule
                        valid &= transaction.Insert(schedule) > 0;

                        // add bill and schedule
                        valid &= AddBillAndSchedule(schedule, transaction);

                        if (valid)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                    finally
                    {
                        transaction.Close();
                    }

                    #endregion New Schedule
                }
            }
            else return false;
        }

        private bool AddBillAndSchedule(Schedule schedule, SQLiteConnection trasaction)
        {
            var valid = true;

            // scheduling
            for (int i = 0; i < schedule.Installment; i++)
            {
                // create bill
                var bill = new Bill
                {
                    Installment = $"{i + 1}/{schedule.Installment}", //add on bill at installment property the increment of variable i
                    Date = schedule.Start.AddMonths(i), // add i in months on variable Date.
                    Description = schedule.Description,
                    Price = schedule.Price,
                    IsPaid = false,
                    Type = schedule.End.HasValue
                        ? "C"
                        : "D"
                };

                // insert bill
                valid &= trasaction.Insert(bill) > 0;

                // insert in installments the schedule id + bill id
                valid &= trasaction.Insert(new Installment
                {
                    BillId = bill.Id,
                    ScheduleId = schedule.Id
                }) > 0;
            }

            return valid;
        }

        public bool LoadSchedule()
        {
            var valid = true;

            // Load invoices where the end date of the schedule is equal to null
            var schedules = _sqlService
                .ToList<Schedule>(x =>
                    x.End == null &&
                    x.IsActive == true);

            // verify on table bill if have the bill added.
            foreach (var schedule in schedules)
            {
                var bill = _sqlService
                    .ToList<Bill>(x =>
                        x.Description == schedule.Description &&
                        x.Type == "D")?
                    .FirstOrDefault();

                if (bill == null)
                {
                    valid &= _sqlService.Insert(new Bill
                    {
                        // add new bill
                        Date = new DateTime(
                            DateTime.Today.Year,
                            DateTime.Today.Month,
                            schedule.Start.Day),
                        Description = schedule.Description,
                        Price = schedule.Price,
                        Installment = null,
                        Type = "D",
                        IsPaid = false,
                        Payment = null
                    }) > 0;
                }
            }

            return valid;
        }
    }

    public interface IScheduleFacade
    {
        IList<Schedule> GetAllSchedules();

        bool Save(Schedule schedule);

        bool Delete(Schedule schedule);

        bool LoadSchedule();
    }
}