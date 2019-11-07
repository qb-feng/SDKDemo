using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SDKDemo : MonoBehaviour
{
    #region 公有控制参数
    public bool isLoginVerification = false;
    /// <summary>
    /// 是否请求订单号
    /// </summary>
    public bool isRequestOrderId = false;
    #endregion

    private Button InitButton;
    private Button LoginButton;
    private Button PayOrderButton;
    private Button SaveRoleButton;
    private Button SaveRoleButton2;
    private Button SaveRoleButton3;
    private Button LogoutButton;
    private Button OnGameExitButton;
    private Button SaveUMengLevelButton;//保存友盟统计button(普通计数)
    private Button SaveUMengCountButton;//保存友盟计算button
    private Button CheckSimulatorButton;

    private Text Message;

    private int i = 0;

    private ISDKManager sdkmanager = null;

    // Use this for initialization
    void Start()
    {
        InitButton = transform.Find("InitButton").GetComponent<Button>();
        LoginButton = transform.Find("LoginButton").GetComponent<Button>();
        PayOrderButton = transform.Find("PayOrderButton").GetComponent<Button>();
        SaveRoleButton = transform.Find("SaveRoleButton").GetComponent<Button>();
        SaveRoleButton2 = transform.Find("SaveRoleButton2").GetComponent<Button>();
        SaveRoleButton3 = transform.Find("SaveRoleButton3").GetComponent<Button>();
        LogoutButton = transform.Find("LogoutButton").GetComponent<Button>();
        OnGameExitButton = transform.Find("OnGameExitButton").GetComponent<Button>();
        SaveUMengLevelButton = transform.Find("SaveUMengLevelButton").GetComponent<Button>();
        SaveUMengCountButton = transform.Find("SaveUMengCountButton").GetComponent<Button>();
        CheckSimulatorButton = transform.Find("CheckSimulatorButton").GetComponent<Button>();

        Message = transform.Find("Message").GetComponent<Text>();

        SaveUMengLevelButton.gameObject.SetActive(false);
        SaveUMengCountButton.gameObject.SetActive(false);
        SetGameGoState(false);
        LoginButton.gameObject.SetActive(false);

        sdkmanager = AloneSdk.AloneSDKManager.instance;
        if (sdkmanager == null)
        {
            Debug.LogError("sdkmanager is null !");
            return;
        }

        InitButton.onClick.AddListener(() =>
        {
            SDKLogManager.DebugLog("正在初始化sdk！", SDKLogManager.DebugType.LogWarning);
            SDKData.InitArgModel initArg = new SDKData.InitArgModel()
            {
                onComplete = (result) =>
                {
                    if (result)
                    {
                        SDKLogManager.DebugLog("初始化成功！", SDKLogManager.DebugType.LogWarning);
                        InitButton.gameObject.SetActive(false);
                        LoginButton.gameObject.SetActive(true);
                    }
                },
                onSDKLogoutComplete = (state) =>
                 {
                     if (state)
                     {
                         SDKLogManager.DebugLog("注销成功！", SDKLogManager.DebugType.LogWarning);
                         LoginButton.gameObject.SetActive(true);
                         SetGameGoState(false);
                     }
                     else
                     {
                         SDKLogManager.DebugLog("注销失败！", SDKLogManager.DebugType.LogError);
                     }
                 },
                onSDKMessageCallBack = (message) =>
                {
                    Message.text = message;
                },
            };

            sdkmanager.InitSDK(initArg);

        });

        LoginButton.onClick.AddListener(() =>
        {
            SDKLogManager.DebugLog("正在登录！", SDKLogManager.DebugType.LogWarning);
            // SDKManager.Instance.Login((a) => { });
            sdkmanager.Login((result) =>
            {
                if (result)
                {
                    SDKLogManager.DebugLog("登入成功！开始验证：", SDKLogManager.DebugType.LogWarning);
                    if (isLoginVerification)
                    {
                        StartCoroutine(LoginVerification((userid) =>
                        {
                            SDKLogManager.DebugLog("验证成功，用户id：" + userid, SDKLogManager.DebugType.LogWarning);
                            LoginButton.gameObject.SetActive(false);
                            SetGameGoState(true);
                        }));
                    }
                    else
                    {
                        SDKLogManager.DebugLog("验证成功！", SDKLogManager.DebugType.LogWarning);
                        LoginButton.gameObject.SetActive(false);
                        SetGameGoState(true);
                    }
                }
            }, UnityEngine.Random.Range(0, 2) == 0 ? "qq" : "wx");
        });

        PayOrderButton.onClick.AddListener(() =>
        {
            SDKLogManager.DebugLog("生成订单中~", SDKLogManager.DebugType.LogWarning);
            var payOrderData = new SDKData.PayOrderData()
            {
                orderId = SDKData.PayOrderData.GetCurrentTimeMiss(),
                userid = sdkmanager.SDK_Id,
                amount = 6,
                productId = "10001",
                productName = "60钻石",
                productDesc = "60钻石",
                callbackMessage = "回调给服务器时的附加消息",
                productCount = 1,
                callbackUrl = "http://111.231.206.145/juhe/u9Pay/payCallBack",
                roleID = "roleiid",
                roleName = "roleiid",//角色名字
                zoneID = 2,//区id
                zoneName = "2区",//区名字
                orderTime = SDKData.PayOrderData.GetCurrentTimeMiss(),
                extra = "",
                gamename = N3DClient.GameConfig.GetClientConfig("GameName", "yyty"),
                ratio = 10,//充值比例
            };
            payOrderData.extra = "" + payOrderData.zoneID + "|" + payOrderData.roleID + "|" + payOrderData.productId;


            Action<OrderCreateModel> orderCreateAction = (order) =>
            {
                payOrderData.amount = order.amount;
                payOrderData.callbackUrl = order.callbackurl;
                payOrderData.orderId = order.orderid;

                sdkmanager.PayItem(payOrderData);
            };

            if (isRequestOrderId)
            {
                StartCoroutine(CreateOrder(orderCreateAction, payOrderData));
            }
            else
            {
                orderCreateAction(new OrderCreateModel()
                {
                    amount = (int)payOrderData.amount,
                    callbackurl = payOrderData.callbackUrl,
                    orderid = SDKData.PayOrderData.GetCurrentTimeMiss(),
                });
            }


            // SDKManager.Instance.PayOrder(payOrderData);

        });

        Action<SDKData.UpdatePlayerInfoType> onSaveRoleInfoClick = (type) => 
        {
            SDKLogManager.DebugLog("正在保存角色信息！！", SDKLogManager.DebugType.LogWarning);
            var roleData = new SDKData.RoleData()
            {
                roleId = "123456",
                roleLevel = "22",
                roleName = "测试角色",
                createTime = System.DateTime.Now.Millisecond.ToString(),
                realmId = "1",
                realmName = "1区测试服",
                chapter = "1-1",
                arg = "这是一条保存角色信息的附加信息！",
            };
            sdkmanager.UpdatePlayerInfo(roleData, type);
        };

        SaveRoleButton.onClick.AddListener(() =>
        {
            onSaveRoleInfoClick(SDKData.UpdatePlayerInfoType.createRole);
        });
        SaveRoleButton2.onClick.AddListener(() =>
        {
            onSaveRoleInfoClick(SDKData.UpdatePlayerInfoType.enterGame);
        });
        SaveRoleButton3.onClick.AddListener(() =>
        {
            onSaveRoleInfoClick(SDKData.UpdatePlayerInfoType.levelUp);
        });


        LogoutButton.onClick.AddListener(() =>
        {
            SDKLogManager.DebugLog("正在登出账号！！", SDKLogManager.DebugType.LogWarning);
            //SDKManager.Instance.Logout();
            sdkmanager.Logout();
        });

        OnGameExitButton.onClick.AddListener(() =>
        {
            SDKLogManager.DebugLog("正在退出sdk！！", SDKLogManager.DebugType.LogWarning);
            //SDKManager.Instance.OnGameExit(() =>
            //{
            //    Debug.LogWarning("SDK 已经退出！");
            //});
            sdkmanager.ExitGame();

#if UNITY_EDITOR
            SetGameGoState(false);
            LoginButton.gameObject.SetActive(false);
            InitButton.gameObject.SetActive(true);
#endif

        });

        SaveUMengLevelButton.onClick.AddListener(() =>
        {
            //UmengManager.Instance.TriggerEvent(UMengCustomEventID.TestEventID);
        });

        SaveUMengCountButton.onClick.AddListener(() =>
        {
            var data = new System.Collections.Generic.Dictionary<string, string>() 
            {
                {"test1","1"},
                {"test2","4"},
                {"test3","8"},
                {"test4","16"},
            };
            //UmengManager.Instance.TriggerEvent(UMengCustomEventID.TestComputingEvent, data);
        });

        CheckSimulatorButton.onClick.AddListener(() =>
        {
            CheckSimulator();
        });


    }



    public void SetGameGoState(bool state)
    {
        PayOrderButton.gameObject.SetActive(state);
        SaveRoleButton.gameObject.SetActive(state);
        SaveRoleButton2.gameObject.SetActive(state);
        SaveRoleButton3.gameObject.SetActive(state);
        LogoutButton.gameObject.SetActive(state);
        OnGameExitButton.gameObject.SetActive(state);
    }

    private readonly string ServerListURL = "http://111.231.206.145/xylz_pay_api/operateApi/";
    /// <summary>
    /// 登录验证
    /// </summary>
    private IEnumerator LoginVerification(Action<string> onComplete)
    {
        string sdkType = N3DClient.GameConfig.GetClientConfig("mSdkTag");//sdk类型

        string url = ServerListURL + sdkType + "loginVerification.php";
        SDKLogManager.DebugLog("登录验证url:" + url);
        Dictionary<string, string> postData = new Dictionary<string, string>();
        //TODO  登录验证参数

        string platform = "win";
#if UNITY_ANDROID
        platform = "android";
#elif UNITY_IOS
        platform = "ios";
#endif
        var sdkInstance = AloneSdk.AloneSDKManager.instance;
        postData.Add("platform", platform);//平台
        postData.Add("userid", sdkInstance.GetSDKParamer("userId"));//渠道
        postData.Add("token", sdkInstance.GetSDKParamer("token"));
        postData.Add("checkloginurl", sdkInstance.GetSDKParamer("checkloginurl"));//登录验证url

        postData.Add("channeltype", sdkInstance.GetSDKParamer("channeltype"));//渠道类型
        postData.Add("channelcode", sdkInstance.GetSDKParamer("channelcode"));//渠道代码

        WWWForm form = null;
        WWW www = null;
        if (postData != null)
        {
            form = new WWWForm();
            foreach (var item in postData)
            {
                form.AddField(item.Key, item.Value);
            }
        }
        www = new WWW(url, form);

        yield return www;
        try
        {
            if (string.IsNullOrEmpty(www.error))
            {
                string content = System.Text.Encoding.UTF8.GetString(www.bytes);
                if (!string.IsNullOrEmpty(content))
                {
                    SDKLogManager.DebugLog("玩家验证结果：" + content);
                    var result = LitJson.JsonMapper.ToObject<LoginVerificationData>(content);
                    if (result.code == 0)
                    {
                        SDKLogManager.DebugLog("玩家验证成功！" + result.data);
                        onComplete(result.data);
                    }
                    else
                    {
                        SDKLogManager.DebugLog("玩家验证失败！" + content, SDKLogManager.DebugType.LogError);
                    }
                }
            }
        }
        catch (Exception e)
        {
            SDKLogManager.DebugLog("玩家验证报错！" + e.Message, SDKLogManager.DebugType.LogError);
        }
    }

    /// <summary>
    /// 请求服务器生成订单
    /// </summary>
    private IEnumerator CreateOrder(Action<OrderCreateModel> onComplete, SDKData.PayOrderData payData)
    {
        string sdkType = N3DClient.GameConfig.GetClientConfig("mSdkTag");//sdk类型

        string url = ServerListURL + sdkType + "orderCreate.php";
        SDKLogManager.DebugLog("支付验证url:" + url);
        Dictionary<string, string> postData = new Dictionary<string, string>();
        //TODO  登录验证参数

        string platform = "win";
#if UNITY_ANDROID
        platform = "android";
#elif UNITY_IOS
        platform = "ios";
#endif
        var sdkInstance = AloneSdk.AloneSDKManager.instance;
        postData.Add("platform", platform);//平台
        postData.Add("userid", sdkInstance.GetSDKParamer("userId"));//渠道
        postData.Add("roleid", payData.roleID);
        postData.Add("productid", payData.productId);//商品号
        postData.Add("zoneid", payData.zoneID.ToString());//区id
        postData.Add("money", payData.amount.ToString());//区id

        postData.Add("channeltype", sdkInstance.GetSDKParamer("channeltype"));//渠道类型
        postData.Add("channelcode", sdkInstance.GetSDKParamer("channelcode"));//渠道代码

        WWWForm form = null;
        WWW www = null;
        if (postData != null)
        {
            form = new WWWForm();
            foreach (var item in postData)
            {
                form.AddField(item.Key, item.Value);
            }
        }
        www = new WWW(url, form);

        yield return www;

        try
        {
            if (string.IsNullOrEmpty(www.error))
            {
                string content = System.Text.Encoding.UTF8.GetString(www.bytes);
                if (!string.IsNullOrEmpty(content))
                {
                    SDKLogManager.DebugLog("订单生成返回结果：" + content);
                    var result = LitJson.JsonMapper.ToObject<OrderCreateModel>(content);
                    if (result.code == 0)
                    {
                        SDKLogManager.DebugLog("订单生成成功！" + result.orderid + "  amount:" + result.amount);
                        onComplete(result);
                    }
                    else
                    {
                        SDKLogManager.DebugLog("订单生成失败！" + content, SDKLogManager.DebugType.LogError);
                    }
                }
            }
        }
        catch (Exception e)
        {
            SDKLogManager.DebugLog("玩家订单生成报错！" + e.Message, SDKLogManager.DebugType.LogError);
        }


    }

    /// <summary>
    /// 判断是否是模拟器
    /// </summary>
    private bool CheckSimulator()
    {
        //判断是否要检测模拟器
        var checkSimulatorEnable = N3DClient.GameConfig.GetClientConfigBool("CheckSimulatorEnable", false);
        if (checkSimulatorEnable)
        {
            return JavaUtils.CheckSimulator();
        }
        return false;
    }

    /// <summary>
    /// 服务器下发的生成的订单结构
    /// </summary>
    [System.Serializable]
    public class OrderCreateModel
    {
        public int code;//返回码
        public string orderid;
        public int amount;//金额
        public string callbackurl;//订单回调地址
    }


    [System.Serializable]
    public class LoginVerificationData
    {
        public int code;
        public string data;
    }
}


