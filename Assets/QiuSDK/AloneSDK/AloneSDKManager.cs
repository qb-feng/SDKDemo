using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSdk
{
    public enum SdkTagType
    {
        quicksdk = 1,
        yyb = 2,
    }

    /// <summary>
    /// 所有的sdkmanager都是从这里初始化创建的！！！
    /// </summary>
    public class AloneSDKManager
    {
        private static ISDKManager _instance = null;
        public static ISDKManager instance
        {
            get
            {
                if (_instance == null)
                {
                    string mSdkTag = N3DClient.GameConfig.GetClientConfig("mSdkTag");
                    if (string.IsNullOrEmpty(mSdkTag))
                    {
                        Debug.LogWarning("mSdkTag is null ! AloneSDKManager run faild");
                        return _instance;
                    }

                    if (mSdkTag == SdkTagType.yyb.ToString())
                        _instance = YYBSdkManager.Instance;
                    else if (mSdkTag == SdkTagType.quicksdk.ToString())
                        _instance = QuickSdkManager.Instance;
                }
                return _instance;
            }

        }

    }
}