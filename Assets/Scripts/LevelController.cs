using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : UnitySingleton<LevelController>
{
    public enum PlayerTurn
    {
        P1,
        P2
    }

    public PlayerTurn currentTurn { get; private set; }

    public void ChangeTurn()
    {
        if (currentTurn == PlayerTurn.P1) currentTurn = PlayerTurn.P2;
        else currentTurn = PlayerTurn.P1;
    }

}
