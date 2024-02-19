// using System.Collections;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using Npgsql;
using UnityEngine;

/// <summary>
/// Communicates with a PostgreSQL database to store user metrics. Currently not in use.
/// </summary>
public class DatabaseScript : MonoBehaviour
{
// 	//Todo - create database and connect it here
// 	//Commented out to remove warnings in Unity
// 	//private string UserID = "";
// 	//private string Password = "";

// 	public MetricsTracker metricsTracker;

// 	private NpgsqlConnection connection;

// 	private const string CONNECTION_STRING = "Host=localhost:5432;" +
//     "Username=postgres;" +
//     "Password=Hotpockets22!;" +
//     "Database=MSS";
	
// 	// Start is called before the first frame update
// 	void Start()
// 	{
// 		if (metricsTracker == null)
// 		{
// 			Debug.Log ("MetricsTracker is null on the Database script.\n Include direct link to improve performance.");
// 			metricsTracker = GameObject.Find("MetricsTracker").GetComponent<MetricsTracker>();
// 		}
// 		ConnectToDatabase();
// 	}

// 	// Update is called once per frame
// 	void Update()
// 	{
			
// 	}

// 	void ConnectToDatabase() 
// 	{
// 		connection = new NpgsqlConnection(CONNECTION_STRING);
// 		connection.Open();
// 		Add();
// 	}

// 	private void Add()
// 	{
// 		string commandText = "INSERT INTO users (username) VALUES ('Justin')";
// 		var cmd = new NpgsqlCommand(commandText, connection);
// 		Debug.Log(cmd);
// 		cmd.ExecuteNonQuery();
// 	}

// 	// In the future, call this when the user succesfully ties the knot!
// 	void OnDestroy()
// 	{
// 		string commandText = "INSERT INTO users (username, duration) VALUES ('test1'," + metricsTracker.timer + ")";
// 		var cmd = new NpgsqlCommand(commandText, connection);
// 		Debug.Log(cmd);
// 		cmd.ExecuteNonQuery();
// 	}
}
