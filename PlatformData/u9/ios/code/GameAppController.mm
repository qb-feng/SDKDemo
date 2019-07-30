#import "GameAppController.h"
#import "UnityAppController.h"
#import <Bugly/Bugly.h>
#import "UnityInterface.h"
#include <string>

@implementation GameAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
	[super application:application didFinishLaunchingWithOptions:launchOptions];
	
	[Bugly startWithAppId:@"a9d44a971b"];
	
    return YES;
}
@end


#pragma mark cplusplus code 
extern "C"
{
	void ShowQuitAlert()
	{
		UIAlertController* alert = [UIAlertController alertControllerWithTitle:@"退出确认" message:@"确定要退出游戏吗?" preferredStyle:UIAlertControllerStyleAlert];  
      
	    UIAlertAction* defaultAction = [UIAlertAction actionWithTitle:@"继续游戏" style:UIAlertActionStyleDefault  
			handler:^(UIAlertAction * action) {  
				UnitySendMessage("_AppRoot", "OnNativeMessage", "QuitGame");
	   	}];
	    UIAlertAction* cancelAction = [UIAlertAction actionWithTitle:@"退出游戏" style:UIAlertActionStyleDefault  
			handler:^(UIAlertAction * action) {  

	 	}];
	  
	    [alert addAction:defaultAction];  
	    [alert addAction:cancelAction];  
	    [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:alert animated:YES completion:nil];  
	}
	
	const char* GetSDKTag()
	{
		return strdup("");
	}
	
	float GetScreenBrightness()
	{
		return [UIScreen mainScreen].brightness;
	}
	
	void SetScreenBrightness(float val)
	{
		[UIScreen mainScreen].brightness = val;
	}
	
	void CallSDKLogin()
	{
		
	}
	
    void CallSDKSwitchLogin()
	{
		
	}
	
    void CallSDKLogout()
	{
		
	}
	
    void CallSDKShowAccountCenter()
	{
		
	}
	
    void CallSDKShowMenu(const char* uid, const char* token)
	{
		
	}
	
    void CallSDKHideMenu()
	{
		
	}
	
	void CallSetUID(const char* uid)
	{
		
	}
	
	void CallSetRoleInfo(int serverID, const char* serverName, const char* roleID, const char* roleName, const char* roleLevel)
	{
		
	}
	
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
    void CallSDKSendData(int t, int serverID, const char* serverName, const char* roleID, const char* roleName, const char* roleLevel, int moneyNum, int roleCreateTime, 
		int roleLevelUpTime, const char* vip, const char* uid)
	{
		
	}
	
    void CallSDKPay(const char* rechargeID, const char* productID, const char* productName, const char* productDesc, int price, int ratio,
		int buyNum, int coinNum, const char* serverID, const char* serverName,
		const char* roleID, const char* roleName, int roleLevel, const char* vip, const char* cpOrderNum, const char* extension, const char* uid)
	{
		
	}

	void QQShare(int type, const char* title, const char* summary, const char* targetUrl, const char* imageUrl)
    {
    }
    
    void QQShareQzone(const char* title, const char* summary, const char* targetUrl, const char* imageUrl)
    {
    }
    
    void QQAddFriend(const char* uid, const char* comment, const char* validation)
    {
    }
    
    void QQBindGroup(const char* unionId, const char* unionName, const char* zoneId, const char* roleId)
    {
    }
    
    void QQCheckBindGroup(const char* unionId, const char* zoneId, const char* roleId)
    {
    }
    
    void QQUnbindGroup(const char* unionId, const char* zoneId, const char* roleId)
    {
    }
    
    void QQJoinGroup(const char* unionId, const char* zoneId, const char* roleId)
    {
    }
    
    void QQCheckJoinGroup(const char* unionId, const char* zoneId)
    {
    }
    
    void QQBuLuo()
    {
    }
    
    void QQOpenVip(const char* code, const char* month)
    {
    }
    
    void QQAuthorization()
    {
    }
    
    void QQGetUserInfoVip()
    {
    }
    
    void RegisterPushAccount(const char* account)
    {
        //[[XGPushTokenManager defaultTokenManager] bindWithIdentifier:[NSString stringWithUTF8String:account] type:XGPushTokenBindTypeAccount];
    }
    
    void RegisterPushTag(const char* tag)
    {
        //[[XGPushTokenManager defaultTokenManager] bindWithIdentifier:[NSString stringWithUTF8String:tag] type:XGPushTokenBindTypeTag];
    }
    
    void PushDebugEnabled(bool enable)
    {
        //[[XGPush defaultManager] setEnableDebug:enable];
    }
    
    void FaceBookShare(const char* url)
    {
    }

    void DreamerAppScore()
    {
    }
} 
