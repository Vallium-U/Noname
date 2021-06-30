using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public AudioSource clip;
    //public AudioClip[] drop;

    // Start is called before the first frame update
    void Awake()
    {

        clip = GetComponent<AudioSource>();
    }
    private void Start()
    {
        // clip.clip = drop[Random.Range(0, drop.Length)];
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        //clip.clip = drop[Random.Range(0, drop.Length)];
        if (other.relativeVelocity.magnitude >= 35)
        {
            clip.volume = Random.Range(0.25f, 0.75f);
            clip.pitch = Random.Range(0.55f, 1.1f);
            clip.PlayOneShot(clip.clip);
        }
    }
    private void OnCollisionStay(Collision other)
    {
        //clip.clip = drop[Random.Range(0, drop.Length)];
        if (other.relativeVelocity.magnitude >= 15 && !clip.isPlaying)
        {
            clip.volume = Random.Range(0.25f, 0.75f);
            clip.pitch = Random.Range(0.55f, 1.1f);
            clip.PlayOneShot(clip.clip);
        }
    }
}
