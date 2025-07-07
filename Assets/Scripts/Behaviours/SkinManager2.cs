using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStarSdk;
public class SkinManager2 : MonoBehaviour
{
    public static SkinManager2 Instance;


    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    public Texture2D currentskin;


    public void SelectSkin(Texture2D tex,Texture2D uiskin) {
        currentskin = tex;
        GuiManager2.instance.ShowSkinDownloadPanel(uiskin);
    }

    public void DownloadCurrentSkin() {

        SuperStarAd.Instance.ShowForceInterstitialWithLoader((result)=> {

            if (result)
            {
                if (result)
                {

                    // minecraftButton.SetActive(false);
                    NativeGallery.SaveImageToGallery(currentskin, "3DSkins", "Skin", null);

                   // HideExportPanel();
                   // ShowAlertPanel("Skin Saved Successfully!.", danger: false);
                  //  SuperStarSdkManager.Instance.Rate();
                    ToastManager.Instance.ShowToast("Skin Saved Successfully!.");


                }
                else
                {
                    ToastManager.Instance.ShowToast("AD is not available");
                }
            }
        },3);
    }

}
