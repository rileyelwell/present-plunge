using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PresentStack : MonoBehaviour {

    public static PresentStack instance;
    private List<GameObject> _stackedPresents = new List<GameObject>();

    private float _minSpawnPositionX = -8f, _maxSpawnPositionX = 8f, _minSpawnPositionY = -3.5f, _maxSpawnPositionY = -4.2f;
    private float _presentHeightOffset = 0.5f;

    private float _elapsedStackTime = 0f;


    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Update() {
        if (_stackedPresents.Count() > 0) {
            _elapsedStackTime += Time.deltaTime;
        }
    }

    public void ResetStack() {
        for (int i = 0; i < _stackedPresents.Count(); i++) {
            // set present to inactive to add it back to the object pool
            _stackedPresents[i].SetActive(false);
        }
        // reset the stack list by clearing it completely
        _stackedPresents.Clear();
    }

    public void PlaceBackgroundPresents() {
        // get a random position (within limits) for the stack
        float _randomX = Random.Range(_minSpawnPositionX, _maxSpawnPositionX);
        float _randomY = Random.Range(_minSpawnPositionY, _maxSpawnPositionY);
        float _stackPositionY = _randomY;

        // print("Placing Present Function");

        for (int i = 0; i < _stackedPresents.Count(); i++) {
            // add a background present and adjust its position
            GameObject _backgroundPresent = ObjectPool.instance.GetPooledBackgroundPresents();
            Vector3 _spawnPosition = new Vector3(_randomX, _randomY, 0f);

            // if the present is not the bottom present, then adjust y position to be above the base
            if (i > 0)
                _stackPositionY += _presentHeightOffset;
                _spawnPosition.y = _stackPositionY;
            
            if (_backgroundPresent != null) {
                // print($"Placing a present at {_spawnPosition}");
            _backgroundPresent.transform.position = _spawnPosition;

            // correctly set sorting order for the present stack
            _backgroundPresent.GetComponent<SpriteRenderer>().sortingOrder += i;

            _backgroundPresent.SetActive(true);
            }
        }
    }

    public void AddPresentToStack(GameObject present) {
        _stackedPresents.Add(present);
    }

    public GameObject GetTopPresentOfStack() {
        return _stackedPresents.Last();
    }

    public int GetPresentStackCount() {
        return _stackedPresents.Count();
    }

    public float GetStackTime() {
        // print(_elapsedStackTime);
        return _elapsedStackTime;
    }

    public void ResetStackTime() {
        _elapsedStackTime = 0;
    }
    
}
