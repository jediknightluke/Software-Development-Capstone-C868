using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for CreateCustomer.xaml
    /// </summary>
    public partial class CreateCustomer : Window
    {
        public CreateCustomer()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CreateCustomer_Click(object sender, RoutedEventArgs e)
        {


            string timestamp = DateTime.Parse(SharedDataHelp.createTimestamp()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture);
            string userName = SharedDataHelp.getCurrentUserName();
            if (string.IsNullOrEmpty(countryTextbox.Text) ||
                string.IsNullOrEmpty(addressTextbox.Text) ||
                string.IsNullOrEmpty(cityTextbox.Text) ||
                string.IsNullOrEmpty(zipTextbox.Text) ||
                string.IsNullOrEmpty(phoneTextbox.Text) ||
                string.IsNullOrEmpty(nameTextbox.Text) ||
                (yesRadioButton.IsChecked == false && noRadioButton.IsChecked == false))
            {
                MessageBox.Show("Ensure all fields are valid");
            }
            else
            {
                int active = (bool)yesRadioButton.IsChecked ? 1 : 0;
                bool activeBool = (bool)yesRadioButton.IsChecked ? true : false;
                int countryId = SharedDataHelp.createRecordID(timestamp, userName, "country", $"'{countryTextbox.Text}'");
                int cityId = SharedDataHelp.createRecordID(timestamp, userName, "city", $"'{cityTextbox.Text}', '{countryId}'");
                int addressId = SharedDataHelp.createRecordID(timestamp, userName, "address", $"'{addressTextbox.Text}', '', '{cityId}', '{zipTextbox.Text}', '{phoneTextbox.Text}'");

                CreateCustomerRecordDataClass createCustomer = new CreateCustomerRecordDataClass();

                createCustomer.customerName = nameTextbox.Text;
                createCustomer.phone = phoneTextbox.Text;
                createCustomer.address = addressTextbox.Text;
                createCustomer.city = cityTextbox.Text;
                createCustomer.zipCode = zipTextbox.Text;
                createCustomer.country = countryTextbox.Text;
                createCustomer.active = activeBool;


                SharedDataHelp.CreateCustomer(createCustomer, addressId);
                MessageBox.Show("Customer Created!");

                Close();
            }

        }

    }
}
