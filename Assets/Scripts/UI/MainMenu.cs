using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameplaySceneName;
    // Start is called before the first frame update
    public void goToGameplayScene()
    {
        SceneTransitionManager.Instance.TransitionToSceneInstant(gameplaySceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
