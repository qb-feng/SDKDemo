using UnityEngine;
using System.Collections;

/// <summary>
/// 回调接收物体
/// </summary>
public class PlatSDKMessageHandler : MonoSingleton<PlatSDKMessageHandler>, ISDKMessageable
{

    private PlatSDKManagerBase currentSDKManager = null;//当前sdk管理器

    /// <summary>
    /// 初始化 传入当前的sdkManager
    /// </summary>
    public void Init(PlatSDKManagerBase sdkManaager)
    {
        if (sdkManaager == null)
        {
            Debug.LogError("PlatSDKMessageHandler init error! sdkManager is error!" + sdkManaager);
            return;
        }
        currentSDKManager = sdkManaager;
    }


    #region 回調方法
    public void DebugLogCallBack(string arg)
    {
        currentSDKManager.DebugLogCallBack(arg);
    }

    public void DebugErrorCallBack(string arg)
    {
        currentSDKManager.DebugErrorCallBack(arg);
    }
    public void ContentCallBack(string arg)
    {
        currentSDKManager.ContentCallBack(arg);
    }

    public void LoginCallBack(string arg)
    {
        currentSDKManager.LoginCallBack(arg);
    }

    public void SaveInfoCallBack(string arg)
    {
        currentSDKManager.SaveInfoCallBack(arg);
    }

    public void CheckUpdateCallBack(string arg)
    {
        currentSDKManager.CheckUpdateCallBack(arg);
    }

    public void PayResultCallBack(string arg)
    {
        currentSDKManager.PayResultCallBack(arg);
    }

    public void InitCallBack(string arg)
    {
        currentSDKManager.InitCallBack(arg);
    }

    public void ExitGameCallBack(string arg)
    {
        currentSDKManager.ExitGameCallBack(arg);
    }

    public void PayCreateCallBack(string arg)
    {
        currentSDKManager.PayCreateCallBack(arg);
    }

    public void LogoutCallBack(string arg)
    {
        currentSDKManager.LogoutCallBack(arg);
    }

    public void GetTokenCallBack(string arg)
    {
        currentSDKManager.GetTokenCallBack(arg);
    }


    public void TestFunc(string arg)
    {
        Debug.LogWarning("这是一个安卓调用unity的回调方法：" + arg);
    }

    #endregion

}
