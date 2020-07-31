using Finances.Data;
using Finances.Facade;
using Finances.Model;
using Finances.Service;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using TechTalk.SpecFlow;

namespace Finances.Test
{
    [Binding]
    public class BillFacadeSteps
    {
        private Bill _bill;
        private readonly ISqlService _sqlService;
        private readonly IBillFacade _billFacade;
        private bool _result;

        public BillFacadeSteps()
        {
            var service = Dependencies.GetDependencies()
                .AddSingleton(x => Substitute.For<ISqlService>())
                .BuildServiceProvider();

            _sqlService = service.GetRequiredService<ISqlService>();
            _billFacade = service.GetRequiredService<IBillFacade>();
        }

        [Given(@"a bill")]
        public void GivenABill()
        {
            _bill = Builder<Bill>
                .CreateNew()
                .With(x => x.Id = 0)
                .Build();
        }

        [Given(@"is paid")]
        public void GivenIsPaid()
        {
            _bill.IsPaid = true;
        }

        [Given(@"possible insert on bill database")]
        public void GivenPossibleInsertOnDatabase()
        {
            _sqlService
                .Insert(_bill)
                .ReturnsForAnyArgs(1);
        }

        [Given(@"not possible insert on bill database")]
        public void GivenNotPossibleInsertOnDatabase()
        {
            _sqlService
               .Insert(_bill)
               .ReturnsForAnyArgs(0);
        }

        [Given(@"have (.*) installments")]
        public void GivenHaveInstallments(int size)
        {
            _bill.Installment = size.ToString();
        }

        [Given(@"type is credit card")]
        public void GivenTypeIsCreditCard()
        {
            _bill.Type = "Credit Card";
        }

        [When(@"you save bill")]
        public void WhenYouSaveBill()
        {
            _result = _billFacade.Save(_bill);
        }

        [Then(@"result should be true")]
        public void ThenResultShouldBeTrue()
        {
            _result.ShouldBeTrue();
        }
    }
}