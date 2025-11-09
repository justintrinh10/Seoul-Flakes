using System;
using UnityEngine;

public class CustomerRenderer : MonoBehaviour
{
    [Header("Sprite Parents")]
    [SerializeField] private Transform neutralParent;
    [SerializeField] private Transform happyParent;
    [SerializeField] private Transform accessoryParent;
    [SerializeField] private Transform effectParent;

    private GameObject currentNeutral;
    private GameObject currentHappy;
    private GameObject currentAccessory;
    private GameObject sparkleIcon;
    private GameObject frustratedIcon;

    private string currentColor = "";

    private void Awake()
    {
        if (neutralParent == null) neutralParent = transform.Find("Neutral");
        if (happyParent == null) happyParent = transform.Find("Happy");
        if (accessoryParent == null) accessoryParent = transform.Find("Accessory");
        if (effectParent == null) effectParent = transform.Find("Effect");

        if (effectParent != null)
        {
            sparkleIcon = effectParent.Find("sparkleIcon")?.gameObject;
            frustratedIcon = effectParent.Find("frustratedIcon")?.gameObject;
        }

        // Call to set sorting layer for all renderers
        SetSortingLayerAndOrderForAllRenderers("Customer");

        HideAll();
    }

    private void OnEnable()
    {
        SubscribeToCustomerEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromCustomerEvents();
    }

    private void SubscribeToCustomerEvents()
    {
        var type = typeof(Customer);
        var changeAppearanceField = type.GetField("changeAppearance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var onStateChangeField = type.GetField("onStateChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (changeAppearanceField != null)
        {
            Action<string, string> changeAppearance = (Action<string, string>)changeAppearanceField.GetValue(null);
            changeAppearance += UpdateAppearance;
            changeAppearanceField.SetValue(null, changeAppearance);
        }

        if (onStateChangeField != null)
        {
            Action<string> onStateChange = (Action<string>)onStateChangeField.GetValue(null);
            onStateChange += UpdateState;
            onStateChangeField.SetValue(null, onStateChange);
        }
    }

    private void UnsubscribeFromCustomerEvents()
    {
        var type = typeof(Customer);
        var changeAppearanceField = type.GetField("changeAppearance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var onStateChangeField = type.GetField("onStateChange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (changeAppearanceField != null)
        {
            Action<string, string> changeAppearance = (Action<string, string>)changeAppearanceField.GetValue(null);
            changeAppearance -= UpdateAppearance;
            changeAppearanceField.SetValue(null, changeAppearance);
        }

        if (onStateChangeField != null)
        {
            Action<string> onStateChange = (Action<string>)onStateChangeField.GetValue(null);
            onStateChange -= UpdateState;
            onStateChangeField.SetValue(null, onStateChange);
        }
    }

    private void HideAll()
    {
        if (neutralParent != null)
        {
            foreach (Transform child in neutralParent)
                child.gameObject.SetActive(false);
        }
        if (happyParent != null)
        {
            foreach (Transform child in happyParent)
                child.gameObject.SetActive(false);
        }
        if (accessoryParent != null)
        {
            foreach (Transform child in accessoryParent)
                child.gameObject.SetActive(false);
        }
        if (effectParent != null)
        {
            foreach (Transform child in effectParent)
                child.gameObject.SetActive(false);
        }
    }

    private void UpdateAppearance(string color, string accessory)
    {
        HideAll();

        currentColor = color;

        string neutralName = "neutral" + Capitalize(color);
        Transform neutral = neutralParent.Find(neutralName);
        if (neutral != null)
        {
            currentNeutral = neutral.gameObject;
            currentNeutral.SetActive(true);
        }

        Transform acc = accessoryParent.Find(accessory);
        if (acc != null)
        {
            currentAccessory = acc.gameObject;
            currentAccessory.SetActive(true);
        }
    }

    private void UpdateState(string state)
    {
        if (sparkleIcon != null) sparkleIcon.SetActive(false);
        if (frustratedIcon != null) frustratedIcon.SetActive(false);

        if (currentNeutral != null) currentNeutral.SetActive(false);
        if (currentHappy != null) currentHappy.SetActive(false);

        if (state == "happy")
        {
            string happyName = "happy" + Capitalize(currentColor);
            Transform happy = happyParent.Find(happyName);
            if (happy != null)
            {
                currentHappy = happy.gameObject;
                currentHappy.SetActive(true);
            }

            if (sparkleIcon != null) sparkleIcon.SetActive(true);
        }
        else if (state == "angry")
        {
            if (frustratedIcon != null) frustratedIcon.SetActive(true);
            if (currentNeutral != null) currentNeutral.SetActive(true);
            else
            {
                string neutralName = "neutral" + Capitalize(currentColor);
                Transform neutral = neutralParent.Find(neutralName);
                if (neutral != null)
                {
                    currentNeutral = neutral.gameObject;
                    currentNeutral.SetActive(true);
                }
            }
        }
        else
        {
            string neutralName = "neutral" + Capitalize(currentColor);
            Transform neutral = neutralParent.Find(neutralName);
            if (neutral != null)
            {
                currentNeutral = neutral.gameObject;
                currentNeutral.SetActive(true);
            }
        }
    }

    private string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }

private void SetSortingLayerAndOrderForAllRenderers(string layerName)
{
    SetSortingLayerForChildren(neutralParent, layerName, 0); // Body gets 0 or low order
    SetSortingLayerForChildren(happyParent, layerName, 0);   // Body gets 0 or low order
    SetSortingLayerForChildren(accessoryParent, layerName, 10); // Accessories get higher order (e.g. 10)
    SetSortingLayerForChildren(effectParent, layerName, 0);  // Effects can be 0 or low order (or a separate layer)
}

private void SetSortingLayerForChildren(Transform parent, string layerName, int sortingOrder)
{
    if (parent == null) return;

    // Loop through all children of the parent transform
    foreach (Transform child in parent)
    {
        // Try to get the SpriteRenderer component of the child
        SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Set the sorting layer and order
            spriteRenderer.sortingLayerName = layerName;
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}

private void Awake()
{
    if (neutralParent == null) neutralParent = transform.Find("Neutral");
    if (happyParent == null) happyParent = transform.Find("Happy");
    if (accessoryParent == null) accessoryParent = transform.Find("Accessory");
    if (effectParent == null) effectParent = transform.Find("Effect");

    if (effectParent != null)
    {
        sparkleIcon = effectParent.Find("sparkleIcon")?.gameObject;
        frustratedIcon = effectParent.Find("frustratedIcon")?.gameObject;
    }

    // Call to set sorting layer for all renderers
    SetSortingLayerAndOrderForAllRenderers("Customer");

    HideAll();
}

}