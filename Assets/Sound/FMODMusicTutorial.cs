using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODMusicTutorial : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Music;
    
    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Tutorial");
        Music.start();
        Music.release();
    }

    public void OnDestroy()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}