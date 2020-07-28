using Finances.Facade;
using Finances.Model;
using System.Collections.Generic;
using System.Windows;

namespace Finances
{
    /// <summary>
    /// Interaction logic for ScheduleBill.xaml
    /// </summary>
    public partial class ScheduleBill : Window, IScheduleBill
    {
        private readonly IBillFacade _billFacade;
        private readonly IScheduleFacade _scheduleFacade;
        private readonly IScheduleManager _scheduleManager;
        private IList<Schedule> _schedules;

        public ScheduleBill(IScheduleFacade scheduleFacade, IBillFacade billFacade, IScheduleManager scheduleManager)
        {
            _scheduleFacade = scheduleFacade;
            _billFacade = billFacade;
            _scheduleManager = scheduleManager;
        }

        public ScheduleBill Factory()
        {
            InitializeComponent();

            LoadInterface();

            return this;
        }

        private void LoadInterface()
        {
            editButton.IsEnabled = false;
            deleteButton.IsEnabled = false;

            _schedules = _scheduleFacade.GetAllSchedules();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            var schedule = _scheduleManager.Factory();
            schedule.ShowDialog();
            LoadInterface();
        }

        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (ScheduleList.SelectedIndex >= 0)
            {
                var schedule = _schedules[ScheduleList.SelectedIndex];

                var bill = _billFacade.GetBill(schedule.BillId);

                var result = MessageBox.Show($"Do you want remove {bill?.Description} on value ${bill?.Value}?", "Remove bill", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _scheduleFacade.Remove(schedule);
                    LoadInterface();
                }
            }
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (ScheduleList.SelectedIndex >= 0)
            {
                var schedule = _scheduleManager.Factory();
                schedule.ShowDialog();
                LoadInterface();
            }
        }

        private void ScheduleList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ScheduleList.SelectedIndex >= 0)
            {
                editButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
            }
            else
            {
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }
    }

    public interface IScheduleBill
    {
        ScheduleBill Factory();
    }
}