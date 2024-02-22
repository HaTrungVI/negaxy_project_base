using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceTest : MonoBehaviour
{
    public void Reward_Debug()
    {
        ADManager.Instance.rewardAction = () => { Debug.Log("FUCK U"); };
        EventDispatcherExtension.FireEvent(EventID.Ad_RewardCall);
    }
}
