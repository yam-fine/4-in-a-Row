using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Image cvpButton;
    [SerializeField] private Image pvpButton;
    [SerializeField] private Image cvcButton;
    [SerializeField] private Image resetButton;
    [SerializeField] private Color32 highlightedColor;
    [SerializeField] private GameObject diskSpawnParent;
    [SerializeField] private GameObject colliderSpawnParent;
    [SerializeField] private ConnectGameGrid gameBoard;

    private Color32 regularColor;
    private string diskTag = "Disk";
    private bool gameStarted = false;
    private VictoryScreen vs;

    public ConnectGameGrid GameBoard { get => gameBoard; }

    private static UIManager instance;
    public static UIManager Instance {
        get {
            if (!instance)
                instance = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            return instance;
        }
    }

    public enum GameModeSelector {
        PvC,
        PvP,
        CvC
    }
    [HideInInspector] public GameModeSelector currentMode;

    private GameManager gm;

    private void Start() {
        gm = GameManager.Instance;
        vs = VictoryScreen.Instance;
        confirmButton.gameObject.SetActive(false);
        regularColor = new Color32(255, 255, 255, 255);
    }

    public void GameEndBlue() {
        vs.BlueWon();
        GameEnd();
    }

    public void GameEndRed() {
        vs.RedWon();
        GameEnd();
    }

    public void GameEndDraw() {
        vs.Draw();
        GameEnd();
    }

    /// <summary>
    /// functions that need to happen on any way a game can end at
    /// </summary>
    private void GameEnd() {
        gameBoard.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ContinueButton() {
        RestartButton();
        confirmButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

    public void ComputerButton() {
        currentMode = GameModeSelector.PvC;

        confirmButton.gameObject.SetActive(true);
        cvpButton.color = highlightedColor;
        pvpButton.color = regularColor;
        cvcButton.color = regularColor;
    }

    public void RegularButton() {
        currentMode = GameModeSelector.PvP;

        confirmButton.gameObject.SetActive(true);
        pvpButton.color = highlightedColor;
        cvpButton.color = regularColor;
        cvcButton.color = regularColor;
    }

    public void CvCButton() {
        currentMode = GameModeSelector.CvC;

        confirmButton.gameObject.SetActive(true);
        pvpButton.color = regularColor;
        cvpButton.color = regularColor;
        cvcButton.color = highlightedColor;
    }

    public void ConfirmButton() {
        switch (currentMode) {
            case GameModeSelector.PvC:
                gm.LocalPlayer = new RegularPlayer();
                gm.EnemyPlayer = new ComputerPlayer();
                break;
            case GameModeSelector.PvP:
                gm.LocalPlayer = new RegularPlayer();
                gm.EnemyPlayer = new RegularPlayer();
                break;
            case GameModeSelector.CvC:
                gm.LocalPlayer = new ComputerPlayer();
                gm.EnemyPlayer = new ComputerPlayer();
                break;
        }

        gameBoard.gameObject.SetActive(true);
        mainMenuPanel.SetActive(false);
        resetButton.gameObject.SetActive(false);
        gameStarted = true;
        gm.StartGame();
    }

    public void CloseButton() {
        mainMenuPanel.SetActive(false);
        resetButton.gameObject.SetActive(false);
    }

    public void MenuButton() {
        resetButton.gameObject.SetActive(true);
        if (mainMenuPanel.activeInHierarchy)
            mainMenuPanel.SetActive(false);
        else {
            mainMenuPanel.SetActive(true);
            if (gameStarted)
                confirmButton.gameObject.SetActive(false);
        }
    }

    public void RestartButton() {
        DestroyAllDisks();
        DisableAllColliders();
        gameBoard.gameObject.SetActive(false);
        ResetMainMenu();
        vs.ResetVictoryScreen();
    }

    /// <summary>
    /// destroys all disk objects in the board
    /// </summary>
    private void DestroyAllDisks() {
        foreach (Transform child in diskSpawnParent.GetComponentsInChildren<Transform>()) {
            if (child.CompareTag(diskTag)) {
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// disables all colliders on the board in order to reset the board
    /// </summary>
    private void DisableAllColliders() {
        foreach (Transform child in colliderSpawnParent.GetComponentsInChildren<Transform>()) {
            if (child.name != colliderSpawnParent.name)
                child.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void ResetMainMenu() {
        mainMenuPanel.SetActive(true);
        resetButton.gameObject.SetActive(true);
        cvpButton.color = regularColor;
        pvpButton.color = regularColor;
        confirmButton.gameObject.SetActive(false);

        gameStarted = false;

        gm.ResetGameManager();
        Board.Instance.ResetBoard();
    }
}