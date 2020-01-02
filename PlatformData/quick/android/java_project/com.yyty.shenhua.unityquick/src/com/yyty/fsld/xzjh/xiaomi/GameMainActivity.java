package com.yyty.fsld.xzjh.xiaomi;

import com.quicksdk.utility.AppConfig;

import android.os.Bundle;

public class GameMainActivity extends QuickUnityPlayerproxyActivity {

	private static GameMainActivity mainActivity = null;

	public static GameMainActivity GetInstance() {
		// TODO Auto-generated constructor stub
		return mainActivity;
	}

	protected void onCreate(Bundle bundle) {
		mainActivity = this;
		super.onCreate(bundle);
		

	}

	public String getProductCode() {
		return AppConfig.getInstance().getConfigValue("product_code");
	}

	public String getProductKey() {
		return AppConfig.getInstance().getConfigValue("product_key");
	}

	@Override
	public void onWindowFocusChanged(boolean paramBoolean) {
		Log.v(MainActivity.sdktag, "GameMainActivity onWindowFocusChanged:" + paramBoolean, null);
		super.onWindowFocusChanged(paramBoolean);
	}
}
