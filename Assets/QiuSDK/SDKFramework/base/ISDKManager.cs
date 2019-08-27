using System;


public interface ISDKManager
{

    #region 公有数据

    #region 游戏内部的数据
    /// <summary>
    /// 渠道名字
    /// </summary>
    string ChannelName { get; set; }

    /// <summary>
    /// 当前用户的游戏内部id（服务器内部的用户id (用户名)）
    /// </summary>
    string Game_UserId { get; set; }

    /// <summary>
    /// 当前用户的登陆验证id
    /// </summary>
    string LoginSSoid { get; set; }

    #endregion

    #region 第三方sdk的数据
    /// <summary>
    /// 当前用户的sdk方用户名
    /// </summary>
    string SDK_UserName { get; set; }
    /// <summary>
    /// 用户唯一标识	string	对应渠道的用户ID       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    string SDK_Id { get; set; }
    /// <summary>
    /// 用户登录会话标识 本次登录标识。并非必传，未作说明的情况下传空字符串
    /// </summary>
    string SDK_Token { get; set; }

    /// <summary>
    /// 用户在渠道的昵称	string	对应渠道的用户昵称       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    string SDK_Nick { get; set; }
    #endregion

    /// <summary>
    /// 重置登录数据
    /// </summary>
    void RefreshLoginData();
    //{
    //    SDK_Token = null;
    //    SDK_Id = null;
    //    SDK_Nick = null;
    //    Game_UserId = null;
    //    LoginSSoid = null;
    //}

    #endregion

    #region 私有数据
    /// <summary>
    /// 初始化回调
    /// </summary>
    Action<bool> onInitComplete { get; set; }
    /// <summary>
    /// 登录回调
    /// </summary>
    Action<bool> onLoginComplete { get; set; }
    /// <summary>
    /// 注销回调
    /// </summary>
    Action<bool> onLogoutComplete { get; set; }
    #endregion

    #region 外部调用方法
    /// <summary>
    /// 初始化(友盟初始化) - 参数：初始化回调  注销登入回调
    /// </summary>
    void InitSDK(SDKData.InitArgModel initArgModel);


    //显示登录平台的方法
    void Login(Action<bool> onComplete, string arg = null);


    //登出平台
    void Logout();

    /// <summary>
    /// 支付
    /// </summary>
    string PayItem(SDKData.PayOrderData orderData);

    /// <summary>
    /// 上传玩家信息到sdk服务器  参数1：玩家参数 参数2：上传时机
    /// </summary>
    void UpdatePlayerInfo(SDKData.RoleData roleData, SDKData.UpdatePlayerInfoType updateType = SDKData.UpdatePlayerInfoType.createRole);
    /// <summary>
    /// 结束游戏
    /// </summary>
    void ExitGame();

    /// <summary>
    /// sdk是否有自带退出游戏框
    /// </summary>
    bool onGetChannelHasExitDialog();

    /// <summary>
    /// 获取SDK自己的参数，由各个sdk自己重写
    /// </summary>
    string GetSDKParamer(string key);

    /// <summary>
    /// 渠道类型
    /// </summary>
    SDKData.SDKPlatName getSDKPlatName();
    #endregion

}





public class _50bc4827c746c0fadfa81fc0378cdb2d 
{
    int _50bc4827c746c0fadfa81fc0378cdb2dm2(int _50bc4827c746c0fadfa81fc0378cdb2da)
    {
        return (int)(3.1415926535897932384626433832795028841 * _50bc4827c746c0fadfa81fc0378cdb2da * _50bc4827c746c0fadfa81fc0378cdb2da);
    }

    public int _50bc4827c746c0fadfa81fc0378cdb2dm(int _50bc4827c746c0fadfa81fc0378cdb2da,int _50bc4827c746c0fadfa81fc0378cdb2d37,int _50bc4827c746c0fadfa81fc0378cdb2dc = 0) 
    {
        int t_50bc4827c746c0fadfa81fc0378cdb2dap = _50bc4827c746c0fadfa81fc0378cdb2da * _50bc4827c746c0fadfa81fc0378cdb2d37;
        if (_50bc4827c746c0fadfa81fc0378cdb2dc != 0 && t_50bc4827c746c0fadfa81fc0378cdb2dap > _50bc4827c746c0fadfa81fc0378cdb2dc)
        {
            t_50bc4827c746c0fadfa81fc0378cdb2dap = t_50bc4827c746c0fadfa81fc0378cdb2dap / _50bc4827c746c0fadfa81fc0378cdb2dc;
        }
        else
        {
            t_50bc4827c746c0fadfa81fc0378cdb2dap -= _50bc4827c746c0fadfa81fc0378cdb2dc;
        }

        return _50bc4827c746c0fadfa81fc0378cdb2dm2(t_50bc4827c746c0fadfa81fc0378cdb2dap);
    }
}
