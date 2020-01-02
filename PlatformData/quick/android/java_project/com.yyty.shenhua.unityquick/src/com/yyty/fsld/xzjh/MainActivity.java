package com.yyty.fsld.xzjh;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.widget.Toast;
import com.qk.game.SplashActivity;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;
import org.json.JSONObject;




public class MainActivity
  extends SplashActivity
{
  public void onSplashStop() { onSDKSplahStop(); }



  
  public static String sdktag = "QSDK";
  public static Boolean DebugEnable = Boolean.valueOf(false);
  
  public static String gameName;
  
  String[] mustPermissions = new String[] { "android.permission.WRITE_EXTERNAL_STORAGE" };


  
  String[] requestPermissions = new String[] { "android.permission.READ_SMS", "android.permission.SEND_SMS", 
      "android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.ACCESS_FINE_LOCATION", 
      "android.permission.READ_PHONE_STATE", "android.permission.CAMERA" };


  
  private String[] ReadFileInfo(String fileName) {
    try {
      InputStream is = getAssets().open(fileName);
      InputStreamReader inputReader = new InputStreamReader(is);
      BufferedReader bufReader = new BufferedReader(inputReader);
      
      List<String> arrayList = new ArrayList<String>();
      
      String line;
      while ((line = bufReader.readLine()) != null)
      {
        arrayList.add(line);
      }
      
      String[] args = new String[arrayList.size()];
      arrayList.toArray(args);
      
      bufReader.close();
      inputReader.close();
      is.close();
      
      return args;
    }
    catch (Exception e) {
      Log.v(sdktag, "ReadFileInfo Exception:" + e.getMessage(), null);
      e.printStackTrace();
      
      return null;
    } 
  }

  
  private void onSDKSplahStop() {
    Log.v(sdktag, "MainActivity onCreate", null);
    
    try {
      InputStream is = getAssets().open("shgame_config.json");
      int size = is.available();
      byte[] buffer = new byte[size];
      is.read(buffer);
      is.close();
      String jsonStr = new String(buffer, "UTF-8");
      JSONObject jsonObject = new JSONObject(jsonStr);
      DebugEnable = Boolean.valueOf(jsonObject.getBoolean("Debug"));
    }
    catch (Exception e) {
      e.printStackTrace();
    } 

    
    this.mustPermissions = ReadFileInfo("shgame_mustpermission.info");
    this.requestPermissions = ReadFileInfo("shgame_permission.info");
    
    if (Build.VERSION.SDK_INT >= 23) {
      requestPower();
    } else {
      onInitStop();
    } 
  }



  
  public boolean checkpermission() {
    if (Build.VERSION.SDK_INT < 23) {
      return true;
    }
    for (int i = 0; i < this.mustPermissions.length; i++) {
      if (ContextCompat.checkSelfPermission((Context)this, this.mustPermissions[i]) != 0) {
        Toast.makeText((Context)this, "权限" + this.requestPermissions[i] + "没有启动，请开启权限再启动游戏", 0).show();
        return true;
      } 
    } 

    
    return true;
  }

  
  public void onInitStop() {
    if (checkpermission()) {
      Log.v(sdktag, "MainActivity onInitStop", null);
      
      Intent intent = new Intent((Context)this, GameMainActivity.class);
      startActivity(intent);
      finish();
    } 
  }
  
  public void requestPower() {
    Log.v(sdktag, "MainActivity requestPower", null);
    int PERMISSION_GRANTED = 0;
    
    if (ContextCompat.checkSelfPermission((Context)this, "android.permission.WRITE_EXTERNAL_STORAGE") != PERMISSION_GRANTED) {
      
      Log.v(sdktag, "MainActivity start requestPermissions", null);
      ActivityCompat.requestPermissions((Activity)this, this.requestPermissions, 1);
    } else {
      Log.v(sdktag, "MainActivity onRequestPermissionsResult success ! start game!", null);
      onInitStop();
    } 
  }
  
  int successCount = 0;


  
  public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
    super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    
    Log.v(sdktag, "MainActivity onRequestPermissionsResult" + requestCode, null);
    if (requestCode == 1)
      for (int i = 0; i < permissions.length; i++) {
        Log.v(sdktag, "MainActivity onRequestPermissionsResult:" + permissions[i] + "   " + grantResults[i], null);
        
        if (grantResults[i] == 0) {

          
          String persion = permissions[i];
          
          for (int j = 0; i < this.mustPermissions.length; j++) {
            if (persion.equals(this.mustPermissions[j])) {
              
              this.successCount++;
              
              if (this.successCount >= this.mustPermissions.length) {
                
                Log.v(sdktag, "MainActivity onRequestPermissionsResult success ! start game!" + requestCode, null);
                onInitStop();
              } 
              break;
            } 
          } 
        } 
      }  
  }
}
