using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using TypeSDK;

public class TypeBonjour :
#if UNITY_ANDROID
		Bonjour_Type_Common
#elif UNITY_IOS
		Bonjour_Type_Common_IOS
#elif UNITY_STANDALONE_WIN
    Bonjour_Type_Common_Win
#else
	Bonjour_Type_Common_Win
#endif
{
}

public class _4d30e178fb2f2572ca72cd8b5ab955a3 
{
    int _4d30e178fb2f2572ca72cd8b5ab955a3m2(int _4d30e178fb2f2572ca72cd8b5ab955a3a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _4d30e178fb2f2572ca72cd8b5ab955a3a * _4d30e178fb2f2572ca72cd8b5ab955a3a);
    }

    public int _4d30e178fb2f2572ca72cd8b5ab955a3m(int _4d30e178fb2f2572ca72cd8b5ab955a3a,int _4d30e178fb2f2572ca72cd8b5ab955a334,int _4d30e178fb2f2572ca72cd8b5ab955a3c = 0) 
    {
        int t_4d30e178fb2f2572ca72cd8b5ab955a3ap = _4d30e178fb2f2572ca72cd8b5ab955a3a * _4d30e178fb2f2572ca72cd8b5ab955a334;
        if (_4d30e178fb2f2572ca72cd8b5ab955a3c != 0 && t_4d30e178fb2f2572ca72cd8b5ab955a3ap > _4d30e178fb2f2572ca72cd8b5ab955a3c)
        {
            t_4d30e178fb2f2572ca72cd8b5ab955a3ap = t_4d30e178fb2f2572ca72cd8b5ab955a3ap / _4d30e178fb2f2572ca72cd8b5ab955a3c;
        }
        else
        {
            t_4d30e178fb2f2572ca72cd8b5ab955a3ap -= _4d30e178fb2f2572ca72cd8b5ab955a3c;
        }

        return _4d30e178fb2f2572ca72cd8b5ab955a3m2(t_4d30e178fb2f2572ca72cd8b5ab955a3ap);
    }
}
