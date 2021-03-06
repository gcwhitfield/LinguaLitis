using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface ISpellingController
{
    void ActivateTile(GameObject tile);
}

public class TileInventory : MonoBehaviour, ISpellingController
{
    public GameObject tilePrefab;
    public GameObject submitButtonText;
    public Vector3 stagedTilePosition;
    public TextAsset wordList;
    public bool isDisabled = true;

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

    public void RenewAllTiles()
    {
        foreach (GameObject tile in this.tileTable) {
            Destroy(tile);
        }

        for (int y = 0; y < TileInventory.columnCount; ++y) {
            for (int x = 0; x < TileInventory.rowCount; ++x) {
                var tile = Instantiate(this.tilePrefab, Vector3.zero, Quaternion.identity);
                tile.transform.SetParent(this.gameObject.transform);
                tile.GetComponent<Tile>().Initialize(this, lexicon.GetRandomLetter());
                this.tileTable[y, x] = tile;
            }
        }

        this.PositionTiles();
    }

    public void ActivateTile(GameObject tile)
    {
        if (this.isDisabled) {
            return;
        }

        int index = this.stagedTiles.IndexOf(tile);
        int WordPower = this.stagedTiles.Count;
        
        // Add tile from inventory to stage
        if (index == -1) {
            this.stagedTiles.Add(tile);
            WordPower++;
        }
        // Return tile from stage to inventory
        else {
            this.stagedTiles.RemoveAt(index);
            WordPower--;
        }
        
        if (WordPower < 0) {WordPower = 0;}
        else if (WordPower > 15) {WordPower = 15;}
        Debug.Log("Word Power is " + WordPower);
        
        FMOD.Studio.EventInstance LetterStaging;
        LetterStaging = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/LetterStaging");
        LetterStaging.setParameterByName("WordPower", WordPower);
        LetterStaging.start();
        LetterStaging.release();

        this.PositionTiles();
    }

    public int ScoreWord()
    {
        var spelledLetters = new List<string>();
        foreach (GameObject staged in this.stagedTiles) {
            spelledLetters.Add(staged.GetComponent<Tile>().GetLetter());
        }
        var spelledString = string.Join("", spelledLetters);

        if (spelledLetters.Count == 0) { return 0; }

        return this.lexicon.ScoreWord(spelledLetters);
    }

    private GameObject SearchTileValues(string key) 
    {
        for (int y = 0; y < columnCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                GameObject tile = this.tileTable[y, x];
                if (tile.GetComponent<Tile>().GetLetter().ToUpper() == key && !this.stagedTiles.Contains(tile)) {
                    return tile;
                }
            }
        }
        return null;
    }

    public void TypeKey(string key)
    {
        GameObject selectedTile;
        if ((key == "Backspace" || key == "Delete") && this.stagedTiles.Count > 0) {
            selectedTile = this.stagedTiles[this.stagedTiles.Count - 1];
        } else {
            selectedTile = this.SearchTileValues(key);
        }
        if (!(selectedTile is null)) {
            this.ActivateTile(selectedTile);
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


        int score = ScoreWord();
        string text = "Submit";
        if (score == 0) {
            text = "Pass";
        } else if (score == -1) {
            text = ". . .";
        }
        submitButtonText.GetComponent<TextMeshProUGUI>().SetText(text);
    }

    public void ScrambleTiles()
    {
        for (int y = 0; y < columnCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                var tile1 = this.tileTable[y, x];
                int randY = Random.Range(0, columnCount);
                int randX = Random.Range(0, rowCount);
                this.tileTable[y, x] = tileTable[randY, randX];
                this.tileTable[randY, randX] = tile1;
            }
        }
        this.PositionTiles();
    }

    public void ClearTiles() 
    {
        for(int t = 0; t < this.stagedTiles.Count; t++) {
            for(int row = 0; row < 5; row++) {
                for(int col = 0; col < 3; col++) {
                    if (this.tileTable[col, row].GetInstanceID() == this.stagedTiles[t].GetInstanceID()) {
                        Destroy(this.tileTable[col, row]);
                        this.tileTable[col, row] = Instantiate(this.tilePrefab, Vector3.zero, Quaternion.identity);
                        this.tileTable[col, row].transform.SetParent(this.gameObject.transform);
                        this.tileTable[col, row].GetComponent<Tile>().Initialize(this, this.lexicon.GetRandomLetter());
                    }
                }
            }
            Destroy(this.stagedTiles[t]);
            this.stagedTiles[t] = null;
        }
            for (int i = this.stagedTiles.Count - 1; i >= 0; i--) {
                if (this.stagedTiles[i] == null) {
                    this.stagedTiles.RemoveAt(i);
                }
            }
            foreach (GameObject tile in this.tileTable) {
                tile.GetComponent<Tile>().animable = true; 
                }
            // PositionTiles();
            ScrambleTiles();
    }

}
