using SQLite;
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
    // / <summary>
    // / Interaction logic for Gym.xaml
    // / </summary>
    public partial class Gym : Page
    {
        List<Exercise> appendedWorkout = new List<Exercise>();
        GymWorkout addIt = new GymWorkout();
        ListObject listItem = new ListObject();
        List<Border> setsOfListItems = new List<Border>();
        List<GymWorkout> workoutsList = new List<GymWorkout>();
        SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");


        public Gym()
        {
            InitializeComponent();
            // Create the table in SQLite Database
            conn.CreateTable<Exercise>();
        }

        private void AddWorkout_Click(object sender, RoutedEventArgs e)
        {
            // Cannot delete a workout while adding another
            deleteWorkout.IsEnabled = false;
            deleteWorkout.Height = 0;
            workoutToDelete.Height = 0;
            deleteTab.Height = 0;

            // Shifts page shown from gym to add workout
            AddWorkoutPopup.Visibility = Visibility.Visible;

            // To remove the residual exercises and textblocks of the last workout
            if (appendedWorkout.Count > 0)
            {
                appendedWorkout.Clear();
                if (workoutStack.Children.Count > 0)
                {
                    workoutStack.Children.RemoveAt(workoutStack.Children.Count - 1);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Adds a specific exercise
            AddExercisePopup.Visibility = Visibility.Visible;
            AddWorkoutPopup.Visibility = Visibility.Collapsed;
            repsEntered.Text = "12-12-12";
        }

        private void ExerciseEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Verifies that the exercise name entered is not too long
            exerciseEntered.MaxLength = 30;
            if (exerciseEntered.Text.Length != 0 && repsEntered.Text.Length != 0)
            {
                addEntered.IsEnabled = true;
            }
            else
            {
                addEntered.IsEnabled = false;
            }
        }

        private void RepsEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Makes sure that the reps entered are not too long or contains spacing
            repsEntered.MaxLength = 20;
            if ((exerciseEntered.Text.Length != 0 && repsEntered.Text.Length != 0) && repsEntered.Text.Contains(" ") == false)
            {
                addEntered.IsEnabled = true;
            }
            else
            {
                addEntered.IsEnabled = false;

            }
        }

        private void AddEntered_Click(object sender, RoutedEventArgs e)
        {
            // Adds EXERCISE to the WORKOUT
            Exercise newestWorkout = new Exercise();

            // Where _Entered are the textboxes
            newestWorkout.exerciseName = exerciseEntered.Text.ToString();
            newestWorkout.reps = repsEntered.Text.ToString();

            // Adding the 'Exercise' to appendedWorkout<Exercise>
            appendedWorkout.Add(newestWorkout);

            // Clears the textbox for next usage
            exerciseEntered.Text = "";
            repsEntered.Text = "";

            // Splits up the reps enter by the form x-x-x into ["x", "x", "x"]
            string[] setRep = newestWorkout.reps.Split('-');

            // Switches from EXERCISE page to the WORKOUT page
            AddExercisePopup.Visibility = Visibility.Collapsed;
            AddWorkoutPopup.Visibility = Visibility.Visible;

            // Makes sure its valid
            ValidWorkout();

            // Initiate 2 textblocks to visually represent in the XAML
            TextBlock exerciseList = new TextBlock();
            TextBlock exerciseName = new TextBlock();

            // Makes the exercise name "textblock" bold
            Run bold = new Run();
            bold.Text = newestWorkout.exerciseName; 
            bold.FontWeight = FontWeights.Bold;
            bold.FontSize = 20;
            exerciseName.Inlines.Add(bold);

            // The aesthetic backing of the text
            Rectangle yellowTextBack = new Rectangle();

            // The visual reps split up in the textblock
            exerciseList.Text += "REPS    ";
            foreach (var reps in setRep)
            {
                exerciseList.Text += reps;
                exerciseList.Text += "    ";
            }
            exerciseList.Text += "\n";

            // Style of the rectangle back that is under the textblocks
            Style textblock = this.FindResource("rectangleYellowStyle") as Style;
            yellowTextBack.Style = textblock;
            yellowTextBack.Height = 50;
            yellowTextBack.Width = 600;

            // Making it arrange in the stackpanel
            exerciseName.Margin = new Thickness(80, -50, 0, 0);
            exerciseList.Margin = new Thickness(80, -20, 0, 0);
            yellowTextBack.Margin = new Thickness(50, 0, 0, 0);

            // adding the exercise to the workout
            workoutStack.Children.Add(yellowTextBack);
            workoutStack.Children.Add(exerciseName);
            workoutStack.Children.Add(exerciseList);
        }

        private void ClearText(object sender, KeyboardFocusChangedEventArgs e)
        {
                  // Clears the text of the current workout name textbox when clicked on
                  // From "Enter Workout Name" >>> ""
                  if (namingWorkout.Text == "Enter Workout Name")
                  {
                      namingWorkout.Text = "";
                      namingWorkout.MaxLength = 38;
                  }
                  ValidWorkout();
        }

        private void ValidWorkout()
        {
            // Checks exercises and title are given
            if (namingWorkout.Text.Length > 0 && namingWorkout.Text != "Enter Workout Name")
            {
                if (appendedWorkout.Count == 0)
                {
                    addToWorkouts.IsEnabled = false;
                }
                else
                {
                    addToWorkouts.IsEnabled = true;
                }
            }
        }

        private void WorkoutsSavedToDB()
        {
            // Adds workout to DB
             foreach (GymWorkout x in workoutsList)
               {
                foreach (Exercise i in x.exercises)
                {
                    conn.Insert(i);
                }
            }
                        
               //  string sql = ;
             //    SQLiteCommand command = new SQLiteCommand(sql, conn);
             //    command.ExecuteNonQuery();

        }


        private void addCurrentWorkout(object sender, RoutedEventArgs e)
        {
                ValidWorkout();
                TextBlock setList = new TextBlock();
                TextBlock setTitle = new TextBlock();

            TextBlock exerciseName = new TextBlock();
          //   GymWorkout localWorkout = new GymWorkout();
            // localWorkout = addIt;

            Run bold = new Run();
            

            bold.FontWeight = FontWeights.Bold;
            bold.FontSize = 36;
            addIt.workoutName = namingWorkout.Text;
            bold.Text = addIt.workoutName.ToUpper();

            setTitle.Inlines.Add(bold);
                addIt.exercises = appendedWorkout;
                listItem.workoutTitle = setTitle;
                

                listItem.workoutList = setList;

                for (int i = 0; i < addIt.exercises.Count; i++)
                {
                    listItem.workoutList.Text += addIt.exercises[i].exerciseName;
                    listItem.workoutList.Text += "\n";
                    listItem.workoutList.Text += addIt.exercises[i].reps;
                    listItem.workoutList.Text += "\n";
                }

               Border expandingRectangles = new Border();

               Rectangle setRect = new Rectangle();
          
               listItem.workoutRect = setRect;
       
               listItem.workoutRect.Margin = new Thickness(0, 5, 0, 0);

            Style style = this.FindResource("rectangleYellowStyle") as Style;
               listItem.workoutRect.Height = (addIt.exercises.Count) * 32 + 65; // 60, 50
               listItem.workoutList.Margin = new Thickness(50, (-32 * addIt.exercises.Count) - 15, 0, 0);
            double x = listItem.workoutRect.Height; 
               listItem.workoutTitle.Margin = new Thickness(50, -x + 5 , 0, 0); //  -x + 10
               listItem.workoutRect.Style = style;
            listItem.workoutRect.Width = 800;
            workoutsPanel.Width = 850;
          StackPanel listItemStack = new StackPanel();

               TextBlock itemNumber = new TextBlock();
               itemNumber.Width = 100;
               itemNumber.Height = 80;
               itemNumber.Text = (setsOfListItems.Count + 1).ToString();
               itemNumber.FontSize = 50;
               itemNumber.Margin = new Thickness(300, -100, 0, 0);
 
               listItemStack.Children.Add(listItem.workoutRect);
               listItemStack.Children.Add(listItem.workoutTitle);
               listItemStack.Children.Add(listItem.workoutList);

               expandingRectangles.Child = listItemStack;
            expandingRectangles.Width = 800;

               workoutsPanel.Children.Add(expandingRectangles);
               workoutsPanel.Children.Add(itemNumber);

               AddWorkoutPopup.Visibility = Visibility.Collapsed;
               deleteWorkout.IsEnabled = true;
               deleteWorkout.Height = 50;
               workoutToDelete.Height = 50;
               deleteTab.Height = 50;
               namingWorkout.Text = "";
               workoutStack.Children.Clear();

               setsOfListItems.Add(expandingRectangles);
               workoutsList.Add(addIt);
               WorkoutsSavedToDB();
        }

        private void resetText(object sender, TextChangedEventArgs e)
        {
            if (namingWorkout.Text != "" && namingWorkout.Text != "Enter Workout Name")
            {
                addToWorkouts.IsEnabled = true;
                ValidWorkout();
            }
            else
            {
                try
                {
                    addToWorkouts.IsEnabled = false;
                }
                catch
                {
                    ValidWorkout();
                }
            }
        }

        private void RemoveWorkout(object sender, RoutedEventArgs e)
        {

                try
                {
                    setsOfListItems.RemoveAt(Convert.ToInt32(workoutToDelete.Text.ToString()) - 1);
                    deleteWorkout.Content = "Done!";
                    workoutsPanel.Children.Clear();
                    for (var i = 0; i < setsOfListItems.Count; i++)
                    {
                        workoutsPanel.Children.Add(setsOfListItems[i]);
                        TextBlock itemNumber = new TextBlock();
                        itemNumber.Width = 100;
                        itemNumber.Height = 80;
                        itemNumber.FontSize = 50;
                        itemNumber.Margin = new Thickness(300, -100, 0, 0);
                        itemNumber.Text = (i + 1).ToString();
                        workoutsPanel.Children.Add(itemNumber);
                    }
                }
                catch
                {
                    deleteWorkout.Content = "Invalid!";
                    
                }
      

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
    public class GymWorkout
    {
        // will not change at any point
        public string workoutName;
        public List<Exercise> exercises = new List<Exercise>();
    }

    [Table("GymExercises")]
    public class Exercise
    {
        //   public string workoutName;
        [PrimaryKey, Unique, AutoIncrement]
        public int ID { get; set; }

        public string WorkoutName { get; set; }
        public string exerciseName { get; set; }
        public string reps { get; set; }
    }

    public class ListObject
    {
        // public Border expandingRectangles;
        public TextBlock workoutList;
        public TextBlock workoutTitle;
        public Rectangle workoutRect;
       //  public TextBlock itemID;
    }


}
