using System.Collections;
using UnityEngine;

public class CustomerDialogueBox : MonoBehaviour
{
    private BingsuData bingsuData;

    private SpriteRenderer textBoxRenderer;
    private SpriteRenderer baseIconRenderer;
    private SpriteRenderer toppingIconRenderer;
    private SpriteRenderer drizzleIconRenderer;

    private Transform customerTransform;

    [Header("Offsets")]
    [SerializeField] private Vector3 textBoxOffset = new Vector3(0f, 2.2f, 0f);
    [SerializeField] private Vector3 baseIconOffset = new Vector3(-0.7f, 1f, 0f);
    [SerializeField] private Vector3 toppingIconOffset = new Vector3(0.7f, 1f, 0f);
    [SerializeField] private Vector3 drizzleIconOffset = new Vector3(0f, -0.2f, 0f);

    [Header("Sprites for Parts")]
    public Sprite textBox;
    public Sprite[] baseSprites;
    public Sprite[] toppingSprites;
    public Sprite drizzleSprite;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float popScaleMultiplier = 1.15f;
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private bool isFadingOut = false;
    private float dialogueScale = 1f;
    private bool dialogueInitialized = false;

    void Start()
    {
        EnsureDialogueInitialized();
        // Start invisible + tiny
        SetAlpha(0f);
        transform.localScale = Vector3.zero;
        StartCoroutine(FadeInAndPop());
    }

    private SpriteRenderer FindOrCreateRenderer(string name, Vector3 offset, int order)
    {
        Transform child = transform.Find(name);
        SpriteRenderer sr;

        if (child != null)
        {
            sr = child.GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = child.gameObject.AddComponent<SpriteRenderer>();
        }
        else
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = offset;
            sr = obj.AddComponent<SpriteRenderer>();
        }

        sr.sortingLayerName = "Customer";
        sr.sortingOrder = order;
        sr.transform.localScale = Vector3.one; // ensure full size
        return sr;
    }

    private void EnsureDialogueInitialized()
    {
        if (dialogueInitialized) return;

        // Create or reuse renderers
        textBoxRenderer = FindOrCreateRenderer("TextBoxRenderer", Vector3.zero, 4);
        baseIconRenderer = FindOrCreateRenderer("BaseIconRenderer", baseIconOffset, 5);
        toppingIconRenderer = FindOrCreateRenderer("ToppingIconRenderer", toppingIconOffset, 5);
        drizzleIconRenderer = FindOrCreateRenderer("DrizzleIconRenderer", drizzleIconOffset, 5);

        // Always set text box sprite if available
        if (textBoxRenderer != null)
        {
            textBoxRenderer.sprite = textBox;
            textBoxRenderer.enabled = textBox != null;
        }

        dialogueInitialized = true;
    }

    public void CreateDialogueBox(BingsuData bData, Transform customer, Vector3? offset = null, float scale = 1f)
    {
        EnsureDialogueInitialized();
        bingsuData = bData;
        customerTransform = customer;
        dialogueScale = scale;

        Vector3 posOffset = offset ?? textBoxOffset;

        // Position text box relative to parent
        transform.localPosition = posOffset;

        // Clear previous sprites
        baseIconRenderer.sprite = null;
        toppingIconRenderer.sprite = null;
        drizzleIconRenderer.sprite = null;

        // Assign bingsu icons if complete
        if (bingsuData != null && bingsuData.bingsuComplete())
        {
            // Base icon
            string baseIconName = "base" + char.ToUpper(bingsuData.getBaseToppingType()[0]) + bingsuData.getBaseToppingType().Substring(1);
            var baseSpriteFound = FindSpriteFlexible(baseSprites, baseIconName);
            if (baseSpriteFound != null)
            {
                baseIconRenderer.sprite = baseSpriteFound;
                baseIconRenderer.enabled = true;
                baseIconRenderer.transform.localScale = Vector3.one;
                baseIconRenderer.transform.localPosition = baseIconOffset;
            }
            else if (debugMode)
            {
                Debug.Log($"CustomerDialogueBox: looking for base '{baseIconName}' found=False (baseSprites count={(baseSprites==null?0:baseSprites.Length)})");
                if (baseSprites != null)
                {
                    string names = string.Join(",", System.Array.ConvertAll(baseSprites, s => s==null?"<null>":s.name));
                    Debug.Log("CustomerDialogueBox: baseSprites names = " + names);
                }
            }

            // Topping icon
            string toppingIconName = "topping" + char.ToUpper(bingsuData.getToppingType()[0]) + bingsuData.getToppingType().Substring(1);
            var toppingSpriteFound = FindSpriteFlexible(toppingSprites, toppingIconName);
            if (toppingSpriteFound != null)
            {
                toppingIconRenderer.sprite = toppingSpriteFound;
                toppingIconRenderer.enabled = true;
                toppingIconRenderer.transform.localScale = Vector3.one;
                toppingIconRenderer.transform.localPosition = toppingIconOffset;
            }
            else if (debugMode)
            {
                Debug.Log($"CustomerDialogueBox: looking for topping '{toppingIconName}' found=False (toppingSprites count={(toppingSprites==null?0:toppingSprites.Length)})");
                if (toppingSprites != null)
                {
                    string names = string.Join(",", System.Array.ConvertAll(toppingSprites, s => s==null?"<null>":s.name));
                    Debug.Log("CustomerDialogueBox: toppingSprites names = " + names);
                }
            }

            // Drizzle icon
            if (bingsuData.hasDrizzle() && drizzleSprite != null)
            {
                drizzleIconRenderer.sprite = drizzleSprite;
                drizzleIconRenderer.enabled = true;
                drizzleIconRenderer.transform.localScale = Vector3.one;
                drizzleIconRenderer.transform.localPosition = drizzleIconOffset;
            }
            drizzleIconRenderer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            toppingIconRenderer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            baseIconRenderer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            textBoxRenderer.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            drizzleIconRenderer.transform.position = drizzleIconOffset + transform.position;
            toppingIconRenderer.transform.position = toppingIconOffset + transform.position;
            toppingIconRenderer.transform.position -= new Vector3(0.6f, 0f, 0f);
            baseIconRenderer.transform.position = baseIconOffset + transform.position;
            baseIconRenderer.transform.position += new Vector3(0.6f, 0f, 0f);
        }
    }

