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

public class SpeedInput : g_Input
{
    public enum SpeedTypes : int { Down = -1, NoChange=0, Up = 1};
    private SpeedTypes speed;     //加减速

    public SpeedTypes Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
}

public class ResolutionInput : g_Input
{
    public enum ResolutionTypes : int { Incr=1, NoChange=0, Desc=-1 };
    private ResolutionTypes resolution;       //清晰度 1：增加Incr；-1：减小Desc；不变NoChange:0

    public ResolutionTypes Resolution
    {
        get
        {
            return resolution;
        }

        set
        {
            if (value == null)
                throw new NullReferenceException("value == null");
            resolution = value;
        }
    }
}

public class TurnInput : g_Input
{
    private float turn;            //左右转向：向左<0, 向右>0，不变=0
    
    public float Turn
    {
        get
        {
            return turn;
        }

        set
        {
            if (value == null)
                throw new NullReferenceException("value == null");
            turn = value;
        }
    }
}

/// <summary>
/// Controller input.
/// </summary>
public class ControllerInput : g_Input
{
    private float touch;            //左右转向：向左<0, 向右>0，不变=0

    public enum SpeedTypes : int { Down = -1, NoChange=0, Up = 1};
    private SpeedTypes speedup;     //加减速

    public enum ClearTypes : int { Incr=1, NoChange=0, Desc=-1 };
    private ClearTypes clear;       //清晰度 1：增加Incr；-1：减小Desc；不变NoChange:0

    public float Touch
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

    public ClearTypes Clear
    {
        get
        {
            return clear;
        }

        set
        {
            if (value == null)
                throw new NullReferenceException("value == null");
            clear = value;
        }
    }
}

// bike input
public class BikeInput : g_Input
{
//    public enum SpeedTypes : int { Down = -1, NoChange=0, Up = 1};
//    private SpeedTypes speed;     //加减速
	private float speed;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
	// speed2 for second speed flag
	private float speed2;
	public float Speed2{
		get{
			return speed2;
		}
		set{
			speed2 = value;
		}
	}

    private float turn;            //左右转向：向左<0, 向右>0，不变=0

    public float Turn
    {
        get
        {
            return turn;
        }

        set
        {
            if (value == null)
                throw new NullReferenceException("value == null");
            turn = value;
        }
    }
}

public class PeopleInput : g_Input
{
	private float heartRate;
	private float oxygen;
	public float HeartRate
	{
		get {
			return heartRate;
		}
		set {
			heartRate = value;
		}
	}
	public float Oxygen
	{
		get {
			return oxygen;
		}
		set {
			oxygen = value;
		}
	}
}