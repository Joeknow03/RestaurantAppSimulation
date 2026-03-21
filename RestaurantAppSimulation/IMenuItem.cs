namespace RestaurantAppSimulation;

public interface IMenuItem
{
    string Name { get; }
    void Obtain(); // get raw ingredients 
    void Serve(); // serve prepared food
}