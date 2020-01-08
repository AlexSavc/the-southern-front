using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip selectionSound;

    public AudioClip DeselectionSound;

    public AudioSource source;

    void Start()
    {
        if (source == null) source = GetComponent<AudioSource>();
    }

    public void PlaySelectionSound()
    {
        source.PlayOneShot(selectionSound);
    }

    public void PlayDeselectionSound()
    {
        source.PlayOneShot(DeselectionSound);
    }
}
