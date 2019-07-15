using UnityEngine;
using System.Collections;
using LitJson;
using VIVOSDKData;
using SDKData;
/*
 * 注意：由于vivo 初始化必须在activity中的oncreate方法中执行，因此除了本脚本，还需要在vivo sdk的mainactivcity中的oncreate方法里配置appid跟调试模式！
 * 
 */

/// <summary>
/// VIVO SDK 管理器
/// </summary>
public class VIVOPlatManager : PlatSDKManagerBase
{
    private string appid = "515a5ad644ed3e2b6d11e3e070e3abe7";
    private string uid = string.Empty;//用户登录后openId

    #region unity 访问方法
    public override void Init(string arg)
    {
        CallAndoridFunc("Init");
        //初始化成功 - 没有初始化回调，因此直接提示初始化成功
        AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.InitSuccess);
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
                mRoleId = arg.roleId,
                mRoleLevel = arg.roleLevel,
                mRoleName = arg.roleName,
                mServiceAreaID = arg.realmId,
                mServiceAreaName = arg.realmName,
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
        sendModel.mProductPrice = ((int)(arg.amount * 100)).ToString();//单位为分，5元 = 600 
        sendModel.mProductName = arg.productName;
        sendModel.mProductDes = arg.productDesc;
        sendModel.mAppId = appid;
        sendModel.mUid = uid;

        //可选项 - 暂时不填
        //sendModel.mBlance = string.Empty;//账户余额
        //sendModel.mVip = string.Empty;//vip等级
        //sendModel.mLevel = string.Empty;//用户等级
        //sendModel.mParty = string.Empty;//工会
        //sendModel.mRoleId = string.Empty;//角色id
        //sendModel.mRoleName = string.Empty;//角色名字
        //sendModel.mServerName = string.Empty;//区服名字
        // sendModel.mExtInfo = arg.callbackMessage;//扩展参数


        //TODO 将model发送给服务器，获生成具体订单信息
        sendModel.mTransNo = "";
        sendModel.mVivoSignature = "";


        //TODO  将服务器生成的具体订单model发送给sdk
        CallAndoridFunc("PayOrder", JsonMapper.ToJson(sendModel));
        AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_Createing);
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
    /// 登入回调函数
    /// </summary>
    public override void LoginCallBack(string arg)
    {
        var model = CheckCallBackArg<LoginCallBackModel>(arg);
        if (model == null)
        {
            if (arg == "-1")
            {
                DebugLog("登录操作已经注销！" + arg);

                AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Un);
            }
            else if (arg == "0")
            {
                AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Cancle);
                DebugLog("登录操作已经取消！" + arg);
            }
            else
            {
                DebugLog("登录回调数据异常：" + arg);
            }
            return;
        }

        DebugLog("收到登录回调：" + " 登录状态：" + model.loginStatue + " 回调oppenid：" + model.openId + "回调token：" + model.authToken);
        uid = model.openId;//赋值uid

        //TODO 安卓登入成功 - 此时需要将authToken发送给服务器校验access_token接口”校验帐户信息的有效
        //TODO 暂时先进游戏 - 验证逻辑放到选服那里
        AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Success, new SDKLoginCompleteData()
        {
            result = true,
            arg = model.authToken,
        });


    }

    /// <summary>
    /// 订单结果回调
    /// </summary>
    public override void PayResultCallBack(string arg)
    {
        var model = CheckCallBackArg<PayCreateCallBackModel>(arg);
        if (model == null)
            return;
        if (model.isSucc)
        {
            DebugLog("安卓部分订单支付成功：订单流水号：" + model.transNo);
            AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateSuccess);
        }
        else
        {
            DebugLog("安卓部分订单支付出错：订单流水号：" + model.transNo + "错误码：" + model.errorCode);
            AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateError);
        }
    }

    /// <summary>
    /// 退出sdk的回调
    /// </summary>
    public override void ExitGameCallBack(string arg)
    {
        if (arg.Equals("1"))
        {
            DebugLog("SDK 退出成功！");
            AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.ExitSuccess);
        }
        else if (arg.Equals("0"))
        {
            DebugLog("SDK 退出失败！");
            AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.ExitError);
        }
        else
        {
            DebugLog("SDK 退出回调异常！回调内容：" + arg);
        }
    }

    #endregion

}
#region Vivo 数据结构
namespace VIVOSDKData
{
    /// <summary>
    ///保存角色信息的数据model 
    /// </summary>
    [System.Serializable]
    public class SaveRoleDataModel
    {
        public string mRoleId;//角色id
        public string mRoleLevel;//等级
        public string mRoleName;//名字
        public string mServiceAreaID;//区服id
        public string mServiceAreaName;//区服名字
    }


    /// <summary>
    /// 支付申请model 
    /// </summary>
    [System.Serializable]
    public class PayDataModel
    {
        //必填项
        public string mTransNo;//交易流水号 	由订单推送接口返回
        public string mVivoSignature;//验签	由订单推送接口返回，字段为accessKey
        public string mProductName;//商品名称
        public string mProductDes;//商品描述
        public string mProductPrice;//商品价格 单位为分 6元=600  不能600.0
        public string mAppId;//appid
        public string mUid;//uid 登陆后获取

        //可选项
        public string mBlance;//账户余额
        public string mVip;//vip等级
        public string mLevel;//用户等级
        public string mParty;//工会
        public string mRoleId;//角色id
        public string mRoleName;//角色名字
        public string mServerName;//区服名字
        public string mExtInfo;//扩展参数
    }

    /// <summary>
    /// 登录回调model
    /// </summary>
    [System.Serializable]
    public class LoginCallBackModel
    {
        public int loginStatue;// 登录状态
        public string userName;// 用户名
        public string openId;
        public string authToken; // 效验码
    }


    /// <summary>
    /// 订单回调结果
    /// </summary>
    [System.Serializable]
    public class PayCreateCallBackModel
    {
        public string transNo;// 交易流水号
        public bool isSucc;
        public string errorCode;// 支付回调结果
    }
}
#endregion
