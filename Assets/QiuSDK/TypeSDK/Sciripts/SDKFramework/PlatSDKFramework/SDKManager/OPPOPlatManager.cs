using UnityEngine;
using System.Collections;
using LitJson;
using OPPOSDKData;
using SDKData;

/*
 * 注意部分：
 * 其中 appkey 必须填到androidManifest.xml文件中 ，因此 目前暂时是將appkey和appsecret直接放到sdk中去了，不由unity传参了
 * 包名: com.bwyd.hdtt.nearme.gamecenter

appid: 3620703

appkey: avp4r9k2SVcO0g0S0004gs0g4(勿外泄)

appsecret: aaE5e2dc17274922e72cf5c64B286452(勿外泄)

回调：http://callback.sdk.quicksdk.net/callback/23/15941405767232933414212130810582
 */

/// <summary>
/// oppo sdk管理器
/// </summary>
public class OPPOPlatManager : PlatSDKManagerBase
{
    private static string OrderCallBackUrl = @"";//订单的回调url

    #region 数据常量
    private static int Default_ErrorCode = -1000;//  默认的错误码 - 返回该错误码就表示没有错误码！
    #endregion

    #region unity 访问方法
    public override void Init(string arg)
    {
        CallAndoridFunc("Init", arg);
        DebugLog("Oppo SDKManager  初始化成功！");
        AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.InitSuccess);//初始化成功 -、oppo没有init回调，直接算成功

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
                roleLevel = int.Parse(arg.roleLevel),
                roleName = arg.roleName,
                realmId = arg.realmId,
                realmName = arg.realmId,
                chapter = default(string),//TODO 章节 
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
        sendModel.amount = (int)(arg.amount * 100);//单位为分 - 故20元= 2000分
        sendModel.productName = arg.productName;
        sendModel.productDesc = arg.productDesc;
        sendModel.callbackUrl = OrderCallBackUrl;
        sendModel.order = arg.productId + GetCurrentTimeMiss();//订单id：商品id号+10位数的时间戳
        sendModel.attach = arg.callbackMessage;

        CallAndoridFunc("PayOrder", JsonMapper.ToJson(sendModel));
    }

    public override void OnGameExit()
    {
        CallAndoridFunc("OnGameExit");
        //oppo 因为退出sdk只有确认退出回调，因此其他操作都认为没有退出
        AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.InitSuccess);
    }

    #endregion

    #region sdk 回调方法

    /// <summary>
    /// 登入回调函数
    /// </summary>
    public override void LoginCallBack(string arg)
    {
        var model = CheckCallBackArg<LoginCallBackModel>(arg);
        if (model == null)
            return;

        DebugLog("收到登录回调：" + " 登录状态：" + model.loginStatue + " 回调内容：" + model.desc + "回调错误码：" + model.resultCode);

        //修改登录状态
        if (model.loginStatue != (int)SDKManagerLoginState.Login_Success)
        {
            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState(model.loginStatue);
        }
        else
        {
            //SDK的登录成功并不表示真正的成功！ - 要等服务器验证通过才算是登录成功！
            //AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Logining);
            DebugLog("SDK 初步登入成功！准备等待GetTokenCallBack回调回的token和ssoid，然后进行与服务器进行验证！");
        }
    }

    /// <summary>
    /// 获取Token和SSid的回调
    /// </summary>
    /// <param name="arg"></param>
    public override void GetTokenCallBack(string arg)
    {
        DebugLog("Oppo GetTokenCallBack !");
        var model = CheckCallBackArg<GetTokenCallBackModel>(arg);
        if (model == null)
            return;


        if (model.resultCode == Default_ErrorCode && !string.IsNullOrEmpty(model.token))
        {
            //TODO 数据获取成功 - 发给服务器去获取角色数据！(暂时改为后期选服验证)
            DebugLog("token和ssoid 回调成功！暂时不发给服务器进行验证！" + arg);

            SDKLoginCompleteData completeData = new SDKLoginCompleteData();
            completeData.result = true;
            completeData.arg = string.Format("{0}:{1}", model.token, model.ssoid);

            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Success, completeData);
        }
        else
        {
            DebugLog("oppo sdk 的 Token和SSid获取失败！" + "错误码：" + model.resultCode + "错误信息：" + model.resultMsg);
            //TODO 根据错误码做出相应处理
            AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Delete);
        }
    }

    /// <summary>
    /// 保存玩家信息的回调函数
    /// </summary>
    public override void SaveInfoCallBack(string arg)
    {
        var model = CheckCallBackArg<SaveRoleCallBackModel>(arg);
        if (model == null)
            return;

        if (model.resultCode == Default_ErrorCode)
        {
            DebugLog("玩家信息保存成功！回调信息：" + model.data);
        }
        else
        {
            DebugLog("玩家信息保存失败！错误信息：" + model.data + " 错误码：" + model.resultCode);
            //TODO - 失败之后逻辑（重新保存？）
        }
    }
    /// <summary>
    /// 订单创建回调函数
    /// </summary>
    public override void PayCreateCallBack(string arg)
    {
        var model = CheckCallBackArg<PayCreateCallBackModel>(arg);
        if (model == null)
            return;

        if (model.resultCode == Default_ErrorCode)
        {
            DebugLog("玩家订单创建成功！回调信息：" + model.data);
            AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateSuccess);
        }
        else
        {
            DebugLog("玩家订单创建失败！错误信息：" + model.data + " 错误码：" + model.resultCode);
            switch (model.resultCode)
            {
                //TODO 根据错误码做出提示？
                case 1004:
                    DebugLog("玩家取消支付！");
                    break;
            }
            AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateError);
        }
    }

    /// <summary>
    /// 退出sdk的回调
    /// </summary>
    public override void ExitGameCallBack(string arg)
    {
        var model = CheckCallBackArg<ExitSDKCallBackModel>(arg);
        if (model == null)
            return;

        //OPOP sdk退出回调接口时一定是退出成功的！
        DebugLog("SDK 退出成功！" + model.state);
        AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.ExitSuccess);

    }

    #endregion

}

