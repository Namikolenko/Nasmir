﻿using ChessApplication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;

namespace ChessApplication.User.WPF
{
    /// <summary>
    /// Логика взаимодействия для OnlineGameWindow.xaml
    /// </summary>
    public partial class OnlineGameWindow : Window
    {
        ServerChat chatWindow;
        public Image chessSprites;
        public Button prevButton;
        string madeMove = "";

        static Chess chess;
        List<string> allMoves;
        Dictionary<string, int> eatenFigures = new Dictionary<string, int>();

        public Button[,] BoardCell = new Button[8, 8];
        byte[] data = new byte[256]; // Buffer
        NetworkStream stream;
        StringBuilder response;

        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("25.56.174.87"), 8888); // Адрес хамачи
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // создаем сокет
        bool flag;

        public OnlineGameWindow()
        {
            InitializeComponent();
            chess = new Chess();
            InitializeComponent();
            drawCoordinates();
            drawBoard();
            figureStender(chess);
            eatenFiguresFiller();
            eatenFiguresShower();
            response = new StringBuilder();
            socket.Connect(ipPoint);
            chatWindow = new ServerChat();
            chatWindow.Show();
            byte[] colordata = new byte[256];
            string color = "";
            int bytes = 0;
            response = new StringBuilder();
            while (color == "")
            {
                do
                {
                    bytes = socket.Receive(colordata, colordata.Length, 0);
                    response.Append(Encoding.Unicode.GetString(colordata, 0, bytes));
                }
                while (socket.Available > 0);
                color = response.ToString();

            }
            if (color == "b")
            {
                flag = false;
                ColorTextBlock.Content = "You are black!";
            }
            else
            {
                flag = true;
                ColorTextBlock.Content = "You are white!";
            }
            RecieveMessage();
        }

        void eatenFiguresFiller()
        {
            eatenFigures["p"] = 0;
            eatenFigures["r"] = 0;
            eatenFigures["n"] = 0;
            eatenFigures["b"] = 0;
            eatenFigures["q"] = 0;
            eatenFigures["k"] = 0;

            eatenFigures["P"] = 0;
            eatenFigures["R"] = 0;
            eatenFigures["N"] = 0;
            eatenFigures["B"] = 0;
            eatenFigures["Q"] = 0;
            eatenFigures["K"] = 0;
        }

        void eatenFiguresShower()
        {
            FigureShower.Children.Clear();
            foreach (KeyValuePair<string, int> figure in eatenFigures)
            {
                if (figure.Value != 0)
                {
                    Image img = new Image();
                    string imageLocation = "../chess24/" + (char.IsUpper(figure.Key, 0) ? "w" : "b") + figure.Key + ".png";
                    img.Source = new BitmapImage(new Uri(@imageLocation, UriKind.Relative));


                    StackPanel sp = new StackPanel();
                    sp.Height = FigureShower.Height / 4;
                    sp.Width = FigureShower.Width / 3;

                    Label l = new Label();
                    l.Height = sp.Height / 2;
                    l.Width = sp.Width;
                    l.Content = figure.Value;
                    l.HorizontalContentAlignment = HorizontalAlignment.Center;

                    sp.Children.Add(img);
                    sp.Children.Add(l);

                    FigureShower.Children.Add(sp);
                }
            }
        }
        private void numberDrawer(StackPanel sp)
        {
            sp.Background = Brushes.Chocolate;
            for (int i = 8; i > 0; i--)
            {
                Label number = new Label();
                number.Height = sp.Height / 8;
                number.Width = sp.Width;
                number.Content = $"\n{i}";
                sp.Children.Add(number);
            }
        }

        private void letterDrawer(WrapPanel wp)
        {
            wp.Background = Brushes.Chocolate;
            for (int i = 0; i < 8; i++)
            {
                Label letter = new Label();
                letter.Height = wp.Height;
                letter.Width = wp.Width / 8;
                letter.Content = Convert.ToChar('a' + i).ToString().PadLeft(7);
                wp.Children.Add(letter);
            }
        }

