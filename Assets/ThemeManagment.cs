using UnityEngine;
using UnityEngine.UI;

public class ThemeManagment : MonoBehaviour
{
    public Type type;
    Image image;

    private void OnEnable()
    {
        image = GetComponent<Image>();

        if(type == Type.topBar)
        {
            image.color = GetColorFromPrefs("topBar");
        }
    }

   


    public static Color GetColorFromPrefs(string playerPrefsKey)
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string hex = PlayerPrefs.GetString(playerPrefsKey);
            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                return color;
            }
            else
            {
                return Color.white;
                Debug.LogWarning("Invalid Hex Color in key: " + playerPrefsKey + " → Value: " + hex);
            }
        }
        else
        {
            Debug.LogWarning("No color found in PlayerPrefs with key: " + playerPrefsKey);
        }

        return Color.white;
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
