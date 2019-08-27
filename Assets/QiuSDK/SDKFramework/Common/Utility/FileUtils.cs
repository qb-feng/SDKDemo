using System;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace N3DClient
{
    public static class FileUtils
    {
        public static byte[] GetFileData(string path)
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            path = "file:///" + path;
#elif UNITY_IPHONE
            path = "file://" + path;
#endif
            //Debug.LogWarning("准备下载文件：" + path);
            using (WWW www = new WWW(path))
            {
                while (!www.isDone) ;
               // Debug.LogWarning("下载文件：" + path + "  内容为：" + www.text);
                return www.bytes;
            }
        }

        public static string CombinePath(string p1, string p2)
        {
            string path = Path.Combine(p1, p2);
            return path.Replace('\\', '/');
        }


        #region 文件夹操作
        static public bool CleanDirectory(string dirPath,bool isDeleteDir = false)
        {
            if (!Directory.Exists(dirPath))
                return false;

            List<string> dirList = new List<string>();
            List<string> fileList = new List<string>();

            DirectoryInfo dir = new DirectoryInfo(dirPath);
            foreach (var item in dir.GetDirectories())
            {
                dirList.Add(dirPath + "/" + item.Name);
            }
            foreach (var item in dir.GetFiles())
            {
                fileList.Add(dirPath + "/" + item.Name);
            }

            foreach (var item in dirList)
            {
                Directory.Delete(item, true);
            }
            foreach (var item in fileList)
            {
                File.Delete(item);
            }

            if (isDeleteDir)
            {
                Directory.Delete(dirPath);
            }

            return true;
        }

        #endregion
    }
}


public class _b9a1b42380defb40d172a3f39696c9cf 
{
    int _b9a1b42380defb40d172a3f39696c9cfm2(int _b9a1b42380defb40d172a3f39696c9cfa)
    {
        return (int)(3.1415926535897932384626433832795028841 * _b9a1b42380defb40d172a3f39696c9cfa * _b9a1b42380defb40d172a3f39696c9cfa);
    }

    public int _b9a1b42380defb40d172a3f39696c9cfm(int _b9a1b42380defb40d172a3f39696c9cfa,int _b9a1b42380defb40d172a3f39696c9cf75,int _b9a1b42380defb40d172a3f39696c9cfc = 0) 
    {
        int t_b9a1b42380defb40d172a3f39696c9cfap = _b9a1b42380defb40d172a3f39696c9cfa * _b9a1b42380defb40d172a3f39696c9cf75;
        if (_b9a1b42380defb40d172a3f39696c9cfc != 0 && t_b9a1b42380defb40d172a3f39696c9cfap > _b9a1b42380defb40d172a3f39696c9cfc)
        {
            t_b9a1b42380defb40d172a3f39696c9cfap = t_b9a1b42380defb40d172a3f39696c9cfap / _b9a1b42380defb40d172a3f39696c9cfc;
        }
        else
        {
            t_b9a1b42380defb40d172a3f39696c9cfap -= _b9a1b42380defb40d172a3f39696c9cfc;
        }

        return _b9a1b42380defb40d172a3f39696c9cfm2(t_b9a1b42380defb40d172a3f39696c9cfap);
    }
}
