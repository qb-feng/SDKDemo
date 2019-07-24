#import "UnityAppController.h"
#import "XGPush.h"

@interface CustomAppController : UnityAppController < XGPushDelegate >
@end

IMPL_APP_CONTROLLER_SUBCLASS (CustomAppController)

@implementation CustomAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];

    NSLog(@"[XGDemo] Start!!!!!!!!!");
    [[XGPush defaultManager] startXGWithAppID:2200295179 appKey:@"IY3468L3UIHS" delegate:self];
    
    return YES;
}

/**
 收到通知的回调

 @param application  UIApplication 实例
 @param userInfo 推送时指定的参数
 */
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {

    [super application:application didReceiveRemoteNotification:userInfo];

    NSLog(@"[XGDemo] receive Notification");
    [[XGPush defaultManager] reportXGNotificationInfo:userInfo];
}


/**
 收到静默推送的回调

 @param application  UIApplication 实例
 @param userInfo 推送时指定的参数
 @param completionHandler 完成回调
 */
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler {
    NSLog(@"[XGDemo] receive slient Notification");
    NSLog(@"[XGDemo] userinfo %@", userInfo);

    [super application:application didReceiveRemoteNotification:userInfo fetchCompletionHandler:completionHandler];

    [[XGPush defaultManager] reportXGNotificationInfo:userInfo];
    completionHandler(UIBackgroundFetchResultNewData);
}

// iOS 10 新增 API
// iOS 10 会走新 API, iOS 10 以前会走到老 API
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
// App 用户点击通知
// App 用户选择通知中的行为
// App 用户在通知中心清除消息
// 无论本地推送还是远程推送都会走这个回调
- (void)xgPushUserNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)(void))completionHandler {
    NSLog(@"[XGDemo] click notification");
    
    [[XGPush defaultManager] reportXGNotificationResponse:response];
    
    completionHandler();
}

// App 在前台弹通知需要调用这个接口
- (void)xgPushUserNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(UNNotificationPresentationOptions))completionHandler {
    [[XGPush defaultManager] reportXGNotificationInfo:notification.request.content.userInfo];
    completionHandler(UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound | UNNotificationPresentationOptionAlert);
}
#endif

@end