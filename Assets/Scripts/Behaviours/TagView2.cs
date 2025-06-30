using UnityEngine;
using UnityEngine.UI;

public class TagView2 : MonoBehaviour
{
	public Image background;

	public Text nameText;

	public void SetName(string name)
	{
		nameText.text = name;
	}

	public void Clicked()
	{
		SoundManager2.instance.PlayButtonSound();
		GuiManager2.instance.ShowTagsMapsByTag(nameText.text);
	}
}
