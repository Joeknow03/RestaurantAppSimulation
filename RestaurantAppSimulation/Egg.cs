using System;

namespace RestaurantAppSimulation;

public sealed class Egg : CookedFood, IDisposable
{
    public int? Quality { get; private set; }
    private static int _totalCreated = 0;
    private static Random _random = new Random();
    private int _myInstanceNumber;
    private bool _shellsDiscarded = false; // safety flag to avoid discarding twice
 
    public Egg()
    {
        Name = "Egg";
 
        _totalCreated++;
        _myInstanceNumber = _totalCreated;
        int rawQuality = _random.Next(1, 101);
        
        if (_myInstanceNumber % 2 == 0)
        {
            Quality = null;  // employee forgot to check
        }
        else
        {
            Quality = rawQuality;
        }
        _internalQuality = rawQuality;
    }
    private int _internalQuality;
    
    public override void Obtain() { }
    
    public void Crack()
    {
        if (_internalQuality < 25)
        {
            throw new Exception("Rotten egg! Internal quality is " + _internalQuality);
        }
        
    }
    
    public void DiscardShells()
    {
        if (!_shellsDiscarded)
        {
            _shellsDiscarded = true;
        }
    }
    
    public override void Cook() { }
    
    public override void Serve() { }
    
    public void Dispose()
    {
        DiscardShells();
    }
}