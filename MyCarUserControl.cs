using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float horizontal { get { return h; } }
	private Boolean isSpeed;

    public float c_v
    {
        set
        {
            v = value;
        }
    }

    public float c_h
    {
        set
        {
            h = value;
        }
    }

    void Awake()
    {
        controller = GetComponent<MyCarController>();
        serialPort = GetComponent<MySerialPort>();
        blueToothCon = GetComponent<BlueToothCon>();
    }
    float h = 0f;
    float v = 0f;
	float hh;
    float handbrake;
    public void Start()
    {
        m_switch = 1;
        ArrayList info;
        try
        {
            info = MySerialPort.LoadFile(Application.persistentDataPath, "config.txt");
            m_switch = float.Parse(info[3].ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            DebugOnScreen.Add("config_error", "配置信息错误");
        }
		index = 1;
		foreach (Transform child in empty.transform) {
			va.Add (child.transform.position);
		}
		hh = 0;
		isSpeed = false;
		Debug.Log (empty);
    }
    void FixedUpdate()
    {
        KeyBoardController();
        //		SerialController ();
        //		BlueToothController();
    }


    void KeyBoardController()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        //handbrake = Input.GetAxis("Jump");
		//Debug.Log(v + "-------" +h);
		if (h == -2f || isSpeed == true) {
			isSpeed = true;
			RunOnlySpeed ();
		} else {
      Debug.Log(v);
			frontAngle2.transform.localRotation = Quaternion.Euler(new Vector3(0, 20f * h, -15f * h));
			//Debug.Log (v);
			controller.Move(h, 1f, 0, handbrake, 3 * v);
		}
    }
    void SerialController()
    {
        h = serialPort.Angle;
        v = serialPort.Speed;
        h = m_switch * (h - 33f) / 33f;
        if (h > 1)
            h = 1;
        else if (h < -1)
            h = -1;
        frontAngle2.transform.localRotation = Quaternion.Euler(new Vector3(0, 20f * h, -15f * h));
        controller.Move(h, 1f, 0, handbrake, 60f * v / 100f);
    }
    void BlueToothController()
    {
        h = blueToothCon.Angle;
        v = blueToothCon.Speed;
        h = -(h - 33f) / 33f;
        frontAngle2.transform.localRotation = Quaternion.Euler(new Vector3(0, 20f * h, -15f * h));
        controller.Move(h, 1f, 0, handbrake, 60f * v / 100f);
    }

	void RunOnlySpeed(){
		//v = serialPort.Speed;
		v3 = frontAngle2.transform.position;
		judge (v3, va [index]);
		h = angle_360 (v3, va [index]);
		h = Mathf.Clamp (h, -20, 20);
		float change = h - hh;
		if (change > 1 || change < -1) {
			hh += change * 0.01f;
		} else {
			hh = h;
		}
		//Debug.Log (hh);
		frontAngle2.transform.localRotation = Quaternion.Euler (new Vector3(0,10f*hh/15f,-7f*hh/15f));
		controller.Move(h/20f, 1f, 0, handbrake, 3 * v);
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
		Debug.Log (ret);
		return ret;
	} 

	void judge(Vector3 v1, Vector3 v2){
		Debug.Log (index);
		if (Vector3.Distance(v1, v2) < 3f) {
			index += 1;
		}
		if (index == va.Count) {
			index = 0;
		}
	}
}
