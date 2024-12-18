using AntEngine;
using System;
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
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for WinnerPage.xaml
    /// </summary>
    public partial class WinnerPage : Window
    {

        private Map my_map;

        public WinnerPage(List<string> winnerList)
        {
            InitializeComponent();

            WinnerListBox.ItemsSource = winnerList;
        }


    }
}
