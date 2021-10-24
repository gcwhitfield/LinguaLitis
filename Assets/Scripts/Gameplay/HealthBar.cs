using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private GameObject mask;
    private Vector3 maskInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.mask = this.transform.Find("Mask").gameObject;
        this.maskInitialPosition = this.mask.transform.position;

        this.SetHealth(0.01f);
    }

    public void SetHealth(float proportion)
    {
        var healthBarWidth = 3.0625f;
        this.mask.transform.position = this.maskInitialPosition + Vector3.left * (1.0f - proportion) * healthBarWidth;
    }
}
