using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class CustomLoding2 : MonoBehaviour
{
    public TextMeshProUGUI welcomePercentText;
    public TextMeshProUGUI tapToContinueText;
    public Image welcomeProgress;

    public float fillDuration = 3f; // How long to fill (seconds)

    private bool loadingDone = false;

    void Start()
    {
        tapToContinueText.gameObject.SetActive(false);
        welcomeProgress.fillAmount = 0f; // Make sure your Image Type is set to Filled in the Inspector!
        welcomePercentText.text = "0 %";

    }

    void Update()
    {

        if (!loadingDone)
        {
            if (welcomeProgress.fillAmount != 1f)
            {
                welcomeProgress.fillAmount += welcomeProgress.fillAmount + Time.deltaTime * fillDuration;
                welcomePercentText.text = (int)(welcomeProgress.fillAmount * 100f) + " %";
            }
            else
            {
                tapToContinueText.gameObject.SetActive(true);
                loadingDone = true;
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
