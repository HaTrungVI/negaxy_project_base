using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using QuocAnh.SDK;

public class ServiceEditor : OdinEditorWindow
{

    [MenuItem("Negaxy/Service")]
    public static void ShowWindow()
    {
        GetWindow<ServiceEditor>();
    }

    //================================= ADS VAR =================================
    [Title("APPLOVIN SDK", titleAlignment: TitleAlignments.Centered)]

    [FoldoutGroup("APPLOVIN")]
    public string SDKKey_AppLovin; // SDK key
    [FoldoutGroup("APPLOVIN")]
    public string UserID_AppLovin; // user id
    [FoldoutGroup("APPLOVIN")]
    public bool useBannerAd_AppLovin; // whether to use banner ad
    [FoldoutGroup("APPLOVIN")]
    public bool useInterAd_AppLovin; // whether to use intern ad
    [FoldoutGroup("APPLOVIN")]
    public bool userRewardAd_AppLovin; // whether to use reward ad

    #region BANNER AD VAR

#if ODIN_INSPECTOR
    [FoldoutGroup("APPLOVIN")]
    [ShowIf("useBannerAd_AppLovin")]
    [Header("BANNER AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd_AppLovin")]
#endif
    [FoldoutGroup("APPLOVIN")]
    public string bannerAdUnitId; // banner ad id -> Retrieve the ID from your account

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd_AppLovin")]
#endif
    [FoldoutGroup("APPLOVIN")]
    public BannerPos bannerPosition; // position of banner

#if ODIN_INSPECTOR
    [ShowIf("useBannerAd_AppLovin")]
#endif
    [FoldoutGroup("APPLOVIN")]
    public Color bannerColor; // color of banner background
    #endregion

    #region INTERN AD VAR

#if ODIN_INSPECTOR
    [FoldoutGroup("APPLOVIN")]
    [ShowIf("useInterAd_AppLovin")]
    [Header("INTERN AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
    [ShowIf("useInterAd_AppLovin")]
#endif
    [FoldoutGroup("APPLOVIN")]
    public string internAdID; // intern ad ID

    #endregion

    #region REWARD AD VAR

#if ODIN_INSPECTOR
    [FoldoutGroup("APPLOVIN")]
    [ShowIf("userRewardAd_AppLovin")]
    [Header("REWARD AD"), Space(10)]
#endif

#if ODIN_INSPECTOR
    [ShowIf("userRewardAd_AppLovin")]
#endif
    [FoldoutGroup("APPLOVIN")]
    public string rewardAdId;

    #endregion

    //================================= ADS VAR =================================


    //================================= APPFLYER VAR =================================
    [Title("APPFLYER SDK", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("APPFLYER")]
    public string AppID_AppFlyper;
    [FoldoutGroup("APPFLYER")]
    public string DevKey_AppFlyper;
    [FoldoutGroup("APPFLYER")]
    public string MacOSAppID_AppFlyper;


    [Title("DONT DESTROY ON LOAD", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("DONTDESTROYONLOAD")]
    public bool AD_DontDestroy = true;
    [FoldoutGroup("DONTDESTROYONLOAD")]
    public bool AppFlyer_DontDestroy = true;
    [FoldoutGroup("DONTDESTROYONLOAD")]
    public bool FireBase_DontDestroy = true;

    //================================= APPFLYER VAR =================================
    [Title("EXECUTE", titleAlignment: TitleAlignments.Centered)]
    [Button(ButtonSizes.Large)]
    public void SETUP()
    {
        //declare array to find existed SDk in scene to destroy
        ServiceSetup[] _existedSDK = FindObjectsOfType<ServiceSetup>();
        //if there is already an existed sdk
        if (_existedSDK.Length > 0)
        {
            //loop all of it
            for (int i = 0; i < _existedSDK.Length; i++)
            {
                //destroy all of it
                DestroyImmediate(_existedSDK[i].gameObject);
            }
        }

        //create new object with service setup class
        ServiceSetup _SDK = new GameObject("SDK").AddComponent<ServiceSetup>();

        //============= ADS ==================

        //************** ID **************
        _SDK.SDKKey_AppLovin = SDKKey_AppLovin;
        _SDK.UserID_AppLovin = UserID_AppLovin;
        _SDK.useBannerAd_AppLovin = useBannerAd_AppLovin;
        _SDK.useInterAd_AppLovin = useInterAd_AppLovin;
        _SDK.userRewardAd_AppLovin = userRewardAd_AppLovin;
        //************** ID **************

        //************** BANNER PROPERTIES **************
        _SDK.bannerAdUnitId = bannerAdUnitId;
        _SDK.bannerPosition = bannerPosition;
        _SDK.bannerColor = bannerColor;
        //************** BANNER PROPERTIES **************

        //************** INTERN PROPERTIES **************
        _SDK.internAdID = internAdID;
        //************** INTERN PROPERTIES **************

        //************** REWARD PROPERTIES **************
        _SDK.rewardAdId = rewardAdId;
        //************** REWARD PROPERTIES **************

        //============= ADS ==================


        //============= APPLOVIN ==================
        _SDK.AppID_AppFlyper = AppID_AppFlyper;
        _SDK.DevKey_AppFlyper = DevKey_AppFlyper;
        _SDK.MacOSAppID_AppFlyper = MacOSAppID_AppFlyper;
        //============= APPLOVIN ==================


        //============= DONTDESTROY ==================\
        _SDK.AD_DontDestroy = AD_DontDestroy;
        _SDK.AppFlyer_DontDestroy = AppFlyer_DontDestroy;
        _SDK.FireBase_DontDestroy = FireBase_DontDestroy;
        //============= DONTDESTROY ==================

    }

}
