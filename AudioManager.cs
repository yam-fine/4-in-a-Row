using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioClip blueDiskAudio;
    [SerializeField] private AudioClip redDiskAudio;
    [SerializeField] private AudioClip Victory;
    [SerializeField] private AudioClip Tie;
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private AudioSource audioSource;

    private static AudioManager instance;
    public static AudioManager Instance {
        get {
            if (!instance)
                instance = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
            return instance;
        }
    }

    public void OnButtonClick() {
        audioSource.clip = buttonPress;
        audioSource.Play();
    }

    public void OnWin() {
        audioSource.clip = Victory;
        audioSource.Play();
    }

    public void OnDraw() {
        audioSource.clip = Tie;
        audioSource.Play();
    }

    public void OnBlueDisk() {
        audioSource.clip = blueDiskAudio;
        audioSource.Play();
    }

    public void OnRedDisk() {
        audioSource.clip = redDiskAudio;
        audioSource.Play();
    }
}