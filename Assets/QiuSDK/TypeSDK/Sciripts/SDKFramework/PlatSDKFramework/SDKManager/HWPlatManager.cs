using UnityEngine;
using System.Collections;
using LitJson;
using HWSDKData;
using SDKData;
using System.Collections.Generic;
using System.Text;
using System.Linq;
/*
 * 注意：appid等数据除了在下面填以为，还要在hw渠道对应的xml文件中配置！！！
 */

/// <summary>
/// VIVO SDK 管理器
/// </summary>
public class HWPlatManager : PlatSDKManagerBase
{
    string appId = "100317679";
    //商户id（支付id）
    string merchantId = "890086000102085763";
    //商户名字
    string merchantName = "深圳市博万移动科技有限公司";
    //支付私钥
    string pay_priv_key = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCTrcwdnBuTvXOy4XrFgxI/rWi1TVDMQlmOT3YNnta1O9jzaRtLpJTWMzpr/ORo77abSeyr+l9G55C6nEAMbmvfAp1aQluZxbJGw3nP9vz2brfeqlnGSY7tMwG14/f/mx2CzbWTzCV0bEWIHq/EYnaJ9oH0i9UDWteGZGDY2CuS8j7t9+9nydwGp5pylIYYnQDe1k0rS0Go9B7sADcYl9ehZeAZgCKH38Dz7fs3BwvWB3tzBLdlu6iXDU25TsXiSWvwm9noLNivVSwlkHiccf3q0tc6BjszZbszTXpxdiXGAuBKEAWpyNBzY1OHgaoXrDG5rg3naekrO7NTGBTaGsAHAgMBAAECggEAA4f5gMIxNtaJlBRZ0Vaotja+cXxAZg9EwUOmJGFYFmT3rDj8VZkzBad/Jtf8k7yo9L4rmvXKz/u3Y5p3aHvMjRG6/c6RnLP0EbiKg5w5q3W1gNCq9FCL2PZ0w9kIOxlbWq2Aa+8gNXrXSMjH8l3LsJh9ociW1rgWExkUyxU+1Caxn0jJNySZY0l7KvgUNoGeEE7lXSt5uWCkiioB4xh3mj4qPPDMwDhMd0ROzJRY6YO75DcEo0TNpMvuRJziBElGJ+sQSKJtpGdqwwlJmMQhp1JoH2aZ64RcbsnoGFaUI/ewehcE0e1lVBIISfZlRvoyMGIC/jNNZhar6Zd+eDP+VQKBgQDkzLw1UZzs+TpDtXPqsLQP9KZQWaCsXTkxpXO4srBHcvwsNGeqJeewjebynVYD9+Lf0LKNM3Y4adXfg91z+6G3s8B0OCqjXE1y8RTfQ2ZWD4/bD+6jGjRj5F1VpHtAV6X5U/rZIsyCXe4BIWuxLSqDUx/D4UphNjq3MBfNSx62YwKBgQClPD1XqZrx9QaQLadaNbixJHqETzOGyh9yA07QygZhHcv/D66US+gkXRRWQwxszb8Xvo+0U/YFe9rbM5gBwvnk/2Mn4GABp/zedaXVguk57e4CNo2LdrocpThn4pH06CRcNfVnw7ZoN+xti5Mraort9FBLdaSsqLqOjJzj0eyfDQKBgHYXfiT/BMIDOSFtbHD711RM7/KU8CtGyphnTz1LbVTTcnjWa5MUkWs9MrCSqKzPqxfePepHX4NqjOsawph0jlmku3bA7rD2mTr0V9OMlCtjSNrGdGNWySet3MaxaLQjCRWRVO3x2iCHnqqSt2TxbPnvU3vksmFOfivC9OYPGcEFAoGBAIgpBM+ZNtKNvFPRGygOPtzSl5cyAN7g7lqweSE6aVlcCX9sd8tCZdCZVyniZHnAbejuIlNSINjSAD5D3M4O0ogvH5F7pwGWYZ0by4UPNIuFEm2GcMttEd6LE9kfbnEnXtcRq+FO+KAW/WAj9SmAyW1et1AHaKTTAbB4FMzzbw59AoGBAJETXgqsnwOOKTRJFtn+LWKYLSaDR2al4UhBXKCSccYKJ9prWgcoITZq31FfjPYqWElHljQoca6bBtcar/+p2fFebMhFnR2wskAL2C4Np5kHWCyw0iW/gp1rophc+R7WisFFCNnUb17dxhq8vcM/wPXpYeXK83HFD8P57Phj7Lhs";
    //支付公钥
    string pay_pub_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAk63MHZwbk71zsuF6xYMSP61otU1QzEJZjk92DZ7WtTvY82kbS6SU1jM6a/zkaO+2m0nsq/pfRueQupxADG5r3wKdWkJbmcWyRsN5z/b89m633qpZxkmO7TMBteP3/5sdgs21k8wldGxFiB6vxGJ2ifaB9IvVA1rXhmRg2NgrkvI+7ffvZ8ncBqeacpSGGJ0A3tZNK0tBqPQe7AA3GJfXoWXgGYAih9/A8+37NwcL1gd7cwS3Zbuolw1NuU7F4klr8JvZ6CzYr1UsJZB4nHH96tLXOgY7M2W7M016cXYlxgLgShAFqcjQc2NTh4GqF6wxua4N52npKzuzUxgU2hrABwIDAQAB";
    //回调地址
    string payResultCallBackUrl = "";

