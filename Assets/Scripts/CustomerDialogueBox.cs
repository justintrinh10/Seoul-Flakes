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
    [SerializeField] private Vector3 baseIconOffset = new Vector3(-1f, 0.3f, 0f);
    [SerializeField] private Vector3 toppingIconOffset = new Vector3(1f, 0.3f, 0f);
    [SerializeField] private Vector3 drizzleIconOffset = new Vector3(0f, -0.2f, 0f);

    [Header("Sprites for Parts")]
    public Sprite textBox;
    public Sprite[] baseSprites;
    public Sprite[] toppingSprites;
    public Sprite drizzleSprite;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float popScaleMultiplier = 1.15f;

    private bool isFadingOut = false;
    private float dialogueScale = 1f;

    void Start()
    {
        // Create or reuse renderers
        textBoxRenderer = FindOrCreateRenderer("TextBoxRenderer", Vector3.zero, 4);
        baseIconRenderer = FindOrCreateRenderer("BaseIconRenderer", baseIconOffset, 5);
        toppingIconRenderer = FindOrCreateRenderer("ToppingIconRenderer", toppingIconOffset, 5);
        drizzleIconRenderer = FindOrCreateRenderer("DrizzleIconRenderer", drizzleIconOffset, 5);

        // Always show text box
        if (textBoxRenderer != null)
        {
            textBoxRenderer.sprite = textBox;
            textBoxRenderer.enabled = true;
        }

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

    public void CreateDialogueBox(BingsuData bData, Transform customer, Vector3? offset = null, float scale = 1f)
    {
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
            foreach (var s in baseSprites)
            {
                if (s.name == baseIconName)
                {
                    baseIconRenderer.sprite = s;
                    baseIconRenderer.enabled = true;
                    baseIconRenderer.transform.localScale = Vector3.one;
                    baseIconRenderer.transform.localPosition = baseIconOffset;
                    break;
                }
            }

            // Topping icon
            string toppingIconName = "topping" + char.ToUpper(bingsuData.getToppingType()[0]) + bingsuData.getToppingType().Substring(1);
            foreach (var s in toppingSprites)
            {
                if (s.name == toppingIconName)
                {
                    toppingIconRenderer.sprite = s;
                    toppingIconRenderer.enabled = true;
                    toppingIconRenderer.transform.localScale = Vector3.one;
                    toppingIconRenderer.transform.localPosition = toppingIconOffset;
                    break;
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
        }
    }

    void Update()
    {
        if (customerTransform != null && !isFadingOut)
        {
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
}
