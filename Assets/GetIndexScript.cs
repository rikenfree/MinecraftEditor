using UnityEngine;

public class GetIndexScript : MonoBehaviour
{
    public void GetIndex(int index)
    {
        Debug.Log("Selected Color Index: " + index);
        PlayerPrefs.SetInt("selectedColorIndex", index);

        // Refresh theme on current scene's UI
        ThemeManagment[] themeElements = FindObjectsByType<ThemeManagment>(FindObjectsSortMode.None);
        foreach (var element in themeElements)
        {
            element.ApplyThemeFromOutside();
        }
    }
}
