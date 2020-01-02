#import <Foundation/Foundation.h>
#import "sdkmanager.h"
#import "sdkdelegate.h"
#import <XHappStoreSDK/YLPlatformSDK_Kit.h>
#import <AdSupport/AdSupport.h>
//#import <PolymerizeSDK_kit.h>
//#import <UPolymerizeSDK.h>

@implementation sdkmanager

//单例
static sdkmanager* _sharedInstance = nil;

+ (sdkmanager*)Instance
{
    @synchronized(self.class)
    {
        if (_sharedInstance == nil) {
            _sharedInstance = [[self.class alloc] init];
        }
        
        return _sharedInstance;
    }
}

//方法
-(void)SDKmanagerinit
{
    NSLog(@"ios:初始化!");
    
    //退出账号 的回调
    [YLPlatformSDK_Kit sharedInstance].ylExitAccount =
    ^(NSString *currentUserName) {
        NSLog(@"退出账号成功===%@", currentUserName);
        [self SendMessageToUnity:@"IOSLogoutCallBack" ARGS:@"SUCCESS"];
    };
		
		    //支付回调
    [YLPlatformSDK_Kit sharedInstance].ylResultCallBack = ^(PaymentStatus code) {
        switch (code) {
            case PaymentStatusSussess:
            {
                NSLog(@"支付成功");
            }
            break;
            case PaymentStatusFailure:
            {
                NSLog( @"支付失败");
            }
            break;
            case PaymentStatusError:
            {
                NSLog( @"支付异常");
            }
            break;
            default:
            break;
        }
    };
		
	
    
    NSLog(@"ios:初始化成功!");
  
}


//登入方法
-(void)SDKmanagerlogin
{
    NSLog(@"--点击登录--");
	[[YLPlatformSDK_Kit sharedInstance] showLogin:YES];//显示登录界面
    //登录成功回调
    [YLPlatformSDK_Kit sharedInstance].ylLoginSuccessBack = ^(NSString *paramToken) {
        NSLog(@"登录成功回调%@", paramToken);
        NSLog(@"username=%@", [[YLPlatformSDK_Kit sharedInstance] currentUserName]);
		
        NSString *username = [[YLPlatformSDK_Kit sharedInstance] currentUserName];
        NSString *sendArgs = [NSString stringWithFormat:@"%@|%@",username,paramToken];
       
        //登入回调
        [self SendMessageToUnity:@"IOSLoginCallBack" ARGS:sendArgs];
    };
}

//注销方法
-(void)SDKmanagerlogout
{
    [[YLPlatformSDK_Kit sharedInstance] cancelAccount];//注销账户 悬浮球消失
}


//保存角色信息
-(void)SDKmanagersavedata:(NSString*)roleid RNAME:(NSString*)rolename SERVERID:(NSString*)serverid RLEVEL:(NSString*)rolelevel SERVERNAME:(NSString*)servername
{
    NSLog(@"上传数据点击");
	
    [[YLPlatformSDK_Kit sharedInstance]getUserRolename:rolename
                                            andRoleID:roleid
                                        andServerNum:serverid andLevel:rolelevel
                                        complement:^(bool result){ //返回的result 为BOOL YES  NO
                                            if (result) {NSLog(@"上传成功");} else {NSLog(@"上传失败");}
                                        }];



}

//充值const char* orderid,const char* rolename,const char* serverid,const char* amount,const char* productid,const char* productname,const char* extra,const char* gamename
-(void)SDKmanagersavedata:(NSString*)orderid RNAME:(NSString*)rolename SERVERID:(NSString*)serverid MONEY:(NSString*)money PROID:(NSString*)productid PRONAME:(NSString*)productname EXT:(NSString*)extra GAMENAME:(NSString*)gamename SERNAME:(NSString*)servername RLEVEL:(NSString*)rolelevel 
{
	    NSLog(@"充值开始！");
	
    [[YLPlatformSDK_Kit sharedInstance] showRechargeOrderId:orderid
                                                  serverNum:serverid
                                                 playerName:rolename
                                                productName:productname
                                                     amount:[money floatValue]>0.0f?[money floatValue]:6.0
                                                      extra:extra
                                                   gameName:@"封神论道"
                                                   animated:YES
                                               andproductIDname:productid
                                              andcompletion:^(NSString *resultData) {
                                                  NSLog(@"苹果内购返回值:%@",resultData);
                                              }];	
	
	
    NSLog(@"充值結束！");

}


