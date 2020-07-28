﻿using Finances.Facade;
using Finances.Model;
using Finances.Module;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Finances
{
    /// <summary>
    /// Interaction logic for AddBill.xaml
    /// </summary>
    public partial class BillManager : Window, IBillManager
    {
        private readonly IBillFacade _billFacade;
        private readonly IBillModule _billModule;

        private int _billId = 0;

        public BillManager(IBillFacade billFacade, IBillModule billModule)
        {
            _billFacade = billFacade;
            _billModule = billModule;
        }

        public BillManager Factory()
        {
            InitializeComponent();

            clickBotton.Content = "Add bill";

            date.SelectedDate = DateTime.Now;
            description.Text = "";
            value.Text = "-1";
            type.Text = "Debit Card";
            isPay.IsChecked = false;

            return this;
        }

        public BillManager Factory(Bill bill)
        {
            InitializeComponent();

            _billId = bill.Id;

            date.SelectedDate = bill.Date;
            description.Text = bill.Description;
            //value.Text = bill.Value.ToString();
            type.Text = bill.Type == "D"
                ? "Debit Card"
                : "Credit Card";
            //isPay.IsChecked = bill.IsPay;

            clickBotton.Content = "Edit bill";

            return this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Bill bill, string error) = _billModule.ValidateBill(
                date.SelectedDate,
                description.Text,
                installment.Text,
                value.Text,
                type.Text,
                isPay.IsChecked.GetValueOrDefault());

            if (bill != null)
            {
                bill.Id = _billId;

                _billFacade.Save(bill);

                Close();
            }
            else
            {
                Error.Content = error;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }

    internal interface IBillManager
    {
        BillManager Factory();

        BillManager Factory(Bill bill);
    }
}