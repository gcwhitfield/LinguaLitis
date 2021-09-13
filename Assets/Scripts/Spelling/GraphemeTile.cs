using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphemeTile : MonoBehaviour
{
    public ISpellingController spellingController;

    void OnMouseDown()
    {
        spellingController.ActivateTile(gameObject);
    }
}
