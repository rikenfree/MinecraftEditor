using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThemeManagment : MonoBehaviour
{
    public Type type;
    Image image;
    TextMeshProUGUI textMeshPro;
    Text uiText;

    private void OnEnable()
    {
        // Apply theme when the object becomes active (after scene change etc.)
        if (ColorClass.instance != null)
        {
            ApplyTheme();
        }
    }

    private IEnumerator Start()
    {
        // Wait until ColorClass.instance is assigned (only for first scene load)
        yield return new WaitUntil(() => ColorClass.instance != null);

        image = GetComponent<Image>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        uiText = GetComponent<Text>();

        ApplyTheme();
    }

    private void ApplyTheme()
    {
        int selectedIndex = PlayerPrefs.GetInt("selectedColorIndex", 0);

        switch (type)
        {
            case Type.topBar:
                if (image != null)
                    image.color = ColorClass.instance.colors[selectedIndex].topBarcolor;
                break;

            case Type.button:
                if (image != null)
                    image.color = ColorClass.instance.colors[selectedIndex].buttoncolor;
                break;

            case Type.headertext:
                if (textMeshPro != null)
                    textMeshPro.color = ColorClass.instance.colors[selectedIndex].headerTextcolor;
                else if (uiText != null)
                    uiText.color = ColorClass.instance.colors[selectedIndex].headerTextcolor;
                break;

            case Type.buttontext:
                if (textMeshPro != null)
                    textMeshPro.color = ColorClass.instance.colors[selectedIndex].buttonTextcolor;
                else if (uiText != null)
                    uiText.color = ColorClass.instance.colors[selectedIndex].buttonTextcolor;
                break;

            case Type.normaltext:
                if (textMeshPro != null)
                    textMeshPro.color = ColorClass.instance.colors[selectedIndex].normalTextcolor;
                else if (uiText != null)
                    uiText.color = ColorClass.instance.colors[selectedIndex].normalTextcolor;
                break;

            case Type.background:
                if (image != null)
                    image.color = ColorClass.instance.colors[selectedIndex].backGroundcolor;
                break;
        }
    }

    // Public method for external calls (from ColorClass)
    public void ApplyThemeFromOutside()
    {
        ApplyTheme();
    }

    // Optional utility to get color from PlayerPrefs string
    public static Color GetColorFromPrefs(string playerPrefsKey)
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string hex = PlayerPrefs.GetString(playerPrefsKey);
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                return color;
            }
            else
            {
                Debug.LogWarning("Invalid Hex Color in key: " + playerPrefsKey + " → Value: " + hex);
                return Color.white;
            }
        }
        else
        {
            Debug.LogWarning("No color found in PlayerPrefs with key: " + playerPrefsKey);
            return Color.white;
        }
    }

    public enum Type
    {
        none,
        button,
        topBar,
        headertext,
        buttontext,
        normaltext,
        background
    }
}
