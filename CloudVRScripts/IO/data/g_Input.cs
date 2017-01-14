using System;

/// <summary>
/// Every game's input type must implement this interface.
/// </summary>
// should be done with generics and not be an empty interface .. but at the moment c# is driving me crazy :|
public interface g_Input
{
}

/// <summary>
/// Gyroscope input. The rotation is represented as a quaternion, stored in a float array.
/// </summary>
public class GyroInput : g_Input
{
    private float[] data;

    public float[] Data
    {
        get
        {
            return data;
        }

        set
        {
            if (value == null)
                throw new NullReferenceException("value == null");

            data = value;
        }
    }
}

/// <summary>
/// Touch input. Can be of two types: touch up or touch down.
/// </summary>
public class TouchInput : g_Input
{
    public enum TouchTypes : int { Down = 0, Up = 1};

    private TouchTypes data;

    public TouchTypes Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }
}

/// <summary>
/// Controller input.
/// </summary>
public class ControllerInput : g_Input
{
    private float[] touch;       //正在触摸遥控器触控板的位置(x, y坐标)

    public enum SpeedTypes : int { Down = -1, NoChange=0, Up = 1};
    private SpeedTypes speedup;

    public float[] Touch
    {
        get
        {
            return touch;
        }

        set
        {
            if (value == null)
                throw new NullReferenceException("value == null");
            touch = value;
        }
    }

    public SpeedTypes Speedup
    {
        get
        {
            return speedup;
        }

        set
        {
            speedup = value;
        }
    }
}