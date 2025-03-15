using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MineGrid Grid { get; set; }
    public Vector2Int GridPos { get; set; } = Vector2Int.zero;
    public bool IsMine { get; set; }

    public void Click()
    {
        if (IsMine) Grid.ExplodeMines();
        else Clear()
    }

    public void Clear()
    {
        int adjacentMines = 0;
        var enumerator = GetAdjacentTiles();
        while (enumerator.MoveNext())
        {
            var tile = enumerator.Current;
            if (tile.IsMine) adjacentMines++;
            else tile.Clear();
        }
    }

    public IEnumerator<Tile> GetAdjacentTiles()
    {
        for (int x = GridPos.x - 1; x < GridPos.x + 1; x++)
        {
            for (int y = GridPos.x - 1; y < GridPos.x + 1; y++)
            {
                if (Grid.Tiles[x] == null || Grid.Tiles[x][y] == null || Grid.Tiles[x][y] == this) continue;
                yield return Grid.Tiles[x][y];
            }
        }
    }
}
