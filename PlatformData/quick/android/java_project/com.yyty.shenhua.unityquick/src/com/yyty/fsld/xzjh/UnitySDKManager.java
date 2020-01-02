package com.yyty.fsld.xzjh;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;

public class UnitySDKManager {
	public static GameMainActivity GetGameMainActivity() {
		return GameMainActivity.GetInstance();
	}

	public static void ShowExitGameView() {
		final GameMainActivity gameMainActivity = GetGameMainActivity();

		gameMainActivity.runOnUiThread(new Runnable() {

			public void run() {
				AlertDialog.Builder builder = new AlertDialog.Builder((Context) gameMainActivity);
				builder.setTitle("确定退出游戏?");
				builder.setNegativeButton("确定", new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface arg0, int arg1) {
						System.exit(0);
					}
				});
				builder.setPositiveButton("取消", new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface dialog, int which) {
					}
				});

				builder.create();
				builder.show();
			}
		});
	}

	public static void StartSDKInit() {

	}
}
