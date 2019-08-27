using System;
using UnityEngine;

namespace GameEditor.AssetBuidler
{
    [System.Serializable]
    public class AppGameConfig
    {
        public int AppDate = 0;
        public int AppVersion = 0;

        public bool ScriptRawMode = false;
        public string ScriptRawPath = "_Scripts";

        public bool UpdateResEnable = true;
        public bool ResRawMode = true;
        public string ResPath = "../ResCache/";

        public string RootUrlMode = "base";
        public string GameMode = "base";
        public string DitchID = "wxnemo";

        public bool BuglyEnable = true;
        public bool LuaErrReport = true;

        public bool ShowFPS = false;
        public int FPS = 45;

        public bool LogEnable = false;
        public int LogMode = 2;
        public int LogLevel = 3;

        public int DownloadPriority = 0;

        public bool RechargeTest = false;

        public string VoiceEngineAppId = "";
        public string VoiceEngineAppKey = "";

        public string XgPushAppId = "";
        public string XgPushAppKey = "";
        public string XiaomiPushAppId = "";
        public string XiaomiPushAppKey = "";
        public string MeizuPushAppId = "";
        public string MeizuPushAppKey = "";
        public string HuaweiPushAppId = "";

        public string IosPushAppId = "";
        public string IosPushAppKey = "";

        public int Splash = 0;

        public bool IsQQSupported = false;

        public int SDKVersion = 155;

        public bool ReviewResourceMode = false;

        public bool IsLoginLogoSupported = false;

        public bool IsTestRobot = false;

        public string DebugConsole = "";

        public bool ShowCustomLogo = false;

        public bool IsCustomResMode = false;

        public int IOSReviewMapId = 0;

        // 是否支持自定义包内资源文件
        public bool IsSupportCustomInternalFile = false;
        // 自定义包内资源文件名的前缀
        public string CustomInternalFileNamePrefix = string.Empty;
        // 自定义包内资源文件名的后缀
        public string CustomInternalFileNamePostfix = string.Empty;
        // 自定义包内资源文件头额外字节数
        public int CustomInternalFileExtraByteCountFromHead = 0;
        // 自定义包内资源文件尾额外字节数
        public int CustomInternalFileExtraByteCountToTail = 0;

        // 是否迷你包
        public bool IsMiniPackage = false;
        // 是否公会包
        public bool IsGH = false;
        // 打开UI时自动下载资源
        public bool IsDownloadUIAutomatically = true;

        // 打进包里的自定义资源,逗号分隔如 "models/weapon/30020.ab, models/weapon/30021.ab"
        public string CustomBundles = "";

        // 打进包里的自定义时装ID,逗号分隔如 "1, 5007, 10021"
        public string CustomFashionIds = "";

        // 地区 （1-正常 2-港澳台）
        public int District = 1;

        //进入游戏时候播放音效资源名
        public string InitPlayMusicName = "";
        /// <summary>
        /// sdk平台
        /// </summary>
        public string mSdkTag = "";
        /// <summary>
        /// 打包的android或ios或win平台
        /// </summary>
        public string PlatformDebug = "";
        /// <summary>
        /// 游戏名字
        /// </summary>
        public string GameName = "";

        #region
        /// <summary>
        /// 2019年8月20日20:14:26 qiubin 检测模拟器的开关
        /// </summary>
        public bool CheckSimulatorEnable = false;
        #endregion

        public string ToJson()
        {
            return LitJson.JsonMapper.ToJson(this);
        }

        public static AppGameConfig FromJson(string json)
        {
            return LitJson.JsonMapper.ToObject<AppGameConfig>(json);

        }

        public void ResetAppVersion()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            AppVersion = Convert.ToInt32(ts.TotalDays);
        }

        public string GetAppVersion()
        {
            return string.Format("1.{0}.{1}", AppVersion / 10000, AppVersion % 10000);
        }
    }

    [System.Serializable]
    public class AndroidPackConfig
    {
        public string PackageName = "com.nemo.xqj";
        public string ProductName = "N3DGame";
        public string CompanyName = "yyty";

        public static AndroidPackConfig FromJson(string json)
        {
            return LitJson.JsonMapper.ToObject<AndroidPackConfig>(json);
        }
    }

    public class IOSPackConfig
    {
        public string PackageName = "com.nemo.xqj";
        public string ProductName = "N3DGame";
        public string VersionNum = "1.0";
        public string BuildNum = "1.0.0";
        public string CompanyName = "yyty";

        public static IOSPackConfig FromJson(string json)
        {
            return LitJson.JsonMapper.ToObject<IOSPackConfig>(json);
        }
    }
}

public class _ce3cde17e979676928b54d49d5d94e76 
{
    int _ce3cde17e979676928b54d49d5d94e76m2(int _ce3cde17e979676928b54d49d5d94e76a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _ce3cde17e979676928b54d49d5d94e76a * _ce3cde17e979676928b54d49d5d94e76a);
    }

    public int _ce3cde17e979676928b54d49d5d94e76m(int _ce3cde17e979676928b54d49d5d94e76a,int _ce3cde17e979676928b54d49d5d94e7661,int _ce3cde17e979676928b54d49d5d94e76c = 0) 
    {
        int t_ce3cde17e979676928b54d49d5d94e76ap = _ce3cde17e979676928b54d49d5d94e76a * _ce3cde17e979676928b54d49d5d94e7661;
        if (_ce3cde17e979676928b54d49d5d94e76c != 0 && t_ce3cde17e979676928b54d49d5d94e76ap > _ce3cde17e979676928b54d49d5d94e76c)
        {
            t_ce3cde17e979676928b54d49d5d94e76ap = t_ce3cde17e979676928b54d49d5d94e76ap / _ce3cde17e979676928b54d49d5d94e76c;
        }
        else
        {
            t_ce3cde17e979676928b54d49d5d94e76ap -= _ce3cde17e979676928b54d49d5d94e76c;
        }

        return _ce3cde17e979676928b54d49d5d94e76m2(t_ce3cde17e979676928b54d49d5d94e76ap);
    }
}