    #region unity 访问方法
    public override void Init(string arg)
    {
        InitDataModel model = new InitDataModel()
        {
            appId = appId,
            merchantId = merchantId,
            merchantName = merchantName,
            pay_priv_key = pay_priv_key,
            pay_pub_key = pay_pub_key,
            payResultCallBackUrl = payResultCallBackUrl,
        };
        CallAndoridFunc("Init", JsonMapper.ToJson(model));
    }
    /// <summary>
    /// 检测更新的方法 
    /// </summary>
    public override void CheckUpdate()
    {
        CallAndoridFunc("CheckUpdate");
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
                rank = arg.roleLevel,
                role = arg.roleName,
                area = arg.realmName,
                sociaty = default(string),
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
        PayDataModel sendModel = new PayDataModel();
        sendModel.amount = string.Format("{0:F2}", arg.amount);//单位为元，5元 = 5.00
        sendModel.productDesc = arg.productDesc;
        sendModel.productName = arg.productName;
        sendModel.requesId = arg.productId + GetCurrentTimeMiss();//订单号

        //固定参数
        sendModel.applicationID = appId;
        sendModel.merchantId = merchantId;
        sendModel.merchantName = merchantName;
        sendModel.serviceCatalog = "X6";
        sendModel.sdkChannel = 3;

        sendModel.url = payResultCallBackUrl;
        sendModel.currency = "CNY";
        sendModel.country = "CN";
        sendModel.urlVer = "2";
        sendModel.extReserved = "侧保留字段，回调给服务器！";

        sendModel.sign = "将当前model发送给服务器签名返回的数据！";


        // 签名sign
        RsgSign(PaySignUtilgetStringForSign(sendModel), (sign) =>
        {
            sendModel.sign = sign;
            DebugLog("签名成功sign：" + sign);

            //TODO  将服务器生成的具体订单model发送给sdk
            CallAndoridFunc("PayOrder", JsonMapper.ToJson(sendModel));
            AndroidPlatSDKManager.Instance.ChangeSDKManagerPayOrderState(SDKManagerPayOrderState.PayOrder_CreateSuccess);
        });


    }


