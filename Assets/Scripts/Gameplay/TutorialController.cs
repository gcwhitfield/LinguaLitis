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
        screens[currScreenIndex].SetActive(true);
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

    void Update()
    {
        // listen for inputs from player. increment or decrement current screen based on keyboard input
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currScreenIndex++;
            ShowScreen();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currScreenIndex--;
            ShowScreen();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneTransitionManager.Instance.TransitionToScene(nextScene);
        }
    }
}
