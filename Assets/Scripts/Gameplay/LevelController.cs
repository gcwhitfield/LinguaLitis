using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : UnitySingleton<LevelController>
{

    // the player whose turn is currently active
    public GameManager.Player currPlayer { get; private set; }
    public GameObject player1G;
    public GameObject player2G;

    // while the game is waiting for the player to type a word, this is set to true.
    // Otherwise, set to false
    private bool _waitForWord = false;



    private void Start()
    {
        currPlayer = GameManager.Player.P1;    
    }

    public void OnPlayerEndTurn()
    {
        // TODO: re-enable control for the opposite player

        // TODO: remove the staged tiles for currPlayer, give currPlayer new tiles
    }

    public void OnPlayerBeginTurn()
    {

        // disable control for the opposite player
        print("Current Turn: " + currPlayer.ToString());

        StartCoroutine("WaitForWord");
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

        if (currPlayer == GameManager.Player.P1) { currPlayerG = player1G; oppPlayerG = player2G; }
        else { currPlayerG = player2G; oppPlayerG = player1G; }

        // TODO: add damage calculation to word
        int wordDmgAmt = 10;

        // TODO: player the attack animation here

        // damage the opposite player
        Health oppHealth = oppPlayerG.GetComponent<Health>(); 
        oppHealth.BumpHp(-wordDmgAmt);
        if (oppHealth.IsDead())
        {
            OnPlayerDied(oppPlayerG);
            return; // break out of gameplay loop
        }

        _waitForWord = false;
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        OnPlayerEndTurn();

        if (currPlayer == GameManager.Player.P1) currPlayer = GameManager.Player.P2;
        else currPlayer = GameManager.Player.P1;

        OnPlayerBeginTurn();
    }

}
