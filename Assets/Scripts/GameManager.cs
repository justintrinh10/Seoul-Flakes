using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector2 customerCoolDownRange = new Vector2(5, 10);
    private float customerCoolDownTimer;
    private int numCustomer = 3;
    private Queue<Customer> customerQueue = new Queue<Customer>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customerCoolDownTimer = UnityEngine.Random.Range(customerCoolDownRange.x, customerCoolDownRange.y);
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

    void queueCustomer(Customer newCutomer)
    {
        
    }
}
