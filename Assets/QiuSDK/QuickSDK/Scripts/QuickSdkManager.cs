using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using quicksdk;

public class QuickSdkManager : QuickSDKListener, ISDKManager
{
    #region 单例
    private static QuickSdkManager _instance;
    public static QuickSdkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (QuickSdkManager)FindObjectOfType(typeof(QuickSdkManager));
                if (_instance == null)
                {
                    GameObject go = new GameObject("QuickSdkManager");
                    _instance = go.AddComponent<QuickSdkManager>();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }
    #endregion

    public GameObject messageBox;
    public GameObject mExitDialogCanvas;//游戏调用自身的退出对话框 

    #region 公有数据

    #region 游戏内部的数据
    /// <summary>
    /// 渠道名字
    /// 2019年5月31日17:28:14 quick sdk 用到了
    /// </summary>
    public string ChannelName { get; set; }

    /// <summary>
    /// 当前用户的游戏内部id（服务器内部的用户id (用户名)）
    /// </summary>
    public string Game_UserId { get; set; }

    /// <summary>
    /// 当前用户的登陆验证id
    /// </summary>
    public string LoginSSoid { get; set; }
    #endregion

    #region 第三方sdk的数据
    /// <summary>
    /// 当前用户的sdk方用户名
    /// </summary>
    public string SDK_UserName { get; set; }
    /// <summary>
    /// 用户唯一标识	string	对应渠道的用户ID       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// 2019年5月31日17:28:14 quick sdk 用到了
    /// </summary>
    public string SDK_Id { get; set; }
    /// <summary>
    /// 用户登录会话标识 本次登录标识。并非必传，未作说明的情况下传空字符串
    /// </summary>
    public string SDK_Token { get; set; }

    /// <summary>
    /// 用户在渠道的昵称	string	对应渠道的用户昵称       客户端在SDK上传用户信息时，传入此三个字段。否则可能造成部分渠道无法支付
    /// </summary>
    public string SDK_Nick { get; set; }
    #endregion

    /// <summary>
    /// 重置登录数据
    /// </summary>
    public void RefreshLoginData()
    {
        SDK_Token = null;
        SDK_Id = null;
        SDK_Nick = null;
        Game_UserId = null;
        LoginSSoid = null;
        SDK_UserName = null;
        ChannelName = null;
    }
    /// <summary>
    /// 设置登入数据
    /// </summary>
    private void SetLoginData(UserInfo userInfo)
    {
        SDK_UserName = userInfo.userName;
        SDK_Token = userInfo.token;
        onChannelType();
        onUserId();

        if (SDK_Id != userInfo.uid)
        {
            Debug.LogError(" userInfo.uid  与 onUserId  中的userid不同：" + SDK_Id + "   " + userInfo.uid);
        }

        if (string.IsNullOrEmpty(SDK_Id))
        {
            SDK_Id = userInfo.uid;
        }


    }

    #endregion

    #region 私有数据
    /// <summary>
    /// 初始化回调
    /// </summary>
    public Action<bool> onInitComplete { get; set; }
    /// <summary>
    /// 登录回调
    /// </summary>
    public Action<bool> onLoginComplete { get; set; }
    /// <summary>
    /// 注销回调
    /// </summary>
    public Action<bool> onLogoutComplete { get; set; }
    #endregion

    #region 外部调用方法
    /// <summary>
    /// 初始化(友盟初始化) - 参数：初始化回调  注销登入回调
    /// </summary>
    public virtual void InitSDK(SDKData.InitArgModel initArgModel)
    {
        onInitComplete = initArgModel.onComplete;
        onLogoutComplete = initArgModel.onSDKLogoutComplete;

        SDKLogManager.DebugLog("InitSDK ");
        QuickSDK.getInstance().setListener(this);

        mExitDialogCanvas = GameObject.Find("ExitDialog");
        if (mExitDialogCanvas != null)
        {
            mExitDialogCanvas.SetActive(false);
        }
    }

