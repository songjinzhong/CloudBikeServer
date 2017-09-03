using System.Threading;
using UnityEngine;

/// <summary>
/// This class reads the input from the <see cref="IClient"/>
/// Is considered as the only access point to this data.
/// </summary>
public class PeopleInputManager
{
	// the source of remote input data
	private IClient clientConnection;

	private float heartRate = 0f;
	private float oxygen = 0f;

	public PeopleInputManager(){

	}

	public PeopleInputManager(IClient socket)
	{
		init(socket);
	}

	public void init(IClient socket)
	{
		clientConnection = socket;

		Thread t = new Thread(new ThreadStart(read));
		t.Start();
	}

	/// <summary>
	/// Reads the last remote input received from the client.
	/// This method is called periodically by the worker thread and runs until the client is disconnected.
	/// </summary>
	void read()
	{
		g_Input input;

		while((input = clientConnection.readPeopleInput()) != null)
		{
			if (input is PeopleInput) {
				handBikeInput ((PeopleInput)input);
			}
		}
	}

	// handles bikeinput
	private void handBikeInput(PeopleInput input){
		heartRate = input.HeartRate;
		oxygen = input.Oxygen;
	}
	public float HeartRate
	{
		get{
			return heartRate;
		}
		set{
			if (value == null)
				heartRate = 0f;
			heartRate = value;
		}
	}
	public float Oxygen
	{
		get{
			return oxygen;
		}
		set{
			if (value == null)
				oxygen = 0f;
			oxygen = value;
		}
	}
}