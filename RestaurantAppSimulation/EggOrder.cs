using System;

namespace RestaurantAppSimulation;

public class EggOrder
{
    private int quantity;
    private int qualityValue;
    
    private static Random sharedRandom = new Random();
    
    private static int totalEggOrdersCreated = 0;
    private int myInstanceNumber;
 
    // Constructor: takes how many eggs to order
    public EggOrder(int quantity)
    {
        this.quantity = quantity;
        totalEggOrdersCreated++;
        myInstanceNumber = totalEggOrdersCreated;
        qualityValue = sharedRandom.Next(1, 101);
    }
    
    public int GetQuantity()
    {
        return quantity;
    }
 
    // Returns the egg quality - but returns null every 2nd, 4th, 6th time
    public int? GetQuality()
    {
        if (myInstanceNumber % 2 == 0)
        {
            return null;
        }
 
        return qualityValue;
    }
    
    public void Crack()
    {
        if (qualityValue < 25)
        {
            throw new Exception("ROTTEN EGG! Quality is " + qualityValue + " (needs to be at least 25)");
        }
        
    }
    
    public void DiscardShell()
    {
        // Throw away the shell
    }
    
    public void Cook()
    {
        // Cook everything together
    }
}