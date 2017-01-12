using UnityEngine;

/// <summary>
/// This class represents a player. Is responsible for instantiating a prefab object and the game's IO.
/// </summary>
[RequireComponent(typeof(VRCamera))]
[RequireComponent(typeof(RemoteInputManager))]
[RequireComponent(typeof(PlayerController))]
class Player
{
    private IClient clientConnection;

    private PlayerController playerController;
    private RemoteOutputManager remoteOutputManager;
	private GameObject cvr;

	public Player(IClient connection, GameObject g)
    {
        clientConnection = connection;
		cvr = g;

        // instantiate a the prefab
        //GameObject playerObject = (GameObject) Object.Instantiate(Resources.Load("VRCharacter"));
		//GameObject playerObject = (GameObject) this.
		GameObject playerObject = cvr;
		//Debug.Log (playerObject.name);

        remoteOutputManager = new RemoteOutputManager(playerObject.GetComponent<VRCamera>(), clientConnection);

        // player controller
        playerController = playerObject.GetComponent<PlayerController>();
        playerController.init(playerObject, new RemoteInputManager(clientConnection));
    }

    internal void Update()
    {
        remoteOutputManager.Update();
    }

    internal void Finish()
    {
        remoteOutputManager.finish();
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
