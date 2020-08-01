using Finances.Facade;
using Finances.Model;
using Finances.Service;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using TechTalk.SpecFlow;

namespace Finances.Test
{
    [Binding]
    public class IsScheduleSteps
    {
        private readonly ISqlService _sqlService;
        private readonly IBillFacade _billFacade;
        private Bill _bill;
        private (bool IsSchedule, string Error) _result;

        public IsScheduleSteps()
        {
            var service = Dependencies.GetDependencies()
                .AddSingleton(x => Substitute.For<ISqlService>())
                .BuildServiceProvider();

            _sqlService = service.GetRequiredService<ISqlService>();
            _billFacade = service.GetRequiredService<IBillFacade>();
        }

        [Given(@"the bill")]
        public void GivenTheBill()
        {
            _bill = Builder<Bill>
                .CreateNew()
                .Build();
        }

        [Given(@"exists this description on schedule table")]
        public void GivenExistsThisDescriptionOnScheduleTable()
        {
            _sqlService
                .ToList<Schedule>(x => x.Description == _bill.Description)
                .ReturnsForAnyArgs(Builder<Schedule>
                    .CreateListOfSize(1)
                    .Build());
        }

        [Given(@"don't exists this description on schedule table")]
        public void GivenDonTExistsThisDescriptionOnScheduleTable()
        {
            _sqlService
                .ToList<Schedule>(x => x.Description == _bill.Description)
                .ReturnsNull();
        }

        [Given(@"don't exists this bill vinculed in installments")]
        public void GivenDonTExistsThisBillVinculedInInstallments()
        {
            _sqlService
                .ToList<Installment>(x => x.BillId == _bill.Id)
                .ReturnsNull();
        }

        [Given(@"exists this bill vinculed in installments")]
        public void GivenExistsThisBillVinculedInInstallments()
        {
            _sqlService
                .ToList<Installment>(x => x.BillId == _bill.Id)
                .ReturnsForAnyArgs(Builder<Installment>
                    .CreateListOfSize(1)
                    .TheFirst(1)
                    .With(x => x.BillId = _bill.Id)
                    .Build());
        }

        [Given(@"exists a after bills in bill table with different description")]
        public void GivenExistsAAfterBillsInBillTableWithDifferentDescription()
        {
            _sqlService
                .ToList<Bill>(x => x.Id == _bill.Id)
                .ReturnsForAnyArgs(Builder<Bill>
                    .CreateListOfSize(1)
                    .TheFirst(1)
                    .With(x => x.Id = _bill.Id)
                    .With(x => x.Description = "Different Description")
                    .Build());
        }

        [Given(@"exists a after bills in bill table")]
        public void GivenExistsAAfterBillsInBillTable()
        {
            _sqlService
                .ToList<Bill>(x => x.Id == _bill.Id)
                .ReturnsForAnyArgs(Builder<Bill>
                    .CreateListOfSize(1)
                    .TheFirst(1)
                    .With(x => x.Id = _bill.Id)
                    .With(x => x.Description = _bill.Description)
                    .Build());
        }

        [Given(@"don't exists a after bills in bill table")]
        public void GivenDonTExistsAAfterBillsInBillTable()
        {
            _sqlService
                .ToList<Bill>(x => x.Id == _bill.Id)
                .ReturnsNull();
        }

        [When(@"check is schedule")]
        public void WhenCheckIsSchedule()
        {
            _result = _billFacade
                .IsSchedule(_bill);
        }

        [When(@"check is schedule with delete flag")]
        public void WhenCheckIsScheduleWithDeleteFlag()
        {
            _result = _billFacade
                .IsSchedule(_bill, delete: true);
        }

        [Then(@"isSchedule should be true")]
        public void ThenIsScheduleShouldBeTrue()
        {
            _result.IsSchedule.ShouldBeTrue();
        }

        [Then(@"isSchedule should be false")]
        public void ThenIsScheduleShouldBeFalse()
        {
            _result.IsSchedule.ShouldBeFalse();
        }

        [Then(@"error should be null")]
        public void ThenErrorShouldBeNull()
        {
            _result.Error.ShouldBeNull();
        }

        [Then(@"error message is ""(.*)""")]
        public void ThenErrorMessageIs(string error)
        {
            _result.Error.ShouldBe(error);
        }
    }
}