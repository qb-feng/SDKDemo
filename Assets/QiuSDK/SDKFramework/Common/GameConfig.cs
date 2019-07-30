using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace N3DClient
{
    public static class GameConfig
    {
        private static Dictionary<string, string> mClientConfigMap = new Dictionary<string, string>();
        private static Dictionary<string, string> mServerConfigMap = new Dictionary<string, string>();

        static GameConfig()
        {
            DefaultFont = null;
            DefaultSprite = null;
            DefaultAnimatorController = null;

            string configPath = FileUtils.CombinePath(AssetConfig.SDKStreamingAssetsPath, "config.game");
            byte[] data = FileUtils.GetFileData(configPath);
            string configData = System.Text.Encoding.UTF8.GetString(data);
            if (string.IsNullOrEmpty(configData)) 
            {
                Debug.LogError("config.game 配置表导入失败！");
                return;
            }
            Dictionary<string, object> _mClientConfigMap = LitJson.JsonMapper.ToObject<Dictionary<string, object>>(configData);
            foreach (var v in _mClientConfigMap)
            {
                mClientConfigMap.Add(v.Key, v.Value.ToString());
            }
            Debug.LogWarning("GameConfig 初始化成功！");

            LogEnable = GetClientConfigBool("LogEnable", false);
        }

        public static string GetClientConfig(string key, string defaultValue = "")
        {
            string value = "";
            if (mClientConfigMap.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }

        public static void SetClientConfig(string key, string value)
        {
            string oldVal = "";
            if (mClientConfigMap.TryGetValue(key, out oldVal))
            {
                mClientConfigMap[key] = value;
            }
            else
            {
                mClientConfigMap.Add(key, value);
            }
        }

        public static string GetRootUrl()
        {
            string urlMode = GetClientConfig("RootUrlMode");
            if (urlMode == "dev")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "base")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "trial")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "zj")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "leniu_base")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "gonghui_base")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "hktw_dev")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "hktw_base")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "korea_dev")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "korea_base")
                return "http://192.168.1.206:6566/wx/api_active.php";
            else if (urlMode == "hj")
                return "http://xqj.gznemo.shmagicsword.com/wx/api_active.php";
            else if (urlMode == "linyou")
            {
                var district = GetClientConfigInt("District");
                if (district == 2) //港澳台
                    return "http://mhdqgm.originmood.com/wx/api_active.php";
                else if (district == 3) // 韩国
                {
                    return "http://gmtool.sjgmachine.panggame.com/wx/api_active.php";
                }
                return "http://manager.xqj.linygame.com/wx/api_active.php";
            }
            else if (urlMode == "leniu")
                return "http://xqj.center.lnert.com/wx/api_active.php";
            else
                return "";
        }

        public static int GetClientConfigInt(string key, int defaultValue = 0)
        {
            string value = "";
            if (mClientConfigMap.TryGetValue(key, out value))
            {
                int outVal = 0;
                if (int.TryParse(value, out outVal))
                    return outVal;
            }
            return defaultValue;
        }

        public static bool GetClientConfigBool(string key, bool defaultValue = false)
        {
            string value = "";
            if (mClientConfigMap.TryGetValue(key, out value))
            {
                bool outVal = false;
                if (bool.TryParse(value, out outVal))
                    return outVal;
            }
            return defaultValue;
        }

        public static string GetServerConfig(string key)
        {
#if UNITY_STANDALONE_WIN || UNITY_IPHONE
            if (key == "version_url" || key == "res_url")
            {
                string url = GetClientConfig("ResUrl");
                if (!string.IsNullOrEmpty(url))
                    return url;
            }
#endif

            //	if (key == "version_url" || key == "res_url")
            //	{
            //		return "http://192.168.0.123:8080/downloads/";
            //	}

            string value = "";
            mServerConfigMap.TryGetValue(key, out value);
            return value;
        }

        public static void SetServerConfig(string key, string value)
        {
            if (mServerConfigMap.ContainsKey(key))
                mServerConfigMap[key] = value;
            else
                mServerConfigMap.Add(key, value);
        }

        public static void AddServerConfig(string key, string value)
        {
            mServerConfigMap.Add(key, value);
        }

        public static void ClearServerConfig()
        {
            mServerConfigMap.Clear();
        }

        #region Default Value
        static public Font DefaultFont { get; set; }
        static public Sprite DefaultSprite { get; set; }
        static public RuntimeAnimatorController DefaultAnimatorController { get; set; }
        static public bool LogEnable { get; private set; }

        #endregion
    }
}
