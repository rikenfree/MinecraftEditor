using UnityEngine;
using UnityEngine.SceneManagement;
using SuperStarSdk;
using Main.Controller;

public class ManageScene : MonoBehaviour
{
    public void SkinEditor()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                SceneManager.LoadSceneAsync(1);

            },3);
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    public void CapEditor()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void MapsMinecraft()
    {
        SceneManager.LoadSceneAsync(6);
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ClickButtonShare()
    {
        SoundController.instance.PlayClickSound();
        base.gameObject.SetActive(value: false);
        SuperStarSdkManager.Instance.Share();
    }


}
