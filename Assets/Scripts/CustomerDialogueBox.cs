using System;
using UnityEngine;

public class CustomerDialogueBox : MonoBehaviour
{
    BingsuData bingsuData;
    private SpriteRenderer textBoxRenderer;
    private SpriteRenderer baseIconRenderer;
    private SpriteRenderer toppingIconRenderer;
    private SpriteRenderer drizzleIconRenderer;
    private Vector3 baseIconOffset;
    private Vector3 toppingIconOffset;
    private Vector3 drizzleIconOffset;

    [Header("Sprites for Parts")]
    public Sprite textBox;
    public Sprite[] baseSprites;
    public Sprite[] toppingSprites;
    public Sprite drizzleSprite;
    void Start()
    {
        baseIconOffset = new Vector3(-1f, 1f, 0f);
        toppingIconOffset = new Vector3(1f, 1f, 0f);
        drizzleIconOffset = new Vector3(0f, 0f, 0f);
        if (textBoxRenderer == null)
        {
            GameObject baseObj = new GameObject("TextBoxRenderer");
            baseObj.transform.SetParent(transform);
            baseObj.transform.localPosition = Vector3.zero;
            textBoxRenderer = baseObj.AddComponent<SpriteRenderer>();
            textBoxRenderer.sortingLayerName = "Customer";
            textBoxRenderer.sortingOrder = 4;
            textBoxRenderer.enabled = true;
        }
        if (baseIconRenderer == null)
        {
            GameObject baseObj = new GameObject("BaseIconRenderer");
            baseObj.transform.SetParent(transform);
            baseObj.transform.localPosition = baseIconOffset;
            baseIconRenderer = baseObj.AddComponent<SpriteRenderer>();
            baseIconRenderer.sortingLayerName = "Customer";
            baseIconRenderer.sortingOrder = 5;
            baseIconRenderer.enabled = false;
        }
        if (toppingIconRenderer == null)
        {
            GameObject baseObj = new GameObject("ToppingIconRenderer");
            baseObj.transform.SetParent(transform);
            baseObj.transform.localPosition = toppingIconOffset;
            toppingIconRenderer = baseObj.AddComponent<SpriteRenderer>();
            toppingIconRenderer.sortingLayerName = "Customer";
            toppingIconRenderer.sortingOrder = 5;
            toppingIconRenderer.enabled = false;
        }
        if (drizzleIconRenderer == null)
        {
            GameObject baseObj = new GameObject("DrizzleIconRenderer");
            baseObj.transform.SetParent(transform);
            baseObj.transform.localPosition = drizzleIconOffset;
            drizzleIconRenderer = baseObj.AddComponent<SpriteRenderer>();
            drizzleIconRenderer.sortingLayerName = "Customer";
            drizzleIconRenderer.sortingOrder = 5;
            drizzleIconRenderer.enabled = false;
        }

    }

    public void createDialogueBox(BingsuData bData)
    {
        bingsuData = bData;
        if (bingsuData.bingsuComplete())
        {
            string baseIconName = "base" + char.ToUpper(bingsuData.getBaseToppingType()[0]) + bingsuData.getBaseToppingType().Substring(1);
            foreach (var s in baseSprites)
            {
                if (s.name == name)
                {
                    baseIconRenderer.sprite = s;
                    baseIconRenderer.enabled = true;
                }
            }
            string toppingIconName = "topping" + char.ToUpper(bingsuData.getToppingType()[0]) + bingsuData.getToppingType().Substring(1);
            foreach (var s in baseSprites)
            {
                if (s.name == name)
                {
                    toppingIconRenderer.sprite = s;
                    toppingIconRenderer.enabled = true;
                }
            }
            if (bingsuData.hasDrizzle())
            {
                drizzleIconRenderer.sprite = drizzleSprite;
                drizzleIconRenderer.enabled = true;
            }
        }
    }
}