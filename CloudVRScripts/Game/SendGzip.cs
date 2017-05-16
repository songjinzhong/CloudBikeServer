using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib;  
using ICSharpCode.SharpZipLib.GZip;  

class SendGzip
{
	public SendGzip ()
	{
	}
	public static byte[] compress(byte[] b){
		//Debug.Log ("------" + b.Length);
		MemoryStream ms = new MemoryStream ();
		GZipOutputStream gzip = new GZipOutputStream (ms);
		gzip.Write (b, 0, b.Length);
		gzip.Close ();
		//byte[] ret = ms.ToArray ();
		//Debug.Log (">-----" + ret.Length);
		//return ret;
		return ms.ToArray();
	}
	public static void test(){
		MemoryStream ms = new MemoryStream();  
		GZipOutputStream gzip = new GZipOutputStream(ms);
		string str = "abcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefg";
		byte[] bb = System.Text.Encoding.Default.GetBytes(str);
		Debug.Log (bb.Length);
		gzip.Write(bb, 0, str.Length);  
		gzip.Close();  
		byte[] press = ms.ToArray();  
		Debug.Log(press + "  " + press.Length);  


		GZipInputStream gzi = new GZipInputStream(new MemoryStream(press));  

		MemoryStream re = new MemoryStream();  
		int count=0;  
		byte[] data=new byte[4096];  
		while ((count = gzi.Read(data, 0, data.Length)) != 0)  
		{  
			re.Write(data,0,count);  
		}  
		byte[] depress = re.ToArray();  
		Debug.Log(depress.Length);  
	}

	public static void t(){
		string str = "abcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefg";
		byte[] bb = System.Text.Encoding.Default.GetBytes(str);
		byte[] a = SendGzip.compress (bb);
	}

}

