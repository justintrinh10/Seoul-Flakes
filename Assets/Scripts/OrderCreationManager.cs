using System;
using UnityEngine;

public class OrderCreationManager : MonoBehaviour
{
    Order currentOrder;
    Bingsu currentBingsu;
    public static event Action onError;
    public static event Action onPlaceDish;
    public static event Action onThrowTrash;
    public static event Action onPlaceFood;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentOrder = new Order();
        currentOrder.transform.position = new Vector3(-0.38f, -3.12f, 0);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        currentBingsu = new Bingsu();
        currentBingsu.transform.position = new Vector3(-0.51f, -2.65f, 0);
        currentBingsu.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    }

    void onMatchaIceClick()
    {
        if (currentBingsu.addTopping("matcha"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onVanillaIceClick()
    {
        if (currentBingsu.addTopping("vanilla"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onChocolateIceClick()
    {
        if (currentBingsu.addTopping("chocolate"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onMangoIceClick()
    {
        if (currentBingsu.addTopping("mango"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onUbeIceClick()
    {
        if (currentBingsu.addTopping("ube"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onIceMachineClick()
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

    void onChocolateLogoClick()
    {
        if (currentBingsu.addLogo())
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onTrashClick()
    {
        clearOrder();
        onThrowTrash?.Invoke();
    }

    void onCondensedMilkClick()
    {
        if (currentOrder.AddCondensedMilk())
        {
            displayOrder();
            onPlaceDish?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onStrawberryClick()
    {
        if (currentBingsu.addBaseTopping("strawberry"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onMangoClick()
    {
        if (currentBingsu.addBaseTopping("mango"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onUbeClick()
    {
        if (currentBingsu.addBaseTopping("ube"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onChocolateClick()
    {
        if (currentBingsu.addBaseTopping("chocolate"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onInjeolmiClick()
    {
        if (currentBingsu.addBaseTopping("injeolmi"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onPatClick()
    {
        if (currentBingsu.addBaseTopping("pat"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onTrayClick()
    {
        if (currentOrder.AddTray())
        {
            displayOrder();
            onPlaceDish?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onBowlClick()
    {
        if (currentOrder.getOrderData().hasTray() && currentBingsu.addBowl())
        {
            displayOrder();
            onPlaceDish?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onDrizzleClick()
    {
        if (currentBingsu.addDrizzle())
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onBungeoppangClick()
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

    void onCheeseCakeClick()
    {
        if (currentBingsu.addTopping("cheeseCake"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void onChocolateBarClick()
    {
        if (currentBingsu.addTopping("chocolateBar"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void TiramisuClick()
    {
        if (currentBingsu.addTopping("tiramisu"))
        {
            displayOrder();
            onPlaceFood?.Invoke();
            return;
        }
        onError?.Invoke();
    }

    void clearOrder()
    {
        OrderData temp1 = new OrderData();
        currentOrder.createOrder(temp1);
        BingsuData temp2 = new BingsuData();
        currentBingsu.createBingsu(temp2);
        displayOrder();
    }

    void displayOrder()
    {
        currentOrder.OrderSpriteSignals();
        currentBingsu.bingsuSpriteSignals();
    }
}
