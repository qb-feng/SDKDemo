using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Xml;

public class U3DTypeSDK : MonoBehaviour
{

    private static U3DTypeSDK _instance;


    #region 公有数据

    #region 游戏内部的数据
    /// <summary>
    /// 渠道名字
    /// </summary>
    public string ChannelName { get; private set; }

    /// <summary>
    /// 当前用户的游戏内部id（服务器内部的用户id (用户名)）
    /// </summary>
    public string Game_UserId { get; private set; }

    /// <summary>
    /// 当前用户的登陆验证id
    /// </summary>
    public string LoginSSoid { get; private set; }
    #endregion

    #region 第三方sdk的数据
    /// <summary>
    /// 当前用户的sdk方用户名
    /// </summary>
    public string SDK_UserName { get; private set; }
    /// <summary>
    /// 用户唯一标识	string	对应渠道的用户ID       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    public string SDK_Id { get; private set; }
    /// <summary>
    /// 用户登录会话标识 本次登录标识。并非必传，未作说明的情况下传空字符串
    /// </summary>
    public string SDK_Token { get; private set; }

    /// <summary>
    /// 用户在渠道的昵称	string	对应渠道的用户昵称       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    public string SDK_Nick { get; private set; }
    #endregion

    /// <summary>
    /// 更新玩家信息的时机
    /// </summary>
    public enum UpdatePlayerInfoType
    {
        /// <summary>
        /// 创建角色时机
        /// </summary>
        createRole = 1,
        /// <summary>
        /// 角色升级时机
        /// </summary>
        levelUp = 2,
        /// <summary>
        /// 选定角色进入游戏，不能为空字符串
        /// </summary>
        enterGame = 3,
    }

    /// <summary>
    /// 重置登录数据
    /// </summary>
    public void RefreshLoginData()
    {
        SDK_Token = null;
        SDK_Id = null;
        SDK_Nick = null;
        Game_UserId = null;
        LoginSSoid = null;
    }

    #endregion

