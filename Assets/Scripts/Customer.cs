using System;
using UnityEngine;

public class Customer
{
    private Order currentOrder;
    private string state;
    private string[] allStates = { "angry", "normal", "happy" };
    private string color;
    private string[] allColors = { };
    private string accessory;
    private string[] allAccessory = { };

    public Customer()
    {
        currentOrder = new Order();
        state = allStates[1];
        color = allColors[UnityEngine.Random.Range(0, allColors.Length)];
        accessory = allAccessory[UnityEngine.Random.Range(0, allAccessory.Length)];
    }

    public void randomCustomer()
    {
        currentOrder = new Order();
        currentOrder.randomOrder();
    }

    public void setOrder(Order order)
    {
        currentOrder = order;
    }

    public Order GetCurrentOrder()
    {
        return currentOrder;
    }
}
