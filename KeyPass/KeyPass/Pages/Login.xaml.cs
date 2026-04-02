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

namespace KeyPass.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Ассинхронный метод аутентификации пользователя
        /// Отправляет логин и проль на сервер и получает токен
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public async Task Auth(string login, string password)
        {
            string? Token = await UserContext.Login(login, password);
            if (Token == null)
                MessageBox.Show("Логин и пароль указаны не верно");
            else
            {
                MainWindow.Token = Token; ;
                MainWindow.init.OpenPages(new Pages.Main());
            }
        }
        
        private void BthAuth(object sender, RoutedEventArgs e)
        {
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

            Auth(tbLogin.Text, tbPassword.Password);
        }
    }
}
