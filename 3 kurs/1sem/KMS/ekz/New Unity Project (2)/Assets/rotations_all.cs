using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotations_all : MonoBehaviour {

	public Quaternion rot;

	public float angle;
    // Use this for initialization
    void Start () {
		
		rot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
		angle += 1;
        //transform.Rotate(1,1,1);

        //transform.eulerAngles = new Vector3(40,0,0);

        //Quaternion new_angle = Quaternion.AngleAxis(angle, Vector3.right);

        //transform.rotation = rot*new_angle;


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Rotate(v, h, 0);
    }
}
