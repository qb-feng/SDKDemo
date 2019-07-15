using UnityEngine;
using System.Collections;

/// <summary>
/// sdk回调信息接口
/// </summary>
public interface ISDKMessageable
{
    #region 共有回调
    void DebugLogCallBack(string arg);
    void DebugErrorCallBack(string arg);
    #endregion

    #region 各自实现的回调
    void ContentCallBack(string arg);
    void LoginCallBack(string arg);
    void SaveInfoCallBack(string arg);
    void CheckUpdateCallBack(string arg);
    void PayResultCallBack(string arg);//订单支付结果的回调
    void InitCallBack(string arg);
    void ExitGameCallBack(string arg);
    void PayCreateCallBack(string arg);//订单创建后的回调
    void LogoutCallBack(string arg);
    void GetTokenCallBack(string arg);
    #endregion
}