    /// <summary>
    /// 结束sdk
    /// </summary>
    public override void OnGameExit()
    {
        // TODO 华为没有退出sdk - 直接退出成功
        //CallAndoridFunc("OnGameExit");
        AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.ExitSuccess);
    }

    #endregion

    #region sdk 回调方法

    /// <summary>
    /// HW sdk特有的连接回调接口（默认在sdk内init后调用的），此接口回调后表示连接成功，可以进行检测更新接口
    /// </summary>
    public override void ContentCallBack(string arg)
    {
        DebugLog("连接成功！回调结果码：" + arg);
        //调用检测更新接口
        CheckUpdate();
        AndroidPlatSDKManager.Instance.ChangeSDKManagerState(SDKManagerState.InitSuccess);//连接成功后将状态设置为初始化成功！
    }

    /// <summary>
    /// 检测更新后的回调方法
    /// </summary>
    public override void CheckUpdateCallBack(string arg)
    {
        DebugLog("检查更新返回结果码：" + arg);
        //TODO 根据结果码执行更新？
        if (arg == "0")
        {
            DebugLog("不用更新！");
        }
        else
        {
            DebugLog("需要更新！");
        }

    }

    public override void SaveInfoCallBack(string arg)
    {
        DebugLog("保存角色信息后返回结果码：" + arg);
    }

    /// <summary>
    /// 登入回调函数如果用户首次登录华为帐号进行游戏，那么调用login接口时，将不会返回只有playId的结果（即isAuth为0的情况）,只会返回isAuth的结果。
    /// 强烈建议应用将对鉴权签名的验证放在服务器端进行，以保证验证结果的安全性。    
    /// </summary>
    public override void LoginCallBack(string arg)
    {
        var model = CheckCallBackArg<LoginCallBackModel>(arg);
        if (model == null)
        {
            return;
        }
        DebugLog("登录 回调码：" + model.rst + "  回调结果：" + model.result);
        //TODO 回调数据暂时未用
        if (model.result != null)
        {
            if (model.result.isAuth == 0)
            {
                DebugLog("sdk 快速登入成功！！");
                //TODO 

            }
            else if (model.result.isAuth == 1)
            {
                DebugLog("sdk 登入成功！准备将登入数据发给服务器进行验签！");
                //TODO 将数据发送给服务器进行验签
                SDKLoginCompleteData data = new SDKLoginCompleteData();
                data.result = true;
                data.arg = string.Format("{0}:{1}:{2}:{3}:{4}:{5}",
                    model.result.playerId,
                    model.result.displayName,
                    model.result.playerLevel,
                    model.result.isAuth,
                    model.result.ts,
                    model.result.gameAuthSign);
                AndroidPlatSDKManager.Instance.ChangeSDKManagerLoginState((int)SDKManagerLoginState.Login_Success, data);
            }
        }
    }

    /// <summary>
    /// 订单结果回调
    /// </summary>
    public override void PayResultCallBack(string arg)
    {
        var model = CheckCallBackArg<PayResultModel>(arg);
        if (model == null)
            return;
        if (model.checkRst)
        {
            DebugLog("安卓部分 订单验签成功：回调结果码：" + model.retCode);
        }
        else
        {
            DebugLog("安卓部分 订单验签失败：回调结果码：" + model.retCode);
        }
        //TODO 发送数据到服务器进行二次验证

    }

    #endregion

    #region 私有方法
    private string PaySignUtilgetStringForSign(PayDataModel model)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();


        param.Add(HwPayConstant.KEY_MERCHANTID, model.merchantId);
        param.Add(HwPayConstant.KEY_APPLICATIONID, model.applicationID);
        param.Add(HwPayConstant.KEY_PRODUCTNAME, model.productName);
        param.Add(HwPayConstant.KEY_PRODUCTDESC, model.productDesc);
        param.Add(HwPayConstant.KEY_REQUESTID, model.requesId);

        param.Add(HwPayConstant.KEY_AMOUNT, model.amount);
        param.Add(HwPayConstant.KEY_PARTNER_IDS, "");
        param.Add(HwPayConstant.KEY_CURRENCY, model.currency);
        param.Add(HwPayConstant.KEY_COUNTRY, model.country);
        param.Add(HwPayConstant.KEY_URL, model.url);

        param.Add(HwPayConstant.KEY_URLVER, model.urlVer);
        param.Add(HwPayConstant.KEY_EXPIRETIME, "");
        param.Add(HwPayConstant.KEY_SDKCHANNEL, model.sdkChannel.ToString());

        return GetNoSign(param);
    }

    private string GetNoSign(Dictionary<string, string> param)
    {
        StringBuilder content = new StringBuilder();
        bool isFirstParm = true;

        var dicSort = from objDic in param orderby objDic.Key ascending select objDic;//按照升序排序
        foreach (var dic in dicSort)
        {
            if (!string.IsNullOrEmpty(dic.Value))
            {
                content.Append((isFirstParm ? "" : "&") + dic.Key + "=" + dic.Value);
                isFirstParm = false;
            }
        }

        return content.ToString();
    }

    /// <summary>
    /// 请求服务器签名验证
    /// </summary>
    private void RsgSign(string singString, System.Action<string> onComplete)
    {
        //TODO 与服务器进行交互验证
        onComplete(singString);
    }

    #endregion

}
#region HW数据结构
namespace HWSDKData
{
    /// <summary>
    ///初始化model 参数
    /// </summary>
    [System.Serializable]
    public class InitDataModel
    {
        public string appId;
        public string merchantId;
        public string merchantName;
        public string pay_priv_key;
        public string pay_pub_key;
        public string payResultCallBackUrl;
    }

    /// <summary>
    ///保存角色信息的数据model 
    /// </summary>
    [System.Serializable]
    public class SaveRoleDataModel
    {
        public string rank;//等级
        public string role;//角色名称
        public string area;//游戏区服，可不设置此属性
        public string sociaty;//游戏公会，可不设置此属性
    }

    /// <summary>
    /// 支付申请model 
    /// </summary>
    [System.Serializable]
    public class PayDataModel
    {
        public string productDesc;// 商品内容
        public string productName;// 商品编号
        public string requesId;// 支付订单号
        public string amount;// 商品价格 格式 20 = 20.00元商品金额，商品所要支付金额。此金额将会在支付时显示给用户确认。1、请保留小数点后两位，如20.00。如果不按照格式传入金额，会导致支付失败。2、部分币种amount只能设置为整数金额，例如只能是5.00，不能设置为5.02。涉及币种包括：BEF、XOF、XAF、XPF、KMF、GRD、GNF、HUF、IDR、JPY、LUF、MGA、MGA、MGF、PYG、PTE、RWF、KRW、ESP、TRL、VND

