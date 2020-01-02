package com.yyty.fsld.xzjh.huawei;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import org.json.JSONObject;


import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.widget.LinearLayout;
import android.widget.Toast;

public class MainActivity extends com.qk.game.SplashActivity {

	// quick sdk+++++++++++++++++++++++++++++++++++++++++++++++++++
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		try {
			InputStream is = getAssets().open("shgame_config.json");
			int size = is.available();
			byte[] buffer = new byte[size];
			is.read(buffer);
			is.close();
			String jsonStr = new String(buffer, "UTF-8");
			JSONObject jsonObject = new JSONObject(jsonStr);
			DebugEnable = jsonObject.getBoolean("Debug");// log

		} catch (Exception e) {
			e.printStackTrace();
		}
		
		super.onCreate(savedInstanceState);	
		Log.v(sdktag, "MainActivity onCreate", null);
	}

	@Override
	public void onSplashStop() {
		Log.v(sdktag, "MainActivity onSplashStop", null);
		onSDKSplahStop();
	}

	// quick sdk+++++++++++++++++++++++++++++++++++++++++++++++++++

	public static String sdktag = "QSDK";
	public static Boolean DebugEnable = false;
	public static String gameName;

	// 必备权限
	String[] mustPermissions = new String[] { Manifest.permission.WRITE_EXTERNAL_STORAGE,
			/* Manifest.permission.READ_SMS */ };

	// 动�?�申请的权限
	String[] requestPermissions = new String[] { Manifest.permission.READ_SMS, Manifest.permission.SEND_SMS,
			Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.ACCESS_FINE_LOCATION,
			Manifest.permission.READ_PHONE_STATE, Manifest.permission.CAMERA };

	// 读取本地配置�????
	private String[] ReadFileInfo(String fileName) {

		try {
			InputStream is = getAssets().open(fileName);
			InputStreamReader inputReader = new InputStreamReader(is);
			BufferedReader bufReader = new BufferedReader(inputReader);

			List<String> arrayList = new ArrayList<String>();

			String line;
			while ((line = bufReader.readLine()) != null) {

				arrayList.add(line);
			}

			String[] args = new String[arrayList.size()];
			arrayList.toArray(args);

			bufReader.close();
			inputReader.close();
			is.close();

			return args;

		} catch (Exception e) {
			Log.v(sdktag, "ReadFileInfo Exception:" + e.getMessage(), null);
			e.printStackTrace();
		}
		return null;
	}

	private void onSDKSplahStop() {

		Log.v(sdktag, "MainActivity onSDKSplahStop", null);

		mustPermissions = ReadFileInfo("shgame_mustpermission.info");// 必备的权限列�????
		requestPermissions = ReadFileInfo("shgame_permission.info");// 申请的权限列�????

		if (android.os.Build.VERSION.SDK_INT >= 23) {
			requestPower();
		} else {
			onInitStop();
		}
	}

	public boolean checkpermission() {

		// return true;

		if (android.os.Build.VERSION.SDK_INT < 23) {
			return true;
		}
		for (int i = 0; i < mustPermissions.length; i++) {
			if (ContextCompat.checkSelfPermission(this, mustPermissions[i]) != PackageManager.PERMISSION_GRANTED) {
				Toast.makeText(this, "" + "权限" + requestPermissions[i] + "没有启动，请开启权限再启动游戏", Toast.LENGTH_SHORT).show();
				return true;
			}

		}

		return true;
	}

	public void onInitStop() {
		
		Log.v(sdktag, "MainActivity onSDKSplahStop", null);
		
		if (checkpermission()) {
			Log.v(sdktag, "MainActivity onInitStop", null);
			// 闪屏播放结束，跳转游戏主Activity，并结束闪屏Activity
			Intent intent = new Intent(this, GameMainActivity.class);
			startActivity(intent);
			this.finish();
		}
	}

	public void requestPower() {
		Log.v(sdktag, "MainActivity 获取权限", null);
		int PERMISSION_GRANTED = PackageManager.PERMISSION_GRANTED;
		// 判断是否已经赋予权限
		if (ContextCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE) != PERMISSION_GRANTED) {
			{
				// 申请权限，字符串数组内是�?????个或多个要申请的权限�?????1是申请权限结果的返回参数，在onRequestPermissionsResult可以得知申请结果
				// Log.v(sdktag, "MainActivity 尝试读取外部�???", null);
				// // 读取文件的操�????
				// try {
				// FileInputStream fis = openFileInput("versionx");
				// fis.close();
				// fis = null;
				//
				// } catch (Exception e) {
				// e.printStackTrace();
				// }

				Log.v(sdktag, "MainActivity start requestPermissions", null);
				ActivityCompat.requestPermissions(this, requestPermissions, 1);
			}
		} else {
			Log.v(sdktag, "MainActivity 获取权限时玩家已经赋予权限，跳过权限赋予", null);
			onInitStop();
		}
	}

	int successCount = 0;

	@Override
	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {

		super.onRequestPermissionsResult(requestCode, permissions, grantResults);

		Log.v(sdktag, "MainActivity 权限获取结果" + requestCode, null);
		if (requestCode == 1) {
			for (int i = 0; i < permissions.length; i++) {
				Log.v(sdktag, "MainActivity 权限申请结果" + permissions[i] + "   " + grantResults[i], null);

				if (grantResults[i] == PackageManager.PERMISSION_GRANTED) {

					// 统计是否有必要权限，有就可以进入游戏
					String persion = permissions[i];

					for (int j = 0; i < mustPermissions.length; j++) {
						if (persion.equals(mustPermissions[j])) {

							successCount = successCount + 1;

							if (successCount >= mustPermissions.length) {

								Log.v(sdktag, "MainActivity 必备权限已获取，进入游戏" + requestCode, null);
								onInitStop();
							}
							break;
						}
					}

					// Toast.makeText(this, "" + "权限" + permissions[i] + "申请成功",
					// Toast.LENGTH_SHORT).show();

				} else {
					// Toast.makeText(this, "" + "权限" + permissions[i] +
					// "申请失败,可能会导致游戏运行异常，请手动打�?????",
					// Toast.LENGTH_SHORT).show();
				}
			}
		}
	}

}
