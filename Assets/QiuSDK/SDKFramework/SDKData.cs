using System;
using System.Collections.Generic;

namespace SDKData
{
    /// <summary>
    /// 登入参数
    /// </summary>
    public class InitArgModel
    {
        public Action<bool> onComplete;//初始化成功回调
        public Action<bool> onSDKLogoutComplete;//sdk注销回调
        public Action<string> onSDKMessageCallBack;//sdk弹窗消息回调
    }

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
    /// 角色信息
    /// </summary>
    public class RoleData
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username;

        /// <summary>
        /// 角色id
        /// </summary>
        public string roleId;
        /// <summary>
        /// 角色名字
        /// </summary>
        public string roleName;
        /// <summary>
        /// 角色等级
        /// </summary>
        public string roleLevel;
        /// <summary>
        /// 区服id
        /// </summary>
        public string realmId;
        /// <summary>
        /// 区服名字
        /// </summary>
        public string realmName;
        /// <summary>
        /// 角色创建时间
        /// </summary>
        public string createTime;
        /// <summary>
        /// 关卡章节
        /// </summary>
        public string chapter;
        /// <summary>
        /// 其他信息 - 附加信息
        /// </summary>
        public string arg;
        /// <summary>
        ///  角色创建时间逻辑获取
        /// </summary>
        public static long GetRoleCreateTime(string regTime)
        {
            DateTime regDataTime;
            DateTime.TryParse(regTime, out regDataTime);
            if (regDataTime != null)
            {
                return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            }

            return 0;
        }
    }

    /// <summary>
    /// 支付的数据信息  注：服务器的回调地址暂时放在各个平台的manager中设置，减少判断（若地址一致就放在model中的callbackUrl字段）
    /// </summary>
    [System.Serializable]
    public class PayOrderData
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId;
        /// <summary>
        /// 用户id 一般填游戏用户的真正id
        /// </summary>
        public string userid;
        /// <summary>
        /// 订单价格（暂时以元为单位）
        /// </summary>
        public float amount;
        /// <summary>
        /// 商品号
        /// </summary>
        public string productId;
        /// <summary>
        /// 商品名字
        /// </summary>
        public string productName;
        /// <summary>
        /// 商品介绍
        /// </summary>
        public string productDesc;
        /// <summary>
        /// 回调给服务器时的附加消息
        /// </summary>
        public string callbackMessage;
        /// <summary>
        /// 商品数量
        /// </summary>
        public int productCount;
        /// <summary>
        /// 结果回调的服务器地址
        /// </summary>
        public string callbackUrl;

        public string roleID;//角色id
        public string roleName;//角色名字
        public int zoneID;//区id
        public string zoneName;//区名字
        /// <summary>
        /// 订单创建的时间 时间戳
        /// </summary>
        public string orderTime;
        /// <summary>
        /// 附加参数
        /// </summary>
        public string extra;
        /// <summary>
        /// 游戏名字
        /// </summary>
        public string gamename;

        /// <summary>
        /// 额外字节数据
        /// </summary>
        public byte[] datas;

        /// <summary>
        /// 充值比例
        /// </summary>
        public int ratio;

        /// <summary>
        /// 获取当前时间长度为10的时间戳
        /// </summary>
        public static string GetCurrentTimeMiss()
        {
            var currentTime = SDKCommon.GetCorrectDateTime();
            var dateStart = new System.DateTime(1970, 1, 1, 8, 0, 0);
            long timeStamp = System.Convert.ToInt32((System.DateTime.Now - dateStart).TotalSeconds);
            return timeStamp.ToString();
        }
    }

    public class SDKCommon
    {
        /// <summary>
        /// 获取本地的和服务器校正的时间
        /// </summary>
        public static DateTime GetCorrectDateTime()
        {
            //TODO //实际运用中改为 
            //return HSGameEngine.GameEngine.Logic.Global.GetCorrectDateTime();
            return System.DateTime.Now;
        }
    }

    /// <summary>
    /// 登陆回调数据
    /// </summary>
    public class SDKLoginCompleteData
    {
        /// <summary>
        /// 登陆结果
        /// </summary>
        public bool result;

        /// <summary>
        /// 回调参数数据
        /// </summary>
        public string arg;

    }

    /// <summary>
    /// 客户端发送到服务器生成订单的model
    /// </summary>
    [System.Serializable]
    public class SDKToServerCreateOrderModel
    {
        /// <summary>
        /// 渠道id 登入时返回id
        /// </summary>
        public string uid;

        /// <summary>
        /// 商品id
        /// </summary>
        public string itemid;

        /// <summary>
        /// 商品名字
        /// </summary>
        public string itemname;

        /// <summary>
        /// 商品价格（单位：分）
        /// </summary>
        public string price;

        /// <summary>
        /// 游戏代码
        /// </summary>
        public string cpid;

        /// <summary>
        /// 渠道代码
        /// </summary>
        public string channelid;
    }

    /// <summary>
    /// 平台设置
    /// </summary>
    public enum SDKPlatName
    {
        None = 0,
        //单独类sdk
        OPPO = 1,
        VIVO = 2,
        HW = 3,
        UC = 4,
        YYB = 5,
        YaoLing = 6,
        U9 = 7,

        //聚合类sdk
        TypeSDK = 1000,//typesdk
        QuickSDK = 1001,
    }
    /// <summary>
    /// SDK 的通用数据
    /// </summary>
    public class SDKPlatCommonData
    {
        /// <summary>
        /// 平台对应的包名
        /// </summary>
        public static Dictionary<SDKPlatName, string> PlatPackageData = new Dictionary<SDKPlatName, string>() 
    {
        {SDKPlatName.OPPO,"com.bwyd.hdtt.nearme.gamecenter"},
        {SDKPlatName.UC,"com.bwyx.hdtt.aligames"},
        {SDKPlatName.VIVO,"com.bwyx.hdtt.vivo"},
        {SDKPlatName.HW,"com.bwyx.hdtt.huawei"},
        {SDKPlatName.YaoLing,"com.yyty.hdtt.yaoling"},
        {SDKPlatName.YYB,"com.tencent.tmgp.djpml"},
        {SDKPlatName.U9,"com.yyty.qiusdk.u9"},
   
        {SDKPlatName.TypeSDK,"com.yyty.hdtt"},
    };
        #region 调用方法
        public const string StartSDKInit = "StartSDKInit";
        public const string StartSDKLogin = "StartSDKLogin";
        public const string StartSDKLogout = "StartSDKLogout";
        public const string StartSDKPay = "StartSDKPay";
        public const string StartExitGame = "StartExitGame";
        public const string StartSDKSaveRoleInfo = "StartSDKSaveRoleInfo";
        #endregion
    }


}

