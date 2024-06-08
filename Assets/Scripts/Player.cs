using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField] private GameObject _backgroundElfPrefab;
    private GameObject backgroundElf;
    private Vector2 _backgroundElfSpawnPosition = new Vector2(-10, -3);

    private Rigidbody2D _rb;
    private Animator _animator;

    private float minX = -8f, maxX = 8f;
    private float _speed, _baseSpeed = 6f;

    [SerializeField]
    private float _dashPower = 5f, _dashTime = 0.1f, _dashCooldown = 1f;

    private bool _facingRight = true, _canMove = true, _isDashing = false, _canDash = true;
    private int _maxLevelPresents = 20, _totalPlacedPresents = 0;

    [SerializeField] private TrailRenderer _trailRenderer;
    

    private void Awake() {
        _rb = GetComponent<Rigidbody2D> ();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        backgroundElf = Instantiate(_backgroundElfPrefab, _backgroundElfSpawnPosition, Quaternion.identity);
        backgroundElf.SetActive(false);
        _speed = _baseSpeed;
    }

    private void Update() {
        // prevent all other movement when dashing
        if (_isDashing)
            return;

        // get horizontal input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float movementX = horizontalInput * _speed;

        // apply normal horizontal movement
        if (_canMove)
            Move(movementX);

        // activate dash upon key press if player can dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
            StartCoroutine(Dash());

        // place presents down upon key press
        if (Input.GetKeyDown(KeyCode.E))
            PlacePresents();

        // if the number of presents placed are the over/equal the max
        if (_totalPlacedPresents >= _maxLevelPresents) {
            // have the background elf walk across the screen
            backgroundElf.SetActive(true);
            backgroundElf.GetComponent<BackgroundElf>().isAwake = true;
            _totalPlacedPresents = 0;
        }
    }

    private void Move(float movementX) {
        // apply the movement input to the rigidbody
        if (movementX != 0) {
            // play walking animation
            _animator.SetBool(TagManager.WALKING_ANIMATION_FLAG, true);

            _rb.velocity = new Vector2(movementX, 0f);

            if (movementX > 0 && transform.localScale.x < 0) {
                FlipSprite();
                _facingRight = true;
            }
            else if (movementX < 0 && transform.localScale.x > 0) {
                FlipSprite();
                _facingRight = false;
            }
            
        } else {
            // stop all player movement
            _rb.velocity = new Vector2(0f, 0f);

            // set the param to end the walking animation
            _animator.SetBool(TagManager.WALKING_ANIMATION_FLAG, false);
        }
        
        // check for bounds and clamp th player's position
        Vector3 currentPos = transform.position;
        if (currentPos.x >= maxX) {
            _rb.position = new Vector2(maxX, _rb.position.y);
        } 
        else if (currentPos.x <= minX) {
            _rb.position = new Vector2(minX, _rb.position.y);
        }

    }

    private void FlipSprite() {
        // switch the toggle flag for direction the player is facing
        _facingRight = !_facingRight;

        // multiply the player's x local scale by -1
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
    }

    private void PlacePresents() {
        int presentsPlaced = PresentStack.instance.GetPresentStackCount();
        if (presentsPlaced > 0) {
            // place the background presents
            PresentStack.instance.PlaceBackgroundPresents();

            // play the placing presents sound clip
            SoundManager.instance.PlayStackPlacingSound();

            // save the score before resetting the stack so it can be used to update the score
            // int presentsPlaced = PresentStack.instance.GetPresentStackCount();
            _totalPlacedPresents += presentsPlaced;

            // detatch all children (presents) from player
            transform.DetachChildren();

            // reset the stack of presents completely
            PresentStack.instance.ResetStack();

            // update the total score value for the player (after resetting stack because of the stopwatch)
            ScoreManager.instance.UpdateTotalScore(presentsPlaced);

            // update the stack score back down to 0 (as presents have been placed)
            ScoreManager.instance.UpdateStackScore(0);

            // reset stopwatch for the current stack
            PresentStack.instance.ResetStackTime();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == TagManager.ANVIL_TAG) {
            // print("Player has been hit!");

            // stop movement and shooting
            _rb.velocity = new Vector2(0f, 0f);
            _animator.SetBool(TagManager.WALKING_ANIMATION_FLAG, false);
            _canMove = false;

            // turn off collider so no damage can be taken
            GetComponent<CapsuleCollider2D>().enabled = false;

            // change sprite opacity (50% transparent)
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);

            // allow movement again after set time
            StartCoroutine(noDamageAfterHit());
        }
        
    }

    IEnumerator noDamageAfterHit() {
        // wait one second
        yield return new WaitForSeconds(1f);

        // allow movement
        _canMove = true;

        // return sprite to original opacity
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        // turn off collider so no damage can be taken
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    IEnumerator Dash() {
        // set the bools
        _isDashing = true;
        _canDash = false;

        _rb.velocity = new Vector2(transform.localScale.x * _dashPower, 0f);
        _trailRenderer.emitting = true;

        yield return new WaitForSeconds(_dashTime);

        _trailRenderer.emitting = false;
        _isDashing = false;

        yield return new WaitForSeconds(_dashCooldown);

        _canDash = true;
    }
}
