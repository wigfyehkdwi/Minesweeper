using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public MineGrid Grid { get; set; }
    public Vector2Int GridPos { get; set; } = Vector2Int.zero;
    public bool IsMine { get; set; }
    public bool IsClear { get; set; }
    public bool IsFlagged { get; set; }

    public Sprite TileSprite;
    public Sprite ClearTileSprite;
    public Sprite MineTileSprite;
    public Sprite FlaggedTileSprite;
    public Sprite UnknownTileSprite;
    public Text adjacentMineText;

    private Image _image;

    public void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = TileSprite;
    }

    public void Click()
    {
        if (IsMine) Grid.ExplodeMines();
        else if (!IsClear) Clear();
    }

    public void Clear()
    {
        IsClear = true;
        _image.sprite = ClearTileSprite;

        var adjacentTiles = GetAdjacentTiles();
        int adjacentMines = 0;
        foreach (var tile in adjacentTiles) if (tile.IsMine) adjacentMines++;

        if (adjacentMines == 0)
        {
            foreach (var tile in adjacentTiles) if (!tile.IsClear) tile.Clear();
        }
        else
        {
            adjacentMineText.text = adjacentMines.ToString();
            adjacentMineText.gameObject.SetActive(true);
        }
    }

    public void Explode()
    {
        _image.sprite = MineTileSprite;
    }

    public List<Tile> GetAdjacentTiles()
    {
        var list = new List<Tile>();
        for (int x = Math.Max(GridPos.x - 1, 0); x <= Math.Min(GridPos.x + 1, Grid.Width - 1); x++)
        {
            for (int y = Math.Max(GridPos.y - 1, 0); y <= Math.Min(GridPos.y + 1, Grid.Height - 1); y++)
            {
                if (Grid.Tiles[x][y] == this) continue;
                list.Add(Grid.Tiles[x][y]);
            }
        }
        return list;
    }
}
