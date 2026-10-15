using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class anim_door : MonoBehaviour

{

    public Animator doorAnimator;

    public string openParameter = "Open";



    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("Player"))

        {

            doorAnimator.SetBool(openParameter, true);

        }

    }



    private void OnTriggerExit(Collider other)

    {

        if (other.CompareTag("Player"))

        {

            doorAnimator.SetBool(openParameter, false);

        }

    }

}

