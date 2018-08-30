// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TypeSDKTool
{
    public delegate void TypeHttpCBKDelegate(string cbkText,UnityEngine.Object crossData);

    public class HttpModel:MonoBehaviour
	{
		private static volatile HttpModel _instance;
		private static object syncRoot = new object(); 
		private static  GameObject _container;

		private HttpModel():base()
		{
		}
		//get instance
		public static HttpModel Ins
		{
			get 
			{
				if(null == _instance)
				{
					_container = new GameObject();
					_container.name = "HttpModel";
					UnityEngine.Object.DontDestroyOnLoad(_container);
					lock(syncRoot)
					{
						if(null == _instance)
						{
							_instance = _container.AddComponent(typeof(HttpModel))as HttpModel;
						}
					}
				}
				return _instance; 
			}
		}

		//GET请求
		public IEnumerator HttpGet(string url, TypeHttpCBKDelegate cbkFunc, UnityEngine.Object crossData )
		{
			
			WWW getData = new WWW (url);
			yield return getData;
			
			if (getData.error != null)
			{
				//GET请求失败
				Debug.Log("error is :"+ getData.error);
                cbkFunc(getData.text, crossData);
				
			} else
			{
				//GET请求成功
				Debug.Log("request ok : " + getData.text);
                cbkFunc(getData.text, crossData);
            }
		}

		//GET请求


		public IEnumerator HttpPost(string url,Dictionary<string,string> postData,TypeHttpCBKDelegate cbkFunc, UnityEngine.Object crossData)
		{
			Debug.Log("create wwwform");
			WWWForm form = new WWWForm();
			
			foreach(string key in postData.Keys)
			{
				Debug.Log("read post data add "+postData[key]);
				form.AddField(key, postData[key]);
			}
			Debug.Log("create form finish");
			
			Debug.Log("start httppost :"+ url);
			WWW getData = new WWW(url, form);
			
			yield return getData;
			
			if (getData.error != null)
			{
				//GET请求失败
				Debug.Log("error is :" + getData.error);
				cbkFunc(getData.error, crossData);
			}
			else
			{
				//GET请求成功
				Debug.Log("request ok : " + getData.text);
				cbkFunc(getData.text, crossData);
			}
		}
	}
}

