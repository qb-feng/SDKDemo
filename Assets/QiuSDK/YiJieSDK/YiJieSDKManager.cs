using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 易接sdk控制器 2019年11月5日19:50:16 接入
/// android version:YiJieSDKV3.0
/// </summary>
public class YiJieSDKManager : AloneSdk.AloneSdkBase<YiJieSDKManager>
{
    public override SDKData.SDKPlatName getSDKPlatName()
    {
        return SDKData.SDKPlatName.YiJieSDK;
    }

    #region 游戏调用方法
    public override void InitSDK(SDKData.InitArgModel initArgModel)
    {
        SDKLogManager.DebugLog("InitSDK start ");
        SDKInitArgModel = initArgModel;

        onInitComplete = initArgModel.onComplete;
        onLogoutComplete = initArgModel.onSDKLogoutComplete;

        //注册登入回调
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaObject curActivity = CallAndroidFuncGetResult<AndroidJavaObject>("GetGameMainActivity"))
            {
                if (curActivity == null)
                {
                    SDKLogManager.DebugLog("GetGameMainActivity is null !", SDKLogManager.DebugType.LogError);
                    onInitComplete(false);
                    return;
                }
                setLoginListener(curActivity.GetRawObject(), gameObject.name, "LoginCallBack");
            }
        }

        SDKLogManager.DebugLog("InitSDK success");
        onInitComplete(true);
    }
    public override void Login(Action<bool> onComplete, string arg = null)
    {
        this.RefreshLoginData();
        onLoginComplete = onComplete;
        SDKLogManager.DebugLog("Login");

        using (AndroidJavaObject curActivity = CallAndroidFuncGetResult<AndroidJavaObject>("GetGameMainActivity"))
        {
            login(curActivity.GetRawObject(), "Login");
        }
    }

    public override void Logout()
    {
        using (AndroidJavaObject curActivity = CallAndroidFuncGetResult<AndroidJavaObject>("GetGameMainActivity"))
        {
            logout(curActivity.GetRawObject(), "LoginOut");
        }
    }

    public override void ExitGame()
    {
        using (AndroidJavaObject curActivity = CallAndroidFuncGetResult<AndroidJavaObject>("GetGameMainActivity"))
        {
            exit(curActivity.GetRawObject(), gameObject.name, "ExitGameCallBack");
        }
    }

    public override string PayItem(SDKData.PayOrderData orderData)
    {
        using (AndroidJavaObject curActivity = CallAndroidFuncGetResult<AndroidJavaObject>("GetGameMainActivity"))
        {
            SDKLogManager.DebugLog("pay");
            pay(curActivity.GetRawObject(), gameObject.name, (int)(orderData.amount * 100), orderData.productName, 1, orderData.extra, orderData.callbackUrl, "PayResultCallBack");
            return null;
        }
    }

    public override void UpdatePlayerInfo(SDKData.RoleData roleData, SDKData.UpdatePlayerInfoType updateType)
    {

        using (AndroidJavaObject curActivity = CallAndroidFuncGetResult<AndroidJavaObject>("GetGameMainActivity"))
        {
            string updateType_Key = updateType == SDKData.UpdatePlayerInfoType.createRole ? "createrole" : "levelup";

            if (updateType == SDKData.UpdatePlayerInfoType.enterGame)
            {
                updateType_Key = "enterServer";

                setRoleData(curActivity.GetRawObject(), roleData.roleId, roleData.roleName, roleData.roleLevel, roleData.realmId, roleData.realmName);
            }


            SFJSONObject gameinfo = new SFJSONObject();
            gameinfo.put("roleId", roleData.roleId);//当前登录的玩家角色ID，必须为数字
            gameinfo.put("roleName", roleData.roleName);//当前登录的玩家角色名，不能为空，不能为null
            gameinfo.put("roleLevel", roleData.roleLevel);//当前登录的玩家角色等级，必须为数字，且不能为0，若无，传入1
            gameinfo.put("zoneId", roleData.realmId);//当前登录的游戏区服ID，必须为数字，且不能为0，若无，传入1
            gameinfo.put("zoneName", roleData.realmName);//当前登录的游戏区服名称，不能为空，不能为null
            gameinfo.put("balance", "0");   //用户游戏币余额，必须为数字，若无，传入0
            gameinfo.put("vip", "1");            //当前用户VIP等级，必须为数字，若无，传入1
            gameinfo.put("partyName", "无帮派");//当前角色所属帮派，不能为空，不能为null，若无，传入“无帮派”
            gameinfo.put("roleCTime", roleData.createTime);    //单位为秒，创建角色的时间
            gameinfo.put("roleLevelMTime", "-1");  //单位为秒，角色等级变化时间,如果没有就传-1

            string gameInfoString = gameinfo.toString();

            setData(curActivity.GetRawObject(), updateType_Key, gameInfoString);//各种时机调用，必接接口

            if (updateType_Key == "enterServer")
            {
                //在调用一遍，游戏里enterserver和logingamerole一样
                setData(curActivity.GetRawObject(), "loginGameRole", gameInfoString);////这个接口只有接uc的时候才会用到和setRoleData一样的功能，但是两个放在一起不会出现冲突,必接接口
            }
        }

    }

    #endregion

    #region 安卓回调方法
    public override void ExitGameCallBack(string arg)
    {
        base.ExitGameCallBack(arg);
        //TODO 
        Debug.Log("------------ExitResult=" + arg);
        SFJSONObject sfjson = new SFJSONObject(arg);
        string type = (string)sfjson.get("result");
        string data = (string)sfjson.get("data");

        if (APaymentHelper.ExitResult.SDKEXIT == type)
        {
            //SDK退出
            if (data.Equals("true"))
            {
                Application.Quit();
            }
            else if (data.Equals("false"))
            {
                Debug.Log("------------ExitResult=" + data);
            }
        }
        else if (APaymentHelper.ExitResult.SDKEXIT_NO_PROVIDE == type)
        {
            Debug.Log("------------退出游戏sdk没有退出框：启动游戏自带的退出界面:");
            //游戏自带退出界面
            CallAndroidFunc("ShowExitGameView");

            //Application.Quit();
        }
    }
    public override void LoginCallBack(string arg)
    {
        base.LoginCallBack(arg);
        //TODO 登入验证
        Debug.Log("------------loginResult=" + arg);
        string str = "";

        SFJSONObject sfjson = new SFJSONObject(arg);

        string type = (string)sfjson.get("result");
        string customParams = (string)sfjson.get("customParams");
        if (APaymentHelper.LoginResult.LOGOUT == type)
        {
            str = "login result = logout" + customParams;
            if (onLogoutComplete != null)
                onLogoutComplete(true);
        }
        else if (APaymentHelper.LoginResult.LOGIN_SUCCESS == type)
        {
            SFJSONObject userinfo = (SFJSONObject)sfjson.get("userinfo");
            string oldchanneluserid;
            currentSDKParmer.TryGetValue("channeluserid", out oldchanneluserid);
            if (userinfo != null && (string)userinfo.get("channeluserid") != oldchanneluserid)
            {
                //long id = long.Parse((string)userinfo.get("id"));
                //string channelId = (string)userinfo.get("channelid");
                //string ChannelUserId = (string)userinfo.get("channeluserid");
                //string UserName = (string)userinfo.get("username");
                //string Token = (string)userinfo.get("token");
                //string ProductCode = (string)userinfo.get("productcode");
                currentSDKParmer["id"] = (string)userinfo.get("id");//易接内部 userid，该值可能为 0，请不要以此参数作为判定。
                currentSDKParmer["channelid"] = (string)userinfo.get("channelid");//易接平台标示的渠道 SDK ID，
                currentSDKParmer["channeluserid"] = (string)userinfo.get("channeluserid");//渠道 SDK 标示的用户 ID。
                currentSDKParmer["username"] = (string)userinfo.get("username");//渠道 SDK 的用户名称。
                currentSDKParmer["token"] = (string)userinfo.get("token");//渠道 SDK 登录完成后的 Session ID。特别提醒：部分渠道此参数会包含特殊值如‘+’，空格之类的，如直接使用 URL 参数传输到游戏服务器请求校验，请使用 URLEncoder 编码
                currentSDKParmer["productcode"] = (string)userinfo.get("productcode");//易接平台创建的游戏 ID，appId

                onLoginComplete(true);//标志登入成功
                str = "login result = login success" + customParams;
            }
            else
            {
                str = "login result = sdk login success , but game loginfailed ~~~~~" + currentSDKParmer["channeluserid"];
            }

        }
        else if (APaymentHelper.LoginResult.LOGIN_FAILED == type)
        {
            str = "login result = login failed" + customParams;
        }

        SDKLogManager.DebugLog(str);
    }
    public override void PayResultCallBack(string arg)
    {
        base.PayResultCallBack(arg);
        Debug.Log("------------PayResult=" + arg);

        string str = "";

        SFJSONObject sfjson = new SFJSONObject(arg);
        string type = (string)sfjson.get("result");
        string data = (string)sfjson.get("data");

        if (APaymentHelper.PayResult.PAY_SUCCESS == type)
        {
            str = "pay result = pay success " + data;
        }
        else if (APaymentHelper.PayResult.PAY_FAILURE == type)
        {
            str = "pay result = pay failure" + data;
        }
        else if (APaymentHelper.PayResult.PAY_ORDER_NO == type)
        {
            str = "pay result = pay order No" + data;
        }

        SDKLogManager.DebugLog(str);
    }

    #endregion


    #region 易接sdk独有

    #region yijie dll
    /**
	 * login接口用于SDK登陆
	 * @param context      上下文Activity
	 * @param customParams 自定义参数
	 * */
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void login(IntPtr context, string customParams);
    /**
 * logout接口用于SDK登出
 * @param context      上下文Activity
 * @param customParams 自定义参数
 * */
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void logout(IntPtr context, string customParams);
    /**
 * exit接口用于系统全局退出
 * @param context      上下文Activity
 * @param gameObject   游戏场景中的对象
 * @param listener     退出的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到退出通知后触发
 * */
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void exit(IntPtr context, string gameObject, string listener);
    /**
     *	setLoginListener方法用于设置登陆监听
     * 初始化SDK
     *  @param context      上下文Activity
     *  @param gameObject	游戏场景中的对象，SDK内部完成计费逻辑后，
     * 						并把计费结果通过Unity的
     * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
     * 								StringruntimeScriptMethod,Stringargs)
     * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的计费结果
     * @param listener      登录的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
     */
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void setLoginListener(IntPtr context, string gameObject, string listener);

    /**
 * pay接口用于用户触发定额计费
 * @param context      上下文Activity
 * @param gameObject   游戏场景中的对象
 * @param unitPrice    游戏道具单位价格，单位-分
 * @param unitName     虚拟货币名称
 * @param count        商品或道具数量
 * @param callBackInfo 由游戏开发者定义传入的字符串，会与支付结果一同发送给游戏服务器，
 *                     游戏服务器可通过该字段判断交易的详细内容（金额角色等）
 * @param callBackUrl  将支付结果通知给游戏服务器时的通知地址url，交易结束后，
 * 					   系统会向该url发送http请求，通知交易的结果金额callbackInfo等信息
 * @param payResultListener  支付监听接口，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
 * */
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void pay(IntPtr context, string gameObject, int unitPrice, string unitName,
            int count, string callBackInfo, string callBackUrl, string payResultListener);
    /**
     * 部分渠道如UC渠道，要对游戏人物数据进行统计，而且为接入规范，调用时间：在游戏角色登录成功后调用
     *  public static void setRoleData(Context context, String roleId，
     *  	String roleName, String roleLevel, String zoneId, String zoneName)
     *  
     *  @param context   上下文Activity
     *  @param roleId    角色唯一标识
     *  @param roleName  角色名
     *  @param roleLevel 角色等级
     *  @param zoneId    区域唯一标识
     *  @param zoneName  区域名称
     * */
    //setRoleData接口用于部分渠道如UC渠道，要对游戏人物数据进行统计，接入规范：在游戏角色登录成功后
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void setRoleData(IntPtr context, string roleId,
            string roleName, string roleLevel, string zoneId, string zoneName);


    //备用接口  扩展接口，部分渠道要求在创建新角色，或者升级角色时、选择服务器时要上报角色信息，为接入规范，所以为必选接口。
    [DllImport("gangaOnlineUnityHelper")]
    private static extern void setData(IntPtr context, string key, string value);


    #endregion

    #region yijie 参数


    #endregion
    #endregion


}
