using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour {

    [SerializeField] private float _minSpeed = 4f, _maxSpeed = 6f, _rotationSpeedMult = 25f;

    private float _speed, _endPosition = -6f;
    private bool _isCaught = false;
    private int _maxStackValue = 10;

    private Vector3 _presentStackedPosition = new Vector3 (0.75f, -0.2f, 0f);
    

    private void Start() {
        // randomize the speed value for each present
        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    private void Update() {
        // if the present has not been caught, keep moving
        if (!_isCaught)
            FallDown();

        // deactivate after leaving screen
        if (transform.position.y < _endPosition)
            gameObject.SetActive(false);
    }

    private void FallDown() {
        transform.Rotate(Vector3.forward * (_speed * _rotationSpeedMult) * Time.deltaTime, Space.Self);
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
    }

    public void ResetPresent() {
        // reset present tags for being caught
        _isCaught = false;
        gameObject.tag = TagManager.PRESENT_TAG;
        gameObject.layer = LayerMask.NameToLayer(TagManager.FALLING_PRESENT_LAYER_TAG);

        // make sure the present's collider is enabled
        // gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(TagManager.FALLING_OBJECT_LAYER_TAG);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;

    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // if a falling present lands on top of the stack (a caught present that is top)
        if (collider.tag == TagManager.CAUGHT_PRESENT_TAG && collider.gameObject == PresentStack.instance.GetTopPresentOfStack() 
            && _isCaught == false && PresentStack.instance.GetPresentStackCount() < _maxStackValue) {
            
            // play the presents sound clip
            SoundManager.instance.PlayStackPlacingSound();

            // set the player as a parent for the present
            transform.SetParent(collider.transform.parent);

            // set present position and rotation (player should now be carrying it)
            transform.position = collider.transform.position + new Vector3(0, 0.5f, 0);
            transform.rotation = Quaternion.identity;

            // toggle the caught flag so it stops moving
            _isCaught = true;

            // change the object tag to indicate the present has been caught (CaughtPresent)
            gameObject.tag = TagManager.CAUGHT_PRESENT_TAG;

            // change the present's layer to be the "top" of the stack
            gameObject.layer = LayerMask.NameToLayer(TagManager.TOP_PRESENT_LAYER_TAG);

            // adjust the present's sorting layer to be displayed above player
            gameObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(TagManager.TOP_PRESENT_LAYER_TAG);

            // change the previous "top" of stack to a "stacked" present layer
            PresentStack.instance.GetTopPresentOfStack().layer = LayerMask.NameToLayer(TagManager.STACKED_PRESENT_LAYER_TAG);

            // adjust the stacked present's sorting layer to be displayed under the top present
            PresentStack.instance.GetTopPresentOfStack().GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(TagManager.STACKED_PRESENT_LAYER_TAG);
            PresentStack.instance.GetTopPresentOfStack().GetComponent<SpriteRenderer>().sortingOrder += PresentStack.instance.GetPresentStackCount();

            // add the current present to the present stack
            PresentStack.instance.AddPresentToStack(gameObject);

            // update the score with the number of presents in the stack
            ScoreManager.instance.UpdateStackScore(PresentStack.instance.GetPresentStackCount());
        }

        // if a falling present is caught by the player and is the first present...
        if (collider.tag == TagManager.PLAYER_TAG && PresentStack.instance.GetPresentStackCount() <= 0) {

            // play the presents sound clip
            SoundManager.instance.PlayStackPlacingSound();

            // set the player as a parent for the present
            transform.SetParent(collider.transform);

            // set present position and rotation (player should now be carrying it)
            // print(collider.gameObject.transform.position);
            if (collider.gameObject.transform.localScale.x > 0) {
                transform.position = collider.gameObject.transform.position + _presentStackedPosition;
            } else {
                transform.position = collider.gameObject.transform.position + new Vector3 
                    (-1 * _presentStackedPosition.x, _presentStackedPosition.y, _presentStackedPosition.z);
            }
            transform.rotation = Quaternion.identity;

            // toggle the caught flag so it stops moving
            _isCaught = true;

            // change the object tag to indicate the present has been caught (CaughtPresent)
            gameObject.tag = TagManager.CAUGHT_PRESENT_TAG;

            // change the present's collision layer to be the "top" of the stack
            gameObject.layer = LayerMask.NameToLayer(TagManager.TOP_PRESENT_LAYER_TAG);

            // adjust the present's sorting layer to be displayed above player
            gameObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(TagManager.TOP_PRESENT_LAYER_TAG);

            // add the current present to the present stack
            PresentStack.instance.AddPresentToStack(gameObject);

            // update the score with the number of presents in the stack
            ScoreManager.instance.UpdateStackScore(PresentStack.instance.GetPresentStackCount());
        }
    }
}
