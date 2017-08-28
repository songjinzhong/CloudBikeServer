using UnityEngine;

/// <summary>
/// Class responsible for handling the output.
/// Sets the resolution of the <see cref="VRCamera"/> based on the client resolution. And sends the rendered image every time <see cref="Update"/> is called.
/// </summary>
class RemoteOutputManager
{
    private VRCamera vrCamera;
    private IClient client;
	//绘制持续时间(秒)
	public float ClearTime = 1000;
	float nowTime = 0;

    public RemoteOutputManager(VRCamera vrCamera, IClient client)
    {
        this.vrCamera = vrCamera;
        this.client = client;

        // get client screen resolution
        int[] screenResolution = client.readScreenResolution();
        Debug.Log("Client screen resolution: " + screenResolution[0] + " x " + screenResolution[1]);
        // set vrCamera resolution
        vrCamera.textureWidth = screenResolution[0];
        vrCamera.textureHeight = screenResolution[1];
    }

    public void Update(string speed)
    {
        sendFrame(vrCamera.GetImage());

		if (nowTime < ClearTime) {
			nowTime += Time.deltaTime * 1000;
		} else {
			nowTime = 0;
			byte[] speedData = getSpeed (speed);
			sendSpeed (speedData);
		}
    }

    private void sendFrame(byte[] bytes)
    {
        client.sendImage(bytes);
    }

	private void sendSpeed(byte[] bytes){
		client.sendSpeed (bytes);
	}

	private byte[] getSpeed(string speed){
		string data = "|" + speed + "|1200|30|20";
		byte[] speedData = System.Text.Encoding.Default.GetBytes (data);
		return speedData;
	}

    internal void finish()
    {
        //vrCamera.Destroy();
    }
}
