/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


public class make_path{
	public GameObject path_object;

	private GameObject cur_arrow;
	private GameObject prev_arrow;

	public GameObject[] arrows;
	public int current_x_pos = 50;
	public int current_y_pos = 20;

	public int i = 0;


	public GameObject[] generate_path(){






		Transform transformed_arrow = path_object.transform;

		//read path from file
		string[] lines = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "coordinates.txt"));

		arrows = new GameObject[lines.Length];

		for (int i = 0; i < lines.Length; i++) {
			GameObject.Instantiate (transformed_arrow, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		}
		foreach (string line in lines)
		{
			string[] tokens = line.Split(' ');
			//Debug.Log (tokens[1] +"\n");
			//cur_arrow = Instantiate(transformed_arrow, new Vector3(Int32.Parse(tokens[0]) - current_x_pos, 0, Int32.Parse(tokens[1]) - current_y_pos), Quaternion.identity);
			// cur_arrow.localScale = new Vector3 (.8f, .8f, .8f);


			arrows[i] = path_object;
			arrows[i].transform.position = new Vector3(Int32.Parse(tokens[0]) - current_x_pos, 0, Int32.Parse(tokens[1]) - current_y_pos);
			arrows[i].transform.localScale = new Vector3 (.8f, .8f, .8f);

			if (i != 0) {
				arrows [i - 1].transform.LookAt (arrows[i].transform);
				arrows [i - 1].transform.Rotate (arrows [i].transform.up * 90);
				arrows[i-1].transform.Rotate(arrows[i].transform.right * 90);
			}
			i++;


			
			cur_arrow = path_object.transform;
			cur_arrow.position = new Vector3(Int32.Parse(tokens[0]) - current_x_pos, 0, Int32.Parse(tokens[1]) - current_y_pos);
			//Debug.Log (cur_arrow.transform.position);
			cur_arrow.localScale = new Vector3 (.8f, .8f, .8f);

			if (prev_arrow != null) {
				prev_arrow.LookAt (cur_arrow.transform);
				prev_arrow.Rotate (Vector3.up * 90);
				prev_arrow.Rotate (Vector3.right * 90);
			} else {
				prev_arrow = cur_arrow;
			}
			arrows[i] = prev_arrow;
			prev_arrow = cur_arrow;
			i++;
 


		}

		foreach (GameObject arrow in arrows){
			//Instantiate(arrow);
			Debug.Log(arrow.transform.position);
		}


		return arrows;
	}

	public GameObject[] getArrows(){
		return arrows;

	}
}

 


public class move_camera : MonoBehaviour {
	//public Transform make_path;
	public GameObject path_object;
	public GameObject[] arrows;

	private float i = 0f;
	private float time_interval = 0.001f;

	private Vector3 start;
	private Vector3 end;
	private Vector3 rotateValue;
	// private path_plotter Path_Obj;
	//private float x;
	//private float y;
	// Use this for initialization


	private make_path path;

	void Start () {
		path = new make_path();
		path.path_object = path_object;
		arrows = path.generate_path ();

		foreach (GameObject arrow in arrows){
			//Instantiate(arrow);
			//Debug.Log(arrow.transform.position);
		}
		
		GameObject path_obj = GameObject.Find("make_path");
		path_plotter path_script = path_obj.GetComponent<path_plotter>();
		path_script.getArrow();
		 

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
*


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