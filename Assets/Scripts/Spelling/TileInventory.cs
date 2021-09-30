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
    public bool isDisabled = true;
    public int wordScore = 0;

    Lexicon lexicon;
    const int columnCount = 3;
    const int rowCount = 5;
    GameObject[,] tileTable = new GameObject[columnCount, rowCount];
    List<GameObject> stagedTiles = new List<GameObject>();

    void Start()
    {
        this.lexicon = this.gameObject.AddComponent<Lexicon>();
        this.lexicon.Initialize(wordList);

        for (int y = 0; y < TileInventory.columnCount; ++y) {
            for (int x = 0; x < TileInventory.rowCount; ++x) {
                var tile = Instantiate(this.tilePrefab, Vector3.zero, Quaternion.identity);
                tile.transform.SetParent(this.gameObject.transform);
                tile.GetComponent<Tile>().Initialize(this, lexicon.GetRandomLetter());
                this.tileTable[y, x] = tile;
            }
        }

        this.PositionTiles();

        foreach (GameObject tile in this.tileTable) {
            tile.GetComponent<Tile>().animable = true;
        }
    }

    public void ActivateTile(GameObject tile)
    {
        if (this.isDisabled) {
            return;
        }

        int index = this.stagedTiles.IndexOf(tile);
        if (index == -1) {
            this.stagedTiles.Add(tile);
        } else {
            this.stagedTiles.RemoveAt(index);
        }

        this.PositionTiles();

        var spelledLetters = new List<string>();
        foreach (GameObject staged in this.stagedTiles) {
            spelledLetters.Add(staged.GetComponent<Tile>().GetLetter());
        }
        var spelledString = string.Join("", spelledLetters);

        var score = this.lexicon.ScoreWord(spelledLetters);
        if (score >= 0) {
            wordScore = score;
            Debug.Log("Score of '" + spelledString + "' is " + this.lexicon.ScoreWord(spelledLetters));
        }
    }

    void PositionTiles()
    {
        float width = this.transform.localScale.x;
        float height = this.transform.localScale.y;
        float horizontalSpacing = width / (TileInventory.rowCount - 1);
        float verticalSpacing = height / (TileInventory.columnCount - 1);
        Vector3 tableBottomLeft = this.transform.position - new Vector3(width, height, 0.0f) * 0.5f;
        for (int y = 0; y < TileInventory.columnCount; ++y) {
            for (int x = 0; x < TileInventory.rowCount; ++x) {
                Vector3 position = tableBottomLeft + new Vector3(horizontalSpacing * x, verticalSpacing * y, 0.0f);
                GameObject tile = this.tileTable[y, x];
                tile.GetComponent<Tile>().Position(position);
            }
        }

        float stagedWidth = horizontalSpacing * (this.stagedTiles.Count - 1);
        Vector3 stagedLeft = this.stagedTilePosition - new Vector3(stagedWidth * 0.5f, 0.0f, 0.0f);
        for (int i = 0; i < this.stagedTiles.Count; ++i) {
            Vector3 position = stagedLeft + horizontalSpacing * new Vector3(i, 0.0f, 0.0f);
            GameObject tile = this.stagedTiles[i];
            tile.GetComponent<Tile>().Position(position);
        }
    }
}
