using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public AudioClip buttonFX;
    public AudioClip upgradeFX;
    public AudioClip teleportFX;
    public AudioClip tastedFX;

    private AudioSource audioSource;

    //Singleton
    private static SoundFX instance;
    public static SoundFX Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<SoundFX>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundFX(string strFX)
    {
        AudioClip clipFX = null;
        switch (strFX)
        {
            case "button":
                clipFX = buttonFX;
                break;
            case "upgrade":
                clipFX = upgradeFX;
                break;
            case "teleport":
                clipFX = teleportFX;
                break;
            case "tasted":
                clipFX = tastedFX;
                break;
            default:
                clipFX = null;
                break;
        }
        audioSource.clip = clipFX;
        audioSource.Play();
    }
}
