using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Cube : MonoBehaviour {

    public float minX; public float maxX;
    public float minZ; public float maxZ;

    public float nX; public float nY; public float nZ;

    // Use this for initialization
    void Start () {
        MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();

        minX = rend.bounds.min.x; maxX = rend.bounds.max.x;
        minZ = rend.bounds.min.z; maxZ = rend.bounds.max.z;

        nY = gameObject.transform.position.y + 5;
    }
	
	// Update is called once per frame
	void Update () {

        nX = Random.Range(minX, maxX);
        nZ = Random.Range(minZ, maxZ);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject cub = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cub.transform.position = new Vector3(nX, nY, nZ);
            //добавление к объекту cub компоненты Rigidbody с гравитацией
            cub.AddComponent<Rigidbody>();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Quaternion quat1 = Quaternion.AngleAxis(10, new Vector3(0, 0, 1));
            gameObject.transform.rotation *= quat1;
        }
    }
}
