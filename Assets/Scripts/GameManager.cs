using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingletonPersistant<GameManager>
{
    public GameObject LevelController;
    public int turnNumber = 1;

    public void Start() {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public enum Player
    {
        P1,
        P2
    };

    public void NextTurn() {
        this.turnNumber += 1;
        LevelController.GetComponent<LevelController>().ChangeTurn();  // replace with submit word function later 
    }


}
