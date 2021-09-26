using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Scene gameplayScene;
    // Start is called before the first frame update
    public void goToGameplayScene()
    {
        SceneTransitionManager.Instance.TransitionToScene(gameplayScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
