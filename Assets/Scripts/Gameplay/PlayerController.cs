/* SFX Guide

To play a oneshot, call

FMODUnity.RuntimeManager.PlayOneShot("event:PATHTOEVENT", GetComponent<Transform>().position);

where you replace PATHTOEVENT with the path to the event.


Current SFX:
event:/SFX/wilhelm -- placeholder SFX


*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Attack()
    {
        // some other stuff too when you attack
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/wilhelm", GetComponent<Transform>().position);
    }
}
