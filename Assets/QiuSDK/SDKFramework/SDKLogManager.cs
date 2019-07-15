using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDKLogManager : MonoBehaviour {

    public void Awake() 
    {
        if (N3DClient.GameConfig.LogEnable)
        {
            gameObject.AddComponent<Consolation.TestConsole>();
        }
    }

    #region log 集中处理
    public static void DebugLog(string message, DebugType type = DebugType.Log)
    {
        if (N3DClient.GameConfig.LogEnable)
        {
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
