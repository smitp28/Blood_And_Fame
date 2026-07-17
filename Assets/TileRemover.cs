using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq; // Required for the .Contains() method

public class TileRemover : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Tilemap targetTilemap;
    [Tooltip("Add all the different tile assets you want to erase from the map.")]
    public TileBase[] tilesToRemove;

    // This attribute allows you to run the method from the Inspector
    [ContextMenu("Delete All Matching Tiles")]
    public void DeleteSpecificTiles()
    {
        if (targetTilemap == null || tilesToRemove == null || tilesToRemove.Length == 0)
        {
            Debug.LogWarning("Please assign the Target Tilemap and add at least one tile to the array.");
            return;
        }

        // Get the bounding box of all tiles in the tilemap
        BoundsInt bounds = targetTilemap.cellBounds;
        int removedCount = 0;

        // Loop through every single cell position within those bounds
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int currentPosition = new Vector3Int(x, y, 0);
                TileBase currentTile = targetTilemap.GetTile(currentPosition);

                // If there is a tile here, and it matches ANY tile in our array, delete it
                if (currentTile != null && tilesToRemove.Contains(currentTile))
                {
                    targetTilemap.SetTile(currentPosition, null);
                    removedCount++;
                }
            }
        }

        Debug.Log($"Successfully removed {removedCount} matching tiles from '{targetTilemap.name}'.");
    }
}