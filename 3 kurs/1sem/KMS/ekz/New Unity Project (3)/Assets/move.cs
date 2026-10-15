using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	Transform bashnya;
	Transform stvol;

	float current_angle = 0;
	// Use this for initialization
	void Start () {

		bashnya = gameObject.transform.Find("bashnya");
		stvol = bashnya.transform.Find("stvol");
	}
	
	// Update is called once per frame
	void Update () {

        float z = Input.GetAxis("Vertical");

        transform.Translate(0,0,z);

        float y = Input.GetAxis("Horizontal");

		transform.Rotate(0,y,0);

		float by = Input.GetAxis("Mouse X");
		bashnya.transform.Rotate(0,by,0);

		float sx = Input.GetAxis("Mouse Y");
		
		current_angle += sx;

		float new_angle = Mathf.Clamp(-current_angle, -20, 30);

		stvol.localEulerAngles = new Vector3(new_angle, 0, 0);
    }
}
