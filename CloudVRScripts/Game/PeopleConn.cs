using UnityEngine;

/// <summary>
/// This class represents a player. Is responsible for instantiating a prefab object and the game's IO.
/// </summary>
[RequireComponent(typeof(VRCamera))]
[RequireComponent(typeof(RemoteInputManager))]
[RequireComponent(typeof(PlayerController))]
class PeopleConn
{
	private IClient clientConnection;

	private PlayerController playerController;

	private GameObject initG;

	public PeopleConn(IClient connection, GameObject g, SpeedOnScreen sos)
	{
		clientConnection = connection;
		initG = g;
		GameObject playerObject = initG.transform.GetChild(1).gameObject;
		playerController = playerObject.GetComponent<PlayerController>();
		playerController.init(playerObject, new PeopleInputManager(clientConnection), sos);
	}

	internal void Update()
	{
		//        remoteOutputManager.Update();
	}

	internal void Finish()
	{
		clientConnection.disconnect();
	}

	public IClient ClientConnection
	{
		get
		{
			return clientConnection;
		}
	}
}
