using UnityEngine;

/// <summary>
/// Minimal integration: when this GameObject is clicked/tapped, try to start the Bungeoppang minigame
/// If no `BungeoppangMinigame` is present in the scene, logs a warning.
/// </summary>
public class BungeoppangMachine : MonoBehaviour
{
    [Header("Integration")]
    [Tooltip("If true, open minigame on mouse/tap. Requires this GameObject to have a Collider for non-UI clicks.")]
    public bool openOnTap = true;

    [Tooltip("Optional: a reference to a BungeoppangMinigame in the scene. If null, the script will FindObjectOfType at runtime.")]
    public BungeoppangMinigame minigame;

    [Header("Debug")]
    public bool debugMode = false;

    void Start()
    {
        if (minigame == null)
        {
            minigame = FindObjectOfType<BungeoppangMinigame>();
            if (debugMode && minigame != null) Debug.Log("BungeoppangMachine: found minigame at Start.");
        }
    }

    void OnMouseDown()
    {
        if (!openOnTap) return;

        if (minigame == null)
        {
            minigame = FindObjectOfType<BungeoppangMinigame>();
            if (minigame == null)
            {
                Debug.LogWarning("BungeoppangMachine: No BungeoppangMinigame found in scene to open.");
                return;
            }
        }

        if (debugMode) Debug.Log("BungeoppangMachine: opening minigame");
        minigame.StartMinigame();
    }

    // For UI-based clicks, you can call this from a Button or IPointerClickHandler.
    public void OpenMinigame()
    {
        if (minigame == null) minigame = FindObjectOfType<BungeoppangMinigame>();
        if (minigame == null)
        {
            Debug.LogWarning("BungeoppangMachine: No BungeoppangMinigame found in scene to open.");
            return;
        }
        minigame.StartMinigame();
    }
}
