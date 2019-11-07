//
//  AppDelegate.h
//  appStoreDemo
//
//  Created by YuLing on 2018/7/14.
//  Copyright © 2018年 YuLing. All rights reserved.
//

#ifndef sdkdelegate_h
#define sdkdelegate_h


#endif /* sdkdelegate_h */
#import <Foundation/Foundation.h>

//申明一个代理
@protocol sdkdelegate<NSObject>
-(void)VoiceRecognitionClientWorkStatus:(int)workStatus obj:(id)aObj;
@end

