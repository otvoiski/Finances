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
                .Build();
        }

        [Given(@"possible insert on database")]
        public void GivenPossibleInsertOnDatabase()
        {
            _sqlService
                .InsertOrReplace(_bill)
                .Returns(1);
        }

        [Given(@"have (.*) installments")]
        public void GivenHaveInstallments(int size)
        {
            //_bill.Installment = size;
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