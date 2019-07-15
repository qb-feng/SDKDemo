using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SDKIOSFunction : MonoBehaviour
{

    #region 需要调用ios的方法

#if UNITY_IOS
    /// <summary>
    /// 初始化方法
    /// </summary>
    [DllImport("__Internal")]
    public static extern void sdkmanagerinit();

    /// <summary>
    /// 登入
    /// </summary>
    [DllImport("__Internal")]
    public static extern void sdkmanagerlogin();

    /// <summary>
    /// 注销
    /// </summary>
    [DllImport("__Internal")]
    public static extern void sdkmanagerlogout();

    /// <summary>
    /// 保存角色信息 角色id 角色名字  服务器id 角色等级
    /// </summary>
    [DllImport("__Internal")]
    public static extern void sdkmanagersavedata(string roleid,string rolename,string serverid,string rolelevel,string servername);

    /// <summary>
    /// 充值  订单号 角色名字 服务器id 金额 商品id（正版填苹果内购商品id，越狱版填nil）商品名称 扩展参数  游戏名字
    /// </summary>
    [DllImport("__Internal")]
    public static extern void sdkmanagerpayorder(string orderid,string rolename,string serverid,string amount,string productid,string productname,string extra,string gamename,string sername,string rlevel);


#endif

    #endregion
}

