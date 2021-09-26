using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TempPlsDeleteLater());
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator TempPlsDeleteLater()
    {
        yield return new WaitForSeconds(2.5F);
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(5);
        }
    }
    
    public void Attack()
    {
        // some other stuff too when you attack
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/wilhelm", GetComponent<Transform>().position);
    }
}
