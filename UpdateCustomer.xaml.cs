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
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        public int countryId;
        public int addressId;
        public int cityId;
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        public Dashboard dashboardObject;




        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextbox.Text) ||
                string.IsNullOrWhiteSpace(phoneTextbox.Text) ||
                string.IsNullOrWhiteSpace(addressTextbox.Text) ||
                string.IsNullOrWhiteSpace(cityTextbox.Text) ||
                string.IsNullOrWhiteSpace(zipTextbox.Text) ||
                string.IsNullOrWhiteSpace(countryTextbox.Text
                )

                )
            {
                MessageBox.Show("Ensure all field are valid.");
            }
            else
            {
                CustomerDetailsDataClass custUpdated = new CustomerDetailsDataClass();
                custUpdated.customerName = nameTextbox.Text;
                custUpdated.customerId = Int32.Parse(custIDSearchBox.Text);
                custUpdated.phone = phoneTextbox.Text;
                custUpdated.address = addressTextbox.Text;
                custUpdated.city = cityTextbox.Text;
                custUpdated.zip = zipTextbox.Text;
                custUpdated.country = countryTextbox.Text;

                custUpdated.countryId = countryId;
                custUpdated.cityId = cityId;
                custUpdated.addressId = addressId;

                if (activeYes.IsChecked == true)
                {
                    custUpdated.active = "True";
                }
                if (activeNo.IsChecked == true)
                {
                    custUpdated.active = "False";
                }

                bool updateCustomer = SharedDataHelp.UpdateCustomer(custUpdated);
                if (updateCustomer)
                {
                    MessageBox.Show($"{custUpdated.customerName} has been updated.");
                    Close();
                }
                else
                {
                    MessageBox.Show($"{custUpdated.customerName} could not be updated.");
                }
            }



            
        }


        private void searchButton_Click(object sender, RoutedEventArgs e)
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

                countryId = Int32.Parse(custSearch.countryId.ToString());
                cityId = Int32.Parse(custSearch.cityId.ToString());
                addressId = Int32.Parse(custSearch.addressId.ToString());

                if (custSearch.active == "True")
                {
                    activeYes.IsChecked = true;
                }
                if (custSearch.active == "False")
                {
                    activeNo.IsChecked = true;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not search ID");
            }

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
