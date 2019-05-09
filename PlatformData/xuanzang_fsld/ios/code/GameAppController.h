#import "UnityAppController.h"

@interface GameAppController : UnityAppController
@end

IMPL_APP_CONTROLLER_SUBCLASS (GameAppController)

#ifdef __cplusplus
extern "C" {
#endif
    // common interface
    void ShowQuitAlert();
    const char* GetSDKTag();
    float GetScreenBrightness();
    void SetScreenBrightness(float val);

    // SDK Interface
    void CallSDKLogin();
    void CallSDKSwitchLogin();
    void CallSDKLogout();
    void CallSDKShowAccountCenter();
    void CallSDKShowMenu(const char* uid, const char* token);
    void CallSDKHideMenu();
    void CallSetUID(const char* uid);
    void CallSetRoleInfo(int serverID, const char* serverName, const char* roleID, const char* roleName, const char* roleLevel);
    /*
    1：选择服务器
    2：创建角色
    3：进入游戏
    4：等级提升
    5：退出游戏
    6: 进入副本
    7：退出副本
    8: VIP等级提升
     */
    void CallSDKSendData(int t, int serverID, const char* serverName, const char* roleID, const char* roleName, const char* roleLevel, int moneyNum, int roleCreateTime, int roleLevelUpTime, const char* vip, const char* uid);
    void CallSDKPay(const char* rechargeID, const char* productID, const char* productName, const char* productDesc, int price, int ratio,
                           int buyNum, int coinNum, const char* serverID, const char* serverName,
                           const char* roleID, const char* roleName, int roleLevel, const char* vip, const char* cpOrderNum, const char* extension, const char* uid);
    
    // QQ Interface
    void QQShare(int type, const char* title, const char* summary, const char* targetUrl, const char* imageUrl);
    void QQShareQzone(const char* title, const char* summary, const char* targetUrl, const char* imageUrl);
    void QQAddFriend(const char* uid, const char* comment, const char* validation);
    void QQBindGroup(const char* unionId, const char* unionName, const char* zoneId, const char* roleId);
    void QQCheckBindGroup(const char* unionId, const char* zoneId, const char* roleId);
    void QQUnbindGroup(const char* unionId, const char* zoneId, const char* roleId);
    void QQJoinGroup(const char* unionId, const char* zoneId, const char* roleId);
    void QQCheckJoinGroup(const char* unionId, const char* zoneId);
    void QQBuLuo();
    void QQOpenVip(const char* code, const char* month);
    void QQAuthorization();
    void QQGetUserInfoVip();
    
    void RegisterPushAccount(const char* account);
    void RegisterPushTag(const char* tag);
    void PushDebugEnabled(bool enable);
    
#ifdef __cplusplus
}
#endif