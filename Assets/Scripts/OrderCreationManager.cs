using System;
using UnityEngine;

public class OrderCreationManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject orderPrefab;
    public GameObject bingsuPrefab;
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

    public void CreateNewOrder()
    {
        // Remove any old ones
        if (currentOrder != null)
            Destroy(currentOrder.gameObject);
        if (currentBingsu != null)
            Destroy(currentBingsu.gameObject);

        // Instantiate fresh ones
        GameObject orderObj = Instantiate(orderPrefab, new Vector3(-0.38f, -3.12f, 0), Quaternion.identity);
        orderObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        currentOrder = orderObj.GetComponent<Order>();

        GameObject bingsuObj = Instantiate(bingsuPrefab, new Vector3(-0.51f, -2.65f, 0), Quaternion.identity);
        bingsuObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        currentBingsu = bingsuObj.GetComponent<Bingsu>();

        currentOrder.AssignBingsu(currentBingsu);

        displayOrder();
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
        if (currentBingsu.addShavedMilk())
        {
            //add Ice machine minigame here
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
        if (currentBingsu.addTopping("bungeoppang"))
        {
            //add Ice machine minigame here
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
}
