package com.yyty.fsld.xzjh.huawei;

import android.app.Application;
import android.content.Context;

import com.quicksdk.Sdk;
import com.quicksdk.apiadapter.IAdapterFactory;
import com.quicksdk.ex.ExCollector;
import com.quicksdk.utility.AppConfig;

public class GameApplication
  extends Application
{
  private IAdapterFactory a = null;


  
  protected void attachBaseContext(Context base) {
	super.attachBaseContext(base);
	  
    AppConfig.getInstance().init(base);
    
    Sdk.getInstance().registerGlobalReceiver(base);
    
   //ExCollector.a(base);
    
    
  }


  
  public void onCreate() {
	  super.onCreate();
	  
    this.a = com.quicksdk.utility.a.a();
   this.a.adtActivity().onApplicationInit((Context)this);
    
    
  }
}
