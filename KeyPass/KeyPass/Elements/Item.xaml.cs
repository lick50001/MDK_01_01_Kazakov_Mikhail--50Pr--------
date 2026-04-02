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
using KeyPass.Models;
using KeyPass.Pages;

namespace KeyPass.Elements
{
    /// <summary>
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        Storage storage;
        Main main;
        public Item(Storage storage, Main main)
        {
            InitializeComponent();
            tbName.Text = storage.Name;
            tbUrl.Text = storage.Url;
            tbLogin.Text = storage.Login;
            tbPassword.Text = storage.Password;

            this.main = main;
            this.storage = storage;
        }

        private void Update(object sender, RoutedEventArgs e) =>
            MainWindow.init.OpenPages(new Pages.Add(Storage));

        private void Delete(object sender, RoutedEventArgs e)
        {
            StorageContext.Delete(Storage.Id);
            this.main.StorageList.Children.Remove(this);
            MessageBox.Show("Данные удалены");
        }
    }
}
