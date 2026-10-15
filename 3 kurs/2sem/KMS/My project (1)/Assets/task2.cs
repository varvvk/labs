using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task2 : MonoBehaviour {

	public Transform targetPos;
	int maxdist = 5;
	int mindist = 1;

	bool Distance(float dist)
	{
		if (dist > mindist && dist < maxdist)
		{
			return true;
		}
		return false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 newpos = transform.position + transform.TransformDirection(new Vector3(x, 0, z));
		if (Distance(Vector3.Distance(newpos, targetPos.position)))
		{
            transform.position = newpos;
        }
    }
}
