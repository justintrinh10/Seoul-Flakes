using System;
using UnityEngine;

public class Bingsu : MonoBehaviour
{
    BingsuData bingsuData;
    public static event Action onAddBowl;
    public static event Action onAddShavedMilk;
    public static event Action<string> onAddBaseTopping;
    public static event Action onAddDrizzle;
    public static event Action<string> onAddTopping;
    public static event Action onAddLogo;
    public static event Action onClearBingsu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Ensure bingsuData is initialized as early as possible so other objects
        // can call into this Bingsu (eg. displayOrder) immediately after Instantiate.
        if (bingsuData == null)
            bingsuData = new BingsuData();
        onClearBingsu?.Invoke();
    }

    public void bingsuSpriteSignals()
    {
        if (bingsuData.hasBowl())
        {
            onAddBowl?.Invoke();
        }
        if (bingsuData.hasShavedMilk())
        {
            onAddShavedMilk?.Invoke();
        }
        if (bingsuData.hasBaseTopping())
        {
            onAddBaseTopping?.Invoke(bingsuData.getBaseToppingType());
        }
        if (bingsuData.hasDrizzle())
        {
            onAddDrizzle?.Invoke();
        }
        if (bingsuData.hasTopping())
        {
            onAddTopping?.Invoke(bingsuData.getToppingType());
        }
        if (bingsuData.hasLogo())
        {
            onAddLogo?.Invoke();
        }
    }

    public void createRandomBingsu()
    {
        bingsuData.RandomBingsu();
        bingsuSpriteSignals();
    }

    public void createBingsu(BingsuData data)
    {
        bingsuData = data;
        bingsuSpriteSignals();
    }

    public bool addBowl()
    {
        if (bingsuData.setBowl())
        {
            onAddBowl?.Invoke();
            return true;
        }
        return false;
    }
    
    public bool addShavedMilk()
    {
        if (bingsuData.setShavedMilk())
        {
            onAddShavedMilk?.Invoke();
            return true;
        }
        return false;
    }

    public bool addBaseTopping(string type)
    {
        if (bingsuData.setBaseTopping(type))
        {
            onAddBaseTopping?.Invoke(type);
            return true;
        }
        return false;
    }

    public bool addDrizzle()
    {
        if (bingsuData.setDrizzle())
        {
            onAddDrizzle?.Invoke();
            return true;
        }
        return false;
    }

    public bool addTopping(string type)
    {
        if (bingsuData.setTopping(type))
        {
            onAddTopping?.Invoke(type);
            return true;
        }
        return false;
    }

    public bool addLogo()
    {
        if (bingsuData.setLogo())
        {
            onAddLogo?.Invoke();
            return true;
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Bingsu other = (Bingsu)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(bingsuData);
    }

    public static bool operator ==(Bingsu a, Bingsu b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.bingsuData == b.bingsuData;
    }

    public static bool operator !=(Bingsu a, Bingsu b)
    {
        return !(a == b);
    }
}
