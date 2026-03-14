using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RestaurantAppSimulation;

public partial class MainWindow : Window
{
    private Employee employee = new Employee();
    private object? currentOrder = null;
    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void SubmitButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            string menuItem = "Chicken";
            if (EggRadio.IsChecked == true)
            {
                menuItem = "Egg";
            }
            
            int quantity = (int)(QuantityInput.Value ?? 1);
            currentOrder = employee.NewRequest(menuItem, quantity);
            
            string actualItem = currentOrder is ChickenOrder ? "Chicken 🐔" : "Egg 🥚";
            string inspectionResult = employee.Inspect(currentOrder);
            
            AddToResults("You asked for: " + menuItem + " x" + quantity);
            AddToResults("Employee got:  " + actualItem + " x" + quantity);
            AddToResults(inspectionResult);
            AddToResults(""); 

            UpdateEggQualityLabel();
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
            AddToResults("");
        }
    }

    private void CopyButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            currentOrder = employee.CopyRequest();
            string actualItem = currentOrder is ChickenOrder ? "Chicken" : "Egg";
            string inspectionResult = employee.Inspect(currentOrder);

            AddToResults("📋 Copied previous request: " + actualItem);
            AddToResults(inspectionResult);
            AddToResults("");
            
            UpdateEggQualityLabel();
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
            AddToResults("");
        }
    }
    
    private void PrepareButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (currentOrder == null)
            {
                AddToResults("ERROR: No order to prepare! Submit a request first.");
                AddToResults("");
                return;
            }

            string result = employee.PrepareFood(currentOrder);

            AddToResults(result);
            AddToResults("");
        }
        catch (Exception ex)
        {
            AddToResults("ERROR: " + ex.Message);
            AddToResults("");
        }
    }
    
    private void ClearButton_Click(object? sender, RoutedEventArgs e)
    {
        ResultsBox.Text = "";
    }

    private void AddToResults(string text)
    {
        ResultsBox.Text = ResultsBox.Text + text + "\n";
        ResultsBox.CaretIndex = ResultsBox.Text?.Length ?? 0;
    }

    private void UpdateEggQualityLabel()
    {
        if (currentOrder is EggOrder eggOrder)
        {
            int? quality = eggOrder.GetQuality();

            if (quality == null)
            {
                // Employee forgot to check!
                EggQualityLabel.Text = "Egg Quality: ";
            }
            else
            {
                // Show the quality with a warning if it's low
                string warning = quality < 25 ? "ROTTEN!" : "";
                EggQualityLabel.Text = "Egg Quality: " + quality + warning;
            }
        }
        else
        {
            EggQualityLabel.Text = "Egg Quality: N/A (chicken order)";
        }
    }
}