using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for a character's health
public class Health : MonoBehaviour
{
	public int maxHp = 50;
	public int curHp = 50;

    public Slider healthbar;

    void Start()
    {
        healthbar.maxValue = maxHp;
        healthbar.value = curHp;
    }

    // void Update()
    // {
    // 	if( Input.GetKeyDown( KeyCode.RightArrow ) ) {
    //         BumpHp(5);
    //     }
    //     if( Input.GetKeyDown( KeyCode.LeftArrow ) ) {
    //         BumpHp(-5);
    //     }
    // }

    // bumpHp(5) will add 5 to health. bumpHp(-5) will subtract 5 from health
    public void BumpHp( int offset ) {
    	curHp += offset;

    	if (curHp < 0) {
    		curHp = 0;
    	} else if (curHp > maxHp) {
    		curHp = maxHp;
    	}

    	healthbar.value = curHp;
    }

    public bool IsDead()
    {
        return curHp == 0;
    }
}
