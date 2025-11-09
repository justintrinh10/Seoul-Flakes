using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;  // Import the Input System package

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    
    [SerializeField, TextArea(3, 10)]
    public string[] cutsceneLines;

    public float typingSpeed = 0.03f;

    private int index = 0;
    private bool isTyping = false;

    private InputAction clickAction;

    void OnEnable()
    {
        // Set up the input action for mouse click
        clickAction = new InputAction(binding: "<Mouse>/leftButton");
        clickAction.performed += _ => OnClick();
        clickAction.Enable();
    }

    void OnDisable()
    {
        clickAction.Disable();  // Clean up when the object is disabled
    }

    void OnClick()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textDisplay.text = cutsceneLines[index];
            isTyping = false;
        }
        else
        {
            NextLine();
        }
    }

    void Start()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textDisplay.text = "";
        foreach (char c in cutsceneLines[index].ToCharArray())
        {
            textDisplay.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (index < cutsceneLines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene("Main Screen"); // Change to your next scene name
        }
    }
}
