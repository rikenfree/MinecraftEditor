using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SuperStarSdk;
using System;

public class CustomLoding2 : MonoBehaviour
{
    public Text welcomePercentText;

    public Text welcomeDoneText;

    public Image welcomeProgress;

    private float welcomeWaitTime = 10f;

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
    private void Start()
    {
        LoadSceneAsync();
    }
    // Update is called once per frame
    void Update()
    {
        if (!welcomeDone)
        {
            float time = Time.time;
            if (time >= welcomeWaitTime )
            {
                welcomeProgress.fillAmount = 1f;
                welcomePercentText.text = "SUCCESS";
                welcomeDoneText.gameObject.SetActive(true);
                welcomeDone = true;
            }
            //else if (time >= welcomeWaitTime)
            //{
            //    //int num = (int)(time / welcomeWaitTime * 100f);
            //    //welcomePercentText.text = 99 + " %";
            //    welcomeProgress.fillAmount = 0.99f;
            //    welcomePercentText.text = "Map Data Loading....";
            // //   welcomeDoneText.gameObject.SetActive(true);
            //   // welcomeDone = true;
            //}
            else
            {
                int num = (int)(time / welcomeWaitTime * 100f);
                welcomeProgress.fillAmount = time / welcomeWaitTime;
                welcomePercentText.text = num + " %";
                welcomeDoneText.gameObject.SetActive(false);
            }
        }
    }
    public void OnClickContinueButton()
    {
        if (welcomeDone)
        {
            //SceneManager.LoadScene(1);

            //SuperStarAd.Instance.ShowInterstitial();
            asyncLoad.allowSceneActivation = true;

        }
    }

    AsyncOperation asyncLoad;
    public void LoadSceneAsync() 
    {

        asyncLoad = SceneManager.LoadSceneAsync(7);
        asyncLoad.allowSceneActivation = false;

    }
}
