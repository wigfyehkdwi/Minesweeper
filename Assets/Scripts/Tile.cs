using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public MineGrid Grid { get; set; }
    public Vector2Int GridPos { get; set; } = Vector2Int.zero;
    public bool IsMine { get; set; }
    private States _state;
    public States State
    {
        get => _state;
        set
        {
            if (value is States.Flagged) Grid.FlaggedMines++;
            else if (_state is States.Flagged) Grid.FlaggedMines--;

            _state = value;
            UpdateSprite();
        }
    }
    public bool IsClearable => State is States.Unchecked or States.Unknown;
    public bool IsClear => State is States.Clear or States.Exploded;

    public Sprite TileSprite;
    public Sprite FlaggedTileSprite;
    public Sprite UnknownTileSprite;
    public Sprite ClearTileSprite;
    public Sprite MineTileSprite;
    public Sprite FalseMineTileSprite;

    public Text adjacentMineText;
    private Image _image;

    public void Awake()
    {
        _image = GetComponent<Image>();
        UpdateSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Grid.GameEnded) return;

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (IsClearable)
                {
                    if (IsMine) Grid.ExplodeMines();
                    else Clear();
                }
                break;
            case PointerEventData.InputButton.Right:
                if (!IsClear) AlterState();
                break;
        }
    }

    public void Clear()
    {
        State = States.Clear;

        var adjacentTiles = GetAdjacentTiles();
        int adjacentMines = 0;
        foreach (var tile in adjacentTiles) if (tile.IsMine) adjacentMines++;

        if (adjacentMines == 0)
        {
            foreach (var tile in adjacentTiles)
            {
                if (tile.IsClearable)
                {
                    tile.Clear();
                }
            }
        }
        else
        {
            adjacentMineText.text = adjacentMines.ToString();
            adjacentMineText.gameObject.SetActive(true);
        }
    }

    public void Explode()
    {
        State = States.Exploded;
    }

    public void AlterState()
    {
        State = (States)(((int)State + 1) % 3);
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

    public void UpdateSprite()
    {
        Sprite sprite = null;
        switch (State)
        {
            case States.Unchecked:
                sprite = TileSprite;
                break;
            case States.Flagged:
                sprite = (IsMine || !Grid.GameEnded) ? FlaggedTileSprite : FalseMineTileSprite;
                break;
            case States.Unknown:
                sprite = UnknownTileSprite;
                break;
            case States.Clear:
                sprite = ClearTileSprite;
                break;
            case States.Exploded:
                sprite = MineTileSprite;
                break;
        }

        if (sprite != null) _image.sprite = sprite;
    }

    public enum States
    {
        Unchecked,
        Flagged,
        Unknown,
        Clear,
        Exploded
    }
}
