using Main.Controller;
using TMPro;
using UnityEngine.UI;

namespace Main.View
{
	public class ErrorCanvas1 : SceneElement1
	{
		public TextMeshProUGUI title;

		public TextMeshProUGUI info;

		public void ShowCustomError(string titleString, string infoString)
		{
			title.text = titleString;
			info.text = infoString;
			base.gameObject.SetActive(value: true);
		}

		public void ShowLoadingSkinOnlineError(bool random)
		{
			title.text = "Can't download skin";
			if (random)
			{
				info.text = "Please try again.";
			}
			else
			{
				info.text = "1. Check Internet connection.\n2. Check if the name is correct.";
			}
			base.gameObject.SetActive(value: true);
		}

		public void ShowSkinWrongSizeError()
		{
			title.text = "Unsupported Skin Size.";
			info.text = "We only support 64x32 or 64x64 skin.";
			base.gameObject.SetActive(value: true);
		}

		public void ShowCantFindMCPESkinError()
		{
			title.text = "Can't Locate MCPE Skin";
			info.text = "Please import from Gallery instead.";
			base.gameObject.SetActive(value: true);
		}

		public void ShowCantSaveMCPESkinError()
		{
			title.text = "Can't Save MCPE Skin";
			info.text = "Please export to Gallery instead.";
			base.gameObject.SetActive(value: true);
		}

		public void ShowCantSaveGallerySkinError()
		{
			title.text = "Error";
			info.text = "Can't save skin to gallery.";
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			SoundController1.Instance.PlayClickSound();
			base.gameObject.SetActive(value: false);
		}
	}
}
