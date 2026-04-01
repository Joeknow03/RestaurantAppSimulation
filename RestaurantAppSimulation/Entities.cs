using System;
using System.Collections.Generic;

namespace RestaurantAppSimulation;

public class Session
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string CookName { get; set; } = "";
    public List<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
    public CookingResult? CookingResult { get; set; }
}

public class CustomerOrder
{
    public int Id { get; set; }
 
    public string CustomerName { get; set; } = "";
    public int ChickenCount { get; set; }
    public int EggCount { get; set; }
    public string DrinkChoice { get; set; } = "No drink";
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
}

public class CookingResult
{
    public int Id { get; set; }
 
    public int ChickensCooked { get; set; }
    public int EggsCooked { get; set; }
    public int RottenEggsFound { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
}