using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : UnitySingletonPersistant<SceneTransitionManager>
{
    public Animator sceneTransitionAnimator;

    public void TransitionToScene(Scene scene)
    {
        StartCoroutine("PlaySceneTransitionAnimation", scene);
    }

    IEnumerator PlaySceneTransitionAnimation(Scene s)
    {
        if (sceneTransitionAnimator == null)
        {
            SceneManager.LoadScene(s.name);
        } else
        {
            sceneTransitionAnimator.SetTrigger("Play");
            float animTime = sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float t = 0;
            while (t < animTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            SceneManager.LoadScene(s.name);
        }
    }
}
