using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawner : MonoBehaviour {

    public static ManagerSpawner instance;
    [SerializeField] private GameObject _gameplayManager;

    private void Awake() {
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
        } 
        
        // print("creating gameplay manager");
        Instantiate(_gameplayManager, transform);
    }
}
