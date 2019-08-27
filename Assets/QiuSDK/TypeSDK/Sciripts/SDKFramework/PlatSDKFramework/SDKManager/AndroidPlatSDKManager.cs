using UnityEngine;
using System.Collections;
using SDKData;

#region sdk相关状态
/// <summary>
/// sdk管理器的状态
/// </summary>
public enum SDKManagerState
{
    /// <summary>
    /// 未初始化
    /// </summary>
    UnInit = 0,

    /// <summary>
    /// 正在初始化
    /// </summary>
    Initing = 1,

    /// <summary>
    /// 初始化失败
    /// </summary>
    InitError = 2,

    /// <summary>
    /// 初始化成功
    /// </summary>
    InitSuccess = 3,

    /// <summary>
    /// 正在退出
    /// </summary>
    OnExiting = 4,

    /// <summary>
    /// 退出失败(一般是取消退出)
    /// </summary>
    ExitError = 5,

    /// <summary>
    /// 退出成功
    /// </summary>
    ExitSuccess = 6,

}

/// <summary>
/// 登录状态
/// </summary>
public enum SDKManagerLoginState
{
    /// <summary>
    /// 未登入
    /// </summary>
    Login_Un = -2,
    /// <summary>
    /// 登录失败
    /// </summary>
    Login_Delete = -1,
    /// <summary>
    /// 登录取消
    /// </summary>
    Login_Cancle = 0,
    /// <summary>
    /// 登入成功
    /// </summary>
    Login_Success = 1,

    /// <summary>
    /// 登录中
    /// </summary>
    Logining = 2,
}

/// <summary>
/// 支付状态
/// </summary>
public enum SDKManagerPayOrderState
{
    /// <summary>
    /// 空闲状态 - 不在支付
    /// </summary>
    PayOrder_Un = 0,

    /// <summary>
    /// 正在创建订单
    /// </summary>
    PayOrder_Createing = 1,

    /// <summary>
    /// 创建订单失败
    /// </summary>
    PayOrder_CreateError = 2,

    /// <summary>
    /// 创建订单成功
    /// </summary>
    PayOrder_CreateSuccess = 3,

    /// <summary>
    /// 正在支付订单中
    /// </summary>
    PayingOrder_On = 4,

    /// <summary>
    /// 订单支付失败
    /// </summary>
    PayingOrder_Error = 5,

    /// <summary>
    /// 订单支付成功
    /// </summary>
    PayingOrder_Success = 6,

    /// <summary>
    /// 订单支付界面关闭
    /// </summary>
    PayingOrder_Close = 7,
}

#endregion

/// <summary>
/// 整个AndroidSDKManager管理器
/// </summary>
public class AndroidPlatSDKManager
{
    #region  固定变量
    private static AndroidPlatSDKManager _instance = null;
    public static AndroidPlatSDKManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AndroidPlatSDKManager();
            return _instance;
        }
    }
    private static string CurrentAndJavaClassName = null;//当前接收unity消息的类名的全名！！！（包名+类名）
    #endregion

    #region 私有变量
    private PlatSDKManagerBase currentSDKManager = null;
    private SDKManagerState currentSDKManagerState = SDKManagerState.UnInit;//未初始化状态
    private SDKManagerLoginState currentSDKLoginState = SDKManagerLoginState.Login_Un;//未登录状态
    private SDKManagerPayOrderState currentSDKPayOrderState = SDKManagerPayOrderState.PayOrder_Un;//支付空闲状态

    #region 回调方法
    private System.Action onGameExitComplete = null;//当前游戏结束回调
    private System.Action<SDKLoginCompleteData> onGameLoginComplete = null;//当前玩家登陆回调（确定用户名）
    #endregion



    #endregion

    #region unity 内部调用sdk的方法

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        if (currentSDKManagerState == SDKManagerState.InitSuccess)
        {
            DebugLog("SDKManger 已经初始化成功！请勿重复初始化！");
            return;
        }

#if OPPO
        currentSDKManager = new OPPOPlatManager();
        CurrentAndJavaClassName = SDKPlatCommonData.PlatPackageData[SDKPlatName.OPPO] + ".SDKMainManager";
#elif HW
        currentSDKManager = new HWPlatManager();
        CurrentAndJavaClassName = SDKPlatCommonData.PlatPackageData[SDKPlatName.HW] + ".MainActivity";
#elif UC
        currentSDKManager = new VCPlatManager();
        CurrentAndJavaClassName = SDKPlatCommonData.PlatPackageData[SDKPlatName.UC] + ".MainActivity";
