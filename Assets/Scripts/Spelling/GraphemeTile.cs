using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphemeTile : MonoBehaviour
{
    public GameObject textObject;
    public bool animable = false;

    ISpellingController spellingController;
    string grapheme;
    const float transitionSpeed = 0.1f;
    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;

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

    public string GetGrapheme()
    {
        return grapheme;
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, transitionSpeed);
    }
}
