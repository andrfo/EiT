using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class move_camera : MonoBehaviour {

	public GameObject path_object;
	public GameObject[] arrows;

	public int current_x_pos = 50;
	public int current_y_pos = 20;


	private int path_length = 0;
	public int closest_path = 0;
	private int lookahead_constant = 5;
	private float distance_threshold = 0.5f;


	private float increment = 0;
	private float time_interval = 0.01f;
	private float closest_dist = Mathf.Infinity;
	private float current_dist;

	private Vector3 start;
	private Vector3 end;
	private Vector3 rotateValue;


	void Start () {

		generate_path ();

		InvokeRepeating("update_position", .5f, time_interval);
		//InvokeRepeating("generate_path", 1.0f, time_interval*100);

	}
		
	void Update () {
		
		for (int i = closest_path; i < path_length; i++) 
		{
			current_dist = vector_distance(Camera.main.gameObject, arrows[i]);

			if (current_dist < distance_threshold)
			{
				if (i + 1 <= path_length) 
				{
					closest_path = i+1;
				}

			}
		}
		set_arrow_status (0, closest_path, false);
		set_arrow_status (closest_path+1, closest_path+lookahead_constant+1, true);
		set_arrow_status (closest_path+lookahead_constant+2, path_length, false);
	}

	void update_position(){
		increment = time_interval * 2;
		//translation
		Vector3 start = Camera.main.transform.position;
		Vector3 end = arrows [closest_path].transform.position;

		//Vector3 end = new Vector3(transform.position.x-increment, transform.position.y, transform.position.z);
		//Camera.main.transform.position = new Vector3(start.x-increment, start.y, start.z);
		Camera.main.transform.position += Camera.main.transform.forward * 40 * increment * Time.deltaTime;
		rotate_camera ();

		//Camera.main.transform.LookAt(arrows[closest_path].transform);
		//Debug.Log (increment);
		//Camera.main.transform.position = new Vector3(start.x + (start.x-end.x)*increment, start.y, start.z + (start.z  + (start.z-end.z)*increment));
		//TODO: NEED TO CHECK WHICH DIRECTIONS WE HAVE TO MOVE THE CAMERA, AND THEN MAKE IT MOVE AT A CONSTANT MOVING RATE. MAYBE JUST CHANGE THE VIEW DIRECTION OF THE CAMERA? THAT WOULD PROBABLY BE BEST.
		/*

		Camera.main.transform.position = new Vector3(start.x+increment, start.y, start.z);


		//rotation
		Camera.main.transform.LookAt(arrows[closest_path].transform);
		*/



	}
		
	void generate_path(){
		int i = 0;

		//read path from file
		string[] lines = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "coordinates.txt"));
		path_length = lines.Length;

		//init new arrow array
		arrows = new GameObject[path_length];

		foreach (string line in lines)
		{
			
			string[] tokens = line.Split(' ');
			arrows [i] = Instantiate(path_object, new Vector3(Int32.Parse(tokens[0]) - current_x_pos, 0.1f, Int32.Parse(tokens[1]) - current_y_pos),  Quaternion.identity) as GameObject;
			arrows[i].transform.localScale = new Vector3 (.8f, .8f, .8f);

			if (i != 0) {
				arrows [i - 1].transform.LookAt (arrows[i].transform);
				arrows [i - 1].transform.Rotate (arrows [i].transform.up * 90);
				arrows[i-1].transform.Rotate(arrows[i].transform.right * 90);
				//TODO: Set last arrow to something telling us we have reached the exit
			}
			i++;
		}
			
		set_arrow_status (4, i, false);

	
	}
	


	void set_arrow_status(int from, int to, bool stat){
		for (int k = from; k < to; k++) {
			if (k <= path_length - 1) {
				arrows [k].SetActive (stat);
			}

		}
	}

	float vector_distance(GameObject Gameobj1, GameObject Gameobj2){
		Vector3 position1 = new Vector3 (Gameobj1.transform.position.x, Gameobj1.transform.position.y, Gameobj1.transform.position.z);
		Vector3 position2 = new Vector3 (Gameobj2.transform.position.x, Gameobj2.transform.position.y, Gameobj2.transform.position.z);
		position1.y = 0;
		position2.y = 0;

		//Debug.Log (Mathf.Abs (Vector3.Distance (position1, position2)));
		return Mathf.Abs (Vector3.Distance (position1, position2));

	}

	void rotate_camera(){
		Vector3 arrow_in_camera_height = new Vector3 (arrows [closest_path].transform.position.x, Camera.main.transform.position.y, arrows [closest_path].transform.position.z);

		var targetRotation = Quaternion.LookRotation(arrow_in_camera_height - Camera.main.transform.position);
		Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);

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