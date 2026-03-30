using System;
namespace RestaurantAppSimulation;

public abstract class MenuItem : IMenuItem
{
    public string Name { get; protected set; } = "Unknown";
    public abstract void Obtain();
    public abstract void Serve();
}