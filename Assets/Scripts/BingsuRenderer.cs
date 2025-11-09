using UnityEngine;

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
        baseRenderer = GetComponent<SpriteRenderer>();
        if (baseRenderer == null)
        {
            baseRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        toppingRenderer = GetComponent<SpriteRenderer>();
        if (toppingRenderer == null)
        {
            toppingRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        logoRenderer = GetComponent<SpriteRenderer>();
        if (logoRenderer == null)
        {
            logoRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        baseRenderer.sprite = null;
        toppingRenderer.sprite = null;
        logoRenderer.sprite = null;
    }

    // Event callbacks
    private void ShowBowl()
    {
        foreach (var sprite in baseSprites)
        {
            if (sprite.name == "emptyBowl")
            {
                baseRenderer.sprite = sprite;
                break;
            }
        }
    }

    private void ShowShavedMilk()
    {
        foreach (var sprite in baseSprites)
        {
            if (sprite.name == "fullBowl")
            {
                baseRenderer.sprite = sprite;
                break;
            }
        }
    }

    private void ShowBaseTopping(string type)
    {
        foreach (var sprite in baseSprites)
        {
            if (sprite.name == type)
            {
                baseRenderer.sprite = sprite;
                break;
            }
        }
    }

    private void ShowDrizzle(){
        string baseName = baseRenderer.sprite.name;
        baseName = char.ToUpper(baseName[0]) + baseName.Substring(1);
        string spriteName = "drizzle" + baseName;
        foreach (var sprite in baseSprites)
        {
            if (sprite.name == spriteName)
            {
                baseRenderer.sprite = sprite;
                break;
            }
        }
    }

    private void ShowLogo(){
        logoRenderer.sprite = logoSprite;
    }

    private void ShowTopping(string type)
    {
        string spriteName = "topping" + char.ToUpper(type[0]) + type.Substring(1);
        foreach (var sprite in toppingSprites)
        {
            if (sprite.name == spriteName)
            {
                toppingRenderer.sprite = sprite;
                break;
            }
        }
    }
}
