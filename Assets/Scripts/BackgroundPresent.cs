using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPresent : MonoBehaviour {

    public bool isAwake = false;
    private float _speed = 3f;


    private void Update() {
        if (isAwake) {
            transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.World);
        }

        // if the background presents have left the screen, deactivate and reset
        if (transform.position.x > 9f) {
            isAwake = false;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == TagManager.BACKGROUND_ELF_TAG) {
            isAwake = true;
        }
    }



    
}
