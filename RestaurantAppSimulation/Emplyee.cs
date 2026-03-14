using System;

namespace RestaurantAppSimulation;

public class Emplyee
{
    private object? lastOrder = null;
    private int requestCount = 0;
    private bool alreadyPrepared = false;
    
    public object NewRequest(string menuItem, int quantity)
    {
        requestCount++;
        string actualItem = menuItem;
        if (requestCount % 3 == 0)
        {
            if (menuItem == "Chicken") // case when employee forgets
            {
                actualItem = "Egg";   
            }
            else
            {
                actualItem = "Chicken";  
            }
        }
 
        // Create the correct type of order
        object newOrder;
        if (actualItem == "Chicken")
        {
            newOrder = new ChickenOrder(quantity);
        }
        else
        {
            newOrder = new EggOrder(quantity);
        }
        
        lastOrder = newOrder;
        alreadyPrepared = false;
        return newOrder;
    }
    
    public object CopyRequest()
    {
        if (lastOrder == null)
        {
            throw new Exception("I have no previous request to copy. Please submit a request first!");
        }
 
        object copy;
        
        if (lastOrder is ChickenOrder lastChicken)
        {
            copy = new ChickenOrder(lastChicken.GetQuantity());
        }
        else
        {
            EggOrder lastEgg = (EggOrder)lastOrder;
            copy = new EggOrder(lastEgg.GetQuantity());
        }
        
        lastOrder = copy;
        alreadyPrepared = false;
 
        return copy;
    }
    
    public string Inspect(object order)
    {
        if (order is ChickenOrder chickenOrder)
        {
            return "Got " + chickenOrder.GetQuantity() + " chicken. No inspection needed.";
        }
        // Eggs need quality inspection
        else if (order is EggOrder eggOrder)
        {
            int? quality = eggOrder.GetQuality();
 
            if (quality == null)
            {
                // Employee forgot to check quality
                return "Got " + eggOrder.GetQuantity() + " egg(s). Employee forgot to check quality!";
            }
            else
            {
                return "Got " + eggOrder.GetQuantity() + " egg(s). Egg quality: " + quality;
            }
        }
        return "Unknown order type!";
    }
    
    public string PrepareFood(object order)
    {
        if (alreadyPrepared)
        {
            throw new Exception("This food has already been prepared! Please submit a new request.");
        }
 
        string result = "";
 
        if (order is ChickenOrder chickenOrder)
        {
            int qty = chickenOrder.GetQuantity();
            for (int i = 0; i < qty; i++)
            {
                chickenOrder.CutUp();
            }
            
            chickenOrder.Cook();
 
            result = "Prepared " + qty + " chicken! Cut up " + qty + " pieces and cooked them all.";
        }
        else if (order is EggOrder eggOrder)
        {
            int qty = eggOrder.GetQuantity();
            int rottenCount = 0;
            
            for (int i = 0; i < qty; i++)
            {
                try
                {
                    eggOrder.Crack();
                }
                catch (Exception)
                {
                    rottenCount++;
                }
                finally
                {
                    eggOrder.DiscardShell();
                }
            }
            eggOrder.Cook();
 
            result = "Prepared " + qty + " egg(s). Discarded shells. Found " + rottenCount + " rotten egg(s).";
        }
        alreadyPrepared = true;
 
        return result;
    }
}