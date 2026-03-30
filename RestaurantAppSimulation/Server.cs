using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantAppSimulation;

public class Server
{
    private TableRequests _tableRequests = new TableRequests();
    private Cook[] _cooks = new Cook[]
    {
        new Cook("Cook Alice"),
        new Cook("Cook Bob")
    };
 
    private object _lockObject = new object();
    private int _nextCookIndex = 0;
    public string LastCookResult { get; private set; } = "";
    public string LastServeResult { get; private set; } = "";
    public event Action? ServingComplete;
    
    public string Receive(string customerName, int chickenQty, int eggQty, string drinkChoice)
    {
        // lock: only one thread can execute this block at a time
        lock (_lockObject)
        {
            for (int i = 0; i < chickenQty; i++)
                _tableRequests.Add<Chicken>(customerName);
 
            for (int i = 0; i < eggQty; i++)
                _tableRequests.Add<Egg>(customerName);
 
            if (drinkChoice == "Tea")         _tableRequests.Add<Tea>(customerName);
            else if (drinkChoice == "Coca Cola") _tableRequests.Add<CocaCola>(customerName);
            else if (drinkChoice == "Pepsi")     _tableRequests.Add<Pepsi>(customerName);
 
            string drinkDisplay = drinkChoice == "No drink" ? "no drink" : drinkChoice;
            return " " + customerName + ": " + chickenQty + " chicken, " + eggQty + " egg, " + drinkDisplay;
        }
    }
    
    public void Send()
    {
        TableRequests requestsSnapshot;
        Cook selectedCook;
 
        lock (_lockObject)
        {
            requestsSnapshot = _tableRequests;
            _tableRequests = new TableRequests(); // reset for next table
 
            // Round-robin: take turns between cooks
            selectedCook = _cooks[_nextCookIndex];
            _nextCookIndex = (_nextCookIndex + 1) % _cooks.Length;
        }
        
        Task<string> cookTask = Task.Run(() =>
        {
            return selectedCook.Process(requestsSnapshot);
        });
        
        cookTask.ContinueWith(completedCookTask =>
        {
            lock (_lockObject)
            {
                LastCookResult = completedCookTask.Result;
                Thread.Sleep(500);
                LastServeResult = ServeWithLinq(requestsSnapshot);
            }
            ServingComplete?.Invoke();
        });
    }
    // ==============================
    private string ServeWithLinq(TableRequests requests)
    {
        string result = "";
        
        List<string> sortedNames = requests.CustomerNames
            .OrderBy(name => name)   
            .ToList();               
 
        foreach (string customerName in sortedNames)
        {
            List<IMenuItem> items = requests[customerName];
            int chickenCount = items.Count(item => item is Chicken);
            int eggCount     = items.Count(item => item is Egg);
            int drinkCount   = items.Count(item => item is Drink);
            
            Drink? drink = items.FirstOrDefault(item => item is Drink) as Drink;
            if (drink != null) drink.Obtain(); 
            result += customerName + " ordered " +
                      drinkCount + " drink, " +
                      eggCount + " egg and " +
                      chickenCount + " chicken\n";
        }
        result += "Please enjoy your food!";
        return result;
    }
 
    public int GetCustomerCount() => _tableRequests.CustomerCount;
}