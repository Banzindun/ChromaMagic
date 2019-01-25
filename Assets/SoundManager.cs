using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //public AudioClip[] ColoredClips;

    public static SoundManager Instance;

    public AudioClip Fanfare;

    public AudioClip LostHealth;

    public AudioClip Lost;

    private AudioSource audioSource;

    private void OnEnable()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        /*Fanfare = Resources.Load<AudioClip>("fanfare");
        LostHealth = Resources.Load<AudioClip>("lostHealth");
        Lost = Resources.Load<AudioClip>("lost");*/

        audioSource = GetComponent<AudioSource>();
    }
	
	public void PlaySound(string name)
    {
        switch (name) {
            case "fanfare":
                audioSource.PlayOneShot(Fanfare);
                break;
            case "lostHealth":
                audioSource.PlayOneShot(LostHealth);
                break;
            case "lost":
                audioSource.PlayOneShot(Lost);
                break;
        }
    }
}
