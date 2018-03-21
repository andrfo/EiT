using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {


	// Use this for initialization
	void Start () {
		for(int i = 0; i < 10; i++)
        {
            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
