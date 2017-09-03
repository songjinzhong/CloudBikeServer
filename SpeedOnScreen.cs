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
	
	void Update()
	{

	}
	
	void OnGUI()
	{
		Display();
	}
	
	void Display()
	{
		float speed = 33f;
    	GUI.Box(new Rect(0, 300, 300, 50), "速度: " + speed,style);
	}
}
