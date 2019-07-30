using UnityEngine;
using System.Collections;
using UnityEngine;
using System;

namespace AloneSdk
{
    /// <summary>
    /// 接单独渠道用的sdk管理器，如yyb，vc，oppo等
    /// </summary>
    public class YYBSdkManager : AloneSdkBase<YYBSdkManager>
    {
        public override SDKData.SDKPlatName getSDKPlatName()
        {
            return SDKData.SDKPlatName.YYB;
        }

        private System.Collections.Generic.Dictionary<string, string> currentSDKParmer = new System.Collections.Generic.Dictionary<string, string>();

        public override void InitSDK(SDKData.InitArgModel initArgModel)
        {
            base.InitSDK(initArgModel);

            if (onInitComplete != null)
                onInitComplete(true);
        }

        public override string PayItem(SDKData.PayOrderData orderData)
        {
            PayArgModels args = new PayArgModels();
            args.isCanChange = false;
            args.saveValue = orderData.productCount.ToString();
            args.zoneId = orderData.zoneID.ToString();
            args.appResData = orderData.datas;

#if UNITY_EDITOR

#elif UNITY_ANDROID
         CallAndroidFunc(SDKData.SDKPlatCommonData.StartSDKPay,LitJson.JsonMapper.ToJson(args));   
#elif UNITY_PHONE

#endif

            return null;
        }

        /// <summary>
        /// 获取应用宝登入参数
        /// </summary>
        private void GetYYBLoginArgs()
        {
            try
            {
                string arg = CallAndroidFuncGetResult("GetSDKParamer");
                DebugLogCallBack("GetSDKParamer：" + arg);
                if (!string.IsNullOrEmpty(arg))
                {
                    LoginArgModel argModel = LitJson.JsonMapper.ToObject<LoginArgModel>(arg);
                    //TODO 处理登入回调
                    if (argModel != null)
                    {
                        currentSDKParmer.Add("platform", argModel.platform.ToString());
                        currentSDKParmer.Add("accessToken", argModel.accessToken);
                        currentSDKParmer.Add("openid", argModel.openid);
                        currentSDKParmer.Add("payToken", argModel.payToken);

                        currentSDKParmer.Add("flag", argModel.flag.ToString());
                        currentSDKParmer.Add("msg", argModel.msg);
                        currentSDKParmer.Add("pf", argModel.pf);
                        currentSDKParmer.Add("pf_key", argModel.pf_key);
                    }
                }
            }
            catch (Exception e)
            {
                DebugErrorCallBack("GetSDKParamer出错：" + e.Message);
            }
        }

        #region 重写回调
        public override void LoginCallBack(string arg)
        {
            try
            {
                DebugLogCallBack("收到登入回调：" + arg);
                if (onLoginComplete != null)
                    onLoginComplete(arg == "1");
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

        #endregion

        [System.Serializable]
        /// <summary>
        /// 登入参数
        /// </summary>
        public class LoginArgModel
        {
            public int platform;
            public string accessToken;
            public string payToken;
            public string openid;

            public int flag;
            public string msg;
            public string pf;
            public string pf_key;
        }

        /**
 * 充值游戏币
 * 
 * @param zoneId
 *            大区id
 * @param saveValue
 *            充值数额
 * @param isCanChange
 *            设置的充值数额是否可改
 * @param resData
 *            代币图标的二进制数据
 * @param listener
 *            充值回调
 **/
        [System.Serializable]
        public class PayArgModels
        {
            public string zoneId;// 区id
            public bool isCanChange;// 设置的充值数额是否可改
            public byte[] appResData;// 代币图标的二进制数据
            public string saveValue;// 充值数额(实际上是购买的游戏币数量)
        }
    }

}