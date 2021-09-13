using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellingController
{
    void ActivateTile(GameObject tile);
}

public class GraphemeTileInventory : MonoBehaviour, ISpellingController
{
    public GameObject graphemeTilePrefab;
    public Vector3 stagedTilePosition;

    const int rowCount = 4;
    GameObject[,] tileTable = new GameObject[rowCount, rowCount];
    const float stagedTileSpacing = 1.125f;
    List<GameObject> stagedTiles = new List<GameObject>();

    void Start()
    {
        float width = transform.localScale.x;
        float spacing = width / (rowCount - 1);
        Vector3 bottomLeft = transform.position - new Vector3(width, width, 0.0f) * 0.5f;
        for (int y = 0; y < rowCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                Vector3 position = bottomLeft + spacing * new Vector3(x, y, 0.0f);
                var tile = Instantiate(graphemeTilePrefab, position, Quaternion.identity);
                tile.GetComponent<GraphemeTile>().spellingController = this;
                tileTable[y, x] = tile;
            }
        }
    }

    public void ActivateTile(GameObject tile)
    {
        stagedTiles.Add(tile);
        ArrangeStagedTiles();
    }

    void ArrangeStagedTiles()
    {
        float width = stagedTileSpacing * (stagedTiles.Count - 1);
        Vector3 left = stagedTilePosition - new Vector3(width * 0.5f, 0.0f, 0.0f);

        for (int i = 0; i < stagedTiles.Count; ++i) {
            GameObject tile = stagedTiles[i];
            tile.transform.position = left + stagedTileSpacing * new Vector3(i, 0.0f, 0.0f);
        }
    }
}
