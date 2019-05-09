using UnityEngine;
using UnityEditor;
using GameEditor.AssetBuidler;
using System;
using System.Drawing.Printing;

public class AssetBuilderCommand
{
    public static void BuildAssetBundles()
    {
        Debug.Log("Start Build AssetBundle......");

        string[] argArr = System.Environment.GetCommandLineArgs();
        if (argArr.Length != 11)
        {
            Debug.LogErrorFormat("AssetBundle Build Argument Error! {0}", argArr.Length);
            return;
        }

        if (argArr[9] == "android")
        {
            BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Android);
            string savePath = string.Format("../../../web/{0}/res/android/", argArr[10]);
            AssetBuilderCtrl.BuildAssetBundles(config, savePath);
        }
        else if (argArr[9] == "ios")
        {
            BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.IOS);
            string savePath = string.Format("../../../web/{0}/res/ios/", argArr[10]);
            AssetBuilderCtrl.BuildAssetBundles(config, savePath);
        }
        else if (argArr[9] == "win")
        {
            BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Win);
            string savePath = string.Format("../../../web/{0}/res/win/", argArr[10]);
            AssetBuilderCtrl.BuildAssetBundles(config, savePath);
        }

        Debug.Log("Finish Build AssetBundle!");
    }

    public static void BuildApp()
    {
        Debug.Log("Start Build App......");
        string[] argArr = System.Environment.GetCommandLineArgs();
        if (argArr.Length != 12)
        {
            Debug.LogErrorFormat("App Build Argument Error! {0}", argArr.Length);
            return;
        }

        if (argArr[9] == "android")
        {
            BuildPlatformConfig config = AssetBuilderConfig.GetConfig(BuildPlatform.Android);
            string bundlePath = string.Format("../../../web/{0}/res/android/", argArr[10]);
            string appSavePath = string.Format("../../../web/{0}/app/android/", argArr[10]);
            AssetBuilderCtrl.BuildAndroidApp(config, bundlePath, appSavePath, argArr[10], argArr[11]);
        }

        Debug.Log("Finish Build App!");
    }

}