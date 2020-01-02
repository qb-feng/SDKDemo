package com.yyty.fsld.xzjh;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.content.Intent;
import android.content.res.Configuration;
import android.content.res.Resources;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.os.Handler.Callback;
import android.os.Message;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.text.TextUtils;
import android.util.Log;
import android.widget.Toast;
import com.quicksdk.BaseCallBack;
import com.quicksdk.Extend;
import com.quicksdk.Payment;
import com.quicksdk.QuickSDK;
import com.quicksdk.Sdk;
import com.quicksdk.User;
import com.quicksdk.entity.GameRoleInfo;
import com.quicksdk.entity.OrderInfo;
import com.quicksdk.entity.ShareInfo;
import com.quicksdk.entity.UserInfo;
import com.quicksdk.notifier.ExitNotifier;
import com.quicksdk.notifier.InitNotifier;
import com.quicksdk.notifier.LoginNotifier;
import com.quicksdk.notifier.LogoutNotifier;
import com.quicksdk.notifier.PayNotifier;
import com.quicksdk.notifier.SwitchAccountNotifier;
import com.quicksdk.utility.AppConfig;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import java.util.ArrayList;
import java.util.HashMap;
import org.json.JSONObject;

public abstract class QuickUnityPlayerproxyActivity extends UnityPlayerActivity implements Handler.Callback {
	private static final String TAG = "unity.support";
	private static final int MSG_LOGIN = 101;
	private static final int MSG_LOGOUT = 102;
	private static final int MSG_PAY = 103;
	private static final int MSG_EXIT = 104;
	private static final int MSG_ROLEINFO = 105;
	private static final int MSG_EXTEND_FUNC = 106;
	private static final int MSG_EXTEND_CALLPLUGIN = 107;
	private static final int MSG_EXTEND_FUNC_SHARE = 108;
	private static final int MSG_EXTEND_FUNC_GOODSINFO = 111;
	private final int REQUEST_RECORD_PERMISSION_SETTING = 999;
	private static final int INIT_SUCCESS = 1;
	private static final int INIT_FAILED = -1;
	private static final int INIT_DEFAULT = 0;
	private QuickUnityInitNotify initNotify = new QuickUnityInitNotify();
	private QuickUnityLoginNotify loginNotify = new QuickUnityLoginNotify();
	private QuickUnitySwitchAccountNotify switchAccountNotify = new QuickUnitySwitchAccountNotify();
	private QuickUnityLogoutNotify logoutNotify = new QuickUnityLogoutNotify();
	private QuickUnityPayNotify payNotify = new QuickUnityPayNotify();
	private QuickUnityExitNotiry exitNotiry = new QuickUnityExitNotiry();
	private String gameObjectName;
	private int initState = 0;
	private String mInitMsg = "";
	Handler mHandler = null;// new Handler(this);
	boolean isLancScape = true;

	protected void onCreate(Bundle bundle) {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onCreate", null);
		// OnSDKCrete();
		super.onCreate(bundle);

		new Thread(new Runnable() {
			public void run() {
				try {
					Thread.sleep(1000);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				// handler.sendMessage();----告诉主线程执行任务

			}
		}).start();

	}

	private void OnSDKCrete() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity OnSDKCrete", null);
		doInit();
		Sdk.getInstance().onCreate(GameMainActivity.GetInstance());
	}

