using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAudioController : MonoBehaviour
{
    public AudioClip Sfx;

    private AudioSource _sfx;
    private bool played = false;

    void Start()
    {
        _sfx = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        _sfx.clip = Sfx;

        if (_sfx.clip == null) return;

        if (!_sfx.isPlaying)
            if (!played)
            {
                _sfx.Play();
                played = true;
            }
            else
                Destroy(gameObject);
    }
}
