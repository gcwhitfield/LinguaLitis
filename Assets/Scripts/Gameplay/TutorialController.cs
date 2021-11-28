using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : UnitySingleton<TutorialController>
{
    [Tooltip("The name of the next scene to transition to")]
    public string nextScene;
    public GameObject[] screens;
    int currScreenIndex = 0;

    void Start()
    {
        if (SceneTransitionManager.Instance.sceneTransitionAnimator)
        {
            SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Open");
        }
        ShowScreen();
    }

    // show the currently active tutorial screen
    void ShowScreen()
    {
        foreach (GameObject s in screens)
        {
            s.SetActive(false);
        }
        screens[currScreenIndex].SetActive(true);
    }

    void PlayPageFlipSound()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/UI/PageFlip");
        evt.start();
        evt.release();
    }

    void Update()
    {
        // listen for inputs from player. increment or decrement current screen based on keyboard input
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currScreenIndex++;
            if (currScreenIndex >= screens.Length)
            {
                currScreenIndex = screens.Length - 1;
            } else // only play the page flip if we are advancing to a valid screen
            {
                PlayPageFlipSound();
            }
            ShowScreen();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currScreenIndex--;
            if (currScreenIndex < 0)
            {
                currScreenIndex = 0;
            } else // only play the page flip if we are advancing to a valid screen
            {
                PlayPageFlipSound();
            }
            ShowScreen();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransitionManager.Instance.TransitionToScene(nextScene);
        }
    }
}
