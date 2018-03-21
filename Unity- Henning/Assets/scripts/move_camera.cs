using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class move_camera : MonoBehaviour {

	//SET START POSITION
	public float current_x_pos = 50f;
	public float current_z_pos = 20f;

	public GameObject path_object;
	public GameObject[] arrows;


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


	void Start () 
	{
		//generate_path ();
		generate_path_old ();
		InvokeRepeating("update_position", .5f, time_interval);
	}
		
	void Update () 
	{
		for (int i = closest_path; i < path_length - 1; i++) 
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
		set_arrow_status (0, closest_path+1, false);
		set_arrow_status (closest_path+2, closest_path+lookahead_constant+2, true);
		set_arrow_status (closest_path+lookahead_constant+3, path_length, false);
	}

	void update_position()
	{
		current_x_pos = Camera.main.transform.position.x;
		current_z_pos = Camera.main.transform.position.z;
		

		//this value seems to work fine
		increment = time_interval * 2;

		//translation
		Vector3 start = Camera.main.transform.position;
		Vector3 end = arrows [closest_path].transform.position;
		Camera.main.transform.position += Camera.main.transform.forward * 40 * increment * Time.deltaTime;

		//rotation
		rotate_camera ();
	}
		
	void generate_path_old()
	{
		int i = 0;

		//read path from file
		string[] lines = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "coordinates.txt"));
		path_length = lines.Length;

		//init new arrow array
		arrows = new GameObject[path_length];

		foreach (string line in lines)
		{
			string[] tokens = line.Split(' ');
			arrows [i] = Instantiate(path_object, new Vector3(Int32.Parse(tokens[0]) - current_x_pos, 0.2f, Int32.Parse(tokens[1]) - current_z_pos),  Quaternion.identity) as GameObject;
			arrows[i].transform.localScale = new Vector3 (.8f, .8f, .8f);

			if (i != 0) 
			{
				arrows [i - 1].transform.LookAt (arrows[i].transform);
				arrows [i - 1].transform.Rotate (arrows [i].transform.up * 90);
				arrows[i-1].transform.Rotate(arrows[i].transform.right * 90);
				//TODO: Set last arrow to something telling us we have reached the exit
			}
			i++;
		}
	}
	/*	
	void generate_path()
	{
		int i = 0;
		List<List<int>> path = best_first_search (current_x_pos, current_z_pos);
		//read path from file
		path_length = path.Count;

		//init new arrow array
		arrows = new GameObject[path_length];


		foreach (List<int> position in path)
		{
			arrows [i] = Instantiate(path_object, new Vector3(position[0] - current_x_pos, 0.1f, position[1] - current_z_pos),  Quaternion.identity) as GameObject;
			arrows[i].transform.localScale = new Vector3 (.8f, .8f, .8f);

			if (i != 0) 
			{
				arrows [i - 1].transform.LookAt (arrows[i].transform);
				arrows [i - 1].transform.Rotate (arrows [i].transform.up * 90);
				arrows [i - 1].transform.Rotate(arrows[i].transform.right * 90);
				//TODO: Set last arrow to something telling us we have reached the exit
			}
			i++;
		}
	}

*/

	void set_arrow_status(int from, int to, bool stat)
	{
		for (int k = from; k < to; k++) 
		{
			if (k <= path_length - 1) 
			{
				arrows [k].SetActive (stat);
			}
		}
	}

	float vector_distance(GameObject Gameobj1, GameObject Gameobj2)
	{
		Vector3 position1 = new Vector3 (Gameobj1.transform.position.x, Gameobj1.transform.position.y, Gameobj1.transform.position.z);
		Vector3 position2 = new Vector3 (Gameobj2.transform.position.x, Gameobj2.transform.position.y, Gameobj2.transform.position.z);
		position1.y = 0;
		position2.y = 0;
		return Mathf.Abs (Vector3.Distance (position1, position2));
	}

	void rotate_camera()
	{
		//TODO: Tune to get a smoother look-at-function. Maybe look between the current and next arrow?
		Vector3 arrow_in_camera_height = new Vector3 (arrows [closest_path].transform.position.x, Camera.main.transform.position.y, arrows [closest_path].transform.position.z);

		var targetRotation = Quaternion.LookRotation(arrow_in_camera_height - Camera.main.transform.position);
		Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);

	}

}
	