        private void drawCoordinates()
        {
            numberDrawer(RowNumbersRight);
            numberDrawer(RowNumbersLeft);
            letterDrawer(ColumnLettersTop);
            letterDrawer(ColumnLettersDown);
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
            Dispatcher.Invoke(() =>
            {
                allMoves = chess.GetAllMoves();
                //Label content
                PlayerColorLabel.Content = (chess.fen.Split()[1] == "w" ? "White " : "Black ") + "turn";
                PlayerColorLabel.Foreground = chess.fen.Split()[1] == "w" ? Brushes.White : Brushes.Black;

                //eatenFiguresRefresher();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BoardCell[i, j].Content = chess.GetFigureAt(j, 7 - i) == '1' ? "" : chess.GetFigureAt(j, 7 - i).ToString();
                        if (BoardCell[i, j].Content.ToString() != "")
                        {

                            BoardCell[i, j].Name = BoardCell[i, j].Name.Substring(0, 2) + BoardCell[i, j].Content.ToString();
                            //Adding an image
                            Image img = new Image();
                            string imageLocation = "../chess24/" +
                                (char.IsUpper(BoardCell[i, j].Content.ToString(), 0) ? "w" : "b") +
                                BoardCell[i, j].Content.ToString() +
                                ".png";
                            img.Source = new BitmapImage(new Uri(@imageLocation, UriKind.Relative));
                            StackPanel stackPnl = new StackPanel();
                            stackPnl.Height = BoardCell[i, j].Height;
                            stackPnl.Width = BoardCell[i, j].Width;
                            stackPnl.Children.Add(img);
                            BoardCell[i, j].Content = stackPnl;
                        }
                        else
                        {
                            BoardCell[i, j].Content = null;
                            BoardCell[i, j].Name = BoardCell[i, j].Name.Substring(0, 2);
                        }
                    }
                }
                eatenFiguresShower();

                if (allMoves.Count() == 0)
                    victory();
            });
            
        }

        private void victory()
        {
            string winColor = chess.fen.Split()[1] == "w" ? "Black" : "White";
            string message = $"{winColor} player wins\nPress OK to return to home screen";
            string caption = "Congratulations";
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);
            if (result == MessageBoxResult.OK)
            {
                LoginWindow lw = new LoginWindow();
                lw.Show();
                this.Close();
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (flag)
            {
                //coloring the board
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BoardCell[i, j].Background = (i + j) % 2 == 1 ? Brushes.Chocolate : Brushes.Bisque;
                    }
                }

                //Coloring choosed cell and possible moves
                if (pressedButton.Content != null)
                {
                    pressedButton.Background = Brushes.Green;
                    foreach (var cell in BoardCell)
                    {
                        if (allMoves.Contains(pressedButton.Name[2] + pressedButton.Name.Substring(0, 2) + cell.Name.Substring(0, 2)))
                        {
                            cell.Background = Brushes.GreenYellow;
                        }
                    }
                }

                object prevButtonContent = null;

                if (prevButton != null && prevButton.Content != null)
                {
                    //Creating a move
                    madeMove = prevButton.Name[2] + prevButton.Name.Substring(0, 2) + pressedButton.Name.Substring(0, 2);
                    if (allMoves.Contains(madeMove))
                    {
                        if (pressedButton.Name.Length == 3)
                        {
                            eatenFigures[pressedButton.Name[2].ToString()] += 1;
                        }

                        chess = chess.Move(madeMove);

                        string pawnPromotion;
                        if ((prevButton.Name[2] == 'P' && pressedButton.Name[1] == '8') || (prevButton.Name[2] == 'p' && pressedButton.Name[1] == '1'))
                        {
                            PawnUpper pu = new PawnUpper(char.IsUpper(prevButton.Name[2]) ? "w" : "b");
                            pu.ShowDialog();
                            pawnPromotion = pu.figureName;
                            chess = chess.PawnPromotion(pressedButton.Name[0] - 'a', pressedButton.Name[1] - '1', pawnPromotion[0]);
                        }

                        figureStender(chess);
                        data = Encoding.Unicode.GetBytes(chess.fen);
                        socket.Send(data);
                        flag = false;
                    }
                }

                prevButton = pressedButton;
                if (prevButtonContent != null)
                {
                    prevButton = null;
                }
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
                    chess = new Chess(answer);
                    figureStender(chess);
                    flag = true;
                }
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chatWindow.Close();
        }
    }
}
