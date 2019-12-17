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
        List<List<Button>> field = new List<List<Button>>();
        List<Button> level = new List<Button>();
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
            //A1Button.Background = default;
            //A1Button.Background = new ImageBrush(new Image);
            field.Add(new List<Button>() { A1Button, A2Button, A3Button, A4Button, A5Button, A6Button, A7Button, A8Button });
            field.Add(new List<Button>() { B1Button, B2Button, B3Button, B4Button, B5Button, B6Button, B7Button, B8Button });
            field.Add(new List<Button>() { C1Button, C2Button, C3Button, C4Button, C5Button, C6Button, C7Button, C8Button });
            field.Add(new List<Button>() { D1Button, D2Button, D3Button, D4Button, D5Button, D6Button, D7Button, D8Button });
            field.Add(new List<Button>() { E1Button, E2Button, E3Button, E4Button, E5Button, E6Button, E7Button, E8Button });
            field.Add(new List<Button>() { F1Button, F2Button, F3Button, F4Button, F5Button, F6Button, F7Button, F8Button });
            field.Add(new List<Button>() { G1Button, G2Button, G3Button, G4Button, G5Button, G6Button, G7Button, G8Button });
            field.Add(new List<Button>() { H1Button, H2Button, H3Button, H4Button, H5Button, H6Button, H7Button, H8Button });

            Setup();


        }

        void Setup()
        {
            field[0][0].Content = "R";
            field[0][1].Content = "N";
            field[0][2].Content = "B";
            field[0][3].Content = "Q";
            field[0][4].Content = "K";
            field[0][5].Content = "B";
            field[0][6].Content = "N";
            field[0][7].Content = "R";

            field[1][0].Content = "P";
            field[1][1].Content = "P";
            field[1][2].Content = "P";
            field[1][3].Content = "P";
            field[1][4].Content = "P";
            field[1][5].Content = "P";
            field[1][6].Content = "P";
            field[1][7].Content = "P";

            field[6][0].Content = "p";
            field[6][1].Content = "p";
            field[6][2].Content = "p";
            field[6][3].Content = "p";
            field[6][4].Content = "p";
            field[6][5].Content = "p";
            field[6][6].Content = "p";
            field[6][7].Content = "p";

            field[7][0].Content = "r";
            field[7][1].Content = "n";
            field[7][2].Content = "b";
            field[7][3].Content = "q";
            field[7][4].Content = "k";
            field[7][5].Content = "b";
            field[7][6].Content = "n";
            field[7][7].Content = "r";
        }
    }
}
