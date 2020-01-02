package com.yyty.fsld.xzjh;

public class Log {
	public static void v(String tag, String msg) {
		v(tag, msg, null);
	}

	public static void v(String tag, String msg, Throwable tr) {
		if (MainActivity.DebugEnable.booleanValue())
			android.util.Log.v(tag, msg, tr);
	}
}
