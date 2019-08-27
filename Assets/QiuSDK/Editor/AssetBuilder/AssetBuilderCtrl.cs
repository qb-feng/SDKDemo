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
    public class AssetBuilderCtrl
    {
        static private string mResInputPath = "Assets/_Resources/";

        static private string mScriptPath = "Assets/_Scripts/";
        static private string mScriptConfigPath = "Assets/_Scripts/config";
        static private string mScriptZipDirPath = "Assets/_Resources/data";
        static private string mScriptZipFileName = "data.bytes";
        static private uint mScriptEncryteSeed = 0x3e5;
        static private string mScriptBundleName = "data.ab";

        static private string mGameConfigName = "config.game";
        static private string mGameConfigPath = "Assets/_GameConfig/config.game";

        static private string mFileListNameX = "filelistx";
        static private string mFileListNameX2 = "filelistx2";
        static private string mZipExt = ".db";
        static private string mDebugExt = ".debug";

        static private string mVersionFileNameX = "versionx";

        static private string mAllDependencesLuaSavePath = "Assets/_Scripts/dependences.lua";
        static private string mAllDependencesFileName = "dependences";

        static private string mDirDependencesLuaSavePath = "Assets/_Scripts/dependences_dir.lua";
        static private string mDirDependencesFileName = "dependences_dir";

        static private string mIOSUIPath = "Assets/_Resources/ui_ios";
        static private string mDefaultResPath = "Assets/_Resources/default";

        static private string mBundleExtension = ".ab";
        static private string[] mBinLibArchs = { "armabi-v7a", "x86" };

        private static readonly string x86LibPath = "/../Temp/StagingArea/libs/x86/libil2cpp.so.debug";
        private static readonly string armeabiLibPath = "/../Temp/StagingArea/libs/armeabi-v7a/libil2cpp.so.debug";

        static private int mOrgBundlePriority = 10000;

        public class ResFileInfo
        {
            public string bundleName;
            public string fileName;
            public string md5;
            public ulong crc32;
            public int fileSize;
            public int priority;
            public List<string> allDepBundleList;
            public List<string> dirDepBundleList;
        }

        #region Build Asset
        static public void BuildAssetBundles(BuildPlatformConfig config, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            Dictionary<string, ResFileInfo> resInfoMap = new Dictionary<string, ResFileInfo>();
            GenerateAssetBundle(config, savePath, ref resInfoMap);
            Dictionary<string, List<string>> allDepList = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dirDepList = new Dictionary<string, List<string>>();
            CollectDependences(resInfoMap, ref allDepList, ref dirDepList);

            //这里为了兼容旧包创建两份filelist文件
            Dictionary<string, ResFileInfo> newResInfoMap = new Dictionary<string, ResFileInfo>(resInfoMap);

            GenerateScript(config, savePath, resInfoMap, newResInfoMap, allDepList, dirDepList, true);

            RecalcPriority(resInfoMap, allDepList, false);
            RecalcPriority(newResInfoMap, allDepList, true);

            GenerateFileList(config, savePath, resInfoMap, newResInfoMap, allDepList);

            Debug.Log("Build AssetBundle Success!");
        }

        static private void GenerateAssetBundle(BuildPlatformConfig config, string savePath, ref Dictionary<string, ResFileInfo> resInfoMap)
        {
            bool isIOS = config.buildTarget == BuildTarget.iOS;
            if (isIOS)
            {
                UnityEngine.Rendering.GraphicsDeviceType[] deviceTypes = new UnityEngine.Rendering.GraphicsDeviceType[2];
                deviceTypes[0] = UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2;
                deviceTypes[1] = UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3;
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, false);
                PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, deviceTypes);
            }

            PlayerSettings.bakeCollisionMeshes = true;
            PlayerSettings.stripUnusedMeshComponents = true;
#if UNITY_5_6_OR_NEWER
            QualitySettings.softParticles = false;
