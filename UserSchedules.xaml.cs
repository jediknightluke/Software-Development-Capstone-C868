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
    /// Interaction logic for UserSchedules.xaml
    /// </summary>
    public partial class UserSchedules : Window
    {
        public UserSchedules()
        {
            InitializeComponent();
            schedulesForUserGrid.DataContext = SharedDataHelp.getUserReport();

        }

    }
}
