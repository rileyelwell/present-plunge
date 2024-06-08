using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour {
    public static GameplayManager instance;
    [SerializeField] private GameObject pausePanel, gameOverPanel, instructionsPanel;
    private bool isPaused, canPause = true;


    private void Awake() {
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
        } 
    }

    private void Start() {
        Time.timeScale = 0f;
        canPause = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseGame();
    }

    public void UpdateStartingVariables() {
        Time.timeScale = 1f;
        canPause = true;
    }

    public void pauseGame() {
        if (!isPaused && canPause) {
            SoundManager.instance.PlayButtonClick();
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            isPaused = true;
        } else {
            resumeGame();
        }
    }

    public void resumeGame() {
        SoundManager.instance.PlayButtonClick();
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private IEnumerator PlayClickSoundWait() {
        SoundManager.instance.PlayButtonClick();
        yield return new WaitForSeconds(1f);
    }

    public void MainMenu() {
        PlayClickSoundWait();
        SceneManager.LoadScene(TagManager.MAIN_MENU_SCENE_TAG);
        Time.timeScale = 1f;
    }

    public void restartGame() {
        PlayClickSoundWait();
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
        Time.timeScale = 1f;
    }

    public void TurnOffInstructions() {
        SoundManager.instance.PlayButtonClick();
        instructionsPanel.SetActive(false);
    }

    public void gameOver() {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        canPause = false;

        // update the score for the end screen
        ScoreManager.instance.UpdateEndScoreScreen();
    }
}
