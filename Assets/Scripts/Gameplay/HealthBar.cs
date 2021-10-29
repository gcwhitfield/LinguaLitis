using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public bool flipped = false;

    private GameObject mask;
    private Vector3 maskInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.mask = this.transform.Find("Mask").gameObject;
        this.maskInitialPosition = this.mask.transform.position;

        this.SetHealth(1.0f);
    }

    public void SetHealth(float proportion)
    {
        var healthBarWidth = 3.0625f;
        var dir = this.flipped ? Vector3.right : Vector3.left;
        this.mask.transform.position = this.maskInitialPosition + dir * (1.0f - proportion) * healthBarWidth;
    }
}
