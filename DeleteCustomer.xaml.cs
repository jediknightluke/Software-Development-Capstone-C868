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
    /// Interaction logic for DeleteCustomer.xaml
    /// </summary>
    public partial class DeleteCustomer : Window
    {
        public DeleteCustomer()
        {
            InitializeComponent();
        }

        public Dashboard dashboardObject;

        private void CustomerSearch(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerDetailsDataClass custSearch = SharedDataHelp.getCustomerDetails(Int32.Parse(custIDSearchBox.Text));

                nameTextbox.Text = custSearch.customerName;
                phoneTextbox.Text = custSearch.phone;
                addressTextbox.Text = custSearch.address;
                cityTextbox.Text = custSearch.city;
                zipTextbox.Text = custSearch.zip;
                countryTextbox.Text = custSearch.country;

                if (custSearch.active == "True")
                {
                    activeYes.IsChecked = true;
                }
                if (custSearch.active == "False")
                {
                    activeNo.IsChecked = true;
                }
            }
            catch
            {
                MessageBox.Show("Could not search the ID.");
            }


        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (SharedDataHelp.DeleteCustomer(Int32.Parse(custIDSearchBox.Text)))
            {
                MessageBox.Show($"{nameTextbox.Text} Deleted");
                Close();
            }
            else
            {
                MessageBox.Show("Could not remove customer");
            }
        }

        private void cancel_ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
