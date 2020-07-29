using Finances.Data;
using Finances.Facade;
using Finances.Model;
using System.Windows;
using System.Windows.Input;

namespace Finances
{
    /// <summary>
    /// Interaction logic for FindBill.xaml
    /// </summary>
    public partial class FindBill : Window, IFindBill
    {
        private readonly IBillFacade _billFacade;

        public Bill Bill { get; private set; }

        public FindBill(IBillFacade billFacade)
        {
            _billFacade = billFacade;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }

        private void ScheduleList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BillList.SelectedItem is Bill bill)
            {
                Bill = bill;
                Close();
            }
        }

        private void Button_Find(object sender, RoutedEventArgs e)
        {
            var bills = _billFacade.FindBills(Find.Text);
            if (bills.Count > 0)
            {
                BillList.ItemsSource = bills;
            }
            else
                MessageBox.Show($"Not found any bills!", "Find bill", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public FindBill Factory()
        {
            InitializeComponent();

            BillList.ItemsSource = null;

            Find.Text = string.Empty;

            return this;
        }
    }

    public interface IFindBill
    {
        FindBill Factory();
    }
}