using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for a character's health
public class Health : MonoBehaviour
{
    public float curHp;
    public float maxHp;

    public GameObject healthBar;

    // void Update()
    // {
    // 	if( Input.GetKeyDown( KeyCode.RightArrow ) ) {
    //         BumpHp(5);
    //     }
    //     if( Input.GetKeyDown( KeyCode.LeftArrow ) ) {
    //         BumpHp(-5);
    //     }
    // }

    void Start()
    {
        this.maxHp = 50.0F;
        this.curHp = this.maxHp;
        this.UpdateHealthBar();
    }

    // bumpHp(5) will add 5 to health. bumpHp(-5) will subtract 5 from health
    public void BumpHp(float healthDelta)
    {
    	curHp += healthDelta;
        
        FMOD.Studio.EventInstance HealthChange;
        HealthChange = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/HealthChange");
        Debug.Log("Health Delta is " + healthDelta);
        HealthChange.setParameterByName("healthDelta", healthDelta);
        HealthChange.start();
        HealthChange.release();

        if (curHp < 0) {
            this.curHp = 0;
        } else if (curHp > maxHp) {
            this.curHp = maxHp;
        }

        this.UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        this.healthBar.GetComponent<HealthBar>().SetHealth((float)curHp / (float)this.maxHp);
    }

    public bool IsDead()
    {
        return curHp == 0;
    }
}