        //独自参数
        public string applicationID;//应用id
        public string merchantId;//商户id
        public string serviceCatalog;//商品类型 游戏设置为"X6"，应用设置为"X5"
        public string merchantName;//商户名称
        public int sdkChannel;//渠道信息  游戏设置为3，应用设置为1

        public string url;//支付结果回调地址
        public string currency;//币种，用于支付的币种 如果不填写则默认为CNY。
        public string country;//国家码 如果不填写则默认为CN
        public string urlVer;//支付结果回调版本。该值可以不传，如果传值则固定值传2。注意：该字段拼接至待签名字段序列时，应修改为 urlver, 例如urlver=2。
        public string extReserved;//	商户侧保留信息 若该字段有值，在华为支付服务器回调接口中原样返回

        public string sign;//签名数据（需要服务器签名过来）
    }

    /// <summary>
    /// 登录回调model
    /// </summary>
    [System.Serializable]
    public class LoginCallBackModel
    {
        public int rst;
        public GameUserData result;
    }

    [System.Serializable]
    public class GameUserData
    {
        /// <summary>
        /// 帐户ID。如果游戏不需要对华为帐号的登录结果进行鉴权，那么当SDK返回playerId的时候就可以使用该值进行游戏。
        /// </summary>
        public string playerId;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string displayName;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int playerLevel;
        /// <summary>
        /// 时间戳 - 用于鉴权签名校验
        /// </summary>
        public string ts;
        /// <summary>
        /// 鉴权签名
        /// </summary>
        public string gameAuthSign;

        /// <summary>
        /// 当isAuth为1的时候，应用需要校验返回的参数鉴权签名。
        /// </summary>
        public int isAuth;
    }

    /// <summary>
    /// 非托管商品结果model
    /// </summary>
    [System.Serializable]
    public class PayResultModel
    {
        public int retCode;
        public PayResultInfo resultInfo;
        public bool checkRst;// 验签结果
    }

    [System.Serializable]
    public class PayResultInfo
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int returnCode;
        /// <summary>
        /// 信息描述
        /// </summary>
        public string errMsg;
        /// <summary>
        /// 华为支付平台订单号
        /// </summary>
        public string orderID;
        /// <summary>
        /// 金额
        /// </summary>
        public string amount;
        /// <summary>
        /// 币种
        /// </summary>
        public string currency;
        /// <summary>
        /// 国家码
        /// </summary>
        public string country;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string time;
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string requestId;
        /// <summary>
        /// 商户名称，来源：开发者注册的公司名
        /// </summary>
        public string userName;
        /// <summary>
        /// 以上数据的签名信息
        /// </summary>
        public string sign;
    }
    public class HwPayConstant
    {
        public static string KEY_MERCHANTID = "merchantId";
        public static string KEY_USER_ID = "userID";
        public static string KEY_USER_NAME = "userName";
        public static string KEY_MERCHANTNAME = "merchantName";
        public static string KEY_APPLICATIONID = "applicationID";
        public static string KEY_PRODUCTNAME = "productName";
        public static string KEY_PRODUCTDESC = "productDesc";
        public static string KEY_REQUESTID = "requestId";
        public static string KEY_AMOUNT = "amount";
        public static string KEY_CURRENCY = "currency";
        public static string KEY_COUNTRY = "country";
        public static string KEY_PARTNER_IDS = "partnerIDs";
        public static string KEY_URL = "url";
        public static string KEY_NOTIFY_URL = "notifyUrl";
        public static string KEY_URLVER = "urlver";
        public static string KEY_SDKCHANNEL = "sdkChannel";
        public static string KEY_EXTRESERVED = "extReserved";
        public static string KEY_SERVICECATALOG = "serviceCatalog";
        public static string KEY_VALIDTIME = "validTime";
        public static string KEY_EXPIRETIME = "expireTime";
        public static string KEY_SITE_ID = "siteId";
        public static string KEY_GFTAMT = "gftAmt";
        public static string KEY_INGFTAMT = "ingftAmt";
        public static string KEY_INSIGN = "inSign";
        public static string KEY_SIGN = "sign";
        public static string KEY_TRADE_TYPE = "tradeType";
        public static string KEY_PRODUCT_NO = "productNo";
        public static string KEY_SIGN_TYPE = "signType";
    }


}
#endregion
