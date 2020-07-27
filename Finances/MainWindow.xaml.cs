using Finances.Facade;
using Finances.Model;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Finances
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBillFacade _billFacade;
        private Wallet _wallet;

        public MainWindow()
        {
            // Colors in Hex
            // #080705 black
            // #40434E charcoal
            // #702632 wine
            // #912F40 red violet
            // #fffffa white
            InitializeComponent();

            var services = Dependencies.GetServiceProvider();
            _billFacade = services.GetRequiredService<IBillFacade>();

            _wallet = new Wallet();

            LoadInterface();
        }

        private void LoadInterface()
        {
            _wallet.Bills = _billFacade.GetAllBills();
            foreach (var bill in _wallet.Bills)
            {
                _wallet.Balance += bill.Value;
            }

            balance.Foreground = _wallet.Balance >= 0
                ? new SolidColorBrush(Colors.Green)
                : balance.Foreground = new SolidColorBrush(Colors.Red);

            balance.Content = $"${_wallet.Balance}";

            BillList.ItemsSource = _wallet.Bills;
            BillList.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bill = new AddBill(_billFacade);
            bill.ShowDialog();
        }
    }
}