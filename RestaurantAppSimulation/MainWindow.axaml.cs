using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RestaurantAppSimulation;

public partial class MainWindow : Window
{
    private Server _server = new Server();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ReceiveButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            int customerNumber = (int)(CustomerNumber.Value ?? 0);
            int chickenQty = (int)(ChickenQty.Value ?? 0);
            int eggQty = (int)(EggQty.Value ?? 0);
            string drinkChoice = GetSelectedDrink();

            string result = _server.Receive(customerNumber, chickenQty, eggQty, drinkChoice);
            AddToResults(result);

            CustomerCountLabel.Text = "Customers at table: " + _server.GetCustomerCount();

            // Reset inputs for next customer
            ChickenQty.Value = 0;
            EggQty.Value = 0;
            DrinkCombo.SelectedIndex = 0;

            // Increment customer number for convenience
            if (CustomerNumber.Value < 7)
            {
                CustomerNumber.Value++;
            }

            // Show egg quality info if eggs were ordered
            if (eggQty > 0)
            {
                EggQualityLabel.Text = "Egg Quality: check results (some may be hidden)";
            }
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
        }
    }

    private void SendButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (_server.GetCustomerCount() == 0)
            {
                AddToResults("No customers have ordered yet!");
                return;
            }

            string result = _server.Send();
            AddToResults(result);

            EggQualityLabel.Text = "Egg Quality: food prepared by Cook";
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
        }
    }

    private void ServeButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            AddToResults("--- Serving food ---");
            string result = _server.Serve();
            AddToResults(result);
            AddToResults("");

            CustomerCountLabel.Text = "Customers at table: 0";
            EggQualityLabel.Text = "Egg Quality: -";
            CustomerNumber.Value = 0;
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
        }
    }

    private void ClearButton_Click(object? sender, RoutedEventArgs e)
    {
        ResultsBox.Text = "";
    }

    private string GetSelectedDrink()
    {
        switch (DrinkCombo.SelectedIndex)
        {
            case 0: return "Tea";
            case 1: return "Coca Cola";
            case 2: return "Pepsi";
            default: return "No drink";
        }
    }

    private void AddToResults(string text)
    {
        ResultsBox.Text = ResultsBox.Text + text + "\n";
        ResultsBox.CaretIndex = ResultsBox.Text?.Length ?? 0;
    }
}