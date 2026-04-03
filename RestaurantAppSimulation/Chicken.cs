using System;
namespace RestaurantAppSimulation;

public sealed class Chicken : CookedFood
{
    // set the name property from the parent class
    public Chicken()
    {
        Name = "Chicken";
    }
    
    public override void Obtain() { }
    
    public void CutUp() { }
    
    public override void Cook() { }
    
    public override void Serve() { }
}