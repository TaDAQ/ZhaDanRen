using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip boom,fire;
    private AudioSource source;
    public static AudioController Instance;
    private void Awake()
    {
      Instance = this;
      source = GetComponent<AudioSource>(); 
    }
    public void PlayBoom()
    {
        source.clip = boom;
        source.Play();
    }

    public void PlayFire(){
        source.clip = fire;
        source.Play();
    }
}
