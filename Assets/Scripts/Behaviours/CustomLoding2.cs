using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using I2.Loc;

public class CustomLoding2 : MonoBehaviour
{
    public TextMeshProUGUI welcomePercentText;
    public TextMeshProUGUI tapToContinueText;
    public Image welcomeProgress;

    public GameObject ProgressBar;

    public float fillDuration = 6f; // How long to fill (seconds)

    private bool loadingDone = false;

    void Start()
    {
        tapToContinueText.gameObject.SetActive(false);
        //welcomeProgress.fillAmount = 0f; // Make sure your Image Type is set to Filled in the Inspector!
        //welcomePercentText.text = "0 %";

    }

    void Update()
    {

        if (!loadingDone)
        {
            float time = Time.time;
            if (time >= fillDuration)
            {
                welcomeProgress.fillAmount = 1f;

                welcomePercentText.text = "SUCCESS";
                string s = LocalizationManager.GetTranslation(welcomePercentText.text);
                welcomePercentText.text = s;

                tapToContinueText.gameObject.SetActive(true);
                loadingDone = true;
                ProgressBar.SetActive(false);
            }
            else
            {
                int num = (int)(time / fillDuration * 100f);
                welcomeProgress.fillAmount = time / fillDuration;
                welcomePercentText.text = num + " %";
                tapToContinueText.gameObject.SetActive(false);
            }
        }
    }



    public void OnClickContinueButton()
    {
        if (loadingDone)
        {
            SoundControllerMain.instance.PlayClickSound();
            SceneManager.LoadSceneAsync(7);
        }
    }
}
