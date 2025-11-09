using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private CustomerData customerData;
    private float timerDuration = 30.0f;
    private float timerPercentAngry = 0.33f;
    private float timer;
    private bool orderDelivered = false;

    public static event Action<string> onStateChange;
    public static event Action<string, string> changeAppearance;
    public static event Action<Customer> onTimerEnd;

    void Start()
    {
        customerData = new CustomerData();
        customerData.randomCustomer();
        changeAppearance?.Invoke(customerData.getColor(), customerData.getAccessory());
        onStateChange?.Invoke(customerData.getState());
        timer = timerDuration;
    }

    void Update()
    {
        if (orderDelivered) return;

        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            orderDelivered = true;
            onStateChange?.Invoke("angry");
            onTimerEnd?.Invoke(this);
        }
        else if (timer <= timerDuration * timerPercentAngry)
        {
            onStateChange?.Invoke("angry");
        }
    }

    public bool DeliverOrder(Order deliveredOrder)
    {
        orderDelivered = true;

        if (customerData.getOrder() == deliveredOrder.getOrderData())
        {
            onStateChange?.Invoke("happy");
            return true;
        }

        onStateChange?.Invoke("angry");
        return false;
    }

    public CustomerData GetCustomerData() => customerData;
}
