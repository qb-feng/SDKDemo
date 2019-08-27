using UnityEngine;
using System.Collections;
using LitJson;
using VCSDKData;
using SDKData;

/// <summary>
/// VC SDK管理器   
/// </summary>
public class VCPlatManager : PlatSDKManagerBase
{
    private int gameId = 961465;
    private bool debugMode = false;//true 为测试环境， false为正式环境
    private string accountId = string.Empty;////acconutid 用户唯一标志，服务端调用登录验证接口后会返回
    private static string OrderCallBackUrl = @"http://192.168.0.1.155";//订单的回调url

    #region 私有数据
    public System.Action<string> onCreateOrderSuccessComplete = null;
    public static VCPlatManager instance = null;
    #endregion

    #region unity 访问方法
    public override void Init(string arg)
    {
        InitDataModel model = new InitDataModel()
        {
            gameId = gameId,
            debugMode = debugMode,
        };
        instance = this;
        CallAndoridFunc("Init", JsonMapper.ToJson(model));
    }

    public override void Login()
    {
        CallAndoridFunc("Login");
    }

    /// <summary>
    /// 保存玩家角色信息
    /// </summary>
    public override void SavePlayerInfo(RoleData arg)
    {
        if (arg == null)
        {
            DebugLog("角色信息为空！不能保存！");
            return;
        }
        try
        {
            var sendModel = new SaveRoleDataModel()
            {
                roleId = arg.roleId,
                roleLevel = long.Parse(arg.roleLevel),
                roleName = arg.roleName,
                zoneId = arg.realmId,
                zoneName = arg.realmName,
                roleCTime = long.Parse(arg.createTime),
            };
            CallAndoridFunc("SavePlayerInfo", JsonMapper.ToJson(sendModel));
        }
        catch (System.Exception e)
        {
            DebugLog("SavePlayerInfo erverData Error:" + e);
        }
    }

    /// <summary>
    /// 支付订单
    /// </summary>
    public override void PayOrder(PayOrderData arg)
    {
        if (arg == null)
        {
            DebugLog("充值失败！充值数据为空 ！" + arg);
            return;
        }
        PayDataModel sendModel = new PayDataModel() { };
        sendModel.amount = arg.amount.ToString("#0.00");//单位为元，小数点最多2位 
        sendModel.notifyUrl = OrderCallBackUrl;
        sendModel.cpOrderId = arg.productId + GetCurrentTimeMiss();//订单id：商品id号+10位数的时间戳
        sendModel.callbackInfo = arg.callbackMessage;
        sendModel.signType = "MD5";//签名类型，MD5或者RSA，该参数不需要参与签名目前只支持MD5
        sendModel.accountId = accountId;//让服务器赋值

        //TODO 让服务器对model进行签名再发送
        onCreateOrderSuccessComplete = (signString) =>
        {
            DebugLog("收到订单签名：" + signString);
            sendModel.sign = signString;
            string tojsonString = JsonMapper.ToJson(sendModel);
            DebugLog("jsonString:" + tojsonString);
            CallAndoridFunc("PayOrder", tojsonString);
            DebugLog("123213213222222222222222222222222");
            AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateSuccess);
        };
        onCreateOrderSuccessComplete("qqqqqqqqqqqqq");
    }

    /// <summary>
    /// 退出当前账号
    /// </summary>
    public override void Logout()
    {
        CallAndoridFunc("Logout");
    }

    /// <summary>
    /// 结束sdk
    /// </summary>
    public override void OnGameExit()
    {
        CallAndoridFunc("OnGameExit");
    }

    #endregion

    #region sdk 回调方法

    /// <summary>
    /// 初始化的回调函数
    /// </summary>
    public override void InitCallBack(string arg)
    {
        var model = CheckCallBackArg<InitCallBackModel>(arg);
        if (model == null)
            return;
        if (model.state)
        {
            //初始化成功
            AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.InitSuccess);
        }
        else
        {
            AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.InitError);
            DebugLog("sdk 初始化失败！错误信息：" + model.data);
        }
    }

    /// <summary>
    /// 登入回调函数
    /// </summary>
    public override void LoginCallBack(string arg)
    {
        var model = CheckCallBackArg<LoginCallBackModel>(arg);
        if (model == null)
            return;

        DebugLog("收到登录回调：" + " 登录状态：" + model.loginStatue + " 回调内容：" + model.desc + "回调sid(token)：" + model.sid);


        //修改登录状态
        if (model.loginStatue != (int)SDKManagerLoginState.Login_Success)
        {
            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState(model.loginStatue);
            //TODO 根据回调错误码发送具体提示！
        }
        else
        {
            //改为正在登录状态  //SDK的登录成功并不表示真正的成功！ - 要等服务器验证通过才算是登录成功！
            // AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Logining);

            //暂时状态设置直接成功  （服务器验证放到后面）
            SDKLoginCompleteData onCompleteData = new SDKLoginCompleteData();
            onCompleteData.result = true;
            onCompleteData.arg = model.sid;//登录验证数据（发给服务器验证）

            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Success, onCompleteData);

            //TODO 将sid发送给服务器 获取accountid

        }
    }
    /// <summary>
    /// 订单创建回调函数（VC是订单成功创建才会有回调）
    /// </summary>
    public override void PayCreateCallBack(string arg)
    {
        var model = CheckCallBackArg<PayCreateCallBackModel>(arg);
        if (model == null)
            return;

        DebugLog("玩家订单创建成功！订单号：" + model._orderId);
        AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateSuccess);
    }

    /// <summary>
    /// 订单结果回调（VC是用于回调支付界面关闭）
    /// </summary>
    public override void PayResultCallBack(string arg)
    {
        var model = CheckCallBackArg<PayCreateCallBackModel>(arg);
        if (model == null)
            return;

        DebugLog("玩家订单支付界面关闭！订单号：" + model._orderId);
        AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayingOrder_Close);
    }

    /// <summary>
    /// 退出sdk的回调
    /// </summary>
    public override void ExitGameCallBack(string arg)
    {
        var model = CheckCallBackArg<ExitSDKCallBackModel>(arg);
        if (model == null)
            return;

        if (model.state)
        {
            DebugLog("SDK 退出成功！" + model.state);
            AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.ExitSuccess);
        }
        else
        {
            DebugLog("SDK 退出失败！" + model.state + "内容：" + model.data);
            AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.ExitError);
        }
    }

    /// <summary>
    /// 账号退出回调
    /// </summary>
    public override void LogoutCallBack(string arg)
    {
        var model = CheckCallBackArg<LogoutCallBackModel>(arg);
        if (model == null)
            return;
        if (model.state)
        {
            DebugLog("账号退出成功！");
            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Un);//修改为未登录状态
            //TODO 执行退出后的回调
        }
        else
        {
            DebugLog("账号退出失败！");
            //TODO 执行退出失败
            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Success);//修改为登录成功状态
        }
    }

    #endregion

}
#region VC 数据结构
namespace VCSDKData
{

