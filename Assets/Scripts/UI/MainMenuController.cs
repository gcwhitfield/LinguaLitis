using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : UnitySingleton<MainMenuController>
{
    [Header("Start Button")]
    public string nextSceneName = "Tutorial";

    // plays a sound, transitions to next scene when "Start" button on the main menu screen gets pressed
    public void StartButtonPressed()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/UI/ButtonBig");
        evt.start();
        evt.release();
        SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
    }

    // plays sound when the cursor hovers over a main menu button
    public void OnButtonHovered()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/UI/ButtonHover");
        evt.start();
        evt.release();
    }

    // plays sound when any of the main menu buttons are pressed (other than the start button, which
    // has its own special sound)
    public void OnButtonPressed()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/UI/ButtonPress");
        evt.start();
        evt.release();
    }
}