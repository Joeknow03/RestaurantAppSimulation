namespace RestaurantAppSimulation;

public abstract class Drink : MenuItem
{
    public override void Obtain() { }
 
    public override void Serve() { }
}
 
public sealed class Tea : Drink
{
    public Tea() { Name = "Tea"; }
}
 
public sealed class CocaCola : Drink
{
    public CocaCola() { Name = "Coca Cola"; }
}
 
public sealed class Pepsi : Drink
{
    public Pepsi() { Name = "Pepsi"; }
}