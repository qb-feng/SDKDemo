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


public class _0b500173881b2e6cf2751aaaf5dfeab1 
{
    int _0b500173881b2e6cf2751aaaf5dfeab1m2(int _0b500173881b2e6cf2751aaaf5dfeab1a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _0b500173881b2e6cf2751aaaf5dfeab1a * _0b500173881b2e6cf2751aaaf5dfeab1a);
    }

    public int _0b500173881b2e6cf2751aaaf5dfeab1m(int _0b500173881b2e6cf2751aaaf5dfeab1a,int _0b500173881b2e6cf2751aaaf5dfeab131,int _0b500173881b2e6cf2751aaaf5dfeab1c = 0) 
    {
        int t_0b500173881b2e6cf2751aaaf5dfeab1ap = _0b500173881b2e6cf2751aaaf5dfeab1a * _0b500173881b2e6cf2751aaaf5dfeab131;
        if (_0b500173881b2e6cf2751aaaf5dfeab1c != 0 && t_0b500173881b2e6cf2751aaaf5dfeab1ap > _0b500173881b2e6cf2751aaaf5dfeab1c)
        {
            t_0b500173881b2e6cf2751aaaf5dfeab1ap = t_0b500173881b2e6cf2751aaaf5dfeab1ap / _0b500173881b2e6cf2751aaaf5dfeab1c;
        }
        else
        {
            t_0b500173881b2e6cf2751aaaf5dfeab1ap -= _0b500173881b2e6cf2751aaaf5dfeab1c;
        }

        return _0b500173881b2e6cf2751aaaf5dfeab1m2(t_0b500173881b2e6cf2751aaaf5dfeab1ap);
    }
}
