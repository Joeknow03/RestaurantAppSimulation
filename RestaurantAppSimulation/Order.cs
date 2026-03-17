using System;

namespace RestaurantAppSimulation;

public class Order
{
    protected int quantity;
    
    public Order(int quantity)
    {
        this.quantity = quantity;
    }
    
    public int GetQuantity()
    {
        return quantity;
    }
    
    public virtual void Cook()
    {
        
    }
    
    public void SubtractQuantity(int amount)
    {
        quantity -= amount;
    }
}