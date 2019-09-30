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
using SQLite;

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
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            
            var a = conn.Table<Preferences.Info>().ToList()[0];
            UserField.Text = a.Username;
            PassField.Password = a.Password;
            
        }

        private void Save(object sender, RoutedEventArgs e)
        {
           
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            
            var a = conn.Table<Preferences.Info>().ToList()[0];
            a.Username = UserField.Text;
            a.Password = PassField.Password;
            
            conn.InsertOrReplace(a);

            Window.GetWindow(this).Close();
        }
    }
}
