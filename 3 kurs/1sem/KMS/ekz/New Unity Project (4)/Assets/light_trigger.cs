using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_trigger : MonoBehaviour {

    public Light light;
    public GameObject cube;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cube")
        {
            light.enabled = true;

            StartCoroutine(ChangeColor());
            
        }
    }

    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(1);
        cube.GetComponent<Renderer>().material.color = Color.red;
    }
}
