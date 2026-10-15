using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot_stvol : MonoBehaviour {

    public GameObject core;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 forwardofstvol = transform.position +
            transform.TransformDirection(Vector3.up * 4);
            GameObject newcore = Instantiate(core, forwardofstvol, transform.rotation);
        }

    }
}
