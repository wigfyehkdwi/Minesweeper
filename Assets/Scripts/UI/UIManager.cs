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

    public GameObject GameRoot;
    public float UIScale = 3;

    private Vector3 ScaleVec;
    private int Width;
    private int Height;
    private int GridWidth;
    private int GridHeight;

    private void Update()
    {
        // Scale the UI correctly
        GameRoot.transform.localScale = ScaleVec;

        var gridRectTransform = Grid.gameObject.GetComponent<RectTransform>();
        gridRectTransform.position = new Vector3(9, 8, 0);
        gridRectTransform.sizeDelta = new Vector2(GridWidth, GridHeight);

        UITransform.sizeDelta = new Vector2(Width, Height);

        MenuBarTransform.anchoredPosition = new Vector3(0, Height - MenuBarTransform.sizeDelta.y);
        MenuBarTransform.sizeDelta = new Vector2(Width, MenuBarTransform.sizeDelta.y);

        CountersTransform.anchoredPosition = new Vector3(0, GridHeight - 14);

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
        ScaleVec = Vector3.one * UIScale;

        GridWidth = MineGrid.TileWidth * Grid.Width;
        GridHeight = MineGrid.TileHeight * Grid.Height;


        Width = 9 // Left border
                       + 8 // Right border
                       + GridWidth;
        Height = 20 // Toolbar
                      + 52 // Top UI/border
                      + 8 // Bottom border
                      + GridHeight;

        int scaledWidth = (int)(Width * Math.Abs(UIScale));
        int scaledHeight = (int)(Height * Math.Abs(UIScale));

        Debug.Log("Updating resolution to " + scaledWidth + "x" + scaledHeight);
        Screen.SetResolution(scaledWidth, scaledHeight, FullScreenMode.Windowed);

        // The rest has been moved into update
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
