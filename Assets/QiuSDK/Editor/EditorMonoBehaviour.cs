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
/// 插入的位置类型
/// </summary>
public enum InsertPosType
{
    /// <summary>
    ///开头插入
    /// </summary>
    Begin = 1,
    /// <summary>
    /// 末尾插入
    /// </summary>
    End = 2,
}

/// <summary>
/// 压出的方法数据
/// </summary>
public class EditorExcuteFuctionData
{
    /// <summary>
    /// 方法名字
    /// </summary>
    public string funcName;
    /// <summary>
    /// 类的名字
    /// </summary>
    public string classType;
}

/// <summary>
/// 编辑器编译执行方法数据
/// </summary>
[System.Serializable]
public class EditorQueueData
{
    /// <summary>
    /// 依次执行的所有方法名字
    /// </summary>
    public List<string> funcNameList;

    /// <summary>
    /// 类的名字
    /// </summary>
    public string classType;
}

[InitializeOnLoad]
public class EditorMonoBehaviour
{
    /// <summary>
    /// 是否可以开始执行
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
            Debug.LogWarning("Script Compile!");
        }
    }

    #region 数据
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
    /// 插入一个数据  默认往末尾插
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
    /// 压出一个数据
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
                if (tempData[0].funcNameList.Count == 0)//方法都执行完毕了，直接删除此类数据
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