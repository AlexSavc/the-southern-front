using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioClip selectionSound;

    public AudioClip DeselectionSound;
    public AudioClip SOVIET;

    public AudioSource source;
    void Awake()
    {
        _instance = this;
    }

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

    public void PlaySovietAnthem()
    {
        source.PlayOneShot(SOVIET);
    }
}
