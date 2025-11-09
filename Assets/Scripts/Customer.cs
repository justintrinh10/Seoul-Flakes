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

    [SerializeField] private float baseBlinkRate = 1.0f;
    [SerializeField] private float minBlinkRate = 0.2f;

    public event Action<bool, float> onFrustratedBlink;
    public event Action<string> onStateChange;
    public event Action<string, string> changeAppearance;
    public static event Action<Customer> onTimerEnd;

    [Header("References")]
    public CustomerDialogueBox dialogueBoxPrefab;
    private CustomerDialogueBox dialogueBoxInstance;

    [Header("Dialogue Box Settings")]
    [SerializeField] private Vector3 dialogueOffset = new Vector3(0, 2.2f, 0);
    [SerializeField] private float dialogueScale = 1f;

    private float blinkTimer = 0f;
    private bool iconVisible = true;

    void Start()
    {
        customerData = new CustomerData();
        customerData.randomCustomer();
        changeAppearance?.Invoke(customerData.getColor(), customerData.getAccessory());
        onStateChange?.Invoke(customerData.getState());
        timer = timerDuration;

        if (dialogueBoxPrefab != null)
        {
            dialogueBoxInstance = Instantiate(dialogueBoxPrefab, transform, false);
            dialogueBoxInstance.CreateDialogueBox(
                customerData.getOrder().getBingsuOrder(),
                transform,
                dialogueOffset,
                dialogueScale
            );
        }
        else
        {
            Debug.LogWarning("Customer: DialogueBoxPrefab is not assigned!");
        }
    }

    void Update()
    {
        if (orderDelivered) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            orderDelivered = true;
            onStateChange?.Invoke("angry");
            onTimerEnd?.Invoke(this);

            if (dialogueBoxInstance != null)
                dialogueBoxInstance.FadeOutAndDestroy();
        }
        else if (timer <= timerDuration * timerPercentAngry)
        {
            if (!isAngry)
            {
                isAngry = true;
                onStateChange?.Invoke("angry");
            }

            float angerDuration = timerDuration * timerPercentAngry;
            float remainingAngerTime = timer;
            float blinkRate = Mathf.Lerp(minBlinkRate, baseBlinkRate, remainingAngerTime / angerDuration);

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