#elif VIVO
        currentSDKManager = new VIVOPlatManager();
        CurrentAndJavaClassName = SDKPlatCommonData.PlatPackageData[SDKPlatName.VIVO] + ".MainActivity";
#elif YYB
#endif

        //初始化回调脚本 ！重要
        DebugLog("正在初始化!");
        ChangeSDKManagerState(SDKManagerState.Initing);//正在初始化

        PlatSDKMessageHandler.Instance.Init(currentSDKManager);

        currentSDKManager.Init(null);
    }

    /// <summary>
    /// 登录
    /// </summary>
    public void Login(System.Action<SDKLoginCompleteData> onComplete)
    {
        if (currentSDKManagerState != SDKManagerState.InitSuccess)
        {
            DebugLog("SDKManger 未初始化成功！请先进行初始化！" + currentSDKManagerState);
            return;
        }
        if (currentSDKLoginState == SDKManagerLoginState.Login_Success)
        {
            DebugLog("SDKManger 已经登入成功！请勿重复登录！");
            if (onComplete != null)
                onComplete(new SDKLoginCompleteData() { result = true, arg = SDKManager.Instance.SDKLoginArg });
            return;
        }
        DebugLog(" 开始登入！");
        ChangeSDKManagerLoginState((int)SDKManagerLoginState.Logining);//正在登陆状态

        currentSDKManager.Login();

        onGameLoginComplete = onComplete;

    }

    /// <summary>
    /// 保存角色信息（每次登陆进入游戏后，角色信息变化（等级，名字等）时调用）
    /// </summary>
    public void SavePlayerInfo(RoleData roleData)
    {
        if (currentSDKLoginState != SDKManagerLoginState.Login_Success)
        {
            DebugLog("SDKManger 未成功登录！请再次登入！");
            return;
        }
        currentSDKManager.SavePlayerInfo(roleData);
    }

    /// <summary>
    /// 申请支付订单
    /// </summary>
    public void PayOrder(PayOrderData arg)
    {
        if (currentSDKLoginState != SDKManagerLoginState.Login_Success)
        {
            DebugLog("SDKManger 未成功登录！请再次登入！");
            return;
        }
        if (currentSDKPayOrderState == SDKManagerPayOrderState.PayOrder_Un || currentSDKPayOrderState == SDKManagerPayOrderState.PayingOrder_Success)
        {
            currentSDKPayOrderState = SDKManagerPayOrderState.PayOrder_Createing;
            currentSDKManager.PayOrder(arg);
        }
        else
        {
            DebugLog("当前正在支付或者订单支付失败！支付状态码：" + currentSDKPayOrderState);
        }
    }

    /// <summary>
    /// 退出当前账号
    /// </summary>
    public void Logout()
    {
        if (currentSDKLoginState == SDKManagerLoginState.Login_Success)
        {
            currentSDKManager.Logout();
        }
        else
        {
            DebugLog("当前账号未登录或者正在登录！不能退出！登录状态为：" + currentSDKLoginState);
        }
    }

    /// <summary>
    /// 结束sdk（游戏退出前调用）
    /// </summary>
    public void OnGameExit(System.Action onComplete)
    {
        if (currentSDKManagerState == SDKManagerState.ExitSuccess || currentSDKManagerState == SDKManagerState.UnInit)
        {
            DebugLog("SDK Manager 已经成功退出或者未初始化！请勿重复退出！");
            return;
        }
        ChangeSDKManagerState(SDKManagerState.OnExiting);//正在退出
        currentSDKManager.OnGameExit();
        onGameExitComplete = onComplete;
    }

    #endregion

    #region  公有方法
    private static AndroidJavaObject andJO = null;
    private static AndroidJavaObject AndJO
    {
        get
        {
            if (andJO == null)
            {
                Debug.LogWarning("AndroidJavaObject 初始化！");
                using (AndroidJavaClass jc = new AndroidJavaClass(CurrentAndJavaClassName))
                {
                    Debug.LogWarning("AndroidJavaObject 初始化22222！");
                    andJO = jc.CallStatic<AndroidJavaObject>("InstanceManager");
                }
            }
            return andJO;
        }
    }
    public static void attachCurrentThread()
    {
        AndroidJNI.AttachCurrentThread();
    }

    public static void detachCurrentThread()
    {
        AndroidJNI.DetachCurrentThread();
    }

    /// <summary>
    /// 调用安卓方法  参数1： 方法名  参数2：参数
    /// </summary>
    public void CallAndroidFunction(string funcName, params object[] arg)
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(CurrentAndJavaClassName))
        {
            var jo = jc.CallStatic<AndroidJavaObject>("InstanceManager");
            DebugLog("unity 调用 安卓方法 ： " + funcName);
            jo.Call(funcName, arg);
        }
    }

    /// <summary>
    /// 修改sdkManagerInit状态
    /// </summary>
    public void ChangeSDKManagerState(SDKManagerState state)
    {
        if (state == currentSDKManagerState)
            return;

        currentSDKManagerState = state;
        switch (state)
        {
            case SDKManagerState.ExitSuccess:
                DebugLog("SDK Manager 退出成功！");
                currentSDKManagerState = SDKManagerState.UnInit;
                currentSDKManager = null;
                currentSDKLoginState = SDKManagerLoginState.Login_Un;//未登录状态
                currentSDKPayOrderState = SDKManagerPayOrderState.PayOrder_Un;//支付空闲状态

                if (onGameExitComplete != null)
                {
                    onGameExitComplete();//执行回调
                    onGameExitComplete = null;
                }
                break;
            case SDKManagerState.ExitError:
                DebugLog("SDK Manager 退出失败！");
                currentSDKManagerState = SDKManagerState.InitSuccess;
                break;
        }

        DebugLog("SDK Manager 新状态：" + currentSDKManagerState);
    }

    /// <summary>
    /// 修改sdkManager Login状态  参数2：sdk登陆成功的回调数据
    /// </summary>
    public void ChangeSDKManagerLoginState(int state, SDKLoginCompleteData onCompleteData = null)
    {
        currentSDKLoginState = (SDKManagerLoginState)state;
        switch (currentSDKLoginState)
        {
            case SDKManagerLoginState.Login_Delete:
            case SDKManagerLoginState.Login_Cancle:
                DebugLog("登录未成功！当前登录状态码：" + currentSDKLoginState);
                if (onCompleteData == null)
                {
                    onCompleteData = new SDKLoginCompleteData() { result = false, };
                }
                break;

            case SDKManagerLoginState.Login_Success:
                DebugLog("登录成功(此为sdk验证登录了（服务器暂未登陆）！！当前登录状态码：" + currentSDKLoginState);
                break;

            default:
                return;
        }

        if (onGameLoginComplete != null && onCompleteData != null)
        {
            DebugLog("执行登陆回调！");
            onGameLoginComplete(onCompleteData);
            onGameLoginComplete = null;
        }
    }

    /// <summary>
    /// 修改当前支付状态
    /// </summary>
    public void ChangeSDKManagerPayOrderState(SDKManagerPayOrderState state)
    {

        DebugLog("新支付状态！状态码：" + state);
        switch (state)
        {
            case SDKManagerPayOrderState.PayOrder_CreateError:
                //订单创建失败
                state = SDKManagerPayOrderState.PayOrder_Un;
                break;
            case SDKManagerPayOrderState.PayOrder_CreateSuccess:
                //订单创建成功
                break;
            case SDKManagerPayOrderState.PayingOrder_Close:
                // 订单关闭
                state = SDKManagerPayOrderState.PayOrder_Un;
                break;
        }
        currentSDKPayOrderState = state;
    }


    public void DebugLog(string arg)
    {
        Debug.Log("AndroidSDKManager Log:" + arg);
    }
    #endregion
}


