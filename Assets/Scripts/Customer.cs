using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private CustomerData customerData;
    private float timerDuration = 30.0f;
    private float timerPercentAngry = 0.33f;
    private float timer;
    private bool orderDelivered = false;
    private bool isAngry = false;
    
    [SerializeField] private float baseBlinkRate = 1.0f;    // Base time between blinks
    [SerializeField] private float minBlinkRate = 0.2f;     // Fastest blink rate when timer is almost up
    public event Action<bool, float> onFrustratedBlink;     // (isVisible, blinkRate)

    public event Action<string> onStateChange;
    public event Action<string, string> changeAppearance;
    public static event Action<Customer> onTimerEnd; // Keep this static as it's for GameManager

    void Start()
    {
        customerData = new CustomerData();
        customerData.randomCustomer();
        changeAppearance?.Invoke(customerData.getColor(), customerData.getAccessory());
        onStateChange?.Invoke(customerData.getState());
        timer = timerDuration;
    }

    private float blinkTimer = 0f;
    private bool iconVisible = true;

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
            if (!isAngry)
            {
                isAngry = true;
                onStateChange?.Invoke("angry");
            }

            // Calculate blink rate based on remaining time
            float angerDuration = timerDuration * timerPercentAngry;
            float remainingAngerTime = timer;
            float blinkRate = Mathf.Lerp(minBlinkRate, baseBlinkRate, remainingAngerTime / angerDuration);
            
            // Handle blinking
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                iconVisible = !iconVisible;
                blinkTimer = blinkRate;
                onFrustratedBlink?.Invoke(iconVisible, blinkRate);
            }
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
