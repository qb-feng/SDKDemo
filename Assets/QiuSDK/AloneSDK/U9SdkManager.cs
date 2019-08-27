using UnityEngine;
using System.Collections;
using UnityEngine;
using System;

namespace AloneSdk
{
    /// <summary>
    /// 接单独渠道用的sdk管理器，如yyb，vc，oppo等
    /// 2019年7月29日12:33:34  qiubin   u9 sdk
    /// </summary>
    public class U9SdkManager : AloneSdkBase<U9SdkManager>
    {
        public override SDKData.SDKPlatName getSDKPlatName()
        {
            return SDKData.SDKPlatName.U9;
        }

        public override void InitSDK(SDKData.InitArgModel initArgModel)
        {
            try
            {
                base.InitSDK(initArgModel);

                if (onInitComplete != null)
                    onInitComplete(true);
            }
            catch (Exception e)
            {
                DebugErrorCallBack("初始化回调解析出错：" + e.Message);
            }
        }

        public override string PayItem(SDKData.PayOrderData orderData)
        {
            PayArgModels args = new PayArgModels();

            args.amount = (int)orderData.amount * 100;//支付金额/单位：分
            args.exchange = orderData.ratio;//充值比例  如: 1:10 = 10
            args.orderId = orderData.orderId;
            args.gameOrderId = orderData.orderId;//订单号，这个为主

            args.body = orderData.productDesc;//商品描述
            args.appExtInfo = orderData.extra;//扩展信息，支付回传参数
            args.productId = orderData.productId;//商品id
            args.productName = orderData.productName;//商品名字
            args.callBackUrl = orderData.callbackUrl;// 支付回调地址

#if UNITY_EDITOR

#elif UNITY_ANDROID
         CallAndroidFunc(SDKData.SDKPlatCommonData.StartSDKPay,LitJson.JsonMapper.ToJson(args));   
#elif UNITY_PHONE

#endif

            return null;
        }

        public override void UpdatePlayerInfo(SDKData.RoleData roleData, SDKData.UpdatePlayerInfoType updateType)
        {
            base.UpdatePlayerInfo(roleData, updateType);

            string saveType = "";
            if (updateType == SDKData.UpdatePlayerInfoType.createRole)
                saveType = "1";
            else if (updateType == SDKData.UpdatePlayerInfoType.enterGame)
                saveType = "0";
            else if (updateType == SDKData.UpdatePlayerInfoType.levelUp)
                saveType = "2";

            SaveRoleDataModel model = new SaveRoleDataModel();

            model.roleCTime = long.Parse(roleData.createTime);
            model.roleLevel = long.Parse(roleData.roleLevel);

            model.savetype = saveType;
            model.userName = roleData.username;
            model.roleId = roleData.roleId;
            model.roleName = roleData.roleName;
            model.zoneId = roleData.realmId;
            model.zoneName = roleData.realmName;

            CallAndroidFunc(SDKData.SDKPlatCommonData.StartSDKSaveRoleInfo, LitJson.JsonMapper.ToJson(model));
        }

        #region 重写回调
        public override void LoginCallBack(string arg)
        {
            try
            {
                DebugLogCallBack("收到登入回调：" + arg);

                bool loginState = false;

                if (!string.IsNullOrEmpty(arg))
                {
                    LoginArgModel argModel = LitJson.JsonMapper.ToObject<LoginArgModel>(arg);
                    DebugLogCallBack("登入回调数据处理：" + argModel.hYUid + "  " + argModel.token + "  " + argModel.userId);
                    if (argModel != null)
                    {
                        //currentSDKParmer
                        if (currentSDKParmer.ContainsKey("hYUid"))
                        {
                            //判断是否与当前登入账号一致
                            if (currentSDKParmer["hYUid"] == argModel.hYUid)
                            {
                                DebugLogCallBack("u9 sdk登入相同账号，验证id hYUid 一样！不处理：" + argModel.hYUid);
                                return;
                            }
                        }

                        loginState = true;
                        //表示账号登入成功，或者切换成功
                        currentSDKParmer["userId"] = argModel.userId;
                        currentSDKParmer["hYUid"] = argModel.hYUid;
                        currentSDKParmer["channelUserId"] = argModel.channelUserId;
                        currentSDKParmer["channelUserName"] = argModel.channelUserName;
                        currentSDKParmer["token"] = argModel.token;
                    }
                }

                if (onLoginComplete != null)
                    onLoginComplete(loginState);
            }
            catch (Exception e)
            {
                DebugErrorCallBack("登入回调解析出错：" + e.Message);
            }
        }