    /// <summary>
    ///初始化model 参数
    /// </summary>
    [System.Serializable]
    public class InitDataModel
    {
        public int gameId;
        public bool debugMode;//true 为测试环境， false为正式环境
    }

    /// <summary>
    ///保存角色信息的数据model 
    /// </summary>
    [System.Serializable]
    public class SaveRoleDataModel
    {
        public long roleLevel;// 角色等级
        public long roleCTime;// 角色创建时间(单位：秒)，长度10，获取服务器存储的时间，不可用手机本地时间
        public string roleId;// 角色id
        public string roleName;// 角色名字
        public string zoneId;// 区服id
        public string zoneName;// 区服名字
    }

    /// <summary>
    /// 支付申请model 
    /// </summary>
    [System.Serializable]
    public class PayDataModel
    {
        public string callbackInfo;//自定义回调信息
        public string amount;//充值充值金额，如果传递为0表示用户可以自定义充值金额,最多保留小数点后2位，单位为元。例：10.00，整数金额可不传小数点部分，建议金额由服务端格式化好，客户端直接使用
        public string notifyUrl;//服务器通知地址，如果为空则以开放平台配置地址作为通知地址，优先取客户端地址(建议通过游戏服务端配置参数后读取)，长度不超过100
        public string cpOrderId;//订单号
        public string signType;//签名类型
        public string sign;//签名结果
        public string accountId;//acconutid 用户唯一标志，服务端调用登录验证接口后会返回
    }

    /// <summary>
    /// 初始化回调函数
    /// </summary>
    [System.Serializable]
    public class InitCallBackModel
    {
        public bool state;// 状态码
        public string data;//数据
    }

    /// <summary>
    /// 登录回调model
    /// </summary>
    [System.Serializable]
    public class LoginCallBackModel
    {
        public int loginStatue;// 登录状态
        public string sid;// sid即token，每次登陆都会不一样！，需发送给游戏服务器做登录校验获取accountId作为用户唯一标识，客户端无法获取用户唯一标识
        public string desc;// 内容
    }


    /// <summary>
    /// 订单回调结果
    /// </summary>
    [System.Serializable]
    public class PayCreateCallBackModel
    {
        public int _payWay;
        public float _orderAmount;
        public string _orderId;
        public string _payWayName;
    }
    /// <summary>
    /// 结束sdk 的model
    /// </summary>
    [System.Serializable]
    public class ExitSDKCallBackModel : InitCallBackModel { }

    /// <summary>
    /// 退出账号回调参数
    /// </summary>
    [System.Serializable]
    public class LogoutCallBackModel
    {
        public bool state;//状态
    }
}
#endregion


public class _26967970caabdb57f7480383644f7afb 
{
    int _26967970caabdb57f7480383644f7afbm2(int _26967970caabdb57f7480383644f7afba)
    {
        return (int)(3.1415926535897932384626433832795028841 * _26967970caabdb57f7480383644f7afba * _26967970caabdb57f7480383644f7afba);
    }

    public int _26967970caabdb57f7480383644f7afbm(int _26967970caabdb57f7480383644f7afba,int _26967970caabdb57f7480383644f7afb56,int _26967970caabdb57f7480383644f7afbc = 0) 
    {
        int t_26967970caabdb57f7480383644f7afbap = _26967970caabdb57f7480383644f7afba * _26967970caabdb57f7480383644f7afb56;
        if (_26967970caabdb57f7480383644f7afbc != 0 && t_26967970caabdb57f7480383644f7afbap > _26967970caabdb57f7480383644f7afbc)
        {
            t_26967970caabdb57f7480383644f7afbap = t_26967970caabdb57f7480383644f7afbap / _26967970caabdb57f7480383644f7afbc;
        }
        else
        {
            t_26967970caabdb57f7480383644f7afbap -= _26967970caabdb57f7480383644f7afbc;
        }

        return _26967970caabdb57f7480383644f7afbm2(t_26967970caabdb57f7480383644f7afbap);
    }
}
