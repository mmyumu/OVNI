using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootEmUpManager : MonoBehaviour {
    public GameObject countdownText;
    private bool isPlaying = false;
    private bool playingIntroduction = true;

    // Start is called before the first frame update
    void Start() {
        isPlaying = false;
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update() {

    }

    public bool IsPlaying() {
        return isPlaying;
    }

    public bool PlayingIntroduction() {
        return playingIntroduction;
    }

    private IEnumerator Countdown() {
        isPlaying = false;
        playingIntroduction = true;
        TMPro.TextMeshProUGUI tmpro = countdownText.GetComponent<TMPro.TextMeshProUGUI>();
        tmpro.enabled = true;
        tmpro.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        tmpro.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        tmpro.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        tmpro.text = "Go!";
        yield return new WaitForSecondsRealtime(1f);
        tmpro.enabled = false;
        isPlaying = true;
        playingIntroduction = false;
    }
}
