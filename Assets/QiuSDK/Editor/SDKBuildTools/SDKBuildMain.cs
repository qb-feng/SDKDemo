using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using SDKData;
public class SDKBuildMain : MonoBehaviour
{
    #region bulid
    [MenuItem("qSDK/Build SDK/Android/OPPO")]
    static void Build_Android_OPPO()
    {
        StartBuildAndroidPackage(SDKPlatName.OPPO);
    }

    [MenuItem("qSDK/Build SDK/Android/VIVO")]
    static void Build_Android_VIVO()
    {
        StartBuildAndroidPackage(SDKPlatName.VIVO);
    }

    [MenuItem("qSDK/Build SDK/Android/VC")]
    static void Build_Android_VC()
    {
        StartBuildAndroidPackage(SDKPlatName.UC);
    }

    [MenuItem("qSDK/Build SDK/Android/HW")]
    static void Build_Android_HW()
    {
        StartBuildAndroidPackage(SDKPlatName.HW);
    }

    [MenuItem("qSDK/Build SDK/Android/YYB")]
    static void Build_Android_YYB()
    {
        StartBuildAndroidPackage(SDKPlatName.YYB);
    }
    #endregion

    #region Copy SDK
    [MenuItem("qSDK/Copy SDK/Android/OPPO")]
    static void Copy_Android_OPPO()
    {
        StartCopyAndroidSDKFile(SDKPlatName.OPPO);
    }

    [MenuItem("qSDK/Copy SDK/Android/VIVO")]
    static void Copy_Android_VIVO()
    {
        StartCopyAndroidSDKFile(SDKPlatName.VIVO);
    }

    [MenuItem("qSDK/Copy SDK/Android/VC")]
    static void Copy_Android_VC()
    {
        StartCopyAndroidSDKFile(SDKPlatName.UC);
    }

    [MenuItem("qSDK/Copy SDK/Android/HW")]
    static void Copy_Android_HW()
    {
        StartCopyAndroidSDKFile(SDKPlatName.HW);
    }

    [MenuItem("qSDK/Copy SDK/Android/YYB")]
    static void Copy_Android_YYB()
    {
        StartCopyAndroidSDKFile(SDKPlatName.YYB);
    }

    [MenuItem("qSDK/Copy SDK/Android/YaoLing")]
    static void Copy_Android_YaoLing()
    {
        StartCopyAndroidSDKFile(SDKPlatName.YaoLing);
    }

    [MenuItem("qSDK/Copy SDK/Android/TypeSDK")]
    static void Copy_Android_TypeSDK()
    {
        StartCopyAndroidSDKFile(SDKPlatName.TypeSDK);
    }

    #endregion


    #region 一些简单操作指令
    [MenuItem("qSDK/Clearing SDK Android")]
    static void Clearing_SDK_Android()
    {
        ClearingAndroidSDKPugins();
    }

