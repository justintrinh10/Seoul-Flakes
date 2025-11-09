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

    [Header("Prefabs")]
    public GameObject dialogueBoxPrefab;  // Assign in Inspector

    private CustomerDialogueBox dialogueBoxInstance;

    void Start()
    {
        // Set up customer data
        customerData = new CustomerData();
        customerData.randomCustomer();

        // Update visuals
        changeAppearance?.Invoke(customerData.getColor(), customerData.getAccessory());
        onStateChange?.Invoke(customerData.getState());

        // Spawn and configure dialogue box
        if (dialogueBoxPrefab != null)
        {
            GameObject dialogueObj = Instantiate(dialogueBoxPrefab, transform);
            dialogueObj.transform.localPosition = new Vector3(0, 2f, 0); // above customer’s head
            dialogueBoxInstance = dialogueObj.GetComponent<CustomerDialogueBox>();
            dialogueBoxInstance.createDialogueBox(customerData.getOrder().getBingsuOrder());
        }

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

    private void OnDestroy()
    {
        if (dialogueBoxInstance != null)
        {
            Destroy(dialogueBoxInstance.gameObject);
        }
    }
}
