using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : UnitySingleton<SceneTransitionManager>
{
    public Animator sceneTransitionAnimator;


    public void TransitionToScene(string sceneName)
    {
        StartCoroutine("PlaySceneTransitionAnimation", sceneName);
    }
    
    public void TransitionToSceneInstant(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator PlaySceneTransitionAnimation(string s)
    {
        if (sceneTransitionAnimator == null)
        {
            SceneManager.LoadScene(s);
        } else
        {
            // play the animation
            sceneTransitionAnimator.SetTrigger("Close");

            // wait for the animatino clip info to be ready
            while (sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
                yield return null;
            }

            // wait for the clip to complete before transitioning to the next scene
            float animTime = sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float t = 0;
            while (t < animTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            SceneManager.LoadScene(s);
        }
    }
}
