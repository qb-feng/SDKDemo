using System.Collections;
using System.Collections.Generic;

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
    YaoLing = 116,
    TypeSDK = 100001,
}