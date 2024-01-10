public class RegularPlayer : Player {
    private bool isSpawning = false;
    private int currColumn, lastRow;
    private Board board;
    private UIManager ui;
    private IDisk spawnedDisk;

    public override void StartTurn() {
        ui = UIManager.Instance;
        board = Board.Instance;

        if (!isSpawning) {
            ui.GameBoard.ColumnClicked += SpawnDisk;
        }
    }

    private void SpawnDisk(int column) {
        ui.GameBoard.ColumnClicked -= SpawnDisk;
        isSpawning = true;
        currColumn = column;

        // find out the row to spawn in
        lastRow = board.boardState.ContainsKey(column) ? board.boardState[column] : 0;
        spawnedDisk = ui.GameBoard.Spawn(diskPrefab, column, lastRow);
        spawnedDisk.StoppedFalling += OnStoppedFalling;
        // increase the top row for the next iteration
        board.boardState[column] = lastRow + 1;
    }

    private void OnStoppedFalling() {
        spawnedDisk.StoppedFalling -= OnStoppedFalling;
        EndTurn(currColumn, lastRow);
    }

    protected override void EndTurn(int column, int row) {
        base.EndTurn(column, row);
        isSpawning = false;
    }

    public override void ResetPlayer() {
        ui.GameBoard.ColumnClicked -= SpawnDisk;
        if (spawnedDisk != null)
            spawnedDisk.StoppedFalling -= OnStoppedFalling;
    }
}