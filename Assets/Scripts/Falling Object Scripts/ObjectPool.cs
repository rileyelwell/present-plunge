using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool instance;

    private List<GameObject> _pooledPresents = new List<GameObject>(), _pooledAnvils = new List<GameObject>();
    private List<GameObject> _pooledBackgroundPresents = new List<GameObject>();
    private int _totalPooledPresents = 20, _totalPooledAnvils = 10, _totalPooledBackgroundPresents = 30;

    [SerializeField]
    private GameObject _presentPrefab, _anvilPrefab, _backgroundPresent;

    [SerializeField] private Transform _backgroundPresentParent;


    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        // instantiate the presents needed for cycling through the game
        for (int i = 0; i < _totalPooledPresents; i++) {
            GameObject _spawnedPresent = Instantiate(_presentPrefab);
            _spawnedPresent.SetActive(false);
            _spawnedPresent.name = $"Present {i}";
            _pooledPresents.Add(_spawnedPresent);
        }

        // instantiate the anvils needed for cycling through the game
        for (int i = 0; i < _totalPooledAnvils; i++) {
            GameObject _spawnedAnvil = Instantiate(_anvilPrefab);
            _spawnedAnvil.SetActive(false);
            _spawnedAnvil.name = $"Anvil {i}";
            _pooledAnvils.Add(_spawnedAnvil);
        }

        // instantiate the background presents needed for cycling through the game
        for (int i = 0; i < _totalPooledBackgroundPresents; i++) {
            GameObject _spawnedBackgroundPresent = Instantiate(_backgroundPresent);
            _spawnedBackgroundPresent.SetActive(false);
            _spawnedBackgroundPresent.name = $"Background Present {i}";
            _spawnedBackgroundPresent.transform.SetParent(_backgroundPresentParent);
            _pooledBackgroundPresents.Add(_spawnedBackgroundPresent);
        }
    }

    // return the list of pooled presents (that are deactivated)
    public GameObject GetPooledPresents() {
        for (int i = 0; i < _pooledPresents.Count; i++) {
            if (!_pooledPresents[i].activeInHierarchy) {
                return _pooledPresents[i];
            }
        }
        return null;
    }

    // return the list of pooled anvils (that are deactivated)
    public GameObject GetPooledAnvils() {
        for (int i = 0; i < _pooledAnvils.Count; i++) {
            if (!_pooledAnvils[i].activeInHierarchy) {
                return _pooledAnvils[i];
            }
        }
        return null;
    }   

    // return the list of pooled presents (that are deactivated)
    public GameObject GetPooledBackgroundPresents() {
        for (int i = 0; i < _pooledBackgroundPresents.Count; i++) {
            if (!_pooledBackgroundPresents[i].activeInHierarchy) {
                return _pooledBackgroundPresents[i];
            }
        }
        return null;
    } 
}