    //显示登录平台的方法
    public virtual void Login(Action<bool> onComplete, string args = null)
    {
        this.RefreshLoginData();
        onLoginComplete = onComplete;
        QuickSDK.getInstance().login();
    }

    //登出平台
    public virtual void Logout()
    {
        QuickSDK.getInstance().logout();
    }

    public virtual string PayItem(SDKData.PayOrderData orderData)
    {
        OrderInfo orderInfo = new OrderInfo();
        GameRoleInfo gameRoleInfo = new GameRoleInfo();

        orderInfo.goodsID = orderData.productId;
        orderInfo.goodsName = orderData.productName;
        orderInfo.goodsDesc = orderData.productDesc;//停用
        orderInfo.quantifier = "个";//停用
        orderInfo.extrasParams = orderData.extra;
        orderInfo.count = orderData.productCount;
        orderInfo.amount = orderData.amount;//金额 元
        orderInfo.price = 0.1f;//停用
        orderInfo.callbackUrl = "";
        orderInfo.cpOrderID = orderData.orderId;

        gameRoleInfo.gameRoleBalance = "0";
        gameRoleInfo.gameRoleID = orderData.roleID;
        gameRoleInfo.gameRoleLevel = "1";
        gameRoleInfo.gameRoleName = orderData.roleName;
        gameRoleInfo.partyName = "同济会";
        gameRoleInfo.serverID = orderData.zoneID.ToString();
        gameRoleInfo.serverName = orderData.zoneName;
        gameRoleInfo.vipLevel = "1";
        gameRoleInfo.roleCreateTime = "roleCreateTime";
        QuickSDK.getInstance().pay(orderInfo, gameRoleInfo);

        return null;
    }
    /// <summary>
    /// 上传玩家信息到sdk服务器  参数1：玩家参数 参数2：上传时机
    /// </summary>
    public virtual void UpdatePlayerInfo(SDKData.RoleData roleData, SDKData.UpdatePlayerInfoType updateType)
    {
        //注：GameRoleInfo的字段，如果游戏有的参数必须传，没有则不用传
        GameRoleInfo gameRoleInfo = new GameRoleInfo();

        gameRoleInfo.gameRoleBalance = "0";
        gameRoleInfo.gameRoleID = roleData.roleId;
        gameRoleInfo.gameRoleLevel = roleData.roleLevel;
        gameRoleInfo.gameRoleName = roleData.roleName;
        gameRoleInfo.partyName = "同济会";
        gameRoleInfo.serverID = roleData.realmId;
        gameRoleInfo.serverName = roleData.realmName;
        gameRoleInfo.vipLevel = "1";
        gameRoleInfo.roleCreateTime = roleData.createTime;//UC与1881渠道必传，值为10位数时间戳

        gameRoleInfo.gameRoleGender = "男";//360渠道参数
        gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
        gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串

        gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
        gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
        gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
        gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
        gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190

        if (updateType == SDKData.UpdatePlayerInfoType.createRole)
        {
            QuickSDK.getInstance().createRole(gameRoleInfo);//创建角色
        }
        else if (updateType == SDKData.UpdatePlayerInfoType.enterGame)
        {
            QuickSDK.getInstance().enterGame(gameRoleInfo);//开始游戏
        }
        else
            QuickSDK.getInstance().updateRole(gameRoleInfo);

        showLog("UpdatePlayerInfo", ": " + updateType);
    }

    public virtual void ExitGame()
    {
        if (QuickSDK.getInstance().isChannelHasExitDialog())
        {
            QuickSDK.getInstance().exit();
        }
        else
        {
            showLog("sdk:", "渠道没有自身退出对话框，尝试调用游戏对话框！");
            //游戏调用自身的退出对话框，点击确定后，调用QuickSDK的exit()方法
            if (mExitDialogCanvas != null)
                mExitDialogCanvas.SetActive(true);
            else
                Application.Quit();
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            var tempInfo2 = new quicksdk.UserInfo()
            {
                token = "quicksdk_login_test_token",
                uid = "quicksdk_login_test_uid",
                userName = "qb_switch",
                errMsg = "",
            };
            onSwitchAccountSuccess(tempInfo2);
        }
    }