#endif

            Dictionary<string, List<string>> bundleAssetsMap = new Dictionary<string, List<string>>();
            Action<string> resetFunc = (string path) =>
            {
                string bundleName = path.Remove(path.LastIndexOf('/'));
                bundleName = bundleName.Replace(mResInputPath, "") + mBundleExtension;
                bundleName = bundleName.ToLower();

                if (isIOS)
                {
                    if (bundleName == "layout_ios.ab")
                    {
                        bundleName = "layout.ab";
                    }
                    else if (bundleName == "layout.ab")
                    {
                        return;
                    }
                    else if (bundleName.StartsWith("ui_ios"))
                    {
                        bundleName = bundleName.Replace("ui_ios/", "ui/");
                        if (path.EndsWith("_alpha.png"))
                        {
                            return;
                        }
                    }
                    else if (bundleName.StartsWith("ui"))
                    {
                        return;
                    }
                }
                else if (config.buildTarget == BuildTarget.Android)
                {
                    if (bundleName == "layout_ios.ab" || bundleName.StartsWith("ui_ios"))
                    {
                        return;
                    }
                }

                if (!bundleName.Contains("data"))
                {
                    List<string> assetList = null;
                    if (!bundleAssetsMap.TryGetValue(bundleName, out assetList))
                    {
                        assetList = new List<string>();
                        bundleAssetsMap.Add(bundleName, assetList);
                    }

                    assetList.Add(path);
                }
            };
            ForeachAsset(mResInputPath, resetFunc);

            List<AssetBundleBuild> bundleBuildList = new List<AssetBundleBuild>();
            foreach (var item in bundleAssetsMap)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = item.Key;
                build.assetBundleVariant = "";
                build.assetNames = item.Value.ToArray();
                bundleBuildList.Add(build);
            }
            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(savePath, bundleBuildList.ToArray(), config.buildOption, config.buildTarget);

            CollectFileInfo(config, savePath, manifest, ref resInfoMap);

            AssetDatabase.Refresh();
            Debug.Log("Generate AssetBundle Success!");
        }

        static public void CreateOutputDir(BuildPlatformConfig config)
        {
            if (!Directory.Exists(config.assetTargetPath))
            {
                Directory.CreateDirectory(config.assetTargetPath);
                AssetDatabase.Refresh();
            }
            if (config.pluginPath != null && !Directory.Exists(config.pluginPath))
            {
                Directory.CreateDirectory(config.pluginPath);
                AssetDatabase.Refresh();
            }
            Debug.Log("Create Resources Output Dir Success!");
        }
        #endregion

        #region Generate FileList
        static private void CollectFileInfo(BuildPlatformConfig config, string savePath, AssetBundleManifest manifest, ref Dictionary<string, ResFileInfo> resInfoMap)
        {

        }

        static public void GenerateFileList(BuildPlatformConfig config, string savePath, Dictionary<string, ResFileInfo> resInfoMap, Dictionary<string, ResFileInfo> newResInfoMap, Dictionary<string, List<string>> allDepList)
        {

        }
        #endregion

        #region Collect Dependences
        static public void CollectDependences(Dictionary<string, ResFileInfo> resInfoMap, ref Dictionary<string, List<string>> allDepList, ref Dictionary<string, List<string>> dirDepList)
        {
            foreach (var item in resInfoMap)
            {
                if (item.Value.allDepBundleList != null && item.Value.allDepBundleList.Count > 0)
                {
                    AddDependency(item.Value.bundleName, item.Value.allDepBundleList, allDepList);
                }
                if (item.Value.dirDepBundleList != null && item.Value.dirDepBundleList.Count > 0)
                {
                    AddDependency(item.Value.bundleName, item.Value.dirDepBundleList, dirDepList);
                }
            }
        }

        static private void AddDependency(string bundleName, List<string> depsName, Dictionary<string, List<string>> depsMap)
        {
        }

        static private void SaveDependencyToLua(string savePath, Dictionary<string, List<string>> depsMap)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("return {");
            foreach (var item in depsMap)
            {
                if (item.Value.Count == 0)
                    continue;

                str.Append("\t[\"");
                str.Append(item.Key);
                str.Append("\"]");
                str.Append(" = {");
                for (int i = 0; i < item.Value.Count; i++)
                {
                    str.Append("\"");
                    str.Append(item.Value[i]);
                    if (i != item.Value.Count - 1)
                        str.Append("\",");
                    else
                        str.Append("\"");
                }
                str.AppendLine("},");
            }
            str.AppendLine("}");

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            File.WriteAllText(savePath, str.ToString());
        }

        static private bool IsScriptBundle(string bundleName)
        {
            return bundleName == mScriptBundleName || bundleName.StartsWith(ScriptAssetSplitConfig.BundlePrefix);
        }

        static public void RecalcPriority(Dictionary<string, ResFileInfo> resInfoMap, Dictionary<string, List<string>> depList, bool isNewFileList)
        {

        }
        #endregion

        #region Generate Script
        static public void GenerateScript(BuildPlatformConfig config, string savePath, Dictionary<string, ResFileInfo> resInfoMap, Dictionary<string, ResFileInfo> newResInfoMap, Dictionary<string, List<string>> allDepList, Dictionary<string, List<string>> dirDepList, bool saveDepToLua)
        {

        }

        static public void GenerateScriptBundle(BuildPlatformConfig config, string savePath, string scriptPath, Dictionary<string, ResFileInfo> resInfoMap)
        {
            string bundleName = scriptPath.Remove(scriptPath.LastIndexOf('/'));
            bundleName = bundleName.Replace(mResInputPath, "") + mBundleExtension;

            List<string> assetList = new List<string>();
            assetList.Add(scriptPath);

            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName;
            build.assetBundleVariant = "";
            build.assetNames = assetList.ToArray();

            List<AssetBundleBuild> bundleBuildList = new List<AssetBundleBuild>();
            bundleBuildList.Add(build);

            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(savePath, bundleBuildList.ToArray(), config.buildOption, config.buildTarget);

            // collect bundle info
            CollectFileInfo(config, savePath, manifest, ref resInfoMap);
        }
        #endregion

        #region Generate Version
        static public string GenerateVersion()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string versionStr = Convert.ToInt32(ts.TotalSeconds).ToString();
            return versionStr;
        }
        #endregion

        #region Tool
        static public void ForeachAsset(string rootPath, Action<string> action)
        {
            DirectoryInfo rootDir = new DirectoryInfo(rootPath);

            foreach (FileInfo file in rootDir.GetFiles())
            {
                if (file.Name.EndsWith(".meta") || file.Name.StartsWith("."))
                {
                    continue;
                }

                action(rootPath + file.Name);
            }

            foreach (DirectoryInfo dir in rootDir.GetDirectories())
            {
                if (dir.Name.StartsWith("."))
                {
                    continue;
                }

                ForeachAsset(rootPath + dir.Name + "/", action);
            }
        }

        static public void CleanDirectory(string dirPath)
        {
            if (FileUtils.CleanDirectory(dirPath))
                AssetDatabase.Refresh();
        }

        static private void CreateDirForFile(string basePath, string filePath)
        {
            string[] paths = filePath.Split('/');
            if (paths.Length > 1)
            {
                string path = basePath;
                for (int i = 0; i < paths.Length - 1; ++i)
                {
                    path += paths[i];
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path += "/";
                }
            }
        }
        #endregion

        #region Build Android

        public static void ReplaceAndroidManifestPushInfo(BuildPlatformConfig config, string platformName, AppGameConfig gameCfg)
        {
            string tempContent = string.Empty;
            string filePath = Application.dataPath + config.pluginPath.Replace("Assets", "") + "AndroidManifest.xml";

            string packConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/default.pack", platformName);
            string packCfgJson = File.ReadAllText(packConfigPath);
            AndroidPackConfig packCfg = AndroidPackConfig.FromJson(packCfgJson);

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    tempContent = sr.ReadToEnd();
                    tempContent = tempContent.Replace("__your_package_name", packCfg.PackageName);
                    tempContent = tempContent.Replace("__xg_push_app_id", gameCfg.XgPushAppId);
                    tempContent = tempContent.Replace("__xg_push_app_key", gameCfg.XgPushAppKey);
                    tempContent = tempContent.Replace("__huawei_push_app_id", gameCfg.HuaweiPushAppId);
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(tempContent);
            }
        }

        public static void CopyAndroidPlugins(BuildPlatformConfig config, string platformName, string packMode)
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
                CopyDirectory(platfromDataPath, pluginPath);
                AssetDatabase.Refresh();
            }

            // copy android data
            string resPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/res", platformName);
            if (Directory.Exists(resPath))
            {
                string resSavePath = pluginPath + "res";
                Directory.CreateDirectory(resSavePath);
                CopyDirectory(resPath, resSavePath);
                AssetDatabase.Refresh();
            }

            {
                string binSavePath = pluginPath + "bin";
                Directory.CreateDirectory(binSavePath);

                string binPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/bin", platformName);
                if (Directory.Exists(binPath))
                {
                    CopyDirectory(binPath, binSavePath);
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
                    CopyDirectory(libsPath, libsSavePath);
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

        public static void CopyAndroidAssets(BuildPlatformConfig config, string platformName, string packMode)
        {
            // copy android assets
            {
                string assetsPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/assets", platformName);
                if (Directory.Exists(assetsPath))
                {
                    string assetsSavePath = "Assets/StreamingAssets";
                    Directory.CreateDirectory(assetsSavePath);
                    CopyDirectory(assetsPath, assetsSavePath);
                    AssetDatabase.Refresh();
                }
            }
        }

        public static void AddFashionBundle(AppGameConfig gameCfg)
        {
            if (string.IsNullOrEmpty(gameCfg.CustomFashionIds.Trim()))
                return;

            StringBuilder sb = new StringBuilder();
            string assetsPath = UnityEngine.Application.dataPath;
            string executeLuaPath = assetsPath + "/_External/LuaTool/get_fashion_bundle_names.lua";
            string fileName = string.Empty;

#if UNITY_EDITOR_WIN
            fileName = "cmd.exe";
            sb.Append("/C ");
            sb.Append(assetsPath + "/_External/LuaTool/lua.exe" + " ");
#endif

#if UNITY_EDITOR_OSX
            fileName = "/usr/local/bin/lua";
#endif

            sb.Append(executeLuaPath);
            sb.Append(" " + UnityEngine.Application.dataPath + "/config/?.lua");
            sb.Append(" " + gameCfg.CustomFashionIds.Replace(" ", ""));

            var p = new System.Diagnostics.Process();
            var info = p.StartInfo;
            info.FileName = fileName;
            info.Arguments = sb.ToString();
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;   //重定向输出
            info.RedirectStandardError = true;
            info.UseShellExecute = false;

            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                Debug.LogError("添加时装资源失败!!" + p.ExitCode);
            }
            else
            {
                Debug.Log("添加时装资源成功!!");
            }

            string ret = p.StandardOutput.ReadToEnd();

            p.Close();

            if (!string.IsNullOrEmpty(ret))
                gameCfg.CustomBundles = string.IsNullOrEmpty(gameCfg.CustomBundles) ? ret : gameCfg.CustomBundles + ", " + ret;
        }

        public static bool CheckCustomBundleIsVaild(string bundlePath, AppGameConfig gameCfg)
        {
            if (string.IsNullOrEmpty(gameCfg.CustomBundles.Trim()))
                return true;

            string[] customResArr = gameCfg.CustomBundles.Split(',');
            Dictionary<string, bool> customResDic = customResArr.ToDictionary(item => item.Trim(), item => true);

            // get version file
            string versionPath = bundlePath + mVersionFileNameX;
            if (!File.Exists(versionPath))
            {
                Debug.LogErrorFormat("Version File not Exist! {0}", versionPath);
                return false;
            }
            string version = File.ReadAllText(versionPath);

            // get filelist info
            string fileListName = mFileListNameX2;
            string fileListDebugPath = bundlePath + fileListName + "_" + version + mDebugExt;
            if (!File.Exists(fileListDebugPath))
            {
                Debug.LogErrorFormat("FileList File not Exist! {0}", fileListDebugPath);
                return false;
            }

            byte[] fileListBytes = File.ReadAllBytes(fileListDebugPath);
            using (StreamReader stream = new StreamReader(new MemoryStream(fileListBytes)))
            {
                while (!stream.EndOfStream)
                {
                    string txt = stream.ReadLine();
                    if (!string.IsNullOrEmpty(txt))
                    {
                        string[] strArr = txt.Split('|');
                        if (strArr.Length == 5)
                        {
                            if (customResDic.ContainsKey(strArr[0]))
                                customResDic.Remove(strArr[0]);
                        }
                    }
                }
            }

            if (customResDic.Count > 0)
            {
                foreach (var kv in customResDic)
                {
                    Debug.Log("不合法的资源:" + kv.Key);
                }

                Debug.LogError("不合法的自定义资源:" + gameCfg.CustomBundles.Trim());
                return false;
            }

            return true;
        }

        public static void CopyAssetBundle(BuildPlatformConfig config, string bundlePath, string customBundles, AppResSize appResSize, int[] mapIDs)
        {

        }

        public static void BuildAndroidApp(BuildPlatformConfig config, string bundlePath, string appSavePath, string platformName, string packMode, AppResSize resSize = AppResSize.Normal, bool isSplashEnabled = true)
        {
            string gameConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/android/config.game", platformName);
            string json = File.ReadAllText(gameConfigPath);
            AppGameConfig gameCfg = AppGameConfig.FromJson(json);

            AddFashionBundle(gameCfg);
            if (!CheckCustomBundleIsVaild(bundlePath, gameCfg))
                return;

            CleanDirectory("Assets/StreamingAssets");
            CleanDirectory("Assets/_Engine/GarbageCode");
            CopyAndroidPlugins(config, platformName, packMode);
            ReplaceAndroidManifestPushInfo(config, platformName, gameCfg);
            CopyAndroidAssets(config, platformName, packMode);
            CopyAssetBundle(config, bundlePath, gameCfg.CustomBundles, resSize, null);

            PackAndroidApp(config, appSavePath, platformName, packMode, gameCfg, isSplashEnabled, resSize);
        }

        public static void PackAndroidApp(BuildPlatformConfig config, string appSavePath, string platformName, string packMode, AppGameConfig gameCfg, bool isSplashEnabled, AppResSize resSize)
        {

        }

        public static void SetAndroidBuildSetting(AndroidPackConfig packCfg, AppGameConfig gameCfg, string packMode)
        {
        }

        #endregion

        #region Build IOS
        public static void BuildIOSApp(BuildPlatformConfig config, string bundlePath, string appSavePath, string platformName, string packMode, AppResSize resSize, AddGarbageCode addGarbageCode)
        {
            string gameConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/config.game", platformName);
            string json = File.ReadAllText(gameConfigPath);
            AppGameConfig gameCfg = AppGameConfig.FromJson(json);

            AddFashionBundle(gameCfg);
            if (!CheckCustomBundleIsVaild(bundlePath, gameCfg))
                return;

            CleanDirectory("Assets/StreamingAssets");
            CleanDirectory("Assets/_Engine/GarbageCode");
            TryAddGarbageCode(addGarbageCode);
            CopyAssetBundle(config, bundlePath, gameCfg.CustomBundles, resSize, new[] { gameCfg.IOSReviewMapId });
            CopyIOSPlugins(config, platformName, packMode);
            CopyIOSAssets(config, platformName, packMode);
            ReplaceIosPushInfo(config, gameCfg);

            ClearExportPath(platformName);
            PackIOSApp(config, appSavePath, platformName, packMode, gameCfg, resSize);
            CopyReadyMixDatas(platformName);
        }

        public static void ClearExportPath(string platformName)
        {
            string exportPath = "../xcode_project/" + platformName;
            if (Directory.Exists(exportPath))
            {
                FileUtil.DeleteFileOrDirectory(exportPath);
                AssetDatabase.Refresh();
            }
        }

        public static void PackIOSApp(BuildPlatformConfig config, string appSavePath, string platformName, string packMode, AppGameConfig gameCfg, AppResSize resSize)
        {
            //gameCfg.ResetAppVersion();

            //switch (resSize)
            //{
            //    case AppResSize.Mini:
            //        gameCfg.DownloadPriority = 9800;
            //        break;
            //    case AppResSize.Mini_1:
            //        gameCfg.DownloadPriority = 9800;
            //        break;
            //    case AppResSize.Mini_2:
            //        gameCfg.DownloadPriority = 9000;
            //        break;
            //    case AppResSize.Mini_3:
            //        gameCfg.DownloadPriority = 7400;
            //        break;
            //    case AppResSize.Normal:
            //        gameCfg.DownloadPriority = 1000;
            //        break;
            //    default:
            //        break;
            //}

            //// load pack config
            //string packConfigPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/default.pack", platformName);
            //string packCfgJson = File.ReadAllText(packConfigPath);
            //IOSPackConfig packCfg = IOSPackConfig.FromJson(packCfgJson);

            //// unity splash
            //{
            //    List<PlayerSettings.SplashScreenLogo> logoList = new List<PlayerSettings.SplashScreenLogo>();

            //    string screenSourcePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/splash_screen/", platformName);//源目录文件夹

            //    string screenTargetPath = string.Format(UnityEngine.Application.dataPath + "/PlatformData/{0}/ios/splash_screen/", platformName);

            //    //存在splash文件时,拷贝到unity
            //    if (Directory.Exists(screenSourcePath))
            //    {
            //        if (Directory.Exists(screenTargetPath))
            //        {
            //            CleanDirectory(screenTargetPath);
            //        }
            //        Directory.CreateDirectory(screenTargetPath);
            //        CopyDirectory(screenSourcePath, screenTargetPath);//拷贝文件夹资源
            //        AssetDatabase.Refresh();
            //        screenTargetPath = string.Format("Assets/PlatformData/{0}/ios/splash_screen/", platformName);
            //    }


            //    if (Directory.Exists(screenTargetPath))
            //    {
            //        for (int i = 1; i <= 3; i++)
            //        {
            //            string logoPath = screenTargetPath + i + ".png";

            //            if (!File.Exists(logoPath))
            //                logoPath = screenTargetPath + i + ".jpg";

            //            if (File.Exists(logoPath))
            //            {
            //                Sprite logoSp = (Sprite)AssetDatabase.LoadAssetAtPath(logoPath, typeof(Sprite));

            //                if (logoSp == null)
            //                {
            //                    TextureImporter import = AssetImporter.GetAtPath(logoPath) as TextureImporter;
            //                    import.textureType = TextureImporterType.Sprite;
            //                    import.spriteImportMode = SpriteImportMode.Single;
            //                    import.mipmapEnabled = false;
            //                    import.sRGBTexture = true;
            //                    import.alphaSource = TextureImporterAlphaSource.FromInput;
            //                    import.anisoLevel = 16;

            //                    AssetDatabase.ImportAsset(logoPath);

            //                    logoSp = (Sprite)AssetDatabase.LoadAssetAtPath(logoPath, typeof(Sprite));
            //                }

            //                if (logoSp != null)
            //                {
            //                    PlayerSettings.SplashScreenLogo logo =
            //                        PlayerSettings.SplashScreenLogo.Create(2.5f, logoSp);
            //                    logoList.Add(logo);
            //                }
            //            }
            //        }

            //        PlayerSettings.SplashScreen.logos = logoList.ToArray();
            //        PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.AllSequential;
            //        PlayerSettings.SplashScreen.backgroundColor = Color.white;
            //        PlayerSettings.SplashScreen.unityLogoStyle = PlayerSettings.SplashScreen.UnityLogoStyle.DarkOnLight;
            //        PlayerSettings.SplashScreen.show = true;
            //        PlayerSettings.SplashScreen.showUnityLogo = false;
            //    }
            //    else
            //    {
            //        PlayerSettings.SplashScreen.show = false;
            //        PlayerSettings.SplashScreen.showUnityLogo = false;
            //    }
            //}

            ////Splash
            //{
            //    string screenSourcePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/assets/splash/", platformName);//源目录文件夹
            //    string screenTargetPath = string.Format(UnityEngine.Application.dataPath + "/PlatformData/{0}/ios/assets/splash/", platformName);
            //    //存在splash文件时,拷贝到unity
            //    if (Directory.Exists(screenSourcePath))
            //    {
            //        if (Directory.Exists(screenTargetPath))
            //        {
            //            CleanDirectory(screenTargetPath);
            //        }
            //        Directory.CreateDirectory(screenTargetPath);
            //        CopyDirectory(screenSourcePath, screenTargetPath);//拷贝文件夹资源
            //        AssetDatabase.Refresh();
            //        screenTargetPath = string.Format("Assets/PlatformData/{0}/ios/assets/splash/", platformName);
            //    }

            //    int splashCount = 0;
            //    for (int i = 1; i <= 3; i++)
            //    {
            //        string iconPath = string.Format("{0}{1}.png", screenTargetPath, i);
            //        Texture2D tex2d = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            //        if (tex2d != null)
            //            splashCount++;
            //        else
            //            break;

            //    }
            //    gameCfg.Splash = splashCount;
            //}




            //// Set icon
            ////幻剑自己出包设置icon，不用我们设置
            //if (!platformName.Contains("hj"))
            //{
            //    string screenSourcePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/icon/", platformName);//源目录文件夹
            //    string screenTargetPath = string.Format(UnityEngine.Application.dataPath + "/PlatformData/{0}/ios/icon/", platformName);
            //    if (Directory.Exists(screenSourcePath))
            //    {
            //        if (Directory.Exists(screenTargetPath))
            //        {
            //            CleanDirectory(screenTargetPath);
            //        }
            //        Directory.CreateDirectory(screenTargetPath);
            //        CopyDirectory(screenSourcePath, screenTargetPath);//拷贝文件夹资源
            //        AssetDatabase.Refresh();
            //    }
            //    screenTargetPath = string.Format("Assets/PlatformData/{0}/ios/icon/", platformName);
            //    int[] sizeList = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
            //    Texture2D[] icons = new Texture2D[sizeList.Length];
            //    for (int i = 0; i < sizeList.Length; i++)
            //    {
            //        string iconPath = string.Format("{0}icon_{1}.png", screenTargetPath, sizeList[i]);
            //        Texture2D textIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath, typeof(Texture2D));
            //        icons[i] = textIcon;
            //    }
            //    PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, icons);
            //}

            //// save game config
            //File.WriteAllText("Assets/StreamingAssets/config.game", gameCfg.ToJson());
            //AssetDatabase.Refresh();

            //// build app
            //ResetConfig();
            //PlayerSettings.stripEngineCode = false;
            //PlayerSettings.iOS.showActivityIndicatorOnLoading = iOSShowActivityIndicatorOnLoading.DontShow;
            //PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            //PlayerSettings.iOS.targetOSVersionString = "8.0";
            //PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
            //PlayerSettings.iOS.backgroundModes = iOSBackgroundMode.RemoteNotification;
            //PlayerSettings.iOS.allowHTTPDownload = true;
            //PlayerSettings.iOS.appInBackgroundBehavior = iOSAppInBackgroundBehavior.Suspend;
            //PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.FastButNoExceptions;
            //PlayerSettings.iOS.buildNumber = packCfg.BuildNum;
            //PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            //PlayerSettings.iOS.cameraUsageDescription = "应用需要访问您的相机，允许吗？";
            //PlayerSettings.iOS.microphoneUsageDescription = "使用语音聊天需要开启麦克风，允许吗？";
            //PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, packCfg.VersionNum);
            //PlayerSettings.applicationIdentifier = packCfg.PackageName;
            //PlayerSettings.productName = packCfg.ProductName;
            //PlayerSettings.bundleVersion = packCfg.VersionNum;

            //UnityEngine.Rendering.GraphicsDeviceType[] deviceTypes = new UnityEngine.Rendering.GraphicsDeviceType[2];
            //deviceTypes[0] = UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2;
            //deviceTypes[1] = UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3;
            //PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, false);
            //PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, deviceTypes);

            //EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
            //EditorUserBuildSettings.allowDebugging = false;
            //EditorUserBuildSettings.connectProfiler = false;
            //EditorUserBuildSettings.development = false;

            //if (packMode == "mono")
            //    PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            //else
            //    PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

            //// 0 - None, 1 - ARM64, 2 - Universal
            //PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 2);

            //PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_2_0);

            //string xcodeProjPath = "../xcode_project/" + platformName + "/xcode_project";
            //CleanDirectory(xcodeProjPath);

            //string[] levels = { "Assets/_Scenes/default.unity" };
            //BuildOptions option = BuildOptions.StrictMode | BuildOptions.CompressWithLz4;
            //if (packMode == "il2cpp")
            //    option |= BuildOptions.Il2CPP;

            //BuildPipeline.BuildPlayer(levels, xcodeProjPath, BuildTarget.iOS, option);

            //string appPath = string.Format("{0}n3dgame_{1}_{2}.apk", appSavePath, platformName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
            //string symbolPath = string.Format("{0}n3dgame_{1}_{2}", appSavePath, platformName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
            //if (packMode == "il2cpp")
            //    CopyAndroidIL2CPPSymbols(symbolPath);
        }

        public static void CopyReadyMixDatas(string platformName)
        {
            string xcodeProjPath = "../xcode_project/" + platformName + "/xcode_project";
            string dataPath = Path.Combine(xcodeProjPath, "Data/Raw");
            string bundlesPath = Path.Combine(dataPath, "ios");
            string configGamePath = Path.Combine(dataPath, "config.game");
            string mixToolPath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/common_ios/mix_tool");

            string exporRootPath = "../xcode_project/" + platformName;
            string exportRawResPath = Path.Combine(exporRootPath, "raw_res");
            string exportRawBundlesPath = Path.Combine(exportRawResPath, "ios");
            string exportRawConfigGamePath = Path.Combine(exportRawResPath, "config.game");
            string exportMixToolPath = Path.Combine(exporRootPath, "mix_tool");

            if (Directory.Exists(exportRawResPath))
            {
                Directory.Delete(exportRawResPath);
            }

            Directory.CreateDirectory(exportRawResPath);

            FileUtil.CopyFileOrDirectory(bundlesPath, exportRawBundlesPath);
            FileUtil.CopyFileOrDirectory(configGamePath, exportRawConfigGamePath);
            FileUtil.CopyFileOrDirectory(mixToolPath, exportMixToolPath);
            string[] files = Directory.GetFiles(exportMixToolPath, "*.meta", SearchOption.AllDirectories);
            foreach (var filePath in files)
            {
                File.Delete(filePath);
            }
        }

        public static void CopyIOSPlugins(BuildPlatformConfig config, string platformName, string packMode)
        {
            string pluginPath = config.pluginPath;

            // clear plugin dir
            {
                if (Directory.Exists(pluginPath))
                {
                    Directory.Delete(pluginPath, true);
                }
                Directory.CreateDirectory(pluginPath);
                AssetDatabase.Refresh();
            }

            {
                string codePath = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/code", platformName);
                if (Directory.Exists(codePath))
                {
                    DirectoryInfo dir = new DirectoryInfo(codePath);
                    foreach (FileInfo item in dir.GetFiles())
                    {
                        if (item.Extension != ".meta")
                        {
                            string filePath = codePath + "/" + item.Name;
                            string savePath = pluginPath + item.Name;
                            File.Copy(filePath, savePath, true);
                        }
                    }
                    AssetDatabase.Refresh();
                }
            }
        }

        public static void CopyIOSAssets(BuildPlatformConfig config, string platformName, string packMode)
        {

            // Copy review mode resources
            {
                string reviewResDir = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/assets/review_res/", platformName);
                if (Directory.Exists(reviewResDir))
                {
                    string destDir = string.Format("Assets/StreamingAssets/review_res/");
                    if (Directory.Exists(destDir))
                    {
                        Directory.Delete(destDir);
                    }

                    Directory.CreateDirectory(destDir);

                    DirectoryInfo info = new DirectoryInfo(reviewResDir);
                    foreach (var f in info.GetFiles())
                    {
                        f.CopyTo(Path.Combine(destDir, f.Name));
                    }
                }
            }

            // Copy custom mode resources
            {
                string reviewResDir = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/assets/custom_res/", platformName);
                if (Directory.Exists(reviewResDir))
                {
                    string destDir = string.Format("Assets/StreamingAssets/custom_res/");
                    if (Directory.Exists(destDir))
                    {
                        Directory.Delete(destDir);
                    }

                    Directory.CreateDirectory(destDir);

                    DirectoryInfo info = new DirectoryInfo(reviewResDir);
                    foreach (var f in info.GetFiles())
                    {
                        f.CopyTo(Path.Combine(destDir, f.Name));
                    }
                }
            }

            // Copy recharge_config resources
            {
                string reviewResDir = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/assets/recharge_config/", platformName);
                if (Directory.Exists(reviewResDir))
                {
                    string destDir = string.Format("Assets/StreamingAssets/recharge_config/");
                    if (Directory.Exists(destDir))
                    {
                        Directory.Delete(destDir);
                    }

                    Directory.CreateDirectory(destDir);

                    DirectoryInfo info = new DirectoryInfo(reviewResDir);
                    foreach (var f in info.GetFiles())
                    {
                        f.CopyTo(Path.Combine(destDir, f.Name));
                    }
                }
            }

            // Copy splash resources
            {
                string reviewResDir = string.Format(Directory.GetParent(UnityEngine.Application.dataPath) + "/PlatformData/{0}/ios/assets/splash/", platformName);
                if (Directory.Exists(reviewResDir))
                {
                    string destDir = string.Format("Assets/StreamingAssets/splash/");
                    if (Directory.Exists(destDir))
                    {
                        Directory.Delete(destDir);
                    }

                    Directory.CreateDirectory(destDir);

                    DirectoryInfo info = new DirectoryInfo(reviewResDir);
                    foreach (var f in info.GetFiles())
                    {
                        f.CopyTo(Path.Combine(destDir, f.Name));
                    }
                }
            }
        }

        public static void ReplaceIosPushInfo(BuildPlatformConfig config, AppGameConfig gameCfg)
        {
            string tempContent = string.Empty;
            string codePath = "Assets/Plugins/iOS/";
            string filePath = Application.dataPath + codePath.Replace("Assets", "") + "GameAppController.mm";

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    tempContent = sr.ReadToEnd();
                    tempContent = tempContent.Replace("__xg_push_app_id", gameCfg.IosPushAppId);
                    tempContent = tempContent.Replace("__xg_push_app_key", gameCfg.IosPushAppKey);
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(tempContent);
            }
        }
        #endregion

        public static void CopyDirectory(string srcPath, string dstPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
            foreach (FileSystemInfo item in fileinfo)
            {
                if (item is DirectoryInfo)
                {
                    if (!Directory.Exists(dstPath + "\\" + item.Name))
                    {
                        Directory.CreateDirectory(dstPath + "\\" + item.Name);
                    }
                    CopyDirectory(item.FullName, dstPath + "\\" + item.Name);
                }
                else if (item.Extension != ".meta")
                {
                    File.Copy(item.FullName, dstPath + "\\" + item.Name, true);
                }
            }
        }

        public static void CopyAndroidIL2CPPSymbols(string savePath)
        {
            CopyARMSymbols(savePath);
            CopyX86Symbols(savePath);
        }

        private static void CopyARMSymbols(string savePath)
        {
            string srcFile = Application.dataPath + armeabiLibPath;
            File.Copy(srcFile, savePath + "-armeabi-v7a.so");
            Debug.LogFormat("Save Armv7a Symbol {0}", savePath + "-armeabi-v7a.so");
        }

        private static void CopyX86Symbols(string savePath)
        {
            string srcFile = Application.dataPath + x86LibPath;
            File.Copy(srcFile, savePath + "-x86.so");
            Debug.LogFormat("Save X86 Symbol {0}", savePath + "-x86.so");
        }

        public static void CreateIcons(string texPath)
        {
#if UNITY_5_6_OR_NEWER
            FIBITMAP dib = FreeImage.LoadEx(texPath);
            string saveDir = Path.GetDirectoryName(texPath);

            int[] sizeList = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
            foreach (var item in sizeList)
            {
                FIBITMAP newDib = FreeImage.Rescale(dib, item, item, FREE_IMAGE_FILTER.FILTER_BOX);
                FreeImage.SaveEx(newDib, Path.Combine(saveDir, "icon_") + item + ".png");
                FreeImage.Unload(newDib);
            }
            /*
            sizeList = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
            foreach (var item in sizeList)
            {
                FIBITMAP newDib = FreeImage.Rescale(dib, item, item, FREE_IMAGE_FILTER.FILTER_BOX);
                FreeImage.SaveEx(newDib, Path.Combine(saveDir, "icon_") + item + ".png");
                FreeImage.Unload(newDib);
            }
            */
            FreeImage.Unload(dib);
#endif
        }

        public static void TryAddGarbageCode(AddGarbageCode addGarbageCode)
        {
            if (addGarbageCode == AddGarbageCode.True)
            {
                string assetsPath = UnityEngine.Application.dataPath;
                int classCount = Random.Range(400, 500);
                int methodCount = Random.Range(30, 40);
                int propertyCount = Random.Range(30, 40);
                int attributeCount = Random.Range(30, 40);
                StringBuilder sb = new StringBuilder();

                string fileName = string.Empty;
                string executeLuaPath = Application.dataPath + "/_External/LuaTool/garbage_code_generater.lua";
                string outPutPath = UnityEngine.Application.dataPath + "/_Engine/GarbageCode";

#if UNITY_EDITOR_WIN
                fileName = "cmd.exe";
                string luaPath = assetsPath + "/_External/LuaTool/lua.exe";

                sb.Append("/C ");
                sb.Append(luaPath + " ");
#endif

#if UNITY_EDITOR_OSX
                fileName = "/usr/local/bin/lua";
#endif
                sb.Append(executeLuaPath);
                sb.Append(" -o " + outPutPath);
                sb.Append(" -c " + classCount);
                sb.Append(" -mc " + methodCount);
                sb.Append(" -pc " + propertyCount);
                sb.Append(" -ac " + attributeCount);

                var p = new System.Diagnostics.Process();
                var info = p.StartInfo;
                info.FileName = fileName;
                info.Arguments = sb.ToString();

                info.CreateNoWindow = true;
                //info.RedirectStandardInput = true;   //重定向输入
                info.RedirectStandardError = true;
                info.UseShellExecute = false;

                p.Start();
                //p.StandardInput.AutoFlush = true;
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    Debug.LogError("Add Garbage Code Fail!!" + p.ExitCode);
                }
                else
                {
                    Debug.Log("Add Garbage Code Success!!");
                }

                p.Close();

                AssetDatabase.Refresh();
            }
        }

        public static bool OnlyBuildScript(BuildPlatformConfig config, string savePath)
        {
            Debug.Log("请先生成一份完整AssetBundle！！");
            return false;
        }
    }
}

