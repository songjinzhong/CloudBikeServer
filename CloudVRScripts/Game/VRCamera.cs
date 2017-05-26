using System;
using UnityEngine;

/// <summary>
/// Virtual reality camera. Uses two cameras, for right and left eyes.
/// </summary>
public class VRCamera : MonoBehaviour
{
    // texture on which the two cameras render
    private RenderTexture renderTexture;
    // texture used to capture the screen
    private Texture2D texture;

    // camera resolution
    public int textureWidth = 1920;

	public float dd = 0.5626f;
	public int textureHeight;

    public Camera _cameraLeft;
    public Camera _cameraRight;

    // "lenses" (cameras) regulation parameters, adjust these as you would adjust the lenses on a real VR headset
    public float Distance = 0f;
    public float Degree = 0f;

    private float leftCameraX;
    private float leftCameraDegree;
    private float rightCameraX;
    private float rightCameraDegree;

    // scale the resolution of the camera, is needed for performance reasons. Rendering at 1080p is often too heavy, causing fps problems.
    public int imageScaleFactor = 1;
	private GUIStyle bb=new GUIStyle();  

	//横向滑动条数值  
	private int horizontalValue = 1920;

	private int clear = 0;

	//控制每次按下音量健变化的分辨率的宽度
	private int CHANGE = 20;

	//接受最大和最小的图片分辨率
	private int MAX_image = 3000;
	private int MIN_image = 500;

    void Start()
    {
		textureHeight = (int)(textureWidth * dd);
        renderTexture = new RenderTexture(textureWidth/imageScaleFactor, textureHeight/imageScaleFactor, 16);
        texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // both cameras will render on this texture
        _cameraLeft.targetTexture = renderTexture;
        _cameraRight.targetTexture = renderTexture;
		//bb.normal.background = null; //这是设置背景填充的  
		bb.fontSize = 50;  
		//SendGzip.test ();
		//SendGzip.t();
    }

    /// <summary>
    /// Captures the image in the currently active render texture and stores it in a Texture2D. Then encodes the Texture2D data as JPG.
    /// This method is super performance intensive and is definitely not a good solution.
    /// </summary>
    internal byte[] GetImage()
    {
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

		byte[] bytes = texture.EncodeToJPG(30);
		//Debug.Log ("------" + bytes.Length);
		//byte[] b = SendGzip.compress (bytes);
		//Debug.Log ("------" + b.Length);
		//return b;
        return bytes;
    }

    internal void Destroy()
    {
        Destroy(gameObject);
        RenderTexture.active = null;
    }
	//private string txt = "1920";
    void Update()
    {
		//Debug.Log (clear);
		if (clear != 0) {
			horizontalValue += clear * CHANGE;
			if (horizontalValue < MIN_image) {
				horizontalValue = MIN_image;
			} else if (horizontalValue > MAX_image) {
				horizontalValue = MAX_image;
			}
			clear = 0;
		}
		if (horizontalValue >= MIN_image && horizontalValue<=MAX_image && horizontalValue != textureWidth) {
			textureWidth = horizontalValue ;
			textureHeight = (int)(textureWidth * dd);
			UpdateImages ();
		}
        AdjustCameras();

    }
	void UpdateImages(){
		//Debug.Log (horizontalValue);
		renderTexture = new RenderTexture(textureWidth/imageScaleFactor, textureHeight/imageScaleFactor, 16);
		texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// both cameras will render on this texture
		_cameraLeft.targetTexture = renderTexture;
		_cameraRight.targetTexture = renderTexture;
		//renderTexture = new RenderTexture(textureWidth/imageScaleFactor, textureHeight/imageScaleFactor, 16);
	}
		
	void OnGUI()
	{
		//txt = GUI.TextField(new Rect(0, 0, 120, 60),txt,bb);
		//计算滑动进度  
		horizontalValue = (int)GUI.HorizontalSlider( new Rect (10, 10, 200, 20), horizontalValue, 500, 3000);  
	}

    private void AdjustCameras()
    {
        leftCameraX = -Distance / 2;
        rightCameraX = Distance / 2;
        leftCameraDegree = Degree / 2;
        rightCameraDegree = -Degree / 2;
        const double tolerance = 0.000000000000000001;
        // Adjust rotations
        if (Math.Abs(leftCameraDegree - _cameraLeft.transform.localRotation.y) > tolerance)
            _cameraLeft.transform.localRotation = new Quaternion(_cameraLeft.transform.localRotation.x, leftCameraDegree, _cameraLeft.transform.localRotation.z, _cameraLeft.transform.localRotation.w);
        if (Math.Abs(rightCameraDegree - _cameraRight.transform.localRotation.y) > tolerance)
            _cameraRight.transform.localRotation = new Quaternion(_cameraRight.transform.localRotation.x, rightCameraDegree, _cameraRight.transform.localRotation.z, _cameraRight.transform.localRotation.w);
        // Adjust x positions of cameras
        if (Math.Abs(leftCameraX - _cameraLeft.transform.localPosition.x) > tolerance)
            _cameraLeft.transform.localPosition = new Vector3(leftCameraX, _cameraLeft.transform.localPosition.y, _cameraLeft.transform.localPosition.z);
        if (Math.Abs(rightCameraX - _cameraRight.transform.localPosition.x) > tolerance)
            _cameraRight.transform.localPosition = new Vector3(rightCameraX, _cameraRight.transform.localPosition.y, _cameraRight.transform.localPosition.z);
    }

	// set and get
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