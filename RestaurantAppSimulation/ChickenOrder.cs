using System;

namespace RestaurantAppSimulation;

public class ChickenOrder : Order
{
    private int quantity;
    
    public ChickenOrder(int quantity) : base(quantity)
    {
        this.quantity = quantity;
    }
    
    public void CutUp()
    {
        
    }
    
    public override void Cook()
    {
        // cooking process
    }
}