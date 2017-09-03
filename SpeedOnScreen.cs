using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * 用于将当前速度显示在图像上
 */
public  class SpeedOnScreen : MonoBehaviour {
	static List<string> names = new List<string>();
	public GUIStyle style = null;
	public Rect rect;

	private bool showFlag = false;
	private float speed = 0f;
	private float distance = 0f;
	private float heartRate = 0f;
	private float oxygen = 0f;
	
	void Update()
	{

	}
	
	void OnGUI()
	{
		Display();
	}
	
	void Display()
	{
		if (showFlag) {
			GUI.Box(new Rect(0, 100, 300, 50), "速度: " + speed,style);
			GUI.Box(new Rect(10, 100, 300, 50), "行程: " + distance,style);
			GUI.Box(new Rect(20, 100, 300, 50), "心率: " + heartRate,style);
			GUI.Box(new Rect(30, 100, 300, 50), "血氧: " + oxygen,style);
		}
	}
	public float Speed{
		set{
			speed = value;
		}
	}
	public float Distance{
		set{
			distance = value;
		}
	}
	public float HeartRate{
		set{
			heartRate = value;
		}
	}
	public float Oxygen{
		set{
			oxygen = value;
		}
	}
	public bool ShowFlag{
		set{
			showFlag = value;
		}
	}
}
