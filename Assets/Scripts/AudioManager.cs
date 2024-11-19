using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip matchSound;
    public AudioClip unmatchSound;
    public AudioClip gmaeoverSound;

    [Header("Audio Source")]
    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton Pattern to ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Get or create an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayMatchSound()
    {
        audioSource.PlayOneShot(matchSound);
    }
    // Added Sounds
    public void PlayUnmatchSound()
    {
        audioSource.PlayOneShot(unmatchSound);
    }

    public void PlayGameOverSOund()
    {
        audioSource.PlayOneShot(gmaeoverSound);
    }
}
