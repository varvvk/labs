using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot_core : MonoBehaviour {

    public float speed = 120f;

    public float lifeTime = 8f;

    public GameObject explosion1;
    // Use this for initialization
    void Start () {

        Destroy(gameObject, lifeTime);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {

            // 11. скрыть снаряд
            GetComponent<Renderer>().enabled = false;

            // 13. создать эффект взрыва в точке столкновения
            Instantiate(explosion1, gameObject.transform);

        }
    }
}
