using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Windows;
using System.IO;
using System.Threading;
using SQLite;
using System.Configuration;


namespace wellbeingPage
{
    class GetStudentData
    {
        
        public static async Task DownloadLiveMarks(bool ShowError)
        {
            
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            
            var a  = conn.Table<Preferences.Info>().ToList()[0].Username;
            var b = conn.Table<Preferences.Info>().ToList()[0].Password;

            var c = conn.Table<Preferences.Info>().ToList()[0].StudentNO;

            var cTask = Task.Run(() => StartDownload(a,b,c,ShowError));
            await cTask;
        }

                
        private static void ParseMarks(string inp, string year)
        {
           
            Subject sub = new Subject();
            List<string> Lines = new List<string>(inp.Split(new[] { "\n" }, StringSplitOptions.None).ToList());

            //FIND NAME + TEACHER
            List<string> First = Lines[0].Split(new[] { " " }, StringSplitOptions.None).ToList();
            int Pindex = 0;
            for (int j = 0; j < First.Count; j++)
            {
                //Console.WriteLine(First[j]);
                if (First[j] == "Mrs" || First[j] == "Ms" || First[j] == "Miss" || First[j] == "Mr" || First[j] == "Dr" || First[j] == "Rev")
                {
                    Pindex = j;
                        
                }
            }
            sub.Name = String.Join(" ",First.ToList().GetRange(0, Pindex)) + "\n";
            sub.teacher = String.Join(" ", First.ToList().GetRange(Pindex, First.Count - Pindex));
            sub.Year = year;
            //MessageBox.Show(sub.Name + "         " + sub.teacher);

            // FIND MARKS;
            List<string> tests = Lines.ToList().GetRange(4, Lines.Count - 5);

            
             
            foreach (var a in tests)
            {
                int SlashCount = a.Split('/').Length - 1;
                if (SlashCount >= 3) //Assume there is marks
                {
                    //Module 1 Project 7/04/2019 20 / 20 100 93% 6.00%          EXAMPLE OF LAYOUT

                    List<string> line1 = a.Split(new[] { " " }, StringSplitOptions.None).ToList();
                    List<string> line = new List<string>();

                    for (var i  = 0; i < line1.Count; i ++)
                    {
                        if (line1[i][0] == '/' && line1[i].Length >= 2) // AMC format
                        {
                            line.Add("/");
                            line.Add(line1[i].Remove(0, 1));
                        } else
                        {
                            
                            line.Add(line1[i]);
                        }
                    }

                    var mrk = new Mark();

                    mrk.weight= Convert.ToDouble(line[line.Count - 1].Split(new[] { "%" }, StringSplitOptions.None).ToList()[0]); // Getting weight as last term
                    mrk.average = Convert.ToInt32(line[line.Count - 2].Split(new[] { "%" }, StringSplitOptions.None).ToList()[0]);
                    mrk.percent = Convert.ToDouble(line[line.Count - 6]) / Convert.ToDouble(line[line.Count - 4]);
                    mrk.year = year;
                    mrk.mark = line[line.Count - 6] + "/" +line[line.Count - 4];
                    //MessageBox.Show(sub.YourScores[0]); 

                    mrk.date = Convert.ToDateTime(line[line.Count - 7]);
                    mrk.subject = sub.Name;
                    var subset = line.ToList().GetRange(0, line.Count - 7);
                    mrk.name = String.Join(" ", subset) + "\n";
         
                    sub.marks.Add(mrk);
                        
                } //no marks allocated
                else
                {
                                
                }
                                    
            }
            var split = Lines.Last().Split(new[] { "%" }, StringSplitOptions.None).ToList();
            if (sub.marks.Count > 0)
            {
                sub.YourAverage = Convert.ToInt32((split[0].Split(new[] { " " }, StringSplitOptions.None).ToList()).Last());
                try
                {
                    sub.EveryoneAverage = Convert.ToInt32((split[1].Split(new[] { " " }, StringSplitOptions.None).ToList()).Last());

                }
                catch
                {
                    sub.EveryoneAverage = 0;
                }
                
                
            }
            if (split.Count == 2)
            {
                sub.EveryoneAverage = Convert.ToInt32((split[0].Split(new[] { " " }, StringSplitOptions.None).ToList()).Last());
            }
            if (sub.YourAverage < 0)
            {
                sub.YourAverage = -1;
            }

            sub.All = sub.Year + sub.Name;

           
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");

            conn.CreateTable<Subject>();
            conn.InsertOrReplace(sub);
     

            conn.CreateTable<Mark>();
            foreach (var j in sub.marks)
            {
                j.All = j.year + j.name + j.subject;
                conn.InsertOrReplace(j);
            }
            

        }

        private static bool StartDownload(string username, string password, string studentNum, bool ShowError)
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            var Options = new ChromeOptions();
            Options.AddArgument("--window-position=-32000,-32000");  //sets chrome window to out of screen (-32000, -320000)
            Options.AddAdditionalCapability("useAutomationExtension", false);
            //Options.AddArgument("--headless");
            //Options.AddArgument("--always-on");

