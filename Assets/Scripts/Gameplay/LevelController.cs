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
    public GameObject pauseMenu;
    public GameObject FMOD;
    public Animator WinGraphicAnimtor;
    // while the game is waiting for the player to type a word, this is set to true.
    // Otherwise, set to false
    private bool _waitForWord = false;
    public bool _isPaused = false;
    public KeyCode pauseKey;
    private KeyCode[] validKeys = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, 
    KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, 
    KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Delete, KeyCode.Backspace, KeyCode.Space, KeyCode.Return};

    private void Start()
    {
        currPlayer = GameManager.Player.P1;
        OnPlayerBeginTurn();
        this.pauseMenu.SetActive(false);
        if (SceneTransitionManager.Instance.sceneTransitionAnimator)
        {
            SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Open");
        }
    }

    private void Update()
    {
        this.PauseHandler();
        this.TypingHandler();
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

        // play the win animation
        if (WinGraphicAnimtor)
            WinGraphicAnimtor.SetTrigger("Show");
        else
            Debug.LogWarning("WinGraphicAnimtor of LevelController is equal to NULL. Did you forget to set it?");
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

    void PauseHandler()
    {
        if (this._isPaused) {

                if (Input.GetKeyDown(this.pauseKey)) {
                Time.timeScale = 1;
                this._isPaused = false;
                this.pauseMenu.SetActive(false);
                this.FMOD.GetComponent<FMODMusicBattle>().Resume();
                EnablePlayerControl();
            }

        } else {
            
            if (Input.GetKeyDown(this.pauseKey)) {
                Time.timeScale = 0;
                this._isPaused = true;
                this.pauseMenu.SetActive(true);
                this.FMOD.GetComponent<FMODMusicBattle>().Pause();
                DisablePlayerControl();
            }
        }
    }

    void TypingHandler()
    {
        // if game gets laggy we can use an event handler to only check for this when a keydown event happens
        foreach(var key in this.validKeys){
            if(Input.GetKeyDown(key)) {
                if (key == KeyCode.Space) {
                    if (this.currPlayer == GameManager.Player.P1) {
                        player1Inventory.GetComponent<TileInventory>().ScrambleTiles();
                    } else if (this.currPlayer == GameManager.Player.P2) {
                        player2Inventory.GetComponent<TileInventory>().ScrambleTiles();
                    }
                } else if (key == KeyCode.Return) {
                    this.SubmitWord();
                } else {
                    string keyValue = key.ToString();
                    if (this.currPlayer == GameManager.Player.P1) {
                        player1Inventory.GetComponent<TileInventory>().TypeKey(keyValue);
                    }
                    else if (this.currPlayer == GameManager.Player.P2) {
                        player2Inventory.GetComponent<TileInventory>().TypeKey(keyValue);
                    }
                }
            }
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
            if(wordDmgAmt > 0)
            {

            }
        }

        //Triggers attack animation on successful attack
        if(wordDmgAmt > 0)
        {
                currPlayerG.GetComponent<Animator>().ResetTrigger("Attack");
                currPlayerG.GetComponent<Animator>().SetTrigger("Attack");
        }

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
