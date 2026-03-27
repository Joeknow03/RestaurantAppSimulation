using System;
using System.Collections.Generic;

namespace RestaurantAppSimulation;

public class Cook
{
    public event EventHandler? Processed;
    private TableRequests? _currentRequests = null;
    
    public string OnServerReady(object? sender, EventArgs e)
    {
        if (_currentRequests == null)
            return "No requests to process!";
 
        return Process(_currentRequests);
    }
    public void SetRequests(TableRequests requests)
    {
        _currentRequests = requests;
    }
    
    public string Process(TableRequests requests)
    {
        string result = "";
 
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
 
            result += "All chicken prepared.\n";
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
                    try
                    {
                        egg.Crack();
                        egg.Cook();
                    }
                    catch (Exception)
                    {
                        rottenCount++;
                    }
                }
            }
 
            result += "All eggs prepared. Rotten: " + rottenCount + "\n";
        }
        
        Processed?.Invoke(this, EventArgs.Empty);
 
        return result;
    }
}