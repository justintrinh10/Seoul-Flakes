using UnityEngine;

public class BingsuRenderer : MonoBehaviour
{
    private Bingsu bingsuParent;
    private SpriteRenderer bingsuSprite;

    [Header("Sprites for Parts")]
    public Spritep baseSprites;
    public Sprite logoSprite;
    public Sprite[] toppingSprites;   

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
        bingsuSprite = GetComponent<SpriteRenderer>();
        if (bingsuSprite == null)
        {
            bingsuSprite = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    // Event callbacks
    private void ShowBowl() => bingsuSprite.sprite = bowlSprite;

    private void ShowShavedMilk()
    {
        // You could layer multiple sprites, or just replace
        bingsuSprite.sprite = shavedMilkSprite;
    }

    private void ShowDrizzle() => bingsuSprite.sprite = drizzleSprite;

    private void ShowLogo() => bingsuSprite.sprite = logoSprite;

    private void ShowBaseTopping(string type)
    {
        // Find matching sprite by name
        foreach (var sprite in baseToppingSprites)
        {
            if (sprite.name == type)
            {
                bingsuSprite.sprite = sprite;
                break;
            }
        }
    }

    private void ShowTopping(string type)
    {
        foreach (var sprite in toppingSprites)
        {
            if (sprite.name == type)
            {
                bingsuSprite.sprite = sprite;
                break;
            }
        }
    }
}
