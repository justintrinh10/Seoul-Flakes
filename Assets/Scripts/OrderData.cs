using System;

public class OrderData
{
    private BingsuData bingsuOrder;
    private bool tray;
    private bool condenseMilkCup;
    private bool condenseMilk;
    private float condenseMilkVal = 0.5f;

    public OrderData()
    {
        bingsuOrder = new BingsuData();
        tray = false;
        condenseMilkCup = false;
        condenseMilk = false;
    }

    public void randomOrder()
    {
        bingsuOrder = new BingsuData();
        bingsuOrder.RandomBingsu();
        tray = true;
        condenseMilkCup = true;
        condenseMilk = true;
    }

    public float calcVal()
    {
        float totalVal = 0.0f;

        totalVal += bingsuOrder.calcVal();

        if (condenseMilk)
        {
            totalVal += condenseMilkVal;
        }

        return totalVal;
    }

    public void setBingsuOrder(BingsuData BingsuData)
    {
        bingsuOrder = BingsuData;
    }

    public BingsuData getBingsuOrder()
    {
        return bingsuOrder;
    }

    public void setTray()
    {
        tray = true;
    }

    public void setCondenseMilkCup()
    {
        condenseMilkCup = true;
    }

    public void setCondenseMilk()
    {
        condenseMilk = true;
    }

    public bool hasTray()
    {
        return tray;
    }

    public bool hasCondenseMilkCup()
    {
        return condenseMilkCup;
    }

    public bool hasCondenseMilk()
    {
        return condenseMilk;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        OrderData other = (OrderData)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(bingsuOrder, tray, condenseMilkCup, condenseMilk);
    }

    public static bool operator ==(OrderData a, OrderData b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return (a.bingsuOrder == b.bingsuOrder &&
                a.tray == b.tray &&
                a.condenseMilkCup == b.condenseMilkCup &&
                a.condenseMilk == b.condenseMilk);
    }

    public static bool operator !=(OrderData a, OrderData b)
    {
        return !(a == b);
    }

    public void clearOrder()
    {
        bingsuOrder.clearBingsu();
        tray = false;
        condenseMilkCup = false;
        condenseMilk = false;
    }
}
