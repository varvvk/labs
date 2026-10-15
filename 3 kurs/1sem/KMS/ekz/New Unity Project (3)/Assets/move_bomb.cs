using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_bomb : MonoBehaviour {

	public GameObject explosion;

	void Start () {

        Destroy(gameObject, 2f);
    }
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.TransformDirection(Vector3.forward * 1f);
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "goal")
		{
			Instantiate(explosion, gameObject.transform);
		}
	}
}
