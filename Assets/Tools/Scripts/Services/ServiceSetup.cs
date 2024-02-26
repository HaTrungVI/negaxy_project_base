using UnityEngine;
using AppsFlyerSDK;
using Firebase;
using Sirenix.OdinInspector;

namespace QuocAnh.SDK
{
        public class ServiceSetup : MonoBehaviour
        {
                //================================= ADS =================================

                [Header("AD")]
                public string SDKKey_AppLovin; // SDK key
                public string UserID_AppLovin; // user id
                public bool useBannerAd_AppLovin; // whether to use banner ad
                public bool useInterAd_AppLovin; // whether to use intern ad
                public bool userRewardAd_AppLovin; // whether to use reward ad

                #region BANNER AD VAR

#if ODIN_INSPECTOR
                [ShowIf("useBannerAd_AppLovin")]
                [Header("BANNER AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
                [ShowIf("useBannerAd_AppLovin")]
#endif
                public string bannerAdUnitId; // banner ad id -> Retrieve the ID from your account

#if ODIN_INSPECTOR
                [ShowIf("useBannerAd_AppLovin")]
#endif
                public BannerPos bannerPosition; // position of banner

#if ODIN_INSPECTOR
                [ShowIf("useBannerAd_AppLovin")]
#endif
                public Color bannerColor; // color of banner background
                #endregion

                #region INTERN AD VAR

#if ODIN_INSPECTOR
                [ShowIf("useInterAd_AppLovin")]
                [Header("INTERN AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
                [ShowIf("useInterAd_AppLovin")]
#endif
                public string internAdID; // intern ad ID

                #endregion

                #region REWARD AD VAR

#if ODIN_INSPECTOR
                [ShowIf("userRewardAd_AppLovin")]
                [Header("REWARD AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
                [ShowIf("userRewardAd_AppLovin")]
#endif
                public string rewardAdId;

                #endregion

                //================================= ADS =================================


                //================================= APPFLYER =================================

                [Header("APP FLYER"), Space(20)]
                public string AppID_AppFlyper;
                public string DevKey_AppFlyper;
                public string MacOSAppID_AppFlyper;

                [Header("DONT DESTROY"), Space(20)]
                public bool AD_DontDestroy;
                public bool AppFlyer_DontDestroy;
                public bool FireBase_DontDestroy;

                //================================= APPFLYER =================================
                void Awake()
                {
                        SDKInitiate();
                }

                public void SDKInitiate()
                {
                        MaxSDKInit();
                        AdFlyerInit();
                        Destroy(this.gameObject);
                }

                private void MaxSDKInit()
                {

                        //============================== ID ==================================

                        ADManager.Instance.bannerAdUnitId = bannerAdUnitId;
                        ADManager.Instance.internAdID = internAdID;
                        ADManager.Instance.rewardAdId = rewardAdId;

                        //=====================================================================


                        //============================== GENERAL ==============================

                        //set sdk key as mandatory procedure 
                        MaxSdk.SetSdkKey(SDKKey_AppLovin);
                        //set user id to get player info
                        MaxSdk.SetUserId(UserID_AppLovin);
                        //initiate applovin sdk
                        MaxSdk.InitializeSdk();

                        //=====================================================================


                        //============================== BANNER ==============================

                        if (useBannerAd_AppLovin)
                        {
                                ADManager.Instance.bannerColor = bannerColor;
                                ADManager.Instance.bannerPosition = bannerPosition;
                        }

                        //=====================================================================


                        //============================== EVENT ==============================

                        //event when sdk of applovin have been initialized then it -
                        //will do all task that given below
                        MaxSdkCallbacks.OnSdkInitializedEvent += SDKConfig =>
                        {

                                #region ID Setup

                                #endregion

                                #region LOAD AD ON START - DEBUG

                                //initiate banner ad =>>>>>> TEST >>>>>>>=
                                ADManager.Instance.BannerAdCall();
                                //InterstitialCall();
                                #endregion

                                #region INTERN AD EVENT SETUP

                                if (useInterAd_AppLovin)
                                {
                                        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += ADManager.Instance.OnInterstitialLoadedEvent;
                                        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += ADManager.Instance.OnInterstitialLoadFailedEvent;
                                        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += ADManager.Instance.OnInterstitialDisplayedEvent;
                                        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += ADManager.Instance.OnInterstitialClickedEvent;
                                        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += ADManager.Instance.OnInterstitialHiddenEvent;
                                        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += ADManager.Instance.OnInterstitialAdFailedToDisplayEvent;
                                }


                                #endregion

                                #region REWARD AD

                                if (userRewardAd_AppLovin)
                                {
                                        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += ADManager.Instance.OnRewardedAdLoadedEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += ADManager.Instance.OnRewardedAdLoadFailedEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += ADManager.Instance.OnRewardedAdDisplayedEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += ADManager.Instance.OnRewardedAdClickedEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += ADManager.Instance.OnRewardedAdRevenuePaidEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += ADManager.Instance.OnRewardedAdHiddenEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += ADManager.Instance.OnRewardedAdFailedToDisplayEvent;
                                        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += ADManager.Instance.OnRewardedAdReceivedRewardEvent;
                                }


                                #endregion

                                #region EVENT DISPATCHER SETUP

                                //Register call
                                //EventDispatcherExtension.RegisterListener(EventID.Ad_Init, (n) => ADManager.Instance.IDInitiation());
                                if (useBannerAd_AppLovin) { EventDispatcherExtension.RegisterListener(EventID.Ad_BannerCall, (n) => ADManager.Instance.BannerAdCall()); }
                                if (useInterAd_AppLovin) { EventDispatcherExtension.RegisterListener(EventID.Ad_InternCall, (n) => ADManager.Instance.InterstitialCall()); }
                                if (userRewardAd_AppLovin) { EventDispatcherExtension.RegisterListener(EventID.Ad_RewardCall, (n) => ADManager.Instance.RewardCall()); }


                                #endregion

                        };

                        //=====================================================================

                }


                private void AdFlyerInit()
                {


                        if (DevKey_AppFlyper != string.Empty && AppID_AppFlyper != string.Empty)
                        {
                                AppsFlyer.initSDK(DevKey_AppFlyper, AppID_AppFlyper);
                                AppsFlyer.startSDK();
                        }

                        if (AD_DontDestroy) DontDestroyOnLoad(ADManager.Instance);

                }
        }

}
