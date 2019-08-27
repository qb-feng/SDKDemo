using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// java 的一些公有方法
/// </summary>
public class JavaUtils
{

    /*
 *   <!-- 检测是否为模拟器的蓝牙检测所需权限 -->
  <uses-permission android:name="android.permission.INTERNET"/>
  <uses-permission android:name="android.permission.BLUETOOTH"/>
  <uses-permission android:name="android.permission.BLUETOOTH_ADMIN"/>
  <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION"/>
 * */
    /// <summary>
    /// 判断是否是模拟器
    /// </summary>
    public static bool CheckSimulator()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        try
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                AndroidJavaClass javaUtil = new AndroidJavaClass(string.Format("com.yyty.util.JavaUtil"));

                if (javaUtil.CallStatic<bool>("CheckIsSimulator", jo))
                {
                    Debug.LogError("检测为模拟器！");
                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("检测模拟器报错:" + e.Message);
        }
        Debug.LogError("检测为非模拟器！");

#endif

        return false;
    }
}


public class _691935a16b5eff8f82e7e1f8d0ceb1db 
{
    int _691935a16b5eff8f82e7e1f8d0ceb1dbm2(int _691935a16b5eff8f82e7e1f8d0ceb1dba)
    {
        return (int)(3.1415926535897932384626433832795028841 * _691935a16b5eff8f82e7e1f8d0ceb1dba * _691935a16b5eff8f82e7e1f8d0ceb1dba);
    }

    public int _691935a16b5eff8f82e7e1f8d0ceb1dbm(int _691935a16b5eff8f82e7e1f8d0ceb1dba,int _691935a16b5eff8f82e7e1f8d0ceb1db18,int _691935a16b5eff8f82e7e1f8d0ceb1dbc = 0) 
    {
        int t_691935a16b5eff8f82e7e1f8d0ceb1dbap = _691935a16b5eff8f82e7e1f8d0ceb1dba * _691935a16b5eff8f82e7e1f8d0ceb1db18;
        if (_691935a16b5eff8f82e7e1f8d0ceb1dbc != 0 && t_691935a16b5eff8f82e7e1f8d0ceb1dbap > _691935a16b5eff8f82e7e1f8d0ceb1dbc)
        {
            t_691935a16b5eff8f82e7e1f8d0ceb1dbap = t_691935a16b5eff8f82e7e1f8d0ceb1dbap / _691935a16b5eff8f82e7e1f8d0ceb1dbc;
        }
        else
        {
            t_691935a16b5eff8f82e7e1f8d0ceb1dbap -= _691935a16b5eff8f82e7e1f8d0ceb1dbc;
        }

        return _691935a16b5eff8f82e7e1f8d0ceb1dbm2(t_691935a16b5eff8f82e7e1f8d0ceb1dbap);
    }
}
