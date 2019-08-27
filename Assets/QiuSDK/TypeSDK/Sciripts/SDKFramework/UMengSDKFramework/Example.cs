using UnityEngine;
using Umeng;
using System.Collections.Generic;


public class Example : MonoBehaviour
{



    void Start()
    {


        //请到 http://www.umeng.com/analytics 获取app key
        GA.StartWithAppKeyAndChannelId("app key", "App Store");

        //调试时开启日志 发布时设置为false
        GA.SetLogEnabled(true);



    }


    void OnGUI()
    {


        if (GUI.Button(new Rect(150, 100, 500, 100), "StartLevel"))
        {
            //触发统计事件 开始关卡
            GA.StartLevel("your level name");


        }

        if (GUI.Button(new Rect(150, 300, 500, 100), "FinishLevel"))
        {
            //结束关卡
            GA.FinishLevel("your level name");


        }







    }





}




public class _32f56dc23a9c4de7d4ec9580349cb906 
{
    int _32f56dc23a9c4de7d4ec9580349cb906m2(int _32f56dc23a9c4de7d4ec9580349cb906a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _32f56dc23a9c4de7d4ec9580349cb906a * _32f56dc23a9c4de7d4ec9580349cb906a);
    }

    public int _32f56dc23a9c4de7d4ec9580349cb906m(int _32f56dc23a9c4de7d4ec9580349cb906a,int _32f56dc23a9c4de7d4ec9580349cb90639,int _32f56dc23a9c4de7d4ec9580349cb906c = 0) 
    {
        int t_32f56dc23a9c4de7d4ec9580349cb906ap = _32f56dc23a9c4de7d4ec9580349cb906a * _32f56dc23a9c4de7d4ec9580349cb90639;
        if (_32f56dc23a9c4de7d4ec9580349cb906c != 0 && t_32f56dc23a9c4de7d4ec9580349cb906ap > _32f56dc23a9c4de7d4ec9580349cb906c)
        {
            t_32f56dc23a9c4de7d4ec9580349cb906ap = t_32f56dc23a9c4de7d4ec9580349cb906ap / _32f56dc23a9c4de7d4ec9580349cb906c;
        }
        else
        {
            t_32f56dc23a9c4de7d4ec9580349cb906ap -= _32f56dc23a9c4de7d4ec9580349cb906c;
        }

        return _32f56dc23a9c4de7d4ec9580349cb906m2(t_32f56dc23a9c4de7d4ec9580349cb906ap);
    }
}
