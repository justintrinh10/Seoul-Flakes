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
        // Try to find the Bingsu component on this GameObject or any parent.
        bingsuParent = GetComponentInParent<Bingsu>();
        if (bingsuParent == null)
        {
            Debug.LogError("No Bingsu component found on this GameObject or any parent!");
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

        // Create or reuse child renderers
        // Prefer existing child GameObjects (e.g. when using a prefab with placeholder sprites)
        Transform child;

        child = transform.Find("BaseRenderer");
        if (child != null)
        {
            baseRenderer = child.GetComponent<SpriteRenderer>();
            if (baseRenderer == null) baseRenderer = child.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject baseObj = new GameObject("BaseRenderer");
            baseObj.transform.SetParent(transform);
            baseObj.transform.localPosition = Vector3.zero;
            baseRenderer = baseObj.AddComponent<SpriteRenderer>();
        }
        baseRenderer.sortingLayerName = "Bingsu";
        baseRenderer.sortingOrder = 3;
        baseRenderer.enabled = false;

        child = transform.Find("ToppingRenderer");
        if (child != null)
        {
            toppingRenderer = child.GetComponent<SpriteRenderer>();
            if (toppingRenderer == null) toppingRenderer = child.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject topObj = new GameObject("ToppingRenderer");
            topObj.transform.SetParent(transform);
            topObj.transform.localPosition = Vector3.zero;
            toppingRenderer = topObj.AddComponent<SpriteRenderer>();
        }
        toppingRenderer.sortingLayerName = "Bingsu";
        toppingRenderer.sortingOrder = 4;
        toppingRenderer.enabled = false;

        child = transform.Find("LogoRenderer");
        if (child != null)
        {
            logoRenderer = child.GetComponent<SpriteRenderer>();
            if (logoRenderer == null) logoRenderer = child.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject logoObj = new GameObject("LogoRenderer");
            logoObj.transform.SetParent(transform);
            logoObj.transform.localPosition = Vector3.zero;
            logoRenderer = logoObj.AddComponent<SpriteRenderer>();
        }
        logoRenderer.sortingLayerName = "Bingsu";
        logoRenderer.sortingOrder = 5;
        logoRenderer.enabled = false;

        // Ensure any serialized/default sprites on prefab are cleared so newly-instantiated
        // bingsu prefabs don't appear pre-built. This handles the case where Bingsu
        // fired onClearBingsu() earlier (before these renderers existed).
        ClearBingsu();
    }

    private void Awake()
    {
        // As early as possible, clear any serialized sprites on known child renderers so
        // instantiated prefabs don't show placeholder art before Start runs.
        Transform child;

        child = transform.Find("BaseRenderer");
        if (child != null)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = null;
                sr.enabled = false;
            }
        }

        child = transform.Find("ToppingRenderer");
        if (child != null)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = null;
                sr.enabled = false;
            }
        }

        child = transform.Find("LogoRenderer");
        if (child != null)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = null;
                sr.enabled = false;
            }
        }

        // Ensure runtime SpriteRenderer references exist so event callbacks (which may fire
        // before Start) don't encounter null refs. We prefer existing child objects but
        // create fallback renderers if they're missing.
        Transform existing;

        existing = transform.Find("BaseRenderer");
        if (existing != null)
        {
            baseRenderer = existing.GetComponent<SpriteRenderer>();
            if (baseRenderer == null) baseRenderer = existing.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject go = new GameObject("BaseRenderer");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            baseRenderer = go.AddComponent<SpriteRenderer>();
        }
        baseRenderer.sortingLayerName = "Bingsu";
        baseRenderer.sortingOrder = 3;
        baseRenderer.enabled = false;

        existing = transform.Find("ToppingRenderer");
        if (existing != null)
        {
            toppingRenderer = existing.GetComponent<SpriteRenderer>();
            if (toppingRenderer == null) toppingRenderer = existing.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject go = new GameObject("ToppingRenderer");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            toppingRenderer = go.AddComponent<SpriteRenderer>();
        }
        toppingRenderer.sortingLayerName = "Bingsu";
        toppingRenderer.sortingOrder = 4;
        toppingRenderer.enabled = false;

        existing = transform.Find("LogoRenderer");
        if (existing != null)
        {
            logoRenderer = existing.GetComponent<SpriteRenderer>();
            if (logoRenderer == null) logoRenderer = existing.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject go = new GameObject("LogoRenderer");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            logoRenderer = go.AddComponent<SpriteRenderer>();
        }
        logoRenderer.sortingLayerName = "Bingsu";
        logoRenderer.sortingOrder = 5;
        logoRenderer.enabled = false;
    }

    // --- Event callbacks ---
    private void ShowBowl()
    {
        if (baseRenderer == null)
        {
            if (debugMode) Debug.LogWarning("BingsuRenderer.ShowBowl called but baseRenderer is null.");
            return;
        }
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, "emptyBowl");
        baseRenderer.enabled = true;
    }

    private void ShowShavedMilk()
    {
        if (baseRenderer == null)
        {
            if (debugMode) Debug.LogWarning("BingsuRenderer.ShowShavedMilk called but baseRenderer is null.");
            return;
        }
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, "fullBowl");
        baseRenderer.enabled = true;
    }

    private void ShowBaseTopping(string type)
    {
        if (baseRenderer == null)
        {
            if (debugMode) Debug.LogWarning("BingsuRenderer.ShowBaseTopping called but baseRenderer is null.");
            return;
        }
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, type);
        baseRenderer.enabled = true;
    }

    private void ShowDrizzle()
    {
        if (baseRenderer == null)
        {
            if (debugMode) Debug.LogWarning("BingsuRenderer.ShowDrizzle called but baseRenderer is null.");
            return;
        }
        if (baseRenderer.sprite == null) return;

        string baseName = baseRenderer.sprite.name;
        string spriteName = "drizzle" + char.ToUpper(baseName[0]) + baseName.Substring(1);
        baseRenderer.sprite = FindSpriteFlexible(baseSprites, spriteName);
        baseRenderer.enabled = true;
    }

    private void ShowLogo()
    {
        if (logoRenderer == null)
        {
            if (debugMode) Debug.LogWarning("BingsuRenderer.ShowLogo called but logoRenderer is null.");
            return;
        }
        logoRenderer.sprite = logoSprite;
        logoRenderer.transform.localPosition = new Vector3(0, 0.51f, 0);
        logoRenderer.enabled = true;
    }

    private void ShowTopping(string type)
    {
        string spriteName = "topping" + char.ToUpper(type[0]) + type.Substring(1);
        if (toppingRenderer == null)
        {
            if (debugMode) Debug.LogWarning("BingsuRenderer.ShowTopping called but toppingRenderer is null.");
            return;
        }
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
        if (baseRenderer != null)
        {
            baseRenderer.enabled = false;
            baseRenderer.sprite = null;
        }
        if (toppingRenderer != null)
        {
            toppingRenderer.enabled = false;
            toppingRenderer.sprite = null;
        }
        if (logoRenderer != null)
        {
            logoRenderer.enabled = false;
            logoRenderer.sprite = null;
        }

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
