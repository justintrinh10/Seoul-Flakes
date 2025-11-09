#if UNITY_EDITOR
using UnityEngine;
#else
using UnityEngine;
#endif
using UnityEngine.Events;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
#endif

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
    private bool active = false; // minigame active flag
    private WorkspaceManager workspaceManager;

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

        // auto-find workspace manager and set visibility according to current workspace
        workspaceManager = FindObjectOfType<WorkspaceManager>();
        if (workspaceManager != null)
        {
            // set initial visibility
            gameObject.SetActive(workspaceManager.GetCurrentIndex() == 0);
            // subscribe to workspace changes
            workspaceManager.onWorkspaceChanged += OnWorkspaceChanged;
        }

        // Try to auto-wire a UI Button (left-workspace canvas) named like 'Ice' or 'Shaved'
        // so designers don't have to hookup the OnClick manually. This is a convenience
        // fallback for dev/testing only.
        var allButtons = FindObjectsOfType<Button>();
        foreach (var btn in allButtons)
        {
            var n = btn.gameObject.name.ToLowerInvariant();
            if (n.Contains("ice") || n.Contains("shaved") || n.Contains("icemachine") || n.Contains("ice-machine"))
            {
                btn.onClick.AddListener(OpenMinigame);
                if (debugMode) Debug.Log($"ShavedIceMachine: auto-wired Button '{btn.gameObject.name}' to OpenMinigame()");
            }
        }
    }

    private void OnDestroy()
    {
        if (workspaceManager != null)
            workspaceManager.onWorkspaceChanged -= OnWorkspaceChanged;
    }

    private void OnWorkspaceChanged(int newIndex)
    {
        // only show the ice machine when the left workspace (index 0) is active
        gameObject.SetActive(newIndex == 0);
    }

    // Called when the player clicks/taps the machine in the world (requires a Collider on the GameObject)
    private void OnMouseDown()
    {
        if (debugMode) Debug.Log("ShavedIceMachine: OnMouseDown");
        OpenMinigame();
    }

    // Public method to open/start the minigame (can be wired to UI Buttons)
    public void OpenMinigame()
    {
        // If workspaceManager exists, ensure we're on the left workspace
        if (workspaceManager != null && workspaceManager.GetCurrentIndex() != 0)
        {
            if (debugMode) Debug.Log("ShavedIceMachine: OpenMinigame blocked - not in left workspace");
            return;
        }

        if (debugMode) Debug.Log("ShavedIceMachine: OpenMinigame starting minigame");
        StartMinigame();
    }

    void Update()
    {
        if (!active) return;
        HandleInput();
        ApplyFillingLogic();
    }

    void HandleInput()
    {
        // Input handling: prefer legacy Input if available, otherwise use the new Input System.
#if ENABLE_LEGACY_INPUT_MANAGER
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
#elif ENABLE_INPUT_SYSTEM
        // New Input System
        // Ensure EnhancedTouch support is enabled
        EnhancedTouchSupport.Enable();

        var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        if (touches.Count > 0)
        {
            var t = touches[0];
            if (t.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                float dy = Mathf.Abs(t.delta.y);
                swipeEnergy += dy * sensitivity;
                swipeEnergy = Mathf.Clamp(swipeEnergy, 0f, 1f);
            }

            swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
            return;
        }

        // Mouse fallback using new Input System
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                lastMousePos = Mouse.current.position.ReadValue();
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                Vector2 cur = Mouse.current.position.ReadValue();
                float dy = Mathf.Abs(cur.y - lastMousePos.y);
                lastMousePos = cur;
                swipeEnergy += dy * sensitivity;
                swipeEnergy = Mathf.Clamp(swipeEnergy, 0f, 1f);
                swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
            }
            else
            {
                swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
            }
        }
#else
        // Unknown input configuration: attempt legacy Input in try/catch to avoid exceptions
        try
        {
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
                swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
            }
        }
        catch (System.InvalidOperationException)
        {
            // Input class not available under current Input System settings; just decay energy
            swipeEnergy = Mathf.MoveTowards(swipeEnergy, 0f, decayRate * Time.deltaTime);
        }
#endif
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
                // stop accepting input after completion
                StopMinigame();
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

    /// <summary>
    /// Start the minigame: enable input and reset the bowl.
    /// </summary>
    public void StartMinigame()
    {
        ResetBowl();
        active = true;
        if (debugMode) Debug.Log("ShavedIceMachine: minigame started");
    }

    /// <summary>
    /// Stop the minigame: disable input.
    /// </summary>
    public void StopMinigame()
    {
        active = false;
        if (debugMode) Debug.Log("ShavedIceMachine: minigame stopped");
    }
}