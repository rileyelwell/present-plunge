using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    [SerializeField] private TMPro.TextMeshProUGUI _stackScoreText, endScoreText, _totalScoreText;
    [SerializeField] private Image _heart1, _heart2, _heart3;

    public static ScoreManager instance;
    private int _stackScoreValue = 0, _totalScoreValue = 0, _lifeCount = 3;
    private float _stackHeightMultiplier = 1f, _stackTimeMultiplier = 1f;
    private float _fullStackBonus = 5f, _halfStackBonus = 2f;
    private float _fastestTimeBonus = 5f, _fastTimeBonus = 2f;
    public float _difficultyFactor = 3f;


    private void Awake() {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        // display starting scores (should be 0)
        _stackScoreText.text = $"{_stackScoreValue}";
        _totalScoreText.text = $"{_totalScoreValue}";
    }

    private void Update() {
        // if (_totalScoreValue > 100)
        //     _difficultyFactor = (_totalScoreValue / 100) + 2;
    }

    // this is called every time a present is stacked
    public void UpdateStackScore(int points) {
        // update the stack score with the number of stacked presents
        _stackScoreValue = points;
        _stackScoreText.text = $"{_stackScoreValue}";
    }

    // this is called when the player places the presents down
    public void UpdateTotalScore(int stackScore) {
        // reset the multipliers from the previous stack
        _stackTimeMultiplier = 1f;
        _stackHeightMultiplier = 1f;

        // update the height multiplier based on the stack's height
        if (stackScore == 10) {
            // print("Full stack bonus");
            _stackHeightMultiplier = _fullStackBonus;
        } else if (stackScore < 10 && stackScore >= 5) {
            // print("Half stack bonus");
            _stackHeightMultiplier = _halfStackBonus;
        }

        // update the time multiplier based on how long the stack has been held for
        float _totalStackTime = PresentStack.instance.GetStackTime();

        // if it is a full stack, there is a possible "fast" time bonus
        if (stackScore == 10) {
            if (_totalStackTime < 20f) {
                // print("Fastest time bonus");
                _stackTimeMultiplier = _fastestTimeBonus;
            } else if (_totalStackTime > 20f && _totalStackTime < 30f) {
                // print("Fast time bonus");
                _stackTimeMultiplier = _fastTimeBonus;
            }
        }  

        // update the total score calculated with multipliers and display to the user
        _totalScoreValue += (int)(stackScore * _stackHeightMultiplier * _stackTimeMultiplier);
        _totalScoreText.text = $"{_totalScoreValue}";
    }

    public void UpdateLifeCount() {
        // play animation for heart

        // remove correct number heart from screen, compare to _lifeCount value
        switch (_lifeCount) {
            case 3: 
                _heart3.enabled = false;
                break;
            case 2: 
                _heart2.enabled = false;
                break;
            case 1: 
                _heart1.enabled = false;
                break;
        }

        // decrement the number of player's lives
        _lifeCount--;

        // if lives are 0, the player has died, call game over.
        if (_lifeCount == 0)
            GameplayManager.instance.gameOver();
    }

    public void UpdateEndScoreScreen() {
        endScoreText.text = $"Score: {_totalScoreValue}";
    }
}
