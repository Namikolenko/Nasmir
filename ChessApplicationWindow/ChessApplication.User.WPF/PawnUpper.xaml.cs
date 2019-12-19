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
    /// Логика взаимодействия для PawnUpper.xaml
    /// </summary>
    public partial class PawnUpper : Window
    {
        string color = "b";
        public string figureName;
        public PawnUpper(string color)
        {
            this.color = color;
            InitializeComponent();
            buttonCreator();
        }
        void buttonCreator()
        {
            //Figures

            foreach (var item in new string[] { "r", "n", "b", "q" })
            {
                Image img = new Image();
                string adress = "../chess24/" + color + item + ".png";
                img.Source = new BitmapImage(new Uri(adress, UriKind.Relative));

                Button button = new Button();
                button.Name = color == "w" ? item.ToUpper() : item;
                button.Height = Figures.Height;
                button.Width = Figures.Width / 4;
                button.Click += buttonNameReturner;

                StackPanel sp = new StackPanel();
                sp.Height = button.Height;
                sp.Width = button.Width;
                sp.Children.Add(img);

                button.Content = sp;
                Figures.Children.Add(button);
            }
        }

        private void buttonNameReturner(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            figureName = pressedButton.Name;
            this.Close();
            //this.DialogResult = pressedButton.Name;
        }
    }
}
