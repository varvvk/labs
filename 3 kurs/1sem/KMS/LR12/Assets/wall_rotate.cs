using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall_rotate : MonoBehaviour {

    public GameObject Wall;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "corpus")
        { Wall.transform.Rotate(1, 0, 0); }
    }
}
