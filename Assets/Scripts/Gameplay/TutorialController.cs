using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : UnitySingleton<TutorialController>
{
    [Tooltip("The name of the next scene to transition to")]
    public string nextScene;

    void Start()
    {
        if (SceneTransitionManager.Instance.sceneTransitionAnimator)
        {
            SceneTransitionManager.Instance.sceneTransitionAnimator.SetTrigger("Open");
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneTransitionManager.Instance.TransitionToScene(nextScene);
        }
    }
}
