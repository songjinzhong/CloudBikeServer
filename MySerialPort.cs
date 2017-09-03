using UnityEngine;
using System.Collections;
//Other libraries
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
//串口命名空间
using System.IO.Ports;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public class MySerialPort : MonoBehaviour {
	public String portName;
	private int data1;
	private int firstData;
	private Boolean b_first = true;
	private float data2;
	public int Angle{get{ return data1;}}
	public float Speed{get{ return data2;}}
	string byteToString;
	Byte a_byte=97;
	Byte b_byte=98;
	Byte[] num;
	List<byte> liststr;//在ListByte中读取数据，用于做数据处理
	List<byte> ListByte;//存放读取的串口数据
	private Thread tPort;
	private Thread tPortDeal;//这两个为两个线程，一个是读取串口数据的线程一个是处理数据的线程
	bool isStartThread;//控制FixedUpdate里面的两个线程是否调用（当准备调用串口的Close方法时设置为false）
	byte[] strOutPool = new byte[6];
	SerialPort spstart;

	void Start()
	{
		data1 = 33;
		data2 = 0f;
		liststr = new List<byte>();
		ListByte = new List<byte>();
		portName = null;
		ArrayList info;
		try{
			info = LoadFile (Application.persistentDataPath, "config.txt");
			portName = info [1].ToString();
		}catch(Exception e){
			//DebugOnScreen.Add ("config_error","配置信息错误");
		}
		spstart = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
		try
		{
			spstart.Open();
			isStartThread=true;
			tPort = new Thread(DealData);
			tPort.Start();
			tPortDeal = new Thread(ReceiveData);
			tPortDeal.Start();
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
			if (portName == null) {
				
			} else {
				//DebugOnScreen.Add ("COM 错误", portName + " 端口打开失败，请检查端口");
			}
		}


	}
	void FixedUpdate()
	{
		if (isStartThread)
		{
			if (!tPortDeal.IsAlive)
			{
				tPortDeal = new Thread(ReceiveData);
				tPortDeal.Start();
			}
			if (!tPort.IsAlive)
			{
				tPort = new Thread(DealData);
				tPort.Start();
			}
		}
	}
	void OnApplicationQuit(){
		spstart.Close ();
	}
	private void ReceiveData()
	{
		try
		{
			Byte[] buf = new Byte[1];
			string sbReadline2str = string.Empty;
			if (spstart.IsOpen)
			{
				//tickcount++;
				spstart.Read(buf, 0, 1);
			}
			if (buf.Length == 0)
			{
				return;
			}
			if (buf != null)
			{
				for (int i = 0; i < buf.Length; i++)
				{
					ListByte.Add(buf[i]);
				}
				if(ListByte[ListByte.Count-1]==b_byte&&ListByte.Count>22){
					ListByte.Clear();
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
			//spantime = 0;
		}
	}

	private void DealData(){
		while(true){
			liststr.Add (ListByte[0]);
			ListByte.Remove (ListByte[0]);
			num=new byte[8];
			if(liststr [liststr.Count - 1] == a_byte){
				for (int i = 0; i < liststr.Count - 1; i++) {
					num [i] = liststr [i];
				}
				byteToString= System.Text.Encoding.ASCII.GetString ( num );
				if (b_first) {
					firstData = int.Parse (byteToString);
					b_first = false;
				} else {
					data1 = 33 + int.Parse (byteToString) - firstData;
				}
				liststr.Clear ();
			}
			if(liststr [liststr.Count - 1] == b_byte){
				for (int i = 0; i < liststr.Count - 1; i++) {
					num [i] = liststr [i];
				}
				byteToString= System.Text.Encoding.ASCII.GetString ( num );
				data2 = float.Parse (byteToString);
				liststr.Clear ();
			}
		}
	}


	IEnumerator ClosePort()//该方法为关闭串口的方法，当程序退出或是离开该页面或是想停止串口时调用。
	{
		isStartThread=false;//停止掉FixedUpdate里面的两个线程的调用
		yield return new WaitForSeconds(1);//等一秒钟，让两个线程确实停止之后在执行Close方法
		spstart.Close();
	}

	public static ArrayList LoadFile(string Path, string name){
		ArrayList arrlist = new ArrayList ();
		StreamReader sr = null;
		try{
			sr = File.OpenText(Path+"//"+name);
		}catch(Exception e){
			//DebugOnScreen.Add ("config", Application.persistentDataPath + "下没有配置 config.txt 文件");
			return arrlist;
		}
		string line;
		while ((line = sr.ReadLine ()) != null) {
			arrlist.Add (line);
		}
		sr.Close ();
		sr.Dispose ();
		return arrlist;
	}
}






