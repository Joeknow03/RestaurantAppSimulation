using System;
namespace RestaurantAppSimulation;

public class Server
{
    private TableRequests _tableRequests = new TableRequests();
    private Cook _cook = new Cook();
    private bool _foodPrepared = false;
    public string Receive(int customerNumber, int chickenQty, int eggQty, string drinkChoice)
    {
        for (int i = 0; i < chickenQty; i++)
        {
            _tableRequests.Add(customerNumber, new Chicken());
        }
        
        for (int i = 0; i < eggQty; i++)
        {
            _tableRequests.Add(customerNumber, new Egg());
        }
        
        if (drinkChoice == "Tea")
        {
            _tableRequests.Add(customerNumber, new Tea());
        }
        else if (drinkChoice == "Coca Cola")
        {
            _tableRequests.Add(customerNumber, new CocaCola());
        }
        else if (drinkChoice == "Pepsi")
        {
            _tableRequests.Add(customerNumber, new Pepsi());
        }
        // if "No drink" then we add nothing
 
        string drinkDisplay = drinkChoice == "No drink" ? "no drink" : drinkChoice;
        return "Customer " + customerNumber + ": " +
               chickenQty + " chicken, " + eggQty + " egg, " + drinkDisplay;
    }
    
    public string Send()
    {
        string result = "--- Sending all requests to Cook ---\n";
        result += _cook.Process(_tableRequests);
        _foodPrepared = true;
        return result;
    }
    
    public string Serve()
    {
        if (!_foodPrepared)
        {
            return "Food hasn't been sent to Cook yet!";
        }
 
        string result = "";
        int customerCount = _tableRequests.CustomerCount;
 
        for (int c = 0; c < customerCount; c++)
        {
            IMenuItem[] items = _tableRequests[c];
 
            int chickenCount = 0;
            int eggCount = 0;
            string drinkName = "no drink";
 
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] is Chicken)
                {
                    chickenCount++;
                }
                else if (items[i] is Egg)
                {
                    eggCount++;
                }
                else if (items[i] is Drink)
                {
                    items[i].Obtain();
                    drinkName = items[i].Name;
                }
            }
 
            result += "Customer " + c + " is served " +
                      chickenCount + " chicken, " +
                      eggCount + " egg, " +
                      drinkName + "\n";
        }
 
        result += "Please enjoy your food!";
        
        _tableRequests = new TableRequests();
        _foodPrepared = false;
 
        return result;
    }
 
    public int GetCustomerCount()
    {
        return _tableRequests.CustomerCount;
    }
}