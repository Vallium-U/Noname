using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;

    [Range(0.0f,1.0f)]
    public float volume;
    [Range(0.1f,3.0f)]
    public float pitch;

    public bool playOnAwake;
    public bool loop;
    [Range(0.0f,1.0f)]
    public float spatialBlend;

    public float minDistance;
    public float maxDistance;
    
    [Range(0f, 1f)]
    public float volumeVariance = .1f;
    
    [Range(0f, 1f)]
    public float pitchVariance = .1f;
    
    public AudioMixerGroup mixerGroup;
    
    [HideInInspector]
    public AudioSource source;
}
