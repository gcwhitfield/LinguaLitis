using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexicon : MonoBehaviour
{
    Dictionary<string, int> letterPoints = new Dictionary<string, int> {
        { "a", 1 }, { "b", 3 }, { "c", 2 }, { "d", 2 }, { "e", 1 }, { "f", 2 },
        { "g", 3 }, { "h", 2 }, { "i", 1 }, { "j", 4 }, { "k", 3 }, { "l", 2 },
        { "m", 2 }, { "n", 1 }, { "o", 1 }, { "p", 3 }, { "q", 4 }, { "r", 1 },
        { "s", 1 }, { "t", 1 }, { "u", 2 }, { "v", 3 }, { "w", 3 }, { "x", 4 },
        { "y", 3 }, { "z", 4 },
    };

    string[] wordList;
    HashSet<string> wordSet;

    public Lexicon(TextAsset wordListTextAsset)
    {
        string[] separators = { "\r\n", "\n" };
        this.wordList = wordListTextAsset.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        this.wordSet = new HashSet<string>(this.wordList);
        Debug.Log("Loaded " + this.wordSet.Count + " words into vocabulary");
    }

    public string GetRandomLetter()
    {
        var randomWord = this.wordList[Random.Range(0, this.wordList.Length)];
        var randomLetter = randomWord[Random.Range(0, randomWord.Length)];
        return randomLetter.ToString();
    }

    // Returns -1 if the input is not a valid word, or otherwise a nonnegative number equal to the score.
    public int ScoreWord(List<string> word)
    {
        if (!this.wordSet.Contains(string.Join("", word))) {
            return -1;
        }

        int score = 0;
        foreach (string letter in word) {
            score += this.letterPoints[letter];
        }
        return score;
    }
}
