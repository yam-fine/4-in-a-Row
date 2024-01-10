using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour {
    [SerializeField] private Text blueWin;
    [SerializeField] private Text redWin;
    [SerializeField] private Text draw;
    private Image background;

    private static VictoryScreen instance;
    public static VictoryScreen Instance { get => instance; }

    private void Awake() {
        instance = this;
        background = GetComponent<Image>();
        background.enabled = false;
    }

    public void BlueWon() {
        DisableAllText();
        background.enabled = true;
        blueWin.GetComponent<Text>().enabled = true;
    }

    public void RedWon() {
        DisableAllText();
        background.enabled = true;
        redWin.GetComponent<Text>().enabled = true;
    }

    public void Draw() {
        DisableAllText();
        background.enabled = true;
        draw.GetComponent<Text>().enabled = true;
    }

    /// <summary>
    /// disables all possible texts on the winning screen
    /// </summary>
    private void DisableAllText() {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
            if (child.name != gameObject.name)
                child.GetComponent<Text>().enabled = false;
        }
    }

    public void ResetVictoryScreen() {
        DisableAllText();
        background.enabled = false;
    }
}