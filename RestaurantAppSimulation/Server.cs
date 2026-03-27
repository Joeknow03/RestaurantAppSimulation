using System;
using System.Collections.Generic;

namespace RestaurantAppSimulation;

public class Server
{
    private TableRequests _tableRequests = new TableRequests();
    private Cook _cook = new Cook();
 
    // Stores the result from cooking so the UI can display it
    public string LastCookResult { get; private set; } = "";
    public string LastServeResult { get; private set; } = "";
    public event EventHandler? Ready;
    
    public Server()
    {
        Ready += (sender, e) =>
        {
            _cook.SetRequests(_tableRequests);
            LastCookResult = _cook.OnServerReady(sender, e);
        };
        
        _cook.Processed += (sender, e) =>
        {
            LastServeResult = ServeFood();
        };
    }
    
    public string Receive(string customerName, int chickenQty, int eggQty, string drinkChoice)
    {
        // Add one Chicken object per chicken ordered
        for (int i = 0; i < chickenQty; i++)
        {
            _tableRequests.Add<Chicken>(customerName);
        }
 
        // Add one Egg object per egg ordered
        for (int i = 0; i < eggQty; i++)
        {
            _tableRequests.Add<Egg>(customerName);
        }
 
        // Add the drink
        if (drinkChoice == "Tea")
            _tableRequests.Add<Tea>(customerName);
        else if (drinkChoice == "Coca Cola")
            _tableRequests.Add<CocaCola>(customerName);
        else if (drinkChoice == "Pepsi")
            _tableRequests.Add<Pepsi>(customerName);
        // "No drink" = nothing added
 
        string drinkDisplay = drinkChoice == "No drink" ? "no drink" : drinkChoice;
        return " " + customerName + ": " + chickenQty + " chicken, " + eggQty + " egg, " + drinkDisplay;
    }
    
    public void Send()
    {
        Ready?.Invoke(this, EventArgs.Empty);
    }
    
    private string ServeFood()
    {
        string result = "";
        
        foreach (IMenuItem item in _tableRequests)
        {
            if (item is Drink)
            {
                item.Obtain(); // pour the drink
            }
        }
        
        foreach (string customerName in _tableRequests.CustomerNames)
        {
            List<IMenuItem> items = _tableRequests[customerName];
 
            int chickenCount = 0;
            int eggCount = 0;
            string drinkName = "no drink";
 
            foreach (IMenuItem item in items)
            {
                if (item is Chicken) chickenCount++;
                else if (item is Egg) eggCount++;
                else if (item is Drink) drinkName = item.Name;
            }
 
            result += customerName + " is served " +
                      chickenCount + " chicken, " +
                      eggCount + " egg, " +
                      drinkName + "\n";
        }
 
        result += "Please enjoy your food!";
 
        // Reset for next table
        _tableRequests = new TableRequests();
 
        return result;
    }
 
    public int GetCustomerCount() => _tableRequests.CustomerCount;
}