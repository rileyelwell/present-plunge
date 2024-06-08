using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    [SerializeField] private AudioClip anvilHitSoundClip, placingStackSoundClip, buttonClickSoundClip, slidingPresentsSoundClip;

    private AudioSource[] _audioSources;


    private void Awake() {
        if (instance == null)
            instance = this;

        _audioSources = gameObject.GetComponents<AudioSource>();
    }

    public void PlayAnvilSound() {
        _audioSources[1].clip = anvilHitSoundClip;
        _audioSources[1].volume = 0.1f;
        _audioSources[1].Play();
    }

    public void PlayStackPlacingSound() {
        _audioSources[1].clip = placingStackSoundClip;
        _audioSources[1].volume = 0.1f;
        _audioSources[1].Play();
    }

    public void PlayButtonClick() {
        _audioSources[1].clip = buttonClickSoundClip;
        _audioSources[1].volume = 0.1f;
        _audioSources[1].Play();
    }

    public void PlaySlidingPresents() {
        _audioSources[1].clip = slidingPresentsSoundClip;
        _audioSources[1].volume = 0.1f;
        // _audioSources[1].loop = true;
        _audioSources[1].Play();
    }

    
    
}
