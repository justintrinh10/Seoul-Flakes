using UnityEngine;
using System.Linq;

[System.Serializable]
public class ToppingOffset
{
    public string toppingName;
    public Vector3 localPosition;  // custom offset for this topping
}

public class BingsuRenderer : MonoBehaviour
{
    private Bingsu bingsuParent;

    private SpriteRenderer baseRenderer;
    private SpriteRenderer toppingRenderer;
    private SpriteRenderer logoRenderer;

    [Header("Sprites for Parts")]
    public Sprite[] baseSprites;
    public Sprite[] toppingSprites;
    public Sprite logoSprite;

    [Header("Topping Offsets")]
    public ToppingOffset[] toppingOffsets;

    private void OnEnable()
    {
        Bingsu.onAddBowl += ShowBowl;
        Bingsu.onAddShavedMilk += ShowShavedMilk;
        Bingsu.onAddDrizzle += ShowDrizzle;
        Bingsu.onAddLogo += ShowLogo;
        Bingsu.onAddBaseTopping += ShowBaseTopping;
        Bingsu.onAddTopping += ShowTopping;
    }

    private void OnDisable()
    {
        Bingsu.onAddBowl -= ShowBowl;
        Bingsu.onAddShavedMilk -= ShowShavedMilk;
        Bingsu.onAddDrizzle -= ShowDrizzle;
        Bingsu.onAddLogo -= ShowLogo;
        Bingsu.onAddBaseTopping -= ShowBaseTopping;
        Bingsu.onAddTopping -= ShowTopping;
    }

    private void Start()
    {
        bingsuParent = transform.parent.GetComponent<Bingsu>();
        if (bingsuParent == null)
        {
            Debug.LogError("No Bingsu component found on parent!");
        }

        ToppingOffset temp = new ToppingOffset();
        temp.toppingName = "tiramisu";
        temp.localPosition = new Vector3(-0.1f, 5.57f, 0.0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "bungeoppang";
        temp.localPosition = new Vector3(-0.39f, 7.56f, 0.0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "chocolateBar";
        temp.localPosition = new Vector3(2.67f, 4.74f, 0.0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "cheeseCake";
        temp.localPosition = new Vector3(-0.02f, 5.02f, 0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "matcha";
        temp.localPosition = new Vector3(-0.07f, 5.58f, 0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "vanilla";
        temp.localPosition = new Vector3(-0.07f, 5.58f, 0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "chocolate";
        temp.localPosition = new Vector3(-0.07f, 5.58f, 0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "ube";
        temp.localPosition = new Vector3(-0.07f, 5.58f, 0f);
        toppingOffsets.Append(temp);
        temp.toppingName = "mango";
        temp.localPosition = new Vector3(-0.07f, 5.58f, 0f);
        toppingOffsets.Append(temp);

        // Create child renderers if they don't exist
        if (baseRenderer == null)
        {
            GameObject baseObj = new GameObject("BaseRenderer");
            baseObj.transform.SetParent(transform);
            baseObj.transform.localPosition = Vector3.zero;
            baseRenderer = baseObj.AddComponent<SpriteRenderer>();
            baseRenderer.sortingLayerName = "Bingsu";
            baseRenderer.sortingOrder = 3;
            baseRenderer.enabled = false;
        }

        if (toppingRenderer == null)
        {
            GameObject topObj = new GameObject("ToppingRenderer");
            topObj.transform.SetParent(transform);
            topObj.transform.localPosition = Vector3.zero;
            toppingRenderer = topObj.AddComponent<SpriteRenderer>();
            toppingRenderer.sortingLayerName = "Bingsu";
            toppingRenderer.sortingOrder = 4;
            toppingRenderer.enabled = false;
        }

        if (logoRenderer == null)
        {
            GameObject logoObj = new GameObject("LogoRenderer");
            logoObj.transform.SetParent(transform);
            logoObj.transform.localPosition = Vector3.zero;
            logoRenderer = logoObj.AddComponent<SpriteRenderer>();
            logoRenderer.sortingLayerName = "Bingsu";
            logoRenderer.sortingOrder = 5;
            logoRenderer.enabled = false;
        }
    }

    // --- Event callbacks ---
    private void ShowBowl()
    {
        baseRenderer.sprite = FindSprite(baseSprites, "emptyBowl");
        baseRenderer.enabled = true;
    }

    private void ShowShavedMilk()
    {
        baseRenderer.sprite = FindSprite(baseSprites, "fullBowl");
        baseRenderer.enabled = true;
    }

    private void ShowBaseTopping(string type)
    {
        baseRenderer.sprite = FindSprite(baseSprites, type);
        baseRenderer.enabled = true;
    }

    private void ShowDrizzle()
    {
        if (baseRenderer.sprite == null) return;

        string baseName = baseRenderer.sprite.name;
        string spriteName = "drizzle" + char.ToUpper(baseName[0]) + baseName.Substring(1);
        baseRenderer.sprite = FindSprite(baseSprites, spriteName);
        baseRenderer.enabled = true;
    }

    private void ShowLogo()
    {
        logoRenderer.sprite = logoSprite;
        logoRenderer.transform.localPosition = new Vector3(0, 0.51f, 0);
        logoRenderer.enabled = true;
    }

    private void ShowTopping(string type)
    {
        string spriteName = "topping" + char.ToUpper(type[0]) + type.Substring(1);
        toppingRenderer.sprite = FindSprite(toppingSprites, spriteName);

        // Apply local position for this topping type
        Vector3 offset = Vector3.zero;
        foreach (var t in toppingOffsets)
        {
            if (t.toppingName == type)
            {
                offset = t.localPosition;
                break;
            }
        }
        toppingRenderer.transform.localPosition = offset;

        toppingRenderer.enabled = true;
    }

    // --- Utility ---
    private Sprite FindSprite(Sprite[] sprites, string name)
    {
        foreach (var s in sprites)
        {
            if (s.name == name) return s;
        }
        Debug.LogWarning($"Sprite '{name}' not found!");
        return null;
    }
}
