using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullGuySounds : MonoBehaviour
{
    // TODO: There is a variable called "IsPlayer1" inside of the FMOD project. Right now, this code
    // does not set IsPlayer1. This code needs to be changed so that IsPlayer1 is set to 1 when
    // it is player 1's turn, and IsPlayer1 is set to 0 when it is player 2's turn

    //  this function is executed via an AnimationEvent during SkullGuy's attack animation
    public void PlayAttackSoundPart1()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/Boney M/BMAttack01");
        evt.setParameterByName("AttackStage", 1);
        evt.start();
        evt.release();
    }

    //  this function is executed via an AnimationEvent during SkullGuy's attack animation
    public void PlayAttackSoundPart2()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/Boney M/BMAttack01");
        evt.setParameterByName("AttackStage", 2);
        evt.start();
        evt.release();
    }

    //  this function is executed via an AnimationEvent during SkullGuy's attack animation
    public void PlayAttackSoundPart4()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/Boney M/BMAttack01");
        evt.setParameterByName("AttackStage", 4);
        evt.start();
        evt.release();
    }

    //  this function is executed via an AnimationEvent during SkullGuy's attack animation
    public void PlayAttackSoundPart3()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/Boney M/BMAttack01");
        evt.setParameterByName("AttackStage", 3);
        evt.start();
        evt.release();
    }

    //  this function is executed via an AnimationEvent during SkullGuy's attack animation
    public void PlayAttackSoundPart5()
    {
        FMOD.Studio.EventInstance evt;
        evt = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/Boney M/BMAttack01");
        evt.setParameterByName("AttackStage", 5);
        evt.start();
        evt.release();
    }
}
