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
    const float transitionSpeed = 0.5f;
    GameObject[,] tileTable = new GameObject[rowCount, rowCount];
    const float stagedTileSpacing = 1.125f;
    List<GameObject> stagedTiles = new List<GameObject>();

    void Start()
    {
        for (int y = 0; y < rowCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                var tile = Instantiate(graphemeTilePrefab, transform.position, Quaternion.identity);
                tile.GetComponent<GraphemeTile>().spellingController = this;
                tileTable[y, x] = tile;
                tile.transform.position = transform.position;
            }
        }

        PositionTiles();
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
    }

    void PositionTiles()
    {
        float width = transform.localScale.x;
        float spacing = width / (rowCount - 1);
        Vector3 tableBottomLeft = transform.position - new Vector3(width, width, 0.0f) * 0.5f;
        for (int y = 0; y < rowCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                Vector3 position = tableBottomLeft + spacing * new Vector3(x, y, 0.0f);
                GameObject tile = tileTable[y, x];
                tile.GetComponent<GraphemeTile>().targetPosition = position;
            }
        }

        float stagedWidth = spacing * (stagedTiles.Count - 1);
        Vector3 stagedLeft = stagedTilePosition - new Vector3(stagedWidth * 0.5f, 0.0f, 0.0f);
        for (int i = 0; i < stagedTiles.Count; ++i) {
            Vector3 position = stagedLeft + spacing * new Vector3(i, 0.0f, 0.0f);
            GameObject tile = stagedTiles[i];
            tile.GetComponent<GraphemeTile>().targetPosition = position;
        }
    }
}
