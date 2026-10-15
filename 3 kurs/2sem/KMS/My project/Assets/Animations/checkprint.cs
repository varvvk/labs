using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkprint : MonoBehaviour
{
    public Animator animator;

    [Header("Sound")]
    public AudioClip printSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Запуск анимации печати чека. Вызывается из PaymentManager
    // при успешном завершении каждого задания.
    public void Print()
    {
        if (animator != null)
            animator.SetTrigger("hit");

        if (audioSource != null && printSound != null)
            audioSource.PlayOneShot(printSound);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Print();
        }
    }
}
