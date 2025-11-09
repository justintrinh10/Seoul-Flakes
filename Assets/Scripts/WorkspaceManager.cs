using UnityEngine;
using System;

public class WorkspaceManager : MonoBehaviour
{
    [Header("Workspaces")]
    public GameObject[] workspaces;      // Assign Left, Center, Right workspace objects (with SpriteRenderer)

    [Header("Workspace Canvases")]
    public Canvas[] workspaceCanvases;   // Assign LeftCanvas, CenterCanvas, RightCanvas

    private int currentIndex = 1;        // Start at center workspace

    // Event fired when workspace changes. Parameter is the new currentIndex (0=Left,1=Center,2=Right)
    public event Action<int> onWorkspaceChanged;

    /// <summary>
    /// Returns the current workspace index: 0=Left, 1=Center, 2=Right (if assigned in that order).
    /// </summary>
    public int GetCurrentIndex() => currentIndex;

    void Start()
    {
        UpdateWorkspace();
    }

    public void MoveRight()
    {
        if (currentIndex < workspaces.Length - 1)
        {
            currentIndex++;
            UpdateWorkspace();
        }
    }

    public void MoveLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateWorkspace();
        }
    }

    private void UpdateWorkspace()
    {
        for (int i = 0; i < workspaces.Length; i++)
        {
            bool isActive = (i == currentIndex);

            // Enable the workspace SpriteRenderer
            if (workspaces[i] != null)
            {
                var sr = workspaces[i].GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.enabled = isActive;
            }

            // Enable the corresponding Canvas
            if (workspaceCanvases != null && workspaceCanvases.Length > i)
            {
                workspaceCanvases[i].gameObject.SetActive(isActive);
            }
        }

        // notify listeners
        onWorkspaceChanged?.Invoke(currentIndex);
    }
}
