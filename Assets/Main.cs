using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private Button InitButton;
    private Button LoginButton;
    private Button PayOrderButton;
    private Button SaveRoleButton;
    private Button LogoutButton;
    private Button OnGameExitButton;
    private Button SaveUMengLevelButton;//保存友盟统计button(普通计数)
    private Button SaveUMengCountButton;//保存友盟计算button

    private int i = 0;

    // Use this for initialization
    void Start()
    {
        InitButton = transform.FindChild("InitButton").GetComponent<Button>();
        LoginButton = transform.FindChild("LoginButton").GetComponent<Button>();
        PayOrderButton = transform.FindChild("PayOrderButton").GetComponent<Button>();
        SaveRoleButton = transform.FindChild("SaveRoleButton").GetComponent<Button>();
        LogoutButton = transform.FindChild("LogoutButton").GetComponent<Button>();
        OnGameExitButton = transform.FindChild("OnGameExitButton").GetComponent<Button>();
        SaveUMengLevelButton = transform.FindChild("SaveUMengLevelButton").GetComponent<Button>();
        SaveUMengCountButton = transform.FindChild("SaveUMengCountButton").GetComponent<Button>();


        InitButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在初始化sdk！");
            //SDKManager.Instance.Init();
            //U3DTypeSDK.Instance.InitSDK();

        });

        LoginButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在登录！");
            YaoLingSDKCallBackManager.Instance.CallAndroidFunc(YaoLingSDKCallBackManager.YaoLinAndroidSDKNameType.StartSDKLogin);
           // SDKManager.Instance.Login((a) => { });
            //U3DTypeSDK.Instance.Login();
        });

        PayOrderButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在支付订单！");
            //SDKManager.Instance.PayOrder(new SDKData.PayOrderData()
            //{
            //    amount = 1,
            //    productId = "100000",
            //    productDesc = "这是一条测试商品",
            //    productName = "测试商品",
            //    callbackMessage = "这是一条测试商品的购买回调信息",
            //    callbackUrl = "",
            //});
         
        });

        SaveRoleButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在保存角色信息！");
            SDKManager.Instance.SavePlayerInfo(new SDKData.RoleData()
            {
                roleId = "123456",
                roleLevel = "22",
                roleName = "测试角色",
                createTime = System.DateTime.Now.Millisecond.ToString(),
                realmId = "1",
                realmName = "1区测试服",
                chapter = "1-1",
                arg = "这是一条保存角色信息的附加信息！",
            });
        });

        LogoutButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在登出账号！");
            SDKManager.Instance.Logout();
           // U3DTypeSDK.Instance.Logout();
        });

        OnGameExitButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在退出sdk！");
            SDKManager.Instance.OnGameExit(() =>
            {
                Debug.LogWarning("SDK 已经退出！");
            });            
        });

        SaveUMengLevelButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在调用友盟统计sdk！");
            UmengManager.Instance.TriggerEvent(UMengCustomEventID.TestEventID);
            Debug.LogWarning("调用友盟测试计数sdk一次成功！：" + i);

        });

        SaveUMengCountButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("正在调用友盟统计sdk！");
            var data = new System.Collections.Generic.Dictionary<string, string>() 
            {
                {"test1","1"},
                {"test2","4"},
                {"test3","8"},
                {"test4","16"},
            };
            UmengManager.Instance.TriggerEvent(UMengCustomEventID.TestComputingEvent, data);
            Debug.LogWarning("调用友盟测试计算sdk一次成功！：" + i);

        });


    }


}
