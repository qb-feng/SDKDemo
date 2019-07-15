using UnityEngine;
using System.Collections;
using LitJson;
using SDKData;

/// <summary>
/// sdkManager抽象类
/// </summary>
public abstract class PlatSDKManagerBase : ISDKMessageable
{
    #region unity調用方法
    /// <summary>
    /// 初始化方法
    /// </summary>
    public abstract void Init(string arg);

    /// <summary>
    /// //检查自更新方法
    /// </summary>
    public virtual void CheckUpdate() { }

    /// <summary>
    /// 登入
    /// </summary>
    public abstract void Login();

    /// <summary>
    /// 保存玩家的角色信息
    /// </summary>
    public abstract void SavePlayerInfo(RoleData arg);

    /// <summary>
    /// 支付订单
    /// </summary>
    public abstract void PayOrder(PayOrderData arg);

    /// <summary>
    /// 退出账号
    /// </summary>
    public virtual void Logout() { }

    /// <summary>
    /// 游戏结束前的清理工作
    /// </summary>
    public abstract void OnGameExit();
    #endregion

    #region sdk回調接口
    public void DebugLogCallBack(string arg)
    {
        DebugLog(arg);
    }

    public void DebugErrorCallBack(string arg)
    {
        DebugLog(arg);
    }

    public virtual void ContentCallBack(string arg)
    {

    }

    public virtual void LoginCallBack(string arg)
    {

    }

    public virtual void SaveInfoCallBack(string arg)
    {

    }

    public virtual void CheckUpdateCallBack(string arg)
    {

    }

    public virtual void PayResultCallBack(string arg)
    {

    }

    public virtual void InitCallBack(string arg)
    {

    }

    public virtual void ExitGameCallBack(string arg)
    {

    }

    public virtual void PayCreateCallBack(string arg)
    {

    }

    public virtual void LogoutCallBack(string arg)
    {

    }

    public virtual void GetTokenCallBack(string arg)
    {

    }
    #endregion

    #region 公有方法
    protected void DebugLog(string arg)
    {
        AndroidPlatSDKManager.Instance.DebugLog(arg);
    }
    /// <summary>
    /// 检测回调方法的参数是否正确
    /// </summary>
    protected T CheckCallBackArg<T>(string arg)
    {
        if (string.IsNullOrEmpty(arg))
        {
            DebugLog(typeof(T).ToString() + "  CallBack: arg is null !");
            return default(T);
        }
        var model = JsonMapper.ToObject<T>(arg);
        if (model == null)
        {
            DebugLog(typeof(T).ToString() + " CallBack:" + "参数model转换为null ！");
        }
        return model;
    }

    /// <summary>
    /// 获取当前时间长度为10的时间戳
    /// </summary>
    protected string GetCurrentTimeMiss()
    {
        var currentTime = SDKCommon.GetCorrectDateTime();
        var dateStart = new System.DateTime(1970, 1, 1, 8, 0, 0);
        long timeStamp = System.Convert.ToInt32((System.DateTime.Now - dateStart).TotalSeconds);
        return timeStamp.ToString();
    }

    /// <summary>
    /// 调用安卓方法 方法名 参数
    /// </summary>
    protected void CallAndoridFunc(string funcName, params object[] arg)
    {
        AndroidPlatSDKManager.Instance.CallAndroidFunction(funcName, arg);

    }

    #endregion
}
