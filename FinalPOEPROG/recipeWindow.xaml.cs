using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinalPOEPROG
{
    /// <summary>
    /// Interaction logic for recipeWindow.xaml
    /// </summary>
    public partial class recipeWindow : Window
    {
        public class Ingredient
        {
            public string Name { get; set; }
            public string Quantity { get; set; }

            public string Ing { get; set; }
        }
        public class Recipe
        {
            public string Name { get; set; }
            public string Name2 { get; set; }
            public int NumberOfIngredients => Ingredients.Count;
            public List<Ingredient> Ingredients { get; set; }
            public int Calories { get; set; }
            public string FoodGroup { get; set; }
            public string Steps { get; set; }
        }

        private List<Recipe> recipes;
        private List<Ingredient> currentIngredients = new List<Ingredient>();
        private int ingredientsToAdd;



        public recipeWindow(List<Recipe> recipes)//passing the list through method
        {
            InitializeComponent();
            this.recipes = recipes;
        }

        private void saveRecipe_click(object sender, RoutedEventArgs e)
        {
            int cal = int.TryParse(CaloriesTextBox.Text, out cal) ? cal : 0;
            if (cal > 300)
            {
                calorieDelegate(cal);       //implementing delegate
            }

            var recipe = new Recipe
            {
                Name = RecipeNameTextBox.Text,

                Ingredients = new List<Ingredient>(currentIngredients),
                Calories = int.TryParse(CaloriesTextBox.Text, out int calories) ? calories : 0 ,
                FoodGroup = (FoodGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Steps = stepsBox.Text
            };

            recipes.Add(recipe);
            ClearInputFields();
            MessageBox.Show("Recipe saved successfully!");
        }
        private void calorieDelegate(int cal)
        {
            MessageBox.Show("!!!CALORIES EXCEEDED 300 !!! "+cal);
        }

        private void ClearInputFields()
        {
            RecipeNameTextBox.Text = string.Empty;
            NumIngredientsTextBox.Text = string.Empty;
            ingBox.Text = string.Empty;
            ingredientBox.Text = string.Empty;
            quantityBox.Text = string.Empty;
            CaloriesTextBox.Text = string.Empty;
            FoodGroupComboBox.SelectedIndex = -1;
            stepsBox.Text = string.Empty;
            IngredientsListBox.Items.Clear();
            currentIngredients.Clear();
        }

        private void showRecipes_click(object sender, RoutedEventArgs e)
        {
            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var recipe in recipes)
            {
                sb.AppendLine($"Name: {recipe.Name}");
                sb.AppendLine($"Number of Ingredients: {recipe.NumberOfIngredients}");
                int count = 0;
                foreach (var ingredient in recipe.Ingredients)
                {
                    count++;
                    sb.AppendLine($"Ingredient {count}: {ingredient.Quantity} {ingredient.Ing} of {ingredient.Name}");
                }
                sb.AppendLine($"Calories: {recipe.Calories}");
                sb.AppendLine($"Food Group: {recipe.FoodGroup}");
                sb.AppendLine($"Steps: {recipe.Steps}");
                sb.AppendLine(new string('-', 50));
            }

            MessageBox.Show(sb.ToString(), "Recipes");
        }

        private void saveIngredient_click(object sender, RoutedEventArgs e)
        {

            if (currentIngredients.Count < ingredientsToAdd)
            {
                var ingredient = new Ingredient
                {
                    Name = ingredientBox.Text,
                    Quantity = quantityBox.Text,
                    Ing = ingBox.Text,
                };
                currentIngredients.Add(ingredient);
                IngredientsListBox.Items.Add($"{ingredient.Quantity} - {ingredient.Ing} of {ingredient.Name}");
                ingBox.Text = string.Empty;
                quantityBox.Text = string.Empty;
                ingredientBox.Text = string.Empty;


                if (currentIngredients.Count == ingredientsToAdd)
                {
                    MessageBox.Show("All ingredients added.");
                }
            }
            else
            {
                MessageBox.Show("All specified ingredients have already been added.");
            }


        }
       

        private void NumIngredientsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumIngredientsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumIngredientsTextBox.Text, out int numIngredients))
            {
                ingredientsToAdd = numIngredients;
                currentIngredients.Clear();
                IngredientsListBox.Items.Clear();
            }
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            Close();

        }
    }

}
