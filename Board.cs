using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField] private int numColumns = 7;
    [SerializeField] private int numRows = 6;
    [SerializeField] private Disk blueDiskPrefab;
    [SerializeField] private Disk redDiskPrefab;

    // saves the amount of disks in each column of the board
    [HideInInspector] public Dictionary<int, int> boardState = new Dictionary<int, int>();

    private ulong blueBitboard = 0;
    private ulong redBitboard = 0;

    public ulong BlueBitboard { get => blueBitboard; }
    public ulong RedBitboard { get => redBitboard; }
    public int NumColumns { get => numColumns; }
    public int NumRows { get => numRows; }

    private static Board instance;
    public static Board Instance {
        get {
            if (!instance)
                instance = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
            return instance;
        }
    }

    public Disk BlueDiskPrefab { get => blueDiskPrefab; }
    public Disk RedDiskPrefab { get => redDiskPrefab; }

    /// <summary>
    /// updates the bitboards when a player makes a move
    /// </summary>
    /// <param name="column"></param>
    /// <param name="isBluePlayer"></param>
    /// <param name="row"></param>
    public void MakeMove(int column, bool isBluePlayer, int row) {
        // calculate 2 to the power of position for (row, col)
        ulong newBit = 1UL << (column + (row * numColumns));
            
        // change the correct bit to 1
        if (isBluePlayer) {
            blueBitboard |= newBit;
        }
        else {
            redBitboard |= newBit;
        }
    }

    /// <summary>
    /// checks if there is a winning formation on the board
    /// </summary>
    /// <param name="bitboard"></param>
    /// <returns></returns>
    public bool IsWin(ulong bitboard) {

        // Horizontal check
        ulong m = bitboard & (bitboard >> numColumns);
        if ((m & (m >> numColumns * 2)) != 0) 
            return true;

        // Diagonal \
        int shiftAmount = NumColumns - 1;
        m = bitboard & (bitboard >> shiftAmount);
        if ((m & (m >> shiftAmount * 2)) != 0) 
            return true;

        // Diagonal /
        shiftAmount = NumColumns + 1;
        m = bitboard & (bitboard >> shiftAmount);
        if ((m & (m >> shiftAmount * 2)) != 0) 
            return true;

        // Vertical
        m = bitboard & (bitboard >> 1);
        if ((m & (m >> 2)) != 0) 
            return true;

        // Nothing found
        return false;
    }

    public void ResetBoard() {
        boardState.Clear();
        blueBitboard = 0;
        redBitboard = 0;
    }
}