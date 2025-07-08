using UnityEngine;

public class GetIndexScript : MonoBehaviour
{
    public void GetIndex(int index)
    {
        Debug.Log("Selected Color Index: " + index);
        PlayerPrefs.SetInt("selectedColorIndex", index);

        // Refresh theme on ALL UI elements (including inactive ones)
        ThemeManagment[] themeElements = Resources.FindObjectsOfTypeAll<ThemeManagment>();
        foreach (var element in themeElements)
        {
            if (element.gameObject.scene.IsValid()) // exclude prefabs
            {
                element.ApplyThemeFromOutside();
            }
        }
    }
}
