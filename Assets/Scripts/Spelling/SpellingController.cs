using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellingController
{
    void ActivateTile(GraphemeTile tile);
}

public class SpellingController : ISpellingController
{
    public void ActivateTile(GraphemeTile tile)
    {
        Debug.Log("Tile activated");
    }
}
