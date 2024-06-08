using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour {
    private float _speed, _presentHitSpeed = 5f, _rotationSpeed = 180;

    [SerializeField] private float _minSpeed = 4.5f, _maxSpeed = 8.5f;

    private float _endPosition = -6f;

    private bool _hasCollided = false;

    private void Start() {
        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    private void Update() {
        FallDown();

        // deactivate after leaving screen
        if (transform.position.y < _endPosition) {
            gameObject.SetActive(false);
        }
    }

    private void FallDown() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    public void ResetAnvil() {
        _hasCollided = false;
    }

    private void ScatterPresents(GameObject hitPresent) {
        // if the anvil is to the left (or equal) of the hit present
        if (hitPresent.transform.position.x >= transform.position.x) {
            // make this present fly to the right
            hitPresent.transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime, Space.Self);
            hitPresent.transform.Translate(Vector3.right * _presentHitSpeed * Time.deltaTime, Space.World);
        } else {
            // make this present fly to the left
            hitPresent.transform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime, Space.Self);
            hitPresent.transform.Translate(Vector3.left * _presentHitSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // if player collides with (catches) a present
        if (_hasCollided == false && (collider.tag == TagManager.PLAYER_TAG || collider.tag == TagManager.CAUGHT_PRESENT_TAG)) {
            // print("Anvil collided with a caught present or player");

            // update collided flag to reduce redundant collisions
            _hasCollided = true;

            // play the anvil hit sound
            SoundManager.instance.PlayAnvilSound();

            // update the lives counter
            ScoreManager.instance.UpdateLifeCount();

            // play animation?

            // remove the parent hierarchy completely for presents and player
            if (collider.tag == TagManager.CAUGHT_PRESENT_TAG) {
                // detatch the children from the parent (player)
                collider.gameObject.transform.parent.DetachChildren();

                // make the presents "go flying"
                // ScatterPresents(collider.gameObject);
            } else {
                // detatch the children from the player
                collider.gameObject.transform.DetachChildren();
            }
            
            // set anvil as inactive
            gameObject.SetActive(false);

            // reset the stack of presents completely
            PresentStack.instance.ResetStack();

            // update the score (resets to 0)
            ScoreManager.instance.UpdateStackScore(PresentStack.instance.GetPresentStackCount());

            // reset stopwatch for the current stack
            PresentStack.instance.ResetStackTime();
        }
    }
    
}