            Console.WriteLine("Logging into live marks with username: " + username + " and password:  " + password);

            ChromeDriver driver = new ChromeDriver(driverService, Options);
            try
            {
                driver.Navigate().GoToUrl("https://parentportal.ccgs.wa.edu.au/");
                System.Threading.Thread.Sleep(5000);
                var search_box = driver.FindElementById("TextBoxUserName");
                search_box.SendKeys(username);
                search_box = driver.FindElementById("textBox2");
                search_box.SendKeys(password);
                driver.FindElementById("loginButton").Click();


                driver.Navigate().GoToUrl("https://parentportal.ccgs.wa.edu.au/stures.aspx?sid=" + studentNum);
                System.Threading.Thread.Sleep(3000);
                var a = driver.FindElementByXPath("//*[@id='cla_table']/tbody/tr[2]/td[2]/a");

                Actions action = new Actions(driver);
                action.MoveToElement(a).Click(a).Build().Perform();

                var window_after = driver.WindowHandles[1];
                driver.Close();
                driver.SwitchTo().Window(window_after);
                System.Threading.Thread.Sleep(5000);

                var element = driver.FindElementByCssSelector("body");
                System.Threading.Thread.Sleep(4000);

                string PrevClipboard = "";

                string Data = "";

                var t = new Thread((ThreadStart)(() =>

                {

                    PrevClipboard = Clipboard.GetText();
                    //Console.Write(PrevClipboard);

                    element.Click();

                    //Console.WriteLine("Yes");


                    element.SendKeys(Keys.Control + "a");
                    System.Threading.Thread.Sleep(50);
                    element.SendKeys(Keys.Control + "c");

                    System.Threading.Thread.Sleep(50);
                    try
                    {
                        Data = Clipboard.GetText();
                        Clipboard.Clear();

                        Clipboard.SetText(PrevClipboard);
                    }
                    catch { }


                }));

                t.SetApartmentState(ApartmentState.STA);

                t.Start();

                t.Join();


                driver.Close();
                driver.Quit();

                var lines = Data.Split(new[] { "\n" }, StringSplitOptions.None);

                var FirstLine = lines[0].Split(new[] { " " }, StringSplitOptions.None);

                var stry = FirstLine.Last();
                string year = stry.Substring(0, 4);
                Console.WriteLine("------------------:" + year.Contains('\n'));
                Console.WriteLine(year);

                int[] indexSub = new int[10];
                int[] indexProg = new int[10];

                int num1 = 0;
                int num2 = 0;

                for (int line = 0; line < lines.Length; line++)
                {
                    if ((lines[line].Contains(" Dr ") || lines[line].Contains(" Mr ") || lines[line].Contains(" Mrs ") || lines[line].Contains(" Ms ") || lines[line].Contains(" Miss ") || lines[line].Contains("Rev")) && lines[line].Contains(":") == false)
                    {
                        indexSub[num2] = line;
                        num2++;
                    }
                    if (lines[line].Contains("Progressive mark"))
                    {
                        indexProg[num1] = line;
                        num1++;
                    }
                }
                int run = 0;

                try
                {
                    while (indexProg[run] != '\0')
                    {
                        Console.WriteLine(indexSub[run] + "   " + indexProg[run]);
                        var subset = lines.ToList().GetRange(indexSub[run], indexProg[run] - indexSub[run] + 1);
                        //Console.WriteLine(String.Join("\n", subset));
                        ParseMarks(String.Join("\n", subset), year);

                        run++;
                    }

                }
                catch
                {

                }


                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    MainWindow.GetFromDB();
                    Marks.CurrentResults.Clear();
                });
                foreach (Subject sub in Marks.SubjectResults)
                {
                    if (sub.marks.Count > 0 && sub.Year == Marks.CurrentYear)
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {
                            Marks.CurrentResults.Add(sub);
                        });
                    }
                }



                App.Current.Dispatcher.Invoke((Action)delegate
                    {

                        var str = "Last Updated: " + DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");

                        ((MainWindow)System.Windows.Application.Current.MainWindow).LastUp.Text = str;
                        ((MainWindow)System.Windows.Application.Current.MainWindow).ReloadButton.IsEnabled = true;
                        ((MainWindow)Application.Current.MainWindow).ReloadRotater.Angle = 0;


                    });
                MainWindow.GetFromDB();
                Console.WriteLine("Chromedriver was unable to complete webscraping");


                return true;
            }
            catch
            {
                Console.WriteLine("Chromedriver was unable to complete webscraping");
                driver.Quit();
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ReloadButton.IsEnabled = true;
                    ((MainWindow)Application.Current.MainWindow).ReloadRotater.Angle = 0;

                    if (ShowError)
                    {
                        MessageBox.Show("There was an error connecting to Live Marks. Please ensure that you:\n\n   " +
                            " - Are connected to the internet\n\n    - Have inputted the correct credentials (change in settings)\n\n    " +
                            "- Do not exit the Chrome window\n\n\n And then try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });



                return false;
            }
        }
    }
}