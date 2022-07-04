using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
    public Button defaultButton;
    public List<Button> menuButtons;
    public GameObject cursor;
    public GameObject moneyText;

    private bool mapEnabled = false;

    // Start is called before the first frame update
    void Start() {
        defaultButton.Select();
        UpdateMoney();
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnMapClick() {
        EnableMap(true);
        InteractableMenu(false);
        mapEnabled = true;
    }

    public void OnFightClick() {
        Debug.Log("Fight");
        SceneManager.LoadScene("ShootEmUp");
    }

    public void OnPressBack() {
        if (mapEnabled) {
            EnableMap(false);
            InteractableMenu(true);
            defaultButton.Select();
            mapEnabled = false;
        }
    }

    private void EnableMap(bool enabled) {
        cursor.SetActive(enabled);
    }

    private void InteractableMenu(bool interactable) {
        foreach (Button menuButton in menuButtons) {
            menuButton.interactable = interactable;
        }
    }

    private void UpdateMoney() {
        TMPro.TextMeshProUGUI tmpro = moneyText.GetComponent<TMPro.TextMeshProUGUI>();
        tmpro.text = PlayerPrefs.GetInt("money").ToString("C");
    }
}
