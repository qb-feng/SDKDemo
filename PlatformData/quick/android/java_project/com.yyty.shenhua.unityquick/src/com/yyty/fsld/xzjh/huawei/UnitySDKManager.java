package com.yyty.fsld.xzjh.huawei;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;

public class UnitySDKManager {

	public static GameMainActivity GetGameMainActivity() {
		// TODO Auto-generated constructor stub
		return GameMainActivity.GetInstance();
	}

	public static void ShowExitGameView() {

		final Activity activity = GetGameMainActivity();

		activity.runOnUiThread(new Runnable() {
			public void run() {
				// TODO Auto-generated constructor stub
				// 渠道不存在�??出界面，如百度移动游戏等，此时需在此处弹出游戏�??出确认界面，否则会出现渠道审核不通过情况
				// �??出确认窗口需要游戏自定义，并且实现资源回收，�??死进程等�??出�?�辑
				AlertDialog.Builder builder = new AlertDialog.Builder((Context) activity);
				builder.setTitle("确定退出游戏?");
				builder.setNegativeButton("确定", new DialogInterface.OnClickListener() {

					@Override
					public void onClick(DialogInterface arg0, int arg1) {
						System.exit(0);
					}
				});
				builder.setPositiveButton("取消", new DialogInterface.OnClickListener() {

					@Override
					public void onClick(DialogInterface dialog, int which) {

					}
				});
				builder.create();
				builder.show();
			}
		});

	}

	public static void StartSDKInit() {
		Activity activity = GetGameMainActivity();
		activity.runOnUiThread(new Runnable() {
			public void run() {
				GetGameMainActivity().OnSDKCrete();
			}
		});
	}
}
