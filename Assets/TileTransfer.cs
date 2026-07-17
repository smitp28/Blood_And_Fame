using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq; // Required for the .Contains() method

public class TileTransfer : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Tilemap sourceTilemap;
    public Tilemap destinationTilemap;

    [Tooltip("Add the specific tile assets you want to cut and move.")]
    public TileBase[] tilesToCut;

    [ContextMenu("Cut Specific Tiles to New Tilemap")]
    public void CutSpecificTiles()
    {
        // Ensure all references are set
        if (sourceTilemap == null || destinationTilemap == null)
        {
            Debug.LogWarning("Please assign both the Source and Destination Tilemaps in the inspector.");
            return;
        }

        if (tilesToCut == null || tilesToCut.Length == 0)
        {
            Debug.LogWarning("Please add at least one tile to the array to cut.");
            return;
        }

        BoundsInt bounds = sourceTilemap.cellBounds;
        int movedCount = 0;

        // Loop through the entire grid within the source bounds
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int currentPosition = new Vector3Int(x, y, 0);

                // Get the tile at this coordinate
                TileBase currentTile = sourceTilemap.GetTile(currentPosition);

                // If a tile exists here AND it is inside our array of target tiles
                if (currentTile != null && tilesToCut.Contains(currentTile))
                {
                    // 1. Copy the exact tile to the destination tilemap
                    destinationTilemap.SetTile(currentPosition, currentTile);

                    // 2. Erase the tile from the original source tilemap
                    sourceTilemap.SetTile(currentPosition, null);

                    movedCount++;
                }
            }
        }

        Debug.Log($"Successfully cut and moved {movedCount} specific tiles from '{sourceTilemap.name}' to '{destinationTilemap.name}'.");
    }
}