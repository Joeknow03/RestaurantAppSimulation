using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RestaurantAppSimulation;

public partial class MainWindow : Window
{
    private Server server = new Server();
    private bool foodSentToCook = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ReceiveButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            int chickenQty = (int)(ChickenQty.Value ?? 0);
            int eggQty = (int)(EggQty.Value ?? 0);

            MenuItem drink = GetSelectedDrink();

            string result = server.Receive(chickenQty, eggQty, drink);
            AddToResults(result);

            CustomerCountLabel.Text = "Customers at table: " + server.GetCustomerCount() + " / 8";

            ChickenQty.Value = 0;
            EggQty.Value = 0;
            DrinkCombo.SelectedIndex = 0;
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
            if (server.GetCustomerCount() == 0)
            {
                AddToResults("No customers have ordered yet!");
                return;
            }

            AddToResults("--- Sending orders to Cook ---");
            string result = server.Send();
            AddToResults(result);

            foodSentToCook = true;
            EggQualityLabel.Text = "Egg Quality: food is being prepared by Cook...";
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
            if (!foodSentToCook)
            {
                AddToResults("Food hasn't been sent to the Cook yet!");
                return;
            }

            AddToResults("--- Serving food ---");
            string result = server.Serve();
            AddToResults(result);
            AddToResults("");

            foodSentToCook = false;
            CustomerCountLabel.Text = "Customers at table: 0 / 8";
            EggQualityLabel.Text = "Egg Quality: -";
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

    private MenuItem GetSelectedDrink()
    {
        switch (DrinkCombo.SelectedIndex)
        {
            case 0: return MenuItem.Tea;
            case 1: return MenuItem.CocaCola;
            case 2: return MenuItem.Pepsi;
            default: return MenuItem.NoDrink;
        }
    }

    private void AddToResults(string text)
    {
        ResultsBox.Text = ResultsBox.Text + text + "\n";
        ResultsBox.CaretIndex = ResultsBox.Text?.Length ?? 0;
    }
}