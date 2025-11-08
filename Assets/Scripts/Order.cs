using System;
using UnityEngine;

public class Order : MonoBehaviour
{
    private OrderData orderData;
    [SerializeField] private Bingsu bingsu;
    public static event Action onAddTray;
    public static event Action onAddCondensedMilk;

    void Start()
    {
        orderData = new OrderData();
        orderData.setTray();
        onAddTray?.Invoke();
    }

    public void OrderSpriteSignals()
    {
        if (orderData.hasTray())
        {
            onAddTray?.Invoke();
        }
        if (orderData.hasCondenseMilk())
            onAddCondensedMilk?.Invoke();

        if (bingsu != null && orderData.getBingsuOrder().hasBowl())
            bingsu.createBingsu(orderData.getBingsuOrder());
    }

    public void createOrder()
    {
        orderData.randomOrder();
        OrderSpriteSignals();
    }

    public void createOrder(OrderData oData)
    {
        orderData = oData;
        OrderSpriteSignals();
    }

    public bool AddCondensedMilk()
    {
        if (orderData.setCondenseMilk())
        {
            onAddCondensedMilk?.Invoke();
            return true;
        }
        return false;
    }

    public void AssignBingsu(Bingsu bingsuRef)
    {
        bingsu = bingsuRef;
    }

    public void RemoveBingsu()
    {
        bingsu = null;
    }

    public Bingsu GetBingsu() => bingsu;
    public OrderData GetOrderData() => orderData;

    public bool HasBingsu() => bingsu != null;

    public OrderData getOrderData()
    {
        return orderData;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Order other = (Order)obj;
        return orderData == other.orderData;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(orderData);
    }

    public static bool operator ==(Order a, Order b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        return a.orderData == b.orderData;
    }

    public static bool operator !=(Order a, Order b)
    {
        return !(a == b);
    }
}