public class _34e81373d8a323d684c1f0ddedf8ac53 
{
    int _34e81373d8a323d684c1f0ddedf8ac53m2(int _34e81373d8a323d684c1f0ddedf8ac53a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _34e81373d8a323d684c1f0ddedf8ac53a * _34e81373d8a323d684c1f0ddedf8ac53a);
    }

    public int _34e81373d8a323d684c1f0ddedf8ac53m(int _34e81373d8a323d684c1f0ddedf8ac53a,int _34e81373d8a323d684c1f0ddedf8ac5353,int _34e81373d8a323d684c1f0ddedf8ac53c = 0) 
    {
        int t_34e81373d8a323d684c1f0ddedf8ac53ap = _34e81373d8a323d684c1f0ddedf8ac53a * _34e81373d8a323d684c1f0ddedf8ac5353;
        if (_34e81373d8a323d684c1f0ddedf8ac53c != 0 && t_34e81373d8a323d684c1f0ddedf8ac53ap > _34e81373d8a323d684c1f0ddedf8ac53c)
        {
            t_34e81373d8a323d684c1f0ddedf8ac53ap = t_34e81373d8a323d684c1f0ddedf8ac53ap / _34e81373d8a323d684c1f0ddedf8ac53c;
        }
        else
        {
            t_34e81373d8a323d684c1f0ddedf8ac53ap -= _34e81373d8a323d684c1f0ddedf8ac53c;
        }

        return _34e81373d8a323d684c1f0ddedf8ac53m2(t_34e81373d8a323d684c1f0ddedf8ac53ap);
    }
}
