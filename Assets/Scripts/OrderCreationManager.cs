using System;
using UnityEngine;

public class OrderCreationManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject orderPrefab;
    public GameObject bingsuPrefab;
    
    [Header("Minigames")]
    [Tooltip("Optional: assign the Shaved Ice machine instance (ShavedIceMachine)")]
    public ShavedIceMachine shavedIceMachine;
    
    [Tooltip("Optional: assign the Bungeoppang minigame manager")]
    public BungeoppangMinigame bungeoppangMinigame;
    
    [Tooltip("Optional: assign the WorkspaceManager to check which workspace is active")] 
    public WorkspaceManager workspaceManager;
    
    private Order currentOrder;
    private Bingsu currentBingsu;

    public static event Action onError;
    public static event Action onPlaceDish;
    public static event Action onThrowTrash;
    public static event Action onPlaceFood;

    void Start()
    {
        CreateNewOrder();
    }

    private void Awake()
    {
        // Auto-find workspace manager if not assigned
        if (workspaceManager == null)
            workspaceManager = FindObjectOfType<WorkspaceManager>();

        // Try to auto-find minigame instances if left unassigned
        if (shavedIceMachine == null)
            shavedIceMachine = FindObjectOfType<ShavedIceMachine>();
        if (bungeoppangMinigame == null)
            bungeoppangMinigame = FindObjectOfType<BungeoppangMinigame>();

        // Validate prefabs
        if (!ValidatePrefab(orderPrefab, "OrderPrefab") || !ValidatePrefab(bingsuPrefab, "BingsuPrefab"))
            return;
    }

    private bool ValidatePrefab(GameObject prefab, string prefabName)
    {
        if (prefab == null)
        {
            Debug.LogError($"{prefabName} is not assigned in the inspector.");
            onError?.Invoke();
            return false;
        }
        return true;
    }

    public void CreateNewOrder()
    {
        // Remove old orders if they exist
        if (currentOrder != null)
            Destroy(currentOrder.gameObject);
        if (currentBingsu != null)
            Destroy(currentBingsu.gameObject);

        // Instantiate fresh prefabs
        GameObject orderObj = Instantiate(orderPrefab, new Vector3(-0.38f, -3.12f, 0), Quaternion.identity);
        orderObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        currentOrder = orderObj.GetComponent<Order>();
        
        if (currentOrder == null)
        {
            Debug.LogError("OrderCreationManager.CreateNewOrder: Instantiated 'orderPrefab' does not contain an Order component.");
            Destroy(orderObj);
            onError?.Invoke();
            return;
        }

        GameObject bingsuObj = Instantiate(bingsuPrefab, new Vector3(-0.51f, -2.65f, 0), Quaternion.identity);
        bingsuObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        currentBingsu = bingsuObj.GetComponent<Bingsu>();

        if (currentBingsu == null)
        {
            Debug.LogError("OrderCreationManager.CreateNewOrder: Instantiated 'bingsuPrefab' does not contain a Bingsu component.");
            Destroy(orderObj);
            Destroy(bingsuObj);
            onError?.Invoke();
            return;
        }

        // Wire them together
        currentOrder.AssignBingsu(currentBingsu);

        // Update visuals
        displayOrder();
    }

    private void displayOrder()
    {
        currentOrder.OrderSpriteSignals();
        currentBingsu.bingsuSpriteSignals();
    }

    public void AddTopping(string topping)
    {
        if (currentBingsu.addTopping(topping))
        {
            displayOrder();
            onPlaceFood?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    // Topping Click Methods
    public void onMatchaIceClick() => AddTopping("matcha");
    public void onVanillaIceClick() => AddTopping("vanilla");
    public void onChocolateIceClick() => AddTopping("chocolate");
    public void onMangoIceClick() => AddTopping("mango");
    public void onUbeIceClick() => AddTopping("ube");

    public void AddBaseTopping(string topping)
    {
        if (currentBingsu.addBaseTopping(topping))
        {
            displayOrder();
            onPlaceFood?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    // Base Topping Click Methods
    public void onStrawberryClick() => AddBaseTopping("strawberry");
    public void onMangoClick() => AddBaseTopping("mango");
    public void onUbeClick() => AddBaseTopping("ube");
    public void onChocolateClick() => AddBaseTopping("chocolate");
    public void onInjeolmiClick() => AddBaseTopping("injeolmi");
    public void onPatClick() => AddBaseTopping("pat");

    // Direct Item Click Methods
    public void onIceMachineClick()
    {
        if (IsInCorrectWorkspace(0) && currentBingsu.addShavedMilk())
        {
            displayOrder();
            onPlaceFood?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public void onBungeoppangClick()
    {
        if (IsInCorrectWorkspace(2) && currentBingsu.addTopping("bungeoppang"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public void onCondensedMilkClick()
    {
        if (currentOrder.AddCondensedMilk())
        {
            displayOrder();
            onPlaceDish?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public void onTrayClick()
    {
        if (currentOrder.AddTray())
        {
            displayOrder();
            onPlaceDish?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public void onBowlClick()
    {
        if (currentOrder.getOrderData().hasTray() && currentBingsu.addBowl())
        {
            displayOrder();
            onPlaceDish?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public void onDrizzleClick()
    {
        if (currentBingsu.addDrizzle())
        {
            displayOrder();
            onPlaceFood?.Invoke();
        }
        else
        {
            onError?.Invoke();
        }
    }

    public void onTrashClick()
    {
        CreateNewOrder();
        onThrowTrash?.Invoke();
    }

    public void clearOrder()
    {
        CreateNewOrder();
        displayOrder();
    }

    private bool IsInCorrectWorkspace(int requiredIndex)
    {
        return workspaceManager != null && workspaceManager.GetCurrentIndex() == requiredIndex;
    }

    public void DeliverCurrentOrderToCustomer(Customer customer)
    {
        if (customer == null || currentOrder == null)
        {
            onError?.Invoke();
            return;
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null || !gm.TryDeliverOrderToCustomer(customer, currentOrder))
        {
            onError?.Invoke();
        }
        else
        {
            onPlaceFood?.Invoke();
        }

        CreateNewOrder(); // Reset for the next order
    }
}
