using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellingController
{
    void ActivateTile(GraphemeTile tile);
}

public class GraphemeTileInventory : MonoBehaviour, ISpellingController
{
    public GameObject graphemeTilePrefab;
    public Vector3 graphemeTileStagingPosition;

    const int rowCount = 4;
    GameObject[,] tiles = new GameObject[rowCount, rowCount];

    void Start()
    {
        float width = transform.localScale.x;
        float spacing = width / (rowCount - 1);
        Vector3 bottomLeft = transform.position - new Vector3(width, width, 0.0f) * 0.5f;
        for (int y = 0; y < rowCount; ++y) {
            for (int x = 0; x < rowCount; ++x) {
                Vector3 position = bottomLeft + spacing * new Vector3(x, y, 0.0f);
                var t = Instantiate(graphemeTilePrefab, position, Quaternion.identity);
                t.GetComponent<GraphemeTile>().spellingController = this;
                tiles[y, x] = t;
            }
        }
    }

    public void ActivateTile(GraphemeTile tile)
    {
        Debug.Log("Tile activated");
        tile.transform.position = graphemeTileStagingPosition;
    }
}
