#import <Foundation/Foundation.h>
#import "sdkmanager.h"
#import "sdkdelegate.h"
#import <AdSupport/AdSupport.h>
#import <UPolymerizeSDK/UPolymerizeSDK.h>
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
    [PolymerizeSDK_kit commonSdk_InitWithGameID:@"105198" andGamekey:@"660d0c4abe8e9bb9477c3d51cc255dde" andChannel:@"deep425"];
    [PolymerizeSDK_kit  commonSdk_OutLoginSuccess:^(BOOL boolSucceed_1) {
        NSLog(@"退出账号===");
    }];

    //退出账号 的回调
    [PolymerizeSDK_kit  commonSdk_OutLoginSuccess:^(BOOL boolSucceed_1) {
        NSLog(@"退出账号===");
                //注销回调
        if(boolSucceed_1)
        {
            [self SendMessageToUnity:@"IOSLogoutCallBack" ARGS:@"SUCCESS"];
        }
        else
        {
            [self SendMessageToUnity:@"IOSLogoutCallBack" ARGS:@"ERROR"];
        }
       // [self SDKmanagerlogin];
    }];

    
    NSLog(@"ios:初始化成功!");
  
}


//登入方法
-(void)SDKmanagerlogin
{
    NSLog(@"--点击登录--");
    [PolymerizeSDK_kit commonSdk_ShowLogin:^(NSString *paramToken) {

        NSLog(@"登录成功回调%@",paramToken);
        NSLog(@"登录成功回调%@",[PolymerizeSDK_kit sharedInstance].subUserName);
	
	/*
        JHRoleModel  * model =[[JHRoleModel alloc]init];
        model.JH_serverID =@"1";
        model.JH_serverName=@"昆仑1服";
        model.JH_roleID =@"0001";
        model.JH_rolename=@"角色名称";
        model.JH_level =@"100级";
        [PolymerizeSDK_kit commonSdk_SetPlayerInfo:model];
	*/

        NSString *username = [PolymerizeSDK_kit sharedInstance].subUserName;
        NSString *sendArgs = [NSString stringWithFormat:@"%@|%@",username,paramToken];
       
        //登入回调
        [self SendMessageToUnity:@"IOSLoginCallBack" ARGS:sendArgs];
    }];
}

//注销方法
-(void)SDKmanagerlogout
{
    [PolymerizeSDK_kit commonSdk_ShowLogout];
}


//保存角色信息
-(void)SDKmanagersavedata:(NSString*)roleid RNAME:(NSString*)rolename SERVERID:(NSString*)serverid RLEVEL:(NSString*)rolelevel SERVERNAME:(NSString*)servername
{
    NSLog(@"上传数据点击");
    JHRoleModel *playModel1=[[JHRoleModel alloc]init];
    playModel1.JH_roleID=roleid;
    playModel1.JH_rolename=rolename;
    playModel1.JH_serverID=serverid;
    playModel1.JH_level=rolelevel;
    playModel1.JH_serverName = servername;

    [PolymerizeSDK_kit commonSdk_SetPlayerInfo:playModel1];
    NSLog(@"上传数据成功！");

}

//充值const char* orderid,const char* rolename,const char* serverid,const char* amount,const char* productid,const char* productname,const char* extra,const char* gamename
-(void)SDKmanagersavedata:(NSString*)orderid RNAME:(NSString*)rolename SERVERID:(NSString*)serverid MONEY:(NSString*)money PROID:(NSString*)productid PRONAME:(NSString*)productname EXT:(NSString*)extra GAMENAME:(NSString*)gamename SERNAME:(NSString*)servername RLEVEL:(NSString*)rolelevel 
{
    NSLog(@"充值开始！");
    /*
       参数不懂，请点击查看参数说明
    */
    NYPayModel *model=[[NYPayModel alloc]init];
    model.orderID=orderid;
    model.serverName=servername;
    model.roleName=rolename;
    model.productName=productname;
    model.amount=[money floatValue]>0.0f?money:@"6.0";
    model.extra=extra;
    model.roleLevel=rolelevel;
    model.productdesc =@"";
    // app名称
    model.gameName=gamename;//(传的是 :游戏名称)
    
    model.productId=productid;
    model.serverID = serverid;
    model.roleID = rolename;
    [PolymerizeSDK_kit commonSdk_StartPay:model];
	
	
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
}