void Update()
{
    if (customerTransform != null && !isFadingOut)
    {
        // Follow customer in world space
        transform.position = customerTransform.position + textBoxOffset;
    }
}


    private IEnumerator FadeInAndPop()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            SetAlpha(t);
            float scale = Mathf.Lerp(0f, popScaleMultiplier, t) * dialogueScale;
            transform.localScale = Vector3.one * scale;

            yield return null;
        }

        float bounceTime = 0.15f;
        elapsed = 0f;
        while (elapsed < bounceTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bounceTime;
            float scale = Mathf.Lerp(popScaleMultiplier, 1f, t) * dialogueScale;
            transform.localScale = Vector3.one * scale;
            yield return null;
        }

        SetAlpha(1f);
        transform.localScale = Vector3.one * dialogueScale;
    }

    public void FadeOutAndDestroy()
    {
        if (!isFadingOut)
            StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFadingOut = true;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            SetAlpha(1f - t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        SetAlphaForRenderer(textBoxRenderer, alpha);
        SetAlphaForRenderer(baseIconRenderer, alpha);
        SetAlphaForRenderer(toppingIconRenderer, alpha);
        SetAlphaForRenderer(drizzleIconRenderer, alpha);
    }

    private void SetAlphaForRenderer(SpriteRenderer sr, float alpha)
    {
        if (sr == null) return;
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }

    // Flexible sprite lookup: exact match, then case-insensitive starts-with, then contains, allowing suffixes like _0
    private Sprite FindSpriteFlexible(Sprite[] sprites, string expectedName)
    {
        if (sprites == null || expectedName == null) return null;

        // 1) exact match
        foreach (var s in sprites)
        {
            if (s != null && s.name == expectedName) return s;
        }

        string expLower = expectedName.ToLowerInvariant();

        // 2) starts with (case-insensitive)
        foreach (var s in sprites)
        {
            if (s == null) continue;
            string name = s.name.ToLowerInvariant();
            if (name.StartsWith(expLower)) return s;
        }

        // 3) contains (case-insensitive)
        foreach (var s in sprites)
        {
            if (s == null) continue;
            string name = s.name.ToLowerInvariant();
            if (name.Contains(expLower)) return s;
        }

        // 4) try replacing underscores/hyphens in expected name
        string alt = expLower.Replace("_", "").Replace("-", "");
        foreach (var s in sprites)
        {
            if (s == null) continue;
            string name = s.name.ToLowerInvariant().Replace("_", "").Replace("-", "");
            if (name.Contains(alt)) return s;
        }

        return null;
    }
}
