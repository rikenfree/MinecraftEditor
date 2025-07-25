using UnityEngine;
using UnityEngine.SceneManagement;
using SuperStarSdk;
using Main.Controller;
using System.Collections;

public class ManageScene : MonoBehaviour
{
    public GameObject ColorPanel;

    private void Start()
    {
        // Show color panel only once on first launch
        if (!PlayerPrefs.HasKey("ColorPanelShown"))
        {
            StartCoroutine(OpenColorPanelDelayed());
            PlayerPrefs.SetInt("ColorPanelShown", 1);
        }
        else
        {
            ColorPanel.SetActive(false);
        }
    }

    IEnumerator OpenColorPanelDelayed()
    {
        yield return new WaitForSeconds(0.5f); // Wait 5 seconds before showing
        ColorPanel.SetActive(true);
    }

    public void SkinEditor()
    {
        SoundControllerMain.instance.PlayClickSound();
        SceneManager.LoadSceneAsync(1);
    }

    public void CapEditor()
    {
        SoundControllerMain.instance.PlayClickSound();
        SceneManager.LoadSceneAsync(3);
    }

    public void MapsMinecraft()
    {
        SoundControllerMain.instance.PlayClickSound();
        SceneManager.LoadSceneAsync(6);
    }

    public void OnBackButtonClick()
    {
        SoundControllerMain.instance.PlayClickSound();
        SceneManager.LoadSceneAsync(3);
    }

    public void ClickButtonShare()
    {
        gameObject.SetActive(false);
        SuperStarSdkManager.Instance.Share();
    }

    public void ClickButtonPlaySound()
    {
        SoundControllerMain.instance.PlayClickSound();
    }
}
