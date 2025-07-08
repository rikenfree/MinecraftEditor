using UnityEngine;
using UnityEngine.SceneManagement;
using SuperStarSdk;
using Main.Controller;
using System.Collections;

public class ManageScene : MonoBehaviour
{
    public void SkinEditor()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                StartCoroutine(LoadSceneAsync(1));
            }, 3);
        }
        else
        {
            StartCoroutine(LoadSceneAsync(1));
        }
    }

    public void CapEditor()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                StartCoroutine(LoadSceneAsync(3));
            }, 3);
        }
        else
        {
            StartCoroutine(LoadSceneAsync(3));
        }
    }

    public void MapsMinecraft()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                StartCoroutine(LoadSceneAsync(6));
            }, 3);
        }
        else
        {
            StartCoroutine(LoadSceneAsync(6));
        }
    }

    public void OnBackButtonClick()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                StartCoroutine(LoadSceneAsync(0));
            }, 3);
        }
        else
        {
            StartCoroutine(LoadSceneAsync(0));
        }
    }

    public void ClickButtonShare()
    {
        SuperStarSdkManager.Instance.Share();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Optionally show loading UI here
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
