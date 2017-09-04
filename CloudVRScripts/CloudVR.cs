using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main class of the project, attach this to any game object in your scene and you're ready to go.
/// </summary>

public class CloudVR : MonoBehaviour
{
    public bool useTCP = true;

	public GameObject VRC;

	public GameObject GetVRC{get{ return VRC;}}

    private IServer server;
	private IServer server2;
	private IServer server3;
    private List<Player> players = new List<Player>();
	private List<BikeConn> bikeConns = new List<BikeConn>();
	private List<PeopleConn> peopleConns = new List<PeopleConn>();

	public GameObject gameServer;
	private SpeedOnScreen sos;

	private int id;

    void Awake ()
    {
        var initDispatcher = UnityThreadHelper.Dispatcher;

		if (useTCP) {
			server = new ServerTCP ();
			server2 = new ServerBikeTCP ();
			server3 = new ServerPeopleTcp ();
		} else {
			server = new ServerUDP ();
			server2 = new ServerBikeTCP ();
			server3 = new ServerPeopleTcp ();
		}

        server.ClientConnected += OnClientConnected;
        server2.ClientConnected += OnBikeConnected;
		server3.ClientConnected += onPeopleConnected;
		id = 0;
		DebugOnScreen.Add ("Bike Connected",bikeConns.Count);
		sos = gameServer.GetComponent<SpeedOnScreen> ();
    }

    void Update ()
    {
        players.ForEach(player => 
        {
            try {
                player.Update();
            } catch
            {
                player.Finish();
                players.Remove(player);
            }
        });
     }

    void OnApplicationQuit()
    {
        server.Disconnect();
		server2.Disconnect ();
		server3.Disconnect ();

        players.ForEach(player => player.Finish());
		bikeConns.ForEach (bikeconn => bikeconn.Finish ());
    }

    /// <summary>
    /// Callback called from the Server when a new client is connected.
    /// Creates a new Player.
    /// </summary>
    void OnClientConnected(object sender, OnClientConnectedEventArgs args) 
    {
		Debug.Log (players.Count);
		if (players.Count >= 2) {
			DebugOnScreen.Add ("two bikes aleardy!","");
			return;
		}
		players.Add(new Player(args.ClientConnection, VRC, id));
		id++;
		if (id > 3) {
            id = -3;
		}
    }

	void OnBikeConnected(object sender, OnClientConnectedEventArgs args)
	{
		try{
			bikeConns.Add (new BikeConn (args.ClientConnection, players[players.Count - 1].P_Bike, 0));
			DebugOnScreen.Add ("Bike Connected",bikeConns.Count);
		}catch{
			Debug.Log ("mobile connect first!");
			DebugOnScreen.Add ("mobile connect first!",bikeConns.Count);
		}
	}

	void onPeopleConnected(object sender, OnClientConnectedEventArgs args)
	{
		try{
			peopleConns.Add(new PeopleConn(args.ClientConnection, players[players.Count - 1].P_Bike, sos));
			DebugOnScreen.Add ("Heart-Oxygen Connected",peopleConns.Count);
		}catch{
			Debug.Log ("1234");
			DebugOnScreen.Add ("mobile connect first!","~~~~~~~");
		}
	}
}