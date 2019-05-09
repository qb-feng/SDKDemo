//
//  AppDelegate.m
//  DemoApp
//
//  Created by YuLing on 2017/12/21.
//  Copyright © 2017年 YuLing. All rights reserved.
//

#import "AppDelegate.h"
#import <UPolymerizeSDK/UPolymerizeSDK.h>
@interface AppDelegate ()

@end

@implementation AppDelegate


- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    
    [PolymerizeSDK_kit application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    return YES;
}
//iOS10以上使用
- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options
{
    [PolymerizeSDK_kit application:app openURL:url sourceApplication:nil annotation:(id _Nonnull)NULL];
    return YES;
}
- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    [PolymerizeSDK_kit application:application openURL:url sourceApplication:nil annotation:(id _Nonnull)NULL];
    return YES;
}

- (void)applicationDidBecomeActive:(UIApplication *)application {
    [PolymerizeSDK_kit commonSdk_IsWillEnterForeground:YES OtherWiseEnterBackgroundOrExitApplication:application];
}
- (void)applicationDidEnterBackground:(UIApplication *)application {
    [PolymerizeSDK_kit commonSdk_IsWillEnterForeground:NO OtherWiseEnterBackgroundOrExitApplication:application];
}
-(UIInterfaceOrientationMask)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(nullable UIWindow *)window{
    return  [PolymerizeSDK_kit application:application supportedInterfaceOrientationsForWindow:window];
}



@end
