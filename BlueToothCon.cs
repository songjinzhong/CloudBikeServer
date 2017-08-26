using UnityEngine;
using System.Collections;
using System;


public class BlueToothCon : MonoBehaviour {
	AndroidJavaClass jc;
	AndroidJavaObject jo;
	private int data1;
	private float data2;
	string receiveData="33a30b";
	public int Angle{get{ return data1;}}
	public float Speed{get{ return data2;}}
	Byte[] num;
	Byte a_byte=97;
	Byte b_byte=98;
	string byteToString;

	void Start () {
		jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		data1 = 33;
		data2 = 0f;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Home)){
			Application.Quit ();
		}
		receiveData = jo.Call<string> ("readButton","a");
		DealData ();
	}
	void OnGUI(){
		if(GUILayout.Button("搜索蓝牙",GUILayout.Height(100))){
			jo.Call ("StartSearchBlueTooth");
		}
	}
	void DealData(){
		num=new byte[8];
		int i=0;
		char[] chars = receiveData.ToCharArray ();
		for(;chars[i]!=a_byte;i++){
			num [i] = (byte)chars [i];
		}
		byteToString= System.Text.Encoding.ASCII.GetString ( num );
		data1 = int.Parse (byteToString);
		num=new byte[8];
		i++;
		for(int j=0;chars[i]!=b_byte;i++,j++){
			num [j] = (byte)chars [i];
		}
		byteToString= System.Text.Encoding.ASCII.GetString ( num );
		data2 = float.Parse (byteToString);
	}
}
