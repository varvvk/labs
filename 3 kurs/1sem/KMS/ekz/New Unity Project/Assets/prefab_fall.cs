using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefab_fall : MonoBehaviour {

    public GameObject prefab;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 position = new Vector3(0, 5, -5);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
