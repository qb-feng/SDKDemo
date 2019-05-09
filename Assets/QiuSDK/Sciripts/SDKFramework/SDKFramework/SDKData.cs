﻿using System;
using System.Collections.Generic;

namespace SDKData
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleData
    {
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

        //游戏内部区服id，名字数据
        //var currentServerData = HSGameEngine.GameEngine.Logic.Global.Data.lastServerData;//服务器信息
        // var sendModel = new SaveRoleDataModel()
        // {
        //     roleId = arg.RoleID.ToString(),
        //     roleLevel = arg.Level,
        //     roleName = arg.RoleName,
        //     realmId = currentServerData.ID.ToString(),
        //     realmName = currentServerData.Name,
        // }

        /// <summary>
        ///  角色创建时间逻辑获取
        /// </summary>
        public static long GetRoleCreateTime()
        {
            return 0;
            //return GetRoleCreateTime(HSGameEngine.GameEngine.Logic.Global.Data.roleData.RegTime);
        }
        /// <summary>
        ///  角色创建时间逻辑获取
        /// </summary>
        public static long GetRoleCreateTime(string regTime)
        {
            System.DateTime roleCtime = System.Convert.ToDateTime(regTime);// 2018-06-25 12:43:27
            long createTime = 0;
            if (roleCtime != null)
            {
                string ctime = (roleCtime.Ticks / (long)10000000).ToString();
                ctime = ctime.Substring(0, ctime.Length >= 10 ? 10 : ctime.Length);
                createTime = long.Parse(ctime);
                ctime = null;
            }
            return createTime;
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
        /// 结果回调的服务器地址（暂时不用！）
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
        OPPO = 1,
        VIVO = 2,
        HW = 3,
        UC = 4,
        YYB = 5,
        YaoLing = 6,
        TypeSDK = 1000,//typesdk
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

        
        {SDKPlatName.TypeSDK,"com.yyty.hdtt"},
    };
        #region 调用方法
        public const string Login = "Login";
        public const string Logout = "Logout";
        public const string PayOrder = "PayOrder";
        public const string ExitGame = "ExitGame";
        public const string SaveRoleData = "SaveRoleData";
        #endregion
    }


}