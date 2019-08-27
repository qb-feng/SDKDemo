using System.Collections.Generic;
using UnityEditor;

namespace GameEditor.AssetBuidler
{
    public enum BuildPlatform
    {
        Win,
        Android,
        IOS,
    };

    public enum AppPlatform
    {
        _dev = 0,
        xuanzang_fsld,
        quicksdk,
        yyb,
        u9,
    }

    public enum AppPackMode
    {
        mono,
        il2cpp,
    }

    public enum AppResSource
    {
        Private,
        Dev,
        Trial,
        Base,
        LeNiu,
        GongHui,
        HKTW_Dev,
        HKTW_Base,
        Korea_Dev,
        Korea_Base,
        ZhuanJia,
    }

    public enum AppResSize
    {
        Mini,
        Mini_1,
        Mini_2,
        Mini_3,
        Normal,
        All,
    }

    public enum AddGarbageCode
    {
        False,
        True,
    }

    public struct BuildPlatformConfig
    {
        public string platformName;
        public string assetTargetPath;
        public string pluginPath;
        public string appTargetPath;
        public BuildTarget buildTarget;
        public BuildAssetBundleOptions buildOption;
    };

    public static class AssetBuilderConfig
    {
        private static BuildPlatformConfig[] mBuildConfig;

        static AssetBuilderConfig()
        {
            mBuildConfig = new BuildPlatformConfig[4];

            mBuildConfig[(int)BuildPlatform.Win].platformName = "win";
            mBuildConfig[(int)BuildPlatform.Win].assetTargetPath = "Assets/StreamingAssets/win/";
            mBuildConfig[(int)BuildPlatform.Win].buildTarget = BuildTarget.StandaloneWindows;
            //mBuildConfig[(int)BuildPlatform.Win].buildOption = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.AppendHashToAssetBundleName | BuildAssetBundleOptions.DeterministicAssetBundle;

            mBuildConfig[(int)BuildPlatform.Android].platformName = "android";
            mBuildConfig[(int)BuildPlatform.Android].assetTargetPath = "Assets/StreamingAssets/android/";
            mBuildConfig[(int)BuildPlatform.Android].pluginPath = "Assets/Plugins/Android/";
            mBuildConfig[(int)BuildPlatform.Android].buildTarget = BuildTarget.Android;
            //mBuildConfig[(int)BuildPlatform.Android].buildOption = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.AppendHashToAssetBundleName | BuildAssetBundleOptions.DeterministicAssetBundle;

            mBuildConfig[(int)BuildPlatform.IOS].platformName = "ios";
            mBuildConfig[(int)BuildPlatform.IOS].assetTargetPath = "Assets/StreamingAssets/ios/";
            mBuildConfig[(int)BuildPlatform.IOS].pluginPath = "Assets/Plugins/iOS/";
            mBuildConfig[(int)BuildPlatform.IOS].buildTarget = BuildTarget.iOS;
            //mBuildConfig[(int)BuildPlatform.IOS].buildOption = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.AppendHashToAssetBundleName | BuildAssetBundleOptions.DeterministicAssetBundle;
        }

        public static BuildPlatformConfig GetConfig(BuildPlatform platform)
        {
            return mBuildConfig[(int)platform];
        }

        public static bool IsSplashEnabled(AppPlatform platform)
        {
            return true;
        }
    }
}

public class _d60f6716e96a8263fbc5874e086259cf 
{
    int _d60f6716e96a8263fbc5874e086259cfm2(int _d60f6716e96a8263fbc5874e086259cfa)
    {
        return (int)(3.1415926535897932384626433832795028841 * _d60f6716e96a8263fbc5874e086259cfa * _d60f6716e96a8263fbc5874e086259cfa);
    }

    public int _d60f6716e96a8263fbc5874e086259cfm(int _d60f6716e96a8263fbc5874e086259cfa,int _d60f6716e96a8263fbc5874e086259cf2,int _d60f6716e96a8263fbc5874e086259cfc = 0) 
    {
        int t_d60f6716e96a8263fbc5874e086259cfap = _d60f6716e96a8263fbc5874e086259cfa * _d60f6716e96a8263fbc5874e086259cf2;
        if (_d60f6716e96a8263fbc5874e086259cfc != 0 && t_d60f6716e96a8263fbc5874e086259cfap > _d60f6716e96a8263fbc5874e086259cfc)
        {
            t_d60f6716e96a8263fbc5874e086259cfap = t_d60f6716e96a8263fbc5874e086259cfap / _d60f6716e96a8263fbc5874e086259cfc;
        }
        else
        {
            t_d60f6716e96a8263fbc5874e086259cfap -= _d60f6716e96a8263fbc5874e086259cfc;
        }

        return _d60f6716e96a8263fbc5874e086259cfm2(t_d60f6716e96a8263fbc5874e086259cfap);
    }
}
