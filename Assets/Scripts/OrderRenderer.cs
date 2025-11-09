using UnityEngine;

public class OrderRenderer : MonoBehaviour
{
    [Header("Order Parts")]
    public SpriteRenderer trayRenderer;
    public SpriteRenderer condensedMilkRenderer;

    [Header("Sprites")]
    public Sprite traySprite;
    public Sprite condensedMilkSprite;

    private Order orderParent;

    private void OnEnable()
    {
        Order.onAddTray += ShowTray;
        Order.onAddCondensedMilk += ShowCondensedMilk;
    }

    private void OnDisable()
    {
        Order.onAddTray -= ShowTray;
        Order.onAddCondensedMilk -= ShowCondensedMilk;
    }

    private void Start()
    {
        orderParent = GetComponent<Order>();
        if (orderParent == null)
        {
            Debug.LogError("Order component not found on this GameObject!");
        }

        // Initial render
        orderParent.OrderSpriteSignals();
        if (trayRenderer == null)
        {
            trayRenderer = gameObject.AddComponent<SpriteRenderer>();

        }
        if (condensedMilkRenderer == null)
        {
            condensedMilkRenderer = gameObject.AddComponent<SpriteRenderer>();
 
        }
    }

    private void ShowTray()
    {
        if (trayRenderer != null)
        {
            trayRenderer.sprite = traySprite;
            trayRenderer.enabled = true;
        }
    }

    private void ShowCondensedMilk()
    {
        if (condensedMilkRenderer != null)
        {
            condensedMilkRenderer.sprite = condensedMilkSprite;
            condensedMilkRenderer.enabled = true;
        }
    }
}
