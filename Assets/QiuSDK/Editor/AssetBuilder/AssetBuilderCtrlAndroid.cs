using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using System.Text.RegularExpressions;

#if UNITY_5_6_OR_NEWER
using FreeImageAPI;
#endif

using N3DClient;
using Random = UnityEngine.Random;
using LitJson;

namespace GameEditor.AssetBuidler
{
    public class AssetBuilderCtrlAndroid
    {
        public static string SDKStreamingAssetsFullName = "SDKStreamingAssets";
        public static string SDKStreamingAssetsFull = Application.streamingAssetsPath + "/" + SDKStreamingAssetsFullName;
        public static string configGame_gameFull = SDKStreamingAssetsFull + "/config.game";

        #region Set Android
        public static void ResetConfig()
        {
            PlayerSettings.companyName = "Nemo";
            PlayerSettings.productName = "N3DGame";
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.use32BitDisplayBuffer = true;
            PlayerSettings.colorSpace = ColorSpace.Gamma;
            PlayerSettings.MTRendering = true;
            PlayerSettings.bakeCollisionMeshes = true;
            PlayerSettings.stripEngineCode = true;
            PlayerSettings.stripUnusedMeshComponents = true;

#if UNITY_5_6_OR_NEWER
            QualitySettings.softParticles = false;
#endif

        }

        /// <summary>
        /// 设置安卓app的参数  外部调用
        /// </summary>
        public static void SetAndroidApp(BuildPlatformConfig config, string bundlePath, string appSavePath, string platformName, string packMode, AppResSize resSize = AppResSize.Normal, bool isSplashEnabled = true)
        {
            string gameConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/config.game", platformName);

            string json = File.ReadAllText(gameConfigPath);
            AppGameConfig gameCfg = AppGameConfig.FromJson(json);

            if (Directory.Exists(SDKStreamingAssetsFull))
                AssetBuilderCtrl.CleanDirectory(SDKStreamingAssetsFull);
            else
                Directory.CreateDirectory(SDKStreamingAssetsFull);

            File.Copy(gameConfigPath, configGame_gameFull);
            AssetDatabase.Refresh();

            CopyAndroidPlugins(config, platformName, packMode);
            ReplaceAndroidManifestPushInfo(config, platformName, gameCfg);

            CopyAndroidAssets(config, platformName, packMode);

            SetAndroidAppBuildSetting(config, appSavePath, platformName, packMode, gameCfg, isSplashEnabled, resSize);
        }

        #region 通用设置

        /// <summary>
        /// 设置打包时unity的所有参数
        /// </summary>
        private static void SetAndroidAppBuildSetting(BuildPlatformConfig config, string appSavePath, string platformName, string packMode, AppGameConfig gameCfg, bool isSplashEnabled, AppResSize resSize)
        {
            gameCfg.ResetAppVersion();
            switch (resSize)
            {
                case AppResSize.Mini:
                    gameCfg.DownloadPriority = 9800;
                    break;
                case AppResSize.Mini_1:
                    gameCfg.DownloadPriority = 9800;
                    break;
                case AppResSize.Mini_2:
                    gameCfg.DownloadPriority = 9000;
                    break;
                case AppResSize.Mini_3:
                    gameCfg.DownloadPriority = 7400;
                    break;
                case AppResSize.Normal:
                    gameCfg.DownloadPriority = 1000;
                    break;
                default:
                    break;
            }

            // load pack config
            string packConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/default.pack", platformName);
            string packCfgJson = File.ReadAllText(packConfigPath);
            AndroidPackConfig packCfg = AndroidPackConfig.FromJson(packCfgJson);

            //设置安卓icon，动画
            SetAndroidSpalshIcon(platformName, gameCfg);
            AssetDatabase.Refresh();

            // save game config  永恒不save，全部由表自己控制
            //File.WriteAllText(configGame_gameFull, gameCfg.ToJson());
            //AssetDatabase.Refresh();

            // build app
            SetAndroidBuildSetting(packCfg, gameCfg, packMode);
            AssetDatabase.Refresh();

            Debug.LogWarning("Set AndroidApp BuildSetting success!");
        }

