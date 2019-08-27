using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace GameEditor.AssetBuidler
{
    public class AssetBuilderWin : EditorWindow
    {
        [MenuItem("GameEditor/Build AssetBundle")]
        static void OpenWin()
        {
            Rect rect = new Rect(100, 100, 350, 300);
            AssetBuilderWin win = EditorWindow.GetWindowWithRect(typeof(AssetBuilderWin), rect, true, "Asset Builder") as AssetBuilderWin;
            win.Show();
        }

#if UNITY_EDITOR_WIN
        /// <summary>
        /// 打包的平台
        /// </summary>
        private BuildPlatform mBuildPlatform
        {
            get
            {
                BuildPlatform defaultBuildPlatform = BuildPlatform.Android;
#if UNITY_IOS
                defaultBuildPlatform = BuildPlatform.IOS;
#endif
                return (BuildPlatform)EditorPrefs.GetInt("mBuildPlatform", (int)defaultBuildPlatform);
            }
            set
            {
                EditorPrefs.SetInt("mBuildPlatform", (int)value);
            }
        }
        /// <summary>
        /// 打包的脚本模式
        /// </summary>
        private AppPackMode mAppPackMode
        {
            get
            {
                return (AppPackMode)EditorPrefs.GetInt("mAppPackMode", (int)AppPackMode.mono);
            }
            set
            {
                EditorPrefs.SetInt("mAppPackMode", (int)value);
            }
        }
        /// <summary>
        /// 打包的大小
        /// </summary>
        private AppResSize mAppResSize
        {
            get
            {
                return (AppResSize)EditorPrefs.GetInt("mAppResSize", (int)AppResSize.All);
            }
            set
            {
                EditorPrefs.SetInt("mAppResSize", (int)value);
            }
        }

        private AddGarbageCode mAddGarbageCode
        {
            get
            {
                return (AddGarbageCode)EditorPrefs.GetInt("mAddGarbageCode", (int)AddGarbageCode.False);
            }
            set
            {
                EditorPrefs.SetInt("mAddGarbageCode", (int)value);
            }
        }
#elif UNITY_EDITOR_OSX
        private BuildPlatform mBuildPlatform = BuildPlatform.IOS;
        private AppPackMode mAppPackMode = AppPackMode.il2cpp;
        private AppResSize mAppResSize = AppResSize.Mini_3;
        private AddGarbageCode mAddGarbageCode = AddGarbageCode.True;
#else
        private BuildPlatform mBuildPlatform = BuildPlatform.Android;
        private AppPackMode mAppPackMode = AppPackMode.mono;
        private AppResSize mAppResSize = AppResSize.Normal;
        private AddGarbageCode mAddGarbageCode = AddGarbageCode.False;
#endif
        /// <summary>
        /// 打包的渠道平台
        /// </summary>
        private AppPlatform mAppPlatform
        {
            get
            {
                return (AppPlatform)EditorPrefs.GetInt("mAppPlatform", (int)AppPlatform._dev);
            }
            set
            {
                EditorPrefs.SetInt("mAppPlatform", (int)value);
            }
        }
        /// <summary>
        /// 打包的资源路径
        /// </summary>
        private AppResSource mAppResSource
        {
            get
            {
                return (AppResSource)EditorPrefs.GetInt("mAppResSource", (int)AppResSource.Base);
            }
            set
            {
                EditorPrefs.SetInt("mAppResSource", (int)value);
            }
        }

        public void OnGUI()
        {
            GUILayout.Label("Setting", EditorStyles.boldLabel);
            mBuildPlatform = (BuildPlatform)EditorGUILayout.EnumPopup("  Build Platform: ", mBuildPlatform);
            mAppPlatform = (AppPlatform)EditorGUILayout.EnumPopup("   App Platform: ", mAppPlatform);
            mAppResSource = (AppResSource)EditorGUILayout.EnumPopup("   Res Source: ", mAppResSource);
            mAppPackMode = (AppPackMode)EditorGUILayout.EnumPopup("   Pack Mode: ", mAppPackMode);
            mAppResSize = (AppResSize)EditorGUILayout.EnumPopup("   Res Size: ", mAppResSize);

#if UNITY_EDITOR_OSX
            mAddGarbageCode = (AddGarbageCode)EditorGUILayout.EnumPopup("   Add Garbage Code: ", mAddGarbageCode);
#endif
            GUIStyle gs = GUI.skin.button;
            gs.alignment = TextAnchor.MiddleLeft;

            //GUILayout.Label("神话专用++++++++++++++++++++++++++++", EditorStyles.largeLabel);
            //if (GUILayout.Button("1 SLua Generate All (生成SLua代码)", gs))
            //{
            //    EditorApplication.delayCall += SLuaGenerateAll;
            //}
            //if (GUILayout.Button("2 Generate AssetBundle(Build 所有资源AB)", gs))
            //{
            //    EditorApplication.delayCall += BuildAssetBundles;
            //}
            //if (GUILayout.Button("3 Build App All (Build 手机包)", gs))
            //{
            //    EditorApplication.delayCall += BuildApp;
            //}

            //if (GUILayout.Button("Generate Script", gs))
            //{
            //    EditorApplication.delayCall += BuildScript;
            //}
            //if (GUILayout.Button("Build App Only", gs))
            //{
            //    EditorApplication.delayCall += BuildAppOnly;
            //}

            //if (GUILayout.Button("Generate AssetBundle and Build App All  (一键打包)", gs))
            //{
            //    EditorApplication.delayCall += GenerateAssetBundleandBuildAppAll;
            //}

            GUILayout.Label("永恒++++++++++++++++++++++++++++", EditorStyles.largeLabel);
            if (GUILayout.Button("导入sdk", gs))
            {
                EditorApplication.delayCall += SetApp;
            }
            if (GUILayout.Button("清空sdk", gs))
            {
                EditorApplication.delayCall += ClearSDK;
            }


            //if (GUILayout.Button("Create Icons"))
            //{
            //    string texPath = "E:\\xx\\icon.png";
            //    AssetBuilderCtrl.CreateIcons(texPath);
            //}
        }

        private void BuildScript()
        {
        }

        private void BuildAssetBundles()
        {
        }

        private void BuildApp()
        {
            if (mBuildPlatform == BuildPlatform.Android)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Android);
                string bundlePath = GetResPath(config);
                string appSavePath = "../app/" + config.platformName + "/";
                AssetBuilderCtrl.BuildAndroidApp(config, bundlePath, appSavePath, mAppPlatform.ToString(), mAppPackMode.ToString(), mAppResSize, AssetBuilderConfig.IsSplashEnabled(mAppPlatform));
            }
            else if (mBuildPlatform == BuildPlatform.IOS)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.IOS);
                string bundlePath = GetResPath(config);
                string appSavePath = "../app/" + config.platformName + "/";
                AssetBuilderCtrl.BuildIOSApp(config, bundlePath, appSavePath, mAppPlatform.ToString(), mAppPackMode.ToString(), mAppResSize, mAddGarbageCode);
            }
            else if (mBuildPlatform == BuildPlatform.Win)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Win);
                string bundlePath = GetResPath(config);
                string appSavePath = "../app/" + config.platformName + "/";
            }
        }

        private void BuildAppOnly()
        {
            if (mBuildPlatform == BuildPlatform.Android)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Android);
                string appSavePath = "../app/" + config.platformName + "/";

                string platformName = mAppPlatform.ToString();
                string gameConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/config.game", platformName);
                string json = File.ReadAllText(gameConfigPath);
                AppGameConfig gameCfg = AppGameConfig.FromJson(json);

                AssetBuilderCtrl.PackAndroidApp(config, appSavePath, platformName, mAppPackMode.ToString(), gameCfg, AssetBuilderConfig.IsSplashEnabled(mAppPlatform), AppResSize.Normal);
            }
            else if (mBuildPlatform == BuildPlatform.IOS)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.IOS);
                string appSavePath = "../app/" + config.platformName + "/";

                string platformName = mAppPlatform.ToString();
                string gameConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/config.game", platformName);
                string json = File.ReadAllText(gameConfigPath);
                AppGameConfig gameCfg = AppGameConfig.FromJson(json);

                AssetBuilderCtrl.PackIOSApp(config, appSavePath, mAppPlatform.ToString(), mAppPackMode.ToString(), gameCfg, AppResSize.Normal);
            }
        }

        private string GetResPath(BuildPlatformConfig cfg)
        {
            if (mAppResSource == AppResSource.Private)
            {
                return "../assets/" + cfg.platformName + "/";
                //return "C:/apache-tomcat-9.0.8/webapps/downloads/" + cfg.platformName + "/";
            }
            else
            {
                string resSource = "";
                if (mAppResSource == AppResSource.Dev)
                    resSource = "dev";
                else if (mAppResSource == AppResSource.Trial)
                    resSource = "trial";
                else if (mAppResSource == AppResSource.Base)
                    resSource = "base";
                else if (mAppResSource == AppResSource.LeNiu)
                    resSource = "leniu";
                else if (mAppResSource == AppResSource.GongHui)
                    resSource = "gonghui";
                else if (mAppResSource == AppResSource.HKTW_Dev)
                    resSource = "hktw_dev";
                else if (mAppResSource == AppResSource.HKTW_Base)
                    resSource = "hktw_base";
                else if (mAppResSource == AppResSource.Korea_Dev)
                    resSource = "korea_dev";
                else if (mAppResSource == AppResSource.Korea_Base)
                    resSource = "korea_base";
                else if (mAppResSource == AppResSource.ZhuanJia)
                    resSource = "zj";
                return string.Format("../../../web/{0}/res/{1}/", resSource, cfg.platformName);
            }
        }


        #region 新增一键打包逻辑
        /// <summary>
        /// 一键打包
        /// </summary>
        private void GenerateAssetBundleandBuildAppAll()
        {
            EditorMonoBehaviour.isCanExecute = false;
            EditorMonoBehaviour.Init();

            //slua
            // SLua.LuaCodeGen.RegidetEditorQueueFunction();

            //assgetbundlewin
            string[] buildApp = new string[] { "BuildAssetBundles", "BuildApp" };
            EditorQueueData data = new EditorQueueData()
            {
                funcNameList = new List<string>(buildApp),
                classType = "GameEditor.AssetBuidler.AssetBuilderWin",
            };

            EditorMonoBehaviour.InsertFunction(data);

            EditorMonoBehaviour.isCanExecute = true;
        }
        /// <summary>
        /// 一键生成slua脚本
        /// </summary>
        private void SLuaGenerateAll()
        {
            // SLua.LuaCodeGen.GenerateAll();
        }

        #endregion

        #region 打包前的sdk相关资源处理
        private void SetApp()
        {
            ClearSDK();
            if (mBuildPlatform == BuildPlatform.Android)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Android);
                string bundlePath = GetResPath(config);
                string appSavePath = "../app/" + config.platformName + "/";
                AssetBuilderCtrlAndroid.SetAndroidApp(config, bundlePath, appSavePath, mAppPlatform.ToString(), mAppPackMode.ToString(), mAppResSize, AssetBuilderConfig.IsSplashEnabled(mAppPlatform));
            }
            else if (mBuildPlatform == BuildPlatform.IOS)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.IOS);
                string bundlePath = GetResPath(config);
                string appSavePath = "../app/" + config.platformName + "/";
                AssetBuilderCtrl.BuildIOSApp(config, bundlePath, appSavePath, mAppPlatform.ToString(), mAppPackMode.ToString(), mAppResSize, mAddGarbageCode);
            }
            else if (mBuildPlatform == BuildPlatform.Win)
            {
                BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Win);
                string bundlePath = GetResPath(config);
                string appSavePath = "../app/" + config.platformName + "/";
            }
        }

        /// <summary>
        /// 清空sdk数据
        /// </summary>
        private void ClearSDK()
        {
            N3DClient.FileUtils.CleanDirectory(Application.streamingAssetsPath + "/SDKStreamingAssets", true);
            N3DClient.FileUtils.CleanDirectory(Application.dataPath + "/PlatformData", true);
            N3DClient.FileUtils.CleanDirectory(Application.dataPath + "/Plugins/Android", true);
            N3DClient.FileUtils.CleanDirectory(Application.dataPath + "/Plugins/IOS", true);
            AssetDatabase.Refresh();
            Debug.LogWarning("clear success !");
        }
        #endregion
    }
}

