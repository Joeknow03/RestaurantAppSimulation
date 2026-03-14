using System;

namespace RestaurantAppSimulation;

public class ChickenOrder
{
    private int quantity;
    
    public ChickenOrder(int quantity)
    {
        this.quantity = quantity;
    }
    
    public int GetQuantity()
    {
        return quantity;
    }
 
    
    public void CutUp()
    {
        
    }
 
    
    public void Cook()
    {
        // cooking process
    }
}