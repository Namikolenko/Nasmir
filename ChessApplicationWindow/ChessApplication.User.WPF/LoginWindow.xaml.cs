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

namespace ChessApplication.User.WPF
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            SoloGame.Click += buttonClickOne;
            OnlineGame.Click += buttonClickTwo;
        }

        private void buttonClickOne(object sender, EventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
        private void buttonClickTwo(object sender, EventArgs e)
        {
            OnlineGameWindow onlineWindow = new OnlineGameWindow();
            onlineWindow.Show();
            this.Close();
        }
    }
}
