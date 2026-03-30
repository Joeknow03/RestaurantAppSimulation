using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace RestaurantAppSimulation;

public partial class MainWindow : Window
{
    private Server _server = new Server();

    public MainWindow()
    {
        InitializeComponent();

    _server.ServingComplete += () =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                AddToResults(_server.LastCookResult);
                AddToResults("--- Serving food ---");
                AddToResults(_server.LastServeResult);
                AddToResults("");

                StatusLabel.Text = "Status: Done! Ready for next table.";
                CustomerCountLabel.Text = "Customers at table: 0";
                SendButton.IsEnabled = true;
                ReceiveButton.IsEnabled = true;
            });
        };
    }

    private void ReceiveButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            string customerName = CustomerNameInput.Text?.Trim() ?? "";

            if (customerName == "")
            {
                AddToResults("Please enter a customer name!");
                return;
            }

            int chickenQty = (int)(ChickenQty.Value ?? 0);
            int eggQty = (int)(EggQty.Value ?? 0);
            string drinkChoice = GetSelectedDrink();

            string result = _server.Receive(customerName, chickenQty, eggQty, drinkChoice);
            AddToResults(result);

            CustomerCountLabel.Text = "Customers at table: " + _server.GetCustomerCount();

            CustomerNameInput.Text = "";
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
            if (_server.GetCustomerCount() == 0)
            {
                AddToResults("No customers have ordered yet!");
                return;
            }

            SendButton.IsEnabled = false;
            ReceiveButton.IsEnabled = false;
            StatusLabel.Text = "Status: Cook is preparing food in background...";

            AddToResults("--- Sending to Cook ---");
            _server.Send();
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
            SendButton.IsEnabled = true;
            ReceiveButton.IsEnabled = true;
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