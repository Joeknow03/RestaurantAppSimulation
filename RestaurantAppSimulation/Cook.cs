using System;

namespace RestaurantAppSimulation;

public class Cook
{
    public string Process(TableRequests requests)
    {
        string result = "";
        IMenuItem[] chickens = requests[new Chicken()];
 
        if (chickens.Length > 0)
        {
            result += "Processing " + chickens.Length + " chicken(s)...\n";
 
            for (int i = 0; i < chickens.Length; i++)
            {
                Chicken chicken = (Chicken)chickens[i];
                chicken.Obtain();
                chicken.CutUp();
                chicken.Cook();
            }
 
            result += "All chicken prepared.\n";
        }
        
        IMenuItem[] eggs = requests[new Egg()];
 
        if (eggs.Length > 0)
        {
            result += "Processing " + eggs.Length + " egg(s)...\n";
            int rottenCount = 0;
 
            for (int i = 0; i < eggs.Length; i++)
            {
                Egg egg = (Egg)eggs[i];
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
                        // counting rotten egg and continue
                        rottenCount++;
                    }
                    
                }
            }
 
            result += "All eggs prepared. Rotten eggs found: " + rottenCount + "\n";
        }
        
        return result;
    }
}