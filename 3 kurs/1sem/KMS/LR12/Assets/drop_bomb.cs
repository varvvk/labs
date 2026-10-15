using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop_bomb : MonoBehaviour {

    public GameObject prefub1;

    public float spawnHeight = 15f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float zoneWidth = transform.localScale.x * 2f;
            float zoneLength = transform.localScale.z * 2f;

            // центр зоны — впереди танка
            Vector3 center = transform.position - transform.forward * 50f;

            float x = Random.Range(-zoneWidth / 2, zoneWidth / 2);
            float z = Random.Range(-zoneLength / 2, zoneLength / 2);

            Vector3 spawnPos = new Vector3(
                center.x + x,
                spawnHeight,
                center.z + z
            );

            Instantiate(prefub1, spawnPos, Quaternion.identity);
        }
    }
}
