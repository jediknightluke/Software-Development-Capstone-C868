using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace BOP3_Task_1_C_Sharp_Application_Development
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string errorLabel;
        public MainWindow()
        {
            InitializeComponent();
            determineLanguage();
        }

        static public int FindUser(string userName, string password)
        {
            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT userId FROM user WHERE userName = '{userName}' AND password = '{password}'", c);
            MySqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.HasRows)
            {
                rdr.Read();
                SharedDataHelp.setCurrentUserId(Convert.ToInt32(rdr[0]));
                SharedDataHelp.setCurrentUserName(userName);
                rdr.Close(); c.Close();
                return SharedDataHelp.getCurrentUserId();
            }
            return 0;
        }

        private void login_Click(object sender, EventArgs e)
        {

            if (FindUser(usernameTextBox.Text, passwordTextBox.Password) != 0)
            {
                this.Hide();
                Dashboard dashboard = new Dashboard();
                dashboard.loginForm = this;
                // Logger.writeUserLoginLog(DataHelper.getCurrentUserId());
                dashboard.Show();
            }
            else
            {
                MessageBox.Show(errorLabel);
                passwordTextBox.Clear();
            }
        }

        private void determineLanguage()
        {
            switch (RegionInfo.CurrentRegion.EnglishName)
            {

                case "Germany":
                    GermanLoginForm();
                    break;
                case "United States":
                    EnglishLoginForm();
                    break;

                default:
                    EnglishLoginForm();
                    break;
            }
        }

        private void EnglishLoginForm()
        {
            LoginHeading.Text = "Log In";
            usernameTextBlock.Text = "Username";
            passwordTextBlock.Text = "Password";
            errorLabel = "The username and password did not match";
            loginButton.Content = "Login";
            projectNameTextBlock.Text = "Software Development Capstone(RYM2) - Task 2";
            classNameTextBlock.Text = "Software Development Capstone C868";
            studentNameTextBlock.Text = "By: Luke Melton";
        }

        private void GermanLoginForm()
        {
            LoginHeading.Text = "Einloggen";
            usernameTextBlock.Text = "Nutzername";
            passwordTextBlock.Text = "Passwort";
            errorLabel = "Der Benutzername und das Passwort stimmten nicht überein";
            loginButton.Content = "Einloggen";
            projectNameTextBlock.Text = "BOP3-Aufgabe 1: C#-Anwendungsentwicklung";
            classNameTextBlock.Text = "Software II – Erweitertes C# – C969";
            studentNameTextBlock.Text = "Von: Luke Melton";
        }
    }


}
