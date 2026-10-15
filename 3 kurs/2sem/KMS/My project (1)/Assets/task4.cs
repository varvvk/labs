using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task4 : MonoBehaviour {

	public GameObject obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ChangeCol()
	{
		obj.GetComponent<Renderer>().material.color = Color.red;
	}

    public void UnChangeCol()
    {
        obj.GetComponent<Renderer>().material.color = Color.white;
    }

}