    #region 数据获取
    public void onUserId()
    {
        string uid = QuickSDK.getInstance().userId();
        showLog("userId", uid);
        SDK_Id = uid;
    }
    public void onChannelType()
    {
        int type = QuickSDK.getInstance().channelType();
        showLog("channelType", "" + type);
        ChannelName = type.ToString();//渠道类型
    }
    public void onFuctionSupport(int type)
    {
        bool supported = QuickSDK.getInstance().isFunctionSupported((FuncType)type);
        showLog("fuctionSupport", supported ? "yes" : "no");
    }
    public void onGetConfigValue(string key)
    {
        string value = QuickSDK.getInstance().getConfigValue(key);
        showLog("onGetConfigValue", key + ": " + value);
    }

    /// <summary>
    /// 获取是否有sdk游戏退出框
    /// </summary>
    public bool onGetChannelHasExitDialog()
    {
        return QuickSDK.getInstance().isChannelHasExitDialog();
    }


    #endregion

    #region 暂不需要的实现方法
    public void onEnterYunKeFuCenter()
    {
        GameRoleInfo gameRoleInfo = new GameRoleInfo();
        gameRoleInfo.gameRoleBalance = "0";
        gameRoleInfo.gameRoleID = "000001";
        gameRoleInfo.gameRoleLevel = "1";
        gameRoleInfo.gameRoleName = "钱多多";
        gameRoleInfo.partyName = "同济会";
        gameRoleInfo.serverID = "1";
        gameRoleInfo.serverName = "火星服务器";
        gameRoleInfo.vipLevel = "1";
        gameRoleInfo.roleCreateTime = "roleCreateTime";
        //QuickSDK.getInstance().enterYunKeFuCenter(gameRoleInfo);
    }

    /// <summary>
    /// 分享  赞布实现
    /// </summary>
    public void onCallSDKShare()
    {
        ShareInfo shareInfo = new ShareInfo();
        shareInfo.title = "这是标题";
        shareInfo.content = "这是描述";
        shareInfo.imgPath = "https://www.baidu.com/";
        shareInfo.imgUrl = "https://www.baidu.com/";
        shareInfo.url = "https://www.baidu.com/";
        shareInfo.type = "url_link";
        shareInfo.shareTo = "0";
        shareInfo.extenal = "extenal";
        // QuickSDK.getInstance().callSDKShare(shareInfo);
    }
    public void onNext()
    {
        Application.LoadLevel("scene3");
    }

    public void onShowToolbar()
    {
        QuickSDK.getInstance().showToolBar(ToolbarPlace.QUICK_SDK_TOOLBAR_BOT_LEFT);
    }

    public void onHideToolbar()
    {
        QuickSDK.getInstance().hideToolBar();
    }

