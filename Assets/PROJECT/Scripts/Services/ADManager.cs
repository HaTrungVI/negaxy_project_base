using System.Collections;
using System.Collections.Generic;
using QuocAnh.pattern;
using Sirenix.OdinInspector;
using UnityEngine;

public class ADManager : Singleton<ADManager>
{

    #region MANDATORY ID

    [Header("MANDATORY AD ID ")]
    public string SDKKey; // SDK key
    public string UserID; // user id
    public bool useBannerAd; // whether to use banner ad
    public bool useInterAd; // whether to use intern ad
    public bool userRewardAd; // whether to use reward ad

    #endregion


    #region BANNER AD VAR

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd")]
    [Header("BANNER AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd")]
#endif
    public string bannerAdUnitId; // banner ad id -> Retrieve the ID from your account

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd")]
#endif
    public BannerPos bannerPosition; // position of banner

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd")]
#endif
    public Color bannerColor; // color of banner background
    #endregion


    #region INTERN AD VAR

#if ODIN_INSPECTOR
    [ShowIf("useInterAd")]
    [Header("INTERN AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
    [ShowIf("useInterAd")]
#endif
    public string internAdID; // intern ad ID

#if ODIN_INSPECTOR
    [ShowIf("useInterAd")]
#endif
    public int _internRetryAttempt; // atemp of re opening intern ads

    #endregion


    #region REWARD AD VAR

#if ODIN_INSPECTOR

    [ShowIf("userRewardAd")]
#endif
    string rewardAdUnitId = "1882960987cccae9";

    int _rewardRetryAttemp;

    #endregion


    void Awake()
    {
        //initiate all necceassary SDK
        IDInitiation();
    }

    // Start is called before the first frame update
    void Start()
    {
        //event when sdk of applovin have been initialized then it -
        //will do all task that given below
        MaxSdkCallbacks.OnSdkInitializedEvent += SDKConfig =>
        {
            #region LOAD AD ON START - DEBUG

            //initiate banner ad 
            BannerAdCall();
            InterstitialCall();
            #endregion

            #region INTERN AD EVENT SETUP

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            #endregion

            #region REWARD AD

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            #endregion

            #region EVENT DISPATCHER SETUP

            //Register call
            EventDispatcherExtension.RegisterListener(EventID.Ad_Init, (n) => IDInitiation());
            EventDispatcherExtension.RegisterListener(EventID.Ad_BannerCall, (n) => BannerAdCall());
            EventDispatcherExtension.RegisterListener(EventID.Ad_InternCall, (n) => InterstitialCall());

            #endregion

        };

    }

    #region MAIN

    //======================================= GENERAL INITIATION =======================================

    /// <summary>
    /// function to initialize all neccessary SDK
    /// </summary>
    private void IDInitiation()
    {
        //set sdk key as mandatory procedure 
        MaxSdk.SetSdkKey(SDKKey);
        //set user id to get player info
        MaxSdk.SetUserId(UserID);
        //initiate applovin sdk
        MaxSdk.InitializeSdk();
    }

    //======================================= GENERAL INITIATION =======================================



    //======================================= BANNER INITIATION ==================================

    /// <summary>
    /// function to initate banner
    /// </summary>
    private void BannerAdCall()
    {
        //store given position that dev selected
        MaxSdkBase.BannerPosition _pos = (MaxSdkBase.BannerPosition)bannerPosition;

        // AppLovin SDK is initialized, start loading ads
        // Banners are automatically sized to 320Ũ50 on phones and 728Ũ90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, _pos);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, bannerColor);

        //shower banner on screen
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    //======================================= BANNER INITIATION ==================================


    //======================================= INTERN INITIATION ==================================

    public void InterstitialCall()
    {
        MaxSdk.LoadInterstitial(internAdID);
    }

    //************************* INTERNAL EVENTS *************************

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        MaxSdk.ShowInterstitial(adUnitId);
        // Reset retry attempt
        _internRetryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        _internRetryAttempt++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, _internRetryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        InterstitialCall();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.

    }

    //************************* INTERNAL EVENTS *************************

    //======================================= INTERN INITIATION ==================================


    //======================================= REWARD INITIATION ==================================

    public void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        MaxSdk.ShowRewardedAd(adUnitId);
        // Reset retry attempt
        _rewardRetryAttemp = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        _rewardRetryAttemp++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, _rewardRetryAttemp));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad

    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.

    }

    //======================================= REWARD INITIATION ==================================
    #endregion

}


#region HELPER

//================================ BANNER =====================================

public enum BannerPos
{
    TopLeft,
    TopCenter,
    TopRight,
    Centered,
    CenterLeft,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}

//================================ BANNER =====================================

#endregion
