using Main.Controller;
using SuperStarSdk;
using System.IO;
using UnityEngine;

namespace Main.View
{
	public class SaveCharacterCanvas : SceneElement
	{
		private Character character;

		private RootController ctrl;

		public InfoCanvas infoCanvas;

		private void Awake()
		{
			ctrl = base.scene.controller;
		}

		private void Start()
		{
			character = base.scene.view.character;
		}

		public void ClickMCPEButton()
		{
			base.scene.controller.sound.PlayClickSound();
			Hide();
            //ctrl.ga.SendEvent("Button Click", "Save to MCPE", "");
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((result) => {
                Debug.Log("Show Intrestitial  => " + result);
            }, 1);
#if UNITY_ANDROID

            CharacterSaveMCPE();
#elif UNITY_IOS
       CharacterSaveGallery();
#endif

        }

		private void CharacterSaveMCPE()
		{
			character.SaveMCPE();
		}

		public void ClickGalleryButton()
		{
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((result) =>
			{
				base.scene.controller.sound.PlayClickSound();
				CharacterSaveGallery();
			}
			, 3);
        }

		private void CharacterSaveGallery(bool isAdLoaded =false)
		{
			character.SaveGallery();
		}

		public void ClickEndlessHillsButton()
		{
			base.scene.controller.sound.PlayClickSound();
			Hide();
			//ctrl.ga.SendEvent("Button Click", "Save to Endless Hills", "");
			//AndroidNativeUtility.ExternalStoragePathLoaded += CheckEndlessHillsFolderExist;
			//SA_Singleton<AndroidNativeUtility>.Instance.GetExternalStoragePath();
		}

		

		public void ClickDiamondMasterButton()
		{
			base.scene.controller.sound.PlayClickSound();
			Hide();
		//	ctrl.ga.SendEvent("Button Click", "Save to Diamond Master", "");
			//AndroidNativeUtility.ExternalStoragePathLoaded += CheckDiamondMasterFolderExist;
			//SA_Singleton<AndroidNativeUtility>.Instance.GetExternalStoragePath();
		}
        
		public void ClickCloseButton()
		{
			base.scene.controller.sound.PlayClickSound();
			Hide();
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
