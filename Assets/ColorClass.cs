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
