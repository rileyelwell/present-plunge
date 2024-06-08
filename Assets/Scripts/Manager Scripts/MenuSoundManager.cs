using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour {
    public static MenuSoundManager instance;
    [SerializeField] private AudioClip buttonClickSoundClip;
    private AudioSource[] _audioSources;


    private void Awake() {
        if (instance == null)
            instance = this;

        _audioSources = gameObject.GetComponents<AudioSource>();
    }

    public void PlayButtonClick() {
        _audioSources[1].clip = buttonClickSoundClip;
        _audioSources[1].volume = 0.1f;
        _audioSources[1].Play();
    }
}
