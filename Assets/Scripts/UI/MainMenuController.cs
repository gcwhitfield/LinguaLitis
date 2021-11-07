using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : UnitySingleton<MainMenuController>
{
    [Header("Start Button")]
    public string gameplaySceneName = "Gameplay";

    // called when the "Start" button on the main menu screen gets pressed
    public void StartButtonPressed()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/UI/ButtonBig");
        evt.start();
        evt.release();
        SceneTransitionManager.Instance.TransitionToScene(gameplaySceneName);
    }
}