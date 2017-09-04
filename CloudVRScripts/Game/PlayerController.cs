using UnityEngine;
using System;

/// <summary>
/// Class used to update the rotation and position of the player.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RemoteInputManager))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_StickToGroundForce;
    [SerializeField]
    private float m_GravityMultiplier;

    private Vector3 m_MoveDir = Vector3.zero;
    private Quaternion targetInitialRotation;

    private GameObject target;

    private CharacterController m_CharacterController;

    public RemoteInputManager inputManager;

    public BikeInputManager inputM = new BikeInputManager();

	public PeopleInputManager peopleInputM;

	private GameObject yuanshi;

	private Vector3 v_y;

	private Vector3 compare;

	private MyCarUserControl mcc;

	private VRCamera vrc;

    private bool flag = false;

	private bool peopleFlag = false;

	private SpeedOnScreen sos;

	// save into db
	private float speed = 0f;
	private float distance = 0f;
	private float heartRate = 0f;
	private float oxygen = 0f;

	// db sql
	private SqlAccess sql = new SqlAccess();

	private float nowTime = 0f;
	private float insertTime = 5000f;

    private void Start()
    {
		yuanshi = this.transform.parent.gameObject;
        m_CharacterController = GetComponent<CharacterController>();
		//y = yuanshi.transform.rotation.y;
		v_y = yuanshi.transform.rotation.eulerAngles;
		mcc = yuanshi.GetComponent<MyCarUserControl> ();
    }

    /// <summary>
    /// Call this method when creating the <see cref="Player"/>
    /// </summary>
    public void init(GameObject target, RemoteInputManager inputManager)
    {
        this.target = target;
        this.inputManager = inputManager;
		this.vrc = target.GetComponent<VRCamera> ();
        targetInitialRotation = target.transform.rotation;
    }

	public void init(GameObject target, BikeInputManager inputManager){
		//this.target = target;
		this.inputM = inputManager;
		//this.vrc = target.GetComponent<VRCamera> ();
		this.flag = true;

		//targetInitialRotation = target.transform.rotation;
	}

	public void init(GameObject target, PeopleInputManager inputManager, SpeedOnScreen sos){
		this.peopleInputM = inputManager;
		this.peopleFlag = true;
		this.sos = sos;
		this.sos.ShowFlag = true;
	}

    private void FixedUpdate()
    {
        if (target == null)
            return;
			updateRotation ();
			setImages ();
		if(flag == true){
			updateControl();
		}
		if (peopleFlag == true) {
			updatePeople ();
		}

		if (peopleFlag || flag) {
			updateSpeedOnScreen ();
		}
//		if (peopleFlag && flag) {
		if (true) {
			if (nowTime < insertTime) {
				nowTime += Time.deltaTime * 1000;
			} else {
				nowTime = 0f;
				insertIntoDB ();
			}
		}
    }

	private void setImages(){
		//Debug.Log (inputManager.c);
		if(inputManager.c != 0){
			vrc.c = vrc.c + inputManager.c;
			inputManager.c = 0;
		}
	}

	private void updateControl(){
        mcc.c_v = inputM.v;
		speed = inputM.v * 10 + inputM.v2;
        mcc.c_h = inputM.h;
	}

	private void updatePeople(){
//		Debug.Log (peopleInputM.HeartRate);
		heartRate = peopleInputM.HeartRate;
//		Debug.Log (peopleInputM.Oxygen);
		oxygen = peopleInputM.Oxygen;
	}

	private void updateSpeedOnScreen(){
		sos.Speed = speed;
		distance += speed * Time.deltaTime;
		sos.Distance = distance;
//		Debug.Log (heartRate);
		sos.HeartRate = heartRate;
//		Debug.Log (oxygen);
		sos.Oxygen = oxygen;
	}

	private void insertIntoDB(){
//		float[] values = new float[]{  };
		double f = convertDateTime();
		Debug.Log(Convert.ToString(f));
	}

	// if client exit
	public void finish(){
        Debug.Log ("finish");
		mcc.c_v = -1f;
		mcc.c_h = 0f;
	}

    /// <summary>
    /// Updates the rotation of the target
    /// </summary>
    private void updateRotation()
    {
		target.transform.rotation = targetInitialRotation * inputManager.Rotation;
		compare = yuanshi.transform.rotation.eulerAngles;
		target.transform.Rotate (compare - v_y);
		//Debug.Log (compare - v_y);
		//Debug.Log (yuanshi.transform.rotation.eulerAngles);
		//target.transform.rotation = target.transform.rotation + yuanshi.transform.rotation;
    }

    /// <summary>
    /// Updates the position of the target
    /// </summary>
    private void updatePosition()
    {
        float speed;
        GetInput(out speed);

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo, m_CharacterController.height / 2f, ~0, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;

        m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
    }

    private void GetInput(out float speed)
    {
        if (inputManager.TouchDown)
            speed = m_WalkSpeed;
        else
            speed = 0;
    }

	// 转时间戳
	private double convertDateTime(){
		System.DateTime s = System.TimeZone.CurrentTimeZone.ToLocalTime (new System.DateTime (1970, 1, 1));
		System.DateTime n = System.DateTime.Now;
		double ret = (double)(n - s).TotalSeconds * 1000;
		return ret;
	}

	public string Speed {
		get{
			return speed.ToString();
		}
	}
}
