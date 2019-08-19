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
        List<string> workout1 = new List<string>
        {
            "DB Bench 3 x 12 50 kg 4011",
            "DB Bench 3 x 12 50 kg 4011",
            "DB Bench 3 x 12 50 kg 4011",
            "DB Bench 3 x 12 50 kg 4011",
            "DB Bench 3 x 12 50 kg 4011"
        };
        List<GymWorkout> workouts = new List<GymWorkout>();
        GymWorkout workout = new GymWorkout();
        Button addButton = new Button();
        StackPanel textblockPanel = new StackPanel();
        StackPanel textboxPanel = new StackPanel();

        public Gym()
        {
            InitializeComponent();
            InitiateAddPanel();
        }
        public EventHandler ladder;
        public void ClimbLadder()
        {
            ladder(this, EventArgs.Empty);
        }
        //When adding workout update variable workout
        private void DataRecord()
        {
            workout.workoutName = "Workout 1";
            workout.exercises = workout1;
            workouts.Add(workout);
        }


        //adds a new component to workouts
        private void AddClicked(object sender, RoutedEventArgs e)
        {
            DataRecord();

            //important code that applies style templates
            Rectangle rectLine = new Rectangle();
            Style style = this.FindResource("rectangleYellowStyle") as Style;
            rectLine.Style = style;
            rectLine.Height = ((workout1.Count - 1) * 36) + 52;
            //(workout1.Count * 50) + 30;
            workoutsPanel.Children.Add(rectLine);

            TextBlock workoutName = new TextBlock();
            style = this.FindResource("textBlockListStyle") as Style;
            workoutName.Style = style;

            workoutName.Text = workout.workoutName;
            workoutName.FontSize = 36;
            workoutName.Margin = new Thickness(55, -((workout1.Count * 31) + 30), 0, 0);
            workoutName.Height = 36;


            TextBlock textLine = new TextBlock();
            style = this.FindResource("textBlockListStyle") as Style;
            textLine.Style = style;

            //+ workout.exercises.ToString();

            foreach (object exercise in workout.exercises)
            {
                textLine.Text += '\n';//System.Environment.NewLine;
                textLine.Text += exercise;
            }

            textLine.Margin = new Thickness(95, -(((workout1.Count) * 31) + 20), 0, 0);
            textLine.Height = workout1.Count * 31;// + 30; ;

            //workoutName.Text;
            workoutsPanel.Children.Add(workoutName);
            workoutsPanel.Children.Add(textLine);
            addButton.IsEnabled = false;
        }

        private void WorkoutPanel()
        {
            workoutsPanel.Width = 1040;
            addPanel.Width = 0;
            exercisePanel.Width = 0;
        }
        private void ExercisePanel()
        {
            workoutsPanel.Width = 0;
            addPanel.Width = 0;
            exercisePanel.Width = 1040;
        }
        private void AddPanel()
        {
            workoutsPanel.Width = 0;
            addPanel.Width = 1040;
            exercisePanel.Width = 0;

        }

        private void WorkoutButton(object sender, RoutedEventArgs e)
        {
            WorkoutPanel();
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            AddPanel();
       
        }
        private void ExerciseButton(object sender, RoutedEventArgs e)
        {
            ExercisePanel();
        }

        private void initiateTextblockPanel()
        {
            textblockPanel.Margin = new Thickness(180, 150, 0, 0);

            TextBlock exercise = new TextBlock();
            TextBlock sets = new TextBlock();
            TextBlock reps = new TextBlock();

            Style style = this.FindResource("textBlockListStyle") as Style;
            exercise.Style = style;
            exercise.Text = "Exercise";
            sets.Style = style;
            sets.Text = "Sets";
            reps.Style = style;
            reps.Text = "Reps";

            Rectangle exerciseRectangle = new Rectangle();
            Rectangle setsRectangle = new Rectangle();
            Rectangle repsRectangle = new Rectangle();

            style = this.FindResource("rectangleYellowStyle") as Style;
            exerciseRectangle.Style = style;
            exerciseRectangle.Width = (exercise.Text.Length * 13.5) + 30;
            setsRectangle.Style = style;
            setsRectangle.Width = (sets.Text.Length * 13.5) + 30;
            repsRectangle.Style = style;
            repsRectangle.Width = (reps.Text.Length * 13.5) + 30;

            exercise.Margin = new Thickness(15, -220, 0, 0);
            sets.Margin = new Thickness(15, -130, 0, 0);
            reps.Margin = new Thickness(15, -40, 0, 0);

            exerciseRectangle.Margin = new Thickness(0, 40, 0, 0);
            setsRectangle.Margin = new Thickness(0, 40, 0, 0);
            repsRectangle.Margin = new Thickness(0, 40, 0, 0);

            textblockPanel.Children.Add(exerciseRectangle);
            textblockPanel.Children.Add(setsRectangle);
            textblockPanel.Children.Add(repsRectangle);
            textblockPanel.Children.Add(exercise);
            textblockPanel.Children.Add(sets);
            textblockPanel.Children.Add(reps);
        }

        private void initiateTextboxPanel()
        {
            textboxPanel.Margin = new Thickness(50, 150, 0, 0);

            TextBox exerciseEntered = new TextBox();
            ComboBox setsEntered = new ComboBox();
            ComboBox repsEntered = new ComboBox();

            Rectangle exerciseTextbox = new Rectangle();
            Rectangle setsCombobox = new Rectangle();
            Rectangle repsCombobox = new Rectangle();

            Style style = this.FindResource("yellowTextbox") as Style;
            exerciseEntered.Style = style;

            style = this.FindResource("rectangleYellowStyle") as Style;
            exerciseTextbox.Style = style;
            setsCombobox.Style = style;
            repsCombobox.Style = style;

            exerciseEntered.Margin = new Thickness(30, 0, 0, 0);

            textboxPanel.Children.Add(exerciseEntered);
            textboxPanel.Children.Add(setsEntered);
            textboxPanel.Children.Add(repsEntered);
            textboxPanel.Children.Add(exerciseTextbox);
            textboxPanel.Children.Add(setsCombobox);
            textboxPanel.Children.Add(repsCombobox);
        }

        private void InitiateAddPanel()
        {
            initiateTextblockPanel();
            initiateTextboxPanel();

            //add to the right of the gym panel
            //addButton.Content = "Add workout";
            //addButton.Click += new RoutedEventHandler(AddClicked);

            //textblockPanel.Children.Add(addButton);
            addPanel.Children.Add(textblockPanel);
            addPanel.Children.Add(textboxPanel);
        }

    }
    public class GymWorkout
    {
        //will not change at any point
        public string workoutName;
        public List<string> exercises = new List<string>();
    }
}
