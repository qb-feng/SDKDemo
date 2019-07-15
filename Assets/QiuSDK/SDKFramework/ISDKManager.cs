using System;
using UnityEngine;


public interface ISDKManager
{

    //private static T _instance;

    ////获得sdk 实例U3DTypeSDK
    //public static T Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
    //            if (_instance == null)
    //            {
    //                GameObject go = new GameObject(typeof(T).Name);
    //                _instance = go.AddComponent<T>();
    //                UnityEngine.Object.DontDestroyOnLoad(_instance);
    //            }
    //        }
    //        return _instance;
    //    }
    //}

    #region 公有数据

    #region 游戏内部的数据
    /// <summary>
    /// 渠道名字
    /// </summary>
    string ChannelName { get; set; }

    /// <summary>
    /// 当前用户的游戏内部id（服务器内部的用户id (用户名)）
    /// </summary>
    string Game_UserId { get; set; }

    /// <summary>
    /// 当前用户的登陆验证id
    /// </summary>
    string LoginSSoid { get; set; }
    #endregion

    #region 第三方sdk的数据
    /// <summary>
    /// 当前用户的sdk方用户名
    /// </summary>
    string SDK_UserName { get; set; }
    /// <summary>
    /// 用户唯一标识	string	对应渠道的用户ID       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    string SDK_Id { get; set; }
    /// <summary>
    /// 用户登录会话标识 本次登录标识。并非必传，未作说明的情况下传空字符串
    /// </summary>
    string SDK_Token { get; set; }

    /// <summary>
    /// 用户在渠道的昵称	string	对应渠道的用户昵称       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    string SDK_Nick { get; set; }
    #endregion

    /// <summary>
    /// 重置登录数据
    /// </summary>
    void RefreshLoginData();
    //{
    //    SDK_Token = null;
    //    SDK_Id = null;
    //    SDK_Nick = null;
    //    Game_UserId = null;
    //    LoginSSoid = null;
    //}

    #endregion

    #region 私有数据
    /// <summary>
    /// 初始化回调
    /// </summary>
    Action<bool> onInitComplete { get; set; }
    /// <summary>
    /// 登录回调
    /// </summary>
    Action<bool> onLoginComplete { get; set; }
    /// <summary>
    /// 注销回调
    /// </summary>
    Action<bool> onLogoutComplete { get; set; }
    #endregion

    #region 外部调用方法
    /// <summary>
    /// 初始化(友盟初始化) - 参数：初始化回调  注销登入回调
    /// </summary>
    void InitSDK(Action<bool> onComplete, Action<bool> onSDKLogoutComplete);


    //显示登录平台的方法
    void Login(Action<bool> onComplete);


    //登出平台
    void Logout();

    /// <summary>
    /// 支付
    /// </summary>
    string PayItem(SDKData.PayOrderData orderData);

    /// <summary>
    /// 上传玩家信息到sdk服务器  参数1：玩家参数 参数2：上传时机
    /// </summary>
    void UpdatePlayerInfo(SDKData.RoleData roleData, SDKData.UpdatePlayerInfoType updateType = SDKData.UpdatePlayerInfoType.createRole);
    /// <summary>
    /// 结束游戏
    /// </summary>
    void ExitGame();

    #endregion

}



