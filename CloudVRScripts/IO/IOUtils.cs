using System;
using System.Net;
using UnityEngine;

class IOUtils
{
    internal static g_Input handleInput(byte[] input)
    {
        switch (input[0])
        {
            case 0:
                return readQuaternion(input);
            case 1:
                return readTouch(input);
            case 2:
                return readSpeed(input);
            case 3:
                return readResolution(input);
            case 4:
                return readTurn(input);
            case 5:
                return readControllerCommand(input);
            case 48:
                return readSpeed (input);
            default:
                throw new ArgumentException("unknown input type");
        }
    }

    /// <summary>
    /// Convert a byte array representing a quaternion into a <see cref="GyroInput"/>.
    /// The order of the quaternion numbers is changed, because the Android's orientation is not compatible with Unity.
    /// </summary>
    private static GyroInput readQuaternion(byte[] quaternion)
    {
        byte[] temp = new byte[4];

        Array.Copy(quaternion, 1, temp, 0, 4);
        float x = NetworkToHostOrderFloat(temp);

        Array.Copy(quaternion, 5, temp, 0, 4);
        float y = NetworkToHostOrderFloat(temp);

        Array.Copy(quaternion, 9, temp, 0, 4);
        float z = NetworkToHostOrderFloat(temp);

        Array.Copy(quaternion, 13, temp, 0, 4);
        float w = NetworkToHostOrderFloat(temp);

        GyroInput input = new GyroInput();
        input.Data = new float[] { -y, x, -z, w };
        return input;
    }

    private static TouchInput readTouch(byte[] input)
    {
        int type = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(input, 1));

        TouchInput touchInput = new TouchInput();
        switch (type)
        {
            case (int)TouchInput.TouchTypes.Down:
                touchInput.Data = TouchInput.TouchTypes.Down;
                break;
            case (int)TouchInput.TouchTypes.Up:
                touchInput.Data = TouchInput.TouchTypes.Up;
                break;
            default:
                throw new ArgumentException("unknown touch type");

        }
        return touchInput;
    }

	private static BikeInput readSpeed(byte[] commands)
    {   
		//Debug.Log("speed turn");
		//Debug.Log(commands[1]);
		//Debug.Log (commands [2]);
		BikeInput input = new BikeInput();

        //byte[] temp = new byte[4];

        //Array.Copy(commands, 1, temp, 0, 4);
        //float s = NetworkToHostOrderFloat(temp);//加减速
		float s = commands[1] - 48;
		//Debug.Log(commands[1]);
        //加减速
        if(s==1)
        {
			//Debug.Log ("1");
			input.Speed = BikeInput.SpeedTypes.Up;
		}else if(s==-1 || s == 0){
			//Debug.Log ("0");
			input.Speed = BikeInput.SpeedTypes.Down;
        }else{
			input.Speed = BikeInput.SpeedTypes.NoChange;
        }

		float x = commands[2] - 48;
		//Debug.Log (x);
		//转向
		input.Turn = (2 * x - 10)/10;

        return input;
    }

    private static ResolutionInput readResolution(byte[] commands)
    {   
        ResolutionInput input=new ResolutionInput();

        byte[] temp = new byte[4];

        Array.Copy(commands, 1, temp, 0, 4);
        float c = NetworkToHostOrderFloat(temp);//清晰度

        //清晰度
        if (c == 1)
        {
            input.Resolution = ResolutionInput.ResolutionTypes.Incr;
        }
        else if (c == -1)
        {
            input.Resolution = ResolutionInput.ResolutionTypes.Desc;
        }
        else
        {
            input.Resolution = ResolutionInput.ResolutionTypes.NoChange;
        }

        return input;
    }

    private static TurnInput readTurn(byte[] commands)
    {   
		Debug.Log ("turn");
		Debug.Log (commands [1]);
        TurnInput input=new TurnInput();

        //byte[] temp = new byte[4];

        //Array.Copy(commands, 1, temp, 0, 4);
        //float x = NetworkToHostOrderFloat(temp);
		float x = commands[1];
		//Debug.Log (x);
        //转向
		input.Turn = (2 * x - 10)/10;
        
        return input;
    }

    /// <summary>
    /// Convert a byte array representing a controller command into a <see cref="ControllerInput"/>.
    /// </summary>
    private static ControllerInput readControllerCommand(byte[] commands){
        ControllerInput input=new ControllerInput();

        byte[] temp = new byte[4];

        Array.Copy(commands, 1, temp, 0, 4);
        float x = NetworkToHostOrderFloat(temp);

        Array.Copy(commands, 5, temp, 0, 4);
        float s = NetworkToHostOrderFloat(temp);//加减速

        Array.Copy(commands, 9, temp, 0, 4);
        float c = NetworkToHostOrderFloat(temp);//清晰度

        //转向
        input.Touch = 2 * x - 1;
        //加减速
        if(s==1)
        {
            input.Speedup = ControllerInput.SpeedTypes.Up;
        }else if(s==-1){
            input.Speedup = ControllerInput.SpeedTypes.Down;
        }else{
            input.Speedup = ControllerInput.SpeedTypes.NoChange;
        }
        //清晰度
        if (c == 1)
        {
            input.Clear = ControllerInput.ClearTypes.Incr;
        }
        else if (c == -1)
        {
            input.Clear = ControllerInput.ClearTypes.Desc;
        }
        else
        {
            input.Clear = ControllerInput.ClearTypes.NoChange;
        }
        return input;
    }


    private static float NetworkToHostOrderFloat(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToSingle(bytes, 0);
    }

    public static int NetworkToHostOrderInt(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToInt32(bytes, 0);
    }
}
