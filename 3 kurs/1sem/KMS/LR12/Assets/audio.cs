using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{

    AudioSource source_tank; // источник звука
    bool isPlaying = false; // пер

    // Use this for initialization
    void Start()
    {
        source_tank = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // если танк движется
        if ((x != 0 || z != 0) && !isPlaying)  // если танк двигается и звук не вкл.
        {
            isPlaying = true;
            source_tank.Play();
        }
        if (x == 0 && z == 0 && isPlaying) // если танк не двиг. и звук  вкл.
        {
            isPlaying = false;
            source_tank.Stop();
        }
    }
}
