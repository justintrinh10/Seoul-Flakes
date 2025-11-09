using System;
using UnityEngine;

public class CustomerRenderer : MonoBehaviour
{
    [Header("Parent Groups")]
    [SerializeField] private Transform neutralParent;
    [SerializeField] private Transform happyParent;
    [SerializeField] private Transform accessoryParent;
    [SerializeField] private Transform effectParent;

    private GameObject currentNeutral;
    private GameObject currentHappy;
    private GameObject currentAccessory;
    private GameObject sparkleIcon;
    private GameObject frustratedIcon;

    private void OnEnable()
    {
        var customer = GetComponent<Customer>();
        Type customerType = typeof(Customer);
        var stateChangeField = customerType.GetField("onStateChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var changeAppearanceField = customerType.GetField("changeAppearance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var timerEndField = customerType.GetField("onTimerEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        stateChangeField?.SetValue(null, (Action<string>)((Action<string>)stateChangeField.GetValue(null) + OnStateChange));
        changeAppearanceField?.SetValue(null, (Action<string, string>)((Action<string, string>)changeAppearanceField.GetValue(null) + OnChangeAppearance));
        timerEndField?.SetValue(null, (Action)((Action)timerEndField.GetValue(null) + OnTimerEnd));
    }

    private void OnDisable()
    {
        Type customerType = typeof(Customer);
        var stateChangeField = customerType.GetField("onStateChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var changeAppearanceField = customerType.GetField("changeAppearance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var timerEndField = customerType.GetField("onTimerEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        stateChangeField?.SetValue(null, (Action<string>)((Action<string>)stateChangeField.GetValue(null) - OnStateChange));
        changeAppearanceField?.SetValue(null, (Action<string, string>)((Action<string, string>)changeAppearanceField.GetValue(null) - OnChangeAppearance));
        timerEndField?.SetValue(null, (Action)((Action)timerEndField.GetValue(null) - OnTimerEnd));
    }

    private void Start()
    {
        sparkleIcon = effectParent.Find("sparkleIcon").gameObject;
        frustratedIcon = effectParent.Find("frustratedIcon").gameObject;

        HideAll();
    }

    private void OnChangeAppearance(string color, string accessory)
    {
        HideAll();

        Transform neutralSprite = neutralParent.Find("neutral" + Capitalize(color));
        if (neutralSprite != null)
        {
            currentNeutral = neutralSprite.gameObject;
            currentNeutral.SetActive(true);
        }

        Transform accSprite = accessoryParent.Find(accessory);
        if (accSprite != null)
        {
            currentAccessory = accSprite.gameObject;
            currentAccessory.SetActive(true);
        }
    }

    private void OnStateChange(string state)
    {
        switch (state.ToLower())
        {
            case "neutral":
                SetNeutral();
                break;
            case "happy":
                SetHappy();
                break;
            case "angry":
                SetAngry();
                break;
        }
    }

    private void OnTimerEnd()
    {
        HideAll();
    }

    private void SetNeutral()
    {
        HideMoodIcons();
        if (currentHappy != null) currentHappy.SetActive(false);
        if (currentNeutral != null) currentNeutral.SetActive(true);
    }

    private void SetHappy()
    {
        HideMoodIcons();

        if (currentNeutral != null) currentNeutral.SetActive(false);
        if (currentHappy != null) currentHappy.SetActive(false);

        string color = currentNeutral != null ? currentNeutral.name.Replace("neutral", "") : "Blue";
        Transform happySprite = happyParent.Find("happy" + color);
        if (happySprite != null)
        {
            currentHappy = happySprite.gameObject;
            currentHappy.SetActive(true);
        }

        if (sparkleIcon != null) sparkleIcon.SetActive(true);
    }

    private void SetAngry()
    {
        HideMoodIcons();

        if (currentHappy != null) currentHappy.SetActive(false);
        if (currentNeutral != null) currentNeutral.SetActive(true);

        if (frustratedIcon != null) frustratedIcon.SetActive(true);
    }

    private void HideAll()
    {
        foreach (Transform t in neutralParent) t.gameObject.SetActive(false);
        foreach (Transform t in happyParent) t.gameObject.SetActive(false);
        foreach (Transform t in accessoryParent) t.gameObject.SetActive(false);
        foreach (Transform t in effectParent) t.gameObject.SetActive(false);

        currentNeutral = null;
        currentHappy = null;
        currentAccessory = null;
    }

    private void HideMoodIcons()
    {
        if (sparkleIcon != null) sparkleIcon.SetActive(false);
        if (frustratedIcon != null) frustratedIcon.SetActive(false);
    }

    private string Capitalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s.Substring(1).ToLower();
    }
}