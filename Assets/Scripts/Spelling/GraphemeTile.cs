using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphemeTile : MonoBehaviour
{
    public ISpellingController spellingController;
    public Vector3 targetPosition;

    const float transitionSpeed = 0.1f;
    Vector3 velocity = Vector3.zero;

    void OnMouseDown()
    {
        spellingController.ActivateTile(gameObject);
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, transitionSpeed);
    }
}