    //获得sdk 实例U3DTypeSDK
    public static U3DTypeSDK Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (U3DTypeSDK)FindObjectOfType(typeof(U3DTypeSDK));
                if (_instance == null)
                {
                    GameObject go = new GameObject("TypeSDK");
                    _instance = go.AddComponent<U3DTypeSDK>();

                    DontDestroyOnLoad(_instance);

                    var singletonRootGo = GameObject.Find("UI Root (2D)");
                    if (singletonRootGo != null)
                    {
                        go.transform.SetParent(singletonRootGo.transform);
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 初始化(包含友盟初始化) - 参数：成功回调  注销登入回调
    /// </summary>
    public void InitSDK(Action onComplete, Action<bool> onSDKLogoutComplete)
    {
        gameObject.name = "TypeSDK";
        ChannelName = N3DClient.GameConfig.GetClientConfig("DitchID", "dev");
        YaoLingSDKCallBackManager.Instance.onSDKLogoutComplete = onSDKLogoutComplete;

#if UNITY_ANDROID
        #region 2018年8月17日14:18:09 qiubin添加 曜灵 116 聚合 SDK
        if (onComplete != null)
            onComplete();
        #endregion
#elif UNITY_IOS
        SDKIOSFunction.sdkmanagerinit();
        if (onComplete != null)
            onComplete();
#endif
        U3DTypeSDK.DebugLog("sdk 初始化成功！！！！当前得到的渠道id为：" + ChannelName, U3DTypeSDK.DebugType.LogWarning);
    }

    //显示登录平台的方法
    public void Login(Action onComplete)
    {
        //重置数据
        RefreshLoginData();
#if UNITY_ANDROID
        #region 2018年8月17日14:18:09 qiubin添加 曜灵 116 聚合 SDK
        //监听回调

        YaoLingSDKCallBackManager.Instance.onSDKLoginComplete = (evtData) =>
        {
            if (evtData == null)
            {
                U3DTypeSDK.DebugLog("登陆失败 !", U3DTypeSDK.DebugType.LogError);
            }
            else
            {
                U3DTypeSDK.DebugLog("登陆回调参数如下：token:" + evtData.token + "username:" + evtData.userName, U3DTypeSDK.DebugType.LogWarning);
                Game_UserId = ChannelName + evtData.userName;//暂时放在userid传给服务器验证
                SDK_UserName = evtData.userName;//真实放置地方
                SDK_Id = evtData.userId;//此参数没有传，暂时用用户名（跟id一样）
                SDK_Token = evtData.token;
                onComplete();
            }
        };

        YaoLingSDKCallBackManager.Instance.CallAndroidFunc(YaoLingSDKCallBackManager.YaoLinAndroidSDKNameType.StartSDKLogin);

        #endregion

#elif UNITY_IOS
        YaoLingSDKCallBackManager.Instance.onSDKLoginComplete = (evtData) =>
        {
            if (evtData == null)
            {
                U3DTypeSDK.DebugLog("登陆失败 !", U3DTypeSDK.DebugType.LogError);
            }
            else
            {
                U3DTypeSDK.DebugLog("登陆回调参数如下：token:" + evtData.token + "username:" + evtData.userName, U3DTypeSDK.DebugType.LogWarning);
                Game_UserId = ChannelName + evtData.userName;//暂时放在userid传给服务器验证
                SDK_UserName = evtData.userName;//真实放置地方
                SDK_Id = evtData.userName;//evtData.userId;//blizmax
                SDK_Token = evtData.token;
                onComplete();
            }
        };
        SDKIOSFunction.sdkmanagerlogin();
#endif

#if UNITY_EDITOR
        YaoLingSDKCallBackManager.Instance.onSDKLoginComplete(new YaoLingSDKCallBackManager.YX116UserInfoModel()
        {
            token = "default_token",
            userName = "default_username",
            userId = "default_userid",
        });
#endif
    }

    //登出平台
    public void Logout()
    {
        //注销回调初始化时候已经监听好了
#if UNITY_ANDROID
        YaoLingSDKCallBackManager.Instance.CallAndroidFunc(YaoLingSDKCallBackManager.YaoLinAndroidSDKNameType.StartSDKLogout);
#elif UNITY_IOS
        SDKIOSFunction.sdkmanagerlogout();
#endif

#if UNITY_EDITOR
        YaoLingSDKCallBackManager.Instance.onSDKLogoutComplete(true);
#endif
    }


    public string PayItem(SDKData.PayOrderData orderData)
    {
        if (string.IsNullOrEmpty(orderData.orderTime))
            orderData.orderTime = System.DateTime.Now.Ticks.ToString();

        var payModel = new YaoLingSDKCallBackManager.YX116PayParamsModel()
        {
            userid = orderData.userid,
            username = SDK_UserName,//只能填sdk的用户名
            amount = double.Parse(string.Format("{0:F2}", orderData.amount)),
            orderid = orderData.orderId,
            rolenid = orderData.roleID.ToString(),
            rolename = orderData.roleName,
            gameServerId = orderData.zoneID.ToString(),
            gameServerName = orderData.zoneName,
            productname = orderData.productName,
            productDesc = orderData.productDesc,
            orderTime = orderData.orderTime,//116 22222 22222  => 22222 22222
            extra = orderData.extra,

            productId = orderData.productId,
            gamename = orderData.gamename,
        };
        #region 2018年8月17日14:18:09 qiubin添加 曜灵 116 聚合 SDK
#if UNITY_ANDROID
        YaoLingSDKCallBackManager.Instance.CallAndroidFunc(YaoLingSDKCallBackManager.YaoLinAndroidSDKNameType.StartSDKPay, LitJson.JsonMapper.ToJson(payModel));
#elif UNITY_IOS
        SDKIOSFunction.sdkmanagerpayorder(payModel.orderid, payModel.rolename, payModel.gameServerId, payModel.amount.ToString()
            , payModel.productId, payModel.productname, payModel.extra, payModel.gamename, payModel.gameServerName, "1");
#endif
        #endregion

        return null;
    }
    /// <summary>
    /// 上传玩家信息到sdk服务器  参数1：玩家参数 参数2：上传时机
    /// </summary>
    public void UpdatePlayerInfo(SDKData.RoleData roleData, UpdatePlayerInfoType updateType = UpdatePlayerInfoType.createRole)
    {
        long roleCTime;
        if (!long.TryParse(roleData.createTime, out roleCTime))
        {
            roleCTime = 0;
        }
        var roleModel = new YaoLingSDKCallBackManager.SaveRoleDataModel()
      {
          userName = SDK_UserName,
          roleLevel = long.Parse(roleData.roleLevel),
          roleCTime = roleCTime,
          roleId = roleData.roleId,
          roleName = roleData.roleName,
          zoneId = roleData.realmId,
          zoneName = roleData.realmName,
      };

        #region 2018年8月17日14:18:09 qiubin添加 曜灵 116 聚合 SDK
#if UNITY_ANDROID
        YaoLingSDKCallBackManager.Instance.CallAndroidFunc(YaoLingSDKCallBackManager.YaoLinAndroidSDKNameType.StartSDKSaveRoleInfo, LitJson.JsonMapper.ToJson(roleModel));
#elif UNITY_IOS
        SDKIOSFunction.sdkmanagersavedata(roleModel.roleId, roleModel.roleName, roleModel.zoneId, roleModel.roleLevel.ToString(), roleModel.zoneName);
#endif
        #endregion

    }
    public void ExitGame()
    {
#if UNITY_ANDROID
        YaoLingSDKCallBackManager.Instance.CallAndroidFunc(YaoLingSDKCallBackManager.YaoLinAndroidSDKNameType.ExitGame);
#elif UNITY_IOS
        Debug.LogWarning("sdk 没有退出游戏的方法！！！直接退出");
        Application.Quit();
#endif

#if UNITY_EDITOR
        U3DTypeSDK.DebugLog("game over", U3DTypeSDK.DebugType.LogWarning);
#endif
    }

    #region log 集中处理
    public static void DebugLog(string message, DebugType type = DebugType.Log)
    {
        if (N3DClient.GameConfig.LogEnable)
        {
            switch (type)
            {
                case DebugType.Log:
                    UnityEngine.Debug.Log(message);
                    break;
                case DebugType.LogWarning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case DebugType.LogError:
                    UnityEngine.Debug.LogError(message);
                    break;
            }
        }
    }

    public enum DebugType
    {
        Log = 1,
        LogWarning = 2,
        LogError = 3,
    }
    #endregion
}



