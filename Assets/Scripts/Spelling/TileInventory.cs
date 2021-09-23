using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellingController
{
    void ActivateTile(GameObject tile);
}

public class LetterPool
{
    Dictionary<string, int> letterPoints = new Dictionary<string, int> {
        { "a", 1 }, { "b", 3 }, { "c", 2 }, { "d", 2 }, { "e", 1 }, { "f", 2 },
        { "g", 3 }, { "h", 2 }, { "i", 1 }, { "j", 4 }, { "k", 3 }, { "l", 2 },
        { "m", 2 }, { "n", 1 }, { "o", 1 }, { "p", 3 }, { "q", 4 }, { "r", 1 },
        { "s", 1 }, { "t", 1 }, { "u", 2 }, { "v", 3 }, { "w", 3 }, { "x", 4 },
        { "y", 3 }, { "z", 4 },
    };
    Dictionary<string, int> letterFrequencies = new Dictionary<string, int> {
        { "a", 10 }, { "b", 6 }, { "c", 7 }, { "d", 7 }, { "e", 10 }, { "f", 5 },
        { "g", 5 }, { "h", 4 }, { "i", 10 }, { "j", 1 }, { "k", 5 }, { "l", 4 },
        { "m", 5 }, { "n", 7 }, { "o", 8 }, { "p", 4 }, { "q", 1 }, { "r", 6 },
        { "s", 2 }, { "t", 10 }, { "u", 8 }, { "v", 3 }, { "w", 1 }, { "x", 1 },
        { "y", 2 }, { "z", 3 },
    };
    List<string> letterList;
    List<string> weightedLetterList = new List<string>();

    public LetterPool()
    {
        letterList = new List<string>(letterPoints.Keys);
        foreach (var ele in letterFrequencies) {
            for (int i = 0; i < ele.Value; ++i) {
                weightedLetterList.Add(ele.Key);
            }
        }
    }

    public string GetRandomLetter()
    {
        return weightedLetterList[Random.Range(0, weightedLetterList.Count)];
    }

    public int ScoreWord(List<string> letterList)
    {
        int score = 0;
        foreach (string letter in letterList) {
            score += letterPoints[letter];
        }
        return score;
    }
}

public class TileInventory : MonoBehaviour, ISpellingController
{
    public GameObject tilePrefab;
    public Vector3 stagedTilePosition;

    LetterPool graphemicInventory = new LetterPool();
    const int columnCount = 3;
    const int rowCount = 5;
    const float transitionSpeed = 0.5f;
    GameObject[,] tileTable = new GameObject[columnCount, rowCount];
    const float stagedTileSpacing = 1.125f;
    List<GameObject> stagedTiles = new List<GameObject>();

    void Start()
    {
        for (int y = 0; y < columnCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                var tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
                tile.transform.SetParent(gameObject.transform);
                tile.GetComponent<Tile>().Initialize(this, graphemicInventory.GetRandomLetter());
                tileTable[y, x] = tile;
            }
        }

        PositionTiles();

        foreach (GameObject tile in tileTable) {
            tile.GetComponent<Tile>().animable = true;
        }
    }

    public void ActivateTile(GameObject tile)
    {
        int index = stagedTiles.IndexOf(tile);
        if (index == -1) {
            stagedTiles.Add(tile);
        } else {
            stagedTiles.RemoveAt(index);
        }

        PositionTiles();

        var spelledLetters = new List<string>();
        foreach (GameObject staged in stagedTiles) {
            spelledLetters.Add(staged.GetComponent<Tile>().GetLetter());
        }
        var spelledString = string.Join("", spelledLetters);

        if (GetComponent<Lexicon>().Includes(spelledString)) {
            Debug.Log("Score of '" + spelledString + "' is " + graphemicInventory.ScoreWord(spelledLetters));
        }
    }

    void PositionTiles()
    {
        float width = transform.localScale.x;
        float height = transform.localScale.y;
        float horizontalSpacing = width / (rowCount - 1);
        float verticalSpacing = height / (columnCount - 1);
        Vector3 tableBottomLeft = transform.position - new Vector3(width, height, 0.0f) * 0.5f;
        for (int y = 0; y < columnCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                Vector3 position = tableBottomLeft + new Vector3(horizontalSpacing * x, verticalSpacing * y, 0.0f);
                GameObject tile = tileTable[y, x];
                tile.GetComponent<Tile>().Position(position);
            }
        }

        float stagedWidth = horizontalSpacing * (stagedTiles.Count - 1);
        Vector3 stagedLeft = stagedTilePosition - new Vector3(stagedWidth * 0.5f, 0.0f, 0.0f);
        for (int i = 0; i < stagedTiles.Count; ++i) {
            Vector3 position = stagedLeft + horizontalSpacing * new Vector3(i, 0.0f, 0.0f);
            GameObject tile = stagedTiles[i];
            tile.GetComponent<Tile>().Position(position);
        }
    }
}
