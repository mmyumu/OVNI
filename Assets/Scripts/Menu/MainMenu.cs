using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Button defaultButton;
    private void Start() {
        defaultButton.Select();
    }

    public void NewGame() {
        ResetPrefs();
        Debug.Log("New Game");
        SceneManager.LoadScene("World");
    }

    public void ContinueGame() {
        Debug.Log("Continue Game");
        SceneManager.LoadScene("World");
    }

    public void QuitGame() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    private void ResetPrefs() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Money", 100000);
    }
}
