using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


public class kamera : MonoBehaviour {

	// Use this for initialization

	public GameObject testste;

	public int current_x = 50;
	public int current_y = 20;

	public int i = 0;
	Vector3 temp = new Vector3(0,0,2);

	void Start () {
		Transform Transformed = testste.transform;

		string[] lines = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "coordinates.txt"));

		foreach (string line in lines)
		{
			
			string[] tokens = line.Split(' ');
			Debug.Log (tokens[1] +"\n");
			Instantiate(Transformed, new Vector3(Int32.Parse(tokens[0]) - current_x, 0, Int32.Parse(tokens[1]) - current_y), Quaternion.identity);
			 
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if (testobj.position.magnitude <= 3){
		//	testobj.position += temp;
		//}
		//transform.position = new Vector3(GameObject.Find("Cube").transform.position.x,3,GameObject.Find("Cube").transform.position.z-8);
		//transform.rotate = new Vector3(0,0,-20);
	}
}
