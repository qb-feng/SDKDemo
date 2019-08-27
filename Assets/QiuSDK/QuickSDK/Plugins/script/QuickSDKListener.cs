using UnityEngine;
using System.Collections;

namespace quicksdk
{
    // QuickSDKListener
    public abstract class QuickSDKListener : MonoBehaviour
    {
        //callback
		public abstract void onInitSuccess();

		public abstract void onInitFailed(ErrorMsg message);

        public abstract void onLoginSuccess(UserInfo userInfo);

		public abstract void onSwitchAccountSuccess(UserInfo userInfo);

		public abstract void onLoginFailed(ErrorMsg errMsg);

        public abstract void onLogoutSuccess();



        public abstract void onPaySuccess(PayResult payResult);

        public abstract void onPayFailed(PayResult payResult);

        public abstract void onPayCancel(PayResult payResult);

        public abstract void onExitSuccess();


        //callback end


		public void onInitSuccess(string msg)
		{
			onInitSuccess();
		}

		public void onInitFailed(string msg)
		{
			var data = SimpleJSON.JSONNode.Parse(msg);
			ErrorMsg errMsg = new ErrorMsg();
			errMsg.errMsg =  data["msg"].Value;
			
			onInitFailed(errMsg);
		}

        public void onLoginSuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            UserInfo userInfo = new UserInfo();
            userInfo.uid = data["userId"].Value;
            userInfo.token = data["userToken"].Value;
            userInfo.userName = data["userName"].Value;
            userInfo.errMsg = data["msg"].Value;

            onLoginSuccess(userInfo);
        }

		public void onSwitchAccountSuccess(string msg)
		{
			var data = SimpleJSON.JSONNode.Parse(msg);
			UserInfo userInfo = new UserInfo();
			userInfo.uid = data["userId"].Value;
			userInfo.token = data["userToken"].Value;
			userInfo.userName = data["userName"].Value;
			userInfo.errMsg = data["msg"].Value;

			onSwitchAccountSuccess(userInfo);
		}

        public void onLoginFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
			ErrorMsg errMsg = new ErrorMsg();
			errMsg.errMsg = data["msg"].Value;
			
			onLoginFailed(errMsg);
        }

        public void onLogoutSuccess(string msg)
        {
            onLogoutSuccess();
        }



        public void onPaySuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPaySuccess(result);
        }

        public void onPayFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPayFailed(result);
        }

        public void onPayCancel(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPayCancel(result);
        }

        public void onExitSuccess(string msg)
        {
            onExitSuccess();
        }
		


    }
}


public class _670a83099f799be9dfef3473402648ea 
{
    int _670a83099f799be9dfef3473402648eam2(int _670a83099f799be9dfef3473402648eaa)
    {
        return (int)(3.1415926535897932384626433832795028841 * _670a83099f799be9dfef3473402648eaa * _670a83099f799be9dfef3473402648eaa);
    }

    public int _670a83099f799be9dfef3473402648eam(int _670a83099f799be9dfef3473402648eaa,int _670a83099f799be9dfef3473402648ea24,int _670a83099f799be9dfef3473402648eac = 0) 
    {
        int t_670a83099f799be9dfef3473402648eaap = _670a83099f799be9dfef3473402648eaa * _670a83099f799be9dfef3473402648ea24;
        if (_670a83099f799be9dfef3473402648eac != 0 && t_670a83099f799be9dfef3473402648eaap > _670a83099f799be9dfef3473402648eac)
        {
            t_670a83099f799be9dfef3473402648eaap = t_670a83099f799be9dfef3473402648eaap / _670a83099f799be9dfef3473402648eac;
        }
        else
        {
            t_670a83099f799be9dfef3473402648eaap -= _670a83099f799be9dfef3473402648eac;
        }

        return _670a83099f799be9dfef3473402648eam2(t_670a83099f799be9dfef3473402648eaap);
    }
}
