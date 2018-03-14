using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


public class path_plotter : MonoBehaviour {

	// Use this for initialization

	public GameObject path_object;
	private Transform temp_obj;
	private Transform prev_obj;

	public int current_x = 50;
	public int current_y = 20;

	public int i = 0;
	Vector3 temp = new Vector3(0,0,2);

	void Start () {
		Transform Transformed = path_object.transform;


		string[] lines = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "coordinates.txt"));

		foreach (string line in lines)
		{
			string[] tokens = line.Split(' ');
			//Debug.Log (tokens[1] +"\n");
			temp_obj = Instantiate(Transformed, new Vector3(Int32.Parse(tokens[0]) - current_x, 0, Int32.Parse(tokens[1]) - current_y), Quaternion.identity);
			temp_obj.localScale = new Vector3 (.8f, .8f, .8f);

			if (prev_obj != null) {
				prev_obj.LookAt (temp_obj);
				prev_obj.transform.Rotate (Vector3.up * 90);
				prev_obj.transform.Rotate(Vector3.right * 90);
				// rotate the previous arrow
			}

			prev_obj = temp_obj;


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
