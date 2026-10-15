using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_3 : MonoBehaviour {

    public Light L2;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "player")
        { 
            L2.transform.Rotate(0, 10, 0); 
        }
    }
}
