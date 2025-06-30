using Main.Controller;
using TMPro;
using UnityEngine.UI;

namespace Main.View
{
	public class InfoCanvas1 : SceneElement1
	{
		public TextMeshProUGUI title;

		public TextMeshProUGUI info;

		public void ShowAboutUsInfo()
		{
			title.text = "Skin Editor 3D Version 1.1.7";
			info.text = "Contact us: remoro.studios@gmail.com";
			base.gameObject.SetActive(value: true);
		}

		public void ShowMCPEExportSuccessInfo()
		{
			title.text = "Success!";
			info.text = "Please restart MCPE :D";
			base.gameObject.SetActive(value: true);
		}

		public void ShowGalleryExportSuccessInfo()
		{
			title.text = "Success!";
			info.text = "Skin is saved to gallery. ";
			base.gameObject.SetActive(value: true);
		}

		public void Show(string t, string i)
		{
			title.text = t;
			info.text = i;
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
		}
	}
}
