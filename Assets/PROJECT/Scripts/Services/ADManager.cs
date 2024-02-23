using System;
using System.Collections;
using System.Collections.Generic;
using QuocAnh.pattern;
using Sirenix.OdinInspector;
using UnityEngine;

public class ADManager : Singleton<ADManager>
{

    #region BANNER AD VAR

    public string bannerAdUnitId; // banner ad id -> Retrieve the ID from your account
    public BannerPos bannerPosition; // position of banner
    public Color bannerColor; // color of banner background

    #endregion

    #region INTERN AD VAR
    public string internAdID; // intern ad ID
    private int _internRetryAttempt; // atemp of re opening intern ads
    #endregion

    #region REWARD AD VAR
    public string rewardAdId;
    public Action rewardAction;
    private int _rewardRetryAttemp;
    #endregion

    public override void Awake()
    {
        base.Awake();
    }

    #region MAIN

    //======================================= BANNER INITIATION ==================================

    /// <summary>
    /// function to initate banner
    /// </summary>
    public void BannerAdCall()
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

    public void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        MaxSdk.ShowInterstitial(adUnitId);
        // Reset retry attempt
        _internRetryAttempt = 0;
    }

    public void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        _internRetryAttempt++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, _internRetryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    public void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        InterstitialCall();
    }

    public void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.

    }

    //************************* INTERNAL EVENTS *************************

    //======================================= INTERN INITIATION ==================================


    //======================================= REWARD INITIATION ==================================

    public void RewardCall()
    {
        MaxSdk.LoadRewardedAd(rewardAdId);
    }

    public void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //check if ad ready
        bool _adReady = MaxSdk.IsRewardedAdReady(adUnitId);
        //if it not then dont execute rest
        if (_adReady == false) return;
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        MaxSdk.ShowRewardedAd(adUnitId);
        // Reset retry attempt
        _rewardRetryAttemp = 0;

    }

    public void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        _rewardRetryAttemp++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, _rewardRetryAttemp));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    public void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        RewardCall();
    }

    public void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad

    }

    public void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.

        //if action for reward does exist give player reward
        if (rewardAction != null) rewardAction();
    }

    public void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
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
