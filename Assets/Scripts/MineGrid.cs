using System;
using UnityEngine;
using System.IO;
using System.Windows.Forms;

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

    public States State { get; set; }

    public double TimeSinceReset { get; set; }
    public UIManager UI;

    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Resize(9, 9);
    }

    // Update is called once per frame
    private void Update()
    {
        if (State == States.Play) TimeSinceReset += Time.deltaTime;
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        UI?.HandleResize();
        ResetGrid();
    }

    public void SetMode(Modes mode)
    {
        int width = 9;
        int height = 9;
        int mines = 10;

        switch (mode)
        {
            case Modes.Intermediate:
                width = height = 16;
                mines = 40;
                break;
            case Modes.Expert:
                width = 30;
                height = 16;
                mines = 99;
                break;
        }
        Mode = mode;
        MineCount = mines;
        Resize(width, height);
    }

    public void ResetGrid()
    {
        State = States.Play;
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
        State = States.Lose;

        for (int i = 0; i < TilesFlat.Length; i++)
        {
            var tile = TilesFlat[i];

            if (tile.IsMine && tile.State is not Tile.States.Flagged) tile.Explode();
            else if (tile.State is Tile.States.Flagged) tile.UpdateSprite(); // Show the incorrectly flagged sprite
        }
    }

    public void CheckWin()
    {
        if (FlaggedMines == MineCount) return; // fast check

        for (int i = 0; i < TilesFlat.Length; i++) // slow check
        {
            var tile = TilesFlat[i];
            if (!(tile.IsMine ? tile.State == Tile.States.Flagged : tile.IsClear)) return;
        }

        // well, we won
        State = States.Win;
        UI?.HandleWin();
    }

    public byte[] Serialize()
    {
        byte[] data = new byte[2 + 1 + (Mode == Modes.Custom ? 1 + 2 + 2 : 0) + (Tiles.Length + 1)/2];
        int i = 0;

        int timeSinceReset = (int)TimeSinceReset;
        data[i++] = (byte)timeSinceReset;
        data[i++] = (byte)((timeSinceReset >> 8) & 255);

        data[i++] = (byte)Mode;
        if (Mode == Modes.Custom)
        {
            data[i++] = (byte)MineCount;
            data[i++] = (byte)((Width >> 8) & 255);
            data[i++] = (byte)(Width & 255);
            data[i++] = (byte)((Height >> 8) & 255);
            data[i++] = (byte)(Height & 255);
        }

        for (int j = 0; j < TilesFlat.Length; j += 2)
        {
            int tilePair = TilesFlat[j].Serialize() << 4;
            if (j + 1 != TilesFlat.Length) tilePair |= TilesFlat[j + 1].Serialize();

            data[i++] = (byte)tilePair;
        }
        return data;
    }

    public void Deserialize(byte[] data)
    {
        int i = 0;

        int timeSinceReset = (data[i++] << 8) | data[i++];

        Mode = (Modes)data[i++];
        if (Mode == Modes.Custom)
        {
            MineCount = data[i++];
            Resize((data[i++] << 8) | data[i++], (data[i++] << 8) | data[i++]);
        }
        else
        {
            SetMode(Mode);
        }

        TimeSinceReset = timeSinceReset;

        for (int j = 0; j < TilesFlat.Length; j += 2)
        {
            byte tilePair = data[i++];

            TilesFlat[j].Deserialize((byte)(tilePair >> 4));
            if (j + 1 != TilesFlat.Length) TilesFlat[j + 1].Deserialize((byte)(tilePair & 15));
        }

        // Recalculate adjacents and FlaggedMines
        FlaggedMines = 0;
        for (int h = 0; h < TilesFlat.Length; h++)
        {
            var tile = TilesFlat[h];

            if (tile.IsMine && tile.State == Tile.States.Flagged) FlaggedMines++;

            var adjacentTiles = tile.GetAdjacentTiles();
            int adjacentMines = 0;
            foreach (var tile2 in adjacentTiles) if (tile2.IsMine) adjacentMines++;

            if (adjacentMines != 0)
            {
                tile.adjacentMineText.text = adjacentMines.ToString();
                tile.adjacentMineText.gameObject.SetActive(true);
            }
        }
    }

    public void SaveGame()
    {
        var dialog = new SaveFileDialog();
        dialog.RestoreDirectory = true;

        Stream stream;
        if (dialog.ShowDialog() == DialogResult.OK && (stream = dialog.OpenFile()) != null)
        {
            using (stream)
            {
                stream.Write(Serialize());
            }
        }
    }

    public void LoadGame()
    {
        var dialog = new OpenFileDialog();
        dialog.RestoreDirectory = true;

        Stream stream;
        if (dialog.ShowDialog() == DialogResult.OK && (stream = dialog.OpenFile()) != null)
        {
            using (stream)
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                Deserialize(data);
            }
        }
    }

    public enum Modes
    {
        Beginner,
        Intermediate,
        Expert,
        Custom
    }

    public enum States
    {
        Play,
        Win,
        Lose
    }
}
