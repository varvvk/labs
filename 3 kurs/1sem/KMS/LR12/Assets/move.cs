using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class move : MonoBehaviour {

    Transform bashnya;	
	Transform stvol; 
	float TankMoveSpeed = 0.8f;
	float RotateSpeed = 1f; 
	float min = 260f;
	float max = 320f;

    // Use this for initialization
    void Start () {
        bashnya = gameObject.transform.Find("bashnya");
        stvol = bashnya.transform.Find("stvol");
    }

    // Update is called once per frame
    void Update () {
        float z = Input.GetAxis("Vertical");     // W / S
        transform.Translate(0, 0, z * TankMoveSpeed);

        float x = Input.GetAxis("Horizontal");   // A / D
        transform.Rotate(0f, x * RotateSpeed, 0f);

        float h = Input.GetAxis("Mouse X");
        bashnya.Rotate(0f, h * RotateSpeed, 0f);      // поворот башни

        float v = Input.GetAxis("Mouse Y");
        // ось Z направлена горизонтально
        //stv.transform.Rotate(0, 0, v);           // полное вращение ствола

        // вращение ствола с ограничениями: 
        stvol.transform.Rotate(0f, 0f, v * RotateSpeed);
        float new_angle = Mathf.Clamp(stvol.transform.localEulerAngles.z, min, max);
        stvol.localEulerAngles = new Vector3(0f, 0f, new_angle);

    }
}
