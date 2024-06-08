using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElf : MonoBehaviour {

    private Vector2 _backgroundElfSpawnPosition = new Vector2(-10, -3);
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _speed = 3f;
    public bool isAwake = false;


    private void Awake() {
        _rb = GetComponent<Rigidbody2D> ();
        _animator = GetComponent<Animator>();
    }
    
    private void Update() {
        if (isAwake) {
            // SoundManager.instance.PlaySlidingPresents();
            _animator.SetBool(TagManager.WALKING_ANIMATION_FLAG, true);
            _rb.velocity = new Vector2(_speed, 0f);
        }

        // if the background elf has moved across the screen, reset and turn off
        if (isAwake && transform.position.x > 9f) {
            isAwake = false;
            gameObject.SetActive(false);
            transform.position = _backgroundElfSpawnPosition;
        }
    }

}