    /// <summary>
    /// 执行一些typesdk导出包后的清理步骤
    /// </summary>
    [MenuItem("qSDK/Clearing TypeSDK AndroidPackage(用于删除typesdk打包后的不必要文件)")]
    static void Clearing_TypeSDK_AndroidPackage()
    {
        // OpenFileManager.OpenAPKFile(target_Dir);

        string target_dir = @"F:\qiubinFile\Bulid\BuildChannelApkTool\程序包\demo\2018年7月23日100719 周一包\GamePackage\Game";
        DirectoryInfo dir = new DirectoryInfo(target_dir);

        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
        foreach (FileSystemInfo i in fileinfo)
        {
            if (i is DirectoryInfo)            //判断是否文件夹
            {
                if (i.Name.Equals("assets"))
                {
                    DirectoryInfo assets = new DirectoryInfo(i.FullName);
                    var assetsDir = assets.GetFileSystemInfos();
                    foreach (var v in assetsDir)
                    {
                        if (v is FileInfo)
                        {
                            Debug.LogWarning(v.Name);
                            switch (v.Name)
                            {
                                case "UPPayPluginExpro.apk":
                                case "ADConfig.txt":
                                case "CPSettings.txt":
                                case "data.bin":
                                case "GAGameSettings.txt":
                                case "UPPayPluginEx.apk":
                                    File.Delete(v.FullName);
                                    Debug.LogWarning(v.FullName + "删除成功！");
                                    continue;
                            }
                        }
                    }
                }
                else if (i.Name.Equals("libs"))
                {
                    DirectoryInfo assets = new DirectoryInfo(i.FullName);
                    var assetsDir = assets.GetFileSystemInfos();
                    foreach (var v in assetsDir)
                    {
                        if (v is FileInfo)
                        {
                            switch (v.Name)
                            {
                                case "alipaySdk-20161222.jar":
                                case "gagamesdk_gn.jar":
                                case "mta-sdk-1.6.2.jar":
                                case "open_sdk_r5788.jar":
                                case "typesdk_gagamesdk.jar":

                                case "UPPayAssistEx.jar":
                                case "UPPayPluginExPro.jar":
                                case "wechat-sdk-android-with-mta-1.1.7.jar":
                                    File.Delete(v.FullName);
                                    Debug.LogWarning(v.FullName + "删除成功！");
                                    break;
                            }
                        }
                    }
                }
                //subdir.Delete(true);          //删除子目录和文件
            }
            else
            {
                //File.Delete(i.FullName);      //删除指定文件
                if (i.Name.Equals("AndroidManifest.xml"))
                {
                    File.Copy(@"F:\qiubinFile\Bulid\BuildChannelApkTool\程序包\demo\2018年7月23日100719 周一包\CopyFile\AndroidManifest.xml", i.FullName, true);
                    Debug.LogWarning(i.FullName + "文件拷贝成功！");
                }
                else if (i.Name.Equals("project.properties"))
                {
                    File.Copy(@"F:\qiubinFile\Bulid\BuildChannelApkTool\程序包\demo\2018年7月23日100719 周一包\CopyFile\project.properties", i.FullName, true);
                    Debug.LogWarning(i.FullName + "文件拷贝成功！");
                }
            }
        }
    }

    #endregion


    /// <summary>
    /// 设置参数
    /// </summary>
    public static void SetAndoridPackagePlayerSetting(SDKPlatName plantName)
    {
        string packageName = SDKPlatCommonData.PlatPackageData[plantName];//包名
        string version = "1.0";//版本号
        string productName = "SDKDemo";//安装包名
        string companyName = "SDKDemo";

        //设置
        PlayerSettings.productName = productName;
        PlayerSettings.companyName = companyName;
        PlayerSettings.bundleIdentifier = packageName;
        PlayerSettings.bundleVersion = version;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "DEMO;" + plantName.ToString());//预定义宏！
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.keystorePass = "yyty0611";
        PlayerSettings.keyaliasPass = "yyty0611";

