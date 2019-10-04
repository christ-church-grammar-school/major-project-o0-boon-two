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
    /// Interaction logic for Gym.xaml
    /// </summary>
    public partial class Gym : Page
    {
        List<Exercise> appendedWorkout = new List<Exercise>();
        GymWorkout addIt = new GymWorkout();
        ListObject listItem = new ListObject();
        List<Border> setsOfListItems = new List<Border>();
        List<Button> orderingDeleteExercise = new List<Button>();
    //    List<int> numberList = new List<int>();

        public Gym()
        {
            InitializeComponent();
            //InitiateAddPanel();
        }

        private void AddWorkout_Click(object sender, RoutedEventArgs e)
        {
            AddWorkoutPopup.Visibility = Visibility.Visible;

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
            AddExercisePopup.Visibility = Visibility.Visible;
            AddWorkoutPopup.Visibility = Visibility.Collapsed;
        }

        private void ExerciseEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            exerciseEntered.MaxLength = 30;
            if (exerciseEntered.Text.Length != 0 && repsEntered.Text.Length != 0 &&
                restEntered.Text.Length != 0 && setsEntered.Text.Length != 0)
            {
                addEntered.IsEnabled = true;
            }
            else
            {
                addEntered.IsEnabled = false;
            }
        }

        private void SetsEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            setsEntered.MaxLength = 3;
            if (exerciseEntered.Text.Length != 0 && repsEntered.Text.Length != 0 &&
                 restEntered.Text.Length != 0 && setsEntered.Text.Length != 0)
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
            repsEntered.MaxLength = 20;
            if (exerciseEntered.Text.Length != 0 && repsEntered.Text.Length != 0 &&
                 restEntered.Text.Length != 0 && setsEntered.Text.Length != 0)
            {
                addEntered.IsEnabled = true;
            }
            else
            {
                addEntered.IsEnabled = false;
            }
        }

        private void RestEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            restEntered.MaxLength = 15;
            if (exerciseEntered.Text.Length != 0 && repsEntered.Text.Length != 0 &&
                 restEntered.Text.Length != 0 && setsEntered.Text.Length != 0)
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
            Exercise newestWorkout = new Exercise();
            newestWorkout.exerciseName = exerciseEntered.Text.ToString();
            newestWorkout.reps = repsEntered.Text.ToString();
            newestWorkout.sets = setsEntered.Text.ToString();
            newestWorkout.rest = restEntered.Text.ToString();

            appendedWorkout.Add(newestWorkout);

            exerciseEntered.Text = "";
            repsEntered.Text = "";
            setsEntered.Text = "";
            restEntered.Text = "";

            AddExercisePopup.Visibility = Visibility.Collapsed;
            AddWorkoutPopup.Visibility = Visibility.Visible;
            ValidWorkout();



            TextBlock exerciseList = new TextBlock();
            
            exerciseList.Text = newestWorkout.exerciseName + " || " + newestWorkout.sets + " x "  + 
                                newestWorkout.reps + " || " + newestWorkout.rest + "\n";

            Button deleteThisExercise = new Button();
            deleteThisExercise.Height = 30;
            deleteThisExercise.Width = 30;
            deleteThisExercise.Content = "X";
            deleteThisExercise.Margin = new Thickness(0, -30,0,0);

            orderingDeleteExercise.Add(deleteThisExercise);
            
            //adding the exercise
            workoutStack.Children.Add(exerciseList);

            //adding a cross
            workoutStack.Children.Add(deleteThisExercise);
        }

       

        private void ClearText(object sender, KeyboardFocusChangedEventArgs e)
        {
                  if (namingWorkout.Text == "Enter Workout Name")
                  {
                      namingWorkout.Text = "";
                      namingWorkout.MaxLength = 38;
                      //ideally changes the text from a grey to black
                      //need to add a feature so it resets where length == 0
                  }
                  ValidWorkout();

        }

        //checks if the workout is valid
        private void ValidWorkout()
        {
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

        private void addCurrentWorkout(object sender, RoutedEventArgs e)
        {
                ValidWorkout();
                TextBlock setList = new TextBlock();
                TextBlock setTitle = new TextBlock(); 
                

                addIt.workoutName = namingWorkout.Text;
                addIt.exercises = appendedWorkout;
                setTitle.Text = addIt.workoutName;
                listItem.workoutTitle = setTitle;
                
                //to make it title cased
                listItem.workoutTitle.FontSize = 36;

                listItem.workoutList = setList;

                for (int i = 0; i < addIt.exercises.Count; i++)
                {
                    listItem.workoutList.Text += addIt.exercises[i].exerciseName;
                    listItem.workoutList.Text += "\n";
                    listItem.workoutList.Text += addIt.exercises[i].sets;
                    listItem.workoutList.Text += " X ";
                    listItem.workoutList.Text += addIt.exercises[i].reps;
                    listItem.workoutList.Text += " || ";
                    listItem.workoutList.Text += addIt.exercises[i].rest;
                    listItem.workoutList.Text += "\n";
                }

               Border expandingRectangles = new Border();

               Rectangle setRect = new Rectangle();
          
               listItem.workoutRect = setRect;
       
               listItem.workoutRect.Margin = new Thickness(0, 5, 0, 0);
               Style style = this.FindResource("rectangleYellowStyle") as Style;
               listItem.workoutRect.Height = (addIt.exercises.Count) * 32 + 55; //60, 50
               listItem.workoutList.Margin = new Thickness(50, (-32 * addIt.exercises.Count) - 5, 0, 0); // -50
               double x = listItem.workoutRect.Height; 
               listItem.workoutTitle.Margin = new Thickness(50, -x + 5 , 0, 0); // -x + 10
               listItem.workoutRect.Style = style;
               StackPanel listItemStack = new StackPanel();

               TextBlock itemNumber = new TextBlock();
               itemNumber.Width = 100;
               itemNumber.Height = 80;
               itemNumber.Text = (setsOfListItems.Count + 1).ToString();
               itemNumber.FontSize = 50;
               itemNumber.Margin = new Thickness(300, -100, 0, 0);
               //numberList.Add(Convert.ToInt32(itemNumber.Text));

          //     listItem.itemID = itemNumber;
               listItemStack.Children.Add(listItem.workoutRect);
               listItemStack.Children.Add(listItem.workoutTitle);
               listItemStack.Children.Add(listItem.workoutList);
       //     listItemStack.Children.Add(itemNumber);
             //  listItemStack.Children.Add(listItem.itemID);
               expandingRectangles.Child = listItemStack;
               style = this.FindResource("Expands") as Style;
               expandingRectangles.Style = style;

               workoutsPanel.Children.Add(expandingRectangles);
               workoutsPanel.Children.Add(itemNumber);

               AddWorkoutPopup.Visibility = Visibility.Collapsed;
               namingWorkout.Text = "";
               workoutStack.Children.Clear();

               setsOfListItems.Add(expandingRectangles);   
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
                addToWorkouts.IsEnabled = false;
            }
        }

        private void RemoveWorkout(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(workoutToDelete.Text.ToString()) > 0 && Convert.ToInt32(workoutToDelete.Text.ToString()) <= setsOfListItems.Count)
            {
                setsOfListItems.RemoveAt(Convert.ToInt32(workoutToDelete.Text.ToString()) - 1);
                deleteWorkout.Content = "Done!";
                workoutsPanel.Children.Clear();
                for (var i = 0; i < setsOfListItems.Count; i++)
                {
                    //setsOfListItems[i].Child[3].Text = (i+1).ToString();
             //       worko
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
            else
            {
                deleteWorkout.Content = "Invalid!";
            }

        }
    }
    public class GymWorkout
    {
        //will not change at any point
        public string workoutName;
        public List<Exercise> exercises = new List<Exercise>();
    }

    public class Exercise
    {
        public string exerciseName;
        public string sets;
        public string reps;
        public string rest;
    }

    public class ListObject
    {
        //public Border expandingRectangles;
        public TextBlock workoutList;
        public TextBlock workoutTitle;
        public Rectangle workoutRect;
       // public TextBlock itemID;
    }


}
