package com.yyty.fsld.xzjh;

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
    //AppConfig.getInstance().init(base);
    
    //Sdk.getInstance().registerGlobalReceiver(base);
    
   // ExCollector.a(base);
    
    super.attachBaseContext(base);
  }


  
  public void onCreate() {
   // this.a = com.quicksdk.utility.a.a();
   // this.a.adtActivity().onApplicationInit((Context)this);
    
    super.onCreate();
  }
}