        /// <summary>
        /// 设置安卓的spalshicon  可通用
        /// </summary>
        private static void SetAndroidSpalshIcon(string platformName, AppGameConfig gameCfg)
        {
            //先清理资源文件
            FileUtils.CleanDirectory(UnityEngine.Application.dataPath + "/PlatformData/");
            AssetDatabase.Refresh();

            #region   unity splash
#if UNITY_5_6_OR_NEWER
            {
                List<PlayerSettings.SplashScreenLogo> logoList = new List<PlayerSettings.SplashScreenLogo>();

                string screenSourcePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/splash_screen/", platformName);//源目录文件夹

                string screenTargetPath = string.Format(UnityEngine.Application.dataPath + "/PlatformData/{0}/android/splash_screen/", platformName);

                //存在splash文件时,拷贝到unity
                if (Directory.Exists(screenSourcePath))
                {
                    if (Directory.Exists(screenTargetPath))
                    {
                        AssetBuilderCtrl.CleanDirectory(screenTargetPath);
                    }
                    Directory.CreateDirectory(screenTargetPath);
                    AssetBuilderCtrl.CopyDirectory(screenSourcePath, screenTargetPath);//拷贝文件夹资源
                    AssetDatabase.Refresh();
                    screenTargetPath = string.Format("Assets/PlatformData/{0}/android/splash_screen/", platformName);
                }

                if (Directory.Exists(screenTargetPath))
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string logoPath = screenTargetPath + i + ".png";

                        if (!File.Exists(logoPath))
                            logoPath = screenTargetPath + i + ".jpg";

                        if (File.Exists(logoPath))
                        {
                            Sprite logoSp = (Sprite)AssetDatabase.LoadAssetAtPath(logoPath, typeof(Sprite));

                            if (logoSp == null)
                            {
                                TextureImporter import = AssetImporter.GetAtPath(logoPath) as TextureImporter;
                                import.textureType = TextureImporterType.Sprite;
                                import.spriteImportMode = SpriteImportMode.Single;
                                import.mipmapEnabled = false;
                                import.sRGBTexture = true;
                                import.alphaSource = TextureImporterAlphaSource.FromInput;
                                import.anisoLevel = 16;

                                AssetDatabase.ImportAsset(logoPath);

                                logoSp = (Sprite)AssetDatabase.LoadAssetAtPath(logoPath, typeof(Sprite));
                            }

                            if (logoSp != null)
                            {
                                PlayerSettings.SplashScreenLogo logo =
                                    PlayerSettings.SplashScreenLogo.Create(2.5f, logoSp);
                                logoList.Add(logo);
                            }
                        }
                    }

                    PlayerSettings.SplashScreen.logos = logoList.ToArray();
                    PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.AllSequential;
                    PlayerSettings.SplashScreen.backgroundColor = Color.white;
                    PlayerSettings.SplashScreen.unityLogoStyle = PlayerSettings.SplashScreen.UnityLogoStyle.DarkOnLight;
                    PlayerSettings.SplashScreen.show = true;
                    PlayerSettings.SplashScreen.showUnityLogo = false;
                }
                else
                {
                    PlayerSettings.SplashScreen.show = false;
                    PlayerSettings.SplashScreen.showUnityLogo = false;
                }
                //  Directory.Delete(screenTargetPath);//设置完后删除？
            }
#endif
            #endregion

            #region    splash
            {
                string screenSourcePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/assets/splash/", platformName);//源目录文件夹
                string screenTargetPath = string.Format(UnityEngine.Application.dataPath + "/PlatformData/{0}/android/assets/splash/", platformName);
                //存在splash文件时,拷贝到unity
                if (Directory.Exists(screenSourcePath))
                {
                    if (Directory.Exists(screenTargetPath))
                    {
                        AssetBuilderCtrl.CleanDirectory(screenTargetPath);
                    }
                    Directory.CreateDirectory(screenTargetPath);
                    AssetBuilderCtrl.CopyDirectory(screenSourcePath, screenTargetPath);//拷贝文件夹资源
                    AssetDatabase.Refresh();
                    screenTargetPath = string.Format("Assets/PlatformData/{0}/android/assets/splash/", platformName);
                }

                int splashCount = 0;
                for (int i = 1; i <= 3; i++)
                {
                    string iconPath = string.Format("{0}{1}.png", screenTargetPath, i);
                    Texture2D tex2d = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
                    if (tex2d != null)
                    {
                        splashCount++;
                        PlayerSettings.showUnitySplashScreen = false;

#if UNITY_2017_1_OR_NEWER
                        PlayerSettings.virtualRealitySplashScreen = tex2d;
#endif
                    }
                    else
                        break;

                }
                AssetDatabase.Refresh();
                gameCfg.Splash = splashCount;
            }
            PlayerSettings.showUnitySplashScreen = false;
            #endregion

