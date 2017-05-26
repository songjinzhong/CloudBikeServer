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
    private List<Player> players = new List<Player>();
	private List<BikeConn> bikeConns = new List<BikeConn>();

	private int id;

    void Awake ()
    {
        var initDispatcher = UnityThreadHelper.Dispatcher;

		if (useTCP) {
			server = new ServerTCP ();
			server2 = new ServerBikeTCP ();
		} else {
			server = new ServerUDP ();
			server2 = new ServerBikeTCP ();
		}

        server.ClientConnected += OnClientConnected;
        server2.ClientConnected += OnBikeConnected;
		id = 0;
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

        players.ForEach(player => player.Finish());
		bikeConns.ForEach (bikeconn => bikeconn.Finish ());
    }

    /// <summary>
    /// Callback called from the Server when a new client is connected.
    /// Creates a new Player.
    /// </summary>
    void OnClientConnected(object sender, OnClientConnectedEventArgs args) 
    {
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
		}catch{
			Debug.Log ("mobile connect first!");
		}
	}
}