using UnityEngine;
using System.Collections;

/// <summary>
/// 2018年8月17日14:32:58 添加曜灵 116 聚合 SDK 回调接口
/// </summary>
public class YaoLingSDKCallBackManager : MonoBehaviour
{
    #region 单例
    private static YaoLingSDKCallBackManager _instance;
    public static YaoLingSDKCallBackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (YaoLingSDKCallBackManager)FindObjectOfType(typeof(YaoLingSDKCallBackManager));
                if (_instance == null)
                {
                    GameObject go = new GameObject("YaoLingSDKCallBackManager");
                    _instance = go.AddComponent<YaoLingSDKCallBackManager>();

                    DontDestroyOnLoad(_instance);

                    var singletonRootGo = U3DTypeSDK.Instance;
                    if (singletonRootGo != null)
                    {
                        go.transform.SetParent(singletonRootGo.transform);
                    }
                }
            }
            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// 登录回调
    /// </summary>
    public System.Action<YX116UserInfoModel> onSDKLoginComplete = null;
    /// <summary>
    /// 支付回调
    /// </summary>
    public System.Action<bool> onSDKPayComplete = null;
    /// <summary>
    /// 账号注销回调
    /// </summary>
    public System.Action<bool> onSDKLogoutComplete = null;

    #region 私有数据
    private static AndroidJavaClass andJC = null;
    private static AndroidJavaClass AndJC
    {
        get
        {
            if (andJC == null)
            {
                SDKLogManager.DebugLog("AndroidJavaObject 初始化!");
                andJC = new AndroidJavaClass("com.yyty.fsld.xzjh.UnitySDKManager");
            }
            return andJC;
        }
    }
    #endregion

    #region 安卓回调方法
    public void LoginCallBack(string arg)
    {
        SDKLogManager.DebugLog("登入回调参数：" + arg);
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
        SDKLogManager.DebugLog("支付回调参数：" + arg);
        if (onSDKPayComplete != null)
        {
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
        SDKLogManager.DebugLog("收到账号注销结果：" + arg);
        bool result = arg == "SUCCESS";
        if (onSDKLogoutComplete != null)
        {
            onSDKLogoutComplete(result);
        }
    }


    #endregion

    #region ios回调方法
    public void IOSLoginCallBack(string arg)
    {
        SDKLogManager.DebugLog("登入回调参数：" + arg);
        if (string.IsNullOrEmpty(arg))
        {
            onSDKLoginComplete(null);
            onSDKLoginComplete = null;
        }
        else
        {
            int index = arg.IndexOf("|", 0);
            YX116UserInfoModel model = new YX116UserInfoModel()
            {
                userName = arg.Substring(0, index),
                token = arg.Substring(index + 1),
            };
            onSDKLoginComplete(model);
        }
    }


    /// <summary>
    /// 支付回调  1 成功  0 取消或失败
    /// </summary>
    /// <param name="arg"></param>
    public void IOSPayResultCallBack(string arg)
    {
        SDKLogManager.DebugLog("支付回调参数：" + arg);
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

    public void IOSInitCallBack(string arg)
    {

    }

    public void IOSExitGameCallBack(string arg)
    {

    }

    public void IOSLogoutCallBack(string arg)
    {
        SDKLogManager.DebugLog("unity: 收到账号注销结果：" + arg);
        bool result = arg == "SUCCESS";
        if (onSDKLogoutComplete != null)
        {
            onSDKLogoutComplete(result);
        }
    }


    #endregion

    #region 公有方法（调安卓的方法暂时写到这）
    public void CallAndroidFunc(YaoLinAndroidSDKNameType funcType, params object[] args)
    {
        try
        {
            string funcName = funcType.ToString();
            SDKLogManager.DebugLog("CallYaoLinSDK:" + funcName);

#if UNITY_ANDROID && !UNITY_EDITOR
            AndJC.CallStatic(funcName, args);
#endif

        }
        catch (System.Exception e)
        {
            SDKLogManager.DebugLog(e.Message, SDKLogManager.DebugType.LogError);
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

        public string productId;//商品id
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
