using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperStarSdk
{
    public class SuperStarAd : MonoBehaviour
    {
        public static SuperStarAd Instance;

        public delegate void AfterInterstitialFunction();



        private static float lastInterstitial;

        private int triggeredAdRetryCounter;

        private static bool triggeredAdReady;


        public bool testMode = false;

        public IronSourceBannerPosition baanerPosition;
        public AdPosition baanerPositionadmob;

        public GameObject bannerImage;
        public GameObject adLoader;

        public GameObject SSIntrestitialPrefab;

        public int ReloadTime = 40;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

        }

        public int NoAds
        {
            get
            {
                return PlayerPrefs.GetInt("NoAds", 0);
            }
            set
            {
                PlayerPrefs.SetInt("NoAds", value);
            }
        }
        public void Setup()
        {
#if UNITY_ANDROID
            string appKey = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISAppKey;
#elif UNITY_IPHONE
        string appKey = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISAppKey;
#else
        string appKey = "unexpected_platform";
#endif

            IronSource.Agent.validateIntegration();

            Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            Debug.Log("unity-script: IronSource.Agent.init");
            IronSource.Agent.init(appKey);
            bannerImage.SetActive(false);

            if (NoAds == 0)
            {
                Debug.Log("no ads is inactive");
                ShowBannerAd();
                LoadInterstitialAd(0);
            }
            else
            {
                Debug.Log("no ads is activated");

                // ShowBannerAd();
                //LoadInterstitialAd(0);
            }
            lastInterstitial = -1000f;

        }

        void OnEnable()
        {
            //Add Rewarded Video Events
            //IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            //IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            //IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            //IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            //IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            //IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            //IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            //IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

            //Add AdInfo Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoAdClickedEvent;


            // Add Interstitial Events
            //IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            //IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            //IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            //IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            //IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            //IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            //IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;


            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialAdClosedEvent;

            // Add Banner Events
            //IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            //IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
            //IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
            //IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
            //IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            //IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;


            //Add AdInfo Banner Events
            IronSourceBannerEvents.onAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent += BannerAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
        }





        //void OnApplicationPause(bool isPaused)
        //{
        //    Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        //    IronSource.Agent.onApplicationPause(isPaused);

        //    if (!isPaused)
        //    {
        //        if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_appopen == 1)
        //        {

        //          //  AdmobManager.Instance.RequestAppOpenAds();

        //        }
        //    }
        //}

        #region RewardedAd callback handlers
        void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
        {
            Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
        }

        void RewardedVideoAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            isRewardGiven = true;
            isRewardShowing = false;
            Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
        }

        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            isRewardGiven = true;
            isRewardShowing = false;
            Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + placement.getRewardAmount() + " name = " + placement.getRewardName());
            //GiveRewardToUser();
        }

        void RewardedVideoAdClosedEvent(IronSourceAdInfo adInfo)
        {

            if (isRewardGiven)
            {
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(true);
                _callback = null;
            }
            else
            {
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(false);
                _callback = null;
            }
            isRewardShowing = false;
            //Time.timeScale = 1;
            // GiveRewardToUser();
            Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
        }
        private void RewardedVideoOnAdUnavailable()
        {
            Debug.Log("unity-script: RewardedVideoOnAdUnavailable");
            //throw new NotImplementedException();
        }

        private void RewardedVideoOnAdAvailable(IronSourceAdInfo obj)
        {
            Debug.Log("unity-script: RewardedVideoOnAdAvailable");
            //  throw new NotImplementedException();
        }
        void RewardedVideoAdStartedEvent()
        {
            Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
        }

        void RewardedVideoAdEndedEvent()
        {
            isRewardShowing = false;
            Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
        }

        void RewardedVideoAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            isRewardShowing = false;
            if (isRewardGiven)
            {
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(false);
                _callback = null;
            }
            // Time.timeScale = 1;
            Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void RewardedVideoAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + placement.getRewardName());
        }
        #endregion

        #region Interstitial callback handlers
        void InterstitialAdReadyEvent(IronSourceAdInfo adInfo)
        {
            isLoadingISIntrestital = false;
            Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        }

        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            //  Time.timeScale = 1;
            isLoadingISIntrestital = false;
            LoadInterstitialAd(0f);
            isIntrestitiallShowing = false;


            Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        }

        void InterstitialAdShowSucceededEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
            adLoader.SetActive(false);
            //if (GameManager.Instance.IdForShowIntrestitialAd == "TryAgain")
            //{
            //    GameManager.Instance.IdForShowIntrestitialAd = "";
            //    GameManager.Instance.CloseAdAfterCallForTryAgain();
            //}
        }

        void InterstitialAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            tryCount = 0;
            adLoader.SetActive(false);
            isIntrestitiallShowing = false;
            if (tryCount <= 0)
            {

                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
            }
            //Time.timeScale = 1;
            Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());

        }

        void InterstitialAdClickedEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialAdClickedEvent");
        }

        void InterstitialAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            // Time.timeScale = 0;
            Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
        }

        void InterstitialAdClosedEvent(IronSourceAdInfo adInfo)
        {
            //  Time.timeScale = 1;
            tryCount = 0;
            adLoader.SetActive(false);
            LoadInterstitialAd(0f);
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;

            Debug.Log("heheheheheh");

        }
        #endregion

        #region Banner callback handlers
        void BannerAdLoadedEvent(IronSourceAdInfo obj)
        {
            //Debug.Log("unity-script: I got BannerAdLoadedEvent");
            bannerImage.SetActive(true);
            Debug.Log("RefreshBanner");

        }

        void BannerAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        }

        void BannerAdClickedEvent(IronSourceAdInfo obj)
        {
            Debug.Log("unity-script: I got BannerAdClickedEvent");
        }

        void BannerAdScreenPresentedEvent(IronSourceAdInfo obj)
        {
            Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
        }

        void BannerAdScreenDismissedEvent(IronSourceAdInfo obj)
        {
            Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
        }

        void BannerAdLeftApplicationEvent(IronSourceAdInfo obj)
        {
            Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
        }
        #endregion


        public void ShowBannerAd()
        {
            if (NoAds != 1)
            {

                if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_banner_ads == 1)
                {
                    Debug.Log("Banner ad load");
                    IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, baanerPosition);
                    bannerImage.SetActive(true);
                }
                else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_banner == 1)
                {
                    AdmobManager.Instance.RequestBanner(baanerPositionadmob);
                    bannerImage.SetActive(true);
                }
            }
        }

        public void HideBannerAd()
        {
            Debug.Log("Hide Banner Ad");
            bannerImage.SetActive(false);
            IronSource.Agent.destroyBanner();
            AdmobManager.Instance.DestrotyBannerAd();
            //ShowBannerAd();
        }



        public bool isLoadingISIntrestital;
        public void LoadInterstitialAd()
        {
            if (!IronSource.Agent.isInterstitialReady() && !isLoadingISIntrestital)
            {
                isLoadingISIntrestital = true;
                IronSource.Agent.loadInterstitial();
            }
            Debug.Log("Load Interstitial AD");
            if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial == 1)
            {
                AdmobManager.Instance.LoadInterstitialAd();
            }

        }

        public void LoadInterstitialAd(float delay)
        {
            Invoke(nameof(LoadInterstitialAd), delay);
        }


        public void ShowInterstitial()
        {
            float time = Time.time;
            if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_interstitial_ads == 1 && IronSource.Agent.isInterstitialReady())
            {

                IronSource.Agent.showInterstitial();
                lastInterstitial = time;

            }
            //else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial == 1 && AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.IsLoaded())
            //{
            //    IronSource.Agent.loadInterstitial();
            //    AdmobManager.Instance.ShowInterstrial();
            //    lastInterstitial = time;
            //}

            else
            {
                LoadInterstitialAd(0f);



            }
        }

        public int InterstitialCounter = 0;


        public bool ISIntrestitialReadyToShow(bool ForceShow = false)
        {
            if (!IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.loadInterstitial();
            }
            float time = Time.time;
            bool isloadedTime = false;
            if (time - lastInterstitial >= ReloadTime)
            {
                //ShowInterstitial(f);
                //lastInterstitial = time;
                isloadedTime = true;
            }
            if (ForceShow)
            {
                isloadedTime = true;
            }
            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_interstitial_ads == 1 && IronSource.Agent.isInterstitialReady() && isloadedTime;
        }


        public void ExampleShowIntrestitialWithCallback()
        {
            ShowInterstitialTimer(TestIntrestitialWithCallback);
        }

        public void ExampleShowIntrestitialWithLoaderCallback()
        {
            ShowForceInterstitialWithLoader(TestIntrestitialWithCallback, 3);
        }

        public void TestIntrestitialWithCallback(bool isCompleted)
        {

            if (isCompleted)
            {
                //Give reward here
                Debug.Log("Intrestitial  completed  Do other thing");

            }
            else
            {
                Debug.Log("Intrestitial  has issue");

                // do next step as reward not available
            }
        }

        public bool IsAdmobIntrestitialAdAvailable(bool ForceShow = false)
        {
            float time = Time.time;
            bool isloadedTime = false;
            if (time - lastInterstitial >= ReloadTime)
            {
                //ShowInterstitial(f);
                //lastInterstitial = time;
                isloadedTime = true;
            }
            if (ForceShow)
            {
                isloadedTime = true;
            }
            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial == 1 && AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd() && isloadedTime;
        }

        public Action<bool> _callbackIntrestital;
        [HideInInspector]
        public bool isIntrestitiallShowing = false;
        public void ShowInterstitialTimer(Action<bool> onComplete)
        {
            tryCount = 0;
            Debug.Log("isIntrestitiallShowing => " + isIntrestitiallShowing);
            if (isIntrestitiallShowing)
            {
                return;
            }

            if (NoAds == 1)
            {
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                return;
            }
            isIntrestitiallShowing = true;
            _callbackIntrestital = onComplete;

            if (testMode)
            {
                isIntrestitiallShowing = false;
                adLoader.SetActive(false);
                if (_callbackIntrestital == null)
                {
                    return;
                }

                _callbackIntrestital.Invoke(true);
                _callbackIntrestital = null;
                return;
            }

#if UNITY_EDITOR
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
#endif
            //Regular call

            adLoader.SetActive(true);
            Debug.Log("ShowInterstitialTimer");
            if (IsAdmobIntrestitialAdAvailable() && ISIntrestitialReadyToShow())
            {
                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial_Probablity)
                {
                    AdmobManager.Instance.ShowInterstrial();
                    lastInterstitial = Time.time;
                }
                else
                {
                    IronSource.Agent.showInterstitial();
                    lastInterstitial = Time.time;
                }
            }
            else if (IsAdmobIntrestitialAdAvailable())
            {
                AdmobManager.Instance.ShowInterstrial();
                lastInterstitial = Time.time;
                if (!ISIntrestitialReadyToShow())
                {
                    IronSource.Agent.loadInterstitial();
                }
            }
            else if (ISIntrestitialReadyToShow())
            {
                Debug.Log("Ready to show");
                IronSource.Agent.showInterstitial();
                lastInterstitial = Time.time;
            }
           
            else
            {
                if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_ssinterstitial == 1)
                {
                    Debug.Log("SSIntrestitialPrefab Ready to show");
                    Instantiate(SSIntrestitialPrefab);
                }
                else
                {

                    Debug.Log("can't show");
                    adLoader.SetActive(false);
                    isIntrestitiallShowing = false;
                    if (_callbackIntrestital == null)
                    {
                        return;
                    }
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }


                LoadInterstitialAd(0f);
            }
        }


        public void OnCloseSSIntrestitial()
        {
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
        }

        public void ShowForceInterstitial(Action<bool> onComplete)
        {
            tryCount = 0;
            if (isIntrestitiallShowing)
            {
                return;
            }
            if (NoAds == 1)
            {
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                return;
            }
            isIntrestitiallShowing = true;
            _callbackIntrestital = onComplete;


            if (testMode)
            {
                isIntrestitiallShowing = false;
                adLoader.SetActive(false);
                if (_callbackIntrestital == null)
                {
                    return;
                }

                _callbackIntrestital.Invoke(true);
                _callbackIntrestital = null;
                return;
            }
#if UNITY_EDITOR
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
#endif

            //Regular call

            adLoader.SetActive(true);
            if (IsAdmobIntrestitialAdAvailable() && ISIntrestitialReadyToShow())
            {

                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial_Probablity)
                {
                    AdmobManager.Instance.ShowInterstrial();
                    lastInterstitial = Time.time;
                    if (!ISIntrestitialReadyToShow())
                    {
                        IronSource.Agent.loadInterstitial();
                    }
                }
                else
                {
                    IronSource.Agent.showInterstitial();
                    lastInterstitial = Time.time;
                }

            }
            else if (IsAdmobIntrestitialAdAvailable(true))
            {
                AdmobManager.Instance.ShowInterstrial();
                lastInterstitial = Time.time;
                if (!ISIntrestitialReadyToShow())
                {
                    IronSource.Agent.loadInterstitial();
                }
            }
            else if (ISIntrestitialReadyToShow(true))
            {
                IronSource.Agent.showInterstitial();
                lastInterstitial = Time.time;
            }
           
            else
            {
                if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_ssinterstitial == 1)
                {
                    Debug.Log("SSIntrestitialPrefab Ready to show");
                    Instantiate(SSIntrestitialPrefab);
                }
                else
                {
                    adLoader.SetActive(false);
                    isIntrestitiallShowing = false;
                    if (_callbackIntrestital == null)
                    {
                        return;
                    }
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }
                LoadInterstitialAd(0f);
            }
        }


        public int tryCount;
        public void ShowForceInterstitialWithLoader(Action<bool> onComplete, int _tryCount)
        {
            tryCount = _tryCount;
            // adLoader.SetActive(true);
            //alreay showing
            if (isIntrestitiallShowing)
            {
                return;
            }

            // No Ads Purchased
            if (NoAds == 1)
            {
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                return;
            }

            //Regular call

            adLoader.SetActive(true);

            isIntrestitiallShowing = true;
            _callbackIntrestital = onComplete;

            if (testMode)
            {
                isIntrestitiallShowing = false;
                adLoader.SetActive(false);
                if (_callbackIntrestital == null)
                {
                    return;
                }

                _callbackIntrestital.Invoke(true);
                _callbackIntrestital = null;
                return;
            }

#if UNITY_EDITOR
            isIntrestitiallShowing = false;
            adLoader.SetActive(false);
            if (_callbackIntrestital == null)
            {
                return;
            }

            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
#endif
            if (IsAdmobIntrestitialAdAvailable() && ISIntrestitialReadyToShow())
            {
                Debug.LogError("Both loaded");
                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial_Probablity)
                {
                    AdmobManager.Instance.ShowInterstrial();
                    lastInterstitial = Time.time;
                    if (!ISIntrestitialReadyToShow())
                    {
                        IronSource.Agent.loadInterstitial();
                    }
                }
                else
                {
                    IronSource.Agent.showInterstitial();
                    lastInterstitial = Time.time;
                }

            }
            else if (IsAdmobIntrestitialAdAvailable(true))
            {
                AdmobManager.Instance.ShowInterstrial();
                lastInterstitial = Time.time;
                if (!ISIntrestitialReadyToShow())
                {
                    IronSource.Agent.loadInterstitial();
                }
            }
            else if (ISIntrestitialReadyToShow(true))
            {
                IronSource.Agent.showInterstitial();
                lastInterstitial = Time.time;
            }
            else
            {

                if (tryCount > 0)
                {
                    // tryCount--;
                    isIntrestitiallShowing = false;
                    LoadInterstitialAd(0);
                    StartCoroutine(IEShowForceInterstitialWithLoader(_callbackIntrestital));
                }
                else
                {
                    adLoader.SetActive(false);
                    isIntrestitiallShowing = false;
                    if (_callbackIntrestital == null)
                    {
                        return;
                    }
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }
                LoadInterstitialAd(0f);
            }
        }

        public IEnumerator IEShowForceInterstitialWithLoader(Action<bool> onComplete)
        {
            tryCount--;
            if (tryCount > 0)
            {
                yield return new WaitForSeconds(1f);
                ShowForceInterstitialWithLoader(onComplete, tryCount);
            }
            else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_ssinterstitial == 1)
            {
                Debug.Log("SSIntrestitialPrefab Ready to show");
                Instantiate(SSIntrestitialPrefab);
            }
            else
            {
                tryCount = 0;
                adLoader.SetActive(false);
                isIntrestitiallShowing = false;
                if (_callbackIntrestital != null)
                {
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }
                LoadInterstitialAd(0f);
            }
        }

        public void ExampleShowReward()
        {
            ShowRewardVideo(ExampleShowRewardAssign);
        }

        public void ExampleShowRewardAssign(bool isrewarded)
        {
            if (isrewarded)
            {
                //Give reward here
                Debug.Log("Reward Given");
            }
            else
            {
                Debug.Log("Reward Eroor Given");
                // do next step as reward not available
            }
        }
        public Action<bool> _callback;
        [HideInInspector]
        public bool isRewardShowing = false;
        string VideoFor = "";
        [HideInInspector]
        public bool isRewardGiven = false;

        public bool IsAdmobRewardAdAvailable()
        {

            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_reward == 1 && AdmobManager.Instance.rewardedAd != null && AdmobManager.Instance.rewardedAd.CanShowAd();

            //return AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd();
        }

        public bool IsISRewardAdAvailable()
        {

            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_reward_ads == 1 && IronSource.Agent.isRewardedVideoAvailable();

            //return AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd();
        }
        public void ShowRewardVideo(Action<bool> onComplete)
        {
            Debug.Log("Show Reward video Ad");

            if (isRewardShowing)
            {
                return;
            }



            isRewardGiven = false;
            isRewardShowing = true;
            _callback = onComplete;

            if (testMode)
            {
                isRewardShowing = false;
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(true);
                _callback = null;
                return;
            }
            //GiveRewardToUser();
#if UNITY_EDITOR
            isRewardShowing = false;
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(true);
            _callback = null;
            return;
#endif

            if (IsISRewardAdAvailable() && IsAdmobRewardAdAvailable())
            {

                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_reward_Probablity)
                {
                    AdmobManager.Instance.ShowRewardVideo();
                }
                else
                {
                    IronSource.Agent.showRewardedVideo();

                }
            }
            else if (IsISRewardAdAvailable())
            {
                //SoundManager.Instance.StopAllSoundRunningVideoAd(true);
                IronSource.Agent.showRewardedVideo();
                AdmobManager.Instance.RequestRewardAd();
            }
            else if (IsAdmobRewardAdAvailable())
            {
                AdmobManager.Instance.ShowRewardVideo();
                IronSource.Agent.loadRewardedVideo();
            }
            else
            {
                AdmobManager.Instance.RequestRewardAd();
                IronSource.Agent.loadRewardedVideo();
                Debug.LogError("Problem in showing video");
                isRewardShowing = false;
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(false);
                _callback = null;
                return;
                // NotificationHandler.Instance.ShowNotification("Reward Ad is not available!");
            }
        }

        public void GiveRewardToUser()
        {
            Debug.LogError("watch video comepleted:" + VideoFor);
            switch (VideoFor)
            {

            }
        }




    }




}
