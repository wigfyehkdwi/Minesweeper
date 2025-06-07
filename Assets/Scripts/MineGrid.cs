using System;
using UnityEngine;

public class MineGrid : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Area => Width * Height;
    public Modes Mode { get; set; }
    public int MineCount { get; set; } = 10;
    public int FlaggedMines { get; internal set; } = 0; // Should ONLY be set within Tile!

    public GameObject TilePrefab;
    public Tile[][] Tiles = new Tile[0][];
    public Tile[] TilesFlat;

    public const byte TileWidth = 16;
    public const byte TileHeight = 16;

    private RectTransform rectTransform;
    private int resWidth;
    private int resHeight;

    public bool GameEnded { get; set; }

    public double TimeSinceReset { get; set; }
    public UIManager UI;

    public const int HeaderSize = 2 + 2; // width + height

    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Resize(9, 9);
    }

    // Update is called once per frame
    private void Update()
    {
        TimeSinceReset += Time.deltaTime;
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        UI?.HandleResize();
        ResetGrid();
    }

    public void ResetGrid()
    {
        GameEnded = false;
        FlaggedMines = 0;
        TimeSinceReset = 0;

        foreach (var tileArray in Tiles) foreach (var tile in tileArray) Destroy(tile.gameObject); // Remove the old tiles

        Tiles = new Tile[Width][];
        TilesFlat = new Tile[Area];
        int i = 0;
        for (int x = 0; x < Width; x++)
        {
            Tiles[x] = new Tile[Height];
            for (int y = 0; y < Height; y++)
            {
                var pos = new Vector3(x * TileWidth, (Height - y - 1) * TileHeight, 0);

                var tile = Instantiate(TilePrefab, transform);
                tile.GetComponent<RectTransform>().localPosition = pos;

                var tilec = tile.GetComponent<Tile>();
                tilec.Grid = this;
                tilec.GridPos = new Vector2Int(x, y);
                Tiles[x][y] = tilec;
                TilesFlat[i++] = tilec;
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
        GameEnded = true;

        for (int i = 0; i < TilesFlat.Length; i++)
        {
            var tile = TilesFlat[i];

            if (tile.IsMine && tile.State is not Tile.States.Flagged) tile.Explode();
            else if (tile.State is Tile.States.Flagged) tile.UpdateSprite(); // Show the incorrectly flagged sprite
        }
    }

    public byte[] Serialize()
    {
        byte[] data = new byte[HeaderSize + (Tiles.Length + 1)/2];
        for (int i = 0; i < TilesFlat.Length; i += 2)
        {
            byte tilePair = TilesFlat[i].Serialize() << 4;
            if (i + 1 != TilesFlat.Length) tilePair |= TilesFlat[i + 1].Serialize();

            data[i / 2 + HeaderSize] = tilePair;
        }
        return data;
    }

    public void Deserialize(byte[] data)
    {
        Resize((data[0] << 8) | data[1], (data[2] << 8) | data[3]);

        for (int i = 0; i < TilesFlat.Length; i += 2)
        {
            byte tilePair = data[i/2 + HeaderSize];

            TilesFlat[i].Deserialize((byte)(tilePair >> 4));
            if (i + 1 != TilesFlat.Length) TilesFlat[i + 1].Deserialize((byte)(tilePair & 15));
        }
    }

    public enum Modes
    {
        Beginner,
        Intermediate,
        Expert,
        Custom
    }
}
