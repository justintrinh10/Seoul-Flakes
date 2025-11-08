using System;

public class Customer
{
    private Order currentOrder;

    public Customer()
    {
        currentOrder = new Order();
    }

    public void randomCustomerOrder()
    {
        currentOrder.randomOrder();
    }

    public void setOrder(Order order)
    {
        currentOrder = order;
    }

    public Order GetCurrentOrder()
    {
        return currentOrder;
    }
}
