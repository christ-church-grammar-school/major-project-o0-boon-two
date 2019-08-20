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

namespace wellbeingPage
{
    class GetStudentData
    {
        public string LastDownload = "";
        public static async Task DownloadLiveMarks(string username, string password, bool ShowError)
        {
            var cTask = Task.Run(() => Start(username,password,ShowError));
            await cTask;
        }

        public static async Task PutMarks()
        {
            var cTask = Task.Run(() => PM());
            await cTask;
            
        }
        
        private static void PM()
        {
            List<FileInfo> info = new List<FileInfo>();

            var dirInfo = new DirectoryInfo("data/marks");
            info.AddRange(dirInfo.GetFiles("*.txt*"));
            foreach (var i in info)
            {
                Subject sub = new Subject();
                
                List<string> Lines = new List<string>(System.IO.File.ReadAllLines("data/marks/" + i.Name));


                //FIND NAME + TEACHER
                List<string> First = Lines[0].Split(new[] { " " }, StringSplitOptions.None).ToList();
                int Pindex = 0;
                for (int j = 0; j < First.Count; j++)
                {
                    //Console.WriteLine(First[j]);
                    if (First[j] == "Mrs" || First[j] == "Ms" || First[j] == "Miss" || First[j] == "Mr" || First[j] == "Dr")
                    {
                        Pindex = j;
                        
                    }
                }
                sub.Name = String.Join(" ",First.ToList().GetRange(0, Pindex)) + "\n";
                sub.teacher = String.Join(" ", First.ToList().GetRange(Pindex, First.Count - Pindex));
                //MessageBox.Show(sub.Name + "         " + sub.teacher);
                
                // FIND MARKS
                List<string> tests = Lines.ToList().GetRange(4, Lines.Count - 5);

                double numerator = 0;
                double denomenator = 0;

                foreach (var a in tests)
                {
                    int SlashCount = a.Split('/').Length - 1;
                    if (SlashCount >= 3) //Assume there is marks
                    {
                        //Module 1 Project 7/04/2019 20 / 20 100 93% 6.00%          EXAMPLE

                        List<string> line = a.Split(new[] { " " }, StringSplitOptions.None).ToList();
                       
                        sub.TestWeights.Add(line[line.Count - 1]); // Getting weight as last term
                        sub.ClassAverages.Add(line[line.Count - 2]);
                        sub.YourScores.Add(line[line.Count - 6] + "/" +line[line.Count - 4]);
                        //MessageBox.Show(sub.YourScores[0]);
                        sub.Date = line[line.Count - 7];
                        
                        var subset = line.ToList().GetRange(0, line.Count - 7);
                        sub.TestNames.Add(String.Join(" ", subset));
                        
                        double value = Convert.ToDouble(line[line.Count - 6]) / Convert.ToDouble(line[line.Count - 4]);
                        double weight = Convert.ToDouble(line[line.Count - 1].Split(new[] { "%" }, StringSplitOptions.None).ToList()[0]);
                        numerator += value * weight;
                        denomenator += weight;

                    } //no marks allocated
                    else
                    {
                        
                    }
                    sub.YourAverage = (int)Math.Round(numerator * 100 / denomenator);
                }
                if (sub.YourAverage < 0)
                {
                    sub.YourAverage = -1;
                }

                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    Marks.SubjectResults.Add(sub);
                });
                

            }
        }

        private static bool Start(string username, string password, bool ShowError)
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
                var search_box = driver.FindElementByName("ctl00$ctl05$TextBoxUserName");
                search_box.SendKeys(username);
                search_box = driver.FindElementById("textBox2");
                search_box.SendKeys(password);
                driver.FindElementById("loginButton").Click();

                driver.Navigate().GoToUrl("https://parentportal.ccgs.wa.edu.au/stures.aspx");
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
                    Data = Clipboard.GetText();
                    Clipboard.Clear();

                    Clipboard.SetText(PrevClipboard);

                }));

                t.SetApartmentState(ApartmentState.STA);

                t.Start();

                t.Join();


                driver.Close();
                driver.Quit();

                var lines = Data.Split(new[] { "\n" }, StringSplitOptions.None);
                int[] indexSub = new int[10];
                int[] indexProg = new int[10];

                int num1 = 0;
                int num2 = 0;

                for (int line = 0; line < lines.Length; line++)
                {
                    if ((lines[line].Contains(" Dr ") || lines[line].Contains(" Mr ") || lines[line].Contains(" Mrs ") || lines[line].Contains(" Ms ") || lines[line].Contains(" Miss ")) && lines[line].Contains(":") == false)
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
                while (indexProg[run] != '\0')
                {

                    var subset = lines.ToList().GetRange(indexSub[run], indexProg[run] - indexSub[run] + 1);
                    Directory.CreateDirectory("data");
                    Directory.CreateDirectory("data/marks");
                    using (StreamWriter outputFile = new StreamWriter("data/marks/Subject" + run + ".txt"))
                    {
                        outputFile.Write(String.Join("", subset));
                    }

                    run++;
                }
                PutMarks();
                return true;
                
            }
            catch
            {
                Console.WriteLine("Chromedriver was unable to complete webscraping");
                driver.Quit();
                if (ShowError) 
                    MessageBox.Show("          Error connecting to Live Marks\n\nPlease ensure that you are:\n -connected to the internet\n -have inputed the correct username and password\n -not shutting Chrome\n\n          And then try again");
                return false;
            }
        }

    }
}