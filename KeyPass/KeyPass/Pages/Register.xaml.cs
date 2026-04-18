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
using KeyPass.Context;
using KeyPass.Pages;

namespace KeyPass.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void BthAuthClick(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string pass = tbPassword.Password;

            if (string.IsNullOrEmpty(tbLogin.Text))
            {
                MessageBox.Show("Необходимо указать логин пользователя");
                return;
            }

            if (string.IsNullOrEmpty(tbPassword.Password))
            {
                MessageBox.Show("Необходимо указатб пароль пользователя");
                return;
            }

            bool isSuccess = await UserContext.Register(login, pass);

            if (isSuccess)
            {
                MessageBox.Show("Регистрация успешна!");
                NavigationService.Navigate(new Main());
            }
        }
    }
}
