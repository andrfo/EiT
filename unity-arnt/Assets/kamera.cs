using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3(GameObject.Find("Cube").transform.position.x,3,GameObject.Find("Cube").transform.position.z-8);
		//transform.rotate = new Vector3(0,0,-20);
	}
}
