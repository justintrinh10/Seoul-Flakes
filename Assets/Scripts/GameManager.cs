using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector2 customerCoolDownRange = new Vector2(5, 10);
    private float customerCoolDownTimer;
    private int numCustomer = 3;
    private Customer[] currentCustomers;
    private bool[] activeCustomers;
    private Vector2[] customerLocation = { new Vector2(-2, 1), new Vector2(0, 1), new Vector2(2, 1) };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clearAllCustomers();
        customerCoolDownTimer = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        customerCoolDownTimer -= Time.deltaTime;
        if(customerCoolDownTimer <= 0.0f)
        {
            Customer newCustomer = new Customer();
            queueCustomer(newCustomer);
            customerCoolDownTimer = UnityEngine.Random.Range(customerCoolDownRange.x, customerCoolDownRange.y);
        }
    }

    void queueCustomer(Customer newCustomer)
    {
        int spot = findFreeCustomerSpace();
        if(spot >= 0)
        {
            currentCustomers[spot] = newCustomer;
            activeCustomers[spot] = true;
            newCustomer.transform.position = customerLocation[spot];
        }
    }
    
    int findFreeCustomerSpace()
    {
        int spot = -1;
        for (int i = 0; i < numCustomer; i++)
        {
            if (!activeCustomers[i])
            {
                spot = i;
                break;
            }
        }
        return spot;
    }
    
    void clearAllCustomers()
    {
        for(int i = 0; i < numCustomer; i++)
        {
            activeCustomers[i] = false;
        }
    }
}
