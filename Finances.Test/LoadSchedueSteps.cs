using Finances.Data;
using Finances.Facade;
using Finances.Service;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Finances.Test
{
    [Binding]
    public class LoadSchedueSteps
    {
        private ISqlService _sqlService;
        private IScheduleFacade _scheduleFacade;
        private IOperable<Schedule> _schedules;
        private bool _result;

        public LoadSchedueSteps()
        {
            var service = Dependencies.GetDependencies()
                .AddSingleton(x => Substitute.For<ISqlService>())
                .BuildServiceProvider();

            _sqlService = service.GetRequiredService<ISqlService>();
            _scheduleFacade = service.GetRequiredService<IScheduleFacade>();
        }

        [Given(@"list of (.*) existent schedules")]
        public void GivenListOfExistentSchedules(int n)
        {
            _schedules = Builder<Schedule>
                .CreateListOfSize(n)
                .Random(1)
                    .With(x => x.End = null)
                    .With(x => x.Id = 15668);
        }

        [Given(@"with today is ""(.*)""")]
        public void GivenWithTodayIs(string date)
        {
            var dateTime = DateTime.Parse(date);

            _schedules
                .With(x => x.Start == dateTime &&
                    x.Start.Month == DateTime.Today.Month &&
                    x.Start.Year == DateTime.Today.Year);
        }

        [Given(@"price on schedule of (.*)")]
        public void GivenPriceOnScheduleOf(Decimal price)
        {
            _schedules
                .With(x => x.Price = (double)price);
        }

        [Given(@"exist this on table bill")]
        public void GivenExistThisOnTableBill()
        {
            _sqlService
                .ToList<Bill>(default)
                .Returns(Builder<Bill>
                    .CreateListOfSize(6)
                    .Build());
        }

        [Given(@"not exist this on table bill")]
        public void GivenNotExistThisOnTableBill()
        {
            var x = _sqlService
                .ToList<Bill>(default)
                .ReturnsNullForAnyArgs();
        }

        [Given(@"not possible insert on database")]
        public void GivenNotPossibleInsertOnDatabase()
        {
            _sqlService
               .Insert(default)
               .ReturnsForAnyArgs(0);
        }

        [Given(@"possible insert on database")]
        public void GivenPossibleInsertOnDatabase()
        {
            _sqlService
               .Insert(default)
               .ReturnsForAnyArgs(1);
        }

        [When(@"the call scheduling list is not null")]
        public void WhenTheCallSchedulingListIsNotNull()
        {
            _sqlService
                .ToList<Schedule>(default)
                .ReturnsForAnyArgs(_schedules.Build());
        }

        [When(@"access load schedule")]
        public void WhenAccessLoadSchedule()
        {
            _result = _scheduleFacade.LoadSchedule();
        }

        [Then(@"the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            _result.ShouldBeTrue();
        }

        [Then(@"the result should be false")]
        public void ThenTheResultShouldBeFalse()
        {
            _result.ShouldBeFalse();
        }
    }
}