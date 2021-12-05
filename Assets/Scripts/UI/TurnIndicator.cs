using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnIndicator : MonoBehaviour
{
    Vector3 destination;
    const float transitionSpeed = 0.0625f;
    Vector3 velocity = Vector3.zero;

    void Start()
    {
        this.SetPlayer(true);
        this.transform.position = this.destination;
    }

    void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, this.destination, ref this.velocity, TurnIndicator.transitionSpeed);
    }

    public void SetPlayer(bool player1)
    {
        this.destination = new Vector3((player1 ? -1 : 1) * 4.25f, 2.75f, 0.0f);
    }
}
