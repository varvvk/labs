using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_cubes : MonoBehaviour {

	MeshRenderer rend;

	float minX;
	float minZ;
	float maxX;
	float maxZ;

	public GameObject sphere;
	// Use this for initialization
	void Start () {
		
		rend = GetComponent<MeshRenderer>();

		minX = rend.bounds.min.x;
		minZ = rend.bounds.min.z;
		maxX = rend.bounds.max.x;
		maxZ = rend.bounds.max.z;
	}
	
	// Update is called once per frame
	void Update () {

		float x = Random.Range(minX, maxX);
		float z = Random.Range(minZ, maxZ);

		if (Input.GetKeyDown(KeyCode.F))
		{
			sphere.AddComponent<Rigidbody>();
			Instantiate(sphere, new Vector3(x, 5, z), Quaternion.identity);
		}
		
	}
}
