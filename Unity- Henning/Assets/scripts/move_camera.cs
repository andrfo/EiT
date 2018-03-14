using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_camera : MonoBehaviour {

	private float i = 0f;
	private float time_interval = 0.001f;

	private Vector3 start;
	private Vector3 end;

	// Use this for initialization
	void Start () {
		
		InvokeRepeating("update_position", 1.0f, time_interval);
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	void update_position(){
		Vector3 start = Camera.main.transform.position;
		Vector3 end = new Vector3(transform.position.x-i, transform.position.y, transform.position.z);
		//Camera.main.transform.position = new Vector3.Lerp(start, end, time_interval);
		Camera.main.transform.position = new Vector3(start.x+i, start.y, start.z);
		i-= time_interval/100;
	}
}
