using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    CustomerData customerData;
    private float timerDuration = 30.0f;
    private float timerPercentAngry = 0.33f;
    private float timer;
    private static event Action<string> onStateChange;
    private static event Action<string, string> changeAppearance;
    private static event Action onTimerEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customerData.randomCustomer();
        changeAppearance?.Invoke(customerData.getColor(), customerData.getAccessory());
        onStateChange?.Invoke(customerData.getState());
        timer = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            onTimerEnd?.Invoke();
        }
        else if (timer <= timerDuration * timerPercentAngry)
        {
            onStateChange?.Invoke("angry");
        }
    }
    
    public bool deliverOrder(Order deliveredOrder)
    {
        if (customerData.getOrder() == deliveredOrder.getOrderData())
        {
            onStateChange?.Invoke("happy");
            return true;
        }
        return false;
    }
}