            #region Set icon
            {
                string screenSourcePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/icon/", platformName);//源目录文件夹
                string screenTargetPath = string.Format(UnityEngine.Application.dataPath + "/PlatformData/{0}/android/icon/", platformName);
                if (Directory.Exists(screenSourcePath))
                {
                    if (Directory.Exists(screenTargetPath))
                    {
                        AssetBuilderCtrl.CleanDirectory(screenTargetPath);
                    }
                    Directory.CreateDirectory(screenTargetPath);
                    AssetBuilderCtrl.CopyDirectory(screenSourcePath, screenTargetPath);//拷贝文件夹资源
                    AssetDatabase.Refresh();
                }
                screenTargetPath = string.Format("Assets/PlatformData/{0}/android/icon/", platformName);
                int[] sizeList = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
                Texture2D[] icons = new Texture2D[sizeList.Length];
                for (int i = 0; i < sizeList.Length; i++)
                {
                    string iconPath = string.Format("{0}icon_{1}.png", screenTargetPath, sizeList[i]);
                    Texture2D textIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath, typeof(Texture2D));
                    icons[i] = textIcon;
                }

                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, icons);
            }
            #endregion

        }

        /// <summary>
        /// 设置buildsetting文字参数 可通用
        /// </summary>
        private static void SetAndroidBuildSetting(AndroidPackConfig packCfg, AppGameConfig gameCfg, string packMode)
        {
            ResetConfig();
            PlayerSettings.Android.androidIsGame = true;
            PlayerSettings.Android.disableDepthAndStencilBuffers = false;
            PlayerSettings.Android.showActivityIndicatorOnLoading = AndroidShowActivityIndicatorOnLoading.DontShow;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel16;
            PlayerSettings.Android.targetDevice = AndroidTargetDevice.FAT;
            PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
            PlayerSettings.Android.forceInternetPermission = true;
            PlayerSettings.Android.forceSDCardPermission = true;
#if UNITY_5_6_OR_NEWER
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, packCfg.PackageName);
#else
            PlayerSettings.bundleIdentifier = packCfg.PackageName;
#endif
            PlayerSettings.productName = packCfg.ProductName;
            PlayerSettings.companyName = packCfg.CompanyName;
            PlayerSettings.bundleVersion = gameCfg.GetAppVersion();

            EditorUserBuildSettings.allowDebugging = false;
#if UNITY_5_6_OR_NEWER
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Internal;
            EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
#endif
            EditorUserBuildSettings.connectProfiler = false;
            EditorUserBuildSettings.development = false;

            //PlayerSettings.Android.keystoreName = "user.keystore";
            //PlayerSettings.Android.keystorePass = "t1nemo123";
            //PlayerSettings.Android.keyaliasName = "t1nemo";
            //PlayerSettings.Android.keyaliasPass = "t1nemo123";

            if (packMode == "mono")
            {
#if UNITY_5_6_OR_NEWER
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
#else
                PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.Mono2x, BuildTarget.Android);
#endif
            }
            else
            {
#if UNITY_5_6_OR_NEWER
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_2_0);
#else
                PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTarget.Android);
                PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