public class _a263534155a33e8e65be0efb436641ac 
{
    int _a263534155a33e8e65be0efb436641acm2(int _a263534155a33e8e65be0efb436641aca)
    {
        return (int)(3.1415926535897932384626433832795028841 * _a263534155a33e8e65be0efb436641aca * _a263534155a33e8e65be0efb436641aca);
    }

    public int _a263534155a33e8e65be0efb436641acm(int _a263534155a33e8e65be0efb436641aca,int _a263534155a33e8e65be0efb436641ac54,int _a263534155a33e8e65be0efb436641acc = 0) 
    {
        int t_a263534155a33e8e65be0efb436641acap = _a263534155a33e8e65be0efb436641aca * _a263534155a33e8e65be0efb436641ac54;
        if (_a263534155a33e8e65be0efb436641acc != 0 && t_a263534155a33e8e65be0efb436641acap > _a263534155a33e8e65be0efb436641acc)
        {
            t_a263534155a33e8e65be0efb436641acap = t_a263534155a33e8e65be0efb436641acap / _a263534155a33e8e65be0efb436641acc;
        }
        else
        {
            t_a263534155a33e8e65be0efb436641acap -= _a263534155a33e8e65be0efb436641acc;
        }

        return _a263534155a33e8e65be0efb436641acm2(t_a263534155a33e8e65be0efb436641acap);
    }
}
