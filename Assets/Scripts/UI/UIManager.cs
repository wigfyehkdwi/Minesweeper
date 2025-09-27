using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MineGrid Grid;

    // Counters
    public Text RemainingMinesText;
    public Text TimerText;

    // New game button management
    public Button NewGameButton;
    public Sprite NewGameSprite;
    public Sprite NewGameDeadSprite;
    public Sprite NewGameWonSprite;

    public RectTransform UITransform;
    public RectTransform CountersTransform;
    public RectTransform MenuBarTransform;

    // winning
    public Window FastestTime;

    public Canvas Canvas;
    public float UIScale = 3;

    private void Update()
    {
        // Update timers
        RemainingMinesText.text = Format(Grid.MineCount - Grid.FlaggedMines);
        if (Grid.State == MineGrid.States.Play) TimerText.text = Format((int)Grid.TimeSinceReset);

        switch (Grid.State)
        {
            case MineGrid.States.Play:
                NewGameButton.image.sprite = NewGameSprite;
                break;
            case MineGrid.States.Win:
                NewGameButton.image.sprite = NewGameWonSprite;
                break;
            case MineGrid.States.Lose:
                NewGameButton.image.sprite = NewGameDeadSprite;
                break;
        }
    }

    public void HandleResize()
    {

        var gridWidth = MineGrid.TileWidth * Grid.Width;
        var gridHeight = MineGrid.TileHeight * Grid.Height;

        int width = 9 // Left border
                       + 8 // Right border
                       + gridWidth;
        int height = 20 // Toolbar
                      + 52 // Top UI/border
                      + 8 // Bottom border
                      + gridHeight;

        int scaledWidth = (int)(width * UIScale);
        int scaledHeight = (int)(height * UIScale);

        Debug.Log("Updating resolution to " + scaledWidth + "x" + scaledHeight);
        Screen.SetResolution(scaledWidth, scaledHeight, FullScreenMode.Windowed);

        Canvas.scaleFactor = UIScale;

        var gridRectTransform = Grid.gameObject.GetComponent<RectTransform>();
        gridRectTransform.position = new Vector3(9, 8, 0);
        gridRectTransform.sizeDelta = new Vector2(gridWidth, gridHeight);

        UITransform.sizeDelta = new Vector2(width, height);

        MenuBarTransform.anchoredPosition = new Vector3(0, height - MenuBarTransform.sizeDelta.y);
        MenuBarTransform.sizeDelta = new Vector2(width, MenuBarTransform.sizeDelta.y);

        CountersTransform.anchoredPosition = new Vector3(0, gridHeight - 14);

    }

    public void HandleWin()
    {
        string key = Grid.Mode.ToString() + ".time";
        if (Grid.Mode != MineGrid.Modes.Custom && (!PlayerPrefs.HasKey(key) || Math.Clamp((int)Grid.TimeSinceReset, 0, 999) < PlayerPrefs.GetInt(key))) FastestTime.gameObject.SetActive(true);
    }

    public static string Format(int n)
    {
        if (n < 0) return '-' + n.ToString().Substring(1).PadLeft(2, '0');
        return n.ToString().PadLeft(3, '0');
    }

    public void ResetGrid()
    {
        Grid.ResetGrid();
    }
}
