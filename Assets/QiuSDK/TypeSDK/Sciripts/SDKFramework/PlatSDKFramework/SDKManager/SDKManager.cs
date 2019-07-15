using UnityEngine;
using System.Collections;
using System;
using SDKData;

/// <summary>
/// 整个sdk Manager
/// </summary>
public class SDKManager
{
    #region 变量
    private static SDKManager _instance = null;
    public static SDKManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SDKManager();
            return _instance;
        }
    }

    #region 公有变量
    private SDKPlatName currentSDKPlatName = SDKPlatName.None;
    /// <summary>
    /// 当前安卓平台渠道名称
    /// </summary>
    public SDKPlatName CurrentSDKPlatName
    {
        get
        {
            if (currentSDKPlatName == SDKPlatName.None)
            {
#if OPPO
                currentSDKPlatName = SDKPlatName.OPPO;
#elif HW
                currentSDKPlatName = SDKPlatName.HW;
#elif UC
                currentSDKPlatName = SDKPlatName.UC;
#elif VIVO
                currentSDKPlatName = SDKPlatName.VIVO;
#elif YYB
                currentSDKPlatName = SDKPlatName.YYB;
#endif
            }
            return currentSDKPlatName;
        }
    }
    /// <summary>
    /// SDK 登入回调参数
    /// </summary>
    public string SDKLoginArg = null;
    #endregion

    #endregion

    #region unity 内部调用sdk的方法

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        UmengManager.Instance.Init();//友盟统计初始化
#if  UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidPlatSDKManager.Instance.Init();
#elif UNITY_IPHONE

#endif
    }

    /// <summary>
    /// 登录 登录回调
    /// </summary>
    public void Login(System.Action<SDKLoginCompleteData> onComplete)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidPlatSDKManager.Instance.Login(onComplete);
#elif UNITY_IPHONE

#endif
    }

    /// <summary>
    /// 保存角色信息（每次登陆进入游戏后，角色信息变化（等级，名字等）时调用）
    /// </summary>
    public void SavePlayerInfo(SDKData.RoleData roleData)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidPlatSDKManager.Instance.SavePlayerInfo(roleData);
#elif UNITY_IPHONE

#endif
    }

    /// <summary>
    /// 申请支付订单
    /// </summary>
    public void PayOrder(SDKData.PayOrderData arg)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidPlatSDKManager.Instance.PayOrder(arg);
#elif UNITY_IPHONE

#endif
    }

    /// <summary>
    /// 退出当前账号
    /// </summary>
    public void Logout()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidPlatSDKManager.Instance.Logout();
#elif UNITY_IPHONE

#endif
    }

    /// <summary>
    /// 结束sdk（游戏退出前调用）
    /// </summary>
    public void OnGameExit(System.Action onComplete)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidPlatSDKManager.Instance.OnGameExit(onComplete);
#elif UNITY_IPHONE

#endif
    }

    #endregion

}


//namespace SDKData
//{
//    /// <summary>
//    /// 角色信息
//    /// </summary>
//    public class RoleData
//    {
//        /// <summary>
//        /// 角色id
//        /// </summary>
//        public string roleId;
//        /// <summary>
//        /// 角色名字
//        /// </summary>
//        public string roleName;
//        /// <summary>
//        /// 角色等级
//        /// </summary>
//        public string roleLevel;
//        /// <summary>
//        /// 区服id
//        /// </summary>
//        public string realmId;
//        /// <summary>
//        /// 区服名字
//        /// </summary>
//        public string realmName;
//        /// <summary>
//        /// 角色创建时间
//        /// </summary>
//        public string createTime;
//        /// <summary>
//        /// 关卡章节
//        /// </summary>
//        public string chapter;
//        /// <summary>
//        /// 其他信息 - 附加信息
//        /// </summary>
//        public string arg;

//        //游戏内部区服id，名字数据
//        //var currentServerData = HSGameEngine.GameEngine.Logic.Global.Data.lastServerData;//服务器信息
//        // var sendModel = new SaveRoleDataModel()
//        // {
//        //     roleId = arg.RoleID.ToString(),
//        //     roleLevel = arg.Level,
//        //     roleName = arg.RoleName,
//        //     realmId = currentServerData.ID.ToString(),
//        //     realmName = currentServerData.Name,
//        // }

//        //角色创建时间逻辑获取
//        // var roleCtime = System.Convert.ToDateTime(HSGameEngine.GameEngine.Logic.Global.Data.roleData.RegTime);// 2018-06-25 12:43:27
//        //long createTime = 0;
//        //if (roleCtime != null)
//        //{
//        //    string ctime = (roleCtime.Ticks / (long)10000000).ToString();
//        //    ctime = ctime.Substring(0, ctime.Length >= 10 ? 10 : ctime.Length);
//        //    createTime = long.Parse(ctime);
//        //    ctime = null;
//        //}
//    }

//    /// <summary>
//    /// 支付的数据信息  注：服务器的回调地址暂时放在各个平台的manager中设置，减少判断（若地址一致就放在model中的callbackUrl字段）
//    /// </summary>
//    public class PayOrderData
//    {
//        /// <summary>
//        /// 订单价格（暂时以元为单位）
//        /// </summary>
//        public float amount;
//        /// <summary>
//        /// 商品号
//        /// </summary>
//        public string productId;
//        /// <summary>
//        /// 商品名字
//        /// </summary>
//        public string productName;
//        /// <summary>
//        /// 商品介绍
//        /// </summary>
//        public string productDesc;
//        /// <summary>
//        /// 回调给服务器时的附加消息
//        /// </summary>
//        public string callbackMessage;

//        /// <summary>
//        /// 结果回调的服务器地址（暂时不用！）
//        /// </summary>
//        public string callbackUrl;
//    }

//    public class SDKCommon
//    {
//        /// <summary>
//        /// 获取本地的和服务器校正的时间
//        /// </summary>
//        public static DateTime GetCorrectDateTime()
//        {
//            //TODO //实际运用中改为 return HSGameEngine.GameEngine.Logic.Global.GetCorrectDateTime();
//            return System.DateTime.Now;
//        }
//    }

//    /// <summary>
//    /// 登陆回调数据
//    /// </summary>
//    public class SDKLoginCompleteData
//    {
//        /// <summary>
//        /// 登陆结果
//        /// </summary>
//        public bool result;

//        /// <summary>
//        /// 回调参数数据
//        /// </summary>
//        public string arg;

//    }
//}