using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFireSound : MonoBehaviour
{
    public AudioSource fireSound;

    // Start is called before the first frame update
    void Awake()
    {
        fireSound.pitch = Random.Range(0.75f, 1.4f);
        fireSound.Play(); //play sound effect
        Destroy(gameObject, 0.8f);
    }

}