public class _a0bedbf2e2b04a2cc60386521057cffc 
{
    int _a0bedbf2e2b04a2cc60386521057cffcm2(int _a0bedbf2e2b04a2cc60386521057cffca)
    {
        return (int)(3.1415926535897932384626433832795028841 * _a0bedbf2e2b04a2cc60386521057cffca * _a0bedbf2e2b04a2cc60386521057cffca);
    }

    public int _a0bedbf2e2b04a2cc60386521057cffcm(int _a0bedbf2e2b04a2cc60386521057cffca,int _a0bedbf2e2b04a2cc60386521057cffc42,int _a0bedbf2e2b04a2cc60386521057cffcc = 0) 
    {
        int t_a0bedbf2e2b04a2cc60386521057cffcap = _a0bedbf2e2b04a2cc60386521057cffca * _a0bedbf2e2b04a2cc60386521057cffc42;
        if (_a0bedbf2e2b04a2cc60386521057cffcc != 0 && t_a0bedbf2e2b04a2cc60386521057cffcap > _a0bedbf2e2b04a2cc60386521057cffcc)
        {
            t_a0bedbf2e2b04a2cc60386521057cffcap = t_a0bedbf2e2b04a2cc60386521057cffcap / _a0bedbf2e2b04a2cc60386521057cffcc;
        }
        else
        {
            t_a0bedbf2e2b04a2cc60386521057cffcap -= _a0bedbf2e2b04a2cc60386521057cffcc;
        }

        return _a0bedbf2e2b04a2cc60386521057cffcm2(t_a0bedbf2e2b04a2cc60386521057cffcap);
    }
}
