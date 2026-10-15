using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_wasd : MonoBehaviour {

	float speed = 0.1f;
	public GameObject cube;
	public GameObject sphere;

	public AudioSource audioSource;
	bool isPlaying = false;
	// Use this for initialization
	void Start () {
		
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		cube.transform.Translate(x*speed, y*speed, 0);

		if(x==0 && y==0 && isPlaying)
		{
			isPlaying = false;
			audioSource.Stop();
		}

        if ((x != 0 || y != 0) && !isPlaying)
        {
            isPlaying = true;
            audioSource.Play();
        }

		if (Input.GetKeyDown(KeyCode.E))
		{
			sphere.GetComponent<AudioSource>().PlayOneShot(sphere.GetComponent<AudioSource>().clip);
		}
    }
}
