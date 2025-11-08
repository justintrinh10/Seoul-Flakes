using System;

public class Customer
{
    private Order currentOrder;

    public Customer()
    {
        currentOrder = new Order();
    }

    public void PlaceRandomOrder()
    {
        currentOrder.randomOrder();
    }

    public Order GetCurrentOrder()
    {
        return currentOrder;
    }
}