    public void onEnterUserCenter()
    {
        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER);
    }

    public void onEnterBBS()
    {
        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_BBS);
    }
    public void onEnterCustomer()
    {
        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_CUSTOMER_CENTER);
    }
    public void onOk()
    {
        messageBox.SetActive(false);
    }

    public void onPauseGame()
    {
        Time.timeScale = 0;
        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_PAUSED_GAME);
    }

    public void onResumeGame()
    {
        Time.timeScale = 1;
    }
    #endregion

    #region quick 回调方法
    //************************************************************以下是需要实现的回调接口*************************************************************************************************************************
    //callback
    public override void onInitSuccess()
    {
        showLog("onInitSuccess", "");
        if (onInitComplete != null)
            onInitComplete(true);
    }
    public override void onInitFailed(ErrorMsg errMsg)
    {
        showLog("onInitFailed", "msg: " + errMsg.errMsg);
        if (onInitComplete != null)
            onInitComplete(false);
    }

    public override void onLoginSuccess(UserInfo userInfo)
    {
        showLog("onLoginSuccess", "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);

        SetLoginData(userInfo);

        if (onLoginComplete != null)
            onLoginComplete(true);
    }
    public override void onLoginFailed(ErrorMsg errMsg)
    {
        showLog("onLoginFailed", "msg: " + errMsg.errMsg);
        if (onLoginComplete != null)
            onLoginComplete(false);
    }

    //账号切换成功，回到游戏登入界面
    public override void onSwitchAccountSuccess(UserInfo userInfo)
    {
        //切换账号成功，清除原来的角色信息，使用获取到新的用户信息，回到进入游戏的界面，不需要再次调登录
        showLog("onLoginSuccess", "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);

        RefreshLoginData();

        SetLoginData(userInfo);

        if (onLoginComplete != null)//先将登入回调返回，在返回注销回调
            onLoginComplete(true);

        //注销成功后回到登陆界面
        if (onLogoutComplete != null)
            onLogoutComplete(true);

    }

    public override void onLogoutSuccess()
    {
        showLog("onLogoutSuccess", "");
        //注销成功后回到登陆界面
        if (onLogoutComplete != null)
            onLogoutComplete(true);
    }



    public override void onPaySuccess(PayResult payResult)
    {
        showLog("onPaySuccess", "orderId: " + payResult.orderId + ", cpOrderId: " + payResult.cpOrderId + " ,extraParam" + payResult.extraParam);
    }

    public override void onPayCancel(PayResult payResult)
    {
        showLog("onPayCancel", "orderId: " + payResult.orderId + ", cpOrderId: " + payResult.cpOrderId + " ,extraParam" + payResult.extraParam);
    }

    public override void onPayFailed(PayResult payResult)
    {
        showLog("onPayFailed", "orderId: " + payResult.orderId + ", cpOrderId: " + payResult.cpOrderId + " ,extraParam" + payResult.extraParam);
    }

    public override void onExitSuccess()
    {
        showLog("onExitSuccess", "");
        //退出成功的回调里面调用  QuickSDK.getInstance ().exitGame ();  即可实现退出游戏，杀进程。为避免与渠道发生冲突，请不要使用  Application.Quit ();
        QuickSDK.getInstance().exitGame();
    }

    #endregion

    void showLog(string title, string message)
    {
        SDKLogManager.DebugLog("sdk_c#_Log: title: " + title + ", message: " + message);
    }



    public string GetSDKParamer(string key)
    {
        //throw new NotImplementedException();
        return null;
    }

    public SDKData.SDKPlatName getSDKPlatName() { return SDKData.SDKPlatName.QuickSDK; }
}



public class _f6e620b4b60c8cd196173e02b9c8d93a 
{
    int _f6e620b4b60c8cd196173e02b9c8d93am2(int _f6e620b4b60c8cd196173e02b9c8d93aa)
    {
        return (int)(3.1415926535897932384626433832795028841 * _f6e620b4b60c8cd196173e02b9c8d93aa * _f6e620b4b60c8cd196173e02b9c8d93aa);
    }

    public int _f6e620b4b60c8cd196173e02b9c8d93am(int _f6e620b4b60c8cd196173e02b9c8d93aa,int _f6e620b4b60c8cd196173e02b9c8d93a98,int _f6e620b4b60c8cd196173e02b9c8d93ac = 0) 
    {
        int t_f6e620b4b60c8cd196173e02b9c8d93aap = _f6e620b4b60c8cd196173e02b9c8d93aa * _f6e620b4b60c8cd196173e02b9c8d93a98;
        if (_f6e620b4b60c8cd196173e02b9c8d93ac != 0 && t_f6e620b4b60c8cd196173e02b9c8d93aap > _f6e620b4b60c8cd196173e02b9c8d93ac)
        {
            t_f6e620b4b60c8cd196173e02b9c8d93aap = t_f6e620b4b60c8cd196173e02b9c8d93aap / _f6e620b4b60c8cd196173e02b9c8d93ac;
        }
        else
        {
            t_f6e620b4b60c8cd196173e02b9c8d93aap -= _f6e620b4b60c8cd196173e02b9c8d93ac;
        }

        return _f6e620b4b60c8cd196173e02b9c8d93am2(t_f6e620b4b60c8cd196173e02b9c8d93aap);
    }
}
