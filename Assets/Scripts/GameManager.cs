using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingletonPersistant<GameManager>
{
    public void Start() {

    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public enum Player
    {
        P1,
        P2
    };

}
