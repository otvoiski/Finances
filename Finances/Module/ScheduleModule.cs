using Finances.Data;
using Finances.Model;
using System;

namespace Finances.Module
{
    public class ScheduleModule : IScheduleModule
    {
        public (Schedule schedule, string error) Validate(int id, int billId, string installment, DateTime? start, bool isActive)
        {
            if (billId < 0) return (null, "You can have find a bill before add one schedule.");
            if (!int.TryParse(installment, out int installmentNumber)) return (null, "Installment is not a number.");
            if (installmentNumber < 0) return (null, "Installment number cannot minor of zero.");

            return (new Schedule
            {
                Id = id,
                BillId = billId,
                Installment = installmentNumber,
                Start = start.GetValueOrDefault(),
                End = installmentNumber > 0
                    ? DateTime.Today.AddMonths(installmentNumber)
                    : default,
                IsActive = isActive
            }, "Ok!");
        }
    }

    public interface IScheduleModule
    {
        (Schedule schedule, string error) Validate(int id, int billId, string parcel, DateTime? start, bool isActive);
    }
}