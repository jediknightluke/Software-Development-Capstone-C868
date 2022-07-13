using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BOP3_Task_1_C_Sharp_Application_Development
{
    /// <summary>
    /// Interaction logic for ViewAllCustomers.xaml
    /// </summary>
    public partial class ViewAllCustomers : Window
    {
        public ViewAllCustomers()
        {
            InitializeComponent();
            viewAllCustomersCalendar.DataContext = SharedDataHelp.ViewAllCustomers();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SortByActive()
        {
            //var sortedCustomers = viewAllCustomersCalendar.Columns[0].Where(c => c.active.Equals(true)); // This lambda expression
            var item = (ViewAllCustomersDataCall)viewAllCustomersCalendar.ItemsSource;
            
        }
    }
}
