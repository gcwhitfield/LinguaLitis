using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphemeTile : MonoBehaviour
{
    public ISpellingController spellingController;
    public GameObject textObject;
    public bool animable = false;

    const float transitionSpeed = 0.1f;
    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;
    string grapheme;

    public void Initialize(ISpellingController setSpellingController, string setGrapheme)
    {
        spellingController = setSpellingController;
        grapheme = setGrapheme;
        textObject.GetComponent<TextMeshProUGUI>().text = grapheme.ToUpper();
    }

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
