using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : UnitySingleton<LevelController>
{
    public int turnNumber = 1;
    // the player whose turn is currently active
    public GameManager.Player currPlayer { get; private set; }
    public GameObject player1G;
    public GameObject player2G;
    public GameObject player1Inventory;
    public GameObject player2Inventory;
    // while the game is waiting for the player to type a word, this is set to true.
    // Otherwise, set to false
    private bool _waitForWord = false;

    private void Start()
    {
        currPlayer = GameManager.Player.P1;
        OnPlayerBeginTurn();
    }

    public void OnPlayerEndTurn()
    {
        // disable control for the opposite player
        print("Current Turn: " + currPlayer.ToString());
        DisablePlayerControl();
    }

    public void OnPlayerBeginTurn()
    {
        EnablePlayerControl();
    }

    public void OnPlayerDied(GameObject deadPlayer)
    {
        if (deadPlayer == player1G)
        {
            print("Player 1 has died!");
        } else
        {
            print("Player 2 has died!");
        }

        // TODO: transition to the win scene using LevelTransitionManager
    }

    public void DisablePlayerControl()
    {
        if (this.currPlayer == GameManager.Player.P1) {
            this.player1Inventory.GetComponent<TileInventory>().isDisabled = true;
        } else {
            this.player2Inventory.GetComponent<TileInventory>().isDisabled = true;
        }
    }

    public void EnablePlayerControl()
    {
        if (this.currPlayer == GameManager.Player.P1) {
            this.player1Inventory.GetComponent<TileInventory>().isDisabled = false;
        } else {
            this.player2Inventory.GetComponent<TileInventory>().isDisabled = false;
        }
    }

    IEnumerator WaitForWord()
    {
        // _waitForWord will be set to false when SubmitWord is called
        _waitForWord = true;
        while (_waitForWord)
            yield return null;
    }

    // called when the player submits their word
    public void SubmitWord()
    {
   
        GameObject currPlayerG; // the player whose turn it currently is
        GameObject oppPlayerG; // the opposite players
        int wordDmgAmt = 0;

        if (currPlayer == GameManager.Player.P1)
        {
            currPlayerG = player1G;
            oppPlayerG = player2G;
            wordDmgAmt = player1Inventory.GetComponent<TileInventory>().wordScore;
        }
        else
        {
            currPlayerG = player2G;
            oppPlayerG = player1G;
            wordDmgAmt = player2Inventory.GetComponent<TileInventory>().wordScore;
        }


        // TODO: add damage calculation to word
        

        // TODO: player the attack animation here

        // damage the opposite player
        Health oppHealth = oppPlayerG.GetComponent<Health>(); 
        oppHealth.BumpHp(-wordDmgAmt);
        if (oppHealth.IsDead())
        {
            OnPlayerDied(oppPlayerG);
            return; // break out of gameplay loop
        }

        // Clearing the tiles
        if (currPlayer == GameManager.Player.P1)
        {
            player1Inventory.GetComponent<TileInventory>().ClearTiles();
        } 
        else 
        {
            player2Inventory.GetComponent<TileInventory>().ClearTiles();
        }

        _waitForWord = false;
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        this.turnNumber += 1;
        OnPlayerEndTurn();

        if (currPlayer == GameManager.Player.P1) currPlayer = GameManager.Player.P2;
        else currPlayer = GameManager.Player.P1;

        OnPlayerBeginTurn();
    }

}
