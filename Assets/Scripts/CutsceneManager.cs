using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [TextArea(3,10)]
    public string[] cutsceneLines;
    public float typingSpeed = 0.03f;

    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click or tap
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
            SceneManager.LoadScene("MainMenu"); // change to your next scene name
        }
    }
}
