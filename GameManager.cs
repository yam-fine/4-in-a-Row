using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Board board;
    private AudioManager am;
    private UI.UIManager ui;
    private int numTurns = 0;
    private int maxTurns;

    // player is initialized via mode selection in Main Menu and is then set here
    private Player localPlayer;
    public Player LocalPlayer {
        get => localPlayer;
        set {
            localPlayer = value;
            localPlayer.diskPrefab = board.BlueDiskPrefab;
            localPlayer.OnEndTurn += DiskStoppedFalling;
        }
    }

    // enemy is initialized via mode selection in Main Menu and is then set here
    private Player enemyPlayer;
    public Player EnemyPlayer {
        get => enemyPlayer;
        set {
            enemyPlayer = value;
            enemyPlayer.diskPrefab = board.RedDiskPrefab;
            enemyPlayer.OnEndTurn += DiskStoppedFalling;
        }
    }

    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (!instance)
                instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            return instance;
        }
    }

    private void Start() {
        board = Board.Instance;
        ui = UI.UIManager.Instance;
        am = AudioManager.Instance;
        maxTurns = board.NumColumns * board.NumRows;
    }

    /// <summary>
    /// called at the start of each seperate game (different from Start)
    /// </summary>
    public void StartGame() {
        Turn();
    }

    /// <summary>
    /// Calls on the appropriate player to execute their turn
    /// </summary>
    public void Turn() {
        if (numTurns % 2 == 0) {
            localPlayer.StartTurn();
        }
        else {
            enemyPlayer.StartTurn();
        }
    }

    private void DiskStoppedFalling(int column, int row) {
        bool isBlue;

        if (numTurns % 2 == 0) {
            isBlue = true;
            am.OnBlueDisk();
        }
        else {
            isBlue = false;
            am.OnRedDisk();
        }

        board.MakeMove(column, isBlue, row);

        ulong bitboardToCheck = isBlue ? board.BlueBitboard : board.RedBitboard;
        if (board.IsWin(bitboardToCheck))
            OnWin(isBlue);

        numTurns++;
        if (numTurns >= maxTurns)
            OnDraw();

        Turn();
    }

    private void OnWin(bool isBlue) {
        am.OnWin();
        if (isBlue)
            ui.GameEndBlue();
        else
            ui.GameEndRed();
    }

    private void OnDraw() {
        am.OnDraw();
        ui.GameEndDraw();
    }

    public void ResetGameManager() {
        numTurns = 0;
        localPlayer.OnEndTurn -= DiskStoppedFalling;
        enemyPlayer.OnEndTurn -= DiskStoppedFalling;

        localPlayer.ResetPlayer();
        enemyPlayer.ResetPlayer();
    }
}