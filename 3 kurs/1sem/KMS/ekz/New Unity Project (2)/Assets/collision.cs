using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour {

	public GameObject sphere;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		transform.Translate(x*0.1f, 0, y*0.1f);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Sphere")
		{
			GetComponent<Renderer>().material.color = Color.blue;
		}
	}
}