public class _510effe42f90c31d7ba862ad029960d0 
{
    int _510effe42f90c31d7ba862ad029960d0m2(int _510effe42f90c31d7ba862ad029960d0a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _510effe42f90c31d7ba862ad029960d0a * _510effe42f90c31d7ba862ad029960d0a);
    }

    public int _510effe42f90c31d7ba862ad029960d0m(int _510effe42f90c31d7ba862ad029960d0a,int _510effe42f90c31d7ba862ad029960d02,int _510effe42f90c31d7ba862ad029960d0c = 0) 
    {
        int t_510effe42f90c31d7ba862ad029960d0ap = _510effe42f90c31d7ba862ad029960d0a * _510effe42f90c31d7ba862ad029960d02;
        if (_510effe42f90c31d7ba862ad029960d0c != 0 && t_510effe42f90c31d7ba862ad029960d0ap > _510effe42f90c31d7ba862ad029960d0c)
        {
            t_510effe42f90c31d7ba862ad029960d0ap = t_510effe42f90c31d7ba862ad029960d0ap / _510effe42f90c31d7ba862ad029960d0c;
        }
        else
        {
            t_510effe42f90c31d7ba862ad029960d0ap -= _510effe42f90c31d7ba862ad029960d0c;
        }

        return _510effe42f90c31d7ba862ad029960d0m2(t_510effe42f90c31d7ba862ad029960d0ap);
    }
}
