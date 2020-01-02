package com.yyty.fsld.xzjh;

import android.os.Bundle;
import com.qk.game.MainActivity;

public class GameMainActivity extends MainActivity {
	private static GameMainActivity mainActivity = null;

	public static GameMainActivity GetInstance() {
		return mainActivity;
	}

	protected void onCreate(Bundle savedInstanceState) {
		mainActivity = this;
		super.onCreate(savedInstanceState);
	}
}
