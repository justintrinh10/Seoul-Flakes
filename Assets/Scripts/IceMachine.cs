using UnityEngine;
using UnityEngine.Events;

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

    [Header("References")]
    [Tooltip("Optional: assign the SpriteRenderer to use for the bowl. If left null the script will try to find a child named 'BowlSprite' or the first child SpriteRenderer.")]
    public SpriteRenderer bowlRenderer;

    [Header("Events")]
    public UnityEvent OnFillComplete;

    [Header("Debug")]
    public bool debugMode = false;

    private float fillAmount = 0f;       // 0 → 1
    private float swipeEnergy = 0f;      // increases with swiping
    private bool isComplete = false;
    private Vector2 lastMousePos;

    void Awake()
    {
        if (bowlRenderer == null)
        {
            var found = transform.Find("BowlSprite");
            if (found != null)
                bowlRenderer = found.GetComponent<SpriteRenderer>();
            else
                bowlRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (bowlRenderer == null)
            Debug.LogWarning("ShavedIceMachine: bowlRenderer not assigned and none found under children.");

        UpdateSprite();
    }

    void Update()
    {
        HandleInput();
        ApplyFillingLogic();
    }

    void HandleInput()
    {
        // Touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                float dy = Mathf.Abs(t.deltaPosition.y);
                swipeEnergy += dy * sensitivity;
                swipeEnergy = Mathf.Clamp(swipeEnergy, 0f, 1f);
            }

            swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
            return;
        }

        // Mouse input (editor / desktop fallback)
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 cur = Input.mousePosition;
            float dy = Mathf.Abs(cur.y - lastMousePos.y);
            lastMousePos = cur;
            swipeEnergy += dy * sensitivity;
            swipeEnergy = Mathf.Clamp(swipeEnergy, 0f, 1f);
            swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
        }
        else
        {
            // gradually decay when not interacting
            swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
        }
    }

    void ApplyFillingLogic()
    {
        if (isComplete) return;

        if (swipeEnergy > 0.01f && fillAmount < 1f)
        {
            fillAmount += swipeEnergy * fillPerEnergy * Time.deltaTime;
            fillAmount = Mathf.Clamp01(fillAmount);
            UpdateSprite();

            if (fillAmount >= 1f)
            {
                isComplete = true;
                if (debugMode) Debug.Log("ShavedIceMachine: Fill complete");
                OnFillComplete?.Invoke();
            }
        }
    }

    void UpdateSprite()
    {
        if (bowlRenderer == null) return;

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
        swipeEnergy = 0f;
        isComplete = false;
        UpdateSprite();
    }
}