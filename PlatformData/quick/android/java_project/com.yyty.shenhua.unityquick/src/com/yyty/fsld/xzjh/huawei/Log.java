package com.yyty.fsld.xzjh.huawei;

//quicksdk ToolBar
public class Log {
	
	public static void v(String tag, String msg) {
		v(tag,msg,null);
	}

	
	public static void e(String tag, String msg) {
		e(tag,msg,null);
	}

	public static void e(String tag, String msg,Throwable tr) {
		if (MainActivity.DebugEnable) {
			android.util.Log.e(tag, msg, tr);
		}
	}
	public static void v(String tag, String msg, Throwable tr) {
		if (MainActivity.DebugEnable) {
			android.util.Log.v(tag, msg, tr);
		}
	}

}
