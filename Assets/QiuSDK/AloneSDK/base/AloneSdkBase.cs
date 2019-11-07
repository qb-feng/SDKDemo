using UnityEngine;
using System.Collections;
using UnityEngine;
using System;
using SDKData;

namespace AloneSdk
{
    /// <summary>
    /// 接单独渠道用的sdk管理器，如yyb，vc，oppo等
    /// </summary>
    public abstract class AloneSdkBase<T> : MonoBehaviour, ISDKMessageable, ISDKManager where T : MonoBehaviour, ISDKManager
    {
        /// <summary>
        /// 获取渠道类型
        /// </summary>
        public abstract SDKPlatName getSDKPlatName();

        #region 安卓activity单列
        private static AndroidJavaClass andJC = null;
        private static AndroidJavaClass AndJC
        {
            get
            {
                if (andJC == null)
                {
                    var plat = Instance.getSDKPlatName();
                    string packName = SDKData.SDKPlatCommonData.PlatPackageData[plat];
                    andJC = new AndroidJavaClass(string.Format("{0}.UnitySDKManager", packName));
                    SDKLogManager.DebugLog("AndroidJavaObject 初始化!" + plat + "  " + packName);
                }
                return andJC;
            }
        }
        #endregion

        #region 单例
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Type type = typeof(T);
                    _instance = (T)UnityEngine.Object.FindObjectOfType(type);
                    if (_instance == null)
                    {
                        GameObject go = new GameObject(type.Name);
                        _instance = go.AddComponent<T>();
                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 公有数据

        #region 游戏内部的数据
        /// <summary>
        /// 渠道名字
        /// 2019年5月31日17:28:14 quick sdk 用到了
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 当前用户的游戏内部id（服务器内部的用户id (用户名)）
        /// </summary>
        public string Game_UserId { get; set; }

        /// <summary>
        /// 当前用户的登陆验证id
        /// </summary>
        public string LoginSSoid { get; set; }
        #endregion

        #region 第三方sdk的数据
        /// <summary>
        /// 当前用户的sdk方用户名
        /// </summary>
        public string SDK_UserName { get; set; }
        /// <summary>
        /// 用户唯一标识	string	对应渠道的用户ID       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
        /// 2019年5月31日17:28:14 quick sdk 用到了
        /// </summary>
        public string SDK_Id { get; set; }
        /// <summary>
        /// 用户登录会话标识 本次登录标识。并非必传，未作说明的情况下传空字符串
        /// </summary>
        public string SDK_Token { get; set; }

        /// <summary>
        /// 用户在渠道的昵称	string	对应渠道的用户昵称       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
        /// </summary>
        public string SDK_Nick { get; set; }
        #endregion

        #region 2019年7月29日16:09:00 缓存数据结构优化- 之后的sdk不使用上面的单个数据，数据保存在结构中，由各子类自己赋值，获取

        protected System.Collections.Generic.Dictionary<string, string> currentSDKParmer = new System.Collections.Generic.Dictionary<string, string>();

        /// <summary>
        /// 获取sdk的参数  key 由各子类自己定义
        /// </summary>
        public virtual string GetSDKParamer(string key)
        {
            try
            {
                string value = null;
                currentSDKParmer.TryGetValue(key, out value);
                if (string.IsNullOrEmpty(value))
                {
                    value = CallAndroidFuncGetResult("GetSDKParamer", key);
                }
                return value;
            }
            catch (Exception e)
            {
                DebugErrorCallBack("GetSDKParamer Error:" + e.Message + "   arg:" + key);
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 重置登录数据
        /// </summary>
        public virtual void RefreshLoginData()
        {
            SDK_Token = null;
            SDK_Id = null;
            SDK_Nick = null;
            Game_UserId = null;
            LoginSSoid = null;
            SDK_UserName = null;
            ChannelName = null;

            currentSDKParmer.Clear();
        }

        /// <summary>
        /// 设置登入数据  -登入后赋值
        /// </summary>
        private void SetLoginData(string sdk_userName, string sdk_token, string sdk_channelName, string sdk_userId)
        {
            SDK_UserName = sdk_userName;
            SDK_Token = sdk_token;
            ChannelName = sdk_channelName;
            SDK_Id = sdk_userId;
        }

        #endregion

        #region 私有数据-给游戏的回调
        /// <summary>
        /// 初始化回调
        /// </summary>
        public Action<bool> onInitComplete { get; set; }
        /// <summary>
        /// 登录回调
        /// </summary>
        public Action<bool> onLoginComplete { get; set; }
        /// <summary>
        /// 注销回调
        /// </summary>
        public Action<bool> onLogoutComplete { get; set; }


        #region 2019年7月29日16:40:30 新增回调保存结构
        public SDKData.InitArgModel SDKInitArgModel { get; set; }
        #endregion

        #endregion

        #region 外部调用方法
        /// <summary>
        /// 初始化(友盟初始化) - 参数：初始化回调  注销登入回调
        /// </summary>
        public virtual void InitSDK(SDKData.InitArgModel initArgModel)
        {
            SDKInitArgModel = initArgModel;

            onInitComplete = initArgModel.onComplete;
            onLogoutComplete = initArgModel.onSDKLogoutComplete;

            SDKLogManager.DebugLog("InitSDK");
            CallAndroidFunc(SDKData.SDKPlatCommonData.StartSDKInit);

        }

        //显示登录平台的方法
        public virtual void Login(Action<bool> onComplete, string arg = null)
        {
            this.RefreshLoginData();
            onLoginComplete = onComplete;
            SDKLogManager.DebugLog("Login");
            CallAndroidFunc(SDKData.SDKPlatCommonData.StartSDKLogin, arg);
        }

        //登出平台
        public virtual void Logout()
        {
            SDKLogManager.DebugLog("Logout");
            CallAndroidFunc(SDKData.SDKPlatCommonData.StartSDKLogout);
        }

        public virtual string PayItem(SDKData.PayOrderData orderData)
        {
            SDKLogManager.DebugLog("PayItem");
#if UNITY_EDITOR

#elif UNITY_ANDROID

#elif UNITY_PHONE

#endif
            return null;
        }
        /// <summary>
        /// 上传玩家信息到sdk服务器  参数1：玩家参数 参数2：上传时机
        /// </summary>
        public virtual void UpdatePlayerInfo(SDKData.RoleData roleData, SDKData.UpdatePlayerInfoType updateType)
        {
            SDKLogManager.DebugLog("Begin UpdatePlayerInfo");
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public virtual void ExitGame()
        {
            SDKLogManager.DebugLog("ExitGame");
            CallAndroidFunc(SDKData.SDKPlatCommonData.StartExitGame);
        }

        #endregion

        //Update is called once per frame
#if UNITY_EDITOR
        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                LogoutCallBack("SUCCESS");
            }
        }
#endif

        #region 数据获取
        /// <summary>
        /// 获取是否有sdk游戏退出框
        /// </summary>
        public virtual bool onGetChannelHasExitDialog()
        {
            SDKLogManager.DebugLog("onGetChannelHasExitDialog");
            return true;
        }


        #endregion


        #region 调用安卓ios方法
        public void CallAndroidFunc(string funcName, params object[] args)
        {
            try
            {
                SDKLogManager.DebugLog("CallAndroidFunc:" + funcName);

#if UNITY_ANDROID && !UNITY_EDITOR
            AndJC.CallStatic(funcName, args);
#endif

            }
            catch (System.Exception e)
            {
                SDKLogManager.DebugLog(e.Message, SDKLogManager.DebugType.LogError);
            }
        }

        public string CallAndroidFuncGetResult(string funcName, params object[] args)
        {
            return CallAndroidFuncGetResult<string>(funcName, args);
        }

        public T CallAndroidFuncGetResult<T>(string funcName, params object[] args)
        {
            try
            {
                SDKLogManager.DebugLog("CallAndroidFuncGetResult:" + funcName);

#if UNITY_ANDROID && !UNITY_EDITOR
                return AndJC.CallStatic<T>(funcName, args);
#endif

            }
            catch (System.Exception e)
            {
                SDKLogManager.DebugLog(e.Message, SDKLogManager.DebugType.LogError);
            }
            return default(T);
        }

        #endregion

        #region  安卓ios回调方法
        //************************************************************以下是需要实现的回调接口*************************************************************************************************************************
        //callback

        #region 共有回调
        public virtual void DebugLogCallBack(string arg) { SDKLogManager.DebugLog("javaMessage:" + arg); }
        public virtual void DebugErrorCallBack(string arg) { SDKLogManager.DebugLog("javaMessage:" + arg, SDKLogManager.DebugType.LogError); }
        #endregion

        #region 各自实现的回调  根据sdk自己选择是否需要重写
        public virtual void LoginCallBack(string arg) { }
        public virtual void ContentCallBack(string arg) { }
        public virtual void SaveInfoCallBack(string arg) { }
        public virtual void CheckUpdateCallBack(string arg) { }
        public virtual void PayResultCallBack(string arg) { }
        public virtual void InitCallBack(string arg) { }
        public virtual void ExitGameCallBack(string arg) { }
        public virtual void PayCreateCallBack(string arg) { }
        public virtual void LogoutCallBack(string arg)
        {

            bool result = arg == "SUCCESS";
            if (onLogoutComplete != null)
                onLogoutComplete(result);

            RefreshLoginData();
        }
        public virtual void GetTokenCallBack(string arg) { }

        /// <summary>
        /// 游戏提示消息回调
        /// </summary>
        public virtual void SendMessageCallBack(string arg) { }

        #endregion

        #endregion

    }

}

public class _f9522689f9d921801239f9a5a187ebb8
{
    int _f9522689f9d921801239f9a5a187ebb8m2(int _f9522689f9d921801239f9a5a187ebb8a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _f9522689f9d921801239f9a5a187ebb8a * _f9522689f9d921801239f9a5a187ebb8a);
    }

    public int _f9522689f9d921801239f9a5a187ebb8m(int _f9522689f9d921801239f9a5a187ebb8a, int _f9522689f9d921801239f9a5a187ebb862, int _f9522689f9d921801239f9a5a187ebb8c = 0)
    {
        int t_f9522689f9d921801239f9a5a187ebb8ap = _f9522689f9d921801239f9a5a187ebb8a * _f9522689f9d921801239f9a5a187ebb862;
        if (_f9522689f9d921801239f9a5a187ebb8c != 0 && t_f9522689f9d921801239f9a5a187ebb8ap > _f9522689f9d921801239f9a5a187ebb8c)
        {
            t_f9522689f9d921801239f9a5a187ebb8ap = t_f9522689f9d921801239f9a5a187ebb8ap / _f9522689f9d921801239f9a5a187ebb8c;
        }
        else
        {
            t_f9522689f9d921801239f9a5a187ebb8ap -= _f9522689f9d921801239f9a5a187ebb8c;
        }

        return _f9522689f9d921801239f9a5a187ebb8m2(t_f9522689f9d921801239f9a5a187ebb8ap);
    }
}