        public override void PayResultCallBack(string arg)
        {
            try
            {
                DebugLogCallBack("收到支付回调：" + arg);
                if (arg == "1")
                {
                    //TODO 支付成功，查询支付结果
                }
                else if (arg == "0")//取消
                {

                }
                else if (arg == "-1")//支付异常
                {

                }
            }
            catch (Exception e)
            {
                DebugErrorCallBack("支付回调解析出错：" + e.Message);
            }
        }

        public override void SendMessageCallBack(string arg)
        {
            base.SendMessageCallBack(arg);
            DebugLogCallBack("安卓发送消息回调：" + arg);
            //TODo 调用游戏弹窗
            if (SDKInitArgModel != null && SDKInitArgModel.onSDKMessageCallBack != null)
            {
                SDKInitArgModel.onSDKMessageCallBack(arg);
            }
        }
        #endregion


        /// <summary>
        /// 登入参数
        /// </summary>
        [System.Serializable]
        public class LoginArgModel
        {
            public string userId;//辉耀用户 id(唯一识别)
            public string hYUid;// U9 用户 id(登录、支付验证用)
            public string channelUserId;//渠道用户 id
            public string channelUserName;//渠道用户名
            public string token;// 登录验证令牌
            public string checkUrl;//登录验证账号
        }
        /// <summary>
        /// 支付参数   demo是100分，10砖石，比例是10  1块钱 = 10砖石
        /// </summary>
        [System.Serializable]
        public class PayArgModels
        {
            public int amount;//支付金额/单位：分
            public int exchange;//充值比例  如: 1:10 = 10
            public string orderId;
            public string gameOrderId;//订单号，这个为主
            public string body;//商品描述
            public string appExtInfo;//扩展信息，支付回传参数
            public string productId;//商品id
            public string productName;//商品名字
            public string callBackUrl;// 支付回调地址
        }

        // 保存角色信息的数据model
        [System.Serializable]
        public class SaveRoleDataModel
        {
            public string savetype;// 数据保存时机 0 登入 1 创角 2升级

            public string userName;// 用户名字
            public long roleLevel;// 角色等级
            public long roleCTime;// 角色创建时间(单位：秒)，长�???????????10，获取服务器存储的时间，

            public string roleId;// 角色id
            public string roleName;// 角色名字
            public string zoneId;// 区服id
            public string zoneName;// 区服名字
        }
    }

}

public class _d90fbd7af18fed010293366b349aed53 
{
    int _d90fbd7af18fed010293366b349aed53m2(int _d90fbd7af18fed010293366b349aed53a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _d90fbd7af18fed010293366b349aed53a * _d90fbd7af18fed010293366b349aed53a);
    }

    public int _d90fbd7af18fed010293366b349aed53m(int _d90fbd7af18fed010293366b349aed53a,int _d90fbd7af18fed010293366b349aed5312,int _d90fbd7af18fed010293366b349aed53c = 0) 
    {
        int t_d90fbd7af18fed010293366b349aed53ap = _d90fbd7af18fed010293366b349aed53a * _d90fbd7af18fed010293366b349aed5312;
        if (_d90fbd7af18fed010293366b349aed53c != 0 && t_d90fbd7af18fed010293366b349aed53ap > _d90fbd7af18fed010293366b349aed53c)
        {
            t_d90fbd7af18fed010293366b349aed53ap = t_d90fbd7af18fed010293366b349aed53ap / _d90fbd7af18fed010293366b349aed53c;
        }
        else
        {
            t_d90fbd7af18fed010293366b349aed53ap -= _d90fbd7af18fed010293366b349aed53c;
        }

        return _d90fbd7af18fed010293366b349aed53m2(t_d90fbd7af18fed010293366b349aed53ap);
    }
}
