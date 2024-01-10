using System;
using UnityEngine;

public abstract class Player {
    [HideInInspector] public Disk diskPrefab;
    public abstract void StartTurn();
    public delegate void EndTurnDeletgate();
    public event Action<int, int> OnEndTurn;
    protected virtual void EndTurn(int column, int row) {
        OnEndTurn?.Invoke(column, row);
    }
    public abstract void ResetPlayer();
}