using Finances.Model;
using Finances.Module;
using Finances.Test.Model;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TechTalk.SpecFlow;

namespace Finances.Test
{
    [Binding]
    public class BillSteps
    {
        private readonly IBillModule _billModule;
        private DataBill _data;
        private (Bill bill, string error) _result;

        public BillSteps()
        {
            var service = Dependencies.GetDependencies()
                .BuildServiceProvider();

            _billModule = service.GetRequiredService<IBillModule>();
        }

        [Given(@"have a bill")]
        public void GivenHaveABill()
        {
            _data = Builder<DataBill>
                .CreateNew()
                .With(x => x.Installment = "0")
                .With(x => x.Value = "0")
                .With(x => x.Type = "Debit Card")
                .Build();
        }

        [When(@"validate bill")]
        public void WhenValidateBill()
        {
            _result = _billModule.ValidateBill(_data.Date, _data.Description, _data.Installment, _data.Value, _data.Type, _data.IsPay);
        }

        [Given(@"description is empty")]
        public void GivenDescriptionIsEmpty()
        {
            _data.Description = null;
        }

        [Given(@"date is null")]
        public void GivenDateIsNull()
        {
            _data.Date = null;
        }

        [Given(@"installment is (.*)")]
        public void GivenInstallmentIs(int value)
        {
            _data.Installment = $"{value}";
        }

        [Given(@"installment is less than (.*)")]
        public void GivenInstallmentIsLessThan(int value)
        {
            _data.Installment = $"{value}";
        }

        [Given(@"value is empty")]
        public void GivenValueIsEmpty()
        {
            _data.Value = null;
        }

        [Given(@"type is empty")]
        public void GivenTypeIsEmpty()
        {
            _data.Type = null;
        }

        [Given(@"type is credit card")]
        public void GivenTypeIsCreditCard()
        {
            _data.Type = "Credit Card";
        }

        [Then(@"result should not be null;")]
        public void ThenResultShouldNotBeNull()
        {
            _result.bill.ShouldNotBeNull();
        }

        [Then(@"result should be null;")]
        public void ThenResultShouldBeNull()
        {
            _result.bill.ShouldBeNull();
        }
    }
}