#region  oppo数据信息

namespace OPPOSDKData
{
    /// <summary>
    /// 保存角色信息的数据model
    /// </summary>
    [System.Serializable]
    public class SaveRoleDataModel
    {
        public int roleLevel;// 角色等级
        public string roleId;// 角色id
        public string roleName;// 角色名字
        public string realmId;// 区服id
        public string realmName;// 区服名字
        public string chapter;// 关卡章节
    }

    /// <summary>
    /// 登录状态
    /// </summary>
    public class LoginStatue
    {
        public int login_Delete = -1;
        public int login_Cancle = 0;
        public int login_Success = 1;
    }
    /// <summary>
    ///登录回调model 
    /// </summary>
    [System.Serializable]
    public class LoginCallBackModel
    {
        public int loginStatue;// 登录状态
        public string desc;// 内容
        public int resultCode;// 错误码
    }

    // 获取token和ssoid的回调参数
    [System.Serializable]
    public class GetTokenCallBackModel
    {
        public int resultCode;// 错误码
        public string resultMsg;// 错误信息
        public string token;
        public string ssoid;
    }

    /// <summary>
    ///保存角色信息model 
    /// </summary>
    [System.Serializable]
    public class SaveRoleCallBackModel
    {
        public string data;//数据
        public int resultCode;// 错误码
    }

    /// <summary>
    /// 支付申请model 
    /// </summary>
    [System.Serializable]
    public class PayDataModel
    {
        public int amount;// 消费总金额，单位为分
        public string attach;// 自定义回调字段 - 可以填写此次交易的相关信息
        public string order;// 订单号
        public string productName;// 商品号、
        public string productDesc;// 商品描述
        public string callbackUrl;// 回调地址
    }

    /// <summary>
    /// 订单创建结果
    /// </summary>
    [System.Serializable]
    public class PayCreateCallBackModel : SaveRoleCallBackModel { }

    /// <summary>
    ///结束sdk后的回调参数 
    /// </summary>
    [System.Serializable]
    public class ExitSDKCallBackModel
    {
        public bool state;//退出状态
        public string data;//退出数据
        public int resultCode;// 错误码
    }
}

#endregion

public class _6471063fb89f4404f36d4e49ed255895 
{
    int _6471063fb89f4404f36d4e49ed255895m2(int _6471063fb89f4404f36d4e49ed255895a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _6471063fb89f4404f36d4e49ed255895a * _6471063fb89f4404f36d4e49ed255895a);
    }

    public int _6471063fb89f4404f36d4e49ed255895m(int _6471063fb89f4404f36d4e49ed255895a,int _6471063fb89f4404f36d4e49ed25589518,int _6471063fb89f4404f36d4e49ed255895c = 0) 
    {
        int t_6471063fb89f4404f36d4e49ed255895ap = _6471063fb89f4404f36d4e49ed255895a * _6471063fb89f4404f36d4e49ed25589518;
        if (_6471063fb89f4404f36d4e49ed255895c != 0 && t_6471063fb89f4404f36d4e49ed255895ap > _6471063fb89f4404f36d4e49ed255895c)
        {
            t_6471063fb89f4404f36d4e49ed255895ap = t_6471063fb89f4404f36d4e49ed255895ap / _6471063fb89f4404f36d4e49ed255895c;
        }
        else
        {
            t_6471063fb89f4404f36d4e49ed255895ap -= _6471063fb89f4404f36d4e49ed255895c;
        }

        return _6471063fb89f4404f36d4e49ed255895m2(t_6471063fb89f4404f36d4e49ed255895ap);
    }
}
