using System;
using System.Collections.Generic;
using System.Threading;

namespace RestaurantAppSimulation;

public class Cook
{
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(2, 2);
    public string Name { get; private set; }
    public Cook(string name)
    {
        Name = name;
    }
    // Process all food in the table requests
    public string Process(TableRequests requests)
    {
        _semaphore.Wait();
        try
        {
            string result = Name + " is preparing food...\n";
 
            // Simulates cooking like in real time
            Thread.Sleep(1500); // 1.5 seconds
 
            // ---- Process all Chickens ----
            List<Chicken> chickens = requests.Get<Chicken>();
            if (chickens.Count > 0)
            {
                result += "Processing " + chickens.Count + " chicken(s)...\n";
                foreach (Chicken chicken in chickens)
                {
                    chicken.Obtain();
                    chicken.CutUp();
                    chicken.Cook();
                }
                result += "✅ All chicken prepared.\n";
            }
 
            // ---- Process all Eggs ----
            List<Egg> eggs = requests.Get<Egg>();
            if (eggs.Count > 0)
            {
                result += "Processing " + eggs.Count + " egg(s)...\n";
                int rottenCount = 0;
 
                foreach (Egg egg in eggs)
                {
                    egg.Obtain();
                    using (egg)
                    {
                        try { egg.Crack(); egg.Cook(); }
                        catch (Exception) { rottenCount++; }
                    }
                }
                result += "All eggs prepared. Rotten: " + rottenCount + "\n";
            }
            // Simulate more cooking time
            Thread.Sleep(500);
 
            result += Name + " finished preparing!\n";
            return result;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}