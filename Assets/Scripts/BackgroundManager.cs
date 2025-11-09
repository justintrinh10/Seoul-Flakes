using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Safe BackgroundManager that toggles UI Images (not entire GameObjects) for left/center/right backgrounds.
/// - Avoids SetActive on background GameObjects so button children don't disappear
/// - Auto-finds Image or GameObject by common names if inspector fields are empty (optional)
/// - Ensures Image.enabled and alpha are set so visuals show at Start
/// - Provides public SwitchToLeftScreen / SwitchToRightScreen for wiring to Buttons
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    [Header("Background Images (preferred)")]
    public Image leftImage;
    public Image centerImage;
    public Image rightImage;

    [Header("Optional: Background GameObjects (used to find Images automatically)")]
    public GameObject leftBackground;
    public GameObject centerBackground;
    public GameObject rightBackground;

    [Header("Navigation Buttons")]
    public Button leftArrowButton;
    public Button rightArrowButton;

    [Header("Behavior")]
    [Tooltip("If true, the script will try to auto-find children named Left/Center/Right and arrow buttons by common names.")]
    public bool autoFindByName = true;

    [Tooltip("Show debug logs from this component.")]
    public bool debugMode = false;

    private enum ScreenState { Left, Center, Right }
    private ScreenState currentScreen = ScreenState.Center;

    private readonly List<string> leftNames = new List<string>{"Left","left","LeftBackground","leftBackground"};
    private readonly List<string> centerNames = new List<string>{"Center","center","CenterBackground","centerBackground"};
    private readonly List<string> rightNames = new List<string>{"Right","right","RightBackground","rightBackground"};
    private readonly List<string> leftButtonNames = new List<string>{"LeftArrowButton","LeftArrow","leftArrowButton","leftArrow","LeftBtn"};
    private readonly List<string> rightButtonNames = new List<string>{"RightArrowButton","RightArrow","rightArrowButton","rightArrow","RightBtn"};

    void Awake()
    {
        // Auto-find Images from the explicit GameObject fields if Images weren't assigned
        if (leftImage == null && leftBackground != null) leftImage = leftBackground.GetComponent<Image>();
        if (centerImage == null && centerBackground != null) centerImage = centerBackground.GetComponent<Image>();
        if (rightImage == null && rightBackground != null) rightImage = rightBackground.GetComponent<Image>();

        // If still not found and autoFindByName is enabled, try common names in the scene
        if (autoFindByName)
        {
            TryAutoFindImages();
            TryAutoFindButtons();
        }

        // Ensure visuals are enabled and visible
        EnsureImageVisible(leftImage);
        EnsureImageVisible(centerImage);
        EnsureImageVisible(rightImage);

        // Wire buttons safely
        if (leftArrowButton != null) leftArrowButton.onClick.AddListener(SwitchToLeftScreen);
        if (rightArrowButton != null) rightArrowButton.onClick.AddListener(SwitchToRightScreen);
    }

    void Start()
    {
        // Default to center screen
        ShowCenterScreen();

        if (debugMode)
        {
            Debug.Log($"BackgroundManager Awake: leftImage={(leftImage!=null)}, centerImage={(centerImage!=null)}, rightImage={(rightImage!=null)}");
            Debug.Log($"BackgroundManager Awake: leftButton={(leftArrowButton!=null)}, rightButton={(rightArrowButton!=null)}");
        }
    }

    // Public switch methods (suitable for wiring in inspector or via button listeners)
    public void SwitchToLeftScreen()
    {
        if (currentScreen == ScreenState.Center)
            ShowLeftScreen();
        else if (currentScreen == ScreenState.Right)
            ShowCenterScreen();
    }

    public void SwitchToRightScreen()
    {
        if (currentScreen == ScreenState.Center)
            ShowRightScreen();
        else if (currentScreen == ScreenState.Left)
            ShowCenterScreen();
    }

    public void ShowLeftScreen()
    {
        ToggleImages(true, false, false);
        currentScreen = ScreenState.Left;
        if (debugMode) Debug.Log("Showing Left Screen");
    }

    public void ShowCenterScreen()
    {
        ToggleImages(false, true, false);
        currentScreen = ScreenState.Center;
        if (debugMode) Debug.Log("Showing Center Screen");
    }

    public void ShowRightScreen()
    {
        ToggleImages(false, false, true);
        currentScreen = ScreenState.Right;
        if (debugMode) Debug.Log("Showing Right Screen");
    }

    private void ToggleImages(bool leftOn, bool centerOn, bool rightOn)
    {
        SetImageState(leftImage, leftOn);
        SetImageState(centerImage, centerOn);
        SetImageState(rightImage, rightOn);
    }

    private void SetImageState(Image img, bool enable)
    {
        if (img == null) return;

        // Enable the Image component rather than the whole GameObject so children (buttons) stay active
        img.enabled = enable;

        // Ensure it's visible (alpha) when enabled
        if (enable)
        {
            Color c = img.color;
            if (c.a <= 0f) { c.a = 1f; img.color = c; }
            img.raycastTarget = false; // background shouldn't block clicks by default
        }
    }

    private void EnsureImageVisible(Image img)
    {
        if (img == null) return;
        // Ensure enabled
        img.enabled = true;

        // If no sprite is assigned, warn (nothing to show)
        if (img.sprite == null)
        {
            Debug.LogWarning($"BackgroundManager: Image '{img.gameObject.name}' has no Sprite assigned.");
        }

        // Ensure alpha isn't zero
        Color c = img.color;
        if (c.a <= 0f) { c.a = 1f; img.color = c; }

        // Background shouldn't capture raycasts so buttons can receive clicks
        img.raycastTarget = false;
    }

    private void TryAutoFindImages()
    {
        if (leftImage == null)
        {
            leftImage = FindImageByNameList(leftNames);
            if (leftImage == null && leftBackground != null) leftImage = leftBackground.GetComponentInChildren<Image>(true);
        }

        if (centerImage == null)
        {
            centerImage = FindImageByNameList(centerNames);
            if (centerImage == null && centerBackground != null) centerImage = centerBackground.GetComponentInChildren<Image>(true);
        }

        if (rightImage == null)
        {
            rightImage = FindImageByNameList(rightNames);
            if (rightImage == null && rightBackground != null) rightImage = rightBackground.GetComponentInChildren<Image>(true);
        }
    }

    private Image FindImageByNameList(List<string> names)
    {
        foreach (var n in names)
        {
            var go = GameObject.Find(n);
            if (go != null)
            {
                var img = go.GetComponent<Image>();
                if (img != null) return img;
            }
        }
        return null;
    }

    private void TryAutoFindButtons()
    {
        if (leftArrowButton == null)
        {
            leftArrowButton = FindButtonByNameList(leftButtonNames);
            if (leftArrowButton == null)
            {
                // fallback: find any Button named "LeftArrowButton" under this GameObject
                var b = transform.Find("LeftArrowButton");
                if (b != null) leftArrowButton = b.GetComponent<Button>();
            }
        }

        if (rightArrowButton == null)
        {
            rightArrowButton = FindButtonByNameList(rightButtonNames);
            if (rightArrowButton == null)
            {
                var b = transform.Find("RightArrowButton");
                if (b != null) rightArrowButton = b.GetComponent<Button>();
            }
        }
    }

    private Button FindButtonByNameList(List<string> names)
    {
        foreach (var n in names)
        {
            var go = GameObject.Find(n);
            if (go != null)
            {
                var btn = go.GetComponent<Button>();
                if (btn != null) return btn;
            }
        }
        return null;
    }
}
