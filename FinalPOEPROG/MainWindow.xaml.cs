using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FinalPOEPROG.recipeWindow;

namespace FinalPOEPROG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBox();
            recipes = new List<Recipe>();
        }

        private void PopulateComboBox()
        {
            scaleComboBox.ItemsSource = recipes.Select(r => r.Name).ToList();
        }

        private void addRecipe_click(object sender, RoutedEventArgs e)
        {
            recipeWindow recipeWindow = new recipeWindow(recipes);
            recipeWindow.ShowDialog();
            displayRecipe_click(sender, e); // Refresh the displayed recipes
            PopulateComboBox(); // Refresh the ComboBox
        }

        private void displayRecipe_click(object sender, RoutedEventArgs e)
        {
            DisplayRecipes(recipes);
        }

        private void scaleButton_click(object sender, RoutedEventArgs e)
        {
            if (scaleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a recipe to scale.");
                return;
            }

            double scaleFactor = 1;
            if (scaleFactorHalfRadioButton.IsChecked == true)
            {
                scaleFactor = 0.5;
            }
            else if (scaleFactorTwoRadioButton.IsChecked == true)
            {
                scaleFactor = 2;
            }
            else if (scaleFactorThreeRadioButton.IsChecked == true)
            {
                scaleFactor = 3;
            }
            else
            {
                MessageBox.Show("Please select a scale factor.");
                return;
            }

            var selectedRecipeName = scaleComboBox.SelectedItem as string;
            var selectedRecipe = recipes.FirstOrDefault(r => r.Name == selectedRecipeName);

            if (selectedRecipe != null)
            {
                foreach (var ingredient in selectedRecipe.Ingredients)
                {
                    if (double.TryParse(ingredient.Quantity, out double quantity))
                    {
                        var scaledQuantity = quantity * scaleFactor;
                        ingredient.Quantity = scaledQuantity.ToString();
                    }
                }

                // refresh the displayed recipes
                displayRecipe_click(sender, e);
            }
        }

        private void resetRecipe_click(object sender, RoutedEventArgs e)
        {
            recipes.Clear();
            displayRecipe_click(sender, e); // Refresh the displayed recipes
            PopulateComboBox(); // Refresh the ComboBox
        }

        private void applyFilter_click(object sender, RoutedEventArgs e)
        {
            recipeListBox.Items.Clear();
            string ingredientName = ingredientNameFilterTextBox.Text.ToLower();
            string foodGroup = (foodGroupFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            bool maxCal = int.TryParse(maxCaloriesFilterTextBox.Text, out int maxCalories);

            var filteredRecipes = recipes.Where(r =>
                (string.IsNullOrWhiteSpace(ingredientName) || r.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientName))) &&
                (foodGroup == "All" || r.FoodGroup == foodGroup) &&
                (!maxCal || r.Calories <= maxCalories)
            ).ToList();

            DisplayRecipes(filteredRecipes);
        }
        private void DisplayRecipes(List<Recipe> recipesToDisplay)
        {

            recipeListBox.Items.Clear();

            foreach (var recipe in recipesToDisplay)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Name: {recipe.Name}");
                sb.AppendLine($"Number of Ingredients: {recipe.NumberOfIngredients}");
                int count = 0;
                foreach (var ingredient in recipe.Ingredients)
                {
                    count++;
                    sb.AppendLine($"Ingredient {count}: {ingredient.Quantity} {ingredient.Ing}(s) of {ingredient.Name}");
                }
                sb.AppendLine($"Calories: {recipe.Calories}");
                sb.AppendLine($"Food Group: {recipe.FoodGroup}");
                sb.AppendLine($"Steps: {recipe.Steps}");
                sb.AppendLine(new string('-', 50));

                recipeListBox.Items.Add(sb.ToString());
            }


        }

        private void exitButton_click(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }


}