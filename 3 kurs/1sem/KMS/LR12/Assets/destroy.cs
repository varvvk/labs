using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    AudioSource audioSource;

    public GameObject explosion1;
    // Use this for initialization
    void Start()
	{
        audioSource = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter(Collision col)
	{
		// если коснулись поверхности (земли / плоскости)
		if (col.gameObject.name == "Terrain")
		{
            audioSource.PlayOneShot(audioSource.clip);

            Instantiate(explosion1, gameObject.transform);

            Destroy(gameObject, audioSource.clip.length);
        }
	}
}
