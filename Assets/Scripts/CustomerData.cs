using System;
using System.Data.SqlTypes;
using UnityEngine;

public class CustomerData
{
    private OrderData currentOrder;
    private string state;
    private string[] allStates = { "angry", "normal", "happy" };
    private string color;
    private string[] allColors = { "red", "orange", "yellow", "green", "blue", "purple", "pink" };
    private string accessory;
    private string[] allAccessory = { "ninjaHeadBand", "balloon" , "sprout", "crown", "devilHorn", "detectiveHat", "bandana", "cowboyHat", "topHat", "chefHat"};

    public CustomerData()
    {
        currentOrder = new OrderData();
        state = allStates[1];
        color = allColors[UnityEngine.Random.Range(0, allColors.Length)];
        accessory = allAccessory[UnityEngine.Random.Range(0, allAccessory.Length)];
    }

    public void randomCustomer()
    {
        currentOrder = new OrderData();
        currentOrder.randomOrder();
    }

    public void setOrder(OrderData OrderData)
    {
        currentOrder = OrderData;
    }

    public OrderData getOrder()
    {
        return currentOrder;
    }

    public string getState()
    {
        return state;
    }

    public bool setState(string s)
    {
        if (state == s)
        {
            return false;
        }
        state = s;
        return true;
    }

    public string getColor()
    {
        return color;
    }

    public string getAccessory()
    {
        return accessory;
    }

    public string getOrderPrompt()
    {
        string prompt = "Can I get a ";
        prompt += currentOrder.getBingsuOrder().getBingsuDescription();
        return prompt;
    }
}
