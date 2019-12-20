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
using System.Net.Sockets;
using System.Net;

namespace ChessApplication.User.WPF
{
    /// <summary>
    /// Логика взаимодействия для ServerChat.xaml
    /// </summary>
    public partial class ServerChat : Window
    {
        private List<TextBlock> messages = new List<TextBlock>();
        byte[] data = new byte[256]; // Buffer
        StringBuilder response;

        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("25.56.174.87"), 8888); // Адрес хамачи
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // создаем сокет
        public ServerChat()
        {
            InitializeComponent();
            response = new StringBuilder();
            socket.Connect(ipPoint);
            byte[] colordata = new byte[256];
            string color = "";
            int bytes = 0;
            RecieveMessage();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            data = new byte[256];
            if (EnteredText.Text != "")
            {
                messages.Add(new TextBlock());
                messages.LastOrDefault().Text = "You: " + EnteredText.Text;
                StackHeap.Children.Add(messages.LastOrDefault());

                data = Encoding.Unicode.GetBytes("Enemy: " + EnteredText.Text);
                socket.Send(data);

                EnteredText.Text = "";
            }
        }

        private async void RecieveMessage()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    byte[] recdata = new byte[256];
                    string answer = "";
                    int bytes = 0;
                    response = new StringBuilder();
                    while (answer == "")
                    {
                        do
                        {
                            bytes = socket.Receive(recdata, recdata.Length, 0);
                            response.Append(Encoding.Unicode.GetString(recdata, 0, bytes));
                        }
                        while (socket.Available > 0);
                        answer = response.ToString();
                    }
                    Dispatcher.Invoke(() =>
                    {
                        messages.Add(new TextBlock());
                        messages.LastOrDefault().Text = answer;
                        StackHeap.Children.Add(messages.LastOrDefault());
                    });
                }
            });
        }
    }
}