	protected void onRestart() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onRestart", null);
		// Sdk.getInstance().onRestart(this);
		super.onRestart();
	}

	protected void onStart() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onStart", null);
		// Sdk.getInstance().onStart(this);
		super.onStart();
	}

	protected void onResume() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onResume", null);
		// Sdk.getInstance().onResume(this);
		super.onResume();
	}

	protected void onNewIntent(Intent intent) {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onNewIntent", null);
		// Sdk.getInstance().onNewIntent(intent);
		super.onNewIntent(intent);
	}

	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onActivityResult", null);
		// super.onActivityResult(requestCode, resultCode, data);
		Sdk.getInstance().onActivityResult(this, requestCode, resultCode, data);
	}

	protected void onPause() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onPause", null);
		// Sdk.getInstance().onPause(this);
		super.onPause();
	}

	protected void onStop() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onStop", null);
		// Sdk.getInstance().onStop(this);
		super.onStop();
	}

	protected void onDestroy() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onDestroy", null);
		// Sdk.getInstance().onDestroy(this);
		super.onDestroy();
	}

	public void doInit() {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity doInit", null);
		this.isLancScape = (getResources().getConfiguration().orientation == 2);
		try {
			if ((ContextCompat.checkSelfPermission(this, "android.permission.READ_PHONE_STATE") != 0)
					|| (ContextCompat.checkSelfPermission(this, "android.permission.WRITE_EXTERNAL_STORAGE") != 0)) {
				ActivityCompat.requestPermissions(this, new String[] { "android.permission.READ_PHONE_STATE",
						"android.permission.WRITE_EXTERNAL_STORAGE" }, 1);
			} else {
				QuickSDK.getInstance().setInitNotifier(this.initNotify).setLoginNotifier(this.loginNotify)
						.setLogoutNotifier(this.logoutNotify).setPayNotifier(this.payNotify)
						.setExitNotifier(this.exitNotiry).setIsLandScape(this.isLancScape)
						.setSwitchAccountNotifier(this.switchAccountNotify);
				Sdk.getInstance().init(this, getProductCode(), getProductKey());
			}
		} catch (Exception e) {
			QuickSDK.getInstance().setInitNotifier(this.initNotify).setLoginNotifier(this.loginNotify)
					.setLogoutNotifier(this.logoutNotify).setPayNotifier(this.payNotify)
					.setExitNotifier(this.exitNotiry).setIsLandScape(this.isLancScape)
					.setSwitchAccountNotifier(this.switchAccountNotify);
			Sdk.getInstance().init(this, getProductCode(), getProductKey());
		}
	}

	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
		Log.v(MainActivity.sdktag, "QuickUnityPlayerproxyActivity onRequestPermissionsResult", null);
		if (grantResults[0] == 0) {
			QuickSDK.getInstance().setInitNotifier(this.initNotify).setLoginNotifier(this.loginNotify)
					.setLogoutNotifier(this.logoutNotify).setPayNotifier(this.payNotify)
					.setExitNotifier(this.exitNotiry).setIsLandScape(this.isLancScape)
					.setSwitchAccountNotifier(this.switchAccountNotify);
			Sdk.getInstance().init(this, getProductCode(), getProductKey());
		} else {
			Log.e("Unity", "onRequestPermissionsResult Fail");
			if (ActivityCompat.shouldShowRequestPermissionRationale(this,
					"android.permission.WRITE_EXTERNAL_STORAGE")) {
				if (ActivityCompat.shouldShowRequestPermissionRationale(this, "android.permission.READ_PHONE_STATE")) {
					Log.d("Unity", "ActivityCompat shouldShowRequestPermissionRationale true");
					return;
				}
			}
			Log.e("Unity", "ActivityCompat shouldShowRequestPermissionRationale false");
			AlertDialog.Builder normalDialog = new AlertDialog.Builder(this);
			normalDialog.setTitle("权限设置");
			normalDialog.setMessage("请在设置中打开权限");
			normalDialog.setPositiveButton("前往应用设置", new DialogInterface.OnClickListener() {
				public void onClick(DialogInterface dialog, int which) {
					Intent intent = new Intent("android.settings.APPLICATION_DETAILS_SETTINGS");
					Uri uri = Uri.fromParts("package", QuickUnityPlayerproxyActivity.this.getPackageName(), null);
					intent.setData(uri);
					QuickUnityPlayerproxyActivity.this.startActivityForResult(intent, 999);
					QuickSDK.getInstance().setInitNotifier(QuickUnityPlayerproxyActivity.this.initNotify)
							.setLoginNotifier(QuickUnityPlayerproxyActivity.this.loginNotify)
							.setLogoutNotifier(QuickUnityPlayerproxyActivity.this.logoutNotify)
							.setPayNotifier(QuickUnityPlayerproxyActivity.this.payNotify)
							.setExitNotifier(QuickUnityPlayerproxyActivity.this.exitNotiry)
							.setIsLandScape(QuickUnityPlayerproxyActivity.this.isLancScape)
							.setSwitchAccountNotifier(QuickUnityPlayerproxyActivity.this.switchAccountNotify);
					Sdk.getInstance().init(QuickUnityPlayerproxyActivity.this,
							QuickUnityPlayerproxyActivity.this.getProductCode(),
							QuickUnityPlayerproxyActivity.this.getProductKey());
				}
			});
			normalDialog.setNegativeButton("关闭", new DialogInterface.OnClickListener() {
				public void onClick(DialogInterface dialog, int which) {
					Toast.makeText(QuickUnityPlayerproxyActivity.this, "权限被拒绝", 0).show();
					QuickSDK.getInstance().setInitNotifier(QuickUnityPlayerproxyActivity.this.initNotify)
							.setLoginNotifier(QuickUnityPlayerproxyActivity.this.loginNotify)
							.setLogoutNotifier(QuickUnityPlayerproxyActivity.this.logoutNotify)
							.setPayNotifier(QuickUnityPlayerproxyActivity.this.payNotify)
							.setExitNotifier(QuickUnityPlayerproxyActivity.this.exitNotiry)
							.setIsLandScape(QuickUnityPlayerproxyActivity.this.isLancScape)
							.setSwitchAccountNotifier(QuickUnityPlayerproxyActivity.this.switchAccountNotify);
					Sdk.getInstance().init(QuickUnityPlayerproxyActivity.this,
							QuickUnityPlayerproxyActivity.this.getProductCode(),
							QuickUnityPlayerproxyActivity.this.getProductKey());
				}
			});
			normalDialog.show();
		}
	}

	public void requestLogin() {
		this.mHandler.sendEmptyMessage(101);
	}

	public void requestLogout() {
		this.mHandler.sendEmptyMessage(102);
	}

	public void requestPay(String goodsId, String goodsName, String goodsDesc, String quantifier, String cpOrderId,
			String callbackUrl, String extraParams, String price, String amount, String count, String serverName,
			String serverId, String roleName, String roleId, String roleBalance, String vipLevel, String roleLevel,
			String partyName, String roleCreateTime) {
		OrderInfo orderInfo = new OrderInfo();
		orderInfo.setGoodsName(goodsName);
		orderInfo.setGoodsID(goodsId);
		orderInfo.setGoodsDesc(goodsDesc);
		orderInfo.setQuantifier(quantifier);
		orderInfo.setCpOrderID(cpOrderId);
		orderInfo.setCallbackUrl(callbackUrl);
		orderInfo.setExtrasParams(extraParams);
		orderInfo.setPrice(Double.valueOf(price).doubleValue());
		orderInfo.setAmount(Double.valueOf(amount).doubleValue());
		orderInfo.setCount(Integer.valueOf(count).intValue());

		GameRoleInfo roleInfo = new GameRoleInfo();
		roleInfo.setServerID(serverId);
		roleInfo.setServerName(serverName);
		roleInfo.setGameRoleID(roleId);
		roleInfo.setGameRoleName(roleName);
		roleInfo.setGameUserLevel(roleLevel);
		roleInfo.setGameBalance(roleBalance);
		roleInfo.setVipLevel(vipLevel);
		roleInfo.setPartyName(partyName);
		roleInfo.setRoleCreateTime(roleCreateTime);

		Message msg = this.mHandler.obtainMessage(103);
		HashMap<String, Object> mapObj = new HashMap();
		mapObj.put("orderInfo", orderInfo);
		mapObj.put("roleInfo", roleInfo);
		msg.obj = mapObj;
		msg.sendToTarget();
	}

	public void requestExit() {
		this.mHandler.sendEmptyMessage(104);
	}

	public void requestCallSDKShare(String title, String content, String imgPath, String imgUrl, String url,
			String type, String shareTo, String extenal) {
		ShareInfo shareInfo = new ShareInfo();
		shareInfo.setTitle(title);
		shareInfo.setContent(content);
		shareInfo.setImgPath(imgPath);
		shareInfo.setImgUrl(imgUrl);
		shareInfo.setUrl(url);
		shareInfo.setType(type);
		shareInfo.setShareTo(shareTo);
		shareInfo.setExtenal(extenal);

		Message msg = this.mHandler.obtainMessage(108);
		msg.obj = shareInfo;
		msg.sendToTarget();
	}

	public void requestCallCustomPlugin(final String roleId, final String roleName, final String serverName,
			final String vip) {
		runOnUiThread(new Runnable() {
			public void run() {
				Extend.getInstance().callPlugin(QuickUnityPlayerproxyActivity.this, 109,
						new Object[] { roleId, roleName, serverName, vip });
			}
		});
	}

	public String getUserId() {
		if (User.getInstance().getUserInfo(this) != null) {
			Log.e("unity.support", "userId->" + User.getInstance().getUserInfo(this).getUID());
			return User.getInstance().getUserInfo(this).getUID();
		}
		Log.e("unity.support", "user is null");
		return null;
	}

	public void requestUpdateRole(String serverId, String serverName, String roleName, String roleId,
			String roleBalance, String vipLevel, String roleLevel, String partyName, String roleCreateTime,
			String roleGender, String rolePower, String partId, String professionId, String profession,
			String partyRoleId, String partyRoleName, String friendlist, String isCreate) {
		GameRoleInfo roleInfo = new GameRoleInfo();
		roleInfo.setServerID(serverId);
		roleInfo.setServerName(serverName);
		roleInfo.setGameBalance(roleBalance);
		roleInfo.setGameRoleID(roleId);
		roleInfo.setGameRoleName(roleName);
		roleInfo.setVipLevel(vipLevel);
		roleInfo.setGameUserLevel(roleLevel);
		roleInfo.setPartyName(partyName);
		roleInfo.setRoleCreateTime(roleCreateTime);

		roleInfo.setGameRoleGender(roleGender);
		roleInfo.setGameRolePower(rolePower);
		roleInfo.setPartyId(partId);

		roleInfo.setProfessionId(professionId);
		roleInfo.setProfession(profession);
		roleInfo.setPartyId(partyRoleId);
		roleInfo.setPartyRoleName(partyRoleName);
		roleInfo.setFriendlist(friendlist);

		boolean isCreateRole = false;
		if ((TextUtils.isEmpty(isCreate)) || ("false".equalsIgnoreCase(isCreate))) {
			isCreateRole = false;
		} else if ("true".equalsIgnoreCase(isCreate)) {
			isCreateRole = true;
		}
		Message msg = this.mHandler.obtainMessage(105);
		if (isCreateRole) {
			msg.arg1 = 1;
		} else {
			msg.arg1 = 0;
		}
		msg.obj = roleInfo;
		msg.sendToTarget();
	}

	public int callFunc(int funcType) {
		if (isFuncSupport(funcType)) {
			Message msg = this.mHandler.obtainMessage(106);
			msg.arg1 = funcType;
			msg.sendToTarget();

			return 1;
		}
		return 0;
	}

	public int callFunc(int funcType, String s) {
		Log.d("lyy", "is called:" + s);
		if (isFuncSupport(funcType)) {
			Message msg = this.mHandler.obtainMessage(111);
			msg.arg1 = funcType;
			msg.obj = s;
			msg.sendToTarget();

			return 1;
		}
		return 0;
	}

	public boolean isFuncSupport(int funcType) {
		return Extend.getInstance().isFunctionSupported(funcType);
	}

	public void setUnityGameObjectName(String gameObjectName) {
		this.gameObjectName = gameObjectName;
		Log.d("lyy", "gameObjectName=" + gameObjectName);
		switch (this.initState) {
		case 1:
			callUnityFunc("onInitSuccess", new JSONObject().toString());
			break;
		case -1:
			JSONObject json = new JSONObject();
			try {
				json.put("msg", this.mInitMsg);
			} catch (Exception e) {
				e.printStackTrace();
			}
			callUnityFunc("onInitFailed", new JSONObject().toString());
			break;
		}
		this.initState = 0;
	}

	public String getChannelName() {
		return AppConfig.getInstance().getConfigValue("quicksdk_channel_name");
	}

	public String getChannelVersion() {
		return AppConfig.getInstance().getChannelSdkVersion();
	}

	public int getChannelType() {
		return AppConfig.getInstance().getChannelType();
	}

	public String getSDKVersion() {
		return AppConfig.getInstance().getSdkVersion();
	}

	public String getConfigValue(String key) {
		return AppConfig.getInstance().getConfigValue(key);
	}

	public boolean isChannelHasExitDialog() {
		return QuickSDK.getInstance().isShowExitDialog();
	}

	public void exitGame() {
		Log.d("lyy", "调用了exitGame()");
		finish();
		System.exit(0);
	}

	private class QuickUnityInitNotify implements InitNotifier {
		private QuickUnityInitNotify() {
		}

		public void onSuccess() {
			if (!TextUtils.isEmpty(QuickUnityPlayerproxyActivity.this.gameObjectName)) {
				QuickUnityPlayerproxyActivity.this.callUnityFunc("onInitSuccess", new JSONObject().toString());
			} else {
				QuickUnityPlayerproxyActivity.this.initState = 1;
			}
		}

		public void onFailed(String msg, String trace) {
			if (!TextUtils.isEmpty(QuickUnityPlayerproxyActivity.this.gameObjectName)) {
				JSONObject json = new JSONObject();
				try {
					json.put("msg", msg);
				} catch (Exception e) {
					e.printStackTrace();
				}
				QuickUnityPlayerproxyActivity.this.callUnityFunc("onInitFailed", new JSONObject().toString());
			} else {
				QuickUnityPlayerproxyActivity.this.mInitMsg = msg;
				QuickUnityPlayerproxyActivity.this.initState = -1;
			}
		}
	}

	private class QuickUnityLoginNotify implements LoginNotifier {
		private QuickUnityLoginNotify() {
		}

		public void onSuccess(UserInfo userInfo) {
			JSONObject json = new JSONObject();
			try {
				json.put("userName", userInfo.getUserName() == null ? "" : userInfo.getUserName());
				json.put("userId", userInfo.getUID() == null ? "" : userInfo.getUID());
				json.put("userToken", userInfo.getToken() == null ? "" : userInfo.getToken());
				json.put("channelToken", userInfo.getChannelToken());

				json.put("msg", "success");
			} catch (Exception e) {
				e.printStackTrace();
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onLoginSuccess", json.toString());
		}

		public void onCancel() {
			JSONObject json = new JSONObject();
			try {
				json.put("msg", "cancel");
			} catch (Exception localException) {
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onLoginFailed", json.toString());
		}

		public void onFailed(String msg, String trace) {
			JSONObject json = new JSONObject();
			try {
				json.put("msg", msg);
			} catch (Exception localException) {
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onLoginFailed", json.toString());
		}
	}

	private class QuickUnitySwitchAccountNotify implements SwitchAccountNotifier {
		private QuickUnitySwitchAccountNotify() {
		}

		public void onCancel() {
		}

		public void onFailed(String msg, String trace) {
		}

		public void onSuccess(UserInfo userInfo) {
			Log.d("lyy", "切换账号成功");
			JSONObject json = new JSONObject();
			try {
				json.put("userName", userInfo.getUserName() == null ? "" : userInfo.getUserName());
				json.put("userId", userInfo.getUID() == null ? "" : userInfo.getUID());
				json.put("userToken", userInfo.getToken() == null ? "" : userInfo.getToken());
				json.put("channelToken", userInfo.getChannelToken());

				json.put("msg", "success");
			} catch (Exception e) {
				e.printStackTrace();
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onSwitchAccountSuccess", json.toString());
		}
	}

	private class QuickUnityLogoutNotify implements LogoutNotifier {
		private QuickUnityLogoutNotify() {
		}

		public void onSuccess() {
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onLogoutSuccess", "success");
		}

		public void onFailed(String msg, String trace) {
		}
	}

	private class QuickUnityPayNotify implements PayNotifier {
		private QuickUnityPayNotify() {
		}

		public void onSuccess(String quickOrderId, String cpOrderId, String extrasParams) {
			JSONObject json = new JSONObject();
			try {
				json.put("orderId", quickOrderId);
				json.put("cpOrderId", cpOrderId);
				json.put("extraParam", extrasParams);
			} catch (Exception localException) {
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onPaySuccess", json.toString());
		}

		public void onCancel(String cpOrderID) {
			JSONObject json = new JSONObject();
			try {
				json.put("orderId", "");
				json.put("cpOrderId", cpOrderID);
				json.put("extraParam", "");
			} catch (Exception localException) {
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onPayCancel", json.toString());
		}

		public void onFailed(String cpOrderID, String message, String trace) {
			JSONObject json = new JSONObject();
			try {
				json.put("orderId", "");
				json.put("cpOrderId", cpOrderID);
				json.put("extraParam", message);
			} catch (Exception localException) {
			}
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onPayFailed", json.toString());
		}
	}

	private class QuickUnityExitNotiry implements ExitNotifier {
		private QuickUnityExitNotiry() {
		}

		public void onSuccess() {
			QuickUnityPlayerproxyActivity.this.callUnityFunc("onExitSuccess", "success");
		}

		public void onFailed(String msg, String trace) {
		}
	}

	public void callUnityFunc(String funcName, String paramStr) {
		if (TextUtils.isEmpty(this.gameObjectName)) {
			Log.e("unity.support", "gameObject is null, please set gameObject first");
			return;
		}
		UnityPlayer.UnitySendMessage(this.gameObjectName, funcName, paramStr);
	}

	public boolean handleMessage(Message msg) {
		switch (msg.what) {
		case 101:
			Log.e("unity.support", "login");
			User.getInstance().login(this);
			break;
		case 102:
			Log.e("unity.support", "logout");

			User.getInstance().logout(this);
			break;
		case 103:
			Log.e("unity.support", "pay");

			HashMap<String, Object> mapObj = (HashMap) msg.obj;
			OrderInfo orderInfo = (OrderInfo) mapObj.get("orderInfo");
			GameRoleInfo roleInfo = (GameRoleInfo) mapObj.get("roleInfo");
			Payment.getInstance().pay(this, orderInfo, roleInfo);
			break;
		case 104:
			Log.e("unity.support", "exit");

			Sdk.getInstance().exit(this);
			break;
		case 105:
			Log.d("unity.support", "update role info");

			GameRoleInfo roleInfo2 = (GameRoleInfo) msg.obj;
			boolean isCreate = msg.arg1 == 1;

			User.getInstance().setGameRoleInfo(this, roleInfo2, isCreate);
			break;
		case 106:
			int funcType = msg.arg1;
			Extend.getInstance().callFunction(this, funcType);
			break;
		case 108:
			Log.e("unity.support", "call channel share");
			ShareInfo shareInfo = (ShareInfo) msg.obj;
			Extend.getInstance().callFunctionWithParams(this, 108, new Object[] { shareInfo });
			break;
		case 111:
			Log.e("unity.support", "call getGoodsInfo");
			String gids = (String) msg.obj;
			ArrayList<String> gidsList = new ArrayList();
			String[] gid = gids.split(",");
			if (gid != null) {
				for (int i = 0; i < gid.length; i++) {
					gidsList.add(gid[i]);
				}
				if (gidsList.size() > 0) {
					Extend.getInstance().callFunctionWithParamsCallBack(this, 111, new BaseCallBack() {
						public void onSuccess(Object... infos) {
							String goodsInfos = (String) infos[0];
							QuickUnityPlayerproxyActivity.this.callUnityFunc("onSuccess", goodsInfos);
						}

						public void onFailed(Object... infos) {
							String msg = (String) infos[0];
							QuickUnityPlayerproxyActivity.this.callUnityFunc("onFail", msg);
						}
					}, new Object[] {

							gidsList });
				}
			} else {
				callUnityFunc("onFailed", gids);
			}
			break;
		}
		return false;
	}

	public abstract String getProductCode();

	public abstract String getProductKey();
}