//发送消息到unity
-(void)SendMessageToUnity:(NSString*)functionName ARGS:(NSString*)args
{
    //NSLog(@"ios发送消息到unity ： %@ ： %@", functionName.UTF8String,args.UTF8String);
    UnitySendMessage("YaoLingSDKCallBackManager", functionName.UTF8String, args.UTF8String);
}


@end



extern "C"
{
    
    //初始化
    void sdkmanagerinit()
    {
        [sdkmanager.Instance SDKmanagerinit];
    }

    //登入
    void sdkmanagerlogin()
    {
        [sdkmanager.Instance SDKmanagerlogin];
    }

    //注销
    void sdkmanagerlogout()
    {
        [sdkmanager.Instance SDKmanagerlogout];
    }


    //保存角色信息 角色id 角色名字  服务器id 角色等级
    void sdkmanagersavedata(const char* roleid,const char* rolename,const char* serverid,const char* rolelevel,const char* sername)
    {
        NSLog(@"开始保存角色信息：");

        NSString *rid = [NSString stringWithUTF8String:roleid];
        NSString *rname = [NSString stringWithUTF8String:rolename];
        NSString *rserverid = [NSString stringWithUTF8String:serverid];
        NSString *rlevel = [NSString stringWithUTF8String:rolelevel];
        NSString *servername = [NSString stringWithUTF8String:sername];
				
        //NSLog(@"roleid:%@",rid);
        //NSLog(@"rolename:%@",rname);
        //NSLog(@"serverid:%@",rserverid);
        //NSLog(@"rolelevel:%@",rlevel);
        //NSLog(@"servername:%@",sername);
		
        [sdkmanager.Instance SDKmanagersavedata:rid RNAME:rname SERVERID:rserverid RLEVEL:rlevel SERVERNAME:servername];
    }


    //充值  订单号 角色名字 服务器id 金额 商品id（正版填苹果内购商品id，越狱版填nil）商品名称 扩展参数  游戏名字
    void sdkmanagerpayorder(const char* orderid,const char* rolename,const char* serverid,const char* amount,const char* productid,const char* productname,const char* extra,const char* gamename,const char* sername,const char* rlevel)
    {
        NSLog(@"开始充值：");

        NSString *ordid = [NSString stringWithUTF8String:orderid];
        NSString *rname = [NSString stringWithUTF8String:rolename];
        NSString *rserverid = [NSString stringWithUTF8String:serverid];
        NSString *money = [NSString stringWithUTF8String:amount];

        NSString *produid = [NSString stringWithUTF8String:productid];
        NSString *produname = [NSString stringWithUTF8String:productname];
        NSString *ext = [NSString stringWithUTF8String:extra];  
        NSString *gname = [NSString stringWithUTF8String:gamename];   
		
		NSString *servername = [NSString stringWithUTF8String:sername]; 
		NSString *rolelevel = [NSString stringWithUTF8String:rlevel]; 

        /*
        NSLog(@"ordid:%@",ordid);
        NSLog(@"rname:%@",rname);
        NSLog(@"rserverid:%@",rserverid);
        NSLog(@"money:%@",money);
        NSLog(@"produid:%@",produid);
        NSLog(@"produname:%@",produname);
        NSLog(@"ext:%@",ext);
        NSLog(@"gname:%@",gname);
        NSLog(@"servername:%@",servername);
		NSLog(@"rolelevel:%@",rolelevel);
        */
        [sdkmanager.Instance SDKmanagersavedata:ordid RNAME:rname SERVERID:rserverid MONEY:money PROID:produid PRONAME:produname EXT:ext GAMENAME:gname SERNAME:servername RLEVEL:rolelevel];

    }
    
    
    
    
    
    
    
    
    
    
    void ShowQuitAlert()
    {
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
    
    void EnterUserCenter()
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
        
    }
    
    void RegisterPushTag(const char* tag)
    {
    }
    
    void PushDebugEnabled(bool enable)
    {
    }
    
    void FaceBookShare(const char* url)
    {
    }
    
    void DreamerAppScore()
    {
    }
}

