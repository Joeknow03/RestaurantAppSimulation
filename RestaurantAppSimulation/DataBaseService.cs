using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAppSimulation;

public class DataBaseService
{
    public int StartSession(string cookName)
    {
        using (RestaurantDBContext db = new RestaurantDBContext())
        {
            Session session = new Session
            {
                StartedAt = DateTime.UtcNow,
                CookName = cookName
            };
 
            db.Sessions.Add(session);
            db.SaveChanges(); 
 
            return session.Id; 
        }
    }
    
    public void SaveCustomerOrder(int sessionId, string customerName,
                                   int chickenCount, int eggCount, string drinkChoice)
    {
        using (RestaurantDBContext db = new RestaurantDBContext())
        {
            CustomerOrder order = new CustomerOrder
            {
                SessionId = sessionId,
                CustomerName = customerName,
                ChickenCount = chickenCount,
                EggCount = eggCount,
                DrinkChoice = drinkChoice
            };
 
            db.CustomerOrders.Add(order);
            db.SaveChanges();
        }
    }
    
    public void CompleteSession(int sessionId, int chickensCooked,
        int eggsCooked, int rottenEggs)
    {
        
        if (sessionId <= 0) return;

        using (RestaurantDBContext db = new RestaurantDBContext())
        {
            Session? session = db.Sessions.Find(sessionId);
            if (session == null) return; // session not found

            session.CompletedAt = DateTime.UtcNow;

            CookingResult result = new CookingResult
            {
                SessionId = sessionId,
                ChickensCooked = chickensCooked,
                EggsCooked = eggsCooked,
                RottenEggsFound = rottenEggs
            };
            db.CookingResults.Add(result);

            db.SaveChanges();
        }
    }
    
    public List<Session> GetAllSessions()
    {
        using (RestaurantDBContext db = new RestaurantDBContext())
        {
            return db.Sessions
                .Include(s => s.CustomerOrders)
                .Include(s => s.CookingResult)
                .OrderByDescending(s => s.StartedAt) // newest first
                .ToList();
        }
    }
    
    public void EnsureDatabaseCreated()
    {
        using (RestaurantDBContext db = new RestaurantDBContext())
        {
            db.Database.EnsureCreated();
        }
    }
}