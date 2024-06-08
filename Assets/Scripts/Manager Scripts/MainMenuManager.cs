using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public void PlayGame() {
        MenuSoundManager.instance.PlayButtonClick();
        SceneManager.LoadScene(TagManager.GAMEPLAY_SCENE_TAG);
    }
}
