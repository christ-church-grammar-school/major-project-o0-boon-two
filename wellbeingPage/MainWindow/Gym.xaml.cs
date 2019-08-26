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
        //List<string> workout1 = new List<string>
        //{
        //    "DB Bench 3 x 12 50 kg 4011",
        //    "DB Bench 3 x 12 50 kg 4011",
        //    "DB Bench 3 x 12 50 kg 4011",
        //    "DB Bench 3 x 12 50 kg 4011",
        //    "DB Bench 3 x 12 50 kg 4011"
        //};
        //List<GymWorkout> workouts = new List<GymWorkout>();
        //GymWorkout workout = new GymWorkout();
        //Button addButton = new Button();
        //StackPanel textblockPanel = new StackPanel();
        //StackPanel textboxPanel = new StackPanel();

        List<Exercise> appendedWorkout = new List<Exercise>();
        Exercise newestWorkout = new Exercise();
        GymWorkout addIt = new GymWorkout();
        TextBlock workoutList = new TextBlock();


        public Gym()
        {
            InitializeComponent();
            //InitiateAddPanel();
        }

        private void AddWorkout_Click(object sender, RoutedEventArgs e)
        {
            AddWorkoutPopup.Visibility = Visibility.Visible;
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

            workoutStack.Children.Add(exerciseList);
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
                addIt.workoutName = namingWorkout.Text;
                addIt.exercises = appendedWorkout;
                workoutList.Text = addIt.workoutName;
                workoutList.Text += "\n";
                for (int i = 0; i < addIt.exercises.Count; i++)
                {
                    workoutList.Text += addIt.exercises[i].exerciseName;
                    workoutList.Text += "\n";
                    workoutList.Text += addIt.exercises[i].sets;
                    workoutList.Text += " X ";
                    workoutList.Text += addIt.exercises[i].reps;
                    workoutList.Text += " || ";
                    workoutList.Text += addIt.exercises[i].rest;
                    workoutList.Text += "\n";
                }
   
                workoutsPanel.Children.Add(workoutList);
                AddWorkoutPopup.Visibility = Visibility.Collapsed;
        }

        private void resetText(object sender, TextChangedEventArgs e)
        {
            if (namingWorkout.Text != "")
            {
                addToWorkouts.IsEnabled = true;
                ValidWorkout();
            }
            else
            {
                addToWorkouts.IsEnabled = false;
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
}