public class _a30c61fc15f991005b6fb34d02b0be76
{
    int _a30c61fc15f991005b6fb34d02b0be76m2(int _a30c61fc15f991005b6fb34d02b0be76a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _a30c61fc15f991005b6fb34d02b0be76a * _a30c61fc15f991005b6fb34d02b0be76a);
    }

    public int _a30c61fc15f991005b6fb34d02b0be76m(int _a30c61fc15f991005b6fb34d02b0be76a, int _a30c61fc15f991005b6fb34d02b0be761, int _a30c61fc15f991005b6fb34d02b0be76c = 0)
    {
        int t_a30c61fc15f991005b6fb34d02b0be76ap = _a30c61fc15f991005b6fb34d02b0be76a * _a30c61fc15f991005b6fb34d02b0be761;
        if (_a30c61fc15f991005b6fb34d02b0be76c != 0 && t_a30c61fc15f991005b6fb34d02b0be76ap > _a30c61fc15f991005b6fb34d02b0be76c)
        {
            t_a30c61fc15f991005b6fb34d02b0be76ap = t_a30c61fc15f991005b6fb34d02b0be76ap / _a30c61fc15f991005b6fb34d02b0be76c;
        }
        else
        {
            t_a30c61fc15f991005b6fb34d02b0be76ap -= _a30c61fc15f991005b6fb34d02b0be76c;
        }

        return _a30c61fc15f991005b6fb34d02b0be76m2(t_a30c61fc15f991005b6fb34d02b0be76ap);
    }
}
