using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    float speed = 0.08f;

    public GameObject Cube1;

    public Texture textur1;
    public Texture textur2;
    public Texture textur3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float dX = Input.GetAxis("Horizontal");
        float dZ = Input.GetAxis("Vertical");
        transform.Translate(dX * speed, 0, dZ * speed);

        float dYm = Input.GetAxis("Mouse X");
        transform.Rotate(0, dYm, 0);

        if (Input.GetKeyDown(KeyCode.Q))
            Cube1.GetComponent<Renderer>().material.mainTexture = textur3;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Cube1")
        { 
            col.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            col.gameObject.GetComponent<Renderer>().material.mainTexture = textur1;
        }

        if (col.gameObject.name == "Cube2")
        { 
            col.gameObject.GetComponent<Renderer>().material.color = new Color(0, 1, 1);
            col.gameObject.GetComponent<Renderer>().material.mainTexture = textur2;
        }
    }
}