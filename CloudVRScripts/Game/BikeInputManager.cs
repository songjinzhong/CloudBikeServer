using System.Threading;
using UnityEngine;

/// <summary>
/// This class reads the input from the <see cref="IClient"/>
/// Is considered as the only access point to this data.
/// </summary>
public class BikeInputManager
{
    // the source of remote input data
    private IClient clientConnection;

    // gyro
    private float[] gyroQuaternion;
    private float[] gyroInitialRotation = null;

    // touch
    private bool touchDown = false;

	//controler
	private float move = 0f;
	private float speed = 0f;
	private int clear = 0;

	public BikeInputManager(){

	}

	public BikeInputManager(IClient socket)
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

        while((input = clientConnection.readInput3()) != null)
        {
			//Debug.Log(input);
//			if (input is GyroInput) {
//				//Debug.Log ("gyro");
//				handleQuaternion ((GyroInput)input);
//			}
//			else if (input is TouchInput){
//				handleTouchInput((TouchInput) input);
//			}
//            else if (input is SpeedInput) {
//                Debug.Log("Speed: " + ((SpeedInput)input).Speed);
//                handleSpeedInput((SpeedInput)input);
//            }
//            else if(input is ResolutionInput) {
//                //if(((ResolutionInput)input).Resolution!=ResolutionInput.ResolutionTypes.NoChange)
//                    //Debug.Log("Resolution: " + ((ResolutionInput)input).Resolution);
//                handleResolutionInput((ResolutionInput)input);
//            }
//            else if(input is TurnInput) {
//                //if (((TurnInput)input).Turn != 0.0)
//                    //Debug.Log("Touch Pad: " + ((TurnInput)input).Turn + "  " + ((TurnInput)input).Turn);
//                handleTurnInput((TurnInput)input);
//            }
//
//            else if (input is ControllerInput)
//            {
//                if (((ControllerInput)input).Touch != 0.0)
//                    Debug.Log("Touch Pad: " + ((ControllerInput)input).Touch + "  " + ((ControllerInput)input).Touch);
//                //if (((ControllerInput)input).Speedup != ControllerInput.SpeedTypes.NoChange)
//                    Debug.Log("Speedup: " + ((ControllerInput)input).Speedup);
//                if(((ControllerInput)input).Clear!=ControllerInput.ClearTypes.NoChange)
//                    Debug.Log("Clear: " + ((ControllerInput)input).Clear);
//                handleControllerInput((ControllerInput)input);
//            } 
			if (input is BikeInput) {
				handBikeInput ((BikeInput)input);
			}
        }
    }

	// handles bikeinput
	private void handBikeInput(BikeInput input){
//		switch (input.Speed) 
//		{
//		case BikeInput.SpeedTypes.Up:
//			speed = 1f;
//			break;
//		case BikeInput.SpeedTypes.Down:
//			speed = -1f;
//			break;
//		case BikeInput.SpeedTypes.NoChange:
//			speed = 0f;
//			break;
//		}
		speed = input.Speed;
		if (input.Turn < -1) {
			move = -2f;
		} else {
			move = Mathf.Clamp (input.Turn, -1, 1);
		}
		//Debug.Log (move);

	}

    /// <summary>
	/// Handles the remote speed command input.
	/// </summary>
	private void handleSpeedInput(SpeedInput input)
	{
		switch (input.Speed) 
		{
			case SpeedInput.SpeedTypes.Up:
				speed = 1f;
				break;
			case SpeedInput.SpeedTypes.Down:
				speed = -1f;
				break;
			case SpeedInput.SpeedTypes.NoChange:
				speed = 0f;
				break;
		}
	}

    /// <summary>
	/// Handles the remote resolution input.
	/// </summary>
	private void handleResolutionInput(ResolutionInput input)
	{
		if (input.Resolution == ResolutionInput.ResolutionTypes.Incr) {
			clear++;
		} else if (input.Resolution == ResolutionInput.ResolutionTypes.Desc) {
			clear--;
		}
	}

    /// <summary>
	/// Handles the remote turn input.
	/// </summary>
	private void handleTurnInput(TurnInput input)
	{
		move = input.Turn;
	}

	/// <summary>
	/// Handles the remote Controller input.
	/// </summary>
	private void handleControllerInput(ControllerInput input)
	{
		switch (input.Speedup) 
		{
			case ControllerInput.SpeedTypes.Up:
				speed = 1f;
				break;
			case ControllerInput.SpeedTypes.Down:
				speed = -1f;
				break;
			case ControllerInput.SpeedTypes.NoChange:
				speed = 0f;
				break;
		}
		move = input.Touch;
		//Debug.Log ((int)input.Clear);
		if (input.Clear == ControllerInput.ClearTypes.Incr) {
			clear++;
		} else if (input.Clear == ControllerInput.ClearTypes.Desc) {
			clear--;
		}
	}

    /// <summary>
    /// Handles the remote touch input.
    /// </summary>
    private void handleTouchInput(TouchInput input)
    {
        switch (input.Data)
        {
            case TouchInput.TouchTypes.Down:
                touchDown = true;
                break;
            case TouchInput.TouchTypes.Up:
                touchDown = false;
                break;
        }
    }

    /// <summary>
    /// Handles the remote rotation input.
    /// </summary>
    private void handleQuaternion(GyroInput input)
    {
        gyroQuaternion = input.Data;

        // save the initial rotation
        if (gyroInitialRotation == null)
            gyroInitialRotation = gyroQuaternion;
    }

    /// <summary>
    /// Returns the last rotation from the client. This value should be multiplied by the initial rotation of the target and the result used as the new rotation of the target.
    /// </summary>
    public Quaternion Rotation
    {
        get
        {
            if (gyroInitialRotation != null)
            {
                Quaternion offsetRotation =
                    Quaternion.Inverse(new Quaternion(gyroInitialRotation[0], gyroInitialRotation[1], gyroInitialRotation[2], gyroInitialRotation[3]))
                    * new Quaternion(gyroQuaternion[0], gyroQuaternion[1], gyroQuaternion[2], gyroQuaternion[3]);

                return offsetRotation;
            }
            else
            {
                return Quaternion.identity;
            }
        }
    }

    /// <summary>
    /// Return true if the display of the client is being touched (touch down == touch and hold).
    /// </summary>
    public bool TouchDown
    {
        get
        {
            return touchDown;
        }
    }

	public float v
	{
		get{
			return speed;
		}
	}

	public float h
	{
		get{
			return move;
		}
	}
	public int c
	{
		get{
			return clear;
		}
		set{
			if (value == null)
				clear = 0;
			clear = value;
		}
	}
}