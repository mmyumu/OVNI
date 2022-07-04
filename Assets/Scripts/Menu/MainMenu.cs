using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Button defaultButton;
    public Button continueButton;

    public List<Button> buttonsOrdered;

    private void Start() {
        defaultButton.Select();

        if (PlayerPrefs.GetInt("money", -1) != -1) {
            continueButton.interactable = true;
        }

        UpdateNavigation();
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
        PlayerPrefs.SetInt("money", 100000);
    }
    private void UpdateNavigation() {
        List<Button> interactableButtons = new List<Button>();
        foreach (Button button in buttonsOrdered) {
            if (button.interactable) {
                interactableButtons.Add(button);
            }
        }

        if (interactableButtons.Count > 1) {
            Navigation nav = interactableButtons[0].navigation;
            nav.selectOnDown = interactableButtons[1];
            interactableButtons[0].navigation = nav;

            if (interactableButtons.Count > 2) {
                for (int i = 1; i < interactableButtons.Count - 1; i++) {
                    nav = interactableButtons[i].navigation;
                    nav.selectOnUp = interactableButtons[i - 1];
                    nav.selectOnDown = interactableButtons[i + 1];
                    interactableButtons[i].navigation = nav;
                }
            }


            nav = interactableButtons[interactableButtons.Count - 1].navigation;
            nav.selectOnUp = interactableButtons[interactableButtons.Count - 2];
            interactableButtons[interactableButtons.Count - 1].navigation = nav;
        }
    }
}
