using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blue_light : MonoBehaviour {
    public Light Point1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider c1) //при входе в триггер
    {
        if (c1.gameObject.name == "corpus")
            Point1.intensity = 100;
    }
    private void OnTriggerExit(Collider c1) //при выходе из триггера
    {
        if (c1.gameObject.name == "corpus")
            Point1.intensity = 0;
    }
}
