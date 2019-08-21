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
    /// Interaction logic for SetSettings.xaml
    /// </summary>
    public partial class SetSettings : Page
    {
        public SetSettings()
        {
            InitializeComponent();
            List<string> Lines = new List<string>(System.IO.File.ReadAllLines("data/cred.txt"));
            UserField.Text = Lines[0];
            PassField.Password = Lines[1];
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            using (StreamWriter outputFile = new StreamWriter("data/cred.txt"))
            {
                outputFile.Write( UserField.Text + "\n" + PassField.Password.ToString());
            }
            Window.GetWindow(this).Close();
        }
    }
}
