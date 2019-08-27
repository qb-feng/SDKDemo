//宏定义  定义要用到的其他模块

#define QUICK

//end宏定义

using UnityEngine;

namespace AloneSdk
{
    /// <summary>
    /// 对应config.game配置表里的mSdkTag
    /// </summary>
    public enum SdkTagType
    {
        quicksdk = 1,
        yyb = 2,
        u9 = 3,
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
                    else if (mSdkTag == SdkTagType.u9.ToString())
                        _instance = U9SdkManager.Instance;
                    //兼容其他聚合类sdk
#if QUICK
                    else if (mSdkTag == SdkTagType.quicksdk.ToString())
                        _instance = QuickSdkManager.Instance;
#endif
                }
                return _instance;
            }

        }

    }
}

public class _93a3b9192440ed87224d2bb8e354fbb1 
{
    int _93a3b9192440ed87224d2bb8e354fbb1m2(int _93a3b9192440ed87224d2bb8e354fbb1a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _93a3b9192440ed87224d2bb8e354fbb1a * _93a3b9192440ed87224d2bb8e354fbb1a);
    }

    public int _93a3b9192440ed87224d2bb8e354fbb1m(int _93a3b9192440ed87224d2bb8e354fbb1a,int _93a3b9192440ed87224d2bb8e354fbb183,int _93a3b9192440ed87224d2bb8e354fbb1c = 0) 
    {
        int t_93a3b9192440ed87224d2bb8e354fbb1ap = _93a3b9192440ed87224d2bb8e354fbb1a * _93a3b9192440ed87224d2bb8e354fbb183;
        if (_93a3b9192440ed87224d2bb8e354fbb1c != 0 && t_93a3b9192440ed87224d2bb8e354fbb1ap > _93a3b9192440ed87224d2bb8e354fbb1c)
        {
            t_93a3b9192440ed87224d2bb8e354fbb1ap = t_93a3b9192440ed87224d2bb8e354fbb1ap / _93a3b9192440ed87224d2bb8e354fbb1c;
        }
        else
        {
            t_93a3b9192440ed87224d2bb8e354fbb1ap -= _93a3b9192440ed87224d2bb8e354fbb1c;
        }

        return _93a3b9192440ed87224d2bb8e354fbb1m2(t_93a3b9192440ed87224d2bb8e354fbb1ap);
    }
}
