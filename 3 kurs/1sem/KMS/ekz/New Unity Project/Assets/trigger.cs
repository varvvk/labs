using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour {

	public GameObject cube;
	public Texture texture1;
	public Quaternion rot1;
	public float angle;
	// Use this for initialization
	void Start () {
		rot1 = cube.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.name == "Cube_wasd")
		{
			cube.GetComponent<Renderer>().material.color = Color.red;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.name == "Cube_wasd")
		{
			cube.GetComponent<Renderer>().material.mainTexture = texture1;

            angle += 1;
            Quaternion new_rot = Quaternion.AngleAxis(angle, new Vector3(1, 0, 0));
			cube.transform.rotation = new_rot * rot1;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if( other.name == "Cube_wasd")
		{
			cube.GetComponent<Renderer>().material.mainTexture = null;
		}
	}

}
