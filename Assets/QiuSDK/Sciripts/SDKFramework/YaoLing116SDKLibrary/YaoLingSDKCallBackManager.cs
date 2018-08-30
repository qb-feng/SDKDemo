using UnityEngine;
using System.Collections;

/// <summary>
/// 2018年8月17日14:32:58 添加曜灵 116 聚合 SDK 回调接口
/// </summary>
public class YaoLingSDKCallBackManager : MonoSingleton<YaoLingSDKCallBackManager>
{
    /// <summary>
    /// 登录回调
    /// </summary>
    public System.Action<YX116UserInfoModel> onSDKLoginComplete = null;
    /// <summary>
    /// 支付回调
    /// </summary>
    public System.Action<bool> onSDKPayComplete = null;

    #region 私有数据
    private static AndroidJavaObject andJO = null;
    private static AndroidJavaObject AndJO
    {
        get
        {
            if (andJO == null)
            {
                Debug.LogWarning("AndroidJavaObject 初始化！");
                using (AndroidJavaClass jc = new AndroidJavaClass("com.yyty.hdtt.yaoling.MainActivity"))
                {
                    Debug.LogWarning("AndroidJavaObject 初始化22222！");
                    andJO = jc.CallStatic<AndroidJavaObject>("GetInstance");
                }
            }
            return andJO;
        }
    }
    #endregion

    #region 回调方法
    public void LoginCallBack(string arg)
    {
        Debug.LogWarning("登入回调参数：" + arg);
        if (string.IsNullOrEmpty(arg))
        {
            onSDKLoginComplete(null);
            onSDKLoginComplete = null;
        }
        else
        {
            onSDKLoginComplete(LitJson.JsonMapper.ToObject<YX116UserInfoModel>(arg));
        }
    }

    public void SaveInfoCallBack(string arg)
    {

    }

    public void CheckUpdateCallBack(string arg)
    {

    }

    /// <summary>
    /// 支付回调  1 成功  0 取消或失败
    /// </summary>
    /// <param name="arg"></param>
    public void PayResultCallBack(string arg)
    {
        Debug.LogWarning("支付回调参数：" + arg);
        if (string.IsNullOrEmpty(arg))
        {
            onSDKPayComplete(false);
            onSDKPayComplete = null;
        }
        else
        {
            onSDKPayComplete(arg.Equals("1"));
        }
    }

    public void InitCallBack(string arg)
    {

    }

    public void ExitGameCallBack(string arg)
    {

    }

    public void PayCreateCallBack(string arg)
    {

    }

    public void LogoutCallBack(string arg)
    {

    }


    #endregion

    #region 公有方法（调安卓的方法暂时写到这）
    public void CallAndroidFunc(YaoLinAndroidSDKNameType funcType, params object[] args)
    {
        string funcName = funcType.ToString();
        Debug.LogWarning("CallYaoLinSDK:" + funcName);
        AndJO.CallStatic(funcName, args);
        return;


        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.CallStatic(funcName, args);
        }
    }
    #endregion

    #region model

    /// <summary>
    /// 登录回调信息
    /// </summary>
    [System.Serializable]
    public class YX116UserInfoModel
    {
        public string account;
        public string userId;
        public string userName;
        public string img;
        public string tele;
        public string token;
        public string gameToken;
        public string gameId;
        public string gameName;
        public string lastLoginTime;
    }

    /// <summary>
    /// 支付参数
    /// </summary>
    [System.Serializable]
    public class YX116PayParamsModel
    {
        public string userid;
        public string username;
        /// <summary>
        /// 订单充值的金额，单位元（只接收 >=1 数值，类型为double类型2位小数，但数值必须是整数，例如 6.00）
        /// </summary>
        public double amount;
        public string orderid;
        public string gamename;
        public string rolenid;
        public string rolename;
        public string gameServerId;
        public string gameServerName;
        public string productname;
        public string productDesc;
        public string orderTime;
        public string extra;
    }

    /// <summary>
    ///保存角色信息的数据model 
    /// </summary>
    [System.Serializable]
    public class SaveRoleDataModel
    {
        public string userName;//用户名
        public long roleLevel;// 角色等级
        public long roleCTime;// 角色创建时间(单位：秒)，长�?10，获取服务器存储的时间，不可用手机本地时�?
        public string roleId;// 角色id
        public string roleName;// 角色名字
        public string zoneId;// 区服id
        public string zoneName;// 区服名字
    }


    public enum YaoLinAndroidSDKNameType
    {
        /// <summary>
        /// 登入
        /// </summary>
        StartSDKLogin = 1,
        /// <summary>
        /// 支付
        /// </summary>
        StartSDKPay = 2,
        /// <summary>
        /// 保存角色信息
        /// </summary>
        StartSDKSaveRoleInfo = 3,
        /// <summary>
        /// 账号登出
        /// </summary>
        StartSDKLogout = 4,
        /// <summary>
        /// 结束游戏
        /// </summary>
        ExitGame = 5,

    }


    #endregion
}
