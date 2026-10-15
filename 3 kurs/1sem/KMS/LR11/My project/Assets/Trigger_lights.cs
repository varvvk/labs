using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_lights : MonoBehaviour {

    public GameObject Cylinder;
    public Light Point1;
    public Light Point2;
    public Light Point3;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col)
    {
        if (col.name == "player")
        {
            Cylinder.transform.Rotate(0, 5, 0);
            if (Point1.intensity < 10) Point1.intensity += 0.001f;
            else Point1.intensity = 0;
            if (Point2.intensity < 10) Point2.intensity += 0.001f;
            else Point2.intensity = 0;
            if (Point3.intensity < 10) Point3.intensity += 0.001f;
            else Point3.intensity = 0;
        }
    }
}
