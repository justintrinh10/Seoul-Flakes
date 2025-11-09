using UnityEngine;

public class BottomPanelSwitcher : MonoBehaviour
{
    public GameObject[] panels;
    private int currentIndex = 0;
    [Header("Options")]
    [Tooltip("If true and panels is empty, the script will auto-find direct child GameObjects as panels.")]
    public bool autoFindChildPanels = true;
    [SerializeField] private bool debugMode = false;

    void Start()
    {
        // Auto-find panels if none assigned
        if ((panels == null || panels.Length == 0) && autoFindChildPanels)
        {
            var children = new System.Collections.Generic.List<GameObject>();
            foreach (Transform t in transform)
            {
                if (t != null && t.gameObject != null)
                    children.Add(t.gameObject);
            }
            panels = children.ToArray();
            if (debugMode) Debug.Log($"BottomPanelSwitcher: auto-found {panels.Length} panels from children of {gameObject.name}");
        }

        if (panels == null || panels.Length == 0)
        {
            Debug.LogWarning($"BottomPanelSwitcher on '{gameObject.name}' has no panels assigned and auto-find is {(autoFindChildPanels?"enabled":"disabled")}. Nothing to show.");
            return;
        }
        ShowPanel(currentIndex);
    }

    public void NextPanel()
    {
        currentIndex = (currentIndex + 1) % panels.Length;
        ShowPanel(currentIndex);
    }

    public void PreviousPanel()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = panels.Length - 1;
        ShowPanel(currentIndex);
    }

    private void ShowPanel(int index)
    {
        if (panels == null || panels.Length == 0)
        {
            if (debugMode) Debug.Log("BottomPanelSwitcher: ShowPanel called but no panels are configured.");
            return;
        }
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] == null)
            {
                if (debugMode) Debug.LogWarning($"BottomPanelSwitcher: panels[{i}] is null");
                continue;
            }
            panels[i].SetActive(i == index);
            if (debugMode && i == index) Debug.Log($"BottomPanelSwitcher: showing panel '{panels[i].name}' (index {i})");
        }
    }
}
