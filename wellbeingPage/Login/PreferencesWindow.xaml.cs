using SQLite;
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
using System.Windows.Shapes;


namespace wellbeingPage
{


    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {

        /*public bool UpdateLiveMarksOnOpen = false; //  false for manual true for evey time opened                                                                      
        public bool HomeworkOnPeriodEnd = true;
        public DateTime HomeworkRemind;
        public int RemindForAssignment = 0; //number of days to start reminding before the assignment due date
        public int RemindForTest = 0; //number of days to start reminding before the Test due date*/
        public class Info
        {
            [PrimaryKey, Unique,AutoIncrement]
            public int ID { get; set; }
            public string PersonName { get; set; }
            public string Username { get; set; }

            public string StudentNO { get; set; } // implement this!

            public string Password { get; set; }
            public string LiveMarksUpdate { get; set; }
        }
        
        public Preferences()
        {
            InitializeComponent();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            
            Info inf = new Info();
            inf.Username = UsernameBox.Text;
            inf.Password = PasswordBox.Password;
            inf.StudentNO = StudentNumBox.Text;
                
            
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            conn.CreateTable<Subject>();
            conn.CreateTable<Info>();
            conn.InsertOrReplace(inf);
            conn.CreateTable<Mark>();
            conn.Close();
            ((MainWindow)System.Windows.Application.Current.MainWindow).Show();
            
            if (sender == subButton)
            {
                GetStudentData.DownloadLiveMarks(true);
                ((MainWindow)System.Windows.Application.Current.MainWindow).ReloadButton.IsEnabled = false;
            }
            this.Close();
        }
    }
}
