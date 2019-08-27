using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Reflection;
using UnityEditor;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
/// ������λ������
/// </summary>
public enum InsertPosType
{
    /// <summary>
    ///��ͷ����
    /// </summary>
    Begin = 1,
    /// <summary>
    /// ĩβ����
    /// </summary>
    End = 2,
}

/// <summary>
/// ѹ���ķ�������
/// </summary>
public class EditorExcuteFuctionData
{
    /// <summary>
    /// ��������
    /// </summary>
    public string funcName;
    /// <summary>
    /// ��������
    /// </summary>
    public string classType;
}

/// <summary>
/// �༭������ִ�з�������
/// </summary>
[System.Serializable]
public class EditorQueueData
{
    /// <summary>
    /// ����ִ�е����з�������
    /// </summary>
    public List<string> funcNameList;

    /// <summary>
    /// ��������
    /// </summary>
    public string classType;
}

[InitializeOnLoad]
public class EditorMonoBehaviour
{
    /// <summary>
    /// �Ƿ����Կ�ʼִ��
    /// </summary>
    public static bool isCanExecute
    {
        get
        {
            return File.Exists(isCanRunEditorTask);
        }
        set
        {
            if (value)
            {
                if (!isCanExecute)
                {
                    File.Create(isCanRunEditorTask);
                }
            }
            else
            {
                if (isCanExecute)
                {
                    File.Delete(isCanRunEditorTask);
                }
            }
        }
    }

    static EditorMonoBehaviour()
    {
        EditorApplication.update += Update;
    }
    private static void Update()
    {
        if (!EditorApplication.isCompiling)
        {
           //ebug.LogWarning("Script Compilation complete!");
            if (isCanExecute)
            {
                var functionData = DequeueFunction();
                if (functionData != null)
                {
                    Type createType = Type.GetType(functionData.classType);
                    var function = Activator.CreateInstance(createType);
                    var method = function.GetType().GetMethod(functionData.funcName);
                    if (method == null)
                        method = function.GetType().GetMethod(functionData.funcName, BindingFlags.NonPublic | BindingFlags.Instance);

                    object result = method.Invoke(function, null);
                    AssetDatabase.Refresh();
                    Debug.LogWarning(functionData.classType + "->" + functionData.funcName + "    : function run ok!");
                }
            }
        }
        else
        {
            //Debug.LogWarning("Script Compile!");
        }
    }

    #region ����
    private static string taskFull = @"./EditorQueueTask.txt";
    private static string isCanRunEditorTask = @"./isCanRunEditorTask";
    public static void Init()
    {
        if (File.Exists(taskFull))
        {
            File.Delete(taskFull);
        }
    }
    private static string ReadFileStream(FileStream fs)
    {
        if (fs.Length == 0)
            return null;

        byte[] datas = new byte[fs.Length];
        fs.Read(datas, 0, datas.Length);
        string datastring = System.Text.Encoding.UTF8.GetString(datas);
        return datastring;
    }
    private static void WriteFileStream(FileStream fs, string content)
    {
        fs.Seek(0, SeekOrigin.Begin);
        fs.SetLength(0);
        byte[] datas = System.Text.Encoding.UTF8.GetBytes(content);
        fs.Write(datas, 0, datas.Length);
    }

    /// <summary>
    /// ����һ������  Ĭ����ĩβ��
    /// </summary>
    /// <param name="function"></param>
    public static void InsertFunction(EditorQueueData functionData, InsertPosType insertPosType = InsertPosType.End)
    {
        if (!File.Exists(taskFull))
        {
            var fc = File.Create(taskFull);
            fc.Close();
            fc.Dispose();
        }
        using (FileStream fs = new FileStream(taskFull, FileMode.Open, FileAccess.ReadWrite))
        {
            string allString = ReadFileStream(fs);
            List<EditorQueueData> tempData = null;
            if (!string.IsNullOrEmpty(allString))
            {
                tempData = LitJson.JsonMapper.ToObject<List<EditorQueueData>>(allString);
            }
            if (tempData == null)
            {
                tempData = new List<EditorQueueData>();
            }
            int insertIndex = 0;
            if (insertPosType == InsertPosType.End)
            {
                insertIndex = tempData.Count;
            }
            tempData.Insert(insertIndex, functionData);

            allString = LitJson.JsonMapper.ToJson(tempData);
            WriteFileStream(fs, allString);

            fs.Close();
        }
    }
    /// <summary>
    /// ѹ��һ������
    /// </summary>
    public static EditorExcuteFuctionData DequeueFunction()
    {
        if (!File.Exists(taskFull))
            return null;
        EditorExcuteFuctionData result = null;
        using (FileStream fs = new FileStream(taskFull, FileMode.Open, FileAccess.ReadWrite))
        {
            string allString = ReadFileStream(fs);
            List<EditorQueueData> tempData = null;
            if (string.IsNullOrEmpty(allString))
            {
                return null;
            }
            tempData = LitJson.JsonMapper.ToObject<List<EditorQueueData>>(allString);
            if (tempData != null && tempData.Count > 0)
            {
                result = new EditorExcuteFuctionData();
                result.funcName = tempData[0].funcNameList[0];
                result.classType = tempData[0].classType;
                tempData[0].funcNameList.RemoveAt(0);
                if (tempData[0].funcNameList.Count == 0)//������ִ�������ˣ�ֱ��ɾ����������
                {
                    tempData.RemoveAt(0);
                }
                string data = "";
                if (tempData.Count > 0)
                    data = LitJson.JsonMapper.ToJson(tempData);
                WriteFileStream(fs, data);
            }
            fs.Close();
            return result;
        }
    }
    #endregion
}

public class _c087c6257740c434332e3910780ba9a8 
{
    int _c087c6257740c434332e3910780ba9a8m2(int _c087c6257740c434332e3910780ba9a8a)
    {
        return (int)(3.1415926535897932384626433832795028841 * _c087c6257740c434332e3910780ba9a8a * _c087c6257740c434332e3910780ba9a8a);
    }

    public int _c087c6257740c434332e3910780ba9a8m(int _c087c6257740c434332e3910780ba9a8a,int _c087c6257740c434332e3910780ba9a847,int _c087c6257740c434332e3910780ba9a8c = 0) 
    {
        int t_c087c6257740c434332e3910780ba9a8ap = _c087c6257740c434332e3910780ba9a8a * _c087c6257740c434332e3910780ba9a847;
        if (_c087c6257740c434332e3910780ba9a8c != 0 && t_c087c6257740c434332e3910780ba9a8ap > _c087c6257740c434332e3910780ba9a8c)
        {
            t_c087c6257740c434332e3910780ba9a8ap = t_c087c6257740c434332e3910780ba9a8ap / _c087c6257740c434332e3910780ba9a8c;
        }
        else
        {
            t_c087c6257740c434332e3910780ba9a8ap -= _c087c6257740c434332e3910780ba9a8c;
        }

        return _c087c6257740c434332e3910780ba9a8m2(t_c087c6257740c434332e3910780ba9a8ap);
    }
}
