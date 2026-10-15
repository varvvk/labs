using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawer : MonoBehaviour
{
    public Animator animator;
    private bool isOpen;

    [Header("Sound")]
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isOpen)
            {
                animator.SetTrigger("close");
                PlaySound(closeSound);
            }
            else
            {
                animator.SetTrigger("open");
                PlaySound(openSound);
            }

            isOpen = !isOpen;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}
