using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot_from_stvol : MonoBehaviour {

	public GameObject bomb;

    //public Transform firePoint;

    void Start () {
		
	}
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
            Vector3 forwardstvol = transform.position + transform.TransformDirection(Vector3.forward*4f);
            Instantiate(bomb, forwardstvol, transform.rotation);

            //Instantiate(bomb, firePoint.position, firePoint.rotation);
        }
	}
}
