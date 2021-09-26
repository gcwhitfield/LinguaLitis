using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : UnitySingleton<LevelController>
{

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
    }

    public void OnPlayerEndTurn()
    {
        // disable control for the opposite player
        print("Current Turn: " + currPlayer.ToString());
    }

    public void OnPlayerBeginTurn()
    {

    }

    public void DisablePlayerControl(GameManager.Player player)
    {
        GameObject playerG;
        if (player == GameManager.Player.P1)
            playerG = player1G;
        else
            playerG = player2G;

        // TODO: disable control
    }

    public void EnablePlayerControl(GameManager.Player player)
    {
        GameObject playerG;
        if (player == GameManager.Player.P1)
            playerG = player1G;
        else
            playerG = player2G;

        // TODO: enable control
    }

    IEnumerator WaitForWord()
    {
        while (_waitForWord)
            yield return null;
    }

    // called when the player submits their word
    public void SubmitWord(GameManager.Player player)
    {
        GameObject currPlayerG; // the player whose turn it currently is
        GameObject oppPlayerG; // the opposite players

        if (player == GameManager.Player.P1) { currPlayerG = player1G; oppPlayerG = player2G; }
        else { currPlayerG = player2G; oppPlayerG = player1G; }

        // TODO: add damage calculation to word
        int wordDmgAmt = 1;

        // TODO: player the attack animation here

        // damage the opposite player
        oppPlayerG.GetComponent<Health>().BumpHp(wordDmgAmt);

        ChangeTurn();
    }

    public void ChangeTurn()
    {
        OnPlayerEndTurn();

        if (currPlayer == GameManager.Player.P1) currPlayer = GameManager.Player.P2;
        else currPlayer = GameManager.Player.P1;
        _waitForWord = false;

        OnPlayerBeginTurn();
    }

}
