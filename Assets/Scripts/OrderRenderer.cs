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
            return;
        }

        // Create child renderers if missing
        if (trayRenderer == null)
        {
            GameObject trayObj = new GameObject("TrayRenderer");
            trayObj.transform.SetParent(transform);
            trayObj.transform.localPosition = Vector3.zero;
            trayRenderer = trayObj.AddComponent<SpriteRenderer>();
        }

        if (condensedMilkRenderer == null)
        {
            GameObject milkObj = new GameObject("CondensedMilkRenderer");
            milkObj.transform.SetParent(transform);
            milkObj.transform.localPosition = Vector3.zero;
            condensedMilkRenderer = milkObj.AddComponent<SpriteRenderer>();
        }

        // Sorting setup
        trayRenderer.sortingLayerName = "Bingsu";
        trayRenderer.sortingOrder = 1;
        condensedMilkRenderer.sortingLayerName = "Bingsu";
        condensedMilkRenderer.sortingOrder = 2;

        // Hide initially
        trayRenderer.sprite = null;
        condensedMilkRenderer.sprite = null;
        trayRenderer.enabled = false;
        condensedMilkRenderer.enabled = false;

        // Trigger any starting state
        orderParent.OrderSpriteSignals();
    }

    private void ShowTray()
    {
        trayRenderer.sprite = traySprite;
        trayRenderer.enabled = true;
    }

    private void ShowCondensedMilk()
    {
        condensedMilkRenderer.sprite = condensedMilkSprite;
        condensedMilkRenderer.enabled = true;
    }
}
