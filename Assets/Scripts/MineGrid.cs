using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGrid : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Area => Width * Height;

    public GameObject Tile;
    public GameObject[][] Tiles = new GameObject[0][];

    public const byte TileWidth = 16;
    public const byte TileHeight = 16;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Resize(9, 9);
    }

    // Update is called once per frame
    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        UpdateResolution();
        Reset();
    }

    public void UpdateResolution()
    {
        int resWidth = 9 // Left border
                     + 8 // Right border
                     + TileWidth * Width; // Tiles (16x16)
        int resHeight = 20 // Toolbar
                      + 52 // Top UI/border
                      + 8 // Bottom border
                      + TileHeight * Height; // Tiles (16x16)

        Debug.Log("Updating resolution to " + resWidth + "x" + resHeight);
        Screen.SetResolution(resWidth, resHeight, FullScreenMode.Windowed);
    }

    public void Reset()
    {
        foreach (var tileArray in Tiles) foreach (var tile in tileArray) Destroy(tile); // Remove the old tiles

        Tiles = new GameObject[Width][];
        for (int x = 0; x < Width; x++)
        {
            Tiles[x] = new GameObject[Height];
            for (int y = 0; y < Height; y++)
            {
                var pos = new Vector3(9 + x * TileWidth, 8 + (Height - y - 1) * TileHeight, 0);
                pos -= rectTransform.localPosition;
                pos += new Vector3(TileWidth / 2f, TileHeight / 2f, 0);
                pos -= new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

                Debug.Log("Placing grid tile @ (" + pos.x + ", " + pos.y + ") for grid pos (" + x + ", " + y + ")");

                var tile = (GameObject)Instantiate(Tile, transform);
                tile.GetComponent<RectTransform>().localPosition = pos;


                tile.GetComponent<Tile>().GridPos = new Vector2Int(x, y);
                Tiles[x][y] = tile;
            }
        }
    }
}
