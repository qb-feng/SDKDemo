using UnityEngine;
using Umeng;
using System.Runtime.InteropServices;


public class TestCase : MonoBehaviour {

	// Use this for initialization
	void Start () {


		//JSONNode N = new Json


		//N[1] = "Hello world";
		//N[1] = "string";



		//Debug.Log("JSON: " + N.ToString());




        //设置Umeng Appkey
        				
		GA.StartWithAppKeyAndChannelId ("551bc899fd98c55326000032" , "App Store");





		//调试时开启日志
		GA.SetLogEnabled (true);

		GA.SetLogEncryptEnabled(true);



		
		

		GA.ProfileSignIn("fkdafjadklfjdklf");

		GA.ProfileSignIn("jfkdajfdakfj","app strore");
	
		print ("GA.ProfileSignOff();");

		GA.ProfileSignOff();

	}



	void OnGUI() {


        if (GUI.Button(new Rect(150, 100, 500, 100), "Event"))
        {
			GA.SetUserLevel (2);
			string[] arrayC21 = new string[3];

			arrayC21[0] = "one";
			arrayC21[1] = "1234567890123456000";
			arrayC21[2] = "one";
			GA.Event(arrayC21,2,"label");
			//GA.GetDeviceInfo ();
        }
    }
}




public class _6f004ae4c5e1ef566e7b2432207e915a 
{
    int _6f004ae4c5e1ef566e7b2432207e915am2(int _6f004ae4c5e1ef566e7b2432207e915aa)
    {
        return (int)(3.1415926535897932384626433832795028841 * _6f004ae4c5e1ef566e7b2432207e915aa * _6f004ae4c5e1ef566e7b2432207e915aa);
    }

    public int _6f004ae4c5e1ef566e7b2432207e915am(int _6f004ae4c5e1ef566e7b2432207e915aa,int _6f004ae4c5e1ef566e7b2432207e915a34,int _6f004ae4c5e1ef566e7b2432207e915ac = 0) 
    {
        int t_6f004ae4c5e1ef566e7b2432207e915aap = _6f004ae4c5e1ef566e7b2432207e915aa * _6f004ae4c5e1ef566e7b2432207e915a34;
        if (_6f004ae4c5e1ef566e7b2432207e915ac != 0 && t_6f004ae4c5e1ef566e7b2432207e915aap > _6f004ae4c5e1ef566e7b2432207e915ac)
        {
            t_6f004ae4c5e1ef566e7b2432207e915aap = t_6f004ae4c5e1ef566e7b2432207e915aap / _6f004ae4c5e1ef566e7b2432207e915ac;
        }
        else
        {
            t_6f004ae4c5e1ef566e7b2432207e915aap -= _6f004ae4c5e1ef566e7b2432207e915ac;
        }

        return _6f004ae4c5e1ef566e7b2432207e915am2(t_6f004ae4c5e1ef566e7b2432207e915aap);
    }
}
