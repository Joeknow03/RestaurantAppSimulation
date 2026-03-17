using System;
namespace RestaurantAppSimulation;

public class Server
{
    // Max 8 customers per table
    private const int MaxCustomers = 8;
    private int customerCount = 0;
    private MenuItem[][] requests = new MenuItem[MaxCustomers][];
    
    private Cook cook = new Cook();
    
    private ChickenOrder? preparedChicken = null;
    private EggOrder? preparedEgg = null;
    
    public string Receive(int chickenQty, int eggQty, MenuItem drink)
    {
        if (customerCount >= MaxCustomers)
        {
            return "Sorry, table is full! Max " + MaxCustomers + " customers.";
        }
        
        int totalItems = chickenQty + eggQty + 1;
        MenuItem[] customerOrder = new MenuItem[totalItems];
 
        int index = 0;
 
        // Add chicken items
        for (int i = 0; i < chickenQty; i++)
        {
            customerOrder[index] = MenuItem.Chicken;
            index++;
        }
 
        // Add egg items
        for (int i = 0; i < eggQty; i++)
        {
            customerOrder[index] = MenuItem.Egg;
            index++;
        }
 
        // Add drink 
        customerOrder[index] = drink;
 
        // Store customer's order in the jagged array
        requests[customerCount] = customerOrder;
        customerCount++;
 
        return "✅ Recorded order for Customer " + (customerCount - 1) +
               ": " + chickenQty + " chicken, " + eggQty + " egg, " + drink;
    }
    
    public string Send()
    {
        int totalChicken = 0;
        int totalEgg = 0;
 
        for (int c = 0; c < customerCount; c++)
        {
            for (int i = 0; i < requests[c].Length; i++)
            {
                if (requests[c][i] == MenuItem.Chicken)
                {
                    totalChicken++;
                }
                else if (requests[c][i] == MenuItem.Egg)
                {
                    totalEgg++;
                }
            }
        }
 
        string result = "Sending to Cook: " + totalChicken + " chicken, " + totalEgg + " egg\n";
        
        if (totalChicken > 0)
        {
            preparedChicken = cook.SubmitChicken(totalChicken);
            result += cook.PrepareChicken(preparedChicken) + "\n";
        }
        
        if (totalEgg > 0)
        {
            preparedEgg = cook.SubmitEgg(totalEgg);
            result += cook.PrepareEgg(preparedEgg) + "\n";
        }
 
        return result;
    }
    
    public string Serve()
    {
        string result = "";
 
        for (int c = 0; c < customerCount; c++)
        {
            int customerChicken = 0;
            int customerEgg = 0;
            MenuItem customerDrink = MenuItem.NoDrink;
 
            for (int i = 0; i < requests[c].Length; i++)
            {
                if (requests[c][i] == MenuItem.Chicken)
                {
                    customerChicken++;
                }
                else if (requests[c][i] == MenuItem.Egg)
                {
                    customerEgg++;
                }
                else
                {
                    customerDrink = requests[c][i];
                }
            }
            
            string drinkName;
            if (customerDrink == MenuItem.NoDrink)
            {
                drinkName = "no drink";
            }
            else if (customerDrink == MenuItem.CocaCola)
            {
                drinkName = "Coca Cola";
            }
            else
            {
                drinkName = customerDrink.ToString();  
            }
 
            result += "Customer " + c + " is served " +
                      customerChicken + " chicken, " +
                      customerEgg + " egg, " +
                      drinkName + "\n";
        }
 
        result += "Please enjoy your food!";
 
        // Reset for next table
        customerCount = 0;
        requests = new MenuItem[MaxCustomers][];
        preparedChicken = null;
        preparedEgg = null;
 
        return result;
    }
    
    public int GetCustomerCount()
    {
        return customerCount;
    }
}