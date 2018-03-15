using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class move_camera : MonoBehaviour {
	//public Transform make_path;
	public GameObject path_object;
	public GameObject[] arrows;



	private float increment = 0;
	private int i;
	private float time_interval = 0.01f;
	private int path_length = 0;
	private int closest_path = 0;

	public int current_x_pos = 50;
	public int current_y_pos = 20;

	private Vector3 start;
	private Vector3 end;
	private Vector3 rotateValue;
	// private path_plotter Path_Obj;
	//private float x;
	//private float y;
	// Use this for initialization



	void Start () {

		generate_path ();

		InvokeRepeating("update_position", .5f, time_interval);
		//InvokeRepeating("generate_path", 1.0f, time_interval*100);

	}

	
	// Update is called once per frame
	void Update () {

		for (int i = closest_path; i < path_length; i++) {
			if( (Mathf.Abs(Camera.main.transform.position.magnitude) - Mathf.Abs(arrows[i].transform.position.magnitude)) < 0.2f){
				set_arrow_status (0, i, false);
				set_arrow_status (i+1, i+6, true);
				closest_path = i;
				break;
			}
				
		}

	}

	void update_position(){
		increment-= time_interval/200;
		//translation
		Vector3 start = Camera.main.transform.position;
		Vector3 end = arrows [closest_path].transform.position;
		Vector3 end = new Vector3(transform.position.x-increment, transform.position.y, transform.position.z);
		//Camera.main.transform.position = new Vector3(start.x + (start.x-end.x)*increment, start.y, start.z + (start.z  + (start.z-end.z)*increment));
		//TODO: NEED TO CHECK WHICH DIRECTIONS WE HAVE TO MOVE THE CAMERA, AND THEN MAKE IT MOVE AT A CONSTANT MOVING RATE. MAYBE JUST CHANGE THE VIEW DIRECTION OF THE CAMERA? THAT WOULD PROBABLY BE BEST.
		/*

		Camera.main.transform.position = new Vector3(start.x+increment, start.y, start.z);


		//rotation
		Camera.main.transform.LookAt(arrows[closest_path].transform);
		*/



	}
		
	void generate_path(){
		i = 0;

		//read path from file
		string[] lines = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "coordinates.txt"));
		path_length = lines.Length;
		//init new arrow array
		arrows = new GameObject[path_length];

		foreach (string line in lines)
		{
			
			string[] tokens = line.Split(' ');
			arrows [i] = Instantiate(path_object, new Vector3(Int32.Parse(tokens[0]) - current_x_pos, 0, Int32.Parse(tokens[1]) - current_y_pos),  Quaternion.identity) as GameObject;
			arrows[i].transform.localScale = new Vector3 (.8f, .8f, .8f);

			if (i != 0) {
				arrows [i - 1].transform.LookAt (arrows[i].transform);
				arrows [i - 1].transform.Rotate (arrows [i].transform.up * 90);
				arrows[i-1].transform.Rotate(arrows[i].transform.right * 90);
			}
			i++;
		}
			
		set_arrow_status (4, i, false);

	
	}
	


	void set_arrow_status(int from, int to, bool stat){
		for (int k = from; k < to; k++) {
			arrows [k].SetActive (stat);
		}
	}

}



/*
 * 
 * 	void update_position(){
		Vector3 start = Camera.main.transform.position;
		Vector3 end = new Vector3(transform.position.x-i, transform.position.y, transform.position.z);
		//Camera.main.transform.position = new Vector3.Lerp(start, end, time_interval);
		Camera.main.transform.position = new Vector3(start.x+i, start.y, start.z);
		i-= time_interval/100;
		x = Input.GetAxis (path_plotter);
		y = Input.GetAxis (path_plotter);

		rotateValue = new Vector3 (x, y * -1, 0);

		transform.eulerAngles = transform.eulerAngles - rotateValue;
 

*/