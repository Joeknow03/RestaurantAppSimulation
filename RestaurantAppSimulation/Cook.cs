using System;

namespace RestaurantAppSimulation;

public class Cook
{
    public ChickenOrder SubmitChicken(int quantity)
    {
        return new ChickenOrder(quantity);
    }
 
    // Submit an egg request
    public EggOrder SubmitEgg(int quantity)
    {
        return new EggOrder(quantity);
    }
 
    // Prepare all the chicken
    public string PrepareChicken(ChickenOrder order)
    {
        int qty = order.GetQuantity();
        for (int i = 0; i < qty; i++)
        {
            order.CutUp();
        }
        order.Cook();
 
        return "Prepared " + qty + " chicken.";
    }
    
    public string PrepareEgg(EggOrder order)
    {
        int qty = order.GetQuantity();
        int rottenCount = 0;
 
        for (int i = 0; i < qty; i++)
        {
            try
            {
                order.Crack();
            }
            catch (Exception)
            {
                rottenCount++;
            }
            finally
            {
                // Always discard shell
                order.DiscardShell();
            }
        }
        order.Cook();
 
        return "Prepared " + qty + " egg(s). Rotten eggs found: " + rottenCount + ".";
    }
}