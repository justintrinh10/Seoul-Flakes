using System;
using UnityEngine;

public class BingsuData
{
    private bool bowl;
    private bool shavedMilk;
    private float shavedMilkVal = 2.0f;
    private bool baseTopping;
    private string baseToppingType;
    private bool drizzle;
    private float drizzleVal = 0.5f;
    private bool topping;
    private string toppingType;
    private bool logo;
    private float logoVal = 0.5f;
    private string[] bingsuBaseToppingType = { "strawberry", "mango", "chocolate", "injeolmi", "ube", "pat" };
    private float[] bingsuBaseToppingValues = { 3.0f, 3.0f, 2.0f, 2.5f, 2.5f, 2.0f };
    private string[] bingsuToppingType = { "tiramisu", "bungeoppang", "mochi", "chocolateBar", "cheeseCake", "matchaIceCream", "vanillaIceCream", "chocolateIceCream", "ubeIceCream" };
    private float[] bingsuToppingValues = { 2.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.5f, 1.5f, 1.5f, 1.5f };

    public BingsuData()
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

    public float calcVal()
    {
        float totalVal = 0.0f;

        if (shavedMilk)
        {
            totalVal += shavedMilkVal;
        }

        if (baseTopping)
        {
            int index = Array.IndexOf(bingsuBaseToppingType, baseToppingType);
            if (index >= 0)
            {
                totalVal += bingsuBaseToppingValues[index];
            }
        }

        if (drizzle)
        {
            totalVal += drizzleVal;
        }

        if (topping)
        {
            int index = Array.IndexOf(bingsuToppingType, toppingType);
            if (index >= 0)
            {
                totalVal += bingsuToppingValues[index];
            }
        }

        if (logo)
        {
            totalVal += logoVal;
        }

        return totalVal;
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

        BingsuData other = (BingsuData)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(bowl, shavedMilk, baseTopping, baseToppingType, drizzle, topping, toppingType, logo);
    }

    public static bool operator ==(BingsuData a, BingsuData b)
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

    public static bool operator !=(BingsuData a, BingsuData b)
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
