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
    
    private DataBaseService _db = new DataBaseService();
    private int _currentSessionId = -1;
    private Dictionary<string, (int chicken, int egg, string drink)> _pendingOrders
        = new Dictionary<string, (int, int, string)>();
    
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
            _pendingOrders[customerName] = (chickenQty, eggQty, drinkChoice);
            return " " + customerName + ": " + chickenQty + " chicken, " + eggQty + " egg, " + drinkDisplay;
        }
    }
    
    public void Send()
    {
        TableRequests requestsSnapshot;
        Cook selectedCook;
        Dictionary<string, (int chicken, int egg, string drink)> ordersSnapshot; // ← добавь это
        int sessionId = -1;
 
        lock (_lockObject)
        {
            requestsSnapshot = _tableRequests;
            _tableRequests = new TableRequests(); // reset for next table
 
            // Round-robin: take turns between cooks
            selectedCook = _cooks[_nextCookIndex];
            _nextCookIndex = (_nextCookIndex + 1) % _cooks.Length;
            ordersSnapshot = _pendingOrders;
            _pendingOrders = new Dictionary<string, (int, int, string)>();

            _currentSessionId = _db.StartSession(selectedCook.Name);

            foreach (var entry in ordersSnapshot)
            {
                _db.SaveCustomerOrder(_currentSessionId, entry.Key,
                    entry.Value.chicken, entry.Value.egg, entry.Value.drink);
            }
        }
        
        Task<string> cookTask = Task.Run(() =>
        {
            return selectedCook.Process(requestsSnapshot);
        });
        
        cookTask.ContinueWith(completedCookTask =>
        {
            try  
            {
                lock (_lockObject)
                {
                    LastCookResult = completedCookTask.Result;
                    Thread.Sleep(500);

                    int chickensCooked = requestsSnapshot.Get<Chicken>().Count;
                    int eggsCooked = requestsSnapshot.Get<Egg>().Count;
                    int rottenEggs = 0;
                    foreach (string line in LastCookResult.Split('\n'))
                    {
                        if (line.Contains("Rotten:"))
                        {
                            int.TryParse(line.Split("Rotten:")[1].Trim(), out rottenEggs);
                        }
                    }

                    _db.CompleteSession(sessionId, chickensCooked, eggsCooked, rottenEggs);
                    LastServeResult = ServeWithLinq(requestsSnapshot);
                }
                ServingComplete?.Invoke();
            }
            catch (Exception ex)
            {
                string innerMsg = ex.InnerException != null ? "\nInner: " + ex.InnerException.Message : "";
                LastServeResult = "ERROR: " + ex.Message + innerMsg;
                ServingComplete?.Invoke();
            }
        });
    }
    
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
    public DataBaseService GetDatabaseService() => _db;
}