#endif
            }
        }
        #endregion

        #region plugins资源操作
        /// <summary>
        /// 修改AndroidManifest.xml，可通用
        /// </summary>
        private static void ReplaceAndroidManifestPushInfo(BuildPlatformConfig config, string platformName, AppGameConfig gameCfg)
        {
            return;
            string tempContent = string.Empty;
            string filePath = Application.dataPath + config.pluginPath.Replace("Assets", "") + "AndroidManifest.xml";
            if (!File.Exists(filePath))
                return;

            string packConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/default.pack", platformName);
            string packCfgJson = File.ReadAllText(packConfigPath);
            AndroidPackConfig packCfg = AndroidPackConfig.FromJson(packCfgJson);

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    tempContent = sr.ReadToEnd();
                    tempContent = tempContent.Replace("__your_package_name", packCfg.PackageName);
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(tempContent);
            }
        }

        /// <summary>
        /// 拷贝plugins资源，可通用（拷贝前会先清除）
        /// </summary>
        private static void CopyAndroidPlugins(BuildPlatformConfig config, string platformName, string packMode)
        {
            string pluginPath = config.pluginPath;

            // clear plugin dir
            {
                string platfromDataPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/{1}", platformName, packMode);
                if (Directory.Exists(pluginPath))
                {
                    Directory.Delete(pluginPath, true);
                    Directory.CreateDirectory(pluginPath);
                }
                AssetBuilderCtrl.CopyDirectory(platfromDataPath, pluginPath);
                AssetDatabase.Refresh();
            }

            // copy android data
            string resPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/res", platformName);
            if (Directory.Exists(resPath))
            {
                string resSavePath = pluginPath + "res";
                Directory.CreateDirectory(resSavePath);
                AssetBuilderCtrl.CopyDirectory(resPath, resSavePath);
                AssetDatabase.Refresh();
            }

            {
                string binSavePath = pluginPath + "bin";
                Directory.CreateDirectory(binSavePath);

                string binPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/bin", platformName);
                if (Directory.Exists(binPath))
                {
                    AssetBuilderCtrl.CopyDirectory(binPath, binSavePath);
                    AssetDatabase.Refresh();

                    DirectoryInfo dir = new DirectoryInfo(binSavePath);
                    foreach (FileInfo item in dir.GetFiles())
                    {
                        if (item.Extension == ".jar")
                        {
                            string jarPath = binSavePath + "/" + item.Name;
                            PluginImporter importer = AssetImporter.GetAtPath(jarPath) as PluginImporter;
                            importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
                            importer.SaveAndReimport();
                        }
                    }
                    AssetDatabase.Refresh();
                }
            }

            {
                string libsSavePath = pluginPath + "libs";
                Directory.CreateDirectory(libsSavePath);

                string libsPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/libs", platformName);
                if (Directory.Exists(libsPath))
                {
                    AssetBuilderCtrl.CopyDirectory(libsPath, libsSavePath);
                    AssetDatabase.Refresh();

                    DirectoryInfo dir = new DirectoryInfo(libsSavePath);
                    foreach (DirectoryInfo item in dir.GetDirectories())
                    {
                        foreach (var soFile in item.GetFiles())
                        {
                            if (soFile.Extension == ".so")
                            {
                                string soPath = libsSavePath + "/" + item.Name + "/" + soFile.Name;
                                PluginImporter importer = AssetImporter.GetAtPath(soPath) as PluginImporter;
                                importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
                                //importer.SetPlatformData(BuildTarget.Android, "CPU", "x86");
                                importer.SaveAndReimport();
                            }
                        }
                    }
                    AssetDatabase.Refresh();
                }
            }
            {
                string jarPath = pluginPath + "bin/androidu3d.jar";
                PluginImporter importer = AssetImporter.GetAtPath(jarPath) as PluginImporter;
                importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
                importer.SaveAndReimport();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 拷贝assets资源到StreamingAssets文件夹下 可通用
        /// </summary>
        private static void CopyAndroidAssets(BuildPlatformConfig config, string platformName, string packMode)
        {
            // copy android assets
            {
                string assetsPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/assets", platformName);
                if (Directory.Exists(assetsPath))
                {
                    string assetsSavePath = config.pluginPath + "assets";
                    Directory.CreateDirectory(assetsSavePath);
                    AssetBuilderCtrl.CopyDirectory(assetsPath, assetsSavePath);
                    AssetDatabase.Refresh();
                }
            }
        }
        #endregion


        #endregion
    }
}