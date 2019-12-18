using ChessApplication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        public Image chessSprites;
        public Button prevButton;
        string madeMove = "";

        static Chess chess;
        List<String> allMoves;

        public Button[,] BoardCell = new Button[8, 8];

        int port = 8888; // Unused port
        IPAddress server = IPAddress.Parse("25.56.174.87"); // Hamachi IP address
        byte[] data = new byte[4096]; // Buffer
        NetworkStream stream;
        StringBuilder response;
        string color = "black";

        private Action<Chess> messageRecieved;

        public MainWindow()
        {
            chess = new Chess("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1");
            Hold();
            InitializeComponent();
            drawCoordinates();
            drawBoard();
            figureStender(chess);
            TcpClient client = new TcpClient();
            client.Connect(server, port);
            response = new StringBuilder();
            stream = client.GetStream();
            //Thread myThread = new Thread(new ThreadStart(ServerThread));
            //myThread.Start();

            //int bytes = 0;
            //while(color == "")
            //{
            //    do
            //    {
            //        bytes = stream.Read(data, 0, data.Length);
            //        response.Append(Encoding.Unicode.GetString(data, 0, bytes));
            //    }
            //    while (stream.DataAvailable);
            //    color = response.ToString();
            //}
            //PlayerColorLabel.Content = "You are ";
            //PlayerColorLabel.Content += color;

            if (color == "black")
            {
                this.Show();
                string answer = "";
                int bytes = 0;
                while (answer == "")
                {
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    answer = response.ToString();
                }
                figureStender(new Chess(answer));
            }

        }

        //private void labelContent(Chess chess)
        //{

        //}

        private void drawCoordinates()
        {
            RowNumbersRight.Background = Brushes.Chocolate;
            for (int i = 8; i > 0; i--)
            {
                Label number = new Label();
                number.Height = RowNumbersRight.Height / 8;
                number.Width = RowNumbersRight.Width;
                number.Content = $"\n{i}";
                RowNumbersRight.Children.Add(number);
            }
            RowNumbersLeft.Background = Brushes.Chocolate;
            for (int i = 8; i > 0; i--)
            {
                Label number = new Label();
                number.Height = RowNumbersLeft.Height / 8;
                number.Width = RowNumbersLeft.Width;
                number.Content = $"\n{i}";
                RowNumbersLeft.Children.Add(number);
            }
            ColumnLettersTop.Background = Brushes.Chocolate;
            for (int i = 0; i < 8; i++)
            {
                Label letter = new Label();
                letter.Height = ColumnLettersTop.Height;
                letter.Width = ColumnLettersTop.Width / 8;
                letter.Content = Convert.ToChar('a' + i).ToString().PadLeft(7);
                ColumnLettersTop.Children.Add(letter);
            }
            ColumnLettersDown.Background = Brushes.Chocolate;
            for (int i = 0; i < 8; i++)
            {
                Label letter = new Label();
                letter.Height = ColumnLettersDown.Height;
                letter.Width = ColumnLettersDown.Width / 8;
                letter.Content = Convert.ToChar('a' + i).ToString().PadLeft(7);
                ColumnLettersDown.Children.Add(letter);
            }
        }

        private void drawBoard()
        {
            int buttonSize = (int)ChessBoard.Width / 8;
            ChessBoard.Height = ChessBoard.Width;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardCell[i, j] = new Button();
                    BoardCell[i, j].Height = buttonSize;
                    BoardCell[i, j].Width = buttonSize;
                    ChessBoard.Children.Add(BoardCell[i, j]);
                    BoardCell[i, j].Background = (i + j) % 2 == 1 ? Brushes.Chocolate : Brushes.Bisque;
                    BoardCell[i, j].Click += ButtonClick;
                    BoardCell[i, j].Name = Convert.ToChar('a' + j).ToString() + Convert.ToChar('1' - i + 7).ToString();
                }
            }
        }

        private void figureStender(Chess chess)
        {
            allMoves = chess.GetAllMoves();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardCell[i, j].Content = chess.GetFigureAt(j, 7 - i) == '1' ? "" : chess.GetFigureAt(j, 7 - i).ToString();
                }
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;

            //coloring the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardCell[i, j].Background = (i + j) % 2 == 1 ? Brushes.Chocolate : Brushes.Bisque;
                }
            }

            //Coloring choosed cell and possible moves
            if (pressedButton.Content.ToString() != "")
            {
                pressedButton.Background = Brushes.Green;
                foreach (var cell in BoardCell)
                {
                    if (allMoves.Contains(pressedButton.Content + pressedButton.Name + cell.Name))
                    {
                        cell.Background = Brushes.GreenYellow;
                    }
                }
            }


            object prevButtonContent = null;

            if (prevButton != null && prevButton.Content != "")
            {
                //Creating a move
                madeMove = prevButton.Content + prevButton.Name + pressedButton.Name;
                if (allMoves.Contains(madeMove))
                {
                    //Making a move
                    chess = chess.Move(madeMove);
                    figureStender(chess);
                    // Sending to server
                    data = Encoding.Unicode.GetBytes(chess.fen);
                    stream.Write(data, 0, data.Length);
                    // Wait for receiving message
                    /*string answer ="";
                    int bytes = 0;
                    while (answer == "")
                    {
                        do
                        {
                            bytes = stream.Read(data, 0, data.Length);
                            response.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (stream.DataAvailable);
                        answer = response.ToString();
                    }
                    //chess = chess.Move(answer);
                    figureStender(new Chess(answer));
                    //pressedButton.Content = answer;*/
                }
                string answer = "";
                int bytes = 0;
                while (answer == "")
                {
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    answer = response.ToString();
                }
                figureStender(new Chess(answer));
            }

            prevButton = pressedButton;
            if (prevButtonContent != null)
            {
                prevButton = null;
            }
        }
        private async void Hold()
        {
            await Task.Run(() => ServerThread());
        }
        private void ServerThread()
        {
            while (true)
            {
                string answer = "";
                int bytes = 0;
                while (answer == "")
                {
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    answer = response.ToString();
                }
                figureStender(new Chess(answer));
            }
        }
    }
}
