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
    public partial class TasksPage : Page
    {
        /// OVERALL TASKS
        
        // implement keeping data after app is closed

        /// MINOR TASKS

        // fix task name text box text trimming

        /// Lists of TaskDatas
        public List<TaskData> HomeworkList = new List<TaskData>();
        public List<TaskData> TestsList = new List<TaskData>();
        public List<TaskData> AssignmentsList = new List<TaskData>();
        public List<TaskData> OtherList = new List<TaskData>();
        public List<TaskData> ummlist = new List<TaskData>();

        /// Other variables
        public EventHandler ladder;
        public string currentsection = "Homework";
        public int currentpos = 0;
        public TaskData Task = new TaskData();
        public double xratio;
        public double yratio;
        public int scrollpos1 = 0;
        public int scrollpos2 = 0;
        public int scrollpos3 = 0;
        public int scrollpos4 = 0;

        public TasksPage()
        {
            // Sets up that page
            InitializeComponent();

            UpdateTasks();
            CloseAddAnItem();
        }

        public void ClimbLadder()
        {
            ladder(this, EventArgs.Empty);
        }


        private void OpenAddAnItem()
        {
            // Creates add an item pop up window
            AddAnItemPopup.Visibility = Visibility.Visible;
        }

        private void CloseAddAnItem()
        {
            // Removes add an item pop up window
            AddAnItemPopup.Visibility = Visibility.Collapsed;

            // Resets typed content in the text boxes
            TaskNameTextBox.Text = "";
            TaskDateTextBox.Text = "";
            TaskMonthTextBox.Text = "";
            TaskYearTextBox.Text = "";

            // Removes error underlines for TaskNameTextBox
            ErrorUnderline1Part1.Width = 0;
            ErrorUnderline1Part2.Width = 0;
            ErrorUnderline1Part3.Width = 0;
            ErrorTextBlock1.Width = 0;
            ErrorTextBlock1.Text = "";

            // Removes error underlines for Task(Date/Month/Year)TextBoxes
            ErrorUnderline2Part1.Width = 0;
            ErrorUnderline2Part2.Width = 0;
            ErrorUnderline2Part3.Width = 0;
            ErrorTextBlock2.Width = 0;
            ErrorTextBlock2.Text = "";
        }


        private void DeleteTasks()
        {
            List<Ellipse> TaskCheckEllipses = new List<Ellipse> { TaskCheckEllipse1, TaskCheckEllipse2, TaskCheckEllipse3, TaskCheckEllipse4, TaskCheckEllipse5, TaskCheckEllipse6, TaskCheckEllipse7, TaskCheckEllipse8 };

            // Deletes any checked task
            for (var i = ummlist.Count - 1; i > -1; i -= 1)
            {
                if (ummlist[i].check)
                {
                    ummlist.RemoveAt(i);
                }
            }

            foreach (var TaskCheckEllipse in TaskCheckEllipses)
            {
                TaskCheckEllipse.Opacity = 0;
            }
        }

        private void UpdateTasks()
        {
            // Selects currently active section
            if (currentsection == "Homework")
            {
                ummlist = HomeworkList;
                currentpos = scrollpos1;
            }
            else if (currentsection == "Assignments")
            {
                ummlist = AssignmentsList;
                currentpos = scrollpos2;
            }
            else if (currentsection == "Tests")
            {
                ummlist = TestsList;
                currentpos = scrollpos3;
            }
            else if (currentsection == "Other")
            {
                ummlist = OtherList;
                currentpos = scrollpos4;
            }

            // Organises most recently added task into the list
            List<TaskData> thislist = new List<TaskData>();
            if (ummlist.Count > 1)
            {
                var addedtask = ummlist[ummlist.Count - 1];
                ummlist.RemoveAt(ummlist.Count - 1);

                List<int> datelist = new List<int>();
                foreach (var task in ummlist)
                {
                    datelist.Add(Convert.ToInt32(task.taskyear + task.taskmonth + task.taskdate));
                }

                var index = 0;
                foreach (var value in datelist)
                {
                    if (Convert.ToInt32(addedtask.taskyear + addedtask.taskmonth + addedtask.taskdate) <= value)
                    {
                        break;
                    }
                    index += 1;
                }

                ummlist.Insert(index, addedtask);
            }
            thislist = ummlist;

            // Updates all task objects
            if (thislist.Count >= 1)
            {
                TaskTextBlock1.Text = thislist[0].taskname + " || " + thislist[0].taskdate + "/" + thislist[0].taskmonth + "/" + thislist[0].taskyear;
                TaskRectangle1.Width = TaskTextBlock1.Text.Length * 13.5 + 65;
                TaskEllipse1.Width = 30;
                TaskCheckEllipse1.Width = 20;
                TaskCheck1.Width = 20;

                if (thislist[0].check)
                {
                    TaskCheckEllipse1.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse1.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock1.Text = "";
                TaskRectangle1.Width = 0;
                TaskEllipse1.Width = 0;
                TaskCheckEllipse1.Width = 0;
                TaskCheck1.Width = 0;
            }
            if (thislist.Count >= 2)
            {
                TaskTextBlock2.Text = thislist[currentpos + 1].taskname + " || " + thislist[currentpos + 1].taskdate + "/" + thislist[currentpos + 1].taskmonth + "/" + thislist[currentpos + 1].taskyear;
                TaskRectangle2.Width = TaskTextBlock2.Text.Length * 13.5 + 65;
                TaskEllipse2.Width = 30;
                TaskCheckEllipse2.Width = 20;
                TaskCheck2.Width = 20;

                if (thislist[currentpos + 1].check)
                {
                    TaskCheckEllipse2.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse2.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock2.Text = "";
                TaskRectangle2.Width = 0;
                TaskEllipse2.Width = 0;
                TaskCheckEllipse2.Width = 0;
                TaskCheck2.Width = 0;
            }
            if (thislist.Count >= 3)
            {
                TaskTextBlock3.Text = thislist[currentpos + 2].taskname + " || " + thislist[currentpos + 2].taskdate + "/" + thislist[2].taskmonth + "/" + thislist[currentpos + 2].taskyear;
                TaskRectangle3.Width = TaskTextBlock3.Text.Length * 13.5 + 65;
                TaskEllipse3.Width = 30;
                TaskCheckEllipse3.Width = 20;
                TaskCheck3.Width = 20;

                if (thislist[currentpos + 2].check)
                {
                    TaskCheckEllipse3.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse3.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock3.Text = "";
                TaskRectangle3.Width = 0;
                TaskEllipse3.Width = 0;
                TaskCheckEllipse3.Width = 0;
                TaskCheck3.Width = 0;
            }
            if (thislist.Count >= 4)
            {
                TaskTextBlock4.Text = thislist[currentpos + 3].taskname + " || " + thislist[currentpos + 3].taskdate + "/" + thislist[currentpos + 3].taskmonth + "/" + thislist[currentpos + 3].taskyear;
                TaskRectangle4.Width = TaskTextBlock4.Text.Length * 13.5 + 65;
                TaskEllipse4.Width = 30;
                TaskCheckEllipse4.Width = 20;
                TaskCheck4.Width = 20;

                if (thislist[currentpos + 3].check)
                {
                    TaskCheckEllipse4.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse4.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock4.Text = "";
                TaskRectangle4.Width = 0;
                TaskEllipse4.Width = 0;
                TaskCheckEllipse4.Width = 0;
                TaskCheck4.Width = 0;
            }
            if (thislist.Count >= 5)
            {
                TaskTextBlock5.Text = thislist[4].taskname + " || " + thislist[currentpos + 4].taskdate + "/" + thislist[currentpos + 4].taskmonth + "/" + thislist[currentpos + 4].taskyear;
                TaskRectangle5.Width = TaskTextBlock5.Text.Length * 13.5 + 65;
                TaskEllipse5.Width = 30;
                TaskCheckEllipse5.Width = 20;
                TaskCheck5.Width = 20;

                if (thislist[currentpos + 4].check)
                {
                    TaskCheckEllipse5.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse5.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock5.Text = "";
                TaskRectangle5.Width = 0;
                TaskEllipse5.Width = 0;
                TaskCheckEllipse5.Width = 0;
                TaskCheck5.Width = 0;
            }
            if (thislist.Count >= 6)
            {
                TaskTextBlock6.Text = thislist[currentpos + 5].taskname + " || " + thislist[currentpos + 5].taskdate + "/" + thislist[currentpos + 5].taskmonth + "/" + thislist[currentpos + 5].taskyear;
                TaskRectangle6.Width = TaskTextBlock6.Text.Length * 13.5 + 65;
                TaskEllipse6.Width = 30;
                TaskCheckEllipse6.Width = 20;
                TaskCheck6.Width = 20;

                if (thislist[currentpos + 5].check)
                {
                    TaskCheckEllipse6.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse6.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock6.Text = "";
                TaskRectangle6.Width = 0;
                TaskEllipse6.Width = 0;
                TaskCheckEllipse6.Width = 0;
                TaskCheck6.Width = 0;
            }
            if (thislist.Count >= 7)
            {
                TaskTextBlock7.Text = thislist[currentpos + 6].taskname + " || " + thislist[currentpos + 6].taskdate + "/" + thislist[currentpos + 6].taskmonth + "/" + thislist[currentpos + 6].taskyear;
                TaskRectangle7.Width = TaskTextBlock7.Text.Length * 13.5 + 65;
                TaskEllipse7.Width = 30;
                TaskCheckEllipse7.Width = 20;
                TaskCheck7.Width = 20;

                if (thislist[currentpos + 6].check)
                {
                    TaskCheckEllipse7.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse7.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock7.Text = "";
                TaskRectangle7.Width = 0;
                TaskEllipse7.Width = 0;
                TaskCheckEllipse7.Width = 0;
                TaskCheck7.Width = 0;
            }
            if (thislist.Count >= 8)
            {
                TaskTextBlock8.Text = thislist[currentpos + 7].taskname + " || " + thislist[currentpos + 7].taskdate + "/" + thislist[currentpos + 7].taskmonth + "/" + thislist[currentpos + 7].taskyear;
                TaskRectangle8.Width = TaskTextBlock8.Text.Length * 13.5 + 65;
                TaskEllipse8.Width = 30;
                TaskCheckEllipse8.Width = 20;
                TaskCheck8.Width = 20;

                if (thislist[currentpos + 7].check)
                {
                    TaskCheckEllipse8.Opacity = 1;
                }
                else
                {
                    TaskCheckEllipse8.Opacity = 0;
                }
            }
            else
            {
                TaskTextBlock8.Text = "";
                TaskRectangle8.Width = 0;
                TaskEllipse8.Width = 0;
                TaskCheckEllipse8.Width = 0;
                TaskCheck8.Width = 0;
            }
        }

        private void AddAnItemClicked(object sender, RoutedEventArgs e)
        {
            // Checks whether the maximum number of tasks have been reached
            if (currentsection == "Homework" && HomeworkList.Count < 8)
            {
                OpenAddAnItem();
            }
            else if (currentsection == "Assignments" && AssignmentsList.Count < 8)
            {
                OpenAddAnItem();
            }
            else if (currentsection == "Tests" && TestsList.Count < 8)
            {
                OpenAddAnItem();
            }
            else if (currentsection == "Other" && OtherList.Count < 8)
            {
                OpenAddAnItem();
            }
        }


        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            // Closes add an item pop up window
            CloseAddAnItem();
        }

        private void CreateButtonClicked(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Today;
            bool checker;

            // Creates error underline if task name not entered
            if (TaskNameTextBox.Text == "")
            {
                ErrorUnderline1Part1.Width = 1;
                ErrorUnderline1Part2.Width = 690;
                ErrorUnderline1Part3.Width = 1;
                ErrorTextBlock1.Width = 690;
                ErrorTextBlock1.Text = "task name not given";
                checker = true;
            }
            // Deletes error underline if task name is entered
            else
            {
                ErrorUnderline1Part1.Width = 0;
                ErrorUnderline1Part2.Width = 0;
                ErrorUnderline1Part3.Width = 0;
                ErrorTextBlock1.Width = 0;
                ErrorTextBlock1.Text = "";
                checker = false;
            }

            // Creates error underline if task(date/month/year) entered is wrong
            if (TaskDateTextBox.Text.Length != 2 || TaskMonthTextBox.Text.Length != 2 || TaskYearTextBox.Text.Length != 4)
            {
                ErrorUnderline2Part1.Width = 1;
                ErrorUnderline2Part2.Width = 345;
                ErrorUnderline2Part3.Width = 1;
                ErrorTextBlock2.Width = 345;
                ErrorTextBlock2.Text = "fields above have to be filled out as DD/MM/YYYY";
            }
            // Creates error underline if task(date/month/year) entered is not a valid date
            else if (Convert.ToInt32(TaskDateTextBox.Text) > 31 || Convert.ToInt32(TaskMonthTextBox.Text) > 12)
            {
                ErrorUnderline2Part1.Width = 1;
                ErrorUnderline2Part2.Width = 345;
                ErrorUnderline2Part3.Width = 1;
                ErrorTextBlock2.Width = 345;
                ErrorTextBlock2.Text = "invalid date and/or month";
            }
            // Creates error underline if task(date/month/year) entered is in the past
            else if (Convert.ToInt32(date.ToString("yyyyMMdd")) > Convert.ToInt32(TaskYearTextBox.Text + TaskMonthTextBox.Text + TaskDateTextBox.Text))
            {
                ErrorUnderline2Part1.Width = 1;
                ErrorUnderline2Part2.Width = 345;
                ErrorUnderline2Part3.Width = 1;
                ErrorTextBlock2.Width = 345;
                ErrorTextBlock2.Text = "can't create a task due in the past";
            }
            // Is called when no errors exist
            else
            {
                // Removes error underline for Task(Date/Month/Year)TextBoxes
                ErrorUnderline2Part1.Width = 0;
                ErrorUnderline2Part2.Width = 0;
                ErrorUnderline2Part3.Width = 0;
                ErrorTextBlock2.Width = 0;
                ErrorTextBlock2.Text = "";

                // Checks whether error underline for TaskNameTextBox exists
                if (checker == false)
                {
                    // Checks which list to add created task to
                    if (currentsection == "Homework")
                    {
                        HomeworkList.Add(new TaskData() { taskname = TaskNameTextBox.Text, taskdate = TaskDateTextBox.Text, taskmonth = TaskMonthTextBox.Text, taskyear = TaskYearTextBox.Text, check = false });
                    }
                    else if (currentsection == "Assignments")
                    {
                        AssignmentsList.Add(new TaskData() { taskname = TaskNameTextBox.Text, taskdate = TaskDateTextBox.Text, taskmonth = TaskMonthTextBox.Text, taskyear = TaskYearTextBox.Text, check = false });
                    }
                    else if (currentsection == "Tests")
                    {
                        TestsList.Add(new TaskData() { taskname = TaskNameTextBox.Text, taskdate = TaskDateTextBox.Text, taskmonth = TaskMonthTextBox.Text, taskyear = TaskYearTextBox.Text, check = false });
                    }
                    else if (currentsection == "Other")
                    {
                        OtherList.Add(new TaskData() { taskname = TaskNameTextBox.Text, taskdate = TaskDateTextBox.Text, taskmonth = TaskMonthTextBox.Text, taskyear = TaskYearTextBox.Text, check = false });
                    }

                    // Updates all task objects (for the task that was just added)
                    UpdateTasks();
                    // Closes add an item pop up window
                    CloseAddAnItem();
                }
            }
        }


        //All the clicked functions are for the removal from the displayed list
        private void Check1Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[0].check = !(ummlist[0].check);
            if (ummlist[0].check)
            {
                TaskCheckEllipse1.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse1.Opacity = 0;
            }
        }

        private void Check2Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[1].check = !(ummlist[1].check);
            if (ummlist[1].check)
            {
                TaskCheckEllipse2.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse2.Opacity = 0;
            }
        }

        private void Check3Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[2].check = !(ummlist[2].check);
            if (ummlist[2].check)
            {
                TaskCheckEllipse3.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse3.Opacity = 0;
            }
        }

        private void Check4Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[3].check = !(ummlist[3].check);
            if (ummlist[3].check)
            {
                TaskCheckEllipse4.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse4.Opacity = 0;
            }
        }

        private void Check5Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[4].check = !(ummlist[4].check);
            if (ummlist[4].check)
            {
                TaskCheckEllipse5.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse5.Opacity = 0;
            }
        }

        private void Check6Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[5].check = !(ummlist[5].check);
            if (ummlist[5].check)
            {
                TaskCheckEllipse6.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse6.Opacity = 0;
            }
        }

        private void Check7Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[6].check = !(ummlist[6].check);
            if (ummlist[6].check)
            {
                TaskCheckEllipse7.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse7.Opacity = 0;
            }
        }

        private void Check8Clicked(object sender, RoutedEventArgs e)
        {
            // Checks which section's task objects to update
            ummlist[7].check = !(ummlist[7].check);
            if (ummlist[7].check)
            {
                TaskCheckEllipse8.Opacity = 1;
            }
            else
            {
                TaskCheckEllipse8.Opacity = 0;
            }
        }


        private void HomeworkButtonClicked(object sender, RoutedEventArgs e)
        {
            // Switches to homework section
            DeleteTasks();
            UpdateTasks();
            Underline.Width = 103.5;
            Underline.Margin = new Thickness(HomeworkButton.Margin.Left + 1, 0, 0, HomeworkButton.Margin.Bottom - 5);
            currentsection = "Homework";
            UpdateTasks();
        }

        private void AssignmentsButtonClicked(object sender, RoutedEventArgs e)
        {
            // Switches to assignments section
            DeleteTasks();
            UpdateTasks();
            Underline.Width = 143;
            Underline.Margin = new Thickness(AssignmentsButton.Margin.Left + 1, 0, 0, AssignmentsButton.Margin.Bottom - 5);
            currentsection = "Assignments";
            UpdateTasks();
        }

        private void TestsButtonClicked(object sender, RoutedEventArgs e)
        {
            // Switches to tests section
            DeleteTasks();
            UpdateTasks();
            Underline.Width = 64;
            Underline.Margin = new Thickness(TestsButton.Margin.Left + 1, 0, 0, TestsButton.Margin.Bottom - 5);
            currentsection = "Tests";
            UpdateTasks();
        }

        private void OtherButtonClicked(object sender, RoutedEventArgs e)
        {
            // Switches to other section
            DeleteTasks();
            UpdateTasks();
            Underline.Width = 64;
            Underline.Margin = new Thickness(OtherButton.Margin.Left + 1, 0, 0, OtherButton.Margin.Bottom - 5);
            currentsection = "Other";
            UpdateTasks();
        }


        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var yratio = e.NewSize.Height / 764.5;
            var xratio = e.NewSize.Width / 1187;

            TaskRectangle1.Margin = new Thickness(xratio * 20, yratio * 20, 0, 0);
            TaskTextBlock1.Margin = new Thickness(xratio * 20 + 50, yratio * 20 + 10, 0, 0);
            TaskEllipse1.Margin = new Thickness(xratio * 20 + 10, yratio * 20 + 10, 0, 0);
            TaskCheckEllipse1.Margin = new Thickness(xratio * 20 + 15, yratio * 20 + 15, 0, 0);
            TaskCheck1.Margin = new Thickness(xratio * 20 + 15, yratio * 20 + 15, 0, 0);

            TaskRectangle2.Margin = new Thickness(xratio * 20, yratio * 100, 0, 0);
            TaskTextBlock2.Margin = new Thickness(xratio * 20 + 50, yratio * 100 + 10, 0, 0);
            TaskEllipse2.Margin = new Thickness(xratio * 20 + 10, yratio * 100 + 10, 0, 0);
            TaskCheckEllipse2.Margin = new Thickness(xratio * 20 + 15, yratio * 100 + 15, 0, 0);
            TaskCheck2.Margin = new Thickness(xratio * 20 + 15, yratio * 100 + 15, 0, 0);

            TaskRectangle3.Margin = new Thickness(xratio * 20, yratio * 180, 0, 0);
            TaskTextBlock3.Margin = new Thickness(xratio * 20 + 50, yratio * 180 + 10, 0, 0);
            TaskEllipse3.Margin = new Thickness(xratio * 20 + 10, yratio * 180 + 10, 0, 0);
            TaskCheckEllipse3.Margin = new Thickness(xratio * 20 + 15, yratio * 180 + 15, 0, 0);
            TaskCheck3.Margin = new Thickness(xratio * 20 + 15, yratio * 180 + 15, 0, 0);

            TaskRectangle4.Margin = new Thickness(xratio * 20, yratio * 260, 0, 0);
            TaskTextBlock4.Margin = new Thickness(xratio * 20 + 50, yratio * 260 + 10, 0, 0);
            TaskEllipse4.Margin = new Thickness(xratio * 20 + 10, yratio * 260 + 10, 0, 0);
            TaskCheckEllipse4.Margin = new Thickness(xratio * 20 + 15, yratio * 260 + 15, 0, 0);
            TaskCheck4.Margin = new Thickness(xratio * 20 + 15, yratio * 260 + 15, 0, 0);

            TaskRectangle5.Margin = new Thickness(xratio * 20, yratio * 340, 0, 0);
            TaskTextBlock5.Margin = new Thickness(xratio * 20 + 50, yratio * 340 + 10, 0, 0);
            TaskEllipse5.Margin = new Thickness(xratio * 20 + 10, yratio * 340 + 10, 0, 0);
            TaskCheckEllipse5.Margin = new Thickness(xratio * 20 + 15, yratio * 340 + 15, 0, 0);
            TaskCheck5.Margin = new Thickness(xratio * 20 + 15, yratio * 340 + 15, 0, 0);

            TaskRectangle6.Margin = new Thickness(xratio * 20, yratio * 420, 0, 0);
            TaskTextBlock6.Margin = new Thickness(xratio * 20 + 50, yratio * 420 + 10, 0, 0);
            TaskEllipse6.Margin = new Thickness(xratio * 20 + 10, yratio * 420 + 10, 0, 0);
            TaskCheckEllipse6.Margin = new Thickness(xratio * 20 + 15, yratio * 420 + 15, 0, 0);
            TaskCheck6.Margin = new Thickness(xratio * 20 + 15, yratio * 420 + 15, 0, 0);

            TaskRectangle7.Margin = new Thickness(xratio * 20, yratio * 500, 0, 0);
            TaskTextBlock7.Margin = new Thickness(xratio * 20 + 50, yratio * 500 + 10, 0, 0);
            TaskEllipse7.Margin = new Thickness(xratio * 20 + 10, yratio * 500 + 10, 0, 0);
            TaskCheckEllipse7.Margin = new Thickness(xratio * 20 + 15, yratio * 500 + 15, 0, 0);
            TaskCheck7.Margin = new Thickness(xratio * 20 + 15, yratio * 500, 0, 0);

            TaskRectangle8.Margin = new Thickness(xratio * 20, yratio * 580, 0, 0);
            TaskTextBlock8.Margin = new Thickness(xratio * 20 + 50, yratio * 580 + 10, 0, 0);
            TaskEllipse8.Margin = new Thickness(xratio * 20 + 10, yratio * 580 + 10, 0, 0);
            TaskCheckEllipse8.Margin = new Thickness(xratio * 20 + 15, yratio * 580 + 15, 0, 0);
            TaskCheck8.Margin = new Thickness(xratio * 20 + 15, yratio * 580 + 15, 0, 0);

            AddATaskButton.Margin = new Thickness(0, TaskRectangle8.Margin.Top - 30, 40, 0);

            HomeworkTextBlock.Margin = new Thickness(xratio * 50, 0, 0, yratio * 30);
            HomeworkButton.Margin = new Thickness(xratio * 50, 0, 0, yratio * 30);

            AssignmentsTextBlock.Margin = new Thickness(xratio * 375, 0, 0, yratio * 30);
            AssignmentsButton.Margin = new Thickness(xratio * 375, 0, 0, yratio * 30);

            TestsTextBlock.Margin = new Thickness(xratio * 750, 0, 0, yratio * 30);
            TestsButton.Margin = new Thickness(xratio * 750, 0, 0, yratio * 30);

            OtherTextBlock.Margin = new Thickness(xratio * 1050, 0, 0, yratio * 30);
            OtherButton.Margin = new Thickness(xratio * 1050, 0, 0, yratio * 30);

            if (currentsection == "Homework")
            {
                Underline.Margin = new Thickness(HomeworkButton.Margin.Left + 1, 0, 0, HomeworkButton.Margin.Bottom - 5);
            }
            else if (currentsection == "Assignments")
            {
                Underline.Margin = new Thickness(AssignmentsButton.Margin.Left + 1, 0, 0, AssignmentsButton.Margin.Bottom - 5);
            }
            else if (currentsection == "Tests")
            {
                Underline.Margin = new Thickness(TestsButton.Margin.Left + 1, 0, 0, TestsButton.Margin.Bottom - 5);
            }
            else if (currentsection == "Other")
            {
                Underline.Margin = new Thickness(OtherButton.Margin.Left + 1, 0, 0, OtherButton.Margin.Bottom - 5);
            }

            DarknessScreen.Width = xratio * 1200;
            DarknessScreen.Height = yratio * 800;

            AddATaskTopBorder.Margin = new Thickness((e.NewSize.Width - 1000) / 2, (e.NewSize.Height - 600) / 2, 0, 0);
            AddATaskBottomBorder.Margin = new Thickness((e.NewSize.Width - 1000) / 2, 0, 0, (e.NewSize.Height - 600) / 2);

            TaskNameTextBlock.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 40, AddATaskTopBorder.Margin.Top + 200, 0, 0);
            TaskNameRectangle.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 270, AddATaskTopBorder.Margin.Top + 200, 0, 0);
            TaskNameTextBox.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 287.75, AddATaskTopBorder.Margin.Top + 202.75, 0, 0);
            ErrorUnderline1Part1.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 270, AddATaskTopBorder.Margin.Top + 255, 0, 0);
            ErrorUnderline1Part2.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 270, AddATaskTopBorder.Margin.Top + 260, 0, 0);
            ErrorUnderline1Part3.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 959, AddATaskTopBorder.Margin.Top + 255, 0, 0);
            ErrorTextBlock1.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 270, AddATaskTopBorder.Margin.Top + 265, 0, 0);

            TaskDateTextBlock.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 40, AddATaskTopBorder.Margin.Top + 330, 0, 0);
            TaskDateRectangle.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 248, AddATaskTopBorder.Margin.Top + 330, 0, 0);
            TaskDateTextBox.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 265.75, AddATaskTopBorder.Margin.Top + 332.5, 0, 0);
            Slash1.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 333, AddATaskTopBorder.Margin.Top + 325, 0, 0);
            TaskMonthRectangle.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 361.5, AddATaskTopBorder.Margin.Top + 330, 0, 0);
            TaskMonthTextBox.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 379.25, AddATaskTopBorder.Margin.Top + 332.5, 0, 0);
            Slash2.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 445, AddATaskTopBorder.Margin.Top + 325, 0, 0);
            TaskYearRectangle.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 473, AddATaskTopBorder.Margin.Top + 330, 0, 0);
            TaskYearTextBox.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 490.75, AddATaskTopBorder.Margin.Top + 332.5, 0, 0);
            ErrorUnderline2Part1.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 248, AddATaskTopBorder.Margin.Top + 385, 0, 0);
            ErrorUnderline2Part2.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 248, AddATaskTopBorder.Margin.Top + 390, 0, 0);
            ErrorUnderline2Part3.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 592, AddATaskTopBorder.Margin.Top + 385, 0, 0);
            ErrorTextBlock2.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 248, AddATaskTopBorder.Margin.Top + 395, 0, 0);

            DeleteButton.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 40, AddATaskTopBorder.Margin.Top + 480, 0, 0);
            CreateButton.Margin = new Thickness(AddATaskTopBorder.Margin.Left + 760, AddATaskTopBorder.Margin.Top + 480, 0, 0);
        }


        public class TaskData
        {
            public string taskname;
            public string taskdate;
            public string taskmonth;
            public string taskyear;
            public bool check;
        }


        private void ShowTasksPage()
        {
            
        }

        private void ShowWellbeingPage()
        {
            
        }
        private void ShowGymPage()
        {
            
        }
        private void ShowLiveMarksPage()
        {
            
        }

        private void TasksClicked(object sender, RoutedEventArgs e)
        {
            ShowTasksPage();
        }

        private void WellbeingClicked(object sender, RoutedEventArgs e)
        {
            ShowWellbeingPage();
        }

        private void GymClicked(object sender, RoutedEventArgs e)
        {
            ShowGymPage();
        }

        private void LiveMarksClicked(object sender, RoutedEventArgs e)
        {
            ShowLiveMarksPage();
        }

        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Visible;
        }
    }
}
