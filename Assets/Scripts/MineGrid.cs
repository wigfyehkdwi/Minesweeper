using System;
using UnityEngine;

public class MineGrid : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Area => Width * Height;
    public int MineCount { get; set; } = 10;

    public GameObject Tile;
    public Tile[][] Tiles = new Tile[0][];

    public const byte TileWidth = 16;
    public const byte TileHeight = 16;

    private RectTransform rectTransform;
    private int resWidth;
    private int resHeight;

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
        resWidth = 9 // Left border
                 + 8 // Right border
                 + TileWidth * Width; // Tiles (16x16)
        resHeight = 20 // Toolbar
                  + 52 // Top UI/border
                  + 8 // Bottom border
                  + TileHeight * Height; // Tiles (16x16)

        Debug.Log("Updating resolution to " + resWidth + "x" + resHeight);
        Screen.SetResolution(resWidth, resHeight, FullScreenMode.Windowed);
    }

    public void Reset()
    {
        foreach (var tileArray in Tiles) foreach (var tile in tileArray) Destroy(tile); // Remove the old tiles

        Tiles = new Tile[Width][];
        for (int x = 0; x < Width; x++)
        {
            Tiles[x] = new Tile[Height];
            for (int y = 0; y < Height; y++)
            {
                var pos = new Vector3(9 + x * TileWidth, 8 + (Height - y - 1) * TileHeight, 0);
                pos -= rectTransform.localPosition;
                pos += new Vector3(TileWidth / 2f, TileHeight / 2f, 0);
                pos -= new Vector3(resWidth / 2f, resHeight / 2f, 0);

                var tile = (GameObject)Instantiate(Tile, transform);
                tile.GetComponent<RectTransform>().localPosition = pos;

                var tilec = tile.GetComponent<Tile>();
                tilec.Grid = this;
                tilec.GridPos = new Vector2Int(x, y);
                Tiles[x][y] = tilec;
            }
        }
        AssignMines();
    }

    public void AssignMines()
    {
        int assignedMines = 0;
        int mineCount = Math.Min(MineCount, Area);
        while (assignedMines < mineCount)
        {
            int x = UnityEngine.Random.Range(0, Width);
            int y = UnityEngine.Random.Range(0, Height);

            var tile = Tiles[x][y];
            if (tile.IsMine) continue;

            tile.IsMine = true;
            assignedMines++;
        }
    }

    public void ExplodeMines()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var tile = Tiles[x][y];
                if (tile.IsMine) tile.Explode();
            }
        }
    }
}
