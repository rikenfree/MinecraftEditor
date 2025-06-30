using UnityEngine;
using UnityEngine.SceneManagement;
using SuperStarSdk;
using Main.Controller;

public class ManageScene : MonoBehaviour
{
    public void SkinEditor()
    {
        SceneManager.LoadSceneAsync(1);
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
