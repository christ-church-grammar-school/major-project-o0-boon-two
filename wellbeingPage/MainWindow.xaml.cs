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

namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clicked(object sender, RoutedEventArgs e)
        {
            ShowWellbeingPage();
        }

        private void ShowWellbeingPage()
        {
            Marks page = new Marks();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }
    }
}
