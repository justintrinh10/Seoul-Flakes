using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Bungeoppang minigame: shows a fish at random positions in a UI area for a short time.
/// Player must tap the fish before it disappears. Repeat until 3 consecutive hits, then complete.
///
/// Usage:
/// - Place this script on a GameObject in the scene (eg. a MinigameManager object).
/// - Provide a RectTransform `playArea` (usually a Canvas/Panel). If null, will use the full Screen space.
/// - Assign a `fishPrefab` (recommended: a UI Button with Image) or leave empty to let the script create a simple button.
/// - Wire `OnMinigameComplete` to resume main-game logic.
/// </summary>
public class BungeoppangMinigame : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("If set, fish will be spawned inside this RectTransform. If null, uses full screen canvas area.")]
    public RectTransform playArea;

    [Tooltip("Prefab to use for the fish. Should be a UI Button or GameObject with a Button component.")]
    public GameObject fishPrefab;

    [Header("Timing")]
    [Tooltip("How long (seconds) each fish stays visible before disappearing")]
    public float fishVisibleSeconds = 1.2f;

    [Tooltip("Delay between fish appearances")]
    public float betweenSpawnDelay = 0.25f;

    [Header("Game")]
    [Tooltip("How many consecutive hits required to win")]
    public int requiredConsecutiveHits = 3;

    [Header("Events")]
    public UnityEvent OnMinigameComplete;

    [Header("Debug")]
    public bool debugMode = false;

    // runtime
    private bool running = false;
    private int consecutiveHits = 0;
    private GameObject currentFish;
    private Canvas parentCanvas;
    private Coroutine runRoutine;

    void Awake()
    {
        if (playArea == null)
        {
            // try to find a Canvas and use its rect
            var canvas = FindObjectOfType<Canvas>();
            if (canvas != null) playArea = canvas.GetComponent<RectTransform>();
        }
        if (playArea != null)
            parentCanvas = playArea.GetComponentInParent<Canvas>();
        else
            parentCanvas = FindObjectOfType<Canvas>();
    }

    /// <summary>
    /// Start the minigame. If already running, this is a no-op.
    /// </summary>
    public void StartMinigame()
    {
        if (running) return;
        running = true;
        consecutiveHits = 0;
        if (debugMode) Debug.Log("BungeoppangMinigame: Starting");
        runRoutine = StartCoroutine(RunMinigameLoop());
    }

    /// <summary>
    /// Stop the minigame early and cleanup.
    /// </summary>
    public void EndMinigame()
    {
        if (!running) return;
        running = false;
        if (runRoutine != null) StopCoroutine(runRoutine);
        DestroyCurrentFish();
        if (debugMode) Debug.Log("BungeoppangMinigame: Ended");
    }

    private IEnumerator RunMinigameLoop()
    {
        while (running)
        {
            // spawn fish
            SpawnFishAtRandomPosition();

            float timer = 0f;
            bool hit = false;
            while (timer < fishVisibleSeconds)
            {
                if (!running) yield break;
                if (currentFish == null) { hit = true; break; } // clicked -> fish destroyed by handler
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            if (currentFish != null)
            {
                // timed out
                if (debugMode) Debug.Log("BungeoppangMinigame: fish timed out");
                DestroyCurrentFish();
                consecutiveHits = 0; // reset streak
            }
            else if (hit)
            {
                consecutiveHits++;
                if (debugMode) Debug.Log($"BungeoppangMinigame: hit! streak={consecutiveHits}");
            }

            if (consecutiveHits >= requiredConsecutiveHits)
            {
                // success
                running = false;
                if (debugMode) Debug.Log("BungeoppangMinigame: Completed");
                OnMinigameComplete?.Invoke();
                yield break;
            }

            // small delay before next fish
            float d = 0f;
            while (d < betweenSpawnDelay)
            {
                d += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    private void SpawnFishAtRandomPosition()
    {
        DestroyCurrentFish();

        if (fishPrefab != null)
        {
            currentFish = Instantiate(fishPrefab, playArea ?? (RectTransform)parentCanvas.transform);
        }
        else
        {
            // create a simple button if no prefab provided
            currentFish = CreateDefaultFishButton();
            currentFish.transform.SetParent(playArea ?? (RectTransform)parentCanvas.transform, false);
        }

        // position randomly inside playArea
        RectTransform rt = currentFish.GetComponent<RectTransform>();
        Rect area = (playArea != null) ? playArea.rect : new Rect(0, 0, Screen.width, Screen.height);

        float x = Random.Range(area.xMin + 30f, area.xMax - 30f);
        float y = Random.Range(area.yMin + 30f, area.yMax - 30f);

        // convert local pos to anchored position
        rt.anchoredPosition = new Vector2(x, y);

        // ensure button click is hooked
        var btn = currentFish.GetComponent<UnityEngine.UI.Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnFishClicked(currentFish));
        }
    }

    private GameObject CreateDefaultFishButton()
    {
        GameObject go = new GameObject("FishButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        var img = go.GetComponent<Image>();
        img.color = Color.cyan; // placeholder color
        var rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(64, 64);
        return go;
    }

    private void OnFishClicked(GameObject fish)
    {
        if (!running) return;
        if (fish != currentFish) return;

        if (debugMode) Debug.Log("BungeoppangMinigame: fish clicked");
        // Destroy fish to signal hit in coroutine
        DestroyCurrentFish();
    }

    private void DestroyCurrentFish()
    {
        if (currentFish != null)
        {
            Destroy(currentFish);
            currentFish = null;
        }
    }
}
