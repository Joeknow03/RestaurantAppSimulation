using System;
namespace RestaurantAppSimulation;

public class TableRequests
{
    private const int MaxCustomers = 8;
    private const int MaxItemsPerCustomer = 30;
    private IMenuItem[][] _requests = new IMenuItem[MaxCustomers][]; // jagged array
    private int[] _itemCounts = new int[MaxCustomers];
    private int _customerCount = 0;
    
    public void Add(int customer, IMenuItem item)
    {
        if (_requests[customer] == null)
        {
            _requests[customer] = new IMenuItem[MaxItemsPerCustomer];
            if (customer + 1 > _customerCount)
            {
                _customerCount = customer + 1;
            }
        }
        _requests[customer][_itemCounts[customer]] = item;
        _itemCounts[customer]++;
    }
    
    public IMenuItem[] this[IMenuItem item]
    {
        get
        {
            System.Type targetType = item.GetType();
            int count = 0;
            for (int c = 0; c < MaxCustomers; c++)
            {
                if (_requests[c] == null) continue;
                for (int i = 0; i < _itemCounts[c]; i++)
                {
                    if (_requests[c][i].GetType() == targetType)
                    {
                        count++;
                    }
                }
            }
            
            IMenuItem[] result = new IMenuItem[count];
            int index = 0;
            for (int c = 0; c < MaxCustomers; c++)
            {
                if (_requests[c] == null) continue;
                for (int i = 0; i < _itemCounts[c]; i++)
                {
                    if (_requests[c][i].GetType() == targetType)
                    {
                        result[index] = _requests[c][i];
                        index++;
                    }
                }
            }
 
            return result;
        }
    }
    
    public IMenuItem[] this[int customer]
    {
        get
        {
            if (_requests[customer] == null)
            {
                return new IMenuItem[0];
            }
            IMenuItem[] result = new IMenuItem[_itemCounts[customer]];
            for (int i = 0; i < _itemCounts[customer]; i++)
            {
                result[i] = _requests[customer][i];
            }
            return result;
        }
    }

    public int CustomerCount
    {
        get { return _customerCount; }
    }
}