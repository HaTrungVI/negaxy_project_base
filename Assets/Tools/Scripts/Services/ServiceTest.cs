using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ServiceTest : MonoBehaviour
{
    void Start()
    {
        ADManager.Instance.rewardAction = () => { Debug.Log("FUCK U"); };
    }

    public void Reward_Debug()
    {
        EventDispatcherExtension.FireEvent(EventID.Ad_RewardCall);
    }

    public void ChangeScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }
}
