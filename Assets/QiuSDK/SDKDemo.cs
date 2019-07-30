using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SDKDemo : MonoBehaviour
{
    private Button InitButton;
    private Button LoginButton;
    private Button PayOrderButton;
    private Button SaveRoleButton;
    private Button LogoutButton;
    private Button OnGameExitButton;
    private Button SaveUMengLevelButton;//保存友盟统计button(普通计数)
    private Button SaveUMengCountButton;//保存友盟计算button

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
        LogoutButton = transform.Find("LogoutButton").GetComponent<Button>();
        OnGameExitButton = transform.Find("OnGameExitButton").GetComponent<Button>();
        SaveUMengLevelButton = transform.Find("SaveUMengLevelButton").GetComponent<Button>();
        SaveUMengCountButton = transform.Find("SaveUMengCountButton").GetComponent<Button>();
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
                    SDKLogManager.DebugLog("登入成功！", SDKLogManager.DebugType.LogWarning);
                    LoginButton.gameObject.SetActive(false);
                    SetGameGoState(true);
                }
            }, Random.Range(0, 2) == 0 ? "qq" : "wx");
        });

        PayOrderButton.onClick.AddListener(() =>
        {
            SDKLogManager.DebugLog("正在支付订单！！", SDKLogManager.DebugType.LogWarning);
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
                callbackUrl = "http://112.345.678.1/payresult.php",
                roleID = "roleiid",
                roleName = "roleiid",//角色名字
                zoneID = 1,//区id
                zoneName = "1区",//区名字
                orderTime = SDKData.PayOrderData.GetCurrentTimeMiss(),
                extra = "附加参数不得为空",
                gamename = N3DClient.GameConfig.GetClientConfig("GameName", "yyty"),
                ratio = 10,//充值比例

            };
            sdkmanager.PayItem(payOrderData);
            // SDKManager.Instance.PayOrder(payOrderData);

        });

        SaveRoleButton.onClick.AddListener(() =>
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

            // SDKManager.Instance.SavePlayerInfo(roleData);
            sdkmanager.UpdatePlayerInfo(roleData, SDKData.UpdatePlayerInfoType.levelUp);
            sdkmanager.UpdatePlayerInfo(roleData, SDKData.UpdatePlayerInfoType.createRole);
            sdkmanager.UpdatePlayerInfo(roleData, SDKData.UpdatePlayerInfoType.enterGame);
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


    }



    public void SetGameGoState(bool state)
    {
        PayOrderButton.gameObject.SetActive(state);
        SaveRoleButton.gameObject.SetActive(state);
        LogoutButton.gameObject.SetActive(state);
        OnGameExitButton.gameObject.SetActive(state);
    }
}
