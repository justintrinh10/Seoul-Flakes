using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject customerPrefab;
    private Vector2 customerCoolDownRange = new Vector2(5, 10);
    private float customerCoolDownTimer;
    private int numCustomer = 3;

    private Customer[] currentCustomers;
    private bool[] activeCustomers;
    private Vector3[] customerLocation = {
        new Vector3(-2, 1, 0),
        new Vector3(0, 1, 0),
        new Vector3(2, 1, 0)
    };

    void Start()
    {
        currentCustomers = new Customer[numCustomer];
        activeCustomers = new bool[numCustomer];
        clearAllCustomers();
        customerCoolDownTimer = 1.0f;
    }

    void Update()
    {
        customerCoolDownTimer -= Time.deltaTime;
        if (customerCoolDownTimer <= 0.0f)
        {
            queueCustomer();
            customerCoolDownTimer = UnityEngine.Random.Range(customerCoolDownRange.x, customerCoolDownRange.y);
        }
    }

    void queueCustomer()
    {
        int spot = findFreeCustomerSpace();
        if (spot >= 0)
        {
            GameObject customerObj = Instantiate(customerPrefab);
            Customer customerScript = customerObj.GetComponent<Customer>();

            currentCustomers[spot] = customerScript;
            activeCustomers[spot] = true;
            customerObj.transform.position = customerLocation[spot];
        }
    }

    int findFreeCustomerSpace()
    {
        for (int i = 0; i < numCustomer; i++)
        {
            if (!activeCustomers[i])
                return i;
        }
        return -1;
    }

    void clearAllCustomers()
    {
        for (int i = 0; i < numCustomer; i++)
        {
            activeCustomers[i] = false;
        }
    }
}
