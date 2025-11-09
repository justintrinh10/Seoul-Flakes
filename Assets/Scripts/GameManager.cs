using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Vector2 customerCoolDownRange = new Vector2(5f, 10f);
    [SerializeField] private float customerDespawnDelay = 2f;
    [SerializeField] private int numCustomer = 3;

    private float customerCoolDownTimer;
    public Customer[] currentCustomers;
    public bool[] activeCustomers;

    private Vector3[] customerLocations = {
        new Vector3(-2, 1, 0),
        new Vector3(0, 1, 0),
        new Vector3(2, 1, 0)
    };

    void OnEnable()
    {
        Customer.onTimerEnd += HandleCustomerTimerEnd;
    }

    void OnDisable()
    {
        Customer.onTimerEnd -= HandleCustomerTimerEnd;
    }

    void Start()
    {
        currentCustomers = new Customer[numCustomer];
        activeCustomers = new bool[numCustomer];
        ClearAllCustomers();
        customerCoolDownTimer = 1.0f;
    }

    void Update()
    {
        customerCoolDownTimer -= Time.deltaTime;

        if (customerCoolDownTimer <= 0.0f)
        {
            QueueCustomer();
            customerCoolDownTimer = UnityEngine.Random.Range(customerCoolDownRange.x, customerCoolDownRange.y);
        }
    }

    public void QueueCustomer()
    {
        int spot = FindFreeCustomerSpot();
        if (spot < 0) return;

        GameObject customerObj = Instantiate(customerPrefab, customerLocations[spot], Quaternion.identity);
        Customer customerScript = customerObj.GetComponent<Customer>();

        currentCustomers[spot] = customerScript;
        activeCustomers[spot] = true;

        Debug.Log($"Spawned customer at spot {spot}");
    }

    /// <summary>
    /// Attempt to deliver an order to a specific Customer instance.
    /// Returns true if the customer accepted the order (matched their request), false otherwise.
    /// </summary>
    public bool TryDeliverOrderToCustomer(int customer, Order order)
    {
        if (order == null) return false;

        bool correct = currentCustomers[customer].DeliverOrder(order);
        StartCoroutine(HandleCustomerExit(customer, correct));
        return correct;
    }

    private IEnumerator HandleCustomerExit(int index, bool correct)
    {
        string mood = correct ? "happy" : "angry";
        Debug.Log($"Customer {index} is {mood} and will leave soon...");

        yield return new WaitForSeconds(customerDespawnDelay);

        Destroy(currentCustomers[index].gameObject);
        currentCustomers[index] = null;
        activeCustomers[index] = false;

        Debug.Log($"Customer at spot {index} has left.");
    }

    private void HandleCustomerTimerEnd(Customer expiredCustomer)
    {
        for (int i = 0; i < numCustomer; i++)
        {
            if (currentCustomers[i] == expiredCustomer)
            {
                Debug.Log($"Customer {i}'s timer ended, removing them...");
                StartCoroutine(HandleCustomerExit(i, false)); 
                break;
            }
        }
    }

    private int FindFreeCustomerSpot()
    {
        for (int i = 0; i < numCustomer; i++)
        {
            if (!activeCustomers[i])
                return i;
        }
        return -1;
    }

    private void ClearAllCustomers()
    {
        for (int i = 0; i < numCustomer; i++)
            activeCustomers[i] = false;
    }
}
