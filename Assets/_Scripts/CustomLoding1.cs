using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Main.Controller;
using I2.Loc;

public class CustomLoding1 : MonoBehaviour
{
    public TextMeshProUGUI welcomePercentText;

    public TextMeshProUGUI SuccessText;

    public GameObject CapeEditorButton;
    public GameObject CustomCapeAddonButton;
    public GameObject ProgressBar;

    public UnityEngine.UI.Image welcomeProgress;

    private float welcomeWaitTime = 0f;

    private bool welcomeDone = false;

    // Start is called before the first frame update
    private void Awake()
    {
#if UNITY_ANDROID
        welcomeWaitTime = 8;
#elif UNITY_IOS
        welcomeWaitTime=0;
#endif
    }

    public void Start()
    {
        ProgressBar.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!welcomeDone)
        {
            float time = Time.time;
            if (time >= welcomeWaitTime)
            {
                welcomeProgress.fillAmount = 1f;
                SuccessText.gameObject.SetActive(true);
                welcomePercentText.gameObject.SetActive(false);

                StartCoroutine(SetSuceessText());
            }
            else
            {
                welcomePercentText.gameObject.SetActive(true);
                int num = (int)(time / welcomeWaitTime * 100f);
                welcomeProgress.fillAmount = time / welcomeWaitTime;
                welcomePercentText.text = num + " %";

                SuccessText.gameObject.SetActive(false);

                CapeEditorButton.SetActive(false);
                CustomCapeAddonButton.SetActive(false);
            }
        }
    }

    public IEnumerator SetSuceessText()
    {
        yield return new WaitForSeconds(2f);
        welcomeDone = true;
        ProgressBar.SetActive(false);

        CapeEditorButton.SetActive(true);
        //string text1 = "Custom Cape Addon";
        //CapeEditorButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationManager.GetTranslation(text1);

        CustomCapeAddonButton.SetActive(true);
        //string text = "Cape Editor";
        //CustomCapeAddonButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationManager.GetTranslation(text);
    }
    public void OnClickCapeEditorButton()
    {
        SoundController1.Instance.PlayClickSound();
        StartCoroutine(LoadSceneAsync(4));
    }

    public void OnClickCustomCapeAddonButton()
    {
        SoundController1.Instance.PlayClickSound();
        StartCoroutine(LoadSceneAsync(5));
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
