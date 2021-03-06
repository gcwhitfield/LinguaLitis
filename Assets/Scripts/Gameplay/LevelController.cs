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
    public Animator player1Animator;
    public Animator player2Animator;
    public GameObject player1Inventory;
    public GameObject player2Inventory;
    public GameObject pauseMenu;
    public GameObject FMOD;
    public GameObject RuneIcon1;
    public GameObject RuneIcon2;
    public Animator WinGraphicAnimtor;
    public GameObject RuneControllerObject;
    public GameObject turnIndicator;
    // while the game is waiting for the player to type a word, this is set to true.
    // Otherwise, set to false
    private bool _waitForWord = false;
    public bool _isPaused = false;
    public KeyCode pauseKey;
    private KeyCode[] validKeys = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, 
    KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, 
    KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Delete, KeyCode.Backspace, KeyCode.Space, KeyCode.Return};

    public enum GameState
    {
        PLAYING,
        WIN
    };

    public GameState state { get; private set; }

    private void Start()
    {
        currPlayer = GameManager.Player.P1;
        OnPlayerBeginTurn();
        this.pauseMenu.SetActive(false);
        if (SceneTransitionManager.Instance.sceneTransitionAnimator)
        {
            SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Open");
        }
        state = GameState.PLAYING;
    }

    private void Update()
    {
        if (state == GameState.PLAYING)
        {
            this.PauseHandler();
            this.TypingHandler();
        }
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

        // No more fun allowed >:D
        // This makes sure that players can't still spell words after the game
        // has ended.  Since the win screen covers up the tiles, you can't click
        // them to activate them but it is still possible using the keyboard and
        // perceptible from sound effects.
        this.player1Inventory.GetComponent<TileInventory>().isDisabled = true;
        this.player2Inventory.GetComponent<TileInventory>().isDisabled = true;

        // play the win animation
        if (WinGraphicAnimtor)
        {
            WinGraphicAnimtor.SetTrigger("Show");
        }
        else
        {
            Debug.LogWarning("WinGraphicAnimtor of LevelController is equal to NULL. Did you forget to set it?");
        }
        // play the win sound
        this.FMOD.GetComponent<FMODMusicBattle>().Pause();
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Victory");
        evt.start();
        evt.release();

        state = GameState.WIN;
    }

    void DisablePlayerControl()
    {
        if (this.currPlayer == GameManager.Player.P1) {
            this.player1Inventory.GetComponent<TileInventory>().isDisabled = true;
        } else {
            this.player2Inventory.GetComponent<TileInventory>().isDisabled = true;
        }
    }

    void EnablePlayerControl()
    {
        if (this.currPlayer == GameManager.Player.P1) {
            this.player1Inventory.GetComponent<TileInventory>().isDisabled = false;
        } else {
            this.player2Inventory.GetComponent<TileInventory>().isDisabled = false;
        }
    }

    // called from PauseHandler and from the OptionsButton event system
    public void UnPause()
    {
        Time.timeScale = 1;
        this._isPaused = false;
        this.pauseMenu.SetActive(false);
        this.FMOD.GetComponent<FMODMusicBattle>().Resume();
        EnablePlayerControl();
    }
    void Pause()
    {
        Time.timeScale = 0;
        this._isPaused = true;
        this.pauseMenu.SetActive(true);
        this.FMOD.GetComponent<FMODMusicBattle>().Pause();
        DisablePlayerControl();
    }

    // called from inside of Update
    void PauseHandler()
    {
        if (this._isPaused) {
                if (Input.GetKeyDown(this.pauseKey)) {
                UnPause();
            }
        } else {
            if (Input.GetKeyDown(this.pauseKey)) {
                Pause();
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

    // called when the player submits their word. Called by the SubmitButton event system
    public void SubmitWord()
    {
        GameObject currPlayerG; // the player whose turn it currently is
        GameObject oppPlayerG; // the opposite players
        Animator currPlayerAnimator;
        Animator oppPlayerAnimator;

        GameObject activePlayerInventory;
        if (currPlayer == GameManager.Player.P1)
        {
            currPlayerG = player1G;
            currPlayerAnimator = player1Animator;
            oppPlayerG = player2G;
            oppPlayerAnimator = player2Animator;
            activePlayerInventory = player1Inventory;
        }
        else
        {
            currPlayerG = player2G;
            currPlayerAnimator = player2Animator;
            oppPlayerG = player1G;
            oppPlayerAnimator = player1Animator;
            activePlayerInventory = player2Inventory;
        }
        int wordDmgAmt = activePlayerInventory.GetComponent<TileInventory>().ScoreWord();

        // If no valid word was spelled, then don't do anything at all.  Player must try again.
        if (wordDmgAmt == -1) {
            // TODO: It would be a good idea to have a visual + auditory indicator that it failed.
            // Otherwise it would feel like the button isn't detecting a click.
            return;
        }

        //Triggers attack animation on successful attack
        if (wordDmgAmt > 0) {
            currPlayerAnimator.ResetTrigger("Attack");
            currPlayerAnimator.SetTrigger("Attack");
        }

        // damage the opposite player
        Health oppHealth = oppPlayerG.GetComponent<Health>(); 
        // oppHealth.BumpHp(-wordDmgAmt);

        // queue rune effects
        // multiplied by 2
        RuneControllerObject.GetComponent<RuneController>().Turn(currPlayer, 2 * wordDmgAmt);

        if (oppHealth.IsDead())
        {
            OnPlayerDied(oppPlayerG);
            return; // break out of gameplay loop
        }

        // Clearing the tiles
        if (wordDmgAmt == 0) {
            activePlayerInventory.GetComponent<TileInventory>().RenewAllTiles();
        }
        activePlayerInventory.GetComponent<TileInventory>().ClearTiles();

        _waitForWord = false;
        ChangeTurn();
    }

    void ChangeTurn()
    {
        this.turnNumber += 1;
        OnPlayerEndTurn();

        if (currPlayer == GameManager.Player.P1) {
            currPlayer = GameManager.Player.P2;
            turnIndicator.GetComponent<TurnIndicator>().SetPlayer(false);
        } else {
            currPlayer = GameManager.Player.P1;
            turnIndicator.GetComponent<TurnIndicator>().SetPlayer(true);
        }

        OnPlayerBeginTurn();
    }

}
