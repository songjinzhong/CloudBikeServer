using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 自行车控制信息，可选择键盘，串口，蓝牙三种方式控制自行车
 * 
 */

[RequireComponent(typeof(MyCarController))]
public class MyCarUserControl : MonoBehaviour
{
	MyCarController controller;
	MySerialPort serialPort;
	BlueToothCon blueToothCon;
	public GameObject empty;
	List<Vector3> va = new List<Vector3>();
	//Vector3 [] va = new Vector3[9];
	Vector3 v3;
	int index;
	private float m_switch;
	public GameObject frontAngle2;
	public float horizontal{get{ return h;}}
	void Awake()
	{
		controller = GetComponent<MyCarController>();
		serialPort = GetComponent<MySerialPort> ();
		blueToothCon = GetComponent<BlueToothCon> ();
	}
	float h;
	float v;
	float hh;
	float handbrake;
	public void Start(){
		m_switch = 1;
		ArrayList info;
		try{
			info = MySerialPort.LoadFile (Application.persistentDataPath, "config.txt");
			m_switch = float.Parse (info [3].ToString ());
		}catch(Exception e){
			Debug.Log (e.ToString ());
			DebugOnScreen.Add ("config_error", "配置信息错误");
			DebugOnScreen.Add ("config_path", Application.persistentDataPath);
		}
		index = 1;
		foreach (Transform child in empty.transform) {
			va.Add (child.transform.position);
		}
		hh = 0;
		Debug.Log (empty);
	}
	void FixedUpdate()
	{
//		KeyBoardController (); // 键盘控制
		SerialController (); // 串口通信
//		BlueToothController(); // 蓝牙控制
	}

	// 键盘控制函数
	void KeyBoardController(){
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");
		handbrake = Input.GetAxis("Jump");
		frontAngle2.transform.localRotation = Quaternion.Euler (new Vector3(0,20f*h,-15f*h));
		controller.Move(h, v, v, handbrake,100f);
	}
	// 串口控制函数
	void SerialController(){
		//h = serialPort.Angle;
		v = serialPort.Speed;

		//frontAngle2.transform.localRotation = Quaternion.Euler (new Vector3(0,20f*h,-15f*h));
		//controller.Move(h, 1f, 0, handbrake,60f*v/100f);
		//Debug.Log(index);
		v3 = frontAngle2.transform.position;
		judge (v3, va [index]);
		h = angle_360 (v3, va [index]);
		//Debug.Log (h);
		//h = m_switch * (h - 33f) / 33f;
		//if (h > 1)
		//	h = 1;
		//else if (h < -1)
		//	h = -1;
		h = Mathf.Clamp (h, -40, 40);
		float change = h - hh;
		if (change > 1 || change < -1) {
			hh += change * 0.01f;
		} else {
			hh = h;
		}
		frontAngle2.transform.localRotation = Quaternion.Euler (new Vector3(0,20f*hh/33f,-15f*hh/33f));
		controller.Move(hh, 1f, 0, handbrake,60f*v/100f);
	}
	// 蓝牙控制函数
	void BlueToothController(){
		h = blueToothCon.Angle;
		v = blueToothCon.Speed;
		h = -(h - 33f) / 33f;
		frontAngle2.transform.localRotation = Quaternion.Euler (new Vector3(0,20f*h,-15f*h));
		controller.Move(h, 1f, 0, handbrake,60f*v/100f);
	}

	//计算夹角
	float angle_360(Vector3 from_, Vector3 to_){
		float x = to_.x - from_.x;
		float y = to_.z - from_.z;
		float r = (float)(Mathf.Rad2Deg * Math.Atan (x / y));
		if (y < 0 && x > 0) {
			r = 180 + r;
		} else if (y < 0 && x < 0) {
			r = r - 180;
		}
		float ret =  r - frontAngle2.transform.parent.transform.parent.transform.rotation.eulerAngles.y; 
		while (ret >= 360) {
			ret -= 360;
		}
		while (ret <= -90) {
			ret += 360;
		}
		//Debug.Log (ret);
		return ret;
	} 

	void judge(Vector3 v1, Vector3 v2){
		Debug.Log (index);
		if (Vector3.Distance(v1, v2) < 2f) {
			index += 1;
		}
		if (index == 16) {
			index = 0;
		}
	}
}
