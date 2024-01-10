using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : Player {
    private int currColumn, lastRow;
    private IDisk spawnedDisk;
    private Board board;

    public override void StartTurn() {
        board = Board.Instance;
        SpawnDisk();
    }

    private void SpawnDisk() {
        List<int> unfilledCols = GetAllUnfilledColumns(board.boardState, board.NumRows, board.NumColumns);
        currColumn = RandomListIndex(unfilledCols);
        lastRow = board.boardState.ContainsKey(currColumn) ? board.boardState[currColumn] : 0;
        spawnedDisk = UI.UIManager.Instance.GameBoard.Spawn(diskPrefab, currColumn, lastRow);
        spawnedDisk.StoppedFalling += OnStoppedFalling;
        board.boardState[currColumn] = lastRow + 1;
    }

    private void OnStoppedFalling() {
        spawnedDisk.StoppedFalling -= OnStoppedFalling;
        EndTurn(currColumn, lastRow);
    }

    /// <summary>
    /// returns a List<int> of all the unfilled columns in the board
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="numRows"></param>
    /// <param name="numCols"></param>
    /// <returns></returns>
    private List<int> GetAllUnfilledColumns(Dictionary<int, int> dictionary, int numRows, int numCols) {
        List<int> arr = new List<int>();

        for (int i = 0; i < numCols; i++) {
            if (!dictionary.ContainsKey(i) || dictionary[i] < numRows)
                arr.Add(i);
        }
        return arr;
    }

    /// <summary>
    /// return the element at a random index in the given List<int>
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    private int RandomListIndex(List<int> arr) {
        System.Random random = new System.Random();

        // Generate a random index within the bounds of the list
        int randomIndex = random.Next(0, arr.Count);

        // Retrieve and return the element at the random index
        int randomValue = arr[randomIndex];
        return randomValue;
    }

    public override void ResetPlayer() {
        spawnedDisk.StoppedFalling -= OnStoppedFalling;
    }
}
