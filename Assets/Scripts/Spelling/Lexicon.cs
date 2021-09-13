using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexicon : MonoBehaviour
{
    public TextAsset wordListText;

    HashSet<string> wordSet;

    void Start()
    {
        string[] separators = { "\r\n" };
        wordSet = new HashSet<string>(wordListText.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries));
        Debug.Log("Loaded " + wordSet.Count + " words into vocabulary");

        Debug.Log("Does the set contain ''? " + Includes("") + " (should be False)");
        Debug.Log("Does the set contain 'apples'? " + Includes("apples") + " (should be True)");
        Debug.Log("Does the set contain 'wertwe'? " + Includes("wertwe") + " (should be False)");
    }

    public bool Includes(string word)
    {
        return wordSet.Contains(word);
    }
}
