using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_open : MonoBehaviour {

	public GameObject Cube_1;
	public GameObject Cube_2;

    public GameObject vint;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "player")
        { 
			Cube_1.transform.Rotate(0, 110, 0);
            Cube_2.transform.Rotate(0, -110, 0);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "player")
        {
            Cube_1.transform.Rotate(0, -110, 0);
            Cube_2.transform.Rotate(0, 110, 0);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "robot")
        {
            vint.transform.Translate(0.01f, 0, 0.01f);
            vint.transform.Rotate(1, 0, 0);
        }
    }
}
