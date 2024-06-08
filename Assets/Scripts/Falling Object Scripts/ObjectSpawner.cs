using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    
    private Vector3 _spawnPosition;
    private float _minSpawnPosition = -8f, _maxSpawnPosition = 8f;
    private bool hasStarted = false;

    private void Update() {
        // begin the object dropping when player begins, turn off instructions
        if (Input.GetKeyDown(KeyCode.Space) && !hasStarted) {
            hasStarted = true;
            GameplayManager.instance.TurnOffInstructions();
            GameplayManager.instance.UpdateStartingVariables();
            InvokeRepeating("SpawnObject", 1f, 0.25f);
        }
    }

    private void SpawnObject() {
        // spawn randomly (somewhat) 
        float _randomValue = Random.Range(0, 4f);
        
        if (_randomValue < 2) {
            SpawnPresent();
        } else {
            SpawnAnvil();
        }
    }

    private void SpawnAnvil() {
        _spawnPosition = new Vector3(Random.Range(_minSpawnPosition, _maxSpawnPosition), 6f, 0f);
        GameObject _anvil = ObjectPool.instance.GetPooledAnvils();

        if (_anvil != null) {
            // update the position for the new spawn
            _anvil.transform.position = _spawnPosition;

            // add other dependencies to reset when spawning
            _anvil.GetComponent<Anvil>().ResetAnvil();

            // set object to active
            _anvil.SetActive(true);
        }
    }

    private void SpawnPresent() {
        _spawnPosition = new Vector3(Random.Range(_minSpawnPosition, _maxSpawnPosition), 6f, 0f);
        GameObject _present = ObjectPool.instance.GetPooledPresents();

        if (_present != null) {
            // update the position for the new spawn
            _present.transform.position = _spawnPosition;

            // add other dependencies to reset when spawning
            _present.GetComponent<Present>().ResetPresent();

            // set object to active
            _present.SetActive(true);
        }
    }

}
