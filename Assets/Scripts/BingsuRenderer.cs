using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    [Header("Topping Offsets")]
    public List<ToppingOffset> toppingOffsets = new List<ToppingOffset>();

    private void OnEnable()
    {
        Bingsu.onAddBowl += ShowBowl;
        Bingsu.onAddShavedMilk += ShowShavedMilk;
        Bingsu.onAddDrizzle += ShowDrizzle;
        Bingsu.onAddLogo += ShowLogo;
        Bingsu.onAddBaseTopping += ShowBaseTopping;
        Bingsu.onAddTopping += ShowTopping;
        Bingsu.onClearBingsu += ClearBingsu;
    }

    private void OnDisable()
    {
        Bingsu.onAddBowl -= ShowBowl;
        Bingsu.onAddShavedMilk -= ShowShavedMilk;
        Bingsu.onAddDrizzle -= ShowDrizzle;
        Bingsu.onAddLogo -= ShowLogo;
        Bingsu.onAddBaseTopping -= ShowBaseTopping;
        Bingsu.onAddTopping -= ShowTopping;
        Bingsu.onClearBingsu -= ClearBingsu;
    }

    private void Start()
    {
        bingsuParent = transform.parent.GetComponent<Bingsu>();
        if (bingsuParent == null)
        {
            Debug.LogError("No Bingsu component found on parent!");
        }

        // populate default topping offsets (only add if list is empty)
        if (toppingOffsets == null)
            toppingOffsets = new List<ToppingOffset>();
        if (toppingOffsets.Count == 0)
        {
            toppingOffsets.Add(new ToppingOffset { toppingName = "tiramisu", localPosition = new Vector3(-0.1f, 5.57f, 0.0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "bungeoppang", localPosition = new Vector3(-0.39f, 7.56f, 0.0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "chocolateBar", localPosition = new Vector3(2.67f, 4.74f, 0.0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "cheeseCake", localPosition = new Vector3(-0.02f, 5.02f, 0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "matcha", localPosition = new Vector3(-0.07f, 5.58f, 0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "vanilla", localPosition = new Vector3(-0.07f, 5.58f, 0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "chocolate", localPosition = new Vector3(-0.07f, 5.58f, 0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "ube", localPosition = new Vector3(-0.07f, 5.58f, 0f) });
            toppingOffsets.Add(new ToppingOffset { toppingName = "mango", localPosition = new Vector3(-0.07f, 5.58f, 0f) });
        }

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

        // Ensure any serialized/default sprites on prefab are cleared so newly-instantiated
        // bingsu prefabs don't appear pre-built. This handles the case where Bingsu
        // fired onClearBingsu() earlier (before these renderers existed).
        ClearBingsu();
    }

    // --- Event callbacks ---
    private void ShowBowl()
    {
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, "emptyBowl");
        baseRenderer.enabled = true;
    }

    private void ShowShavedMilk()
    {
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, "fullBowl");
        baseRenderer.enabled = true;
    }

    private void ShowBaseTopping(string type)
    {
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, type);
        baseRenderer.enabled = true;
    }

    private void ShowDrizzle()
    {
        if (baseRenderer.sprite == null) return;

        string baseName = baseRenderer.sprite.name;
        string spriteName = "drizzle" + char.ToUpper(baseName[0]) + baseName.Substring(1);
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, spriteName);
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
        toppingRenderer.sprite = FindSpriteFlexible(toppingSprites, spriteName);

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
    
    private void ClearBingsu()
    {
        baseRenderer.enabled = false;
        baseRenderer.sprite = null;
        toppingRenderer.enabled = false;
        toppingRenderer.sprite = null;
        logoRenderer.enabled = false;
        logoRenderer.sprite = null;

    }

    // --- Utility ---
    // Flexible sprite lookup similar to CustomerDialogueBox: exact, starts-with, contains, normalized
    private Sprite FindSpriteFlexible(Sprite[] sprites, string expectedName)
    {
        if (sprites == null || expectedName == null) return null;

        foreach (var s in sprites)
        {
            if (s != null && s.name == expectedName) return s;
        }

        string expLower = expectedName.ToLowerInvariant();
        foreach (var s in sprites)
        {
            if (s == null) continue;
            string name = s.name.ToLowerInvariant();
            if (name.StartsWith(expLower)) return s;
        }

        foreach (var s in sprites)
        {
            if (s == null) continue;
            string name = s.name.ToLowerInvariant();
            if (name.Contains(expLower)) return s;
        }

        string alt = expLower.Replace("_", "").Replace("-", "");
        foreach (var s in sprites)
        {
            if (s == null) continue;
            string name = s.name.ToLowerInvariant().Replace("_", "").Replace("-", "");
            if (name.Contains(alt)) return s;
        }

        if (debugMode)
        {
            Debug.LogWarning($"BingsuRenderer: sprite '{expectedName}' not found among {sprites.Length} sprites: {string.Join(",", System.Array.ConvertAll(sprites, x => x==null?"<null>":x.name))}");
        }
        return null;
    }
}
