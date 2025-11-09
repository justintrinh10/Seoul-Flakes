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
    public GameManager gameManager;
    Order currentOrder;
    Bingsu currentBingsu;
    public static event Action onError;
    public static event Action onPlaceDish;
    public static event Action onThrowTrash;
    public static event Action onPlaceFood;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateNewOrder();
    }

    private void Awake()
    {
        // auto-find workspace manager if not assigned
        if (workspaceManager == null)
            workspaceManager = FindObjectOfType<WorkspaceManager>();

        // try to auto-find minigame instances if left unassigned
        if (shavedIceMachine == null)
            shavedIceMachine = FindObjectOfType<ShavedIceMachine>();
        if (bungeoppangMinigame == null)
            bungeoppangMinigame = FindObjectOfType<BungeoppangMinigame>();

        // Basic prefab validation (helpful runtime errors if scene not wired)
        if (orderPrefab == null)
        {
            Debug.LogWarning("OrderCreationManager: 'orderPrefab' is not assigned in the inspector. CreateNewOrder will fail until it is assigned.");
        }
        if (bingsuPrefab == null)
        {
            Debug.LogWarning("OrderCreationManager: 'bingsuPrefab' is not assigned in the inspector. CreateNewOrder will fail until it is assigned.");
        }
    }

    public void CreateNewOrder()
    {
        // Remove any old ones
        if (currentOrder != null)
            Destroy(currentOrder.gameObject);
        if (currentBingsu != null)
            Destroy(currentBingsu.gameObject);

        // Validate prefabs
        if (orderPrefab == null || bingsuPrefab == null)
        {
            Debug.LogError("OrderCreationManager.CreateNewOrder: Missing prefab(s). Ensure 'orderPrefab' and 'bingsuPrefab' are assigned in the inspector.");
            onError?.Invoke();
            return;
        }

        // Instantiate fresh ones
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

    /// <summary>
    /// Public helper to create a new, empty order from scratch. Can be wired to a UI Button.
    /// </summary>
    public void CreateOrderFromScratch()
    {
        CreateNewOrder();
    }

    public void onCustomer1Click()
    {
        if (gameManager != null)
        {
            if (gameManager.activeCustomers[0])
            {
                Debug.Log("Attempting delivery to Customer 1...");
                bool result = gameManager.TryDeliverOrderToCustomer(gameManager.currentCustomers[0], currentOrder);
                Debug.Log("Delivery result: " + result);
            }
            else
            {
                Debug.LogWarning("Customer 1 not active!");
            }
        }
        else
        {
            Debug.LogError("GameManager reference missing!");
        }
    }


    public void onCustomer2Click()
    {
        if (gameManager != null)
        {
            if (gameManager.activeCustomers[1])
            {
                Debug.Log("Attempting delivery to Customer 2...");
                bool result = gameManager.TryDeliverOrderToCustomer(gameManager.currentCustomers[1], currentOrder);
                Debug.Log("Delivery result: " + result);
            }
            else
            {
                Debug.LogWarning("Customer 2 not active!");
            }
        }
        else
        {
            Debug.LogError("GameManager reference missing!");
        }
    }

    public void onCustomer3Click()
    {
        if (gameManager != null)
        {
            if (gameManager.activeCustomers[2])
            {
                Debug.Log("Attempting delivery to Customer 3...");
                bool result = gameManager.TryDeliverOrderToCustomer(gameManager.currentCustomers[2], currentOrder);
                Debug.Log("Delivery result: " + result);
            }
            else
            {
                Debug.LogWarning("Customer 3 not active!");
            }
        }
        else
        {
            Debug.LogError("GameManager reference missing!");
        }        
    }

    public void onMatchaIceClick()
    {
        if (currentBingsu.addTopping("matcha"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onVanillaIceClick()
    {
        if (currentBingsu.addTopping("vanilla"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onChocolateIceClick()
    {
        if (currentBingsu.addTopping("chocolate"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onMangoIceClick()
    {
        if (currentBingsu.addTopping("mango"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onUbeIceClick()
    {
        if (currentBingsu.addTopping("ube"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onIceMachineClick()
    {
        // Only allow shaved-ice minigame when left workspace is active
        if (workspaceManager != null && workspaceManager.GetCurrentIndex() != 0)
        {
            // not on left workspace
            onError?.Invoke();
            return;
        }
        // Direct behavior: add shaved milk immediately (no minigame)
        if (currentBingsu.addShavedMilk())
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onChocolateLogoClick()
    {
        if (currentBingsu.addLogo())
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onTrashClick()
    {
        clearOrder();
        onThrowTrash?.Invoke();
    }

    public void onCondensedMilkClick()
    {
        if (currentOrder.AddCondensedMilk())
        {
            displayOrder();
            onPlaceDish?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onStrawberryClick()
    {
        if (currentBingsu.addBaseTopping("strawberry"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onMangoClick()
    {
        if (currentBingsu.addBaseTopping("mango"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onUbeClick()
    {
        if (currentBingsu.addBaseTopping("ube"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onChocolateClick()
    {
        if (currentBingsu.addBaseTopping("chocolate"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onInjeolmiClick()
    {
        if (currentBingsu.addBaseTopping("injeolmi"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onPatClick()
    {
        if (currentBingsu.addBaseTopping("pat"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onTrayClick()
    {
        if (currentOrder.AddTray())
        {
            displayOrder();
            onPlaceDish?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onBowlClick()
    {
        if (currentOrder.getOrderData().hasTray() && currentBingsu.addBowl())
        {
            displayOrder();
            onPlaceDish?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onDrizzleClick()
    {
        if (currentBingsu.addDrizzle())
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onBungeoppangClick()
    {
        // Only allow bungeoppang minigame when right workspace is active
        if (workspaceManager != null && workspaceManager.GetCurrentIndex() != 2)
        {
            onError?.Invoke();
            return;
        }
        // Direct behavior: add bungeoppang topping immediately (no minigame)
        if (currentBingsu.addTopping("bungeoppang"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onCheeseCakeClick()
    {
        if (currentBingsu.addTopping("cheeseCake"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void onChocolateBarClick()
    {
        if (currentBingsu.addTopping("chocolateBar"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void TiramisuClick()
    {
        if (currentBingsu.addTopping("tiramisu"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    public void clearOrder()
    {
        CreateNewOrder();
        displayOrder();
    }

    public void displayOrder()
    {
        currentOrder.OrderSpriteSignals();
        currentBingsu.bingsuSpriteSignals();
    }

    /// <summary>
    /// Called by a Customer (or other caller) to attempt delivering the currently-built order to that customer.
    /// This will locate the GameManager and ask it to deliver by customer reference. If delivery is attempted
    /// we create a fresh order afterwards.
    /// </summary>
    public void DeliverCurrentOrderToCustomer(Customer customer)
    {
        if (customer == null)
        {
            onError?.Invoke();
            return;
        }

        if (currentOrder == null)
        {
            Debug.LogWarning("OrderCreationManager.DeliverCurrentOrderToCustomer: No current order to deliver.");
            onError?.Invoke();
            return;
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null)
        {
            Debug.LogWarning("OrderCreationManager.DeliverCurrentOrderToCustomer: No GameManager found in scene.");
            onError?.Invoke();
            return;
        }

        bool accepted = gm.TryDeliverOrderToCustomer(customer, currentOrder);

        // Remove the current UI order regardless of correctness and start a fresh one
        if (currentOrder != null)
            Destroy(currentOrder.gameObject);
        if (currentBingsu != null)
            Destroy(currentBingsu.gameObject);

        currentOrder = null;
        currentBingsu = null;

        // Create a new empty order for next customer
        CreateNewOrder();

        if (accepted)
            onPlaceFood?.Invoke();
        else
            onError?.Invoke();
    }
}