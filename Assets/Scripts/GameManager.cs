using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingletonPersistant<GameManager>
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public enum Player
    {
        P1,
        P2
    };


}
