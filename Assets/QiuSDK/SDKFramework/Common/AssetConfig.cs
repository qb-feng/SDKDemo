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