        Debug.LogWarning("PlayerSetting 设置成功！");
        return;
    }

    /// <summary>
    /// 开始打安卓包 参数1：平台名字
    public static void StartBuildAndroidPackage(SDKPlatName plantName)
    {
        string target_dir = @"F:\qiubinFile\Bulid\BuildChannelApkTool\package";//包放置的文件夹
        string target_name = "demo.apk";//包的名字
        string[] build_Scene = FindEnabledEditorScenes();//打包场景
        BuildTarget buildTarget = BuildTarget.Android;//打包平台

        SetAndoridPackagePlayerSetting(plantName);

        //清空上一次的安卓包
        DirectoryInfo dir = new DirectoryInfo(target_dir);

        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
        foreach (FileSystemInfo i in fileinfo)
        {
            if (i is DirectoryInfo)            //判断是否文件夹
            {
                DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                subdir.Delete(true);          //删除子目录和文件
            }
            else
            {
                File.Delete(i.FullName);      //删除指定文件
            }
        }



        GenericBuild(build_Scene, target_dir + "/" + target_name, buildTarget, BuildOptions.None, target_dir);


    }

    /// <summary>
    /// 开始拷贝指定sdk文件
    /// </summary>
    public static void StartCopyAndroidSDKFile(SDKPlatName plantName)
    {
        ClearingAndroidSDKPugins();
        if (CopyAndroidPluginFile(plantName))
        {
            Debug.LogWarning("拷贝成功！");
        }
    }

    /// <summary>
    /// 打包
    /// </summary>
    static void GenericBuild(string[] scenes, string target_Url, BuildTarget build_target, BuildOptions build_options, string target_Dir)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_Url, build_target, build_options);

        if (res.Length > 0)
        {
            throw new System.Exception("BuildPlayer failure: " + res);
        }
        else
        {
            Debug.LogWarning("打包成功！目标路径：" + target_Url);
            OpenFileManager.OpenAPKFile(target_Dir);
        }
    }

    /// <summary>
    /// 获得可用scene
    /// </summary>
    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }


    /// <summary>
    /// 拷贝指定渠道的文件到plugins中的android文件夹
    /// </summary>
    private static bool CopyAndroidPluginFile(SDKPlatName platName)
    {
        string targetDic = (Application.dataPath + "/Plugins/Android").Replace('/', '\\');// 目标路径

        string sourceDicParent = Application.dataPath.Replace("/Assets", "/PingTaiPeiZhi/PlatSDK");
        string sourceDic = string.Format("{0}/{1}", sourceDicParent, platName.ToString()).Replace('/', '\\');

        DirectoryInfo targetDirFile = new DirectoryInfo(sourceDic);
        FileSystemInfo[] targetFile = targetDirFile.GetFileSystemInfos();  //返回目录中所有文件和子目录

        foreach (FileSystemInfo i in targetFile)
        {
            string targetName = targetDic + "\\" + i.Name;
            if (i is DirectoryInfo)
            {
                if (!Directory.Exists(targetName))
                {
                    Directory.CreateDirectory(targetName);   //目标目录下不存在此文件夹即创建子文件夹
                }
                CopyDirectory(i.FullName, targetName);
            }
            else
            {
                File.Copy(i.FullName, targetName);
            }
        }
        SetAndoridPackagePlayerSetting(platName);

        return true;
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    private static void CopyDirectory(string srcPath, string destPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)     //判断是否文件夹
                {
                    if (!Directory.Exists(destPath + "\\" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }
                    CopyDirectory(i.FullName, destPath + "\\" + i.Name);    //递归调用复制子文件夹
                }
                else
                {
                    File.Copy(i.FullName, destPath + "\\" + i.Name);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// 清理sdk插件
    /// </summary>
    private static void ClearingAndroidSDKPugins()
    {
        string targetDic = (Application.dataPath + "/Plugins/Android").Replace('/', '\\');// 目标路径
        //不能删除的文件夹
        List<string> saveDic = new List<string>() 
        {
            "Umeng",
        };

        //不能删除的文件
        List<string> saveFile = new List<string>()
        {

        };

        if (!Directory.Exists(targetDic))
        {
            Debug.LogError(targetDic + " 路径不存在！");
            return;
        }

        //先清理目标路径不需要的文件
        DirectoryInfo targetDirFile = new DirectoryInfo(targetDic);
        FileSystemInfo[] targetFile = targetDirFile.GetFileSystemInfos();  //返回目录中所有文件和子目录
        foreach (FileSystemInfo i in targetFile)
        {
            if (i is DirectoryInfo)
            {
                if (!saveDic.Contains(i.Name))            //判断是否文件夹并且是否能删除
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
            }
            else
            {
                if (!saveFile.Contains(i.Name))
                {
                    File.Delete(i.FullName);      //删除指定文件
                }
            }
        }

        return;
    }
}
