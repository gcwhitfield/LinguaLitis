using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphemeTile : MonoBehaviour
{
    public ISpellingController spellingController;
    public bool animable = false;

    const float transitionSpeed = 0.1f;
    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;

    void OnMouseDown()
    {
        spellingController.ActivateTile(gameObject);
    }

    public void Position(Vector3 position)
    {
        targetPosition = position;
        if (!animable) {
            transform.position = position;
        }
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, transitionSpeed);
    }
}
