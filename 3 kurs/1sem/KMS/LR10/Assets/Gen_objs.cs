using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen_objs : MonoBehaviour {
    public float minX; public float maxX;
    public float minZ; public float maxZ;

    public float nX; public float nY; public float nZ;

    public float sX; public float sZ;

    public GameObject prefub1;

    // Use this for initialization
    void Start()
    {
        MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();

        minX = rend.bounds.min.x; maxX = rend.bounds.max.x;
        minZ = rend.bounds.min.z; maxZ = rend.bounds.max.z;

        nY = gameObject.transform.position.y + 5;
    }

    // Update is called once per frame
    void Update()
    {
        nX = Random.Range(minX, maxX);
        nZ = Random.Range(minZ, maxZ);

        sX = Random.Range(minX, maxX);
        sZ = Random.Range(minZ, maxZ);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject cub = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cub.transform.position = new Vector3(nX, nY, nZ);
            cub.AddComponent<Rigidbody>();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Quaternion quat1 = Quaternion.AngleAxis(10, new Vector3(0, 0, 1));
            transform.rotation *= quat1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 position = new Vector3(sX, 5, sZ);
            Instantiate(prefub1, position, Quaternion.identity);
        }
    }
}
