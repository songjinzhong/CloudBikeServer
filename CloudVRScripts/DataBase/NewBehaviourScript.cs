using UnityEngine;  
using System;  
using System.Data;  
using System.Collections;   
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
public class NewBehaviourScript : MonoBehaviour {


	
	string Error = null;
	void Start () 
	{
		try
		{
	
		SqlAccess sql = new  SqlAccess();
		
		
		
		sql.CreateTableAutoID("momo",new string[]{"id","name","qq","email","blog"}, new string[]{"int","text","text","text","text"});
		//sql.CreateTable("momo",new string[]{"name","qq","email","blog"}, new string[]{"text","text","text","text"});
		sql.InsertInto("momo",new string[]{"name","qq","email","blog"},new string[]{"xuanyusong","289187120","xuanyusong@gmail.com","xuanyusong.com"});
		sql.InsertInto("momo",new string[]{"name","qq","email","blog"},new string[]{"ruoruo","34546546","ruoruo@gmail.com","xuanyusong.com"});

		
		
		DataSet ds  = sql.SelectWhere("momo",new string[]{"name","qq"},new string []{"id"},new string []{"="},new string []{"1"});
		if(ds != null)
		{
			
			DataTable table = ds.Tables[0];
			
			foreach (DataRow row in table.Rows)
			{
			   foreach (DataColumn column in table.Columns)
			   {
					Debug.Log(row[column]);
			   }
			 }
		}	
		
		
		
		 sql.UpdateInto("momo",new string[]{"name","qq"},new string[]{"'ruoruo'","'11111111'"}, "email", "'xuanyusong@gmail.com'"  );
		
		 sql.Delete("momo",new string[]{"id","email"}, new string[]{"1","'000@gmail.com'"}  );
		 sql.Close();
		}catch(Exception e)
		{
			Error = e.Message;
		}
		
		
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		
		if(Error != null)
		{
			GUILayout.Label(Error);
		}
		
	}
}

