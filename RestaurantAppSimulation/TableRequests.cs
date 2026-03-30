using System;
using System.Collections;
using System.Collections.Generic;

namespace RestaurantAppSimulation;

public class TableRequests : IEnumerable<IMenuItem>
{
    
    private Dictionary<string, List<IMenuItem>> _orders 
        = new Dictionary<string, List<IMenuItem>>();
    private List<string> _customerOrder = new List<string>();
    
    public void Add<T>(string customerName) where T : IMenuItem, new()
    {
        // If this customer doesn't have a list yet, create one
        if (!_orders.ContainsKey(customerName))
        {
            _orders[customerName] = new List<IMenuItem>();
            _customerOrder.Add(customerName); // remember insertion order
        }
        _orders[customerName].Add(new T());
    }
    
    public List<T> Get<T>() where T : IMenuItem
    {
        List<T> result = new List<T>();
        
        foreach (string customerName in _customerOrder)
        {
            foreach (IMenuItem item in _orders[customerName])
            {
                if (item is T typedItem)
                {
                    result.Add(typedItem);
                }
            }
        }
 
        return result;
    }
    
    public List<IMenuItem> this[string customerName]
    {
        get
        {
            if (_orders.ContainsKey(customerName))
            {
                return _orders[customerName];
            }
            return new List<IMenuItem>(); // empty list if customer not found
        }
    }
    
    public int CustomerCount => _customerOrder.Count;
    public List<string> CustomerNames => _customerOrder;
    public IEnumerator<IMenuItem> GetEnumerator()
    {
        foreach (string customerName in _customerOrder)
        {
            foreach (IMenuItem item in _orders[customerName])
            {
                if (item is Drink)
                {
                    yield return item; 
                }
            }
        }
        
        foreach (string customerName in _customerOrder)
        {
            foreach (IMenuItem item in _orders[customerName])
            {
                if (item is not Drink)
                {
                    yield return item;
                }
            }
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}