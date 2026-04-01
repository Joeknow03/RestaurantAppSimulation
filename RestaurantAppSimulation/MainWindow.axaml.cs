using System;
using System.Collections.Generic;
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
        
        try
        {
            _server.GetDatabaseService().EnsureDatabaseCreated();
            DbStatusLabel.Text = "🗄️ Database: connected to PostgreSQL";
        }
        catch (Exception ex)
        {
            DbStatusLabel.Text = "🗄️ Database: not found " + ex.Message;
        }
        
        _server.ServingComplete += () =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                AddToResults(_server.LastCookResult);
                AddToResults("--- Serving food ---");
                AddToResults(_server.LastServeResult);
                AddToResults("💾 Session saved to PostgreSQL.");
                AddToResults("");

                StatusLabel.Text = "Status: done! Ready for next table.";
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
            StatusLabel.Text = "Status: Cook is preparing food...";

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
    
    private void RefreshHistoryButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            List<Session> sessions = _server.GetDatabaseService().GetAllSessions();

            HistoryCountLabel.Text = "Sessions in database: " + sessions.Count;
            HistoryBox.Text = "";

            if (sessions.Count == 0)
            {
                HistoryBox.Text = "No sessions found in database yet.\nPress Send to create one!";
                return;
            }

            foreach (Session session in sessions)
            {
                // Session header
                string duration = session.CompletedAt.HasValue
                    ? ((session.CompletedAt.Value - session.StartedAt).TotalSeconds).ToString("F1") + "s"
                    : "in progress";

                HistoryBox.Text += "══════════════════════════════\n";
                HistoryBox.Text += "Session #" + session.Id + " | " +
                                   session.StartedAt.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss") + "\n";
                HistoryBox.Text += "Cook: " + session.CookName + " | Duration: " + duration + "\n";
                HistoryBox.Text += "──────────────────────────────\n";

                // Customer orders
                HistoryBox.Text += "Orders:\n";
                foreach (CustomerOrder order in session.CustomerOrders)
                {
                    string drink = order.DrinkChoice == "No drink" ? "no drink" : order.DrinkChoice;
                    HistoryBox.Text += "  • " + order.CustomerName + ": " +
                                       order.ChickenCount + " chicken, " +
                                       order.EggCount + " egg, " +
                                       drink + "\n";
                }

                // Cooking result
                if (session.CookingResult != null)
                {
                    HistoryBox.Text += "Result: " +
                                       session.CookingResult.ChickensCooked + " chicken cooked, " +
                                       session.CookingResult.EggsCooked + " eggs cooked, " +
                                       session.CookingResult.RottenEggsFound + " rotten\n";
                }

                HistoryBox.Text += "\n";
            }
        }
        catch (Exception ex)
        {
            HistoryBox.Text = "Error loading history: " + ex.Message;
        }
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