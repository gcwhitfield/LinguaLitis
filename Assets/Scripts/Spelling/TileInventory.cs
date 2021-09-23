using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellingController
{
    void ActivateTile(GameObject tile);
}

public class TileInventory : MonoBehaviour, ISpellingController
{
    public GameObject tilePrefab;
    public Vector3 stagedTilePosition;
    public TextAsset wordList;

    Lexicon lexicon;
    const int columnCount = 3;
    const int rowCount = 5;
    const float transitionSpeed = 0.5f;
    GameObject[,] tileTable = new GameObject[columnCount, rowCount];
    const float stagedTileSpacing = 1.125f;
    List<GameObject> stagedTiles = new List<GameObject>();

    void Start()
    {
        this.lexicon = new Lexicon(wordList);

        for (int y = 0; y < columnCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                var tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
                tile.transform.SetParent(gameObject.transform);
                tile.GetComponent<Tile>().Initialize(this, lexicon.GetRandomLetter());
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

        var score = lexicon.ScoreWord(spelledLetters);
        if (score >= 0) {
            Debug.Log("Score of '" + spelledString + "' is " + lexicon.ScoreWord(spelledLetters));
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
