using System;

namespace RestaurantAppSimulation;

public class EggOrder : Order
{
    private int qualityValue;
    private static Random sharedRandom = new Random();
    private static int totalCreated = 0;
    private int myInstanceNumber;
 
    // Constructor: takes how many eggs to order
    public EggOrder(int quantity) : base(quantity)
    {
        totalCreated++;
        myInstanceNumber = totalCreated;
        qualityValue = sharedRandom.Next(1, 101);
    }
    
    public EggOrder(int quantity, int quality) : base(quantity)
    {
        totalCreated++;
        myInstanceNumber = totalCreated;
        qualityValue = quality;
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
    
    public override void Cook()
    {
        // Cook everything together
    }
}