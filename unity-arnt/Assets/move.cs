﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
	public float moveSpeed;

	// Use this for initialization
	void Start () 
	{
		moveSpeed = 1f;
		transform.position = new Vector3(39,1,28);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(moveSpeed*Input.GetAxis("Horizontal")*Time.deltaTime,0f,moveSpeed*Input.GetAxis("Vertical")*Time.deltaTime);
	}
}