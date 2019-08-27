using UnityEngine;

namespace N3DClient
{
    public static class AssetConfig
    {
        /// <summary>
        /// SDK 保存在StreamingAssets下的资源路径
        /// </summary>
        public static string SDKStreamingAssetsPath { get; private set; }
        static AssetConfig()
        {
            SDKStreamingAssetsPath = Application.streamingAssetsPath + "/SDKStreamingAssets/";
        }
    }
}

public class _c2d6bbbf6c4ff9354e65d30e9424848b 
{
    int _c2d6bbbf6c4ff9354e65d30e9424848bm2(int _c2d6bbbf6c4ff9354e65d30e9424848ba)
    {
        return (int)(3.1415926535897932384626433832795028841 * _c2d6bbbf6c4ff9354e65d30e9424848ba * _c2d6bbbf6c4ff9354e65d30e9424848ba);
    }

    public int _c2d6bbbf6c4ff9354e65d30e9424848bm(int _c2d6bbbf6c4ff9354e65d30e9424848ba,int _c2d6bbbf6c4ff9354e65d30e9424848b97,int _c2d6bbbf6c4ff9354e65d30e9424848bc = 0) 
    {
        int t_c2d6bbbf6c4ff9354e65d30e9424848bap = _c2d6bbbf6c4ff9354e65d30e9424848ba * _c2d6bbbf6c4ff9354e65d30e9424848b97;
        if (_c2d6bbbf6c4ff9354e65d30e9424848bc != 0 && t_c2d6bbbf6c4ff9354e65d30e9424848bap > _c2d6bbbf6c4ff9354e65d30e9424848bc)
        {
            t_c2d6bbbf6c4ff9354e65d30e9424848bap = t_c2d6bbbf6c4ff9354e65d30e9424848bap / _c2d6bbbf6c4ff9354e65d30e9424848bc;
        }
        else
        {
            t_c2d6bbbf6c4ff9354e65d30e9424848bap -= _c2d6bbbf6c4ff9354e65d30e9424848bc;
        }

        return _c2d6bbbf6c4ff9354e65d30e9424848bm2(t_c2d6bbbf6c4ff9354e65d30e9424848bap);
    }
}
