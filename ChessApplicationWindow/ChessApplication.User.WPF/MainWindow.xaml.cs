using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace ChessApplication.User.WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int port = 8888;
        string address = "25.56.174.87";
        public MainWindow()
        {
            InitializeComponent();
            StatusLabel.Content = "Waiting for conection...";
            string colorFromServer = "";

            var client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();

            while (colorFromServer == "")
            {
                var data = new byte[64]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                colorFromServer = builder.ToString();
            }
            if (colorFromServer == "black")
                StatusLabel.Content = "You are black!";
            else
                StatusLabel.Content = "You are white!";
        }
    }
}
