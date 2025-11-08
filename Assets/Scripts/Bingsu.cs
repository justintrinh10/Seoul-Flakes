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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bingsuData = new BingsuData();
        bingsuData.setBowl();
        onAddBowl?.Invoke();
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
