using System;
using System.ComponentModel;
using Unity.Burst;
using UnityEngine;

public class Bingsu : MonoBehaviour
{
    private bool bowl;
    private bool shavedMilk;
    private bool baseTopping;
    private string baseToppingType;
    private bool drizzle;
    private bool topping;
    private string toppingType;
    private bool logo;
    private string[] bingsuBaseToppingType = { "strawberry", "mango", "chocolate", "injeolmi", "ube", "pat" };
    private string[] bingsuToppingType = { "tiramisu", "bungeoppang", "mochi", "chocolateBar", "cheeseCake", "matchaIceCream", "vanillaIceCream", "chocolateIceCream", "ubeIceCream" };

    public void Ready()
    {
        bowl = false;
        shavedMilk = false;
        baseTopping = false;
        baseToppingType = "";
        drizzle = false;
        topping = false;
        toppingType = "";
        logo = false;
    }

    public void RandomBingsu()
    {
        bowl = true;
        shavedMilk = true;
        baseTopping = true;
        baseToppingType = bingsuBaseToppingType[UnityEngine.Random.Range(0, bingsuBaseToppingType.Length)];
        drizzle = (UnityEngine.Random.value > 0.5f) ? true : false;
        topping = true;
        toppingType = bingsuToppingType[UnityEngine.Random.Range(0, bingsuToppingType.Length)];
        logo = true;
    }

    public void setBowl()
    {
        bowl = true;
    }

    public void setShavedMilk()
    {
        shavedMilk = true;
    }

    public void setBaseTopping(string type)
    {
        baseTopping = true;
        baseToppingType = type;
    }

    public void setDrizzle()
    {
        drizzle = true;
    }

    public void setTopping(string type)
    {
        topping = true;
        toppingType = type;
    }

    public void setLogo()
    {
        logo = true;
    }

    public bool hasBowl()
    {
        return bowl;
    }

    public bool hasShavedMilk()
    {
        return shavedMilk;
    }

    public bool hasBaseTopping()
    {
        return baseTopping;
    }

    public string getBaseToppingType()
    {
        return baseToppingType;
    }

    public bool hasDrizzle()
    {
        return drizzle;
    }

    public bool hasTopping()
    {
        return topping;
    }

    public string getToppingType()
    {
        return toppingType;
    }

    public bool hasLogo()
    {
        return logo;
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
        return HashCode.Combine(bowl, shavedMilk, baseTopping, baseToppingType, drizzle, topping, toppingType, logo);
    }

    public static bool operator ==(Bingsu a, Bingsu b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return (a.bowl == b.bowl &&
                a.shavedMilk == b.shavedMilk &&
                a.baseTopping == b.baseTopping &&
                a.baseToppingType == b.baseToppingType &&
                a.drizzle == b.drizzle &&
                a.topping == b.topping &&
                a.toppingType == b.toppingType &&
                a.logo == b.logo);
    }

    public static bool operator !=(Bingsu a, Bingsu b)
    {
        return !(a == b);
    }

    public void clearBingsu()
    {
        bowl = false;
        shavedMilk = false;
        baseTopping = false;
        baseToppingType = "";
        drizzle = false;
        topping = false;
        toppingType = "";
        logo = false;
    } 
}
