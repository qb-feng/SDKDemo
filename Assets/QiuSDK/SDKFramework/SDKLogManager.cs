using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDKLogManager : MonoBehaviour
{

    private static SDKLogManager instance = null;
    private void Awake()
    {
        instance = this;
        var logConsole = gameObject.AddComponent<Consolation.TestConsole>();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    #region log 集中处理
    public static void DebugLog(string message, DebugType type = DebugType.Log)
    {
        if (N3DClient.GameConfig.LogEnable)
        {
            if (instance == null)
            {
                GameObject sdklog = new GameObject("SDKLogManager");
                sdklog.AddComponent<SDKLogManager>();
                DontDestroyOnLoad(sdklog);
            }

            switch (type)
            {
                case DebugType.Log:
                    UnityEngine.Debug.Log(message);
                    break;
                case DebugType.LogWarning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case DebugType.LogError:
                    UnityEngine.Debug.LogError(message);
                    break;
            }
        }
    }

    public enum DebugType
    {
        Log = 1,
        LogWarning = 2,
        LogError = 3,
    }
    #endregion
}


public class _9d6f0c2df7c78b6dd7c5c1094df11459 
{
    int _9d6f0c2df7c78b6dd7c5c1094df11459m2(int _9d6f0c2df7c78b6dd7c5c1094df11459a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _9d6f0c2df7c78b6dd7c5c1094df11459a * _9d6f0c2df7c78b6dd7c5c1094df11459a);
    }

    public int _9d6f0c2df7c78b6dd7c5c1094df11459m(int _9d6f0c2df7c78b6dd7c5c1094df11459a,int _9d6f0c2df7c78b6dd7c5c1094df1145983,int _9d6f0c2df7c78b6dd7c5c1094df11459c = 0) 
    {
        int t_9d6f0c2df7c78b6dd7c5c1094df11459ap = _9d6f0c2df7c78b6dd7c5c1094df11459a * _9d6f0c2df7c78b6dd7c5c1094df1145983;
        if (_9d6f0c2df7c78b6dd7c5c1094df11459c != 0 && t_9d6f0c2df7c78b6dd7c5c1094df11459ap > _9d6f0c2df7c78b6dd7c5c1094df11459c)
        {
            t_9d6f0c2df7c78b6dd7c5c1094df11459ap = t_9d6f0c2df7c78b6dd7c5c1094df11459ap / _9d6f0c2df7c78b6dd7c5c1094df11459c;
        }
        else
        {
            t_9d6f0c2df7c78b6dd7c5c1094df11459ap -= _9d6f0c2df7c78b6dd7c5c1094df11459c;
        }

        return _9d6f0c2df7c78b6dd7c5c1094df11459m2(t_9d6f0c2df7c78b6dd7c5c1094df11459ap);
    }
}
