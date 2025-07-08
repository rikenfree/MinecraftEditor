using System.Collections.Generic;
using UnityEngine;

public class ColorClass : MonoBehaviour
{
    public static ColorClass instance;

    [SerializeField]
    public List<colorCombination> colors = new List<colorCombination>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public void GetIndex(int index)
    //{
    //    Debug.Log("Selected Color Index: " + index);
    //    PlayerPrefs.SetInt("selectedColorIndex", index);

    //    Refresh theme on current scene's UI
    //    ThemeManagment[] themeElements = FindObjectsByType<ThemeManagment>(FindObjectsSortMode.None);
    //    foreach (var element in themeElements)
    //    {
    //        element.ApplyThemeFromOutside();
    //    }
    //}
}

[System.Serializable]
public class colorCombination
{
    public Color backGroundcolor;
    public Color buttoncolor;
    public Color topBarcolor;
    public Color headerTextcolor;
    public Color buttonTextcolor;
    public Color normalTextcolor;
}
