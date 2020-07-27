using Finances.Model;
using Finances.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Finances.Facade
{
    public class BillFacade : IBillFacade
    {
        private readonly ISqlService _sqlService;

        public BillFacade(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public bool Insert(Bill bill)
        {
            using (var connection = _sqlService.Factory())
            {
                connection.Insert(bill);
            }

            return false;
        }

        public IList<Bill> GetAllBills()
        {
            using (var connection = _sqlService.Factory())
            {
                return connection.Table<Bill>().ToList();
            }
        }

        //public async Task<Wallet> LoadBalance()
        //{
        //    return await Task.FromResult(WalletSeed());

        //    static Wallet WalletSeed()
        //    {
        //        return new Wallet
        //        {
        //            Bills = new List<Bill>
        //            {
        //                new Bill
        //                {
        //                    Id = 15,
        //                    Date = new DateTime(2020,07,23),
        //                    Description = "Phone",
        //                    Parcel = 0,
        //                    Type = 'D',
        //                    Payment = null,
        //                    Value = 212.65,
        //                    IsPay = false
        //                }
        //            },
        //            Balance = 100.56
        //        };
        //    }
        //}

        public async Task<bool> Update(Bill bill)
        {
            //foreach (var bill in wallet.Bills)
            //{
            //    return await _sqlService.Save(bill);
            //}

            return false;
        }
    }

    public interface IBillFacade
    {
        //Task<Wallet> LoadBalance();

        IList<Bill> GetAllBills();

        //Task<bool> Update(Wallet wallet);
        bool Insert(Bill bill);
    }
}