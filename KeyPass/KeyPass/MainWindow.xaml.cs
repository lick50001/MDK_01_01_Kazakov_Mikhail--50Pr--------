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
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyPass.Pages;

namespace KeyPass
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow init;
        public static string Token { get; set; } = string.Empty;
        public MainWindow()
        {
            init = this;
            InitializeComponent();
            frame.Navigate(new Login());
        }

        public void OpenPages(Page page)
        {
            frame.Navigate(page);
        }
    }
}
