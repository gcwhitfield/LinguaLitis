using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public GameObject textObject;
    public bool animable = false;

    ISpellingController spellingController;
    string letter;
    const float transitionSpeed = 0.1f;
    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;

    public void Initialize(ISpellingController spellingController, string letter)
    {
        this.spellingController = spellingController;
        this.letter = letter;
        this.textObject.GetComponent<TextMeshProUGUI>().text = this.letter.ToUpper();
    }

    void OnMouseDown()
    {
        this.spellingController.ActivateTile(this.gameObject);
    }

    public void Position(Vector3 position)
    {
        this.targetPosition = position;
        if (!this.animable) {
            this.transform.position = position;
        }
    }

    public string GetLetter()
    {
        return this.letter;
    }

    void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, this.targetPosition, ref this.velocity, Tile.transitionSpeed);
    }
}
