using UnityEngine;

public class ShavedIceMachine : MonoBehaviour
{
    [Header("Bowl Sprites")]
    public Sprite emptyBowl;
    public Sprite bowlQuarter;
    public Sprite bowlHalf;
    public Sprite bowlThreeQuarter;
    public Sprite bowlFull;

    [Header("Settings")]
    public float sensitivity = 0.005f;   // how fast swipes add energy
    public float decayRate = 0.6f;       // how fast swipe energy fades
    public float fillPerEnergy = 0.15f;  // fill speed

    private SpriteRenderer bowlRenderer;

    private float fillAmount = 0f;       // 0 → 1
    private float swipeEnergy = 0f;      // increases with swiping

    void Awake()
    {
        bowlRenderer = transform.Find("BowlSprite").GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void Update()
    {
        HandleSwipeInput();
        ApplyFillingLogic();
    }

    void HandleSwipeInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Moved)
        {
            float dy = Mathf.Abs(t.deltaPosition.y);

            swipeEnergy += dy * sensitivity;
            swipeEnergy = Mathf.Clamp(swipeEnergy, 0f, 1f);
        }

        swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
    }

    void ApplyFillingLogic()
    {
        if (swipeEnergy > 0.1f && fillAmount < 1f)
        {
            fillAmount += swipeEnergy * fillPerEnergy * Time.deltaTime;
            fillAmount = Mathf.Clamp01(fillAmount);
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        if (fillAmount <= 0f)
            bowlRenderer.sprite = emptyBowl;
        else if (fillAmount <= 0.25f)
            bowlRenderer.sprite = bowlQuarter;
        else if (fillAmount <= 0.5f)
            bowlRenderer.sprite = bowlHalf;
        else if (fillAmount <= 0.75f)
            bowlRenderer.sprite = bowlThreeQuarter;
        else
            bowlRenderer.sprite = bowlFull;
    }

    public void ResetBowl()
    {
        fillAmount = 0f;
        UpdateSprite();
    }
}