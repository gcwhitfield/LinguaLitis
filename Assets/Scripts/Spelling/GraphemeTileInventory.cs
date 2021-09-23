using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellingController
{
    void ActivateTile(GameObject tile);
}

public class GraphemicInventory
{
    Dictionary<string, int> graphemePoints = new Dictionary<string, int> {
        { "a", 1 }, { "b", 3 }, { "c", 2 }, { "d", 2 }, { "e", 1 }, { "f", 2 },
        { "g", 3 }, { "h", 2 }, { "i", 1 }, { "j", 4 }, { "k", 3 }, { "l", 2 },
        { "m", 2 }, { "n", 1 }, { "o", 1 }, { "p", 3 }, { "q", 4 }, { "r", 1 },
        { "s", 1 }, { "t", 1 }, { "u", 2 }, { "v", 3 }, { "w", 3 }, { "x", 4 },
        { "y", 3 }, { "z", 4 },
    };
    Dictionary<string, int> graphemeFrequencies = new Dictionary<string, int> {
        { "a", 10 }, { "b", 6 }, { "c", 7 }, { "d", 7 }, { "e", 10 }, { "f", 5 },
        { "g", 5 }, { "h", 4 }, { "i", 10 }, { "j", 1 }, { "k", 5 }, { "l", 4 },
        { "m", 5 }, { "n", 7 }, { "o", 8 }, { "p", 4 }, { "q", 1 }, { "r", 6 },
        { "s", 2 }, { "t", 10 }, { "u", 8 }, { "v", 3 }, { "w", 1 }, { "x", 1 },
        { "y", 2 }, { "z", 3 },
    };
    List<string> graphemeList;
    List<string> weightedGraphemeList = new List<string>();

    public GraphemicInventory()
    {
        graphemeList = new List<string>(graphemePoints.Keys);
        foreach (var ele in graphemeFrequencies) {
            for (int i = 0; i < ele.Value; ++i) {
                weightedGraphemeList.Add(ele.Key);
            }
        }
    }

    public string GetRandomGrapheme()
    {
        return weightedGraphemeList[Random.Range(0, weightedGraphemeList.Count)];
    }

    public int ScoreWord(List<string> graphemeList)
    {
        int score = 0;
        foreach (string grapheme in graphemeList) {
            score += graphemePoints[grapheme];
        }
        return score;
    }
}

public class GraphemeTileInventory : MonoBehaviour, ISpellingController
{
    public GameObject graphemeTilePrefab;
    public Vector3 stagedTilePosition;

    GraphemicInventory graphemicInventory = new GraphemicInventory();
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
                var tile = Instantiate(graphemeTilePrefab, Vector3.zero, Quaternion.identity);
                tile.transform.SetParent(gameObject.transform);
                tile.GetComponent<GraphemeTile>().Initialize(this, graphemicInventory.GetRandomGrapheme());
                tileTable[y, x] = tile;
            }
        }

        PositionTiles();

        foreach (GameObject tile in tileTable) {
            tile.GetComponent<GraphemeTile>().animable = true;
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

        var spelledGraphemes = new List<string>();
        foreach (GameObject staged in stagedTiles) {
            spelledGraphemes.Add(staged.GetComponent<GraphemeTile>().GetGrapheme());
        }
        var spelledString = string.Join("", spelledGraphemes);

        if (GetComponent<Lexicon>().Includes(spelledString)) {
            Debug.Log("Score of '" + spelledString + "' is " + graphemicInventory.ScoreWord(spelledGraphemes));
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
                tile.GetComponent<GraphemeTile>().Position(position);
            }
        }

        float stagedWidth = horizontalSpacing * (stagedTiles.Count - 1);
        Vector3 stagedLeft = stagedTilePosition - new Vector3(stagedWidth * 0.5f, 0.0f, 0.0f);
        for (int i = 0; i < stagedTiles.Count; ++i) {
            Vector3 position = stagedLeft + horizontalSpacing * new Vector3(i, 0.0f, 0.0f);
            GameObject tile = stagedTiles[i];
            tile.GetComponent<GraphemeTile>().Position(position);
        }
    }
}
