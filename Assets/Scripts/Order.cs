using System;

public class Order
{
    private Bingsu bingsuOrder;
    private bool tray;
    private bool condenseMilkCup;
    private bool condenseMilk;

    public Order()
    {
        bingsuOrder = new Bingsu();
        tray = false;
        condenseMilkCup = false;
        condenseMilk = false;
    }

    public void randomOrder()
    {
        bingsuOrder = new Bingsu();
        bingsuOrder.RandomBingsu();
        tray = true;
        condenseMilkCup = true;
        condenseMilk = true;
    }

    public Bingsu getBingsuOrder()
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

        Order other = (Order)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(bingsuOrder, tray, condenseMilkCup, condenseMilk);
    }

    public static bool operator ==(Order a, Order b)
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

    public static bool operator !=(Order a, Order b)
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
