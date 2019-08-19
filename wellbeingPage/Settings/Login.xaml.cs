using System;
using System.Collections.Generic;
using System.IO;
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

namespace wellbeingPage.Settings
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void submit(object sender, RoutedEventArgs e)
        {

            Directory.CreateDirectory("data");
            using (StreamWriter outputFile = new StreamWriter("data/cred.txt"))
            {
                outputFile.WriteLine(UsernameBox.Text);
                outputFile.WriteLine(PasswordBox.Password.ToString());
            }
           
            MainWindow win = new MainWindow();
            win.Show();
            
            

        }
    }
}
