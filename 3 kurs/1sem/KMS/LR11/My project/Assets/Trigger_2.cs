using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_2 : MonoBehaviour {

    public Light light1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider c1)
    {
        if (c1.name == "player") light1.enabled = true;
    }

    private void OnTriggerExit(Collider c1)
    { 
        if (c1.name == "player") light1.enabled = false; 
    }
}
