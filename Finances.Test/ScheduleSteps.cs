﻿using Finances.Model;
using Finances.Module;
using Finances.Test.Model;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TechTalk.SpecFlow;

namespace Finances.Test
{
    [Binding]
    public class ScheduleSteps
    {
        private DataSchedule _data;
        private readonly IScheduleModule _scheduleModule;
        private (Schedule schedule, string error) _result;

        public ScheduleSteps()
        {
            var service = Dependencies
                .GetDependencies()
                .BuildServiceProvider();

            _scheduleModule = service.GetRequiredService<IScheduleModule>();
        }

        [Given(@"have one schedule")]
        public void GivenHaveOneSchedule()
        {
            _data = Builder<DataSchedule>
                .CreateNew()
                .With(x => x.Installment = "0")
                .With(x => x.Price = "-1")
                .Build();
        }

        [Given(@"price is empty")]
        public void GivenPriceIsEmpty()
        {
            _data.Price = string.Empty;
        }

        [Given(@"schedule description is empty")]
        public void GivenScheduleDescriptionIsEmpty()
        {
            _data.Description = string.Empty;
        }

        [Given(@"schedule id is (.*)")]
        public void GivenScheduleIdIs(int zero)
        {
            _data.ScheduleId = zero;
        }

        [Given(@"installment is empty")]
        public void GivenInstallmentIsEmpty()
        {
            _data.Installment = string.Empty;
        }

        [Given(@"month is empty")]
        public void GivenMonthIsEmpty()
        {
            _data.Installment = string.Empty;
        }

        [Given(@"installment is not a number")]
        public void GivenInstallmentIsNotANumber()
        {
            _data.Installment = "test";
        }

        [When(@"you access validation")]
        public void WhenYouAccessValidation()
        {
            _result = _scheduleModule.Validate(_data.ScheduleId, _data.Description, _data.Price, _data.Installment, _data.Start, false);
        }

        [Then(@"the result should not be null")]
        public void ThenTheResultShouldNotBeNull()
        {
            _result.schedule.ShouldNotBeNull();
        }

        [Then(@"the result should be null")]
        public void ThenTheResultShouldBeNull()
        {
            _result.schedule.ShouldBeNull();
        }
    }
}