using Finances.Data;
using Finances.Model;
using System;

namespace Finances.Module
{
    public class ScheduleModule : IScheduleModule
    {
        public (Schedule schedule, string error) Validate(int scheduleId, string description, string price, string installment, DateTime? start, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(description)) return (null, "Description can not is empty");
            if (string.IsNullOrWhiteSpace(price)) return (null, "Value can not is empty");
            if (scheduleId < 0) return (null, "Invalid schedule id");
            if (!int.TryParse(installment, out int installmentNumber)) return (null, "Installment is not a number.");
            if (installmentNumber < 0) return (null, "Installment number cannot minor of zero.");
            if (!double.TryParse(price, out double priceNumber)) return (null, "Value is not a number");

            return (new Schedule
            {
                Description = description,
                Price = priceNumber,
                Installment = installmentNumber,
                Start = start.GetValueOrDefault(),
                End = installmentNumber > 0
                    ? start
                        .GetValueOrDefault()
                        .AddMonths(installmentNumber - 1)
                    : (DateTime?)null,
                IsActive = isActive
            }, "Ok!");
        }
    }

    public interface IScheduleModule
    {
        (Schedule schedule, string error) Validate(int scheduleId, string description, string price, string installment, DateTime? start, bool isActive);
    }
}