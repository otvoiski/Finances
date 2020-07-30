﻿using Finances.Data;
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

            valid &= transaction.Table<Installment>().Delete(x => x.ScheduleId == schedule.Id) > 0;

            return valid;
        }

        public IList<ScheduleInteface> GetAllSchedules()
        {
            var schedules = new List<ScheduleInteface>();
            foreach (var schedule in _sqlService.ToList<Schedule>())
            {
                schedules.Add(new ScheduleInteface
                {
                    Id = schedule.Id,
                    Description = schedule.Description,
                    IsActive = schedule.IsActive,
                    End = schedule.End == default(DateTime)
                    ? null
                    : schedule.End,
                    Installments = schedule.Installment,
                    Start = schedule.Start,
                    Price = schedule.Price,
                });
            }

            return schedules;
        }

        public bool Save(Schedule schedule)
        {
            if (schedule.Installment > 0 && schedule.Installment <= 60)
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
    }
}

public interface IScheduleFacade
{
    IList<ScheduleInteface> GetAllSchedules();

    bool Save(Schedule schedule);

    bool Delete(Schedule schedule);
}