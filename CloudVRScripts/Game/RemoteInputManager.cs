﻿using System.Threading;
using UnityEngine;

/// <summary>
/// This class reads the input from the <see cref="IClient"/>
/// Is considered as the only access point to this data.
/// </summary>
public class RemoteInputManager
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

    public RemoteInputManager(IClient socket)
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

        while((input = clientConnection.readInput()) != null)
        {
			//Debug.Log(input);
			if (input is GyroInput) {
				//Debug.Log ("gyro");
				handleQuaternion ((GyroInput)input);
			}
			else if (input is TouchInput){
				handleTouchInput((TouchInput) input);
			}
            else if (input is ControllerInput)
            {
				//Debug.Log (input);
                if (((ControllerInput)input).Touch[0] != 0.0)
                    Debug.Log("Touch Pad: " + ((ControllerInput)input).Touch[0] + "  " + ((ControllerInput)input).Touch[1]);
                if (((ControllerInput)input).Speedup != ControllerInput.SpeedTypes.NoChange)
                    Debug.Log("Speedup: " + ((ControllerInput)input).Speedup);
                bool stateChanged = ((ControllerInput)input).Touch[0] != 0.0 || ((ControllerInput)input).Speedup != ControllerInput.SpeedTypes.NoChange;
                if(stateChanged)
                    handleControllerInput((ControllerInput)input);
            } 
        }
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
		move = input.Touch [0];
		Debug.Log (speed + "----" + move);
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
}