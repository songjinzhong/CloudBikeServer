using UnityEngine;
using System.Collections;

public class CollidersToContinue : MonoBehaviour {

	// Use this for initialization
	private float t;
	private MyCarUserControl cuc;
	void Start(){
		cuc = this.GetComponent<MyCarUserControl> ();
	}

	void OnCollisionEnter(Collision collisionInfo){
		t = Time.time;
	}
	void OnCollisionStay(Collision collisionInfo){
		if(Time.time-t>=3){
			transform.Rotate (Vector3.up*cuc.horizontal*60f, Space.World);
		}
	}
}
