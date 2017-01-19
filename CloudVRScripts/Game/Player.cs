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
	private GameObject initG;

	public Player(IClient connection, GameObject g, int id)
    {
        clientConnection = connection;
		cvr = g;

        // instantiate a the prefab
        //GameObject playerObject = (GameObject) Object.Instantiate(Resources.Load("VRCharacter"));
		//GameObject playerObject = (GameObject) this.
		initG = (GameObject)Object.Instantiate(cvr);
		Vector3 v3 = initG.transform.position;
		initG.transform.position = new Vector3 (v3.x + id * 3, v3.y, v3.z);
		initG.SetActive (true);
		GameObject playerObject = initG.transform.GetChild(1).gameObject;
		//Debug.Log (playerObject.transform.position);

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
		//playerController.finish ();
        clientConnection.disconnect();
		Object.Destroy (initG);
    }

    public IClient ClientConnection
    {
        get
        {
            return clientConnection;
        }
    }
}
